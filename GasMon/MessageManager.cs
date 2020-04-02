using System;
using System.Linq;

namespace GasMon
{
    public class MessageManager
    {
        private readonly SqsService _sqsService;
        private readonly MessageParser _messageParser;
        private readonly LocationChecker _locationChecker;

        public MessageManager(SqsService sqsService, MessageParser messageParser, LocationChecker locationChecker)
        {
            _sqsService = sqsService;
            _messageParser = messageParser;
            _locationChecker = locationChecker;
        }

        public void ProcessMessages(string queueUrl)
        {
            var messages = _sqsService
                .FetchMessagesAsync(queueUrl).Result
                .Select(_messageParser.ParseMessage);

            var filteredMessages = messages.Where(_locationChecker.ValidLocationCheck);

            foreach (var message in filteredMessages)
            {
                Console.WriteLine(message.Reading);
            }
            
            foreach (var message in messages)
            {
                _sqsService.DeleteMessageAsync(queueUrl, message.ReceiptHandle).Wait();
                Console.WriteLine("Message deleted");
            }
        }
    }
}