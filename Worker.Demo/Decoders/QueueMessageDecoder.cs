using Demo.Types;
using Newtonsoft.Json;

namespace Worker.Demo.Decoders
{
    public class QueueMessageDecoder : IMessageDecoder
    {
        public IMessage Decode(string message)
        {
            return JsonConvert.DeserializeObject<CommandMessage>(message);
        }
    }
}