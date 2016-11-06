

namespace RESTGatewayServer
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Description;
    using Microsoft.ServiceBus;

    class Program
    {
        static void Main(string[] args)
        {

            ServiceHost host = new ServiceHost(typeof(GatewayService));
            host.Open();

            Console.WriteLine("Copy the following addresses into a browser to see the method invocations: ");
            foreach (ServiceEndpoint e in host.Description.Endpoints)
            {
                Console.WriteLine(e.ListenUri.ToString());
            }
            Console.WriteLine();
            Console.WriteLine("Press [Enter] to exit");
            Console.ReadLine();

            host.Close();
        }


    }
}