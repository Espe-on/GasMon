using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Newtonsoft.Json;

namespace GasMon
{
    interface ILocationFetcher
    {
        List<Location> LocationFetcher();
    }

    public class LocationsFetcher : ILocationFetcher
    {
        private static readonly string AccessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY");
        private static readonly string SecretKey = Environment.GetEnvironmentVariable("AWS_SECRET_KEY");
        private static readonly string BucketName = Environment.GetEnvironmentVariable("AWS_S3_BUCKET_NAME");
        private const string KeyName = "locations.json";
        private static readonly RegionEndpoint BucketRegion = RegionEndpoint.EUWest2;
        private static IAmazonS3 _client;

        private static async Task<string> ReadObjectDataAsync(IAmazonS3 client)
        {
            GetObjectRequest request = new GetObjectRequest
            {
                BucketName = BucketName,
                Key = KeyName
            };
            using GetObjectResponse response = await client.GetObjectAsync(request);
            await using Stream responseStream = response.ResponseStream;
            using StreamReader reader = new StreamReader(responseStream);
            string
                title = response.Metadata[
                    "x-amz-meta-title"]; // Assume you have "title" as medata added to the object.
            string contentType = response.Headers["Content-Type"];
            Console.WriteLine("Object metadata, Title: {0}", title);
            Console.WriteLine("Content type: {0}", contentType);

            var responseBody = reader.ReadToEnd(); // Now you process the response body.
            return responseBody;
        }

        public List<Location> LocationFetcher()
        {
            _client = new AmazonS3Client(AccessKey, SecretKey, BucketRegion);
            var returnedData = ReadObjectDataAsync(_client).Result;
            return JsonConvert.DeserializeObject<List<Location>>(returnedData);
        }
    }
}