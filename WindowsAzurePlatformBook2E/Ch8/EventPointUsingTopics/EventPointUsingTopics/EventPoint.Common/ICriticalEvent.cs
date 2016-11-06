using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace EventPoint.Common
{
    [ServiceContract(Name = "CriticalEvent", Namespace="http://eventpoint/relay")]
    public interface ICriticalEvent       
    {
        [OperationContract(IsOneWay = true)]
        void 
            RegisterAlert(EventPoint.Common.EventMessage alertMsg);
    }


    public interface ICriticalEventChannel : ICriticalEvent, IClientChannel { }


}
