using Demo.Types;

namespace Worker.Demo.Tests
{
    public class TestMessageDecoder : IMessageDecoder
    {
        public IMessage Decode(string message)
        {
            var values = message.Split(':');
            return new TestMessage
                {       
                    Id = values[1],
                    Message = values[0]
                };
        }
    }
}
