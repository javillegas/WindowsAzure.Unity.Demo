using Demo.Types;

namespace Worker.Demo.Handlers
{
    public class CommandHandler : IMessageHandler
    {
        public bool CanHandle(IMessage message)
        {
            return message is CommandMessage;
        }

        public void Handle(IMessage message)
        {
            
        }
    }
}