// ----------------------------------------------------------------------------------
// Microsoft Developer & Platform Evangelism
// 
// Copyright (c) Microsoft Corporation. All rights reserved.
// 
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// ----------------------------------------------------------------------------------
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
// ----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace EventPoint.Data
{
    public class EventMessageDataSource
    {
        private static CloudStorageAccount storageAccount;
        private EventMessageDataContext context;

        static EventMessageDataSource()
        {
            CloudStorageAccount.SetConfigurationSettingPublisher((configName, configSetter) =>
            {
                configSetter(RoleEnvironment.GetConfigurationSettingValue(configName));
            });

            storageAccount = CloudStorageAccount.FromConfigurationSetting("DataConnectionString");

            CloudTableClient.CreateTablesFromModel(
                typeof(EventMessageDataContext),
                storageAccount.TableEndpoint.AbsoluteUri,
                storageAccount.Credentials);
        }

        public EventMessageDataSource()
        {
            this.context = new EventMessageDataContext(storageAccount.TableEndpoint.AbsoluteUri, storageAccount.Credentials);
            this.context.RetryPolicy = RetryPolicies.Retry(3, TimeSpan.FromSeconds(1));
        }

        public IEnumerable<EventMessage> GetEventMessageEntries()
        {
            var results = from g in this.context.EventMessage
                          where g.PartitionKey == DateTime.UtcNow.ToString("MMddyyyy")
                          select g;
            return results;
        }

        public void AddEventMessage(EventMessage msg)
        {
            this.context = new EventMessageDataContext(storageAccount.TableEndpoint.AbsoluteUri, storageAccount.Credentials);
            this.context.RetryPolicy = RetryPolicies.Retry(3, TimeSpan.FromSeconds(1));

            this.context.AddObject(this.context.EventMessageTableName, msg);
            this.context.SaveChanges();
        }


        //public void DeleteAllEventMessages()
        //{
        //    this.context = new EventMessageDataContext(storageAccount.TableEndpoint.AbsoluteUri, storageAccount.Credentials);
        //    this.context.RetryPolicy = RetryPolicies.Retry(3, TimeSpan.FromSeconds(1));

        //    this.context.AddObject(this.context.EventMessageTableName, msg);
        //    this.context.SaveChanges();
        //}

        //public void DeleteEventMessage(string partitionKey, string rowKey)
        //{

        //    string foo = "asdfads";  
        //    //this.context.AttachTo(this.context.EventMessageTableName, msg, "*");
        //    //this.context.DeleteObject(msg);
        //    this.context.SaveChanges();
        //}

        public void DeleteEventMessage(EventMessage msg)
        {
            this.context.AttachTo(this.context.EventMessageTableName, msg, "*");
            this.context.DeleteObject(msg);
            this.context.SaveChanges();
        }

        #region old

        //public void UpdateImageThumbnail(string partitionKey, string rowKey, string thumbUrl)
        //{
        //    var results = from g in this.context.EventMessage
        //                  where g.PartitionKey == partitionKey && g.RowKey == rowKey
        //                  select g;

        //    var entry = results.FirstOrDefault<EventMessage>();
        //    entry.ThumbnailUrl = thumbUrl;
        //    this.context.UpdateObject(entry);
        //    this.context.SaveChanges();
        //}

        //public void ReinsertPoisonMessage(string messageId, string queueName)
        //{
        //    CloudQueueClient queueStorage = storageAccount.CreateCloudQueueClient();
        //    CloudQueue originalQueue = queueStorage.GetQueueReference(queueName);

        //    CloudQueue poisonQueue = queueStorage.GetQueueReference("poisonmessages");
        //    var message = (from msg in poisonQueue.GetMessages(32, TimeSpan.FromSeconds(1))
        //                   where msg.Id == messageId
        //                   select msg).FirstOrDefault();
        //    if (message != null)
        //    {
        //        originalQueue.AddMessage(new CloudQueueMessage(new PoisonMessage(message).Body));
        //        poisonQueue.DeleteMessage(messageId, message.PopReceipt);
        //    }
        //}


        //public IEnumerable<PoisonMessage> GetPoisonMessages()
        //{
        //    CloudQueueClient queueStorage = storageAccount.CreateCloudQueueClient();
        //    CloudQueue queue = queueStorage.GetQueueReference("poisonmessages");
        //    return from message in queue.PeekMessages(32)
        //           select new PoisonMessage(message);
        //}


        //public class PoisonMessage
        //{
        //    public PoisonMessage(CloudQueueMessage message)
        //    {
        //        var messageParts = message.AsString.Split(new char[] { ',' });
        //        this.QueueName = messageParts[0];
        //        this.InsertionTime = DateTime.Parse(messageParts[1]);
        //        this.Body = String.Format("{0},{1},{2}", messageParts[2], messageParts[3], messageParts[4]);
        //        this.MessageId = message.Id;
        //    }

        //    public string MessageId { get; private set; }
        //    public string QueueName { get; private set; }
        //    public DateTime InsertionTime { get; private set; }
        //    public string Body { get; private set; }
        //}

        #endregion



    }
}