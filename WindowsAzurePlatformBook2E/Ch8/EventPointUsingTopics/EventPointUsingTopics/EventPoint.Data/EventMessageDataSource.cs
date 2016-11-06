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
        CloudStorageAccount storageAccount;
        EventMessageDataContext context;

        //static EventMessageDataSource()
        //{
        //    CloudStorageAccount.SetConfigurationSettingPublisher((configName, configSetter) =>
        //    {
        //        configSetter(RoleEnvironment.GetConfigurationSettingValue(configName));
        //    });

        //    storageAccount = CloudStorageAccount.FromConfigurationSetting("DataConnectionString");

        //    CloudTableClient.CreateTablesFromModel(
        //        typeof(EventMessageDataContext),
        //        storageAccount.TableEndpoint.AbsoluteUri,
        //        storageAccount.Credentials);
        //}

        public EventMessageDataSource()
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
            context = new EventMessageDataContext(storageAccount.TableEndpoint.AbsoluteUri, storageAccount.Credentials);
            context.RetryPolicy = RetryPolicies.Retry(3, TimeSpan.FromSeconds(5));
        }

        public IEnumerable<EventMessage> GetEventMessageEntries()
        {
            var results = from g in context.EventMessage
                          where g.PartitionKey == DateTime.UtcNow.ToString("MMddyyyy")
                          select g;
            return results;
        }

        public void AddEventMessage(EventMessage msg)
        {
            //context = new EventMessageDataContext(storageAccount.TableEndpoint.AbsoluteUri, storageAccount.Credentials);
            //context.RetryPolicy = RetryPolicies.Retry(3, TimeSpan.FromSeconds(5));
            //context.AddObject(context.EventMessageTableName, msg);
            //context.SaveChangesWithRetries();

            //TableServiceContext loccontext = new Microsoft.WindowsAzure.StorageClient.TableServiceContext(storageAccount.TableEndpoint.AbsoluteUri, storageAccount.Credentials);
            //loccontext.RetryPolicy = RetryPolicies.Retry(3, TimeSpan.FromSeconds(5));
            //loccontext.AddObject("EventMessage", msg);
            //loccontext.SaveChangesWithRetries();

            context.AddObject("EventMessage", msg);
            context.SaveChangesWithRetries();
            
         }


        public void DeleteEventMessage(EventMessage msg)
        {
            context.AttachTo(context.EventMessageTableName, msg, "*");
            context.DeleteObject(msg);
            context.SaveChanges();
        }


    }
}