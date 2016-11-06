using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EventPoint_WebRole
{
    public partial class Manager : System.Web.UI.Page
    {
        protected string strConnectionString; 

        protected void Messages_RowDeleted(object sender, System.Web.UI.WebControls.GridViewDeletedEventArgs e)
        {
            // Pause briefly to let data get deleted
            System.Threading.Thread.Sleep(1000);
            Response.Redirect(this.Request.Path);
            
        }

        protected void Messages_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            strConnectionString = Microsoft.WindowsAzure.ServiceRuntime.RoleEnvironment.GetConfigurationSettingValue("DataConnectionString");
            strConnectionString = string.Format("{0} ....", strConnectionString.Substring(0, Math.Min(strConnectionString.Length, 90)));  

        }


    }
}