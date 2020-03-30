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
        private static string _bucketName = Environment.GetEnvironmentVariable("AWS_S3_BUCKET_NAME");
        private static string _keyName = "locations.json";
        private static readonly RegionEndpoint _bucketRegion = RegionEndpoint.EUWest2;
        private static IAmazonS3 _client;

        static void Main()
        {
            _client = new AmazonS3Client(_bucketRegion);
            ReadObjectDataAsync().Wait();
        }
        static async Task ReadObjectDataAsync()
        {
            string responseBody = "";
            try
            {
                GetObjectRequest request = new GetObjectRequest
                {
                    BucketName = _bucketName,
                    Key = _keyName
                };
                using (GetObjectResponse response = await _client.GetObjectAsync(request))
                using (Stream responseStream = response.ResponseStream)
                using (StreamReader reader = new StreamReader(responseStream))
                {
                    string title = response.Metadata["x-amz-meta-title"]; // Assume you have "title" as medata added to the object.
                    string contentType = response.Headers["Content-Type"];
                    Console.WriteLine("Object metadata, Title: {0}", title);
                    Console.WriteLine("Content type: {0}", contentType);

                    responseBody = reader.ReadToEnd(); // Now you process the response body.
                }
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