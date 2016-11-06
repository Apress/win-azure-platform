using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Microsoft.WindowsAzure.ServiceRuntime;
using System.Text;
using System.IO;
using System.Management;
using System.Diagnostics;
using System.ServiceModel;
using HelloService;

namespace HelloAzureCloud_WebRole
{
    public partial class _Default : System.Web.UI.Page
    {
        private string _rootPath = string.Empty;
        private bool _throwExceptions = false;
        private bool _enableonScreenLogging = false;

        protected void Page_Load(object sender, EventArgs e)
        {


            _throwExceptions = GetExceptionsSetting();

            LogInfo("Throw Exceptions Setting is " + _throwExceptions);

            _enableonScreenLogging = GetOnScreenLoggingSetting();

            LogInfo("On screen logging Setting is " + _enableonScreenLogging);

            _rootPath = GetRootPath();

            LogInfo("Root Path is " + _rootPath);

            try
            {
                FillFilesList();
            }
            catch (Exception ex)
            {
               
                LogError("Error in Page_Load " + ex.Message);

               

            }

           
        }

        protected void btnWhere_Click(object sender, EventArgs e)
        {
           
            string currentLogLevel = RoleEnvironment.GetConfigurationSettingValue("LogLevel");
            lblCloudMachineName.Text = Server.MachineName;
            lblRoleId.Text = RoleEnvironment.CurrentRoleInstance.Id;
            lblUserHostAddress.Text = Request.UserHostAddress;
            lblCurrentLogLevel.Text = currentLogLevel;
            lblLocalStoragePath.Text = GetRootPath();
            lblThrowExceptions.Text = _throwExceptions.ToString();

            
            LogInfo(String.Format("This is a {0} message on machine {1}", currentLogLevel, Server.MachineName));

            if (CanAccessLocalStorage())
            {
                lblCanAccessLocalStorage.Text = "true";
            }
            else
            {

                lblCanAccessLocalStorage.Text = "false";
            }

            string sysDir;
            if (CanAccessSystemDir(out sysDir))
            {

                lblCanAccessSystemDirectory.Text = "true." + sysDir;
            }
            else
            {
                lblCanAccessSystemDirectory.Text = "false." + sysDir;

            }

            string winDir;
            if (CanAccessWindowsDir(out winDir))
            {
                lblCanAccessWindowsDirectory.Text = "true." + winDir;
            }
            else
            {
                lblCanAccessWindowsDirectory.Text = "false." + winDir;

            }

            lblUpgradeDomain.Text = RoleEnvironment.CurrentRoleInstance.UpdateDomain.ToString();

            lblFaultDomain.Text = RoleEnvironment.CurrentRoleInstance.FaultDomain.ToString();
        
            try
            {

                CallWorkerRole();

            }
            catch (Exception ex)
            {
                LogError("Inter-role communication Error: " + ex.Message);

            }
        }

        

        protected string GetRootPath()
        {
            LocalResource resource = RoleEnvironment.GetLocalResource("HelloAzureWorldLocalCache");

            return resource.RootPath; 


        }

        protected void UploadFile(FileUpload fu, string rootPath)
        {
            byte[] fb = fu.FileBytes;

            if (fb != null && fb.Length > 0)
            {
                string filePath = rootPath + fu.FileName;
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    


                }

                File.WriteAllBytes(filePath, fb);
                LogInfo(String.Format("Uploaded File {0} to {1}", fu.FileName, filePath));
            }//if


        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                UploadFile(this.fuCache, GetRootPath());
                FillFilesList();
                

            }
            catch (Exception ex)
            {
                LogError(String.Format("Error in btnUpload_Click. {0}", ex.Message));
                
            }
        }

       

        protected void FillFilesList()
        {
            if (_rootPath != string.Empty)
            {
                string[] fileList = Directory.GetFiles(_rootPath);


                if (fileList != null && fileList.Length > 0)
                {
                    LogInfo("Number of files in Local Storage " + fileList.Count());

                    foreach (string s in fileList)
                    {
                        string fn = Path.GetFileName(s);
                        ListItem li = new ListItem(fn, fn);
                        if (!lstFiles.Items.Contains(li))
                        {
                            lstFiles.Items.Add(li);
                        }
                    }

                }//if
            }//if
            else
            {
                LogInfo("File list is empty.");


            }
            

        }

       
        private bool GetExceptionsSetting()
        {
            string te = RoleEnvironment.GetConfigurationSettingValue("ThrowExceptions");

            if (!string.IsNullOrEmpty(te))
            {
                bool temp;

               if( bool.TryParse(te, out temp))
               {
                  
                   return temp;

               }//if

            }//if
            
            return false;
        }

        private bool GetOnScreenLoggingSetting()
        {
            string te = RoleEnvironment.GetConfigurationSettingValue("EnableOnScreenLogging");

            if (!string.IsNullOrEmpty(te))
            {
                bool temp;

                if (bool.TryParse(te, out temp))
                {

                    return temp;

                }//if

            }//if

            return false;
        }


        private bool CanAccessSystemDir(out string sysDir)
        {
            LogInfo("Can access System Directory?");
            sysDir = string.Empty;
            bool ret = false;
            try
            {
                sysDir = Environment.SystemDirectory;

                LogInfo("System Directory is " + sysDir);

                if (!string.IsNullOrEmpty(sysDir))
                    ret = true;
            }
            catch (Exception ex)
            {

                LogError("Error in CanAccessSystemDir " + ex.Message);

            }

            return ret;

        }

        private bool CanAccessWindowsDir(out string winDir)
        {
            LogInfo("Can access Windows Directory?");
            winDir = string.Empty;
            bool ret = false;
            try
            {
                winDir = Environment.GetEnvironmentVariable("windir");


                LogInfo("Windows Directory is " + winDir);

                if (!string.IsNullOrEmpty(winDir))
                    ret = true;
            }
            catch (Exception ex)
            {

                LogError("Error in CanAccessSystemDir " + ex.Message);

            }

            return ret;

        }

        private bool CanAccessLocalStorage()
        {
            LogInfo("Can access Local Storage?");
            bool ret = false;
            try
            {
                string fp = _rootPath + "proazure.txt";

                using (StreamWriter sw = File.CreateText(fp))
                {
                    LogInfo("Created File " + fp);
                    sw.WriteLine("This is a Pro Azure file.");
                    LogInfo("Wrote in File " + fp);


                }//using

                LogInfo("Deleting File " + fp);
                File.Delete(fp);
                LogInfo("Deleted File " + fp);

                ret = true;

            }
            catch (Exception ex)
            {

                LogError("Error in CanAccessSystemDir " + ex.Message);

            }

            return ret;
        }

        #region Logging

        private void LogError(string message)
        {

            System.Diagnostics.Trace.WriteLine("Error", message);

            if (_throwExceptions)
            {
                lblExceptionsTxt.Visible = true;
                lblException.Text += "Exception>" + message + "<br/>"; ;

            }

        }

        private void LogInfo(string message)
        {
            System.Diagnostics.Trace.WriteLine("Information", String.Format("{0} on machine {1}", message, Server.MachineName));

            if (_enableonScreenLogging)
            {
                lblLogsTxt.Visible = true;
                //lblLogging.Text += "<<--------------------------------------------New Section--------------------------------------->><br />";
                lblLogging.Text += "LOG>" + message + "<br/>";

            }
        }
        #endregion

        #region WMI


        public void GetWMIStats()
        {
            LogInfo("WMI");
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

        #region EventLog

        public  void ReadEventLogs()
        {
            try
            {
                EventLog[] events = EventLog.GetEventLogs();

                foreach (EventLog e in events)
                {

                    LogInfo(String.Format("{0}", e.LogDisplayName));
                    
                    foreach (EventLogEntry en in e.Entries)
                    {

                        LogInfo(String.Format("{0}  {1}", en.TimeWritten.ToString("s"), en.Message));

                    }

                }
            }
            catch (Exception ex)
            {
                LogError(ex.Message);

            }

        }
        #endregion

        #region Inter-Role
        private void CallWorkerRole()
        {
            lblWRHostName.Text = "";
            lblWRIp.Text = "";
            lblWRUpgradeDomain.Text = "";
            lblWRFaultDomain.Text = "";
            lblWREndpointAddress.Text = "";

            foreach (RoleInstance ri in RoleEnvironment.Roles["HelloWorkerRole"].Instances)
            {
                string wrIp = ri.InstanceEndpoints["MyInternalEndpoint"].IPEndpoint.ToString();
                lblWREndpointAddress.Text += "/" + wrIp;
                var serviceAddress = new Uri(String.Format("net.tcp://{0}/{1}", wrIp, "helloservice"));
                var endpointAddress = new EndpointAddress(serviceAddress);
                var binding = new NetTcpBinding(SecurityMode.None);
                var client = new ClientProxy(binding, endpointAddress);
                lblWRHostName.Text +=  "/" + client.GetHostName();
                lblWRIp.Text += "/" + client.GetMyIp();
                lblWRUpgradeDomain.Text += "/" + client.GetUpdateDomain().ToString();
                lblWRFaultDomain.Text += "/" + client.GetFaultDomain().ToString();

            }

        }
        #endregion
    }
}
