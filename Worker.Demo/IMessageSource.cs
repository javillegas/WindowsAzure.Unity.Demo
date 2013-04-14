using System.Collections.Generic;

namespace Worker.Demo
{
    public interface IMessageSource
    {
        IEnumerable<string> GetMessages(int numberOfMessages);
        void RemoveMessage(string message);
    }
}
