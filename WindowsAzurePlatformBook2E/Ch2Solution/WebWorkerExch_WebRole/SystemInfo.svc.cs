using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ProAzureCommon;
using System.IO;
using Microsoft.WindowsAzure.ServiceRuntime;
using System.Net;


namespace WebWorkerExch_WebRole
{
    [ServiceBehavior(AddressFilterMode = AddressFilterMode.Any)]
    public class SystemInfo : ISystemInfo
    {
        public const string LOCAL_STORAGE_NAME = "SystemInfoWorkerLocalCache";
        public const string SYSTEM_INFO_MACHINE_NAMES = "machines.txt";
        public const string SYS_INFO_CACHE_XML = "SystemInfoCache.xml";
        public static readonly SystemMessageExchange sysDS = new SystemMessageExchange();

        public void SendSystemInfo(SystemMessageExchange ds)
        {
            if (ds != null && ds.SystemInfo.Rows.Count > 0)
            {
                string machineName =  ds.SystemInfo[0].MachineName;
                string machineLocalStoragePath = ds.SystemInfo[0].LocalStoragePath;
                //Log the message

                WindowsAzureSystemHelper.LogInfo(machineName + ">" + ds.GetXml());

              //  WindowsAzureSystemHelper.PingIps(ds.SystemInfo[0].IPAddress);
                //Add machine names
                WindowsAzureSystemHelper.WriteLineToLocalStorage(SYSTEM_INFO_MACHINE_NAMES,
                    LOCAL_STORAGE_NAME, machineName, false);


                //Copy the file to LocalStorage
                string localStoragePath = WindowsAzureSystemHelper.GetLocalStorageRootPath(LOCAL_STORAGE_NAME);

                try
                {
                    string query = String.Format("MachineName = '{0}' AND LocalStoragePath = '{1}'", machineName, machineLocalStoragePath);
                    //string query = String.Format("MachineName = '{0}'", machineName);
                    WindowsAzureSystemHelper.LogInfo("Query = " + query);
                    System.Data.DataRow[] dRows = sysDS.SystemInfo.Select(query);

                    if (dRows != null && dRows.Length > 0)
                    {
                        sysDS.SystemInfo.Rows.Remove(dRows[0]);
                    }

                    sysDS.SystemInfo.Merge(ds.SystemInfo);
                    sysDS.AcceptChanges();
                    sysDS.WriteXml(Path.Combine(localStoragePath, SYS_INFO_CACHE_XML));
                    WindowsAzureSystemHelper.LogInfo("SystemInfoCache.xml -- " + sysDS.GetXml());




                }
                catch (Exception ex)
                {
                    WindowsAzureSystemHelper.LogError("SendSystemInfo():" + ex.Message);
                }

            }
            else
            {
                WindowsAzureSystemHelper.LogInfo("SendSystemInfo(): null message received");

            }
        }
    }
}
