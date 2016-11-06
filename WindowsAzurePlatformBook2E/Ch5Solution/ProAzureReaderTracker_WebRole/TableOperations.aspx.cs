using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using Microsoft.WindowsAzure.StorageClient.Protocol;
using Microsoft.WindowsAzure.StorageClient.Tasks;
using ProAzureTableStorageClasses;
using System.Data.Services.Client;
using ProAzureCommonLib;

namespace ProAzureReaderTracker_WebRole
{
 public partial class TableOperations : System.Web.UI.Page
 {
  protected void Page_Load(object sender, EventArgs e)
  {
   if (!this.IsPostBack)
   {
    ListTables();
   }
  }

  protected void btnCreateTable_Click(object sender, EventArgs e)
  {
   CreateTable();
  }

  protected void btnDeleteTable_Click(object sender, EventArgs e)
  {
   DeleteTable();
  }


  private WAStorageHelper GetWAStorageHelper()
  {
      return new WAStorageHelper("StorageAccountConnectionString");

  }
  private void CreateTable()
  {

   string statusMessage = String.Empty;

   if (txtCreateTable.Text.Length > 0)
   {
    try
    {


        WAStorageHelper storageHelper = GetWAStorageHelper();
     storageHelper.CreateTable(txtCreateTable.Text);
     statusMessage = "Table created successfully.";
     ListTables();

    }
    catch (DataServiceRequestException ex)
    {
     statusMessage = "Unable to connect to the table storage server. Please check that the service is running.<br>"
                     + ex.Message;
    }
   }
   else
   {
    statusMessage = "Please enter a table name.";
   }
   lblStatus.Text = statusMessage;

  }

  

  

  private void DeleteTable()
  {

   string statusMessage = String.Empty;

   if (txtDeleteTable.Text.Length > 0)
   {
    try
    {

        WAStorageHelper storageHelper = GetWAStorageHelper();
     storageHelper.DeleteTable(txtDeleteTable.Text);
     statusMessage = "Table deleted successfully.";
     ListTables();

    }
    catch (DataServiceRequestException ex)
    {
     statusMessage = "Unable to connect to the table storage server. Please check that the service is running.<br>"
                     + ex.Message;
    }
   }
   else
   {
    statusMessage = "Please select a table name to delete.";
   }
   lblStatus.Text = statusMessage;

  }

  private void ListTables()
  {

   string statusMessage = String.Empty;

  
    try
    {

        WAStorageHelper storageHelper = GetWAStorageHelper();
    lbListTables.DataSource = storageHelper.ListTables();
    lbListTables.DataBind();
   

    }
    catch (DataServiceRequestException ex)
    {
     statusMessage = "Unable to connect to the table storage server. Please check that the service is running.<br>"
                     + ex.Message;
    }
  
   lblStatus.Text = statusMessage;

  }

  protected void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
  {
   txtDeleteTable.Text = lbListTables.SelectedItem.Text;
  }

 
 }
}
