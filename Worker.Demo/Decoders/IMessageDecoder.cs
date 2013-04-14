using Demo.Types;

namespace Worker.Demo.Decoders
{
    public interface IMessageDecoder
    {
        IMessage Decode(string message);
    }
}