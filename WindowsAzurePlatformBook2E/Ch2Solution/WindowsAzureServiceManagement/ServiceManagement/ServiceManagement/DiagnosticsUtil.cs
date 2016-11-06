using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure;

namespace ServiceManagement
{
   public class DiagnosticsUtil
    {
       public static double GetAverageProcessorTime(string deploymentId, string storageAccountName, string storageKey, int timeFrameInMinutes)
       {

           try
           {
               //var account = CloudStorageAccount.FromConfigurationSetting("DataConnectionString");
               var account = new CloudStorageAccount(new StorageCredentialsAccountAndKey(storageAccountName, storageKey), true);
               var context = new PerformanceDataContext(account.TableEndpoint.ToString(), account.Credentials);
               var data = context.PerfData;
               DateTime tf = DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(timeFrameInMinutes));
               // && (DateTime.Compare(tf, d.Timestamp) < 0)
               System.Collections.Generic.List<PerformanceData> selectedData = (from d in data
                                                                                where d.CounterName == @"\Processor(_Total)\% Processor Time"
                                                                                  && d.DeploymentId == deploymentId
                                                                                  && (DateTime.Compare(tf, d.Timestamp) < 0)
                                                                                select d).ToList<PerformanceData>();

               return (from d in selectedData
                       where d.CounterName == @"\Processor(_Total)\% Processor Time"
                       select d.CounterValue).Average();




           }
           catch (System.Exception ex)
           {
               throw ex;
           }
       }

       public static double GetAverageMemoryUsageInMbytes(string deploymentId, string storageAccountName, string storageKey, int timeFrameInMinutes)
       {

           try
           {
               //var account = CloudStorageAccount.FromConfigurationSetting("DataConnectionString");
               var account = new CloudStorageAccount(new StorageCredentialsAccountAndKey(storageAccountName, storageKey), true);
               var context = new PerformanceDataContext(account.TableEndpoint.ToString(), account.Credentials);
               var data = context.PerfData;
               DateTime tf = DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(timeFrameInMinutes));
               // && (DateTime.Compare(tf, d.Timestamp) < 0)
               System.Collections.Generic.List<PerformanceData> selectedData = (from d in data
                                                                                where d.CounterName == @"\Memory\Available MBytes"
                                                                                && d.DeploymentId == deploymentId
                                                                                && (DateTime.Compare(tf, d.Timestamp) < 0)
                                                                                select d).ToList<PerformanceData>();

               if (selectedData.Count > 0)
               {
                   return (from d in selectedData
                           where d.CounterName == @"\Memory\Available MBytes"
                           select d.CounterValue).Average();
               }
               else
               {

                   return 0;

               } 



           }
           catch (System.Exception ex)
           {
               throw ex;
           }


       }
    }
}
