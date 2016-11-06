using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Services.Common;
using System.Data.Services.Client;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;
using Microsoft.WindowsAzure.StorageClient.Protocol;
using Microsoft.WindowsAzure.StorageClient.Tasks;
using System.Configuration;

namespace ProAzureTableStorageClasses
{
   public class ProAzureReaderDataSource
    {
        private TableServiceContext dContext;
        public CloudStorageAccount Account {get;set;}
        public CloudTableClient TableClient { get; set; }

        public const string ENTITY_SET_NAME = "ProAzureReader";
        public ProAzureReaderDataSource()
        {
            Init("StorageAccountConnectionString");
            dContext = TableClient.GetDataServiceContext();
            dContext.RetryPolicy = RetryPolicies.Retry(3, TimeSpan.FromSeconds(5));
            
        }

        public ProAzureReaderDataSource(string storageAccountConnectionString)
        {
            Init(storageAccountConnectionString);
            dContext = TableClient.GetDataServiceContext();
            dContext.RetryPolicy = RetryPolicies.Retry(3, TimeSpan.FromSeconds(5));

        }

        private void Init(string configurationSettingName)
        {
            if (RoleEnvironment.IsAvailable)
            {
                CloudStorageAccount.SetConfigurationSettingPublisher(
                    (configName, configSettingPublisher) =>
                    {
                        var connectionString = RoleEnvironment.GetConfigurationSettingValue(configName);
                        configSettingPublisher(connectionString);
                    }
                );
            }
            else
            {
                CloudStorageAccount.SetConfigurationSettingPublisher(
                    (configName, configSettingPublisher) =>
                    {
                        var connectionString = ConfigurationManager.ConnectionStrings[configName].ConnectionString;
                        configSettingPublisher(connectionString);
                    }
                );
            }

            Account = CloudStorageAccount.FromConfigurationSetting(configurationSettingName);

            TableClient = Account.CreateCloudTableClient();
            TableClient.RetryPolicy = RetryPolicies.Retry(3, TimeSpan.FromMilliseconds(100) );

        }

        public IEnumerable<ProAzureReader> Select()
        {
            var results = from g in dContext.CreateQuery<ProAzureReader>(ENTITY_SET_NAME)
                       where g.PartitionKey == DateTime.UtcNow.ToString("MMddyyyy")
                          select g;
            var r = results.ToArray<ProAzureReader>();
            return r;
        }

        public IEnumerable<ProAzureReader> SelectByCity(string city)
        {
            var results = from g in dContext.CreateQuery<ProAzureReader>(ENTITY_SET_NAME)
                       where g.PartitionKey == DateTime.UtcNow.ToString("MMddyyyy")
                       && g.City == city
                       select g;
         var r = results.ToArray<ProAzureReader>();
         return r;
        }
        public IEnumerable<ProAzureReader> SelectByState(string state)
        {
            var results = from g in dContext.CreateQuery<ProAzureReader>(ENTITY_SET_NAME)
                       where g.PartitionKey == DateTime.UtcNow.ToString("MMddyyyy")
                       && g.State == state
                       select g;
         var r = results.ToArray<ProAzureReader>();
         return r;
        }
        public IEnumerable<ProAzureReader> SelectByCountry(string country)
        {
            var results = from g in dContext.CreateQuery<ProAzureReader>(ENTITY_SET_NAME)
                       where g.PartitionKey == DateTime.UtcNow.ToString("MMddyyyy")
                       && g.Country == country
                       select g;
         var r = results.ToArray<ProAzureReader>();
         return r;
        }

        public IEnumerable<ProAzureReader> SelectByPurchaseDate(DateTime purchaseDate)
        {
            var results = from g in dContext.CreateQuery<ProAzureReader>(ENTITY_SET_NAME)
                       where g.PurchaseDate.Equals(purchaseDate )
                       select g;
         var r = results.ToArray<ProAzureReader>();
         return r;
        }

        public IEnumerable<ProAzureReader> SelectTopN(int topNumber)
        {
            var results = dContext.CreateQuery<ProAzureReader>(ENTITY_SET_NAME).Take(topNumber);
         var r = results.ToArray<ProAzureReader>();
         return r;
        }

        public void AddProAzureReader(ProAzureReader newItem)
        {
         dContext.AddObject(ENTITY_SET_NAME, newItem);
         dContext.SaveChangesWithRetries(SaveChangesOptions.None);
        }
        public void UpdateFeedback(string PartitionKey, string RowKey, string feedback)
        {
            var results = from g in dContext.CreateQuery<ProAzureReader>(ENTITY_SET_NAME)
                          where g.PartitionKey == PartitionKey
                          && g.RowKey == RowKey
                          select g;
            var e = results.FirstOrDefault<ProAzureReader>();
            e.Feedback = feedback;
            dContext.MergeOption = MergeOption.PreserveChanges;
            dContext.UpdateObject(e);
            dContext.SaveChanges();
        }

        public void UpdateUrl(string PartitionKey, string RowKey, string url)
        {
            var results = from g in dContext.CreateQuery<ProAzureReader>(ENTITY_SET_NAME)
                       where g.PartitionKey == PartitionKey
                       && g.RowKey == RowKey
                       select g;
         var e = results.FirstOrDefault<ProAzureReader>();
         e.ReaderUrl = url;
         dContext.MergeOption = MergeOption.PreserveChanges;
         dContext.UpdateObject(e);
         dContext.SaveChanges();
         
        }



        public bool CreateTableIfNotExist(string tableName)
        {
            try
            {
                TableClient.CreateTableIfNotExist(tableName);
                return true;
            }
            catch (StorageClientException ex)
            {
                if ((int)ex.StatusCode == 409)
                {
                    return false;
                }

                throw;
            }
        }

        public IEnumerable<string> ListTables()
        {
            return TableClient.ListTables();

        }
        public void DeleteTable(string tableName)
        {
            TableClient.DeleteTable(tableName);


        }
    }
}
