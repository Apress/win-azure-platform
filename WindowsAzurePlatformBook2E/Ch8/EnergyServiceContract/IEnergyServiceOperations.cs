using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace EnergyServiceContract
{
   
    [ServiceContract(Name = "IEnergyServiceOperations", Namespace = "http://proazure/ServiceBus/energyservice/headend")]
    public interface IEnergyServiceOperations
    {
       
        [OperationContract]
        DateTime GetHeadedTime(string gatewayId);
        [OperationContract]
        string[] GetScheduledCommands(string gatewayId);

    }
}
