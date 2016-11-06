using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;
using System.Collections.Specialized;
using Microsoft.WindowsAzure.StorageClient.Protocol;
using System.Globalization;
using System.Net;
using System.IO;
using System.IO.Compression;


namespace ProAzureCommonLib
{
    /// <summary>
    /// Extensions for Blob-related objects.
    /// </summary>
    public static class BlobExtensions
    {
        public static bool Exists(this CloudBlob blob)
        {
            try
            {
                blob.FetchAttributes();
                return true;
            }
            catch (StorageClientException e)
            {
                if (e.ErrorCode == StorageErrorCode.ResourceNotFound)
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }

        public static bool Exists(this CloudBlobContainer container)
        {
            try
            {
                container.FetchAttributes();
                return true;
            }
            catch (StorageClientException e)
            {
                if (e.ErrorCode == StorageErrorCode.ResourceNotFound)
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }

        public static void UploadCompressedFile(this CloudBlob blob, string filename)
        {
            MemoryStream compressedData = GZipCompressedBlobContents(filename);
            blob.UploadFromStream(compressedData);
        }

        public static void DownloadToCompressedFile(this CloudBlob blob, string filename, bool overwrite)
        {
            MemoryStream memoryStream = new MemoryStream();
            blob.DownloadToStream(memoryStream);
            GZipWriteToFile(memoryStream, filename, overwrite);
        }

        internal static MemoryStream GZipCompressedBlobContents(string fileName)
        {
            var compressedData = new MemoryStream();

            // write file contents to compressed stream
            using (var gzipStream = new GZipStream(compressedData, CompressionMode.Compress))
            {
                byte[] fileBytes = File.ReadAllBytes(fileName);
                gzipStream.Write(fileBytes, 0, fileBytes.Length);
            }

            return compressedData;
        }

        internal static void GZipWriteToFile(MemoryStream inputStream, string filename, bool overwrite)
        {
            FileMode mode = (overwrite) ? FileMode.Create : FileMode.CreateNew;
            FileStream fileStream = new FileStream(filename, mode);

            try
            {
                // compress stream, write to file
                using (var gzipStream = new GZipStream(fileStream, CompressionMode.Compress))
                {
                    inputStream.Position = 0; // make sure we are starting at the beginning
                    byte[] fileBytes = inputStream.ToArray();
                    gzipStream.Write(fileBytes, 0, fileBytes.Length);
                }
            }
            finally
            {
                fileStream.Close();
                fileStream.Dispose();
            }
        }
    }
}
