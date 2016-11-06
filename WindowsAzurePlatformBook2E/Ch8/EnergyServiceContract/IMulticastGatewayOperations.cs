using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace EnergyServiceContract
{
    [ServiceContract(Name = "IMulticastGatewayOperations.", Namespace = "http://proazure/ServiceBus/energyservice/gateway")]
    public interface IMulticastGatewayOperations
    {
        [OperationContract(IsOneWay = true)]
        void Online(string gatewayId, string serviceUri, DateTime utcTime);
        [OperationContract(IsOneWay = true)]
        void GoingOffline(string gatewayId, string serviceUri, DateTime utcTime);

    }

    public interface IMulticastGatewayChannel : IMulticastGatewayOperations, IClientChannel
    {
    }
}
