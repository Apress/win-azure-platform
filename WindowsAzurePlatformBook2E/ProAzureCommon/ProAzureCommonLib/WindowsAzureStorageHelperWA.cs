using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.ServiceRuntime;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.ServiceModel;
using System.Management;
//using Microsoft.Samples.ServiceHosting.StorageClient;
using Microsoft.WindowsAzure.StorageClient;
using Microsoft.WindowsAzure;
using System.Collections.Specialized;
using System.Security;
using Microsoft.Win32;
using System.IO.Compression;

namespace ProAzureCommonLib
{
    public class WindowsAzureStorageHelperWA
    {
        #region private

        
        
        #region Blob
       
        private const string CONTAINER_NAME = "proazuretestcontainer"; 
        
        #endregion

      
        
        #endregion

        #region Properties
        public CloudStorageAccount StorageAccountInfo
        {
         get;
         set;
        }
      
        public string ContainerName
        {
         get;
         set;
        }
        public NameValueCollection ContainerMetaData
        {
         get;
         set;
        }
        public CloudBlobClient BlobStorageType
        {
         get;
         set;
        }

        public CloudQueueClient QueueStorageType
        {
            get ;set;
        }

        public CloudTableClient TableStorageType
        { get; set; }

        #endregion
        #region Constructors
        public WindowsAzureStorageHelperWA(string configurationSettingName)
        {
            ContainerName = CONTAINER_NAME;
        
           
            StorageAccountInfo = CloudStorageAccount.FromConfigurationSetting(configurationSettingName);
            InitStorage(StorageAccountInfo);
        }
        public WindowsAzureStorageHelperWA(string accountName, string accountKey, bool isLocal, string blobEndpointURI, string queueEndpointURI, string tableEndpointURI)
        {

         ContainerName = CONTAINER_NAME;

            StorageCredentials sc = new StorageCredentialsAccountAndKey(accountName, accountKey);
            StorageAccountInfo = new CloudStorageAccount(sc, new Uri(blobEndpointURI), new Uri(queueEndpointURI), new Uri(tableEndpointURI));
            InitStorage(StorageAccountInfo);
        }

        private void InitStorage(CloudStorageAccount st)
        {
            // Open blob storage.
            BlobStorageType = st.CreateCloudBlobClient();
            BlobStorageType.RetryPolicy = RetryPolicies.Retry(2, TimeSpan.FromMilliseconds(100));
            BlobStorageType.GetContainerReference(CONTAINER_NAME).Create();

            // Open queue storage.

            QueueStorageType = st.CreateCloudQueueClient();

            QueueStorageType.RetryPolicy = RetryPolicies.Retry(2, TimeSpan.FromMilliseconds(100));
            QueueStorageType.GetQueueReference("testqueue").CreateIfNotExist();

            // Open table storage.

            TableStorageType = st.CreateCloudTableClient();
            TableStorageType.RetryPolicy = RetryPolicies.Retry(2, TimeSpan.FromMilliseconds(100));
            TableStorageType.CreateTableIfNotExist("testtable");

        }
        #endregion
       
        #region Blob methods
        /// <summary>
        /// Return a list of BLOB containers.
        /// </summary>
        /// <returns></returns>
        /// 
        public IEnumerable<CloudBlobContainer> GetContainers()
        {
            return this.BlobStorageType.ListContainers();
        }
        public string[] GetContainerNames(IEnumerable<CloudBlobContainer> containers)
        {
            if (containers != null && containers.Count() > 0)
            {
                var cN = from CloudBlobContainer b in containers
                         select b.Name;
                return cN.ToArray<string>();
            }

            return new string[] { };

        }

        public IList<CloudBlobContainer> GetContainers(string prefix, int maxResults, ref string marker)
        {
            
            return new List<CloudBlobContainer>(this.BlobStorageType.ListContainers(prefix, ContainerListingDetails.All));
        }

        public bool CreateContainer(string containerName)
        {
            return BlobStorageType.GetContainerReference(containerName).CreateIfNotExist();
                      
        }
        public bool CreateContainer(string containerName, BlobContainerPublicAccessType accessType, NameValueCollection metadata)
        {

            CloudBlobContainer container = BlobStorageType.GetContainerReference(containerName);
            BlobContainerPermissions perm = new BlobContainerPermissions();
            perm.PublicAccess = accessType;
            container.SetPermissions(perm);
            container.Metadata.Add(metadata);
            return container.CreateIfNotExist();
                        
        }

        public BlobContainerProperties GetContainerProperties(string containerName)
        {
            return BlobStorageType.GetContainerReference(containerName).Properties;
           
        }

        public bool SetContainerMetadata(string containerName, NameValueCollection metadata)
        {

            CloudBlobContainer container = BlobStorageType.GetContainerReference(containerName);
            container.Metadata.Add(metadata);
            container.SetMetadata();
            return true;
        }

        public BlobContainerPermissions GetContainerPermissions(string containerName)
        {
            return BlobStorageType.GetContainerReference(containerName).GetPermissions();
        

        }

        public void SetContainerPermissions(string containerName, BlobContainerPermissions perms)
        {
            CloudBlobContainer container = BlobStorageType.GetContainerReference(containerName);
            container.SetPermissions(perms);
          
        }

        public bool DeleteContainer(string containerName)
        {
            BlobStorageType.GetContainerReference(containerName).Delete();
            return true;
        }

        public IEnumerable<IListBlobItem> ListBlobs(string containerName, string prefix)
        {
            return BlobStorageType.GetContainerReference(containerName).ListBlobs();
            
         }

        public IEnumerable<IListBlobItem> ListBlobs(string containerName, string prefix, string delimiter, int maxResults, ref string marker)
        {
                       
           return BlobStorageType.GetContainerReference(containerName).ListBlobs();

            
        }

        public void CreateBlob(string containerName, string blobName, BlobProperties blobProperties, byte[] blobContents, bool overwrite)
        {

            BlobStorageType.GetContainerReference(containerName).GetBlobReference(blobName).UploadByteArray(blobContents);

            
        }

        public void CreateBlob(string containerName, string blobName, BlobProperties blobProperties,string blobContents, bool overwrite)
        {

            BlobStorageType.GetContainerReference(containerName).GetBlobReference(blobName).UploadText(blobContents);


        }
        public void CreateBlob(string containerName, string blobName, BlobProperties blobProperties, Stream blobContents, bool overwrite)
        {
           
            BlobStorageType.GetContainerReference(containerName).GetBlobReference(blobName).UploadFromStream(blobContents);


        }
        public void CreateBlobFromFile(string containerName, string blobName, BlobProperties blobProperties, string fileName, bool overwrite)
        {

            BlobStorageType.GetContainerReference(containerName).GetBlobReference(blobName).UploadFile(fileName);


        }


        public bool DeleteBlob(string containerName, string blobName)
        {

            return BlobStorageType.GetContainerReference(containerName).GetBlobReference(blobName).DeleteIfExists();
          

        }

     
       

        public byte[] GetBlobAsByteArray(string containerName, string blobName)
        {

            return BlobStorageType.GetContainerReference(containerName).GetBlobReference(blobName).DownloadByteArray();
            

        }

        public string GetBlobAsText(string containerName, string blobName)
        {

            return BlobStorageType.GetContainerReference(containerName).GetBlobReference(blobName).DownloadText();


        }

        public void GetBlobAsStream(string containerName, string blobName, Stream outputStream)
        {

            BlobStorageType.GetContainerReference(containerName).GetBlobReference(blobName).DownloadToStream(outputStream);


        }

        public void GetBlobAsFile(string containerName, string blobName, string outputFileName)
        {

            BlobStorageType.GetContainerReference(containerName).GetBlobReference(blobName).DownloadToFile(outputFileName);


        }

     
        public BlobProperties GetBlobProperties(string containerName, string blobName)
        {

           return BlobStorageType.GetContainerReference(containerName).GetBlobReference(blobName).Properties;
            

        }


      

        public void UpdateBlobMetadata(string containerName, string blobName, BlobProperties blobProperties)
        {
            CloudBlob cb = BlobStorageType.GetContainerReference(containerName).GetBlobReference(blobName);
            cb.Properties.CacheControl = blobProperties.CacheControl;
            cb.Properties.ContentEncoding = blobProperties.ContentEncoding;
            cb.Properties.ContentMD5 = blobProperties.ContentMD5;
            cb.Properties.ContentType = blobProperties.ContentType;
           
            BlobStorageType.GetContainerReference(containerName).GetBlobReference(blobName).SetProperties();

            
        }

      
        public void CopyFromBlob(string sourceContainerName, string sourceBlobName, string destinationContainerName, string destinationBlobName, string versionString)
        {
           CloudBlob source = BlobStorageType.GetContainerReference(sourceContainerName).GetBlobReference(sourceBlobName);
           BlobStorageType.GetContainerReference(destinationContainerName).GetBlobReference(destinationBlobName).CopyFromBlob(source);
        }

        public static string GetContentTypeFromExtension(string extension)
        {
            string cType = "application/unknown";
            try
            {
               
                RegistryKey key = Registry.ClassesRoot.OpenSubKey(extension, false);
                if ((key != null) && (key.GetValue("Content Type") != null))
                {
                    return key.GetValue("Content Type").ToString();
                }

                string silverlightSpecial;
                if (((silverlightSpecial = extension) != null) && (silverlightSpecial == ".xap"))
                {
                    cType = "application/x-silverlight-app";
                }
                return cType;
            }
            catch (SecurityException)
            {
            }
            
            if (extension == null)
            {
                return cType;
            }
            string imageFiles = extension;
            if (!(imageFiles == ".docx") && !(imageFiles == ".doc"))
            {
                if ((imageFiles != ".jpg") && (imageFiles != ".jpeg"))
                {
                    if (imageFiles != ".png")
                    {
                        return cType;
                    }
                    return "image/png";
                }
            }
            else
            {
                return "application/msword";
            }
            return "image/jpeg";
        }

        public static byte[] GZipCompressedBlobContents(string fileName)
        {
            var compressedData = new MemoryStream();

            using (var gzipStream = new GZipStream(compressedData,
                CompressionMode.Compress))
            {
                byte[] fileBytes = File.ReadAllBytes(fileName);
                gzipStream.Write(fileBytes, 0,
                    fileBytes.Length);
            }

            return compressedData.ToArray();
        }
       
        #endregion
       
        #region Queue methods
        public IEnumerable<CloudQueue> ListQueues(string prefix)
        {
            if (string.IsNullOrEmpty(prefix))
            {
                return this.QueueStorageType.ListQueues();
            }
            else
            {
                return this.QueueStorageType.ListQueues(prefix);
            }

        }
        public string[] GetQueueNames(IEnumerable<CloudQueue> queues)
        {
            if (queues != null && queues.Count() > 0)
            {
                var cN = from CloudQueue b in queues
                         select b.Name;
                return cN.ToArray<string>();
            }

            return new string[] { };

        }
        public CloudQueue GetQueue(string queueName)
        {
            return  QueueStorageType.GetQueueReference(queueName);

        }

        public bool CreateQueue(string queueName, out bool alreadyExists)
        {
            alreadyExists = false;
            GetQueue(queueName).Create();
            return true;

        }

        public bool DeleteQueue(string queueName)
        {
           GetQueue(queueName).Delete();
           return true;

        }

        public QueueAttributes GetQueueProperties(string queueName)
        {
            return GetQueue(queueName).Attributes;
            
           
        }

        public bool SetQueueProperties(string queueName, NameValueCollection queueProps)
        {
            GetQueue(queueName).Attributes.Metadata.Add(queueProps);
            GetQueue(queueName).SetMetadata();
            return true;

        }

        public bool PutMessage(string queueName, CloudQueueMessage queueMessage)
        {
            GetQueue(queueName).AddMessage(queueMessage);
            return true;


        }

        public bool PutMessage(string queueName, CloudQueueMessage queueMessage, int timeToLiveInSecs)
        {
           GetQueue(queueName).AddMessage(queueMessage, TimeSpan.FromSeconds(timeToLiveInSecs));
            return true;
            

        }

        public IEnumerable<CloudQueueMessage> GetMessages(string queueName, int numberofMessages, int visibilityTimeoutInSecs)
        {
           return GetQueue(queueName).GetMessages(numberofMessages, TimeSpan.FromSeconds(visibilityTimeoutInSecs));


        }


        public IEnumerable<CloudQueueMessage> PeekMessages(string queueName, int numberofMessages)
        {
            return GetQueue(queueName).PeekMessages(numberofMessages);

            
        }

        public CloudQueueMessage PeekMessage(string queueName)
        {
            return GetQueue(queueName).PeekMessage();
            


        }

        public bool DeleteMessage(string queueName, CloudQueueMessage queueMessage)
        {
             GetQueue(queueName).DeleteMessage(queueMessage);
             return true;


        }

        public bool ClearMessages(string queueName)
        {
            GetQueue(queueName).Clear();
            return true;
           

        }
        #endregion
       

     #region Table Operations
        public void CreateTable(string tableName)
        {
         TableStorageType.CreateTable(tableName);
        
        }

        public void DeleteTable(string tableName)
        {
         TableStorageType.DeleteTable(tableName);

        }
        public IEnumerable<string> ListTables()
        {
         return TableStorageType.ListTables();

        }
     #endregion
         
    }
}
