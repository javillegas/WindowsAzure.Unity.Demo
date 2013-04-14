using Demo.Types;

namespace Worker.Demo
{
    public interface IMessageDecoder
    {
        IMessage Decode(string message);
    }
}