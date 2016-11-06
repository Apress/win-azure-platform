using System;
using System.ServiceModel;
using Microsoft.WindowsAzure.StorageClient;

namespace EventPoint.ConsoleApp
{
    [ServiceBehavior(Name = "CriticalEvent", Namespace="sb://eventpoint/relay")]
    class CriticalEventService : EventPoint.Common.ICriticalEvent
    {
        public void RegisterAlert(EventPoint.Common.EventMessage eventMsg)
        {
            Console.WriteLine(String.Format("[{0}] {1}", DateTime.Now.ToLongTimeString(), eventMsg.Message));
        }
    }
}
