using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace EnergyServiceContract
{

    /// <summary>
    /// These are all head-end operations
    /// </summary>
    [ServiceContract(Name = "IOnewayEnergyServiceOperations.", Namespace = "http://proazure/ServiceBus/energyservice/headend/")]
    public interface IOnewayEnergyServiceOperations
    {
        /// <summary>
        /// Send the Energy Meter Value to the head-end server
        /// </summary>
        /// <param name="gatewayId">The id of the control gateway</param>
        /// <param name="meterId">The id of the energy meter device</param>
        /// <param name="kwhValue">The kWh value of the energy meter</param>
        /// <param name="utcTime">Time when the kWh value was read</param>
        /// <returns></returns>
        [OperationContract(IsOneWay=true)]
        void SendKwhValue(string gatewayId, string meterId, double kwhValue, DateTime utcTime);
        /// <summary>
        /// Send the lighting value to the head-end server
        /// </summary>
        /// <param name="gatewayId"></param>
        /// <param name="switchId"></param>
        /// <param name="lightingValue"></param>
        /// <param name="utcTime"></param>
        [OperationContract(IsOneWay = true)]
        void SendLightingValue(string gatewayId, string switchId, int lightingValue, DateTime utcTime);
        /// <summary>
        /// Send the HVAC SetPoint value to the head-end
        /// </summary>
        /// <param name="gatewayId"></param>
        /// <param name="hvacId"></param>
        /// <param name="setPointValue"></param>
        /// <param name="utcTime"></param>
        [OperationContract(IsOneWay = true)]
        void SendHVACSetPoint(string gatewayId, string hvacId, int setPointValue, DateTime utcTime);
        /// <summary>
        /// Send the HVAC Mode to the head-end
        /// </summary>
        /// <param name="gatewayId"></param>
        /// <param name="hvacId"></param>
        /// <param name="mode"></param>
        /// <param name="utcTime"></param>
        [OperationContract(IsOneWay = true)]
        void SendHVACMode(string gatewayId, string hvacId, int mode, DateTime utcTime);
    }

    public interface IOnewayEnergyServiceChannel : IOnewayEnergyServiceOperations, IClientChannel { }
}
 