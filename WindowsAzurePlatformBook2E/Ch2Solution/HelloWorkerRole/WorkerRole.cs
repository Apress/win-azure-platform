﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;
using System.Net;
using System.ServiceModel;
using HelloService;

namespace HelloWorkerRole
{
    public class WorkerRole : RoleEntryPoint
    {
        private const double DefaultSampleRate = 5.0;

        public override void Run()
        {
            // This is a sample worker implementation. Replace with your logic.
            Trace.WriteLine("HelloWorkerRole entry point called", "Information");
           
            SetupConfigurationSettingPublisher();
            Trace.WriteLine("Publisher setup...");

            var internalEndpoint = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["MyInternalEndpoint"];
            var wcfAddress = new Uri(String.Format("net.tcp://{0}", internalEndpoint.IPEndpoint.ToString()));
            Trace.WriteLine(wcfAddress.ToString());
            var wcfHost = new ServiceHost(typeof(HelloServiceImpl), wcfAddress);
            var binding = new NetTcpBinding(SecurityMode.None);
            wcfHost.AddServiceEndpoint(typeof(IHelloService), binding, "helloservice");
            try
            {
                wcfHost.Open();
                while (true)
                {
                    Thread.Sleep(10000);
                    Trace.WriteLine("Working", "Information");
                }
            }
            finally
            {
                wcfHost.Close();

            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;
            SetupDiagnostics();
            Trace.WriteLine("Diagnostics setup...");
            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

          
            RoleEnvironment.Changing += RoleEnvironmentChanging;

            return base.OnStart();
        }

        private void RoleEnvironmentChanging(object sender, RoleEnvironmentChangingEventArgs e)
        {
            // If a configuration setting is changing
            if (e.Changes.Any(change => change is RoleEnvironmentConfigurationSettingChange))
            {
                // Set e.Cancel to true to restart this role instance
                e.Cancel = true;
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


        private void SetupDiagnostics()
        {
            Trace.WriteLine("Setting up diagnostics", "Information");

            DiagnosticMonitorConfiguration diagConfig = DiagnosticMonitor.GetDefaultInitialConfiguration();

            // Add performance counter monitoring for configured counters
            // Run typeperf.exe /q to query the counter list 
            string perfCounterString = RoleEnvironment.GetConfigurationSettingValue("PerformanceCounters");

            if (!string.IsNullOrEmpty(perfCounterString))
            {
                IList<string> perfCounters = perfCounterString.Split(',').ToList();

                // Setup each counter specified in comma delimitered string
                foreach (string perfCounter in perfCounters)
                {
                    diagConfig.PerformanceCounters.DataSources.Add(
                        new PerformanceCounterConfiguration
                        {
                            CounterSpecifier = perfCounter,
                            SampleRate = TimeSpan.FromSeconds(DefaultSampleRate)
                        }
                        );
                }

                // Update counter information in Azure every 30 seconds
                diagConfig.PerformanceCounters.ScheduledTransferPeriod = TimeSpan.FromMinutes(0.5);
            }

            diagConfig.DiagnosticInfrastructureLogs.ScheduledTransferPeriod = TimeSpan.FromMinutes(0.5);

            // Specify a logging level to filter records to transfer
            diagConfig.DiagnosticInfrastructureLogs.ScheduledTransferLogLevelFilter = LogLevel.Verbose;

            // Set scheduled transfer interval for user's Windows Azure Logs to 1 minute
            diagConfig.Logs.ScheduledTransferPeriod = TimeSpan.FromMinutes(5);

            diagConfig.Directories.ScheduledTransferPeriod = TimeSpan.FromMinutes(5);

            Microsoft.WindowsAzure.Diagnostics.CrashDumps.EnableCollection(true);


            //Event Logs
            // Add event collection from the Windows Event Log
            diagConfig.WindowsEventLog.DataSources.Add("System!*");
            diagConfig.WindowsEventLog.DataSources.Add("Application!*");
            diagConfig.WindowsEventLog.DataSources.Add("Security!*");


            // Start the diagnostic monitor with this custom configuration 
            DiagnosticMonitor.Start("Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString", diagConfig);
        }
    }
}
