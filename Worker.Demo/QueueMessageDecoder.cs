using Demo.Types;
using Newtonsoft.Json;

namespace Worker.Demo
{
    public class QueueMessageDecoder : IMessageDecoder
    {
        public IMessage Decode(string message)
        {
            return JsonConvert.DeserializeObject<CommandMessage>(message);
        }
    }
}