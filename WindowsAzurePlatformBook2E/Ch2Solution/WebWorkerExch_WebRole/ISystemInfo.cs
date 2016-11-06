using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ProAzureCommon;

namespace WebWorkerExch_WebRole
{
    // NOTE: If you change the interface name "ISystemInfo" here, you must also update the reference to "ISystemInfo" in Web.config.
    [ServiceContract]
    public interface ISystemInfo
    {
        [OperationContract]
        void SendSystemInfo(SystemMessageExchange ds);
    }
}
