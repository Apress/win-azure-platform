using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Text;
using Microsoft.ServiceHosting.ServiceRuntime;
using System.Configuration;
using AzureSample;


namespace AsyncMessageReceiverSB_Worker
{
    public class WorkerRole : RoleEntryPoint
    {
        public override void Start()
        {
            try
            {
                string endPoint = "sb://proazure.servicebus.windows.net/sample/log/";//ConfigurationSettings.AppSettings["EndPoint"];
                string userName = "proazure"; // ConfigurationSettings.AppSettings["UserName"];
                string password = "proazure";//  ConfigurationSettings.AppSettings["Password"];
                ServiceBusLogger logger = new ServiceBusLogger(endPoint, userName, password);
                logger.Channel.WriteToLog(DateTime.UtcNow, "Worker Role", "Role Started");
                while (true)
                {
                    Thread.Sleep(1000);
                    logger.Channel.WriteToLog(DateTime.UtcNow, "Worker Role", "Working…");

                }
            }
            catch (Exception ex)
            {

                RoleManager.WriteToLog("Error", "Error starting role " + ex.Message);
                RoleManager.WriteToLog("Error", "Stack Trace " + ex.StackTrace);
            }

        }


        public override RoleStatus GetHealthStatus()
        {
            return RoleStatus.Healthy;
        }


    }
}
