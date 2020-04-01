using System;

namespace GasMon
{
    public class MessageManager
    {
        private readonly SqsService _sqsService;

        public MessageManager(SqsService sqsService)
        {
            _sqsService = sqsService;
        }

        public void ProcessMessages(string queueUrl)
        {
            var messages = _sqsService.FetchMessagesAsync(queueUrl).Result;
            foreach (var message in messages)
            {
                Console.WriteLine(message.MessageId);
                _sqsService.DeleteMessageAsync(queueUrl, message.ReceiptHandle).Wait();
            }
        }
    }
}