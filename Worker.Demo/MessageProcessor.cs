using System;
using System.Collections.Generic;
using System.Linq;
using Demo.Types;
using Worker.Demo.Decoders;
using Worker.Demo.Handlers;
using Worker.Demo.Logging;
using Worker.Demo.Sources;

namespace Worker.Demo
{
    public class MessageProcessor
    {
        public MessageProcessor(IMessageSource source,
                                IMessageDecoder decoders, 
                                IEnumerable<IMessageHandler> handlers,
                                ILogger logger)
        {
            MessageSource = source;
            MessageDecoder = decoders;
            MessageHandlers = handlers;
            Logger = logger;
        }

        public IMessageSource MessageSource { get; private set; }

        public IMessageDecoder MessageDecoder { get; private set; }

        public IEnumerable<IMessageHandler> MessageHandlers { get; private set; }
        
        public ILogger Logger { get; private set; }

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