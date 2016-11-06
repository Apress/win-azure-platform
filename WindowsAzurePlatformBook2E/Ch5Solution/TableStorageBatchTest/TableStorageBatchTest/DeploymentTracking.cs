using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Data.Services.Client;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security;
using Microsoft.WindowsAzure.StorageClient;
using Microsoft.WindowsAzure;

namespace TableStorageBatchTest
{
    public class DeploymentTrackingDataServiceContext : TableServiceContext
    {
        public DeploymentTrackingDataServiceContext() : base(null, null) { }
        public IQueryable<DeploymentTracking> Tracking
        {
            get
            {
                return this.CreateQuery<DeploymentTracking>("dddeploymenttracking");
            }
        }
    }
    [Serializable]
    public class DeploymentTracking : TableServiceEntity
    {
        public const string TABLE_NAME = "dddeploymenttracking";
        private long _provId;
        public long ProvId
        {
            get { return _provId; }
            set
            {
                _provId = value;
                PartitionKey = value.ToString();

            }
        }
        public int RetryNumber { get; set; }
        public string Operation { get; set; }
        public bool IsSuccessful { get; set; }
        public string StatusMessage { get; set; }
        public string StatusCode { get; set; }

        private DateTime _created;

        public DateTime Created
        {
            get { return _created; }
            set
            {
                _created = value;
               // RowKey = value.ToString("T");
            }
        }

        public string CreatedBy { get; set; }

        public DeploymentTracking() {
           // RowKey = string.Empty;
        }

        public DeploymentTracking(long provId, int retryNumber, string operation, bool isSuccessful, string statusMessage, string statusCode, DateTime created, string createdBy)
        {
            ProvId = provId;
            PartitionKey = provId.ToString();
            RetryNumber = retryNumber;
            Operation = operation;
            IsSuccessful = isSuccessful;
            StatusMessage = statusMessage;
            StatusCode = statusCode;
            Created = created;
         //   RowKey = DateTime.UtcNow.ToString("T");
        //    RowKey = System.Guid.NewGuid().ToString("N");
          //  RowKey = string.Empty;
            CreatedBy = createdBy;


        }
    }
}
