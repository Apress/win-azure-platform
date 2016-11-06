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
    public class ProAzureReader : TableServiceEntity
    {

        public ProAzureReader() : base()
           
        {
            CreateKeys("NO CITY");
        }

        public ProAzureReader(string readerCity)
        {
            CreateKeys(readerCity);
        }
        public DateTime PurchaseDate
        { get; set; }
        public DateTime EntryDate
        { get; set; }
        public string Country
        { get;set;}
        public string State
        { get; set; }
        public string City
        { get; set; }
        public string Zip
        { get; set; }
        public string PurchaseLocation
        { get; set; }
        public string PurchaseType
        { get; set; }
        public string ReaderName
        { get; set; }
        public string ReaderUrl
        { get; set; }
        public string Feedback
        { get; set; }


        private void CreateKeys(string readerCity)
        {
           //EntryDate = TableStorageConstants.MinSupportedDateTime;
            EntryDate = DateTime.UtcNow;
            City = readerCity;
            RowKey = string.Format("{0:10}_{1}", DateTime.MaxValue.Ticks - EntryDate.Ticks, Guid.NewGuid());
            //By Entry Date: [Query: Get purchase records entered today]
            PartitionKey = EntryDate.ToString("MMddyyyy");

            //Other Partition Key Options
            //By City [Query: Get purchase records by city]
            //PartitionKey = ReaderCity;
            //By Location and a Time component [Query: Get purchase records by city for millions of records, high volume]
            //PartitionKey = String.Format("{0}_{1:10}", ReaderCity, DateTime.MaxValue.Ticks - EntryDate.Ticks); 

        }
    }
}
