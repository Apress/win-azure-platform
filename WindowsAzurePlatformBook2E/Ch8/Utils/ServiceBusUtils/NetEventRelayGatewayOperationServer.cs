using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.ServiceBus;
using ServiceBusUtils;
using EnergyServiceContract;

namespace ServiceBusUtils
{
 public class NetEventRelayGatewayOperationServer : NetOnewayEnergyServiceOperationsServer
 {
  public NetEventRelayGatewayOperationServer(string serviceNamespace, string issuerName, string issuerKey, string servicePath, ConnectivityMode onewayConnectivityMode, Type serviceType, bool usePassword)
   : base(serviceNamespace, issuerName, issuerKey, servicePath, onewayConnectivityMode, serviceType, usePassword)
  {


  }

 }
}
