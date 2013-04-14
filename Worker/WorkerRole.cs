using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading;
using Microsoft.Practices.Unity;
using Microsoft.WindowsAzure.ServiceRuntime;
using Worker.Demo;
using Worker.Demo.Decoders;
using Worker.Demo.Handlers;
using Worker.Demo.Logging;
using Worker.Demo.Sources;

namespace Worker
{
    public class WorkerRole : RoleEntryPoint
    {
        public override void Run()
        {
            // This is a sample worker implementation. Replace with your logic.
            Trace.WriteLine("Worker entry point called", "Information");

            var uc = new UnityContainer();

            uc.RegisterType<ILogger, TableStorageLogger>();
            uc.RegisterType<IMessageSource, QueueMessageSource>();
            uc.RegisterType<IMessageDecoder, QueueMessageDecoder>();
            uc.RegisterType<IMessageHandler, CommandHandler>("CommandHandler");
            uc.RegisterType<IEnumerable<IMessageHandler>, IMessageHandler[]>();

            var processor = uc.Resolve<MessageProcessor>();

            while (true)
            {
                processor.Process();
                Thread.Sleep(10000);
                Trace.WriteLine("Working", "Information");
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 1000;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            return base.OnStart();
        }
    }
}
