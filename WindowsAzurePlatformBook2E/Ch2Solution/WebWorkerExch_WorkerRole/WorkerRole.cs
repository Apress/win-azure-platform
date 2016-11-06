using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using ProAzureCommon;
using System.IO;
using System.ServiceModel;

namespace WebWorkerExch_WorkerRole
{
    public class WorkerRole : RoleEntryPoint
    {
        private const string LOCAL_STORAGE_NAME = "SystemInfoWorkerLocalCache";
        private static int ThreadSleepInMillis = 10000;

        public override void Run()
        {
           
            // This is a sample worker implementation. Replace with your logic.
            Trace.WriteLine("WebWorkerExchange_WorkerRole entry point called", "Information");

            WindowsAzureSystemHelper.LogInfo("Worker Process entry point called");
            ThreadSleepInMillis = WindowsAzureSystemHelper.GetIntConfigurationValue("ThreadSleepTimeInMillis");

            while (true)
            {
                ExecuteExchange();
                Thread.Sleep(ThreadSleepInMillis);
                WindowsAzureSystemHelper.LogInfo("Working");

            }
        }

        private void SetupConfigurationSettingPublisher()
        {
            // This code sets up a handler to update CloudStorageAccount instances when their corresponding
            // configuration settings change in the service configuration file.
            Trace.WriteLine("Setting up configuration setting publishing");
            CloudStorageAccount.SetConfigurationSettingPublisher((configName, configSetter) =>
            {
                // Provide the configSetter with the initial value
                configSetter(RoleEnvironment.GetConfigurationSettingValue(configName));

                RoleEnvironment.Changed += (sender, arg) =>
                {
                    if (arg.Changes.OfType<RoleEnvironmentConfigurationSettingChange>()
                        .Any((change) => (change.ConfigurationSettingName == configName)))
                    {
                        // The corresponding configuration setting has changed, propagate the value
                        if (!configSetter(RoleEnvironment.GetConfigurationSettingValue(configName)))
                        {
                            // In this case, the change to the storage account credentials in the
                            // service configuration is significant enough that the role needs to be
                            // recycled in order to use the latest settings. (for example, the 
                            // endpoint has changed)
                            RoleEnvironment.RequestRecycle();
                        }
                    }
                };
            });
        }
        public override bool OnStart()
        {
            SetupConfigurationSettingPublisher();
            // Start the diagnostic monitor with this custom configuration 
            DiagnosticMonitor.Start("Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString");

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

        private void ExecuteExchange()
        {
            try
            {
                SystemMessageExchange ds = WindowsAzureSystemHelper.GetSystemInfo(LOCAL_STORAGE_NAME, "Worker");


                if (ds == null)
                {
                    WindowsAzureSystemHelper.LogError("ExecuteExchange():SystemMessageExchange DataSet is null");

                }
                else
                {
                    WindowsAzureSystemHelper.LogInfo(ds.GetXml());
                    string url = WindowsAzureSystemHelper.GetStringConfigurationValue("SystemInfoServiceURL");

                    CallSystemInfoService(url, ds);
                }
            }
            catch (Exception ex)
            {

                WindowsAzureSystemHelper.LogError("ExecuteExchange():" + ex.Message);
            }

        }

        private void CallSystemInfoService(string url, SystemMessageExchange ds)
        {
            SystemInfoService.SystemInfoClient client = null;
            BasicHttpBinding bind = new BasicHttpBinding();

            try
            {
                EndpointAddress endpoint = new EndpointAddress(url);

                client = new SystemInfoService.SystemInfoClient(bind, endpoint);
                client.SendSystemInfo(ds);
                WindowsAzureSystemHelper.LogInfo(String.Format("Sent message to Service URL {0}", url));


            }
            catch (Exception ex)
            {
                WindowsAzureSystemHelper.LogError("CallSystemInfoService():" + ex.Message);

            }
            finally
            {
                if (client != null)
                {

                    if (client.State == CommunicationState.Faulted)
                        client.Abort();
                    else
                        client.Close();
                }

            }

        }
    }
}
