using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.RetryPolicies;

namespace Worker.Demo
{
    public class QueueMessageSource : IMessageSource
    {
        private readonly Dictionary<string, CloudQueueMessage> messages = new Dictionary<string, CloudQueueMessage>();
        private readonly CloudQueue queue;

        private const string ConnectionString = "StorageConnectionString";
        private const string QueueName = "test";

        public QueueMessageSource()
        {
            var account = MakeAccount();

            queue = MakeQueue(account);

            queue.CreateIfNotExists();    
        }

        private static CloudStorageAccount MakeAccount()
        {
            var cs = CloudConfigurationManager.GetSetting(ConnectionString);
            var account = CloudStorageAccount.Parse(cs);

            ServicePointManager.FindServicePoint(account.QueueEndpoint).UseNagleAlgorithm = false;
            return account;
        }

        private CloudQueue MakeQueue(CloudStorageAccount account)
        {
            var client = account.CreateCloudQueueClient();
            client.RetryPolicy = new ExponentialRetry(new TimeSpan(0, 0, 0, 2), 10);

            return client.GetQueueReference(QueueName);
        }

        public IEnumerable<string> GetMessages(int numberOfMessages)
        {
            var cloudQueueMessages = queue.GetMessages(numberOfMessages).ToList();
            
            foreach (var message in cloudQueueMessages)
                messages.Add(MakeMessageHash(message.AsString), message);
            
            return cloudQueueMessages.Select(m=>m.AsString);
        }

        public void RemoveMessage(string message)
        {
            var cloudMessage = messages[MakeMessageHash(message)];
            queue.DeleteMessage(cloudMessage);
        }

        protected string MakeMessageHash(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentNullException("message");

            using (var unmanaged = new SHA1CryptoServiceProvider())
            {
                var bytes = Encoding.UTF8.GetBytes(message);

                var computeHash = unmanaged.ComputeHash(bytes);

                if (computeHash.Length == 0)
                    return string.Empty;

                return Convert.ToBase64String(computeHash);
            }
        }
    }
}