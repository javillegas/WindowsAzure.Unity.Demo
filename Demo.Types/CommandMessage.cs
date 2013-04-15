using System.Collections.Generic;

namespace Demo.Types
{
    public class CommandMessage : IMessage
    {
        public CommandMessage()
        {
            Parameters = new Dictionary<string, string>();
        }

        public string Command { get; set; }
        public Dictionary<string, string> Parameters { get; private set; }
    }
}
