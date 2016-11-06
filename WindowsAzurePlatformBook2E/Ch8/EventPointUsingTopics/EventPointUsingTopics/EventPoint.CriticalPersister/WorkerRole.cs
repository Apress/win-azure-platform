using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Threading;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;
using System.Data.SqlClient;
using System.Text;
using Microsoft.ServiceBus;
using System.ServiceModel.Description;
using EventPoint.Common;
using Microsoft.ServiceBus.Messaging;
using Microsoft.Samples.ServiceBusMessaging;

namespace EventPoint.CriticalPersister
{
    public class WorkerRole : RoleEntryPoint
    {
        SubscriptionClient subscription;
        CriticalEventService eventService;

        public override void Run()
        {
            Trace.WriteLine("CriticalPersister entry point called", "Information");

            EventMessageFactory factory = new EventMessageFactory();
            factory.CreateSubscription("CriticalEventsPersister", "0"); // 0 is critical priority per the data generator
            subscription = factory.CreateSubscriptionClient("CriticalEventsPersister");

            eventService = new CriticalEventService();

            // using listener extension methods provided by Microsoft.Samples.ServiceBusMessaging
            subscription.StartListener(message => Publish(message));

            while (true)
            {
                Thread.Sleep(2000);
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            return base.OnStart();
        }

        public override void OnStop()
        {
            if (subscription != null)
                subscription.StopListener();
            base.OnStop();
        }

        private void Publish(BrokeredMessage message)
        {
            try
            {
                eventService.RegisterAlert(message.GetBody<EventMessage>());
                message.Complete();
            }
            catch (Exception)
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
