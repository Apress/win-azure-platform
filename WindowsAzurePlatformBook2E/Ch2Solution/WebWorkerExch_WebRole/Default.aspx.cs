using System;
using System.Collections;
using System.Collections.Generic;
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
using ProAzureCommon;
using System.IO;

namespace WebWorkerExch_WebRole
{
    public partial class _Default : System.Web.UI.Page
    {
      

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ExecuteExchange();
                ListMachines();
            }

        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            ExecuteExchange();
            ListMachines();
        }

        private void ListMachines()
        {
            try
            {
                IList<string> messages = WindowsAzureSystemHelper.ReadAllLinesFromLocalStorage(
                    SystemInfo.SYSTEM_INFO_MACHINE_NAMES, SystemInfo.LOCAL_STORAGE_NAME);

                lbMachines.Items.Clear();

                foreach (string message in messages)
                {
                    lbMachines.Items.Add(message);
                }

                string sysInfoPath = Path.Combine(WindowsAzureSystemHelper.GetLocalStorageRootPath(SystemInfo.LOCAL_STORAGE_NAME), SystemInfo.SYS_INFO_CACHE_XML);
                if (File.Exists(sysInfoPath))
                {
                    string sysInfoFileContents = File.ReadAllText(sysInfoPath);
                    if (!string.IsNullOrEmpty(sysInfoFileContents))
                    {

                        SystemMessageExchange ds = new SystemMessageExchange();
                        ds.ReadXml(new StringReader(sysInfoFileContents));
                        GridView1.DataSource = ds.SystemInfo;
                        GridView1.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                WindowsAzureSystemHelper.LogError(ex.Message);

            }
        }



        private void ExecuteExchange()
        {
            try
            {
                SystemMessageExchange ds = WindowsAzureSystemHelper.GetSystemInfo(SystemInfo.LOCAL_STORAGE_NAME, "Web");


                if (ds == null)
                {
                    WindowsAzureSystemHelper.LogError("ExecuteExchange():SystemMessageExchange DataSet is null");

                }
                else
                {
                    WindowsAzureSystemHelper.LogInfo(ds.GetXml());
                    ISystemInfo sys = new SystemInfo();
                    sys.SendSystemInfo(ds);
                }
            }
            catch (Exception ex)
            {

                WindowsAzureSystemHelper.LogError("ExecuteExchange():" + ex.Message);
            }

        }
    }
}
