using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using Microsoft.ServiceBus;
using ServiceBusUtils;
using EnergyServiceContract;

namespace NetEventRelayServer
{
 class Program
 {
  static void Main(string[] args)
  {
   try
   {
    string serviceNamespaceDomain = ServiceBusHelper.GetServiceBusSolutionName();
    string issuerName = "owner";
    string issuerSecret = "wJBJaobUmarWn6kqv7QpaaRh3ttNVr3w1OjiotVEOL4=";
    ServiceBusEnvironment.SystemConnectivity.Mode = ConnectivityMode.AutoDetect;
   
    TransportClientEndpointBehavior relayCredentials = new TransportClientEndpointBehavior();
    relayCredentials.TokenProvider = SharedSecretTokenProvider.CreateSharedSecretTokenProvider(issuerName, issuerSecret);
    Uri serviceAddress = ServiceBusEnvironment.CreateServiceUri("sb", serviceNamespaceDomain,
               "Gateway/MulticastService/");
    ServiceHost host = new ServiceHost(typeof(MulticastGatewayOperations), serviceAddress);
    host.Description.Endpoints[0].Behaviors.Add(relayCredentials);
    host.Open();


    Console.WriteLine("ServiceUri:" + serviceAddress.ToString());

    Console.WriteLine("Service registered for public discovery.");
    NetEventRelayBinding binding = host.Description.Endpoints[0].Binding as NetEventRelayBinding;
    if (binding != null)
    {
        Console.WriteLine("Scheme:" + binding.Scheme);
        Console.WriteLine("Security Mode:" + binding.Security.Mode);
        Console.WriteLine("Security RelayAuthType:" + binding.Security.RelayClientAuthenticationType.ToString());
        Console.WriteLine("Security Transport.ProtectionLevel:" + binding.Security.Transport.ProtectionLevel.ToString());



    }
    Console.WriteLine("Press [Enter] to exit");
    Console.ReadLine();

    host.Close();
   }
   catch (Exception ex)
   {
    Console.WriteLine(ex.Message);

   }
  }


 }
}
