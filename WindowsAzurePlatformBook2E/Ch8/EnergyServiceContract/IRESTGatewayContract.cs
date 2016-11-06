using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;
using System.ServiceModel.Channels;


namespace EnergyServiceContract
{

    [ServiceContract(Name = "IRESTLightswitch.", Namespace = "http://proazure/ServiceBus/energyservice/gateway")]
    public interface IRESTLightswitch
    {

        [OperationContract(Action = "GET", ReplyAction = "GETRESPONSE")]
        Message GetLightswitchState();


    }
    public interface IRESTLightswitchChannel : IRESTLightswitch, IClientChannel
    {
    }

    [ServiceContract(Name = "IRESTEnergyMeter.", Namespace = "http://proazure/ServiceBus/energyservice/gateway")]
    public interface IRESTEnergyMeter
    {


        [OperationContract(Action = "GET", ReplyAction = "GETRESPONSE")]
        Message GetKWhValue();
    }


    public interface IRESTEnergyMeterChannel : IRESTEnergyMeter, IClientChannel
    {
    }

}
