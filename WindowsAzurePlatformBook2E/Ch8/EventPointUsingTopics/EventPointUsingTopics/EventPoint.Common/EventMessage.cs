using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace EventPoint.Common
{
    [DataContract]
    public class EventMessage
    {
        public EventMessage()
        {
        }
        [DataMember]
        public string Priority { get; set; }
        [DataMember]
        public string Originator { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public string Link { get; set; }
    }
}
