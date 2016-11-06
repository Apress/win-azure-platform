using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using System.Configuration;
using System.Collections.Specialized;
using System.Security;
using System.IO;
using Microsoft.Win32;
using Microsoft.WindowsAzure.StorageClient.Protocol;
using System.Net;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.IO.Compression;

namespace ProAzureCommonLib
{

    public class WAStorageHelper
    {
        #region private fields

        private CloudStorageAccount _cloudStorageAccount;
        private StorageCredentials _credentials;
        private CloudTableClient _tableClient;
        private CloudBlobClient _blobClient;
        private CloudQueueClient _queueClient;
        private CloudBlobContainer _blobContainer;
        private string _containerName = "microsoftsilverlining";
        private const string STORAGE_ACCOUNT_CONNECTION_STRING = "StorageAccountConnectionString";

        #endregion

        public CloudStorageAccount CloudStorageAccount { get { return _cloudStorageAccount; } set { this._cloudStorageAccount = value; } }
        #region constructors

        /// <summary>
        /// Default constructor. 
        /// </summary>
        /// Loads the cloud storage account from the app.config or we.config file, and will override 
        public WAStorageHelper()
        {
            try
            {
                // first, check for role configuration
                bool useRoleConfig = bool.Parse(ConfigurationManager.AppSettings["UseRoleConfigurationSetting"]);
                if (useRoleConfig)
                {
                    InitFromRoleConfigSetting(ConfigurationManager.AppSettings["StorageAccountEndpointName"]);
                    return;
                }

                // check for dev storage
                bool useDevStorage = bool.Parse(ConfigurationManager.AppSettings["UseDevelopmentStorage"]);
                if (useDevStorage)
                {
                    this._cloudStorageAccount = CloudStorageAccount.DevelopmentStorageAccount;
                    return;
                }

                // Get account info for next 2 tests
                string accountName = ConfigurationManager.AppSettings["AccountName"];
                string accountKey = ConfigurationManager.AppSettings["AccountSharedKey"];

                // see if we are loading from custom endpoints
                bool useCustomEndpoints = bool.Parse(ConfigurationManager.AppSettings["UseCustomEndpoints"]);
                if (useCustomEndpoints)
                {
                    InitFromCustomEndpoints(accountName, accountKey);
                    return;
                }

                bool useHttps = bool.Parse(ConfigurationManager.AppSettings["UseHttps"]);
                Init(accountName, accountKey, useHttps);
            }
            catch (FormatException e) // bool.Parse failed, most likely due to incorrect config value
            {
             
                throw e;
            }

        }



        /// <summary>
        /// Overloaded constructor. Creates a StorageHelper object, with endpoints populated from the Azure ServiceConfiguration file.
        /// </summary>
        /// <param name="configString">The name of the configuration setting in the ServiceConfiguartion file.</param>
        public WAStorageHelper(string configString)
        {
            InitFromRoleConfigSetting(configString);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="storageAccount"></param>
        public WAStorageHelper(CloudStorageAccount storageAccount)
        {
            this._cloudStorageAccount = storageAccount;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountName"></param>
        /// <param name="accountKey"></param>
        /// <param name="useHttps"></param>
        public WAStorageHelper(string accountName, string accountKey, bool useHttps)
        {
            Init(accountName, accountKey, useHttps);
        }

        /// <summary>
        /// Overloaded constructor. Allows for creation of StorageHelper object with custom endpoints not defined in config files.
        /// </summary>
        /// <param name="accountName">Account name in Windows Azure portal.</param>
        /// <param name="accountKey">Account key.</param>
        /// <param name="blobEndpointURI"></param>
        /// <param name="queueEndpointURI"></param>
        /// <param name="tableEndpointURI"></param>
        public WAStorageHelper(string accountName, string accountKey, Uri blobEndpointURI, Uri queueEndpointURI, Uri tableEndpointURI)
        {
            StorageCredentialsAccountAndKey credentials = new StorageCredentialsAccountAndKey(accountName, accountKey);
            this._cloudStorageAccount = new CloudStorageAccount(credentials, blobEndpointURI, queueEndpointURI, tableEndpointURI);
        }

        /// <summary>
        /// returns WAStorageHelper. If the account name has devstoreaccount in it, it assumes to be a dev storage and looks for the
        /// StorageAccountConnectionString dtorage string in ServiceConfiguration.cscfg
        /// </summary>
        /// <param name="accountName"></param>
        /// <param name="accountKey"></param>
        /// <param name="useHttps"></param>
        /// <returns></returns>
        public static WAStorageHelper GetWAStorageHelper(string accountName, string accountKey, bool useHttps)
        {
            //if (accountName.IndexOf("devstoreaccount") > -1)
            //{
            //    return new WAStorageHelper("StorageAccountConnectionString");
            //}
            //else
            //{
            //    return new WAStorageHelper(accountName, accountKey, useHttps);
            //}

            return new WAStorageHelper(STORAGE_ACCOUNT_CONNECTION_STRING);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountName"></param>
        /// <param name="accountKey"></param>
        /// <param name="useHttps"></param>
        /// <returns></returns>
        public static CloudStorageAccount GetCloudStorageAccount(string accountName, string accountKey, bool useHttps)
        {
            if (accountName.IndexOf("devstoreaccount") > -1)
            {
                WAStorageHelper was = new WAStorageHelper(STORAGE_ACCOUNT_CONNECTION_STRING);
                return was.CloudStorageAccount;
            }
            else
            {
                WAStorageHelper was = new WAStorageHelper(accountName, accountKey, useHttps);
                return was.CloudStorageAccount;
            }

        }
        #endregion

        #region properties

        /// <summary>
        /// The Account information used to access storage.
        /// </summary>
        /// <remarks>Defaults to retrieving from DataConnectionString configuration settings if not set explicitly or in the constructor.</remarks>
        public CloudStorageAccount StorageAccountInfo
        {
            get { return this._cloudStorageAccount; }
            set { this._cloudStorageAccount = value; }
        }

        public CloudBlobContainer BlobContainer
        {
            get
            {
                if (this._blobContainer == null)
                {
                    this._blobContainer = GetBlobContainer(_containerName);
                }
                return this._blobContainer;
            }
            set { this._blobContainer = value; }
        }

        public CloudBlobClient BlobClient
        {
            get
            {
                // use a singleton pattern to create this so it's only loaded when necessary, and only created once.
                if (this._blobClient == null)
                    this._blobClient = this._cloudStorageAccount.CreateCloudBlobClient();
                return this._blobClient;
            }
            set { this._blobClient = value; }
        }

        public CloudQueueClient QueueClient
        {
            get
            {
                // use a singleton pattern to create this so it's only loaded when necessary, and only created once.
                if (this._queueClient == null)
                    this._queueClient = this._cloudStorageAccount.CreateCloudQueueClient();
                return this._queueClient;
            }

            set { this._queueClient = value; }
        }

        public CloudTableClient TableClient
        {
            get
            {
                // use a singleton pattern to create this so it's only loaded when necessary, and only created once.
                if (this._tableClient == null)
                    this._tableClient = this._cloudStorageAccount.CreateCloudTableClient();
                return this._tableClient;
            }
            set { this._tableClient = value; }
        }

        #endregion

        #region table methods

        public void CreateTable(string tableName)
        {
            TableClient.CreateTable(tableName);
        }

        public void DeleteTable(string tableName)
        {
            TableClient.DeleteTable(tableName);
        }
        public IEnumerable<string> ListTables()
        {
            return TableClient.ListTables();
        }

        #endregion

        #region blob methods

        #region container methods

        /// <summary>
        /// Return a Blob container.
        /// </summary>
        /// <returns>a CloudBlobContainer object.</returns>
        public CloudBlobContainer GetBlobContainer(string address)
        {
            CloudBlobContainer container = BlobClient.GetContainerReference(address);
            container.CreateIfNotExist();
            container.FetchAttributes();
            return container;
        }

        /// <summary>
        /// Return a list of BLOB containers.
        /// </summary>
        /// <returns></returns>
        /// 
        public IEnumerable<CloudBlobContainer> GetContainers()
        {
            return this.BlobClient.ListContainers();
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerName"></param>
        /// <returns></returns>
        public bool DoesContainerExist(string containerName)
        {
            CloudBlobContainer container = GetBlobContainer(containerName);
            return container.Exists();
        }

        public ResultSegment<CloudBlobContainer> GetContainerSegmented(string prefix, int maxResults, ResultContinuation continuationToken)
        {
            return BlobClient.ListContainersSegmented(prefix, ContainerListingDetails.All, maxResults, continuationToken);
        }

        public IList<CloudBlobContainer> GetContainers(string prefix)
        {
            return BlobClient.ListContainers(prefix, ContainerListingDetails.All).ToList<CloudBlobContainer>();
        }

        public bool CreateContainer(string containerName)
        {
            CloudBlobContainer container = GetBlobContainer(containerName);
            return container.CreateIfNotExist();
        }

        public bool CreateContainer(string containerName, BlobContainerPermissions permissions, NameValueCollection metadata)
        {

            CloudBlobContainer container = GetBlobContainer(containerName);
            bool result = container.CreateIfNotExist();
            if (result)
            {
                container.SetPermissions(permissions);
                container.Metadata.Add(metadata);
                container.SetMetadata();
            }
            return result;
        }

        public BlobContainerProperties GetContainerProperties(string containerName)
        {
            CloudBlobContainer container = GetBlobContainer(containerName);
            return container.Properties;
        }

        public void SetContainerMetadata(string containerName, NameValueCollection metadata)
        {
            CloudBlobContainer container = GetBlobContainer(containerName);
            container.CreateIfNotExist();
            container.Metadata.Clear();
            container.Metadata.Add(metadata);
            container.SetMetadata();
        }

        public BlobContainerPermissions GetContainerPermissions(string containerName)
        {
            CloudBlobContainer container = GetBlobContainer(containerName);
            return container.GetPermissions();
        }

        public void SetContainerPermissions(string containerName, BlobContainerPermissions permissions)
        {
            CloudBlobContainer container = GetBlobContainer(containerName);
            container.SetPermissions(permissions);
        }

        public void DeleteContainer(string containerName)
        {
            CloudBlobContainer container = GetBlobContainer(containerName);
            container.Delete();
        }

        public IEnumerable<string> ListBlobNames(string containerName, string prefix)
        {
            IEnumerable<IListBlobItem> blobItems = ListBlobs(containerName, prefix);
            IEnumerable<string> names = ParseBlobNames(blobItems);

            return names;
        }

        public IEnumerable<string> ListBlobNames(string containerName, string prefix, BlobRequestOptions options)
        {
            IEnumerable<IListBlobItem> blobItems = ListBlobs(containerName, prefix, options);
            IEnumerable<string> names = ParseBlobNames(blobItems);

            return names;
        }

        public static IEnumerable<string> ParseBlobNames(IEnumerable<IListBlobItem> blobItems)
        {
            var blobNames = from item in blobItems
                            select Path.GetFileName(item.Uri.LocalPath);
            //select item.Container.Uri.MakeRelativeUri(item.Uri).ToString(); // item.Uri.AbsoluteUri.Replace(item.Container.Uri.AbsoluteUri, string.Empty);

            return blobNames;
        }

        private IEnumerable<string> ListBlobNames(string containerName, string prefix, int maxResults)
        {
            if (maxResults == 0) maxResults = 100;
            ResultContinuation continuation = null;
            IEnumerable<string> names = null;

            do
            {
                ResultSegment<IListBlobItem> resultSegment = ListBlobs(containerName, prefix, maxResults, continuation);
                continuation = resultSegment.ContinuationToken;
                if (names == null)
                    names = ParseBlobNames(resultSegment.Results);
                else
                    names.Concat(ParseBlobNames(resultSegment.Results));
            }
            while (continuation != null);

            return names;
        }

        public IEnumerable<IListBlobItem> ListBlobs(string containerName, string prefix, int maxResults, ref ResultContinuation continuationToken)
        {
            ResultSegment<IListBlobItem> blobResultSegment = ListBlobs(containerName, prefix, maxResults, continuationToken);
            if (blobResultSegment != null && blobResultSegment.Results.Count() > 0)
            {
                continuationToken = blobResultSegment.ContinuationToken; // set so that next query can get next set of results
                return FilterBlobCollection(blobResultSegment.Results.AsParallel(), item => item.Container.Name == containerName);
            }

            return null;
        }

        public ResultSegment<IListBlobItem> ListBlobs(string containerName, string prefix, int maxResults, ResultContinuation continuationToken, BlobRequestOptions options)
        {
            return BlobClient.ListBlobsWithPrefixSegmented(string.Format("\\{0}\\{1}", containerName, prefix), maxResults, continuationToken, options);
        }

        /// <summary>
        /// Returns all blobs in the container
        /// </summary>
        /// <param name="containerName">The name of the container.</param>
        /// <param name="prefix">A prefix for filtering blobs. Use null to get all blobs.</param>
        /// <returns>A list of blobs found in the container matching the prefix.</returns>
        public IEnumerable<IListBlobItem> ListBlobs(string containerName, string prefix)
        {
            return ListBlobs(containerName, prefix, null);
        }

        /// <summary>
        /// Returns all blobs in the container.
        /// </summary>
        /// <param name="containerName">The name of the container.</param>
        /// <param name="prefix">A prefix for filtering blobs. Use null to get all blobs.</param>
        /// <param name="options">Options for the request.</param>
        /// <returns>A list of blobs found in the container matching the prefix.</returns>
        public IEnumerable<IListBlobItem> ListBlobs(string containerName, string prefix, BlobRequestOptions options)
        {
            string adjustedPrefix = string.Format("{0}/{1}", containerName, prefix);
            return BlobClient.ListBlobsWithPrefix(adjustedPrefix, options);
        }

        public ResultSegment<IListBlobItem> ListBlobs(string containerName, string prefix, int maxResults, ResultContinuation continuationToken)
        {
            CloudBlobContainer container = GetBlobContainer(containerName);
            string adjustedPrefix = string.Format("{0}/{1}", containerName, prefix);
            return BlobClient.ListBlobsWithPrefixSegmented(adjustedPrefix, maxResults, continuationToken);
        }

        public IEnumerable<IListBlobItem> FilterBlobCollection(IEnumerable<IListBlobItem> blobCollection, Func<IListBlobItem, bool> predicate)
        {
            return blobCollection.Where<IListBlobItem>(predicate);
        }

        #endregion

        #region blob operations

        #region Byte[] put

        public void PutBlob(string containerName, string blobName, Byte[] contents)
        {
            CloudBlob blob = this.GetBlob(containerName, blobName);
            PutBlob(blob, contents);
        }

        public void PutBlob(string containerName, string blobName, Byte[] contents, bool overwrite)
        {
            CloudBlob blob = this.GetBlob(containerName, blobName);
            BlobRequestOptions options = CreateOverwriteOption(blob, overwrite);
            PutBlob(blob, contents, overwrite);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerName"></param>
        /// <param name="blobName"></param>
        /// <param name="contents"></param>
        /// <param name="fileName">File name is used only for setting content type. The file is not uploaded from file system</param>
        /// <param name="overwrite"></param>
        /// <param name="metadata"></param>
        public void PutBlob(string containerName, string blobName, Byte[] contents, string fileName, bool overwrite, NameValueCollection metadata)
        {
            CloudBlob blob = this.GetBlob(containerName, blobName);
            blob.Metadata.Add(metadata);
            BlobRequestOptions options = CreateOverwriteOption(blob, overwrite);
            PutBlob(blob, contents, fileName, overwrite);
        }

        public void PutBlob(CloudBlob blob, Byte[] contents)
        {
            blob.UploadByteArray(contents);
        }

        public void PutBlob(CloudBlob blob, Byte[] contents, bool overwrite)
        {
            BlobRequestOptions options = CreateOverwriteOption(blob, overwrite);
            blob.UploadByteArray(contents, options);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="blob"></param>
        /// <param name="contents"></param>
        /// <param name="fileName">File name is used only for setting content type. The file is not uploaded from file system</param>
        /// <param name="overwrite"></param>
        public void PutBlob(CloudBlob blob, Byte[] contents, string fileName, bool overwrite)
        {
            BlobRequestOptions options = CreateOverwriteOption(blob, overwrite);
            blob.Properties.ContentType = WAStorageHelper.GetContentTypeFromExtension(Path.GetExtension(fileName));
            blob.UploadByteArray(contents, options);
        }
        #endregion

        #region File put

        public void PutBlobFromFile(string containerName, string blobName, string fileName)
        {
            CloudBlob blob = this.GetBlob(containerName, blobName);
            PutBlobFromFile(blob, fileName);
        }

        public void PutBlobFromFile(string containerName, string blobName, string fileName, NameValueCollection metadata)
        {
            CloudBlob blob = this.GetBlob(containerName, blobName);
            blob.Metadata.Add(metadata);
            PutBlobFromFile(blob, fileName);
        }

        public void PutBlobFromFile(CloudBlob blob, string fileName)
        {
            blob.Properties.ContentType = WAStorageHelper.GetContentTypeFromExtension(Path.GetExtension(fileName));
            blob.UploadFile(fileName);
        }

        public void PutBlobFromFile(string containerName, string blobName, string fileName, bool overwrite)
        {
            CloudBlob blob = this.GetBlob(containerName, blobName);
            PutBlobFromFile(blob, fileName, overwrite);
        }

        public void PutBlobFromFile(string containerName, string blobName, string fileName, bool overwrite, bool zip, NameValueCollection metadata)
        {
            CloudBlob blob = this.GetBlob(containerName, blobName);
            blob.Metadata.Add(metadata);
            if (zip)
                UploadCompressedFileToBlob(blob, fileName, overwrite);
            else
                PutBlobFromFile(blob, fileName, overwrite);
        }

        public void PutBlobFromFile(CloudBlob blob, string fileName, bool overwrite)
        {
            BlobRequestOptions options = CreateOverwriteOption(blob, overwrite);
            blob.Properties.ContentType = WAStorageHelper.GetContentTypeFromExtension(Path.GetExtension(fileName));
            blob.UploadFile(fileName, options);
        }

        #endregion

        #region Stream put

        public void PutBlob(string containerName, string blobName, Stream contents)
        {
            CloudBlob blob = this.GetBlob(containerName, blobName);
            PutBlob(blob, contents);
        }

        public void PutBlob(string containerName, string blobName, Stream contents, bool overwrite)
        {
            CloudBlob blob = this.GetBlob(containerName, blobName);
            BlobRequestOptions options = CreateOverwriteOption(blob, overwrite);
            PutBlob(blob, contents, overwrite);
        }

        public void PutBlob(CloudBlob blob, Stream contents)
        {
            blob.UploadFromStream(contents);
        }

        public void PutBlob(CloudBlob blob, Stream contents, bool overwrite)
        {
            BlobRequestOptions options = CreateOverwriteOption(blob, overwrite);
            blob.UploadFromStream(contents, options);
        }

        #endregion

        #region string put

        public void PutBlob(string containerName, string blobName, string contents)
        {
            CloudBlob blob = this.GetBlob(containerName, blobName);
            PutBlob(blob, contents);
        }

        public void PutBlob(string containerName, string blobName, string contents, bool overwrite)
        {
            CloudBlob blob = this.GetBlob(containerName, blobName);
            BlobRequestOptions options = CreateOverwriteOption(blob, overwrite);
            PutBlob(blob, contents, overwrite);
        }

        public void PutBlob(CloudBlob blob, string contents)
        {
            blob.UploadText(contents);
        }

        public void PutBlob(CloudBlob blob, string contents, bool overwrite)
        {
            BlobRequestOptions options = CreateOverwriteOption(blob, overwrite);
            blob.UploadText(contents, Encoding.UTF8, options); // UTF 8 is consistent with single parameter overloaded method
        }

        #endregion

        public CloudBlob CreateBlob(string containerName, string blobName)
        {
            CloudBlobContainer container = GetBlobContainer(containerName);
            CloudBlob blob = container.GetBlobReference(blobName);
            return blob;
        }

        public CloudBlob CreateBlob(string containerName, string blobName, byte[] contents)
        {
            CloudBlob blob = CreateBlob(containerName, blobName);
            PutBlob(blob, contents);
            return blob;
        }

        public CloudBlob CreateBlob(string containerName, string blobName, string contents)
        {
            CloudBlob blob = CreateBlob(containerName, blobName);
            PutBlob(blob, contents);
            return blob;
        }

        public CloudBlob CreateBlob(string containerName, string blobName, Stream contents)
        {
            CloudBlob blob = CreateBlob(containerName, blobName);
            PutBlob(blob, contents);
            return blob;
        }

        public CloudBlob CreateBlobFileContent(string containerName, string blobName, string fileName)
        {
            CloudBlob blob = CreateBlob(containerName, fileName);
            PutBlobFromFile(blob, fileName, false); // since we're creating, we shouldn't be overwriting anything
            return blob;
        }

        public void DeleteBlob(string containerName, string blobName)
        {
            CloudBlobContainer container = GetBlobContainer(containerName);
            CloudBlob blob = container.GetBlobReference(blobName);
            blob.DeleteIfExists();
        }

        public void DeleteBlobs(IEnumerable<CloudBlob> blobs)
        {
            Parallel.ForEach<CloudBlob>(blobs, blob => { blob.DeleteIfExists(); });
        }

        /// <summary>
        /// Deletes the blob if it has not been modified since the blob was retrieved.
        /// </summary>
        /// <param name="blob">The blob to be deleted.</param>
        /// <returns>True if the blob was deleted, false otherwise.</returns>
        public bool DeleteBlobIfNotModified(CloudBlob blob)
        {
            // This tested slower - probably due to exception overhead
            try
            {
                BlobRequestOptions options = new BlobRequestOptions { AccessCondition = AccessCondition.IfNotModifiedSince(blob.Properties.LastModifiedUtc) };
                blob.Delete(options);
                return true;
            }
            catch (StorageClientException ex)
            {
                if (ex.ErrorCode == StorageErrorCode.ConditionFailed)
                    return false; // blob has been modified since downloading, and was not deleted
                throw ex; // something else went wrong
            }
        }

        public bool DoesBlobExist(string containerName, string blobName)
        {
            CloudBlob blob = GetBlob(containerName, blobName);
            return blob.Exists();
        }

        #region Get Contents

        public byte[] GetBlobContentsAsByte(string containerName, string blobName)
        {
            CloudBlob blob = GetBlob(containerName, blobName);
            return blob.DownloadByteArray();
        }

        public byte[] GetBlobContentsAsByte(string containerName, string blobName, BlobRequestOptions options)
        {
            CloudBlob blob = GetBlob(containerName, blobName);
            return blob.DownloadByteArray(options);
        }

        public string GetBlobContentsAsText(string containerName, string blobName)
        {
            CloudBlob blob = GetBlob(containerName, blobName);
            return blob.DownloadText();
        }

        public string GetBlobContentsAsText(string containerName, string blobName, BlobRequestOptions options)
        {
            CloudBlob blob = GetBlob(containerName, blobName);
            return blob.DownloadText(options);
        }

        public void GetBlobContentsAsFile(string containerName, string blobName, string fileName)
        {
            CloudBlob blob = GetBlob(containerName, blobName);
            blob.DownloadToFile(fileName);
        }
        public void GetBlobAsFileWithCleanup(string containerName, string blobName, string destinationFilePath)
        {
            if (File.Exists(destinationFilePath))
            {
                try
                {

                    File.Delete(destinationFilePath);
                }
                catch (Exception ex)
                {
                    Trace.WriteLine("Exception deleting old file " + ex.Message, "Error");

                }

            }
            if (!File.Exists(destinationFilePath))
            {
                GetBlobContentsAsFile(containerName, blobName, destinationFilePath);


            }
            //else
            //{
            //    LogInfo("Assembly already exists and is loaded. Develop an Unload Assembly function to unload the assembly when done so that it is not locked.");
            //    LogInfo("Loading and Unloading Assemblies: http://people.oregonstate.edu/~reeset/blog/archives/466");

            //}


        }
        public void GetBlobContentsAsFile(string containerName, string blobName, string fileName, BlobRequestOptions options)
        {
            CloudBlob blob = GetBlob(containerName, blobName);
            blob.FetchAttributes();
            blob.DownloadToFile(fileName, options);
        }

        public void GetBlobContentsAsFileIfNotModified(string containerName, string blobName, string fileName)
        {
            CloudBlob blob = GetBlob(containerName, blobName);
            BlobRequestOptions options = CreateIfNotModifiedOption(blob);
            blob.FetchAttributes();
            try
            {
                blob.DownloadToFile(fileName, options);
            }
            catch (StorageClientException ex)
            {
                if (ex.ErrorCode == StorageErrorCode.BadRequest)
                    throw new InvalidOperationException(string.Format("{0} was not downloaded, since the blob has been modified.", blobName));
                else
                    throw ex;
            }
        }

        public void GetBlobContentsAsFileIfModified(string containerName, string blobName, string fileName)
        {
            CloudBlob blob = GetBlob(containerName, blobName);
            BlobRequestOptions options = CreateIfModifiedOption(blob);
            blob.FetchAttributes();
            try
            {
                blob.DownloadToFile(fileName, options);
            }
            catch (StorageClientException ex)
            {
                if (ex.ErrorCode == StorageErrorCode.BadRequest)
                    throw new InvalidOperationException(string.Format("{0} was not downloaded, since the blob has not been modified.", blobName));
                else
                    throw ex;
            }
        }

        public void GetBlobContentsAsStream(string containerName, string blobName, Stream stream)
        {
            CloudBlob blob = GetBlob(containerName, blobName);
            blob.DownloadToStream(stream);
        }
        public void GetBlobContentsAsStream(string containerName, string blobName, Stream stream, BlobRequestOptions options)
        {
            CloudBlob blob = GetBlob(containerName, blobName);
            blob.DownloadToStream(stream, options);
        }

        public string GetSharedAccessSignatureBlobUrl(CloudBlob cb, SharedAccessPermissions perm, int minutesValid)
        {
            var readPolicy = new SharedAccessPolicy()
            {
                Permissions = perm,
                SharedAccessExpiryTime = DateTime.UtcNow + TimeSpan.FromMinutes(minutesValid)
            };
            return cb.Uri.AbsoluteUri + cb.GetSharedAccessSignature(readPolicy);

        }

        public string GetSharedAccessSignatureForPackageContainer(string containerName, string policyName, int expirationHours)
        {

            //Get a reference to the container for which shared access signature will be created.
            CloudBlobContainer container = BlobClient.GetContainerReference(containerName);
            container.CreateIfNotExist();

            //Create a permission policy, consisting of a container-level access policy and a public access setting, and store it on the container. 
            BlobContainerPermissions blobPermissions = new BlobContainerPermissions();
            //The container-level access policy provides read/write access to the container for 10 hours.
            blobPermissions.SharedAccessPolicies.Add(policyName, new SharedAccessPolicy()
            {
                SharedAccessStartTime = DateTime.UtcNow,
                SharedAccessExpiryTime = DateTime.Now.AddHours(expirationHours),
                Permissions = SharedAccessPermissions.Write | SharedAccessPermissions.Read
            });

            //The public access setting explicitly specifies that the container is private, so that it can't be accessed anonymously.
            blobPermissions.PublicAccess = BlobContainerPublicAccessType.Off;

            //Set the permission policy on the container.
            container.SetPermissions(blobPermissions);

            //Get the shared access signature to share with clients.
            //Note that this call passes in an empty access policy, so that the shared access signature will use the 
            //'mypolicy' access policy that's defined on the container.
            return container.GetSharedAccessSignature(new SharedAccessPolicy(), policyName);
        }

        public string GetSharedAccessSignatureForPackageContainer(string containerName, string policyName, int expirationHours, SharedAccessPermissions perm)
        {
           
            //Get a reference to the container for which shared access signature will be created.
            CloudBlobContainer container = BlobClient.GetContainerReference(containerName);
            container.CreateIfNotExist();

            //Create a permission policy, consisting of a container-level access policy and a public access setting, and store it on the container. 
            BlobContainerPermissions blobPermissions = new BlobContainerPermissions();
            //The container-level access policy provides read/write access to the container for 10 hours.
            blobPermissions.SharedAccessPolicies.Add(policyName, new SharedAccessPolicy()
            {
                SharedAccessStartTime = DateTime.UtcNow,
                SharedAccessExpiryTime = DateTime.Now.AddHours(expirationHours),
                Permissions = perm
            });


            //The public access setting explicitly specifies that the container is private, so that it can't be accessed anonymously.
            blobPermissions.PublicAccess = BlobContainerPublicAccessType.Off;

            //Set the permission policy on the container.
            container.SetPermissions(blobPermissions);

            //Get the shared access signature to share with clients.
            //Note that this call passes in an empty access policy, so that the shared access signature will use the 
            //'mypolicy' access policy that's defined on the container.
            return container.GetSharedAccessSignature(new SharedAccessPolicy(), policyName);
        }

        public string GetSharedAccessSignatureBlobUrl(string blobAbsoluteUri, SharedAccessPermissions perm, int minutesValid)
        {
            CloudBlob blob = GetBlob(blobAbsoluteUri);
            return GetSharedAccessSignatureBlobUrl(blob, perm, minutesValid);
        }

        public string GetSharedAccessSignatureBlobUrl(string containerName, string blobName, SharedAccessPermissions perm, int minutesValid)
        {
            CloudBlob blob = GetBlob(containerName, blobName);
            return GetSharedAccessSignatureBlobUrl(blob, perm, minutesValid);
        }
        #endregion

        /// <summary>
        /// Returns a reference to the blob in storage.
        /// </summary>
        /// <param name="containerName">The name of the container for the blob.</param>
        /// <param name="blobName">The name of the blob.</param>
        /// <returns>A CloudBlob object.</returns>
        public CloudBlob GetBlob(string containerName, string blobName)
        {
            CloudBlobContainer container = GetBlobContainer(containerName);
            CloudBlob blob = container.GetBlobReference(blobName);
            return blob;
        }
       

        /// <summary>
        /// 
        /// </summary>
        /// <param name="absoluteBlobUri"></param>
        /// <returns></returns>
        public CloudBlob GetBlob(string absoluteBlobUri)
        {
            return new CloudBlob(absoluteBlobUri);

        }

        public CloudBlob GetBlobIfModified(CloudBlob blob)
        {
            CloudBlob compareBlob = blob.Container.GetBlobReference(blob.Uri.ToString());
            blob.FetchAttributes();
            if (DateTime.Compare(blob.Properties.LastModifiedUtc, compareBlob.Properties.LastModifiedUtc) < 0)
            {
                return compareBlob;
            }
            return blob;
        }

        public bool UpdateBlobIfNotModified(CloudBlob blob, byte[] contents)
        {
            CloudBlob compareBlob = blob.Container.GetBlobReference(blob.Uri.ToString()); // get a new reference to the latest version in storage
            if (DateTime.Compare(blob.Properties.LastModifiedUtc, compareBlob.Properties.LastModifiedUtc) > 0) // compare last modified dates
            {
                blob.UploadByteArray(contents);
                return true;
            }
            return false; // blob has been modified in storage, and was not updated
        }

        public void UpdateBlobMetadata(string containerName, string blobName, NameValueCollection metadata)
        {
            CloudBlob blob = GetBlob(containerName, blobName);
            UpdateBlobMetadata(blob, metadata, null);
        }

        public void UpdateBlobMetadata(CloudBlob blob, NameValueCollection metadata)
        {
            UpdateBlobMetadata(blob, metadata, null);
        }

        public void UpdateBlobMetadata(CloudBlob blob, NameValueCollection metadata, BlobRequestOptions options)
        {
            blob.Metadata.Clear();
            blob.Metadata.Add(metadata);
            UpdateBlobMetadata(blob, options);
        }

        public void UpdateBlobMetadata(CloudBlob blob, BlobRequestOptions options)
        {
            blob.SetMetadata(options);
        }

        public bool UpdateBlobMetadataIfNotModified(CloudBlob blob, NameValueCollection metadata)
        {
            try
            {
                UpdateBlobMetadata(blob, metadata, CreateIfNotModifiedOption(blob));
                return true;
            }
            catch (StorageClientException ex)
            {
                if (ex.ErrorCode == StorageErrorCode.ConditionFailed)
                {
                    return false; // blob has been modified, and was not updated
                }
                throw ex;
            }
        }

        public bool UpdateBlobMetadataIfNotModified(CloudBlob blob)
        {
            try
            {
                UpdateBlobMetadata(blob, CreateIfNotModifiedOption(blob));
                return true;
            }
            catch (StorageClientException ex)
            {
                if (ex.ErrorCode == StorageErrorCode.ConditionFailed)
                {
                    return false; // blob has been modified, and was not updated
                }
                throw ex;
            }
        }

        /// <summary>
        /// Copies the blob to a new container.
        /// </summary>
        /// <param name="blob"></param>
        /// <param name="destinationContainerName"></param>
        /// <param name="destinationBlobName"></param>
        /// <returns></returns>
        public CloudBlob CopyBlob(CloudBlob blob, string destinationContainerName, string destinationBlobName)
        {
            CloudBlobContainer copyContainer = GetBlobContainer(destinationContainerName);
            CloudBlob copyBlob = copyContainer.GetBlobReference(destinationBlobName);
            copyBlob.CopyFromBlob(blob);
            return copyBlob;
        }

        public CloudBlob CopyBlob(CloudBlob blob, string destinationContainerName, string destinationBlobName, NameValueCollection additionalMetadata)
        {
            CloudBlobContainer copyContainer = GetBlobContainer(destinationContainerName);
            CloudBlob copyBlob = copyContainer.GetBlobReference(destinationBlobName);
            copyBlob.CopyFromBlob(blob);
            // we have to do this for now, there is a bug in the SDK where the additional metadata is not copied.
            copyBlob.Metadata.Add(additionalMetadata);
            copyBlob.SetMetadata();
            return copyBlob;
        }

        public CloudBlob CopyBlob(CloudBlob blob, string destinationContainerName, string destinationBlobName, string versionString)
        {
            CloudBlob copyBlob = CopyBlob(blob, destinationContainerName, destinationBlobName);
            SetBlobVersionMetadata(copyBlob, versionString);

            return copyBlob;
        }

        public CloudBlob CopyBlob(string sourceContainerName, string blobName, string destinationContainerName, string destinationBlobName, NameValueCollection additionalMetadata)
        {
            CloudBlob blob = GetBlob(sourceContainerName, blobName);
            CloudBlob copyBlob = CopyBlob(blob, destinationContainerName, destinationBlobName, additionalMetadata);
            return copyBlob;
        }



        #endregion

        #endregion

        #region queue methods

        public IEnumerable<CloudQueue> ListQueues(string prefix)
        {
            if (string.IsNullOrEmpty(prefix))
            {
                return this.QueueClient.ListQueues();
            }
            else
            {
                return this.QueueClient.ListQueues(prefix);
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
            CreateQueue(queueName);

            return QueueClient.GetQueueReference(queueName);

        }

        public QueueListener GetQueueListener(string queueName)
        {
            CloudQueue queue = GetQueue(queueName);
            if (!queue.Exists())
            {
                CreateQueue(queueName);
            }

            return new QueueListener(queue);
        }
        public void SetQueueMetadata(string queueName, NameValueCollection metadata)
        {
            CloudQueue queue = GetQueue(queueName);
            queue.Metadata.Clear();
            queue.Metadata.Add(metadata);
            queue.SetMetadata();
        }

        public bool CreateQueue(string queueName)
        {
            CloudQueue q = QueueClient.GetQueueReference(queueName);
            return q.CreateIfNotExist();
        }

        public void DeleteQueue(string queueName)
        {
            CloudQueue q = QueueClient.GetQueueReference(queueName);
            q.Delete();
        }

        public void AddMessage(string queueName, CloudQueueMessage queueMessage)
        {
            CloudQueue q = QueueClient.GetQueueReference(queueName);
            q.AddMessage(queueMessage);
        }

        public void AddMessage(string queueName, CloudQueueMessage queueMessage, int timeToLiveInSecs)
        {
            CloudQueue q = QueueClient.GetQueueReference(queueName);
            q.AddMessage(queueMessage, new TimeSpan(0, 0, timeToLiveInSecs));
        }

        public void AddMessage(string queueName, string messageBody, int timeToLiveInSecs)
        {
            CloudQueue q = QueueClient.GetQueueReference(queueName);
            CloudQueueMessage queueMessage = new CloudQueueMessage(messageBody);
            q.AddMessage(queueMessage, new TimeSpan(0, 0, timeToLiveInSecs));
        }

        public IEnumerable<CloudQueueMessage> GetMessages(string queueName, int numberofMessages, int visibilityTimeoutInSecs)
        {
            CloudQueue q = QueueClient.GetQueueReference(queueName);
            return q.GetMessages(numberofMessages, new TimeSpan(0, 0, visibilityTimeoutInSecs));
        }

        public IEnumerable<CloudQueueMessage> GetMessages(string queueName, int numberofMessages, TimeSpan timeout)
        {
            CloudQueue q = QueueClient.GetQueueReference(queueName);
            return q.GetMessages(numberofMessages, timeout);
        }


        public IEnumerable<CloudQueueMessage> PeekMessages(string queueName, int numberofMessages)
        {
            CloudQueue q = QueueClient.GetQueueReference(queueName);
            return q.PeekMessages(numberofMessages);
        }

        public CloudQueueMessage PeekMessage(string queueName)
        {
            CloudQueue q = QueueClient.GetQueueReference(queueName);
            return q.PeekMessage();
        }

        public void DeleteMessage(string queueName, CloudQueueMessage queueMessage)
        {
            CloudQueue q = QueueClient.GetQueueReference(queueName);
            q.DeleteMessage(queueMessage);
        }

        public void DeleteMessages(string queueName, IEnumerable<CloudQueueMessage> queueMessages)
        {
            CloudQueue q = QueueClient.GetQueueReference(queueName);
            Parallel.ForEach<CloudQueueMessage>(queueMessages, message => { q.DeleteMessage(message); });
        }

        public void ClearMessages(string queueName)
        {
            CloudQueue q = QueueClient.GetQueueReference(queueName);
            q.Clear();

          
        }

        #endregion

        #region static methods

        /// <summary>
        /// Gets the Storage Helper object based on config data.
        /// It reads the following from the configuration file
        /// </summary>
        /// <returns></returns>
        public static WAStorageHelper GetStorageHelper()
        {
            return new WAStorageHelper();
        }

        public static WAStorageHelper GetStorageHelper(string accountName, string accountKey, bool useHttps)
        {
            return new WAStorageHelper(accountName, accountKey, useHttps);
        }

        public static WAStorageHelper GetStorageHelper(CloudStorageAccount account)
        {
            return new WAStorageHelper(account);
        }

        public static void GetBlobContentsAsFile(string containerName, string blobName, string fileName, WAStorageHelper helper)
        {
            helper.GetBlobContentsAsFile(containerName, blobName, fileName);
        }

        public static CloudBlob GetBlob(string containerName, string blobName, WAStorageHelper storageHelper)
        {
            return storageHelper.GetBlob(containerName, blobName);
        }

        public static void SetBlobVersionMetadata(CloudBlob blob, string versionString)
        {
            if (blob.Metadata.AllKeys.Contains("version"))
            {
                blob.Metadata["version"] = versionString;
            }
            else
            {
                blob.Metadata.Add("version", versionString);
            }
            blob.SetMetadata();
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

        public static MemoryStream GZipCompressedBlobContents(string fileName)
        {
            var compressedData = new MemoryStream();

            using (var gzipStream = new GZipStream(compressedData,
                CompressionMode.Compress))
            {
                byte[] fileBytes = File.ReadAllBytes(fileName);
                gzipStream.Write(fileBytes, 0,
                    fileBytes.Length);
            }

            return compressedData;
        }

        public static void UploadCompressedFileToBlob(CloudBlob blob, string filename, bool overwrite)
        {
            BlobRequestOptions options = CreateOverwriteOption(blob, overwrite);
            MemoryStream compressedData = GZipCompressedBlobContents(filename);
            blob.UploadFromStream(compressedData, options);
        }

        #endregion

        #region private helper methods

        private void Init(string accountName, string accountKey, bool useHttps)
        {
            StorageCredentialsAccountAndKey credentials = new StorageCredentialsAccountAndKey(accountName, accountKey);
            this._cloudStorageAccount = new CloudStorageAccount(credentials, useHttps);
        }

        private void InitFromCustomEndpoints(string accountName, string accountKey)
        {

            Uri blobStorageEndPoint = new Uri(ConfigurationManager.AppSettings["BlobStorageCustomEndpoint"]);
            Uri queueStorageEndpoint = new Uri(ConfigurationManager.AppSettings["QueueStorageCustomEndpoint"]);
            Uri tableStorageEndpoint = new Uri(ConfigurationManager.AppSettings["TableStorageCustomEndpoint"]);
            StorageCredentialsAccountAndKey credentials = new StorageCredentialsAccountAndKey(accountName, accountKey);
            this._cloudStorageAccount = new CloudStorageAccount(credentials, blobStorageEndPoint, queueStorageEndpoint, tableStorageEndpoint);
        }

        private void InitFromRoleConfigSetting(string configString)
        {
            CloudStorageAccount.SetConfigurationSettingPublisher((configName, configSetter) =>
            {
                configSetter(RoleEnvironment.GetConfigurationSettingValue(configName));
            });
            this._cloudStorageAccount = CloudStorageAccount.FromConfigurationSetting(configString);
            //if (RoleEnvironment.IsAvailable)
            //    this._cloudStorageAccount = CloudStorageAccount.FromConfigurationSetting(configString);
            //else
            //    throw new InvalidOperationException("CloudStorageAccount cannot be created from configuration setting in ServiceConfiguration. Component is not running in an Azure environment.");
        }

        #endregion

        #region private static helper methods

        private static BlobRequestOptions CreateOverwriteOption(CloudBlob blob, bool overwrite)
        {
            BlobRequestOptions options = new BlobRequestOptions();
            if (!overwrite)
                options.AccessCondition = AccessCondition.IfNoneMatch(blob.Properties.ETag);
            return options;
        }

        private static BlobRequestOptions CreateIfModifiedOption(CloudBlob blob)
        {
            return new BlobRequestOptions { AccessCondition = AccessCondition.IfModifiedSince(blob.Properties.LastModifiedUtc) };
        }

        private static BlobRequestOptions CreateIfNotModifiedOption(CloudBlob blob)
        {
            return new BlobRequestOptions { AccessCondition = AccessCondition.IfNotModifiedSince(blob.Properties.LastModifiedUtc) };
        }

       



        public static void SendFailedJobMessage(WAStorageHelper storageHelper, string corrId, string queueNameConfigName, string poolId, string machineName)
        {
            string queueName = RoleEnvironment.GetConfigurationSettingValue(queueNameConfigName);
            // CheckConnection();

            storageHelper.CreateQueue(queueName);

            string messageBody = String.Format("{0}:{1}:{2}", poolId, corrId, machineName);
            int ttlsecs = 3600;

            storageHelper.AddMessage(queueName, messageBody, ttlsecs);

            Trace.WriteLine(String.Format("Message {0} sent successfully to queue {1}", messageBody, queueName), "Information");



        }
        #endregion

        #region Async Methods

      

        public IEnumerable<CloudBlobContainer> ListContainersInSegmentsAsynchronously()
        {

            ResultSegment<CloudBlobContainer> resultSegment = null;
            IAsyncResult asynResult = null;

            using (System.Threading.ManualResetEvent evt = new System.Threading.ManualResetEvent(false))
            {
                //Begin the operation to return the first segment of 10 containers in the account.
                asynResult = BlobClient.BeginListContainersSegmented(

                     result =>
                     {
                         CloudBlobClient blobClient = (CloudBlobClient)result.AsyncState;
                         resultSegment = blobClient.EndListContainersSegmented(result);
                         evt.Set();
                      
                     }

                     , BlobClient);


                evt.WaitOne();

            }
          //  asynResult.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(10));

            return resultSegment != null &&
                  resultSegment.Results.Count<CloudBlobContainer>() > 0 ?
                  resultSegment.Results : null;


        }

     

       
        public IEnumerable<CloudQueue> ListQueuesAsync(string prefix)
        {
           
            IAsyncResult asynResult = null;
            ResultSegment<CloudQueue> qSegment = null;
            try
            {
                using (System.Threading.ManualResetEvent evt = new System.Threading.ManualResetEvent(false))
                {

                    asynResult = QueueClient.BeginListQueuesSegmented(prefix, new AsyncCallback(result =>
                    {
                        var qc = result.AsyncState as CloudQueueClient;
                        qSegment = qc.EndListQueuesSegmented(result);
                        evt.Set();

                    }

                        ), QueueClient);

                    evt.WaitOne();

                }

             

          
                return qSegment != null &&
                   qSegment.Results.Count<CloudQueue>() > 0 ?
                   qSegment.Results : null;

               
            }
            catch (StorageClientException )
            {
               throw;
            }
        }

        public void AddMessageAsync(string queueName, CloudQueueMessage queueMessage, int ttlsecs)
        {
            CloudQueue q = QueueClient.GetQueueReference(queueName);

            using (System.Threading.ManualResetEvent evt = new System.Threading.ManualResetEvent(false))
            {
                q.BeginAddMessage(queueMessage, TimeSpan.FromSeconds(ttlsecs), new AsyncCallback(result =>
                {
                    var qc = result.AsyncState as CloudQueue;
                    qc.EndAddMessage(result);
                    evt.Set();

                }

                            ), q);

                evt.WaitOne();

            }
        }

        public IEnumerable<CloudQueueMessage> GetMessagesAsync(string queueName, int numberofMessages, int visibilityTimeoutInSecs)
        {
            CloudQueue q = QueueClient.GetQueueReference(queueName);
            IEnumerable<CloudQueueMessage> ret = null;

            using (System.Threading.ManualResetEvent evt = new System.Threading.ManualResetEvent(false))
            {
                q.BeginGetMessages(numberofMessages, TimeSpan.FromSeconds(visibilityTimeoutInSecs), new AsyncCallback(result =>
                {
                    var qc = result.AsyncState as CloudQueue;
                    ret = qc.EndGetMessages(result);
                    evt.Set();

                }

                                ), q);

                evt.WaitOne();
            }

            return ret;

        }
        #endregion
    }


  
}
