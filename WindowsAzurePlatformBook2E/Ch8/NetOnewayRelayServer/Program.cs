using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using Microsoft.ServiceBus;
using ServiceBusUtils;
using EnergyServiceContract;

namespace NetOnewayRelayServer
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //string serviceBusSolutionName = ServiceBusHelper.GetServiceBusSolutionName();
                //Console.Write("Your Solution Password: ");
                //string password = ServiceBusHelper.ReadPassword();
                //string issuerName = "owner";
                //string issuerKey = "wJBJaobUmarWn6kqv7QpaaRh3ttNVr3w1OjiotVEOL4=";
                //NetOnewayEnergyServiceOperationsServer server = new NetOnewayEnergyServiceOperationsServer(
                //    serviceBusSolutionName,
                //    issuerName,
                //    issuerKey,
                //    "OnewayEnergyServiceOperations",
                //   Microsoft.ServiceBus.ConnectivityMode.AutoDetect,
                //    typeof(OnewayEnergyServiceOperations),
                //    true);

                string serviceNamespaceDomain = ServiceBusHelper.GetServiceBusSolutionName();
                Uri address = ServiceBusEnvironment.CreateServiceUri("sb", serviceNamespaceDomain, "OnewayEnergyServiceOperations");

                ServiceHost host = new ServiceHost(typeof(OnewayEnergyServiceOperations), address);
                host.Open();

               
                Console.WriteLine("ServiceUri:" + address.ToString());

                Console.WriteLine("Service registered for public discovery.");
               NetOnewayRelayBinding binding = host.Description.Endpoints[0].Binding as NetOnewayRelayBinding;
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
