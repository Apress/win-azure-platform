using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using ProAzureDemResContract;

namespace DemResGateway
{
 class GatewayClient : DuplexClientBase<IDemResOperations>, IDemResOperations
 {
  public GatewayClient(object callback)
   : base(callback)
   {}

  public void SendValue(string gatewayId, double value, DateTime gatewayTime)
   {
    Channel.SendValue(gatewayId, value, gatewayTime);
   }
 }
}
