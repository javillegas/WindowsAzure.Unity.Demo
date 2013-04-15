﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Worker.Demo.Decoders;
using Worker.Demo.Handlers;
using Worker.Demo.Logging;
using Worker.Demo.Sources;

namespace Worker.Demo.Tests
{
    [TestClass]
    public class MessageProcessorTests
    {
        [TestMethod]
        public void TestWhenDecodingMessageThenShouldReturnATestMessage()
        {
            var decoder = new TestMessageDecoder();
            const string testMessage = "message content:0";
            var message = decoder.Decode(testMessage);
            Assert.IsInstanceOfType(message, typeof(TestMessage));
        }

        [TestMethod]
        public void TestWhenGetOneMessageFromMessageSourceThenReceivedOneMessage()
        {
            var source = new TestMessageSource();
            source.Load(1);
            var message = source.GetMessages(1);
            Assert.IsTrue(message.Count() == 1);
        }

        [TestMethod]
        public void TestWhenRemoveMessagesFromMessageSourceThenShouldBeEmpty()
        {
            var source = new TestMessageSource();
            source.Load(32);
            var message = source.GetMessages(32);
            foreach (var m in message)
            {
                source.RemoveMessage(m);
            }
            message = source.GetMessages(32);
            Assert.IsFalse(message.Any());
        }

        [TestMethod]
        public void TestWhenTestMessageSourceReturnsMessageThenDecodeMessage()
        {
            var source = new TestMessageSource();
            source.Load(1);
            var message = source.GetMessages(1);
            var decoder = new TestMessageDecoder();
            foreach (var m in message)
            {
                var decodedMessage = decoder.Decode(m);
                Assert.IsInstanceOfType(decodedMessage, typeof(TestMessage));
            }
        }

        [TestMethod]
        public void TestWhenTestMessageCanBeHandledThenIsProcessed()
        {
            var source = new TestMessageSource();
            source.Load(1);
            var decoder = new TestMessageDecoder();
            var handler = new TestMessageHandler();

            var message = source.GetMessages(1);
            foreach (var m in message)
            {
                var decodedMessage = decoder.Decode(m);
                var canHandle = handler.CanHandle(decodedMessage);
                Assert.IsTrue(canHandle);

                handler.Handle(decodedMessage);
            }
        }

        [TestMethod]
        public void TestWhenTestSourceHasMessagesAndHandlerIsPresentThenMessageIsRemovedFromSource()
        {
            var source = new TestMessageSource();
            source.Load(1);
            var decoder = new TestMessageDecoder();
            var handler = new TestMessageHandler();
            var logger = new TestLogger();

            var handlers = new List<IMessageHandler> { handler };
            var processor = new MessageProcessor(source, decoder, handlers, logger);

            processor.Process();

            Assert.IsFalse(source.GetMessages(1).Any());
        }

        [TestMethod]
        public void TestWhenTestSourceHasMessagesAndNoHandlerIsPresentThenMessageIsNotRemovedFromSource()
        {
            var source = new TestMessageSource();
            source.Load(1);
            var decoder = new TestMessageDecoder();
            var logger = new TestLogger();

            var handlers = new List<IMessageHandler>();
            var messageProcessor = new MessageProcessor(source, decoder, handlers, logger);

            messageProcessor.Process();

            Assert.IsTrue(source.GetMessages(1).Any());
        }

        [TestMethod]
        public void TestWhenUsingUnityTestSourceHasMessageAndMessageProcessorIsExecutedThenMessagesAreRemovedFromSource()
        {
            using (var uc = new UnityContainer())
            {
                uc.RegisterType<ILogger, TestLogger>();
                uc.RegisterType<IMessageSource, TestMessageSource>();
                uc.RegisterType<IMessageDecoder, TestMessageDecoder>();
                uc.RegisterType<IMessageHandler, TestMessageHandler>("test");
                uc.RegisterType<IEnumerable<IMessageHandler>, IMessageHandler[]>();

                var ms = new TestMessageSource();
                ms.Load(1);

                uc.RegisterInstance(typeof(IMessageSource), ms);

                var messageProcessor = uc.Resolve<MessageProcessor>();

                messageProcessor.Process();

                Assert.IsFalse(ms.GetMessages(1).Any());
            }
        }
    }
}