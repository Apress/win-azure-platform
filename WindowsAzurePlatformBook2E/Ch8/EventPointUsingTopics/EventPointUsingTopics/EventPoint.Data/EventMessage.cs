using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventPoint.Data
{

    //public enum EventPriority : short
    //{
    //    Low,
    //    Normal,
    //    High,
    //    Critical
    //}

    [Serializable]
    public class EventMessage
        : Microsoft.WindowsAzure.StorageClient.TableServiceEntity
    {

        public EventMessage(string partitionKey, string rowKey)
            : base(partitionKey, rowKey)
        {
        }

        public EventMessage()
        {
            PartitionKey = DateTime.UtcNow.ToString("MMddyyyy");
            // Row key is assigned at runtime
        }

        public int ID { get; set; }
        public string Priority { get; set; }
        public string Originator { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string Link { get; set; }
    }
}
