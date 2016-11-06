using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;


namespace EnergyServiceContract
{

    [ServiceContract(Name = "IEnergyServiceGatewayOperations", Namespace = "http://proazure/ServiceBus/energyservice/gateway")]
    public interface IEnergyServiceGatewayOperations
    {
        [OperationContract]
        bool UpdateSoftware(string softwareUrl);

        [OperationContract]
        bool SetLightingValue(string gatewayId, string deviceId, short switchValue);

        [OperationContract]
        short GetLightingValue(string gatewayId, string deviceId);

        [OperationContract]
        bool SetHVACMode(string gatewayId, string deviceId, int hvMode);
        [OperationContract]
        int GetHVACMode(string gatewayId, string deviceId);

        [OperationContract]
        bool SetHVACSetpoint(string gatewayId, string deviceId, int spValue);
        [OperationContract]
        int GetHVACSetpoint(string gatewayId, string deviceId);

        [OperationContract]
        int GetCurrentTemp(string gatewayId, string deviceId);

        [OperationContract]
        double GetKWhValue(string gatewayId, string deviceId);

    }

    public interface IEnergyServiceGatewayOperationsChannel : IEnergyServiceGatewayOperations, IClientChannel
    {
    }
}
