using System;

using System.Collections.Generic;

using System.Threading;

using System.Linq;

using System.Text;

using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Diagnostics;

using System.Configuration;

using Microsoft.ServiceBus.Samples;

using System.ServiceModel;

using System.ServiceModel.Description;

using Microsoft.ServiceBus;

using Microsoft.ServiceBus.Description;
using AzureSample;

using System.Diagnostics;

namespace LogReceiverWorker
{
    public class WorkerRole : RoleEntryPoint
    {

        private ServiceHost host;



        public override void Run()
        {
            try
            {
                string issuerName = "owner"; // ConfigurationSettings.AppSettings["UserName"];
                string issuerKey = "wJBJaobUmarWn6kqv7QpaaRh3ttNVr3w1OjiotVEOL4=";//  ConfigurationSettings.AppSettings["Password"];
                string calcEndPoint = "sb://proazure.servicebus.windows.net/sample/calc/";//ConfigurationSettings.AppSettings["EndPoint"];
                string logEndPoint = "sb://proazure.servicebus.windows.net/sample/log/";//ConfigurationSettings.AppSettings["EndPoint"];
                ServiceBusLogger logger = new ServiceBusLogger(logEndPoint, issuerName, issuerKey);
                logger.Channel.WriteToLog(DateTime.UtcNow, "Worker Role", "Role Started");

                Uri uri = new Uri(calcEndPoint);
                TransportClientEndpointBehavior sharedSecretServiceBusCredential = new TransportClientEndpointBehavior();
                sharedSecretServiceBusCredential.TokenProvider = TokenProvider.CreateSharedSecretTokenProvider(issuerName, issuerKey);
                host = new ServiceHost(typeof(CalculatorService), uri);
                logger.Channel.WriteToLog(DateTime.UtcNow, "Worker Role", "Service Host created");
                ContractDescription contractDescription = ContractDescription.GetContract(typeof(ICalculator), typeof(CalculatorService));
                ServiceEndpoint serviceEndPoint = new ServiceEndpoint(contractDescription);

                serviceEndPoint.Address = new EndpointAddress(uri);
                logger.Channel.WriteToLog(DateTime.UtcNow, "Worker Role", "Address created");

                serviceEndPoint.Binding = new NetTcpRelayBinding();

                logger.Channel.WriteToLog(DateTime.UtcNow, "Worker Role", "Binding created");

                //Set the DiscoveryType to Public and Publish this in the Service Registry
                ServiceRegistrySettings serviceRegistrySettings = new ServiceRegistrySettings();
                serviceRegistrySettings.DiscoveryMode = DiscoveryType.Public;
                serviceRegistrySettings.DisplayName = "My Calc Service";


                serviceEndPoint.Behaviors.Add(sharedSecretServiceBusCredential);
                serviceEndPoint.Behaviors.Add(serviceRegistrySettings);

                logger.Channel.WriteToLog(DateTime.UtcNow, "Worker Role", "Added behaviors");

                
                host.Description.Endpoints.Add(serviceEndPoint);
                host.Open();
                logger.Channel.WriteToLog(DateTime.UtcNow, "Worker Role", "Host opened");

                while (true)
                {

                    Thread.Sleep(10000);
                    logger.Channel.WriteToLog(DateTime.UtcNow, "Worker Role", "Working…");


                }
            }
            catch (Exception ex)
            {

                Trace.WriteLine("Error starting role " + ex.Message, "Error");
            }
        }



        public override void OnStop()
        {
            host.Close();
            base.OnStop();
        }

        public override bool OnStart()
        {
            DiagnosticMonitor.Start("DiagnosticsConnectionString");

            // Restart the role upon all configuration changes
            // Note: To customize the handling of configuration changes, 
            // remove this line and register custom event handlers instead.
            // See the MSDN topic on “Managing Configuration Changes” for further details 
            // (http://go.microsoft.com/fwlink/?LinkId=166357).
            RoleEnvironment.Changing += RoleEnvironmentChanging;

            return base.OnStart();
        }

        private void RoleEnvironmentChanging(object sender, RoleEnvironmentChangingEventArgs e)
        {
            if (e.Changes.Any(change => change is RoleEnvironmentConfigurationSettingChange))
                e.Cancel = true;
        }


    }

}
