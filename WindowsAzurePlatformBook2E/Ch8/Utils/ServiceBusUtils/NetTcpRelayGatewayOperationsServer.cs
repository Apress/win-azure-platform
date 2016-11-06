using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.ServiceBus;
using ServiceBusUtils;
using EnergyServiceContract;

namespace ServiceBusUtils
{
    public class NetTcpRelayGatewayOperationsServer : BaseNetTcpRelayServer
    {
     public NetTcpRelayGatewayOperationsServer(string solutionName, string solutionPassword, string servicePath, Type serviceType, string protocol)
            : base(solutionName, solutionPassword, servicePath, serviceType, protocol)
        {


        }

     public NetTcpRelayGatewayOperationsServer(string solutionName, string solutionPassword, string servicePath, object instance, string protocol)
            : base(solutionName, solutionPassword, servicePath, instance, protocol)
        {


        }

    }
}
