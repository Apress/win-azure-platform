using System;
using System.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Text;
using Microsoft.ServiceBus;
using System.ServiceModel.Description;
using EventPoint.Common;
using Microsoft.ServiceBus.Messaging;


namespace EventPoint.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.Title = "EventPoint Console";

            Console.WriteLine("This is an on-prem console application that receives critical alerts");
            Console.WriteLine("multi-cast through the Azure AppFabric ServiceBus.");
            Console.WriteLine("");

            EventMessageFactory factory = new EventMessageFactory();
            factory.CreateSubscription("CriticalEventsConsole", "0"); // 0 is critical priority per the data generator
            SubscriptionClient subscription = factory.CreateSubscriptionClient("CriticalEventsConsole");

            Console.WriteLine("Ready....");

            // Demostrates listening via polling Topic
            while (true)
            {
                BrokeredMessage message = subscription.Receive(TimeSpan.FromSeconds(10));
                if (message != null)
                {
                    try
                    {
                        EventMessage evtMsg = message.GetBody<EventMessage>();
                        Console.WriteLine(String.Format("[{0}] {1}", DateTime.Now.ToLongTimeString(), evtMsg.Message));
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
    }
}
