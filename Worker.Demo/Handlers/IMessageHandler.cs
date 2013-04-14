using Demo.Types;

namespace Worker.Demo.Handlers
{
    public interface IMessageHandler
    {
        bool CanHandle(IMessage message);

        void Handle(IMessage message);
    }
}