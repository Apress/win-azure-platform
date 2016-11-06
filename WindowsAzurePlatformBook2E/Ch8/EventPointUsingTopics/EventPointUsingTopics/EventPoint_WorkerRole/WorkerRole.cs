using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Xml.Serialization;

using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;
using EventPoint.Data;
using EventPoint.Common;
using Microsoft.ServiceBus.Messaging;
using Microsoft.Samples.ServiceBusMessaging;

namespace EventPoint_WorkerRole
{
    public class WorkerRole : RoleEntryPoint
    {
        SubscriptionClient subscription;

        public override void Run()
        {
            // This is a sample worker implementation. Replace with your logic.
            Trace.WriteLine("EventPoint_WorkerRole entry point called", "Information");

            EventMessageFactory factory = new EventMessageFactory();
            factory.CreateSubscription("AllEventsWorkerRole"); // no filter, all events
            subscription = factory.CreateSubscriptionClient("AllEventsWorkerRole");

            // using listener extension methods provided by Microsoft.Samples.ServiceBusMessaging
            subscription.StartListener(message => WriteToAzureTable(message));

            while (true)
            {
                Thread.Sleep(2000);
            }
        }




        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            DiagnosticMonitor.Start("DiagnosticsConnectionString");

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
            RoleEnvironment.Changing += RoleEnvironmentChanging;

            return base.OnStart();
        }

        private void RoleEnvironmentChanging(object sender, RoleEnvironmentChangingEventArgs e)
        {
            // If a configuration setting is changing
            if (e.Changes.Any(change => change is RoleEnvironmentConfigurationSettingChange))
            {
                // Set e.Cancel to true to restart this role instance
                e.Cancel = true;
            }
        }

        private void WriteToAzureTable(BrokeredMessage message)
        {
            try
            {
                EventPoint.Common.EventMessage eventMsg = message.GetBody<EventPoint.Common.EventMessage>();
                //var storageAccount = CloudStorageAccount.FromConfigurationSetting("DataConnectionString");
                EventMessageDataSource ds = new EventMessageDataSource();

                EventPoint.Data.EventMessage dataMsg = new EventPoint.Data.EventMessage();
                dataMsg.ID = new Random().Next(10000000);
                // assign a RowKey that will show entries in reverse chronological order, with the Event.ID tacked on
                dataMsg.RowKey = string.Format("{0:10}_{1}", DateTime.MaxValue.Ticks - DateTime.Now.Ticks, dataMsg.ID);

                // convert format
                dataMsg.Link = eventMsg.Link;
                dataMsg.Message = eventMsg.Message;
                dataMsg.Originator = eventMsg.Originator;
                dataMsg.Priority = eventMsg.Priority;
                dataMsg.Title = eventMsg.Title;

                // add message to table storage
                ds.AddEventMessage(dataMsg);
                message.Complete();
            }
            catch(Exception)
            {
                message.Abandon();
            }
            finally
            {
                message.Dispose();
            }
        }
    }
}
