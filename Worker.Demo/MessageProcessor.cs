using System;
using System.Collections.Generic;
using System.Linq;
using Demo.Types;
using Microsoft.Practices.Unity;

namespace Worker.Demo
{
    public class MessageProcessor
    {
        [Dependency]
        public IMessageSource MessageSource { get; set; }

        [Dependency]
        public IMessageDecoder MessageDecoder { get; set; }

        [Dependency]
        public IEnumerable<IMessageHandler> MessageHandlers { get; set; }

        [Dependency]
        public ILogger Logger { get; set; }

        public void Process()
        {
            var messages = MessageSource.GetMessages(32);
            foreach (var message in messages)
                ProcessMessage(message);
        }

        private void ProcessMessage(string message)
        {
            var msg = MessageDecoder.Decode(message);
            
            var handlers = GetMessageHandlers(msg);

            if (!handlers.Any())
            {
                Logger.Write("Unable to process message : " + message);
                return;
            }

            HandleMessage(handlers, msg);
            MessageSource.RemoveMessage(message);
        }

        private List<IMessageHandler> GetMessageHandlers(IMessage msg)
        {
            var handlers = MessageHandlers.Where(h => h.CanHandle(msg)).ToList();
            return handlers;
        }

        private void HandleMessage(IEnumerable<IMessageHandler> handlers, IMessage msg)
        {
            foreach (var h in handlers)
                TryHandleMessage(msg, h);
        }

        private void TryHandleMessage(IMessage msg, IMessageHandler h)
        {
            try
            {
                h.Handle(msg);
            }
            catch (Exception ex)
            {
                Logger.Write(ex.ToString());
            }
        }
    }
}