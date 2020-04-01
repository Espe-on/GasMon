using Amazon.SQS.Model;
using Newtonsoft.Json;
using SnsMessage =  Amazon.SimpleNotificationService.Util.Message;

namespace GasMon
{
    public class MessageParser
    {
        public Reading ParseMessage(Message message)
        {
            //come in later and write a test
            var snsMessage = SnsMessage.ParseMessage(message.Body);
            return JsonConvert.DeserializeObject<Reading>(snsMessage.MessageText);
        }
    }
}