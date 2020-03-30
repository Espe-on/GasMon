using System;
using System.IO;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;

namespace GasMon
{
    class Program
    {
        private static readonly string AccessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY");
        private static readonly string SecretKey = Environment.GetEnvironmentVariable("AWS_SECRET_KEY");
        private static readonly string BucketName = Environment.GetEnvironmentVariable("AWS_S3_BUCKET_NAME");
        private const string KeyName = "locations.json";
        private static readonly RegionEndpoint BucketRegion = RegionEndpoint.EUWest2;
        private static IAmazonS3 _client;

        static void Main()
        {
            _client = new AmazonS3Client(AccessKey,SecretKey,BucketRegion);
            ReadObjectDataAsync().Wait();
        }
        static async Task ReadObjectDataAsync()
        {
            string responseBody = "";
            try
            {
                GetObjectRequest request = new GetObjectRequest
                {
                    BucketName = BucketName,
                    Key = KeyName
                };
                using GetObjectResponse response = await _client.GetObjectAsync(request);
                await using Stream responseStream = response.ResponseStream;
                using StreamReader reader = new StreamReader(responseStream);
                string title = response.Metadata["x-amz-meta-title"]; // Assume you have "title" as medata added to the object.
                string contentType = response.Headers["Content-Type"];
                Console.WriteLine("Object metadata, Title: {0}", title);
                Console.WriteLine("Content type: {0}", contentType);

                responseBody = reader.ReadToEnd(); // Now you process the response body.
                Console.WriteLine(responseBody);
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered ***. Message:'{0}' when writing an object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
            }
        }
    }
}