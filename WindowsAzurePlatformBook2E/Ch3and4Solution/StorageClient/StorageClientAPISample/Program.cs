//
// <copyright file="Program.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mime;
using System.Configuration;
using System.IO;
using System.Net;
using System.Collections.Specialized;
using System.Data.Services.Common;
using System.Data.Services.Client;
using System.Globalization;


namespace Microsoft.Samples.ServiceHosting.StorageClientAPISample
{
    using Microsoft.Samples.ServiceHosting.StorageClient;

    class Program
    {

        static void Main()
        {       
            BlobSamples.RunSamples();

            if (QueueSamples.RunQueueSamples)
            {
                QueueSamples.RunSamples();
            } 

            if (TableSamples.RunTableSamples)
            {
                TableSamples.RunSamples1();
                TableSamples.RunSamples2();
            }

            Console.WriteLine("Press <ENTER>");
            Console.ReadLine();
        }
    }

    internal class TableSamples
    {
        internal static bool RunTableSamples = true;

        internal class SampleEntity : TableStorageEntity
        {

            public SampleEntity(string partitionKey, string rowKey)
                : base(partitionKey, rowKey)
            {
                // We set a default value for the date time field because the table
                // storage service rejects date times not in the correct range.
                C = TableStorageConstants.MinSupportedDateTime;
            }

            public SampleEntity() : base() {
            }

            public int A {
                get;
                set;
            }

            public string B {
                get;
                set;
            }

            public DateTime C {
                get;
                set;
            }

            public Guid D
            {
                get;
                set;
            }
        }

        internal class SampleDataServiceContext : TableStorageDataServiceContext
        {
            internal SampleDataServiceContext(StorageAccountInfo accountInfo)
                : base(accountInfo)
            {
            }

            internal const string SampleTableName = "SampleTable";
        
            public IQueryable<SampleEntity> SampleTable
            {
                get
                {
                    return this.CreateQuery<SampleEntity>(SampleTableName);
                }
            }
        }

        private static string GetExceptionMessage(Exception exception)
        {
            HttpStatusCode statusCode;
            StorageExtendedErrorInformation extendedErrorInfo;
            if (TableStorageHelpers.EvaluateException(exception, out statusCode, out extendedErrorInfo))
            {
                if (extendedErrorInfo != null)
                {
                    return string.Format(CultureInfo.InvariantCulture, "{0} {1}", extendedErrorInfo.ErrorCode ?? "", extendedErrorInfo.ErrorMessage ?? "");
                }
            }

            DataServiceClientException dse = exception.InnerException as DataServiceClientException;
            if (dse != null)
                return dse.Message;
            else
                return exception.Message;
        }

        // this method shows an alternative way of accessing/creating DataServiceContext objects
        // this approach is closer to what tools generate for normal ADO.NET Data Services projects
        internal static void RunSamples1()
        {
            StorageAccountInfo account = null;
            try
            {
                Console.WriteLine("Show how to create tables and queries using the SampleDataServiceContext class...");

                account = StorageAccountInfo.GetDefaultTableStorageAccountFromConfiguration();
                SampleDataServiceContext svc = new SampleDataServiceContext(account);
                svc.RetryPolicy = RetryPolicies.RetryN(3, TimeSpan.FromSeconds(1));

                // Create 'SampleTable'
                // this uses the SampleDataServiceContext class 
                TableStorage.CreateTablesFromModel(typeof(SampleDataServiceContext), account);

                string sampleTableName = SampleDataServiceContext.SampleTableName;

                DeleteAllEntriesFromSampleTable(svc, sampleTableName);


                svc.AddObject(SampleDataServiceContext.SampleTableName, new SampleEntity("sample", "entity"));
                svc.SaveChangesWithRetries();

                var qResult = from c in svc.SampleTable
                              where c.PartitionKey == "samplepartitionkey" && c.RowKey == "samplerowkey1"
                              select c;

                TableStorageDataServiceQuery<SampleEntity> q = new TableStorageDataServiceQuery<SampleEntity>(qResult as DataServiceQuery<SampleEntity>, svc.RetryPolicy);

                try {
                    // the query references the whole key and explicitly addresses one entity
                    // thus, this query can generate an exception if there are 0 results during enumeration
                    IEnumerable<SampleEntity> res = q.ExecuteAllWithRetries();
                    foreach (SampleEntity s in res)
                    {
                        Console.WriteLine("This code is not reached. " + s.PartitionKey);
                    }
                } catch(DataServiceQueryException e) {
                    HttpStatusCode s;
                    if (TableStorageHelpers.EvaluateException(e, out s) && s == HttpStatusCode.NotFound)
                    {
                        // this would mean the entity was not found
                        Console.WriteLine("The entity was not found. This is expected here.");
                    }
                }

                Console.WriteLine("Delete all entries in the sample table.");
                DeleteAllEntriesFromSampleTable(svc, sampleTableName);
                Console.WriteLine("Table sample 1 finished!");
            }
            catch (DataServiceRequestException dsre)
            {
                Console.WriteLine("DataServiceRequestException: " + GetExceptionMessage(dsre));
                ShowTableStorageErrorMessage(account.BaseUri.ToString());
            }
            catch (InvalidOperationException ioe)
            {
                Console.WriteLine("Storage service error: " + GetExceptionMessage(ioe));
                ShowTableStorageErrorMessage(account.BaseUri.ToString());
            }
        }
        
        // shows alternative ways of generating DataServiceContext objects
        internal static void RunSamples2()
        {
            StorageAccountInfo account = null;
            try
            {
                account = StorageAccountInfo.GetDefaultTableStorageAccountFromConfiguration();
                TableStorage tableStorage = TableStorage.Create(account);
                tableStorage.RetryPolicy = RetryPolicies.RetryN(3, TimeSpan.FromSeconds(1));
                // the DataServiceContext object inherits its retry policy from tableStorage in this case
                TableStorageDataServiceContext svc = tableStorage.GetDataServiceContext();


                Console.WriteLine("Table creation, delete and list samples...");
                string sampleTableName = SampleDataServiceContext.SampleTableName;                
                tableStorage.TryCreateTable(sampleTableName);
                
                DeleteAllEntriesFromSampleTable(svc, sampleTableName);

                Console.WriteLine("List all tables in the account.");
                IEnumerable<string> tables2 = tableStorage.ListTables();
                foreach (string n1 in tables2)
                {
                    Console.WriteLine(n1);
                }

                Console.WriteLine("Inserting entities into the table...");

                SampleEntity t = new SampleEntity("samplepartitionkey", "samplerowkey");
                svc.AddObject(sampleTableName, t);
                svc.SaveChangesWithRetries();

                //Detach the existing entity so that we can demonstrate the server side
                //error when you try to insert an same object with the same keys
                svc.Detach(t);

                // Insert an entity with the same keys
                Console.WriteLine("Try to insert the same entity into the table and show how to deal with error conditions.");

                t = new SampleEntity("samplepartitionkey", "samplerowkey");
                svc.AddObject(sampleTableName, t);
                try
                {
                    svc.SaveChangesWithRetries();
                    // getting here is an error because inserting the same row twice raises an exception
                   Console.WriteLine("Should not get here. Succeeded inserting two entities with the same keys");
                }
                catch (Exception e)
                {
                    HttpStatusCode status;
                    StorageExtendedErrorInformation errorInfo;
                    if (TableStorageHelpers.EvaluateException(e, out status, out errorInfo)
                        && status == HttpStatusCode.Conflict)
                    {
                        // the row has already been inserted before, this is expected here
                        if (errorInfo != null)
                        {
                            Console.WriteLine("Attempting to insert row with same keys resulted in error {0} : {1}",
                                               errorInfo.ErrorCode, errorInfo.ErrorMessage);
                        }
                    }
                    else
                        throw;
                }

                svc.Detach(t);

                Console.WriteLine("Insert a large item into the table.");
                t = new SampleEntity("samplepartitionkey", "samplerowkey1");
                t.B = new String('a', 1000);
                svc.AddObject(sampleTableName, t);
                svc.SaveChangesWithRetries();

                Console.WriteLine("Create a normal DataServiceContext object (not TableStorageDataServiceContext) and attach it to a TableStorage object.");
                DataServiceContext svc2 = new DataServiceContext(
                    TableStorage.GetServiceBaseUri(account.BaseUri, account.UsePathStyleUris, account.AccountName));
                tableStorage.Attach(svc2);

                var qResult = from c in svc2.CreateQuery<SampleEntity>(sampleTableName)
                              where c.RowKey == "samplerowkey1"
                              select c;
                
                foreach (SampleEntity cust in qResult)
                {
                    if (cust.B != t.B)
                    {
                        Console.WriteLine("Sample failed. Did not read the entity property just written");
                    }
                }

                Console.WriteLine("Insert many rows in a table and show the API for dealing with query result pagination.");

                int num = 2100;
                Console.WriteLine("Inserting {0} rows.", num.ToString(CultureInfo.CurrentUICulture));
                for (int i = 0; i < num; i++)
                {
                    t = new SampleEntity("samplestring", i.ToString(CultureInfo.InvariantCulture));
                    svc.AddObject(sampleTableName, t);
                    svc.SaveChangesWithRetries();
                    if ((i + 1) % 50 == 0)
                    {
                        Console.WriteLine("Inserted row {0}.", (i + 1).ToString(CultureInfo.CurrentUICulture));
                    }
                }

                Console.WriteLine("Executing query that will return many results. This can take a while...");
                var qResult2 = from c in svc.CreateQuery<SampleEntity>(sampleTableName)
                               where c.PartitionKey == "samplestring"
                               select c;

                TableStorageDataServiceQuery<SampleEntity> tableStorageQuery = new TableStorageDataServiceQuery<SampleEntity>(qResult2 as DataServiceQuery<SampleEntity>);
                IEnumerable<SampleEntity> res = tableStorageQuery.ExecuteAllWithRetries();
                Console.WriteLine("Retrieved query results:");
                foreach (SampleEntity entity in res)
                {
                    Console.WriteLine("Partition key: {0}, row key: {1}.", entity.PartitionKey, entity.RowKey);
                }

                Console.WriteLine("Delete all entries in the sample table.");
                DeleteAllEntriesFromSampleTable(tableStorage.GetDataServiceContext(), sampleTableName);
                tableStorage.DeleteTable(sampleTableName);
                Console.WriteLine("Table samples finished!");
            }
            catch (DataServiceRequestException dsre)
            {
                Console.WriteLine("DataServiceRequestException: " + GetExceptionMessage(dsre));
                ShowTableStorageErrorMessage(account.BaseUri.ToString());
            }
            catch (InvalidOperationException ioe)
            {
                Console.WriteLine("Storage service error: " + GetExceptionMessage(ioe));
                ShowTableStorageErrorMessage(account.BaseUri.ToString());
            }
        }

        private static void DeleteAllEntriesFromSampleTable(TableStorageDataServiceContext svc, string tableName)
        {
            IEnumerable<SampleEntity> res;
            TableStorageDataServiceQuery<SampleEntity> q;

            Console.WriteLine("Deleting all entities from the table...");
            Console.WriteLine("This can take a while...");

            var qResult = from c in svc.CreateQuery<SampleEntity>(tableName)
                          select c;
            
            q = new TableStorageDataServiceQuery<SampleEntity>(qResult as DataServiceQuery<SampleEntity>);
            res = q.ExecuteAllWithRetries();
            int i = 0;
            foreach (SampleEntity s in res)
            {
                svc.DeleteObject(s);
                svc.SaveChangesWithRetries();
                if (++i % 50 == 0)
                {
                    Console.WriteLine("Deleted element " + i);
                }
            }
        }

        private static void ShowTableStorageErrorMessage(string endpoint)
        {
            Console.WriteLine("Please check if the table storage service is running at " + endpoint);
            Console.WriteLine("Detailed information on how to run the development table storage tool " +
                              "locally can be found in the readme file that comes with this sample.");
            Console.WriteLine("Also make sure that you started the table service with the right parameters, " +
                              "that is, exactly as described in the readme file for this sample.");
        }
    }

    internal static class QueueSamples
    {

        internal static bool RunQueueSamples = true;
        internal static int MessagesReceived;

        internal static void MessageReceived(object sender, MessageReceivedEventArgs e) {
            Console.WriteLine("Received message " + e.Message.ContentAsString());
            MessagesReceived++;
        }

        internal static void RunSamples()
        {
            StorageAccountInfo account = null;
            try
            {
                string sampleGuid = (Guid.NewGuid()).ToString("N");
                string name = "queue" + sampleGuid;
                string name2 = "queue2" + sampleGuid;
                string prefix = "prefix" + sampleGuid;
                bool exists = false;
                bool res = false;

                Console.WriteLine("Create queue " + name);
                account = StorageAccountInfo.GetDefaultQueueStorageAccountFromConfiguration();
                QueueStorage queueService = QueueStorage.Create(account);
                queueService.RetryPolicy = RetryPolicies.RetryN(2, TimeSpan.FromMilliseconds(100));
                MessageQueue q = queueService.GetQueue(name);
                res = q.CreateQueue(out exists);
                if (!exists && res)
                {
                    Console.WriteLine("Queue " + name + " successfully created.");
                }

                Console.WriteLine("Get all the queues in an account.");
                IEnumerable<MessageQueue> queues = queueService.ListQueues();
                foreach (MessageQueue qu in queues)
                {
                    Console.WriteLine(qu.Name);
                }

                Console.WriteLine("Create a number of queues and show continuation listing. This can take a while...");
                int numListSample = 60;
                for (int j = 0; j < numListSample; j++) {
                    q = queueService.GetQueue(prefix + (Guid.NewGuid()).ToString("N").Substring(0, 10));
                    q.CreateQueue();
                }
                queues = queueService.ListQueues();
                List<MessageQueue> l = new List<MessageQueue>(queues);
                Console.WriteLine("The following queues are available:");
                foreach (MessageQueue qu in l)
                {
                    Console.WriteLine(qu.Name);
                }

                Console.WriteLine("Find all queues with a given prefix...");
                queues = queueService.ListQueues(prefix);
                l = new List<MessageQueue>(queues);
                Console.WriteLine("Queues with the prefix " + prefix);
                foreach (MessageQueue qu in l)
                {
                    Console.WriteLine(qu.Name);
                }

                Console.WriteLine("Delete all queues with the prefix " + prefix);
                foreach (MessageQueue qu in queues)
                {
                    Console.WriteLine("Delete queue" + qu.Name);
                    qu.Clear();
                    qu.DeleteQueue();
                }

                q = queueService.GetQueue(name);
                if (!q.DoesQueueExist())
                {
                    Console.WriteLine("Queue '{0}' does not exist");
                }             
  
                Console.WriteLine("Delete queue " + name);
                q.Clear();
                q.DeleteQueue();

                Console.WriteLine("Get queue properties.");
                q = queueService.GetQueue(name2);
                res = q.CreateQueue(out exists);
                if (!exists && res)
                {
                    Console.WriteLine("Queue " + name + " successfully created.");
                }
                QueueProperties props = new QueueProperties();
                props = q.GetProperties();
                props.Metadata = new NameValueCollection();
                props.Metadata.Add("meta-sample1", "sample1");
                props.Metadata.Add("meta-sample2", "sample2");
                q.SetProperties(props);
                props = null;
                props = q.GetProperties();
                Console.WriteLine("Queue properties: " + props.Metadata["meta-sample1"] + " " + props.Metadata["meta-sample2"]);

                Console.WriteLine("Put message into the queue.");
                if (q.PutMessage(new Message("<sample>sample message</sample>")))
                {
                    Console.WriteLine("Message successfully put into queue.");
                }
                Console.WriteLine("Get message from the queue.");
                Message msg = q.GetMessage();
                Console.WriteLine(msg.ContentAsString());

                Console.WriteLine("Clear all messages from a queue.");
                for (int i = 0; i < 10; i++)
                {
                    q.PutMessage(new Message("<sample>" + i + "</sample>"));
                }
                q.Clear();

                Console.WriteLine("Delete a single message.");
                for (int i = 0; i < 10; i++)
                {
                    q.PutMessage(new Message("<sample>" + i + "</sample>"));
                }
                Message msg1 = q.GetMessage();
                q.DeleteMessage(msg1);
                q.Clear();

                Console.WriteLine("Automatic reception of messages.");
                q.MessageReceived += MessageReceived;
                q.PollInterval = 500;
                q.StartReceiving();
                for (int i = 0; i < 10; i++)
                {
                    q.PutMessage(new Message("<samplemessage>" + i + "</samplemessage>"));
                    System.Threading.Thread.Sleep(1000);

                }
                q.StopReceiving();
                q.Clear();
                q.DeleteQueue();

                Console.WriteLine("Queue samples finished successfully.");
            }
            catch (System.Net.WebException we)
            {
                Console.WriteLine("Network error: " + we.Message);
                if (we.Status == System.Net.WebExceptionStatus.ConnectFailure)
                {
                    Console.WriteLine("Please check if the queue storage service is running at " + account.BaseUri.ToString());
                    Console.WriteLine("Detailed information on how to run the development storage tool " +
                                      "locally can be found in the readme file that comes with this sample.");
                }
            }
            catch (StorageException se)
            {
                Console.WriteLine("Storage service error: " + se.Message);
            }
        }
    }

    /// <summary>
    /// Simple class to aggregate a blob's properties and its string valued contents
    /// </summary>
    internal class StringBlob
    {
        internal static readonly ContentType TextBlobMIMEType =
            new ContentType("text/plain; charset=UTF-8");

        internal StringBlob()
        {
        }

        internal StringBlob(string name, string value)
        {
            Blob = new BlobProperties(name);
            Blob.ContentType = TextBlobMIMEType.ToString();
            Value = value;
        }

        internal string Value { get; set; }
        internal BlobProperties Blob { get; set; }


        public override string ToString()
        {
            return this.Value +
                (this.Blob.Metadata != null && this.Blob.Metadata.HasKeys() ?
                    " Metadata = " + MetadataToString(this.Blob.Metadata) : "");
        }

        static internal string MetadataToString(NameValueCollection nv)
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


    internal static class BlobSamples
    {

        /// <summary>
        /// Store a UTF-8 encoded string.
        /// </summary>
        private static void PutTextBlob(
            BlobContainer container,
            StringBlob s
            )
        {
            container.CreateBlob(
                s.Blob,
                new BlobContents(Encoding.UTF8.GetBytes(s.Value)),
                true
                );
        }

        /// <summary>
        /// Read a UTF-8 encoded string.
        /// </summary>
        /// <param name="blobName">Name of the blob</param>
        /// <returns>Contents of the blob.</returns>
        internal static StringBlob GetTextBlob(BlobContainer container, string blobName)
        {
            BlobContents contents = new BlobContents(new MemoryStream());
            BlobProperties blob = container.GetBlob(blobName, contents, false);
            if (blob.ContentType == null)
            {
                throw new FormatException("No content type set for blob.");
            }

            ContentType blobMIMEType = new ContentType(blob.ContentType);
            if (!blobMIMEType.Equals(StringBlob.TextBlobMIMEType))
            {
                throw new FormatException("Not a text blob.");
            }

            return new StringBlob
            {
                Blob = blob,
                Value = Encoding.UTF8.GetString(contents.AsBytes())
            };
        }

        internal static bool RefreshTextBlob(BlobContainer container, StringBlob stringBlob)
        {
            BlobContents contents = new BlobContents(new MemoryStream());
            BlobProperties blob = stringBlob.Blob;
            bool modified = container.GetBlobIfModified(blob, contents, false);

            if (!modified)
                return false;

            if (blob.ContentType == null)
            {
                throw new FormatException("No content type set for blob.");
            }

            ContentType blobMIMEType = new ContentType(blob.ContentType);
            if (!blobMIMEType.Equals(StringBlob.TextBlobMIMEType))
            {
                throw new FormatException("Not a text blob.");
            }

            stringBlob.Value = UnicodeEncoding.UTF8.GetString(contents.AsBytes());
            return modified;
        }

        internal static bool UpdateTextBlob(BlobContainer container, StringBlob stringBlob)
        {
            return container.UpdateBlobIfNotModified(stringBlob.Blob,
                new BlobContents(Encoding.UTF8.GetBytes(stringBlob.Value)));
        }

        internal static void DownloadToFile(BlobContainer container, string filepath, string blobName)
        {
            using (FileStream fs = File.Open(filepath, FileMode.Create))
            {
                BlobContents contents = new BlobContents(fs);
                BlobProperties blob = container.GetBlob(blobName, contents, true);
                Console.WriteLine("Downloaded blob {0} to file {1}", blob.Name, fs.Name);
                fs.Close();
            }
        }

        internal static void PutLargeString(BlobContainer container, StringBlob s, int repeatCount)
        {
            s.Blob.ContentType = StringBlob.TextBlobMIMEType.ToString();
            RepeatedStream largeStream = new RepeatedStream(
                new MemoryStream(Encoding.UTF8.GetBytes(s.Value)), repeatCount);
            container.CreateBlob(s.Blob, new BlobContents(largeStream), true);
            Console.WriteLine("Successfully uploaded blob {0} at time {1}", s.Blob.Name, s.Blob.LastModifiedTime);
        }


        internal static void RunSamples()
        {
            StorageAccountInfo account = StorageAccountInfo.GetDefaultBlobStorageAccountFromConfiguration();
            string containerName = StorageAccountInfo.GetConfigurationSetting("ContainerName", null, true);

            NameValueCollection containerMetadata = new NameValueCollection();
            containerMetadata.Add("Name", "StorageSample");

            BlobStorage blobStorage = BlobStorage.Create(account);

            blobStorage.RetryPolicy = RetryPolicies.RetryN(2, TimeSpan.FromMilliseconds(100));

            


            try
            {
                BlobContainer container = blobStorage.GetBlobContainer(containerName);

                //Create the container if it does not exist.
                container.CreateContainer(containerMetadata, ContainerAccessControl.Private);

                ContainerProperties containerProperties = container.GetContainerProperties();
                Console.WriteLine("Container {0} LastModified {1} ETag {2} Metadata {3}",
                                    containerProperties.Name,
                                    containerProperties.LastModifiedTime,
                                    containerProperties.ETag,
                                    StringBlob.MetadataToString(containerProperties.Metadata)
                                 );

                ContainerAccessControl acl = container.GetContainerAccessControl();
                Console.WriteLine("Container has access control {0}", acl);


                // write some text blobs
                NameValueCollection nv1 = new NameValueCollection();
                nv1["m1"] = "v1";
                nv1["m2"] = "v2";

                StringBlob hello1 = new StringBlob("hello.txt", "Hello World");
                hello1.Blob.Metadata = nv1;
                Console.WriteLine("Creating blob hello.txt");
                PutTextBlob(container, hello1);

                BlobProperties prop = container.GetBlobProperties("hello.txt");
                Console.WriteLine("hello.txt content length = " + prop.ContentLength);


                StringBlob goodbye1 = new StringBlob("goodbye.txt", "Goodbye world");
                Console.WriteLine("Creating blob goodbye.txt");
                PutTextBlob(container, goodbye1);

                // read back the blobs
                Console.WriteLine("Getting hello.txt and goodbye.txt");
                StringBlob hello2 = GetTextBlob(container, "hello.txt");
                Console.WriteLine("hello.txt: " + hello2.ToString());
                StringBlob goodbye2 = GetTextBlob(container, "goodbye.txt");
                Console.WriteLine("goodbye.txt " + goodbye2.ToString());


                //Try to get a blob that does not exist
                try
                {
                    GetTextBlob(container, "noSuchBlob");
                }
                catch (StorageClientException sce)
                {
                    //The extended error information when present provides more specific and detailed information
                    // about the cause of the error.
                    Console.WriteLine(
                        "Error attempting to get blob 'noSuchBlob' Error Code = {0} Message = {1}",
                        sce.ExtendedErrorInformation != null ?
                            sce.ExtendedErrorInformation.ErrorCode : sce.ErrorCode.ToString(),
                        sce.Message
                        );
                }

                //update metadata of hello.txt
                hello2.Blob.Metadata["m3"] = "v3";
                Console.WriteLine("Updating metadata of hello.txt");
                container.UpdateBlobMetadata(hello2.Blob);

                hello2.Blob.Metadata["m4"] = "v4";
                container.UpdateBlobMetadataIfNotModified(hello2.Blob);

                //Refresh hello.txt. It has changed.
                bool refreshed = RefreshTextBlob(container, hello1);
                if (refreshed)
                    Console.WriteLine("hello.txt refreshed " + hello1.ToString());
                else
                    Console.WriteLine("hello.txt not refreshed");

                Console.WriteLine("Uploading a large blob");
                PutLargeString(
                    container,
                    new StringBlob("LargeBlob.txt", "Let us repeat this string a large number of times "),
                    50000
                );

                Console.WriteLine("Downloading large blob to file LargeBlob.txt");
                DownloadToFile(container, "LargeBlob.txt", "LargeBlob.txt");

                //Refresh hello.txt. It hasn't changed.
                refreshed = RefreshTextBlob(container, hello2);
                if (refreshed)
                    Console.WriteLine("hello.txt refreshed " + hello2.ToString());
                else
                    Console.WriteLine("hello.txt not refreshed");

                //Change goodbye.txt and refresh it
                StringBlob goodbye3 = new StringBlob("goodbye.txt", "Goodbye again world");
                PutTextBlob(container, goodbye3);

                //Now refresh the other reference to goodbye.txt (goodbye2)
                refreshed = RefreshTextBlob(container, goodbye2);
                if (refreshed)
                    Console.WriteLine("goodbye.txt refreshed " + goodbye2.ToString());
                else
                    Console.WriteLine("goodbye.txt not refreshed");


                //Update hello.txt
                hello2.Value = "Hello again world";
                bool updated = UpdateTextBlob(container, hello2);
                if (updated)
                    Console.WriteLine("hello.txt updated " + hello2.ToString());
                else
                    Console.WriteLine("hello.txt not updated because it has been changed");

                //Try to update goodbye.txt through goodbye1.
                //This should fail because it has been updated thru goodbye3
                goodbye1.Value = "Farewell world";
                updated = UpdateTextBlob(container, goodbye1);
                if (updated)
                    Console.WriteLine("goodbye.txt updated " + goodbye1.ToString());
                else
                    Console.WriteLine("goodbye.txt not updated because it has been changed");

                Console.WriteLine("Creating blob 'deleteme.txt'");
                PutTextBlob(container, new StringBlob("deleteme.txt", "deleteme"));

                Console.WriteLine("Creating blobs f/a.txt and f/b.txt");
                PutTextBlob(container, new StringBlob("f/a.txt", "This is a.txt"));
                PutTextBlob(container, new StringBlob("f/b.txt", "This is b.txt"));

                Console.WriteLine("Enumerating all blobs");

                // enumerate all the blobs
                foreach (object b1 in container.ListBlobs("", false))
                {
                    Console.WriteLine("{0}", ((BlobProperties)b1).Uri);
                }

                Console.WriteLine("Enumerating all blobs with combining common prefixes");
                foreach (object b2 in container.ListBlobs("", true))
                {
                    BlobProperties blobProperties = b2 as BlobProperties;
                    if (blobProperties != null)
                        Console.WriteLine("{0}", blobProperties.Uri);
                    else
                        Console.WriteLine("Common prefix: {0}", (string)b2);
                }

                Console.WriteLine("Deleting blob 'deleteme.txt'");
                container.DeleteBlob("deleteme.txt");

                Console.WriteLine("Enumerate the blobs again");
                foreach (object b3 in container.ListBlobs("", false))
                {
                    Console.WriteLine("{0}", ((BlobProperties)b3).Uri);
                }

                // Create another container
                Console.WriteLine("Creating container 'deleteme'");
                BlobContainer container2 = blobStorage.GetBlobContainer("deleteme");
                container2.CreateContainer();


                // enumerate containers
                foreach (BlobContainer c in blobStorage.ListBlobContainers())
                {
                    Console.WriteLine("Container: {0}", c.ContainerUri);
                }

                // Delete the container
                Console.WriteLine("Deleting container 'deleteme'");
                container2.DeleteContainer();

                // enumerate containers
                foreach (BlobContainer c in blobStorage.ListBlobContainers())
                {
                    Console.WriteLine("Container: {0}", c.ContainerUri);
                }

            }
            catch (System.Net.WebException we)
            {
                Console.WriteLine("Network error: " + we.Message);
                if (we.Status == System.Net.WebExceptionStatus.ConnectFailure)
                {
                    Console.WriteLine("Please check if the blob storage service is running at " + account.BaseUri.ToString());
                    Console.WriteLine("Detailed information on how to run the development storage tool " +
                                      "locally can be found in the readme file that comes with this sample.");
                }
            }
            catch (StorageException se)
            {
                Console.WriteLine("Storage service error: " + se.Message);
            }

        }
    }


    /// <summary>
    /// Helper class to generate large blob content
    /// </summary>
    internal class RepeatedStream : Stream
    {
        Stream stream;
        long length;

        internal RepeatedStream(Stream stream, int n)
        {
            this.stream = stream;
            this.length = stream.Length * n;
        }

        public override long Position
        {
            get;
            set;
        }
        public override long Length
        {
            get
            {
                return length;
            }
        }
        public override int Read(byte[] buffer, int offset, int count)
        {
            long oldPosition = Position;
            while (count > 0 && Position < Length)
            {
                if (stream.Position >= stream.Length)
                    stream.Seek(0, SeekOrigin.Begin);
                int read = stream.Read(buffer, offset, (int)Math.Min(count, stream.Length));
                count -= read;
                offset += read;
                Position += read;
            }
            return (int)(Position - oldPosition);
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            long newPosition = 0;
            switch (origin)
            {
                case SeekOrigin.Begin:
                    newPosition = offset;
                    break;

                case SeekOrigin.End:
                    newPosition = length - offset;
                    break;

                case SeekOrigin.Current:
                    newPosition = Position + offset;
                    break;
            }

            long positionInContainedStream = newPosition % stream.Length;
            stream.Seek(positionInContainedStream, SeekOrigin.Begin);
            Position = newPosition;
            return newPosition;
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

    }
}

