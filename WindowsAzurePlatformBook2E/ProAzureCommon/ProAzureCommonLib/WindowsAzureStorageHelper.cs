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
using Microsoft.Samples.ServiceHosting.StorageClient;
using System.Collections.Specialized;
using System.Security;
using Microsoft.Win32;
using System.IO.Compression;

using System.Reflection;
using System.Diagnostics;

namespace ProAzureCommonLib
{
    public class WindowsAzureStorageHelper
    {
        #region private



        #region Blob

        private const string CONTAINER_NAME = "proazuretestcontainer";

        #endregion



        #endregion

        #region Properties
        public StorageAccountInfo StorageAccountInfo
        {
            get;
            set;
        }
        public BlobContainer Container
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
        public BlobStorage BlobStorageType
        {
            get;
            set;
        }

        public QueueStorage QueueStorageType
        {
            get;
            set;
        }

        public TableStorage TableStorageType
        { get; set; }


        #endregion
        #region Constructors
        public WindowsAzureStorageHelper()
        {
            ContainerName = CONTAINER_NAME;
            // Open blob storage.

            StorageAccountInfo = StorageAccountInfo.GetDefaultBlobStorageAccountFromConfiguration();
            BlobStorageType = BlobStorage.Create(StorageAccountInfo);
            BlobStorageType.RetryPolicy = RetryPolicies.RetryN(2, TimeSpan.FromMilliseconds(100));

            // Open queue storage.

            StorageAccountInfo queueAccount = StorageAccountInfo.GetDefaultQueueStorageAccountFromConfiguration();
            QueueStorageType = QueueStorage.Create(queueAccount);
            QueueStorageType = QueueStorage.Create(queueAccount);
            QueueStorageType.RetryPolicy = RetryPolicies.RetryN(2, TimeSpan.FromMilliseconds(100));

            // Open table storage.

            StorageAccountInfo tableAccount = StorageAccountInfo.GetDefaultTableStorageAccountFromConfiguration();
            TableStorageType = TableStorage.Create(tableAccount);
            TableStorageType = TableStorage.Create(tableAccount);
            TableStorageType.RetryPolicy = RetryPolicies.RetryN(2, TimeSpan.FromMilliseconds(100));
        }
        public WindowsAzureStorageHelper(string accountName, string accountKey, bool isLocal, string blobEndpointURI, string queueEndpointURI, string tableEndpointURI)
        {

            ContainerName = CONTAINER_NAME;

            StorageAccountInfo = new StorageAccountInfo(new Uri(blobEndpointURI), isLocal, accountName, accountKey);

            BlobStorageType = BlobStorage.Create(StorageAccountInfo);
            BlobStorageType.RetryPolicy = RetryPolicies.RetryN(2, TimeSpan.FromMilliseconds(100));
            Container = BlobStorageType.GetBlobContainer(ContainerName);

            //Create the container if it does not exist.
            Container.CreateContainer(ContainerMetaData, ContainerAccessControl.Private);

            // Open queue storage.

            //StorageAccountInfo qaccount = StorageAccountInfo.GetDefaultQueueStorageAccountFromConfiguration();

            StorageAccountInfo qaccount = new StorageAccountInfo(new Uri(queueEndpointURI), isLocal, accountName, accountKey);

            QueueStorageType = QueueStorage.Create(qaccount);
            QueueStorageType = QueueStorage.Create(qaccount);
            QueueStorageType.RetryPolicy = RetryPolicies.RetryN(2, TimeSpan.FromMilliseconds(100));

            // Open table storage.

            //StorageAccountInfo taccount = StorageAccountInfo.GetDefaultTableStorageAccountFromConfiguration();
            StorageAccountInfo taccount = new StorageAccountInfo(new Uri(tableEndpointURI), isLocal, accountName, accountKey);

            TableStorageType = TableStorage.Create(taccount);
            TableStorageType = TableStorage.Create(taccount);
            TableStorageType.RetryPolicy = RetryPolicies.RetryN(2, TimeSpan.FromMilliseconds(100));
        }

        #endregion
        #region Account methods


        #endregion
        #region Blob methods
        /// <summary>
        /// Return a list of BLOB containers.
        /// </summary>
        /// <returns></returns>
        /// 
        public IEnumerable<BlobContainer> GetContainers()
        {
            return this.BlobStorageType.ListBlobContainers();
        }
        public string[] GetContainerNames(IEnumerable<BlobContainer> containers)
        {
            if (containers != null && containers.Count() > 0)
            {
                var cN = from BlobContainer b in containers
                         select b.ContainerName;
                return cN.ToArray<string>();
            }

            return new string[] { };

        }

        public IList<BlobContainer> GetContainers(string prefix, int maxResults, ref string marker)
        {
            return new List<BlobContainer>(this.BlobStorageType.ListBlobContainers(prefix, maxResults, ref marker));
        }

        public bool CreateContainer(string containerName)
        {
            BlobContainer container = BlobStorageType.GetBlobContainer(containerName);
            return container.CreateContainer();

        }
        public bool CreateContainer(string containerName, ContainerAccessControl accessType, NameValueCollection metadata)
        {
            BlobContainer container = BlobStorageType.GetBlobContainer(containerName);

            return container.CreateContainer(metadata, accessType);


        }

        public ContainerProperties GetContainerProperties(string containerName)
        {
            BlobContainer container = BlobStorageType.GetBlobContainer(containerName);
            return container.GetContainerProperties();

        }

        public bool SetContainerMetadata(string containerName, NameValueCollection metadata)
        {

            BlobContainer container = BlobStorageType.GetBlobContainer(containerName);
            return container.SetContainerMetadata(metadata);
        }

        public ContainerAccessControl GetContainerACL(string containerName)
        {
            BlobContainer container = BlobStorageType.GetBlobContainer(containerName);
            return container.GetContainerAccessControl();

        }

        public void SetContainerACL(string containerName, ContainerAccessControl accessType)
        {
            BlobContainer container = BlobStorageType.GetBlobContainer(containerName);
            container.SetContainerAccessControl(accessType);

        }

        public bool DeleteContainer(string containerName)
        {
            BlobContainer container = BlobStorageType.GetBlobContainer(containerName);
            return container.DeleteContainer();

        }

        public IList<string> ListBlobsStr(string containerName, string delimiter, string prefix, string marker, int maxResults)
        {

            if (maxResults == 0) maxResults = 100;


            IList<string> blobs = new List<string>();
            string basePath = string.Empty;
            ListBlobsStr(containerName, prefix, delimiter, maxResults, ref marker, blobs, basePath);

            return blobs;

        }

        private void ListBlobsStr(string containerName, string blobPrefix, string delimiter, int maxResults, ref string marker, IList<string> tn, string basePath)
        {

            IList<object> blobList = new List<object>(ListBlobs(containerName, blobPrefix, delimiter, maxResults, ref marker));


            foreach (object o in blobList)
            {
                if (o is BlobProperties)
                {

                    BlobProperties bp = (BlobProperties)o;

                    string[] structureArr = bp.Name.Split(char.Parse(delimiter));

                    if (structureArr != null && structureArr.Length > 0)
                    {

                        if (tn != null)
                        {
                            tn.Add(basePath + delimiter + (structureArr[structureArr.Length - 1]));

                        }


                    }


                }
                else if (o is string)
                {
                    string bPrefix = (string)o;
                    string[] structureArr = bPrefix.Split(char.Parse(delimiter));

                    if (structureArr != null && structureArr.Length > 0)
                    {
                        string node = string.Empty;
                        string t1 = null;
                        if (structureArr.Length > 1)
                        {
                            node = structureArr[structureArr.Length - 2];
                            t1 = node;
                        }
                        else
                        {
                            node = structureArr[0];
                            t1 = node;
                        }


                        ListBlobsStr(containerName, bPrefix, delimiter, maxResults, ref marker, tn, t1);

                    }



                }



            }
        }
        public IEnumerable<object> ListBlobs(string containerName, string prefix)
        {
            BlobContainer container = BlobStorageType.GetBlobContainer(containerName);
            return container.ListBlobs(prefix, true);

        }

        public IEnumerable<object> ListBlobs(string containerName, string prefix, string delimiter, int maxResults, ref string marker)
        {

            BlobContainer container = BlobStorageType.GetBlobContainer(containerName);
            return container.ListBlobs(prefix, delimiter, maxResults, ref marker);


        }

        public bool CreateBlob(string containerName, BlobProperties blobProperties, BlobContents blobContents, bool overwrite)
        {

            BlobContainer container = BlobStorageType.GetBlobContainer(containerName);
            return container.CreateBlob(blobProperties, blobContents, overwrite);


        }

        public bool DeleteBlob(string containerName, string blobName)
        {

            BlobContainer container = BlobStorageType.GetBlobContainer(containerName);
            return container.DeleteBlob(blobName);


        }

        public bool DeleteBlobIfNotModified(string containerName, BlobProperties cachedBlobProperties, string blobName, out bool modified)
        {

            BlobContainer container = BlobStorageType.GetBlobContainer(containerName);
            return container.DeleteBlobIfNotModified(cachedBlobProperties, out modified);


        }

        public bool DoesBlobExist(string containerName, string blobName)
        {

            BlobContainer container = BlobStorageType.GetBlobContainer(containerName);
            return container.DoesBlobExist(blobName);


        }

        public bool DoesContainerExist(string containerName)
        {

            BlobContainer container = BlobStorageType.GetBlobContainer(containerName);
            return container.DoesContainerExist();


        }

        public BlobProperties GetBlob(string containerName, string blobName, BlobContents returnedBlobContents, bool transferAsChunks)
        {

            BlobContainer container = BlobStorageType.GetBlobContainer(containerName);
            return container.GetBlob(blobName, returnedBlobContents, transferAsChunks);


        }

        public bool GetBlobIfModified(string containerName, string blobName, BlobProperties cachedBlobProperties, BlobContents returnedBlobContents, bool transferAsChunks)
        {

            BlobContainer container = BlobStorageType.GetBlobContainer(containerName);
            return container.GetBlobIfModified(cachedBlobProperties, returnedBlobContents, transferAsChunks);


        }

        public BlobProperties GetBlobProperties(string containerName, string blobName)
        {

            BlobContainer container = BlobStorageType.GetBlobContainer(containerName);
            return container.GetBlobProperties(blobName);


        }


        public bool UpdateBlobIfNotModified(string containerName, BlobProperties cachedBlobProperties, BlobContents blobContents)
        {

            BlobContainer container = BlobStorageType.GetBlobContainer(containerName);
            return container.UpdateBlobIfNotModified(cachedBlobProperties, blobContents);


        }

        public void UpdateBlobMetadata(string containerName, BlobProperties blobProperties)
        {

            BlobContainer container = BlobStorageType.GetBlobContainer(containerName);
            container.UpdateBlobMetadata(blobProperties);


        }

        public bool UpdateBlobMetadataIfNotModified(string containerName, BlobProperties blobProperties)
        {

            BlobContainer container = BlobStorageType.GetBlobContainer(containerName);
            return container.UpdateBlobMetadataIfNotModified(blobProperties);


        }

        public bool CopyBlob(string containerName, BlobProperties sourceBlobProperties, string sourceBlobName, string destinationBlobName, string versionString)
        {

            BlobContainer container = BlobStorageType.GetBlobContainer(containerName);
            return container.CopyBlob(sourceBlobProperties, sourceBlobName, destinationBlobName, versionString);
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

        public static BlobContents GZipCompressedBlobContents(string fileName)
        {
            var compressedData = new MemoryStream();

            using (var gzipStream = new GZipStream(compressedData,
                CompressionMode.Compress))
            {
                byte[] fileBytes = File.ReadAllBytes(fileName);
                gzipStream.Write(fileBytes, 0,
                    fileBytes.Length);
            }

            return new BlobContents(compressedData.ToArray());
        }
        public class StringBlob
        {
            public static readonly ContentType TextBlobMIMEType =
                new ContentType("text/plain; charset=UTF-8");

            public StringBlob()
            {
            }

            public StringBlob(string name, string value)
            {
                Blob = new BlobProperties(name);
                Blob.ContentType = TextBlobMIMEType.ToString();
                Value = value;
            }

            public string Value { get; set; }
            public BlobProperties Blob { get; set; }


            public override string ToString()
            {
                return this.Value +
                    (this.Blob.Metadata != null && this.Blob.Metadata.HasKeys() ?
                        " Metadata = " + MetadataToString(this.Blob.Metadata) : "");
            }

            static string MetadataToString(NameValueCollection nv)
            {
                if (nv == null)
                    return "";
                StringBuilder sb = new StringBuilder();
                bool first = true;
                foreach (string key in nv.Keys)
                {
                    if (!first)
                        sb.Append("; ");
                    string value = nv[key];
                    sb.Append(key);
                    sb.Append(" = ");
                    sb.Append(value);
                    first = false;
                }
                return sb.ToString();
            }
        }


        #endregion

        #region Queue methods
        public IEnumerable<MessageQueue> ListQueues(string prefix)
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
        public string[] GetQueueNames(IEnumerable<MessageQueue> queues)
        {
            if (queues != null && queues.Count() > 0)
            {
                var cN = from MessageQueue b in queues
                         select b.Name;
                return cN.ToArray<string>();
            }

            return new string[] { };

        }
        public MessageQueue GetQueue(string queueName)
        {
            bool alreadyExists;
            CreateQueue(queueName, out alreadyExists);

            return QueueStorageType.GetQueue(queueName);

        }

        public bool CreateQueue(string queueName, out bool alreadyExists)
        {
            alreadyExists = false;
            MessageQueue q = QueueStorageType.GetQueue(queueName);
            return q.CreateQueue(out alreadyExists);

        }

        public bool DeleteQueue(string queueName)
        {
            MessageQueue q = QueueStorageType.GetQueue(queueName);
            return q.DeleteQueue();

        }

        public QueueProperties GetQueueProperties(string queueName)
        {
            MessageQueue q = QueueStorageType.GetQueue(queueName);
            return q.GetProperties();

        }

        public bool SetQueueProperties(string queueName, QueueProperties queueProps)
        {
            MessageQueue q = QueueStorageType.GetQueue(queueName);

            return q.SetProperties(queueProps);

        }

        public bool PutMessage(string queueName, Message queueMessage)
        {
            MessageQueue q = QueueStorageType.GetQueue(queueName);
            return q.PutMessage(queueMessage);


        }

        public bool PutMessage(string queueName, Message queueMessage, int timeToLiveInSecs)
        {
            MessageQueue q = QueueStorageType.GetQueue(queueName);
            return q.PutMessage(queueMessage, timeToLiveInSecs);


        }

        public IEnumerable<Message> GetMessages(string queueName, int numberofMessages, int visibilityTimeoutInSecs)
        {
            MessageQueue q = QueueStorageType.GetQueue(queueName);
            return q.GetMessages(numberofMessages, visibilityTimeoutInSecs);


        }


        public IEnumerable<Message> PeekMessages(string queueName, int numberofMessages)
        {
            MessageQueue q = QueueStorageType.GetQueue(queueName);
            return q.PeekMessages(numberofMessages);


        }

        public Message PeekMessage(string queueName)
        {
            MessageQueue q = QueueStorageType.GetQueue(queueName);
            return q.PeekMessage();


        }

        public bool DeleteMessage(string queueName, Message queueMessage)
        {
            MessageQueue q = QueueStorageType.GetQueue(queueName);
            return q.DeleteMessage(queueMessage);


        }

        public bool ClearMessages(string queueName)
        {
            MessageQueue q = QueueStorageType.GetQueue(queueName);
            return q.Clear();


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

        #region Silverlining

        #endregion

        #region Static functions

        /// <summary>
        /// Gets the Storage Helper object based on account name and account key.
        /// It reads the following from the configuration file
        /// BlobStorageEndpoint, QueueStorageEndpoint, TableStorageEndpoint
        /// </summary>
        /// <param name="accountName">Storage Account Name</param>
        /// <param name="accountKey">Storage Account Key</param>
        /// <returns></returns>
        public static WindowsAzureStorageHelper GetStorageHelper(string accountName, string accountKey)
        {
            if (!string.IsNullOrEmpty(accountName) && !string.IsNullOrEmpty(accountKey))
            {
                try
                {


                    string blobStorageEndPoint = WindowsAzureSystemHelper.GetStringConfigurationValue("BlobStorageEndpoint");
                    string queueStorageEndpoint = WindowsAzureSystemHelper.GetStringConfigurationValue("QueueStorageEndpoint");
                    string tableStorageEndpoint = WindowsAzureSystemHelper.GetStringConfigurationValue("TableStorageEndpoint");

                    return new WindowsAzureStorageHelper(
                        accountName,
                        accountKey, false,
                        blobStorageEndPoint,
                        queueStorageEndpoint,
                        tableStorageEndpoint);



                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }//if

            return null;

        }

        public static NameValueCollection GetBlob(string containerName, string blobName, string destinationFilePath, WindowsAzureStorageHelper storageHelper)
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

                BlobContents blobContents;
                BlobProperties props = GetBlob(containerName, blobName, true, out blobContents, storageHelper);
                SaveBlobLocally(destinationFilePath, props, blobContents);
                if (props != null)
                {
                    return props.Metadata;
                }

            }
            //else
            //{
            //    LogInfo("Assembly already exists and is loaded. Develop an Unload Assembly function to unload the assembly when done so that it is not locked.");
            //    LogInfo("Loading and Unloading Assemblies: http://people.oregonstate.edu/~reeset/blog/archives/466");

            //}

            return null;
        }
        public static BlobProperties GetBlob(string containerName, string blobName, bool transferAsChunks, out BlobContents blobContents, WindowsAzureStorageHelper storageHelper)
        {

            blobContents = new BlobContents(new MemoryStream());
            return storageHelper.GetBlob(containerName, blobName, blobContents, transferAsChunks);


        }

        public static void SaveBlobLocally(string fileName, BlobProperties blobProperties, BlobContents blobContents)
        {
            if (blobContents != null && blobContents.AsBytes() != null && blobContents.AsBytes().Length > 0)
            {

                File.WriteAllBytes(fileName, blobContents.AsBytes());



            }//if

        }
        public static bool PutBlob(WindowsAzureStorageHelper storageHelper, string containerName, string blobName, string fileName, bool overwrite, NameValueCollection metadata)
        {
            BlobProperties blobProperties = new BlobProperties(blobName);
            blobProperties.ContentType = WindowsAzureStorageHelper.GetContentTypeFromExtension(Path.GetExtension(fileName));
            blobProperties.Metadata = metadata;


            BlobContents blobContents = null;


            FileInfo fn = new FileInfo(fileName);
            bool ret = false;
            using (FileStream fs = fn.OpenRead())
            {
                blobContents = new BlobContents(fs);
                ret = storageHelper.CreateBlob(containerName, blobProperties, blobContents, overwrite);
            }


            return ret;

        }
        public static bool PutBlob(WindowsAzureStorageHelper storageHelper, string containerName, string blobName, string fileName, byte[] fileContents, bool overwrite, NameValueCollection metadata)
        {

            BlobProperties blobProperties = new BlobProperties(blobName);
            blobProperties.ContentType = WindowsAzureStorageHelper.GetContentTypeFromExtension(Path.GetExtension(fileName));
            blobProperties.Metadata = metadata;


            BlobContents blobContents = null;
            bool ret = false;
            blobContents = new BlobContents(fileContents);
            ret = storageHelper.CreateBlob(containerName, blobProperties, blobContents, overwrite);


            return ret;

        }

        public static bool CopyBlob(WindowsAzureStorageHelper storageHelper, string accountName, string sourceContainerName, string blobName, string destinationContainerName, string destinationBlobName, NameValueCollection metadata)
        {
            BlobProperties sourceBlobProperties = storageHelper.GetBlobProperties(sourceContainerName, blobName);
            if (metadata != null)
            {
                sourceBlobProperties.Metadata = metadata;
            }
            blobName = String.Format("/{0}/{1}/{2}", accountName, sourceContainerName, blobName);
            return storageHelper.CopyBlob(destinationContainerName, sourceBlobProperties, blobName, destinationBlobName, "2009-04-14");

        }

        #endregion



        public static void GetAssemblyDependencies(string inputContainerName, string assemblyDependencies, string localStoragePath, WindowsAzureStorageHelper storageHelper, bool loadAssemblies)
        {
            string[] assemblies = assemblyDependencies.Split(';');
            if (assemblies != null && assemblies.Length > 0)
            {

                foreach (string a in assemblies)
                {

                    string fileName = Path.GetFileName(a);
                    string localFile = localStoragePath + fileName;
                    WindowsAzureStorageHelper.GetBlob(inputContainerName, a, localFile, storageHelper);
                    if (loadAssemblies)
                    {
                        if (Path.GetExtension(a).IndexOf("dll") > -1)
                        {

                            Assembly asm = Assembly.LoadFrom(localFile);
                        }
                    }


                }//foreach


            }

        }

        public static void GetAssemblyDependenciesFromResources(string localAssemblyPath, string localJobStroragePath)
        {

            Assembly asm = Assembly.ReflectionOnlyLoadFrom(localAssemblyPath);
            GetResourcesFromAssembly(asm, localJobStroragePath);

        }
        public static void GetResourcesFromAssembly(Assembly asm, string localStoragePath)
        {
            //string[] files;
            //string configPath = RoleManager.GetLocalResource("AzureDbg").RootPath;
            //string configFile = configPath + "AzureDbgHostConfig.xml";
            //this.Configuration.Serialize(configFile);
            //this.ExtractBinary("AzureDbgHost.exe", configPath + "AzureDbgHost.exe");
            //this.ExtractBinary("dbgeng.dll", configPath + "dbgeng.dll");
            //this.ExtractBinary("dbghelp.dll", configPath + "dbghelp.dll");
            //this.ExtractBinary("mdbgeng.dll", configPath + "mdbgeng.dll");
            //this.ExtractBinary("Ionic.Zip.Reduced.dll", configPath + "Ionic.Zip.Reduced.dll");
            //this.ExtractBinary("System.Management.Automation.dll", configPath + "System.Management.Automation.dll");
            //if (Directory.Exists(Directory.GetCurrentDirectory() + @"\BIN"))
            //{
            //    files = Directory.GetFiles(Directory.GetCurrentDirectory() + @"\BIN");
            //}
            //else
            //{
            //    files = Directory.GetFiles(Directory.GetCurrentDirectory());
            //}
            //foreach (string s in files)
            //{
            //    string fileName = Path.GetFileName(s);
            //    string destFileName = Path.Combine(configPath, fileName);
            //    File.Copy(s, destFileName, true);
            //}
            //ProcessStartInfo psi = new ProcessStartInfo();
            //psi.FileName = configPath + "AzureDbgHost.exe";
            //psi.Arguments = configFile;
            //this.AzureDbgHost = Process.Start(psi);
            //this.AzureDbgHost.EnableRaisingEvents = true;
            //this.AzureDbgHost.Exited += new EventHandler(this.AzureDbgHost_Exited);
            string[] resourceNames = asm.GetManifestResourceNames();
            foreach (string s in resourceNames)
            {
                string fileName = Path.GetFileName(s);


                ExtractBinary(asm, fileName, localStoragePath + fileName);



            }

        }

        private static void ExtractBinary(Assembly assembly, string binary, string path)
        {
            // Assembly assembly = Assembly.GetExecutingAssembly();
            string resourceName = string.Format("{0}.{1}", assembly.GetName().Name, binary);
            using (Stream binStream = assembly.GetManifestResourceStream(resourceName))
            {
                byte[] buffer = new byte[32 * 1024];
                int read;
                using (Stream output = new FileStream(path, FileMode.Create))
                {
                    while ((read = binStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        output.Write(buffer, 0, read);
                    }
                }
            }
        }






    }
}
