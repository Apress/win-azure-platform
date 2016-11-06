// Andy Edwards, Microsoft 2010
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.WindowsAzure.StorageClient;
using Microsoft.WindowsAzure;

namespace vhdupload
{
   

    public class VhdUpload
    {
        public static void Main(string[] args)
        {
            Config config = Config.Parse(args);
            try
            {
                Console.WriteLine("Uploading: " + config.Vhd.FullName + "\n" +
                                  "To:        " + config.Url.AbsoluteUri);
                UploadVHDToCloud(config);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error uploading vhd:\n" + e.ToString());
            }
        }
        private static bool IsAllZero(byte[] range, long rangeOffset, long size)
        {
            for (long offset = 0; offset < size; offset++)
            {
                if (range[rangeOffset + offset] != 0)
                {
                    return false;
                }
            }
            return true;
        }
        private static void UploadVHDToCloud(Config config)
        {
            StorageCredentialsAccountAndKey creds = new StorageCredentialsAccountAndKey(config.Account, config.Key);

            CloudBlobClient blobStorage = new CloudBlobClient(config.AccountUrl, creds);
            CloudBlobContainer container = blobStorage.GetContainerReference(config.Container);
            container.CreateIfNotExist();

            CloudPageBlob pageBlob = container.GetPageBlobReference(config.Blob);
            Console.WriteLine("Vhd size:  " + Megabytes(config.Vhd.Length));

            long blobSize = RoundUpToPageBlobSize(config.Vhd.Length);
            pageBlob.Create(blobSize);

            FileStream stream = new FileStream(config.Vhd.FullName, FileMode.Open, FileAccess.Read);
            BinaryReader reader = new BinaryReader(stream);

            long totalUploaded = 0;
            long vhdOffset = 0;
            int offsetToTransfer = -1;

            while (vhdOffset < config.Vhd.Length)
            {
                byte[] range = reader.ReadBytes(FourMegabytesAsBytes);

                int offsetInRange = 0;

                // Make sure end is page size aligned
                if ((range.Length % PageBlobPageSize) > 0)
                {
                    int grow = (int)(PageBlobPageSize - (range.Length % PageBlobPageSize));
                    Array.Resize(ref range, range.Length + grow);
                }

                // Upload groups of contiguous non-zero page blob pages.  
                while (offsetInRange <= range.Length)
                {
                    if ((offsetInRange == range.Length) ||
                        IsAllZero(range, offsetInRange, PageBlobPageSize))
                    {
                        if (offsetToTransfer != -1)
                        {
                            // Transfer up to this point
                            int sizeToTransfer = offsetInRange - offsetToTransfer;
                            MemoryStream memoryStream = new MemoryStream(range, offsetToTransfer, sizeToTransfer, false, false);
                            pageBlob.WritePages(memoryStream, vhdOffset + offsetToTransfer);
                            Console.WriteLine("Range ~" + Megabytes(offsetToTransfer + vhdOffset) + " + " + PrintSize(sizeToTransfer));
                            totalUploaded += sizeToTransfer;
                            offsetToTransfer = -1;
                        }
                    }
                    else
                    {
                        if (offsetToTransfer == -1)
                        {
                            offsetToTransfer = offsetInRange;
                        }
                    }
                    offsetInRange += PageBlobPageSize;
                }
                vhdOffset += range.Length;
            }
            Console.WriteLine("Uploaded " + Megabytes(totalUploaded) + " of " + Megabytes(blobSize));
        }

        private static int PageBlobPageSize = 512;
        private static int OneMegabyteAsBytes = 1024 * 1024;
        private static int FourMegabytesAsBytes = 4 * OneMegabyteAsBytes;
        private static string PrintSize(long bytes)
        {
            if (bytes >= 1024 * 1024) return (bytes / 1024 / 1024).ToString() + " MB";
            if (bytes >= 1024) return (bytes / 1024).ToString() + " kb";
            return (bytes).ToString() + " bytes";
        }
        private static string Megabytes(long bytes)
        {
            return (bytes / OneMegabyteAsBytes).ToString() + " MB";
        }
        private static long RoundUpToPageBlobSize(long size)
        {
            return (size + PageBlobPageSize - 1) & ~(PageBlobPageSize - 1);
        }
    }
    public class Config
    {
        public Uri Url;
        public string Key;
        public FileInfo Vhd;
        public string AccountUrl
        {
            get
            {
                return Url.GetLeftPart(UriPartial.Authority);
            }
        }
        public string Account
        {
            get
            {
                string accountUrl = AccountUrl;

                accountUrl = accountUrl.Substring(Url.GetLeftPart(UriPartial.Scheme).Length);
                accountUrl = accountUrl.Substring(0, accountUrl.IndexOf('.'));

                return accountUrl;
            }
        }
        public string Container
        {
            get
            {
                string container = Url.PathAndQuery;
                container = container.Substring(1);
                container = container.Substring(0, container.IndexOf('/'));
                return container;
            }
        }
        public string Blob
        {
            get
            {
                string blob = Url.PathAndQuery;
                blob = blob.Substring(1);
                blob = blob.Substring(blob.IndexOf('/') + 1);

                int queryOffset = blob.IndexOf('?');
                if (queryOffset != -1)
                {
                    blob = blob.Substring(0, queryOffset);
                }
                return blob;
            }
        }
        public static Config Parse(string[] args)
        {
            if (args.Length != 3)
            {
                WriteConsoleAndExit("Usage: vhdupload <file> <url> <keyfile>");
            }

            Config config = new Config();
            config.Url = new Uri(args[1]);
            config.Vhd = new FileInfo(args[0]);

            if (!config.Vhd.Exists)
            {
                WriteConsoleAndExit(args[0] + " does not exist");
            }

            config.ReadKey(args[2]);

            return config;
        }
        public void ReadKey(string filename)
        {
            try
            {
                Key = File.ReadAllText(filename);
                Key = Key.TrimEnd(null);
                Key = Key.TrimStart(null);
            }
            catch (Exception e)
            {
                WriteConsoleAndExit("Error reading key file:\n" + e.ToString());
            }
        }
        private static void WriteConsoleAndExit(string s)
        {
            Console.WriteLine(s);
            System.Environment.Exit(1);
        }
    } 
}
