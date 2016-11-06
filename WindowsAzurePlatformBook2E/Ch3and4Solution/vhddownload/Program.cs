// Andy Edwards, Microsoft 2010
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.WindowsAzure.StorageClient;
using Microsoft.WindowsAzure;


namespace vhddownload
{
    
    public class VhdDownload
    {
        public static void Main(string[] args)
        {
            Config config = Config.Parse(args);
            try
            {
                Console.WriteLine("Downloading: " + config.Url.AbsoluteUri + "\n" +
                                  "To:          " + config.Vhd.FullName);
                DownloadVHDFromCloud(config);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error downloading vhd:\n" + e.ToString());
            }
        }
        private static void DownloadVHDFromCloud(Config config)
        {
            StorageCredentialsAccountAndKey creds = new StorageCredentialsAccountAndKey(config.Account, config.Key);

            CloudBlobClient blobStorage = new CloudBlobClient(config.AccountUrl, creds);
            blobStorage.ReadAheadInBytes = 0;

            CloudBlobContainer container = blobStorage.GetContainerReference(config.Container);
            CloudPageBlob pageBlob = container.GetPageBlobReference(config.Blob);

            // Get the length of the blob
            pageBlob.FetchAttributes();
            long vhdLength = pageBlob.Properties.Length;
            long totalDownloaded = 0;
            Console.WriteLine("Vhd size:  " + Megabytes(vhdLength));

            // Create a new local file to write into
            FileStream fileStream = new FileStream(config.Vhd.FullName, FileMode.Create, FileAccess.Write);
            fileStream.SetLength(vhdLength);

            // Download the valid ranges of the blob, and write them to the file
            IEnumerable<PageRange> pageRanges = pageBlob.GetPageRanges();
            BlobStream blobStream = pageBlob.OpenRead();

            foreach (PageRange range in pageRanges)
            {
                // EndOffset is inclusive... so need to add 1
                int rangeSize = (int)(range.EndOffset + 1 - range.StartOffset);

                // Chop range into 4MB chucks, if needed
                for (int subOffset = 0; subOffset < rangeSize; subOffset += FourMegabyteAsBytes)
                {
                    int subRangeSize = Math.Min(rangeSize - subOffset, FourMegabyteAsBytes);
                    blobStream.Seek(range.StartOffset + subOffset, SeekOrigin.Begin);
                    fileStream.Seek(range.StartOffset + subOffset, SeekOrigin.Begin);

                    Console.WriteLine("Range: ~" + Megabytes(range.StartOffset + subOffset) + " + " + PrintSize(subRangeSize));
                    byte[] buffer = new byte[subRangeSize];

                    blobStream.Read(buffer, 0, subRangeSize);
                    fileStream.Write(buffer, 0, subRangeSize);
                    totalDownloaded += subRangeSize;
                }
            }
            Console.WriteLine("Downloaded " + Megabytes(totalDownloaded) + " of " + Megabytes(vhdLength));
        }
        private static int OneMegabyteAsBytes = 1024 * 1024;
        private static int FourMegabyteAsBytes = 4 * OneMegabyteAsBytes;
        private static string Megabytes(long bytes)
        {
            return (bytes / OneMegabyteAsBytes).ToString() + " MB";
        }

        private static string PrintSize(long bytes)
        {
            if (bytes >= 1024 * 1024) return (bytes / 1024 / 1024).ToString() + " MB";
            if (bytes >= 1024) return (bytes / 1024).ToString() + " kb";
            return (bytes).ToString() + " bytes";
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
                WriteConsoleAndExit("Usage: vhddownload <url> <file> <keyfile>");
            }
            Config config = new Config();
            config.Url = new Uri(args[0]);
            config.Vhd = new FileInfo(args[1]);
            if (config.Vhd.Exists)
            {
                try
                {
                    config.Vhd.Delete();
                }
                catch (Exception e)
                {
                    WriteConsoleAndExit("Failed to delete vhd file:\n" + e.ToString());
                }
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
