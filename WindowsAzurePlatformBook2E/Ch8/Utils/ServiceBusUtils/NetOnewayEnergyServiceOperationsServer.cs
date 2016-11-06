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
   public class NetOnewayEnergyServiceOperationsServer
    {
    public ServiceHost Host { get; set; }

      
       public NetOnewayEnergyServiceOperationsServer(string serviceNamespace, string issuerName, string issuerKey, string servicePath, ConnectivityMode onewayConnectivityMode, Type serviceType, bool usePassword)
       {

           InitService(serviceNamespace, issuerName, issuerKey, servicePath, onewayConnectivityMode, serviceType, usePassword);
          
       }

       //typeof(OnewayEnergyServiceOperations)
       private void InitService(string serviceNamespace, string issuerName, string issuerKey, string servicePath, ConnectivityMode onewayConnectivityMode, Type serviceType, bool usePassword)
       {

           ServiceBusEnvironment.SystemConnectivity.Mode = onewayConnectivityMode;
       
          // Host = new ServiceHost(serviceType, address);
           Host = new ServiceHost(serviceType);
           if (usePassword)
           {
               TransportClientEndpointBehavior behavior = ServiceBusHelper.GetUsernamePasswordBehavior(issuerName, issuerKey);
            Host.Description.Endpoints[0].Behaviors.Add(behavior);
           }

           ServiceUri = Host.Description.Endpoints[0].ListenUri.ToString();
           ServiceRegistrySettings settings = new ServiceRegistrySettings();
           settings.DiscoveryMode = DiscoveryType.Public;
           settings.DisplayName = ServiceUri;
           foreach (ServiceEndpoint se in Host.Description.Endpoints)
               se.Behaviors.Add(settings);

           
           Host.Open();

          
          

       }

       private void InitBindingInCode(string solutionName, string solutionPassword, string servicePath, ConnectivityMode onewayConnectivityMode, Type serviceType)
       {

        //ServiceBusEnvironment.OnewayConnectivity.Mode = onewayConnectivityMode;



           Uri address = ServiceBusEnvironment.CreateServiceUri("sb", solutionName, servicePath);
           ServiceUri = address.ToString();
           Host = new ServiceHost(serviceType, address);

           ContractDescription contractDescription = ContractDescription.GetContract(typeof(IOnewayEnergyServiceOperations), typeof(OnewayEnergyServiceOperations));
           ServiceEndpoint serviceEndPoint = new ServiceEndpoint(contractDescription);
           serviceEndPoint.Address = new EndpointAddress(ServiceUri);
           serviceEndPoint.Binding = new NetOnewayRelayBinding();

           TransportClientEndpointBehavior behavior = ServiceBusHelper.GetUsernamePasswordBehavior(solutionName, solutionPassword);
           serviceEndPoint.Behaviors.Add(behavior);
           Host.Description.Endpoints.Add(serviceEndPoint);

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
