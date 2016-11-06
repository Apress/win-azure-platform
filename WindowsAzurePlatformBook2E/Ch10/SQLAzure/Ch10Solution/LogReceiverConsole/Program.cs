using System;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Description;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Description;

namespace AzureSample
{
    class Program
    {

        static void Main(string[] args)
        {           
                string endPoint = ConfigurationManager.AppSettings["EndPoint"];
                string issuerName = ConfigurationManager.AppSettings["UserName"];
                string issuerKey = ConfigurationManager.AppSettings["Password"];

                Uri uri = new  Uri(endPoint);

                TransportClientEndpointBehavior sharedSecretServiceBusCredential = new TransportClientEndpointBehavior();
                sharedSecretServiceBusCredential.TokenProvider = TokenProvider.CreateSharedSecretTokenProvider(issuerName, issuerKey);

                ServiceHost host = new ServiceHost(typeof(LogService), uri);
            try
            {
                ContractDescription contractDescription = ContractDescription.GetContract(typeof(ILogContract), typeof(LogService));
                ServiceEndpoint serviceEndPoint = new ServiceEndpoint(contractDescription);
                serviceEndPoint.Address = new EndpointAddress(uri);
                serviceEndPoint.Binding = new NetTcpRelayBinding();


                serviceEndPoint.Behaviors.Add(sharedSecretServiceBusCredential);
                host.Description.Endpoints.Add(serviceEndPoint);
                host.Open();

                Console.WriteLine(String.Format("Listening at: {0}", endPoint));
                Console.WriteLine("Press [Enter] to exit");
                Console.ReadLine();
                //host.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            finally
            {
                if (host != null)
                {
                    if (host.State != CommunicationState.Closed)
                        host.Close();
                }
            }
        }

    }

}

