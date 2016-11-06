using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Microsoft.WindowsAzure.StorageClient;
using Microsoft.WindowsAzure;

namespace ServiceManagement
{
    /*
       <EventTickCount>634117372552460000</EventTickCount>
       <DeploymentId>deployment(62)</DeploymentId>
       <Role>WebRole1</Role>
       <RoleInstance>deployment(62).TestDeployViaREST.WebRole1.0</RoleInstance>
       <CounterName>\Processor(_Total)\% Processor Time</CounterName>
       <CounterValue>94.50037</CounterValue>
    */
    public class PerformanceData : Microsoft.WindowsAzure.StorageClient.TableServiceEntity
    {
        public Int64 EventTickCount { get; set; }
        public string DeploymentId { get; set; }
        public string Role { get; set; }
        public string RoleInstance { get; set; }
        public string CounterName { get; set; }
        public double CounterValue { get; set; }

    }

    public class PerformanceDataContext : TableServiceContext
    {
        public IQueryable<PerformanceData> PerfData
        {
            get
            {
                return this.CreateQuery<PerformanceData>("WADPerformanceCountersTable");
            }
        }

        public PerformanceDataContext(string baseAddress, StorageCredentials credentials)
            : base(baseAddress, credentials)
        {
        }

        public void AddCounterRow(Int64 TickCount, string DeploymentId, string Role,
                                    string RoleInstance, string CounterName, double CounterValue)
        {

            //this.AddObject("PerformanceData", new PerformanceData (TickCount, DeploymentId, Role, 
            //                                    RoleInstance, CounterName, CounterValue);

            //this.SaveChanges();

            throw (new NotImplementedException());

        }

    }
}
