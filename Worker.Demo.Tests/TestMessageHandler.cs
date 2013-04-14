using System;

namespace Worker.Demo.Tests
{
    public class TestMessageHandler : IMessageHandler
    {
        public bool CanHandle(IMessage message)
        {
            return message is TestMessage;
        }

        public void Handle(IMessage message)
        {
            var m = (TestMessage)message;
            Console.WriteLine(m.Message);
        }
    }
}