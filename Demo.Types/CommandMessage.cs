using System.Collections.Generic;

namespace Demo.Types
{
    public class CommandMessage : IMessage
    {
        public string Command { get; set; }
        public Dictionary<string, string> Parameters { get; set; }
    }
}
