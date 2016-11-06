using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Description;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Description;
using ProAzureDemResContract;
using AzureSample;
using System.Diagnostics;


namespace DemResWorkerRole
{
 public class WorkerRole : RoleEntryPoint
 {
  private ServiceHost host;
  private ServiceBusLogger logger;

  /*public override void Run()
  {
   try
   {
       string issuerName = GetStringConfigurationValue("UserName");
       string issuerKey = GetStringConfigurationValue("Password");
    string demResEndPoint = GetStringConfigurationValue("DemResEndpoint");
    string logEndPoint = GetStringConfigurationValue("LogEndpoint");

    ServiceBusLogger logger = new ServiceBusLogger(logEndPoint, issuerName, issuerKey);

    logger.Channel.WriteToLog(DateTime.UtcNow, "Worker Role", "Role Started");

    Uri uri = new Uri(demResEndPoint);
    TransportClientEndpointBehavior sharedSecretServiceBusCredential = new TransportClientEndpointBehavior();
    sharedSecretServiceBusCredential.CredentialType = TransportClientCredentialType.SharedSecret;
    sharedSecretServiceBusCredential.Credentials.SharedSecret.IssuerName = issuerName;
    sharedSecretServiceBusCredential.Credentials.SharedSecret.IssuerSecret = issuerKey;
    host = new ServiceHost(typeof(DemResService), uri);
    logger.Channel.WriteToLog(DateTime.UtcNow, "Worker Role", "Service Host created");
    ContractDescription contractDescription = ContractDescription.GetContract(typeof(IDemResOperations), typeof(DemResService));
    ServiceEndpoint serviceEndPoint = new ServiceEndpoint(contractDescription);

    serviceEndPoint.Address = new EndpointAddress(uri);
    logger.Channel.WriteToLog(DateTime.UtcNow, "Worker Role", "Address created");
    NetTcpRelayBinding binding = new NetTcpRelayBinding();
   
     
    serviceEndPoint.Binding = binding;

    logger.Channel.WriteToLog(DateTime.UtcNow, "Worker Role", "Binding created");

    //Set the DiscoveryType to Public and Publish this in the Service Registry
    ServiceRegistrySettings serviceRegistrySettings = new ServiceRegistrySettings();
    serviceRegistrySettings.DiscoveryMode = DiscoveryType.Public;
    serviceRegistrySettings.DisplayName = "Demand-Response Service";


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
  }*/

  public override void Run()
  {
      try
      {

          string logEndPoint = GetStringConfigurationValue("LogEndpoint");
          string issuerName = GetStringConfigurationValue("UserName");
          string issuerKey = GetStringConfigurationValue("Password");
          logger = new ServiceBusLogger(logEndPoint, issuerName, issuerKey);

          logger.Channel.WriteToLog(DateTime.UtcNow, "Worker Role", "Role Started");


          host = new ServiceHost(typeof(DemResService));
          logger.Channel.WriteToLog(DateTime.UtcNow, "Worker Role", "Service Host created");

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
      finally
      {
          if (logger != null)
          {
              if (logger.Channel.State != CommunicationState.Closed)
                  logger.Channel.Close();
          }
      }
  }
  public override void OnStop()
  {
      if (host != null)
      {
          host.Close();
      }
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

  public string GetStringConfigurationValue(string configName)
  {

   try
   {
    return RoleEnvironment.GetConfigurationSettingValue(configName);


   }
   catch (Exception ex)
   {
    logger.Channel.WriteToLog(DateTime.UtcNow,"Worker Role", ex.Message);

    throw ex;

   }


  }
 }
}
