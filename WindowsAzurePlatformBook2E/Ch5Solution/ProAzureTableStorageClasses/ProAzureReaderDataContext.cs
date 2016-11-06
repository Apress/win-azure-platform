using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Services.Common;
using System.Data.Services.Client;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using Microsoft.WindowsAzure.StorageClient.Protocol;
using Microsoft.WindowsAzure.StorageClient.Tasks;

namespace ProAzureTableStorageClasses
{
    public class ProAzureReaderDataContext : TableServiceContext
    {

        public ProAzureReaderDataContext() : base(null, null) { }
     
       public IQueryable<ProAzureReader> ProAzureReader
        {
            get
            {
                return this.CreateQuery<ProAzureReader>("ProAzureReader");
            }
        }

       public void AddRecord(
           DateTime purchaseDate, 
           string country, 
           string state,
           string city,
           string zip,
           string purchaseLocation,
           string purchaseType,
           string readerName,
           string readerUrl,
           string feedback)
       {
           ProAzureReader pa = new ProAzureReader(city);
           pa.Country = country;
           pa.Feedback = feedback;
           pa.PurchaseDate = purchaseDate;
           pa.PurchaseLocation = purchaseLocation;
           pa.PurchaseType = purchaseType;
           pa.ReaderName = readerName;
           pa.ReaderUrl = readerUrl;
           pa.State = state;
           pa.Zip = zip;

           this.AddObject("ProAzureReader", pa);
           this.SaveChanges();
       }
    }
}
