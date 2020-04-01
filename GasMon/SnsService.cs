using System;
using System.Threading.Tasks;
using Amazon.SimpleNotificationService;
using Amazon.SQS;

namespace GasMon
{
    public class SnsService
    {
        private static readonly string TopicArn = Environment.GetEnvironmentVariable("AWS_SNS_TOPIC_ARN");
        private readonly IAmazonSimpleNotificationService _snsClient;
        private readonly IAmazonSQS _sqsClient;

        public SnsService(IAmazonSimpleNotificationService snsClient, IAmazonSQS sqsClient)
        {
            _snsClient = snsClient;
            _sqsClient = sqsClient;
        }

        public async Task<string> SubscribeQueueAsync(string queueUrl)
        {
            return await _snsClient.SubscribeQueueAsync(TopicArn, _sqsClient, queueUrl);
        }

        public async Task UnsubscribeQueueAsync(string subscriptionArn)
        {
            await _snsClient.UnsubscribeAsync(subscriptionArn);
        }
    }
}