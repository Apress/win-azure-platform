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
using System.Net.NetworkInformation;
using System.Diagnostics;


namespace ProAzureCommonLib
{
    public class WindowsAzureSystemHelper
    {
       

        #region Logging

        public static void LogError(string message)
        {
            Trace.WriteLine(String.Format("{0} on machine {1}", message, Environment.MachineName), "Error");

        }

        public static void LogInfo(string message)
        {
            Trace.WriteLine(String.Format("{0} on machine {1}", message, Environment.MachineName), "Information");

         
        }
        #endregion

        #region Configuration 

        public static string GetStringConfigurationValue(string configName)
        {

            try
            {
                return RoleEnvironment.GetConfigurationSettingValue(configName);


            }
            catch (Exception ex)
            {
                LogError(ex.Message);

                throw ex;

            }


        }

        public static bool GetBooleanConfigurationValue(string configName)
        {

            try
            {
                bool ret;
                if (bool.TryParse(RoleEnvironment.GetConfigurationSettingValue(configName), out ret))
                {
                    return ret;
                }
                else
                {
                    LogError(String.Format("Could not parse value for configuration setting {0}", configName));

                    throw new Exception(String.Format("Could not parse value for configuration setting {0}", configName));


                }


            }
            catch (Exception ex)
            {
                LogError(ex.Message);

                throw ex;

            }


        }

        public static int GetIntConfigurationValue(string configName)
        {

            try
            {
                int ret;
                if (int.TryParse(RoleEnvironment.GetConfigurationSettingValue(configName), out ret))
                {
                    return ret;
                }
                else
                {
                    LogError(String.Format("Could not parse value for configuration setting {0}", configName));

                    throw new Exception(String.Format("Could not parse value for configuration setting {0}", configName));


                }


            }
            catch (Exception ex)
            {
                LogError(ex.Message);

                throw ex;

            }


        }

        public static double GetDoubleConfigurationValue(string configName)
        {

            try
            {
                double ret;
                if (double.TryParse(RoleEnvironment.GetConfigurationSettingValue(configName), out ret))
                {
                    return ret;
                }
                else
                {
                    LogError(String.Format("Could not parse value for configuration setting {0}", configName));

                    throw new Exception(String.Format("Could not parse value for configuration setting {0}", configName));


                }


            }
            catch (Exception ex)
            {
                LogError(ex.Message);

                throw ex;

            }


        }



        #endregion

        #region Local Storage Operations

        public static LocalResource GetLocalStorageResource(string localStorageName)
        {
            try
            {
                LocalResource resource = RoleEnvironment.GetLocalResource(localStorageName);

                return resource;

            }
            catch (Exception ex)
            {
                LogError(String.Format("Error in GetLocalStorageResource of {0}. {1}", "WindowsAzureSystemHelper", ex.Message));

                throw ex;
            }



        }

        public static string GetLocalStorageRootPath(string localStorageName)
        {
            try
            {
                LocalResource resource = RoleEnvironment.GetLocalResource(localStorageName);

                return resource.RootPath;

            }
            catch (Exception ex)
            {
                LogError(String.Format("Error in GetLocalStorageRootPath of {0}. {1}", "WindowsAzureSystemHelper", ex.Message));

                throw ex;
            }



        }

        public static bool CanAccessLocalStorage(string localStorageName)
        {
            WindowsAzureSystemHelper.LogInfo("Can access Local Storage?");
            bool ret = false;
            try
            {
                string fp = WindowsAzureSystemHelper.GetLocalStorageRootPath(localStorageName) + "proazure.txt";

                using (StreamWriter sw = File.CreateText(fp))
                {
                    WindowsAzureSystemHelper.LogInfo("Created File " + fp);
                    sw.WriteLine("This is a Pro Azure file.");
                    WindowsAzureSystemHelper.LogInfo("Wrote in File " + fp);


                }//using

                string fpNew = WindowsAzureSystemHelper.GetLocalStorageRootPath(localStorageName) + "proazure2.txt";
                File.Copy(fp, fpNew);
                string fpNew2 = WindowsAzureSystemHelper.GetLocalStorageRootPath(localStorageName) + "proazure3.txt";
                File.Move(fp, fpNew2);
                WindowsAzureSystemHelper.LogInfo("Deleting File " + fpNew2);
                File.Delete(fpNew2);
                WindowsAzureSystemHelper.LogInfo("Deleted File " + fpNew2);

                WindowsAzureSystemHelper.LogInfo("Deleting File " + fpNew);
                File.Delete(fpNew);
                WindowsAzureSystemHelper.LogInfo("Deleted File " + fpNew);

                ret = true;

            }
            catch (Exception ex)
            {

                WindowsAzureSystemHelper.LogError("Error in CanAccessSystemDir " + ex.Message);

            }

            return ret;
        }


        public static void WriteLineToLocalStorage(string fileName, string localStorageName, string message, bool writeDuplicateEntries)
        {
            LogInfo(message);
            string path = GetLocalStorageRootPath(localStorageName);
            path = Path.Combine(path, fileName);
            string entry = String.Format("{0}{1}", message, Environment.NewLine);
            bool write = true;

            if (!writeDuplicateEntries)
            {
                if (!File.Exists(path))
                {
                    using (StreamWriter sw = File.CreateText(path))
                    {

                    }
                }
                string[] lines = File.ReadAllLines(path, Encoding.UTF8);

                if (lines != null && lines.Length > 0)
                {
                    if (lines.Contains<string>(message))
                    {
                        write = false;
                    }
                }
            }
            if (write)
            {
                File.AppendAllText(path, entry, Encoding.UTF8);
            }
        }

        public static IList<string> ReadAllLinesFromLocalStorage(string fileName, string localStorageName)
        {
            List<string> messages = new List<string>();
            string path = Path.Combine(GetLocalStorageRootPath(localStorageName), fileName);


            if (File.Exists(path))
            {
                using (FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    StreamReader reader = new StreamReader(stream, Encoding.UTF8);

                    while (true)
                    {
                        string line = reader.ReadLine();
                        if (line == null) break;
                        messages.Add(line);
                    }
                }
            }

            return messages;
        }



        #endregion

        

        #region Utility Functions
        public static bool CanAccessSystemDir(out string sysDir)
        {
            WindowsAzureSystemHelper.LogInfo("Can access System Directory?");
            sysDir = string.Empty;
            bool ret = false;
            try
            {
                sysDir = Environment.SystemDirectory;

                WindowsAzureSystemHelper.LogInfo("System Directory is " + sysDir);

                if (!string.IsNullOrEmpty(sysDir))
                    ret = true;
            }
            catch (Exception ex)
            {

                WindowsAzureSystemHelper.LogError("Error in CanAccessSystemDir " + ex.Message);

            }

            return ret;

        }

        public static bool CanAccessWindowsDir(out string winDir)
        {
            WindowsAzureSystemHelper.LogInfo("Can access Windows Directory?");
            winDir = string.Empty;
            bool ret = false;
            try
            {
                winDir = Environment.GetEnvironmentVariable("windir");


                WindowsAzureSystemHelper.LogInfo("Windows Directory is " + winDir);

                if (!string.IsNullOrEmpty(winDir))
                    ret = true;
            }
            catch (Exception ex)
            {

                WindowsAzureSystemHelper.LogError("Error in CanAccessSystemDir " + ex.Message);

            }

            return ret;

        }


        public static string Test(string localStorageName)
        {
            StringBuilder strb = new StringBuilder();
            strb.AppendLine("Root Path for Local Storage:" + WindowsAzureSystemHelper.GetLocalStorageRootPath(localStorageName));
            strb.AppendLine("CanAccessLocalStorage:" + WindowsAzureSystemHelper.CanAccessLocalStorage(localStorageName));
            string sysDir;
            strb.Append("CanAccessSystemDir:" + WindowsAzureSystemHelper.CanAccessSystemDir(out sysDir));
            strb.AppendLine("System Directory:" + sysDir);
            strb.Append("CanAccessWindowsDir:" + WindowsAzureSystemHelper.CanAccessWindowsDir(out sysDir));
            strb.AppendLine("Windows Directory:" + sysDir);

            string output = strb.ToString();
            WindowsAzureSystemHelper.LogInfo(output);

            return output;

        }

        #endregion

        #region Email

        public static void SendMail(string hostName, int port,
            string account, string password,
            string domain, string from, string to,
            string mailMessage, string subject,
            bool enableSSL, string replyTo

            )
        {
  
            SmtpClient smtp = new SmtpClient(hostName, port);
            smtp.Credentials = new NetworkCredential(account, password, domain);
            MailMessage message = new MailMessage(new MailAddress(from), new MailAddress(to));
            message.Body = mailMessage;
            message.Subject = subject;
            message.ReplyTo = new MailAddress(replyTo);
            smtp.EnableSsl = enableSSL;


            try
            {
                smtp.Send(message);
                LogInfo("Mail message sent");
            }
            catch (Exception ex)
            {

                LogError("SendMail:" + ex.Message);
            }


        }

        /*
        public static void CreateMessageWithAttachment(string hostName, 
            int port,
            string fileName,
            string account, string password,
            string domain, string from, string to,
            string mailMessage, string subject,
            bool enableSSL, string replyTo)
        {
            // Specify the file to be attached and sent.
            // This example assumes that a file named Data.xls exists in the
            // current working directory.
            string file = "data.xls";
            // Create a message and set up the recipients.
            MailMessage message = new MailMessage(
               "jane@contoso.com",
               "ben@contoso.com",
               "Quarterly data report.",
               "See the attached spreadsheet.");

            // Create  the file attachment for this e-mail message.
            Attachment data = new Attachment(file, MediaTypeNames.Application.Octet);
            // Add time stamp information for the file.
            ContentDisposition disposition = data.ContentDisposition;
            disposition.CreationDate = System.IO.File.GetCreationTime(file);
            disposition.ModificationDate = System.IO.File.GetLastWriteTime(file);
            disposition.ReadDate = System.IO.File.GetLastAccessTime(file);
            // Add the file attachment to this e-mail message.
            message.Attachments.Add(data);

            //Send the message.
            SmtpClient client = new SmtpClient(server);
            // Add credentials if the SMTP server requires them.
            client.Credentials = CredentialCache.DefaultNetworkCredentials;

            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                LogInfo("Exception caught in CreateMessageWithAttachment(): {0}",
                      ex.ToString());
            }
            // Display the values in the ContentDisposition for the attachment.
            ContentDisposition cd = data.ContentDisposition;
            LogInfo("Content disposition");
            LogInfo(cd.ToString());
            LogInfo("File {0}", cd.FileName);
            LogInfo("Size {0}", cd.Size);
            LogInfo("Creation {0}", cd.CreationDate);
            LogInfo("Modification {0}", cd.ModificationDate);
            LogInfo("Read {0}", cd.ReadDate);
            LogInfo("Inline {0}", cd.Inline);
            LogInfo("Parameters: {0}", cd.Parameters.Count);
            foreach (DictionaryEntry d in cd.Parameters)
            {
                LogInfo("{0} = {1}", d.Key, d.Value);
            }
            data.Dispose();
        }
         */
        #endregion

        #region WMI


       public static void GetWMIStats()
        {
            long mb = 1048576; //megabyte in # of bytes 1024x1024

            //Connection credentials to the remote computer - not needed if the logged in account has access
            ConnectionOptions oConn = new ConnectionOptions();
            //oConn.Username = "username";
            //oConn.Password = "password";
            System.Management.ManagementScope oMs = new System.Management.ManagementScope("\\\\localhost", oConn);

            //get Fixed disk stats
            System.Management.ObjectQuery oQuery = new System.Management.ObjectQuery("select FreeSpace,Size,Name from Win32_LogicalDisk where DriveType=3");
            ManagementObjectSearcher oSearcher = new ManagementObjectSearcher(oMs, oQuery);
            ManagementObjectCollection oReturnCollection = oSearcher.Get();

            //variables for numerical conversions
            double fs = 0;
            double us = 0;
            double tot = 0;
            double up = 0;
            double fp = 0;

            //for string formating args
            object[] oArgs = new object[2];
            LogInfo("*******************************************");
            LogInfo("Hard Disks");
            LogInfo("*******************************************");

            //loop through found drives and write out info
            foreach (ManagementObject oReturn in oReturnCollection)
            {
                // Disk name
                LogInfo("Name : " + oReturn["Name"].ToString());

                //Free space in MB
                fs = Convert.ToInt64(oReturn["FreeSpace"]) / mb;

                //Used space in MB
                us = (Convert.ToInt64(oReturn["Size"]) - Convert.ToInt64(oReturn["FreeSpace"])) / mb;

                //Total space in MB
                tot = Convert.ToInt64(oReturn["Size"]) / mb;

                //used percentage
                up = us / tot * 100;

                //free percentage
                fp = fs / tot * 100;

                //used space args
                oArgs[0] = (object)us;
                oArgs[1] = (object)up;

                //write out used space stats
                LogInfo(String.Format("Used: {0:#,###.##} MB ({1:###.##})%", oArgs));
                

                //free space args
                oArgs[0] = fs;
                oArgs[1] = fp;

                //write out free space stats
                LogInfo(String.Format("Free: {0:#,###.##} MB ({1:###.##})%", oArgs));
                LogInfo(String.Format("Size :  {0:#,###.##} MB", tot));
                LogInfo("*******************************************");
            }

            // Get process info including a method to return the user who is running it
            oQuery = new System.Management.ObjectQuery("select * from Win32_Process");
            oSearcher = new ManagementObjectSearcher(oMs, oQuery);
            oReturnCollection = oSearcher.Get();

            LogInfo("");
            LogInfo("");
            LogInfo("*******************************************");
            LogInfo("Processes");

            LogInfo("*******************************************");
            LogInfo("");

            //loop through each process - I limited it to first 6 so the DOS buffer would not overflow and cut off the disk stats
            int i = 0;
            foreach (ManagementObject oReturn in oReturnCollection)
            {
                if (i == 6)
                    break;
                i++;
                LogInfo("*******************************************");
                LogInfo(oReturn["Name"].ToString().ToLower());
                LogInfo("*******************************************");
                //arg to send with method invoke to return user and domain - below is link to SDK doc on it
                //http://msdn.microsoft.com/library/default.asp?url=/library/en-us/wmisdk/wmi/getowner_method_in_class_win32_process.asp?frame=true
                string[] o = new String[2];
                oReturn.InvokeMethod("GetOwner", (object[])o);
                //write out user info that was returned
                LogInfo("User: " + o[1] + "\\" + o[0]);
                LogInfo("PID: " + oReturn["ProcessId"].ToString());

                //get priority
                if (oReturn["Priority"] != null)
                    LogInfo("Priority: " + oReturn["Priority"].ToString());

                //get creation date - need managed code function to convert date - 
                if (oReturn["CreationDate"] != null)
                {
                    try
                    {
                        //get datetime string and convert
                        string s = oReturn["CreationDate"].ToString();
                        DateTime dc = ToDateTime(s);
                        //write out creation date
                        LogInfo("CreationDate: " + dc.AddTicks(-TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).Ticks).ToLocalTime().ToString());
                    }
                    //just in case - I was getting a weird error on some entries
                    catch (Exception err)
                    {
                        LogInfo(err.Message);
                    }
                }
                //this is the amount of memory used
                if (oReturn["WorkingSetSize"] != null)
                {
                    long mem = Convert.ToInt64(oReturn["WorkingSetSize"].ToString()) / 1024;
                    LogInfo(String.Format("Mem Usage: {0:#,###.##}Kb", mem));
                }
                LogInfo("");
            }
        }

        //There is a utility called mgmtclassgen that ships with the .NET SDK that
        //will generate managed code for existing WMI classes. It also generates
        // datetime conversion routines like this one.
        //Thanks to Chetan Parmar and dotnet247.com for the help.
        static System.DateTime ToDateTime(string dmtfDate)
        {
            int year = System.DateTime.Now.Year;
            int month = 1;
            int day = 1;
            int hour = 0;
            int minute = 0;
            int second = 0;
            int millisec = 0;
            string dmtf = dmtfDate;
            string tempString = System.String.Empty;

            if (((System.String.Empty == dmtf) || (dmtf == null)))
            {
                return System.DateTime.MinValue;
            }

            if ((dmtf.Length != 25))
            {
                return System.DateTime.MinValue;
            }

            tempString = dmtf.Substring(0, 4);
            if (("****" != tempString))
            {
                year = System.Int32.Parse(tempString);
            }

            tempString = dmtf.Substring(4, 2);

            if (("**" != tempString))
            {
                month = System.Int32.Parse(tempString);
            }

            tempString = dmtf.Substring(6, 2);

            if (("**" != tempString))
            {
                day = System.Int32.Parse(tempString);
            }

            tempString = dmtf.Substring(8, 2);

            if (("**" != tempString))
            {
                hour = System.Int32.Parse(tempString);
            }

            tempString = dmtf.Substring(10, 2);

            if (("**" != tempString))
            {
                minute = System.Int32.Parse(tempString);
            }

            tempString = dmtf.Substring(12, 2);

            if (("**" != tempString))
            {
                second = System.Int32.Parse(tempString);
            }

            tempString = dmtf.Substring(15, 3);

            if (("***" != tempString))
            {
                millisec = System.Int32.Parse(tempString);
            }

            System.DateTime dateRet = new System.DateTime(year, month, day, hour, minute, second, millisec);

            return dateRet;
        }
        #endregion

        #region Network Utility
        public static string[] GetIpAddresses(string semiColonSeparatedIp)
        {
            string[] ips = semiColonSeparatedIp.Split(new char[] { ';' });

            return ips;

        }
        public static void PingIps(string semiColonSeparatedIp)
        {
            string[] ips = GetIpAddresses(semiColonSeparatedIp);

            if (ips != null && ips.Length > 0)
            {
                foreach (string ip in ips)
                {
                   LogInfo(String.Format("Ping Result for IpAddress {0} is {1}", ip, Ping(ip)));

                }

            }

        }
        public static string Ping(string ipAddressString)
        {

            using (Ping pingSender = new Ping())
            {
                PingOptions pingOptions = null;
                StringBuilder pingResults = null;
                PingReply pingReply = null;
                IPAddress ipAddress = null;
                int numberOfPings = 4;
                int pingTimeout = 1000;
                int byteSize = 32;
                byte[] buffer = new byte[byteSize];
                int sentPings = 0;
                int receivedPings = 0;
                int lostPings = 0;
                long minPingResponse = 0;
                long maxPingResponse = 0;
                //string ipAddressString = "192.168.105.10";

                pingOptions = new PingOptions();
                //pingOptions.DontFragment = true;
                //pingOptions.Ttl = 128;

                ipAddress = IPAddress.Parse(ipAddressString);

                pingResults = new StringBuilder();
                pingResults.AppendLine(string.Format("Pinging {0} with {1} bytes of data:", ipAddress, byteSize));
                pingResults.AppendLine();

                for (int i = 0; i < numberOfPings; i++)
                {
                    sentPings++;

                    pingReply = pingSender.Send(ipAddress, pingTimeout, buffer, pingOptions);

                    if (pingReply.Status == IPStatus.Success)
                    {
                        pingResults.AppendLine(string.Format("Reply from {0}: bytes={1} time={2}ms TTL={3}", ipAddress, byteSize, pingReply.RoundtripTime, pingReply.Options.Ttl));

                        if (minPingResponse == 0)
                        {
                            minPingResponse = pingReply.RoundtripTime;
                            maxPingResponse = minPingResponse;
                        }
                        else if (pingReply.RoundtripTime < minPingResponse)
                        {
                            minPingResponse = pingReply.RoundtripTime;
                        }
                        else if (pingReply.RoundtripTime > maxPingResponse)
                        {
                            maxPingResponse = pingReply.RoundtripTime;
                        }

                        receivedPings++;
                    }
                    else
                    {
                        pingResults.AppendLine(pingReply.Status.ToString());
                        lostPings++;
                    }
                }

                pingResults.AppendLine();
                pingResults.AppendLine(string.Format("Ping statistics for {0}:", ipAddress));
                pingResults.AppendLine(string.Format("\tPackets: Sent = {0}, Received = {1}, Lost = {2}", sentPings, receivedPings, lostPings));
                pingResults.AppendLine("Approximate round trip times in milli-seconds:");
                pingResults.AppendLine(string.Format("\tMinimum = {0}ms, Maximum = {1}ms", minPingResponse, maxPingResponse));


                return pingResults.ToString();
            }
        }

        #endregion



    }
}
