using System;
using Demo.Types;
using Worker.Demo.Decoders;

namespace Worker.Demo.Tests
{
    public class TestMessageDecoder : IMessageDecoder
    {
        public IMessage Decode(string message)
        {
            if(string.IsNullOrWhiteSpace(message))
                throw new ArgumentNullException("message");

            var values = message.Split(':');
            return new TestMessage
                {       
                    Id = values[1],
                    Message = values[0]
                };
        }
    }
}
