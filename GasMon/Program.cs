using System;
using System.Runtime.InteropServices.ComTypes;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.SimpleNotificationService;
using Amazon.SQS;

namespace GasMon
{
    class Program
    {
        static void Main()
        {    
            // var awsCredentials = new BasicAWSCredentials(Environment.GetEnvironmentVariable("AWS_ACCESS_KEY"), Environment.GetEnvironmentVariable("AWS_SECRET_KEY"));
            var locationFetcher = new LocationsFetcher();
            var sqsClient = new AmazonSQSClient(RegionEndpoint.EUWest2);
            var snsClient = new AmazonSimpleNotificationServiceClient(RegionEndpoint.EUWest2);
            var sqsService = new SqsService(sqsClient);
            var snsService = new SnsService(snsClient, sqsClient);
            var messageManager = new MessageManager(sqsService);
            
            
            var locationList = locationFetcher.LocationListMaker();
            foreach (var location in locationList)
            {
                Console.WriteLine($"Location ID:{location.Id} has the location (x:{location.x}, y:{location.y}).");
            }
            using (var queue = new SubscribedQueue(sqsService, snsService))
            {
                var endTime = DateTime.Now.AddMinutes(1);
                while (DateTime.Now < endTime)
                {
                    messageManager.ProcessMessages(queue.QueueUrl);
                }
            }
            Console.WriteLine("finished.");
            
        }
        
    }
}