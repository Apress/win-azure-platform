using System;
using System.Linq;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using System.IO;
using System.Collections.Generic;
using System.Data;


namespace ProAzureCommonLib
{
    public class WADriveHelper
    {
        public const long MAX_DRIVE_SIZE = 1000000000000;
        public const string LOCAL_STORAGE_NAME = "SilverliningLocalStorage";

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetCommonCacheName()
        {

            return WindowsAzureSystemHelper.GetLocalStorageRootPath(LOCAL_STORAGE_NAME) + "cache";
        }

        /// <summary>
        /// Mounts a drive with the specified parameters
        /// </summary>
        /// <param name="accountName"></param>
        /// <param name="accountKey"></param>
        /// <param name="azureDriveContainerName"></param>
        /// <param name="azureDrivePageBlobName"></param>
        /// <param name="azureDriveCacheDirName"></param>
        /// <param name="azureDriveCacheSizeInMB"></param>
        /// <param name="azureDriveSizeInMB"></param>
        /// <param name="driveOptions"></param>
        /// <returns></returns>
        public static string MountAzureDrive(string accountName, string accountKey, string azureDriveContainerName, string azureDrivePageBlobName, string azureDriveCacheDirName, int azureDriveCacheSizeInMB, int azureDriveSizeInMB, DriveMountOptions driveOptions, out bool wasAlreadyMounted)
        {

            // StorageCredentialsAccountAndKey credentials = new StorageCredentialsAccountAndKey(accountName, accountKey);
            CloudStorageAccount csa = WAStorageHelper.GetCloudStorageAccount(accountName, accountKey, false);

            return MountAzureDrive(csa, azureDriveContainerName, azureDrivePageBlobName, azureDriveCacheDirName, azureDriveCacheSizeInMB, azureDriveSizeInMB, driveOptions, out wasAlreadyMounted);
        }




        /// <summary>
        ///  Mounts a drive 
        /// </summary>
        /// <param name="credentials"></param>
        /// <param name="azureDriveContainerName"></param>
        /// <param name="azureDrivePageBlobName"></param>
        /// <param name="azureDriveCacheDirName"></param>
        /// <param name="azureDriveCacheSizeInMB"></param>
        /// <param name="azureDriveSizeInMB"></param>
        /// <returns></returns>
        public static string MountAzureDrive(CloudStorageAccount csa, string azureDriveContainerName, string azureDrivePageBlobName, string azureDriveCacheDirName, int azureDriveCacheSizeInMB, int azureDriveSizeInMB, DriveMountOptions driveOptions, out bool wasAlreadyMounted)
        {

            // Create the blob client

            CloudBlobClient client = csa.CreateCloudBlobClient();
            // Create the blob container which will contain the pageblob corresponding to the azure drive.
            CloudBlobContainer container = client.GetContainerReference(azureDriveContainerName);
            container.CreateIfNotExist();

            // Get the page blob reference which will be used by the azure drive.
            CloudPageBlob blob = container.GetPageBlobReference(azureDrivePageBlobName);


            return MountAzureDrive(csa, blob.Uri, azureDriveCacheDirName, azureDriveCacheSizeInMB, azureDriveSizeInMB, driveOptions, out wasAlreadyMounted);


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountName"></param>
        /// <param name="accountKey"></param>
        /// <param name="vhdUri"></param>
        /// <param name="azureDriveCacheDirName"></param>
        /// <param name="azureDriveCacheSizeInMB"></param>
        /// <param name="azureDriveSizeInMB"></param>
        /// <param name="driveOptions"></param>
        /// <returns></returns>
        public static string MountAzureDrive(string accountName, string accountKey, Uri vhdUri, string azureDriveCacheDirName, int azureDriveCacheSizeInMB, int azureDriveSizeInMB, DriveMountOptions driveOptions, out bool wasAlreadyMounted)
        {

            // StorageCredentialsAccountAndKey credentials = new StorageCredentialsAccountAndKey(accountName, accountKey);
            CloudStorageAccount csa = WAStorageHelper.GetCloudStorageAccount(accountName, accountKey, false);

            return MountAzureDrive(csa, vhdUri, azureDriveCacheDirName, azureDriveCacheSizeInMB, azureDriveSizeInMB, driveOptions, out wasAlreadyMounted);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="csa"></param>
        /// <param name="vhdUri"></param>
        /// <param name="azureDriveCacheDirName"></param>
        /// <param name="azureDriveCacheSizeInMB"></param>
        /// <param name="azureDriveSizeInMB"></param>
        /// <param name="driveOptions"></param>
        /// <returns></returns>
        public static string MountAzureDrive(CloudStorageAccount csa, Uri vhdUri, string azureDriveCacheDirName, int azureDriveCacheSizeInMB, int azureDriveSizeInMB, DriveMountOptions driveOptions, out bool wasAlreadyMounted)
        {
            wasAlreadyMounted = false;
            // Initialize the cache for mounting the drive.
            CloudDrive.InitializeCache(azureDriveCacheDirName, azureDriveCacheSizeInMB);

            CloudDrive drive = null;

            drive = new CloudDrive(vhdUri, csa.Credentials);
            Uri driveUri = CloudDrive.GetMountedDrives().Values.Where(tuple => tuple == vhdUri).FirstOrDefault();
            // Find out whether the drive has already been mounted by some other instance.
            if (driveUri == null)
            {
                try
                {
                    // Create the drive. Currently no method is provided to verify whether the
                    // drive is already created or not.
                    drive.Create(azureDriveSizeInMB);
                }
                catch (CloudDriveException)
                {
                    // An exception can be thrown if the drive already exists. Hence ignore the 
                    // exception here. If anything is not right, the Mount() will fail.
                }
                // Mount the drive.
                string driveLetter = drive.Mount(azureDriveCacheSizeInMB, driveOptions);


                return driveLetter;
            }
            else
            {
                //Drive is already mounted. So get the drive letter for the drive
                IDictionary<string, Uri> drives = CloudDrive.GetMountedDrives();
                IEnumerator<KeyValuePair<string, Uri>> e = drives.GetEnumerator();

                while (e.MoveNext())
                {
                    if (e.Current.Value == vhdUri)
                    {
                        wasAlreadyMounted = true;
                        return e.Current.Key;
                    }
                }

            }

            throw new Exception("Unable to mount the drive." + vhdUri.ToString());
        }

        /// <summary>
        /// Unmounts the specified drive
        /// </summary>
        /// <param name="drive"></param>
        public static void UnmountAzureDrive(CloudDrive drive)
        {
            try
            {
                if (drive != null)
                {

                    drive.Unmount();
                }
            }
            catch (Exception ex)
            {
                WindowsAzureSystemHelper.LogError(String.Format("Error unmounting drive {0} ", ex.Message));

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="csa"></param>
        /// <param name="uriString"></param>
        public static void UnmountAzureDrive(CloudStorageAccount csa, string uriString)
        {

            try
            {

                CloudDrive drive = new CloudDrive(new Uri(uriString), csa.Credentials);
                if (drive != null)
                {
                    drive.Unmount();
                }
            }
            catch (Exception ex)
            {
                WindowsAzureSystemHelper.LogError(String.Format("Error unmounting drive with Uri {0} - {1}", uriString, ex.Message));

            }

        }
        /// <summary>
        /// Unmount the drive
        /// </summary>
        /// <param name="accountName"></param>
        /// <param name="accountKey"></param>
        /// <param name="uriString"></param>
        public static void UnmountAzureDrive(string accountName, string accountKey, string uriString)
        {

            try
            {
                CloudStorageAccount csa = WAStorageHelper.GetCloudStorageAccount(accountName, accountKey, false);
                CloudDrive drive = new CloudDrive(new Uri(uriString), csa.Credentials);
                if (drive != null)
                {
                    drive.Unmount();
                }
            }
            catch (Exception ex)
            {
                WindowsAzureSystemHelper.LogError(String.Format("Error unmounting drive with Uri {0} - {1}", uriString, ex.Message));

            }

        }


        /// <summary>
        /// Unmounts the drive 
        /// </summary>
        /// <param name="accountName"></param>
        /// <param name="accountKey"></param>
        /// <param name="azureDriveContainerName"></param>
        /// <param name="azureDrivePageBlobName"></param>
        public static void UnmountAzureDrive(string accountName, string accountKey, string azureDriveContainerName, string azureDrivePageBlobName)
        {

            try
            {
                CloudStorageAccount csa = WAStorageHelper.GetCloudStorageAccount(accountName, accountKey, false);
                // Create the blob client

                CloudBlobClient client = csa.CreateCloudBlobClient();
                // Create the blob container which will contain the pageblob corresponding to the azure drive.
                CloudBlobContainer container = client.GetContainerReference(azureDriveContainerName);
                // Get the page blob reference which will be used by the azure drive.
                CloudPageBlob blob = container.GetPageBlobReference(azureDrivePageBlobName);

                CloudDrive drive = new CloudDrive(blob.Uri, csa.Credentials);
                if (drive != null)
                {
                    drive.Unmount();
                }
            }
            catch (Exception ex)
            {
                WindowsAzureSystemHelper.LogError(String.Format("Error unmounting drive with blob {0} - {1}", azureDrivePageBlobName, ex.Message));

            }

        }

        /// <summary>
        /// Snapshot the specified drive
        /// </summary>
        /// <param name="drive"></param>
        public static string SnapshotAzureDrive(CloudDrive drive)
        {
            try
            {
                if (drive != null)
                {

                    return drive.Snapshot().ToString();
                }


            }
            catch (Exception ex)
            {
                WindowsAzureSystemHelper.LogError(String.Format("Error in shapshot drive {0} ", ex.Message));

            }

            return string.Empty;
        }

        /// <summary>
        /// Snapshot the drive
        /// </summary>
        /// <param name="accountName"></param>
        /// <param name="accountKey"></param>
        /// <param name="uriString"></param>
        public static string SnapshotAzureDrive(string accountName, string accountKey, string uriString)
        {

            try
            {
                CloudStorageAccount csa = WAStorageHelper.GetCloudStorageAccount(accountName, accountKey, false);
                CloudDrive drive = new CloudDrive(new Uri(uriString), csa.Credentials);
                if (drive != null)
                {
                    return drive.Snapshot().ToString();
                }
            }
            catch (Exception ex)
            {
                WindowsAzureSystemHelper.LogError(String.Format("Error in snapshot drive with Uri {0} - {1}", uriString, ex.Message));

            }

            return string.Empty;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountName"></param>
        /// <param name="accountKey"></param>
        /// <param name="azureDriveContainerName"></param>
        /// <param name="azureDrivePageBlobName"></param>
        /// <param name="?"></param>
        /// <returns></returns>
        public static string SnapshotAzureDrive(string accountName, string accountKey, string azureDriveContainerName, string azureDrivePageBlobName)
        {

            try
            {
                CloudStorageAccount csa = WAStorageHelper.GetCloudStorageAccount(accountName, accountKey, false);
                // Create the blob client

                CloudBlobClient client = csa.CreateCloudBlobClient();
                // Create the blob container which will contain the pageblob corresponding to the azure drive.
                CloudBlobContainer container = client.GetContainerReference(azureDriveContainerName);
                container.CreateIfNotExist();

                // Get the page blob reference which will be used by the azure drive.
                CloudPageBlob blob = container.GetPageBlobReference(azureDrivePageBlobName);
                CloudDrive drive = new CloudDrive(blob.Uri, csa.Credentials);
                if (drive != null)
                {
                    return drive.Snapshot().ToString();
                }
            }
            catch (Exception ex)
            {
                WindowsAzureSystemHelper.LogError(String.Format("Error in snapshot drive  {0} - {1}", azureDrivePageBlobName, ex.Message));

            }

            return string.Empty;

        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="drive"></param>
        public static void DeleteAzureDrive(CloudDrive drive)
        {
            try
            {
                if (drive != null)
                {

                    drive.Delete();
                }
            }
            catch (Exception ex)
            {
                WindowsAzureSystemHelper.LogError(String.Format("Error deleting drive {0} ", ex.Message));

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountName"></param>
        /// <param name="accountKey"></param>
        /// <param name="uriString"></param>
        public static void DeleteAzureDrive(string accountName, string accountKey, string uriString)
        {

            try
            {
                CloudStorageAccount csa = WAStorageHelper.GetCloudStorageAccount(accountName, accountKey, false);
                CloudDrive drive = new CloudDrive(new Uri(uriString), csa.Credentials);
                if (drive != null)
                {
                    drive.Delete();
                }
            }
            catch (Exception ex)
            {
                WindowsAzureSystemHelper.LogError(String.Format("Error deleting drive with Uri {0} - {1}", uriString, ex.Message));

            }

        }


      
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullFilePath"></param>
        /// <param name="text"></param>
        public static void WriteTestFile(string fullFilePath, string text)
        {
            File.WriteAllText(fullFilePath, text);

        }

     
    

     
    


    }
}
