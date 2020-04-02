using System;
using System.Linq;

namespace GasMon
{
    public class MessageManager
    {
        private readonly SqsService _sqsService;
        private readonly MessageParser _messageParser;

        public MessageManager(SqsService sqsService, MessageParser messageParser)
        {
            _sqsService = sqsService;
            _messageParser = messageParser;
        }

        public void ProcessMessages(string queueUrl)
        {
            var messages = _sqsService
                .FetchMessagesAsync(queueUrl).Result
                .Select(_messageParser.ParseMessage);
            
            foreach (var message in messages)
            {
                Console.WriteLine(message.Reading);
                _sqsService.DeleteMessageAsync(queueUrl, message.ReceiptHandle).Wait();
            }
        }
    }
}