using Demo.Types;

namespace Worker.Demo
{
    public interface IMessageHandler
    {
        bool CanHandle(IMessage message);

        void Handle(IMessage message);
    }
}