using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;
using Microsoft.WindowsAzure;

namespace CDNTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //Replace the credentials with your own.
            StorageCredentialsAccountAndKey credentials = new StorageCredentialsAccountAndKey("silverliningstorage1",
    "ZXdXkdkUa7EMxoTtygmbC8CV9keeMxWrBOQaFCfYHNZYjj8DX56y0DofQaaC3DmgCGf049C/SSgEnhapWWoTjT1/zXPAi==");

            //Create a storage account instance
            CloudStorageAccount storageAccount = new CloudStorageAccount(credentials, true);

            //Create a new blob client instance
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            //Create a new container instance
            CloudBlobContainer container = blobClient.GetContainerReference("mycdn");
            //Create the container if it does not exist
            container.CreateIfNotExist();

            //Specify that the container is publicly accessible. This is a requirement for CDN
            BlobContainerPermissions containerPermissions = new BlobContainerPermissions();
            containerPermissions.PublicAccess = BlobContainerPublicAccessType.Container;
            container.SetPermissions(containerPermissions);

            //Create a new blob
            CloudBlob blob = blobClient.GetBlobReference("mycdn/mytestblob.txt");
            blob.UploadText("My first CDN Blob.");

            //Set the Cache-Control header property of the blob and specify your desired refresh interval (in seconds).
            blob.Properties.CacheControl = "public, max-age=30036000";
            blob.SetProperties();

        }
    }
}
