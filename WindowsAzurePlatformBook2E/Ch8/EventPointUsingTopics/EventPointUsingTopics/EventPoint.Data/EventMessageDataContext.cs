using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventPoint.Data
{

    public class EventMessageDataContext
    : Microsoft.WindowsAzure.StorageClient.TableServiceContext
    {
        public EventMessageDataContext(string baseAddress, Microsoft.WindowsAzure.StorageCredentials credentials)
            : base(baseAddress, credentials)
        { }

        // So that we only need to put the actual table name in one place...
        public string EventMessageTableName = "EventMessage";

        public IQueryable<EventMessage> EventMessage
        {
            get
            {
                return this.CreateQuery<EventMessage>(EventMessageTableName);
            }
        }
    }
}
