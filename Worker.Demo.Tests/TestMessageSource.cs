using System.Collections.Generic;
using System.Linq;
using Worker.Demo.Sources;

namespace Worker.Demo.Tests
{
    public class TestMessageSource : IMessageSource
    {
        readonly HashSet<string> messages = new HashSet<string>();

        public void Load(int count)
        {
            Enumerable.Range(0, count)
                      .ToList()
                      .ForEach(index => messages.Add("message content:" + index));
        }

        public IEnumerable<string> GetMessages(int numberOfMessages)
        {
            return messages.Take(numberOfMessages).ToList();
        }

        public void RemoveMessage(string message)
        {
            messages.Remove(message);
        }
    }
}