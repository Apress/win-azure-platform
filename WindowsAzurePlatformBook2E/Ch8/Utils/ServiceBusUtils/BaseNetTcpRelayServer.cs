using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;
using Microsoft.ServiceBus;
using ServiceBusUtils;
using EnergyServiceContract;

namespace ServiceBusUtils
{
    public class BaseNetTcpRelayServer
    {
        public ServiceHost Host { get; set; }


        public BaseNetTcpRelayServer(string solutionName, string solutionPassword, string servicePath, Type serviceType, string protocol)
        {
                InitService(solutionName, solutionPassword, servicePath, serviceType, protocol);
            
        }

        public BaseNetTcpRelayServer(string solutionName, string solutionPassword, string servicePath, object instance, string protocol)
        {
            InitService(solutionName, solutionPassword, servicePath, instance, protocol);

        }


        private void InitService(string solutionName, string solutionPassword, string servicePath,  Type serviceType, string protocol)
        {


         Uri address = ServiceBusEnvironment.CreateServiceUri(protocol, solutionName, servicePath);
            ServiceUri = address.ToString();
            TransportClientEndpointBehavior behavior = ServiceBusHelper.GetUsernamePasswordBehavior(solutionName, solutionPassword);
            Host = new ServiceHost(serviceType, address);
            Host.Description.Endpoints[0].Behaviors.Add(behavior);

            ServiceRegistrySettings settings = new ServiceRegistrySettings();
            settings.DiscoveryMode = DiscoveryType.Public;
            settings.DisplayName = address.ToString();
            foreach (ServiceEndpoint se in Host.Description.Endpoints)
                se.Behaviors.Add(settings);

            Host.Open();

        }

        private void InitService(string solutionName, string solutionPassword, string servicePath, object instance, string protocol)
        {


            Uri address = ServiceBusEnvironment.CreateServiceUri(protocol, solutionName, servicePath);
            ServiceUri = address.ToString();
            TransportClientEndpointBehavior behavior = ServiceBusHelper.GetUsernamePasswordBehavior(solutionName, solutionPassword);
            Host = new ServiceHost(instance, address);
   
            Host.Description.Endpoints[0].Behaviors.Add(behavior);

            ServiceRegistrySettings settings = new ServiceRegistrySettings();
            settings.DiscoveryMode = DiscoveryType.Public;
            settings.DisplayName = address.ToString();
            foreach (ServiceEndpoint se in Host.Description.Endpoints)
                se.Behaviors.Add(settings);

            Host.Open();

        }

      
      


        public virtual string ServiceUri { get; set; }

        public virtual void Close()
        {
            Host.Close();

        }


    }
}
