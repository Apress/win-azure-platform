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

namespace ProAzureReaderTracker_WebRole
{
 public partial class _Default : System.Web.UI.Page
 {
  protected void Page_Load(object sender, EventArgs e)
  {
   if (!Page.IsPostBack)
   {

    BindTodaysData();
   }
   CalendarExtender1.SelectedDate = DateTime.Today;
   txtPurchaseDateFilter_CalendarExtender.SelectedDate = DateTime.Today;
  }

 

 

  

  protected void btnSubmit_Click(object sender, EventArgs e)
  {
   try
   {
    ProAzureReader newReader = new ProAzureReader()
    {
     City = txtCity.Text,
     Country = txtCountry.Text,
     Feedback = txtFeedback.Text,
     PurchaseDate = DateTime.Parse(txtPurchaseDate.Text),
     PurchaseType = ddlPurchaseType.SelectedItem.Text,
     PurchaseLocation = txtPurchaseLocation.Text,
     ReaderName = txtName.Text,
     ReaderUrl = txtUrl.Text,
     State = txtState.Text,
     Zip = txtZip.Text


    };
    ProAzureReaderDataSource ds = new ProAzureReaderDataSource();
    ds.AddProAzureReader(newReader);
    //dlReaders.DataBind();
    BindTodaysData();
   }
   catch (Exception ex)
   {

    lblStatus.Text = "Error adding entry " + ex.Message;
   }
  }

  protected void lbFilterByCity_Click(object sender, EventArgs e)
  {
   if (txtFilter.Text.Length > 0)
   {
    try
    {
     ProAzureReaderDataSource ds = new ProAzureReaderDataSource();
     dlReaders.DataSource = ds.SelectByCity(txtFilter.Text);
     dlReaders.DataBind();
    }
    catch (Exception ex)
    {

     lblStatus.Text = "Error:" + ex.Message;
    }
   }
   else
   {
    lblStatus.Text = "Please enter a City in the filter text";
   }
  }

  protected void lbFilterByState_Click(object sender, EventArgs e)
  {
   if (txtFilter.Text.Length > 0)
   {
    try
    {
     ProAzureReaderDataSource ds = new ProAzureReaderDataSource();
     dlReaders.DataSource = ds.SelectByState(txtFilter.Text);
     dlReaders.DataBind();
    }
    catch (Exception ex)
    {

     lblStatus.Text = "Error:" + ex.Message;
    }
   }
   else
   {
    lblStatus.Text = "Please enter a State in the filter text";
   }
  }

  protected void lbFilterByCountry_Click(object sender, EventArgs e)
  {
   if (txtFilter.Text.Length > 0)
   {
    try
    {
     ProAzureReaderDataSource ds = new ProAzureReaderDataSource();
     dlReaders.DataSource = ds.SelectByCountry(txtFilter.Text);
     dlReaders.DataBind();
    }
    catch (Exception ex)
    {

     lblStatus.Text = "Error:" + ex.Message;
    }
   }
   else
   {
    lblStatus.Text = "Please enter a Country in the filter text";
   }
  }

  protected void lbFilterByPurchaseDate_Click(object sender, EventArgs e)
  {
   if (txtPurchaseDateFilter.Text.Length > 0)
   {
    try
    {
     ProAzureReaderDataSource ds = new ProAzureReaderDataSource();
     dlReaders.DataSource = ds.SelectByPurchaseDate(DateTime.Parse(txtPurchaseDateFilter.Text));
     dlReaders.DataBind();
    }
    catch (Exception ex)
    {

     lblStatus.Text = "Error:" + ex.Message;
    }
   }
   else
   {
    lblStatus.Text = "Please enter a Purchase Date in the filter text";
   }
  }

  protected void lbToday_Click(object sender, EventArgs e)
  {
   BindTodaysData();
  }

  private void BindTodaysData()
  {
   ProAzureReaderDataSource ds = new ProAzureReaderDataSource();
   dlReaders.DataSource = ds.Select();
   dlReaders.DataBind();
  }

  protected void lbTop50_Click(object sender, EventArgs e)
  {
   int n = 50;
   if (txtFilter.Text.Length > 0)
   {
    try
    {
     n = int.Parse(txtFilter.Text);
    }
    catch (Exception)
    {
     lblStatus.Text = "Could not parse number in Filter text box, so using TOP " + n;

    }
   }
   try
   {

    ProAzureReaderDataSource ds = new ProAzureReaderDataSource();
    dlReaders.DataSource = ds.SelectTopN(n);
    dlReaders.DataBind();
   }
   catch (Exception ex)
   {

    lblStatus.Text = "Error:" + ex.Message;
   }

  }

  protected void dlReaders_ItemCommand(object source, DataListCommandEventArgs e)
  {
   string commandArg = e.CommandArgument as string;
   string commandName = e.CommandName;
   DataListItem i = e.Item;
   TextBox tb = i.FindControl("txtUrl") as TextBox;

   if (tb != null && tb.Text.Length > 0)
   {
    string url = tb.Text;
    ProAzureReaderDataSource ds = new ProAzureReaderDataSource();
    ds.UpdateUrl(commandArg, commandName, url);
   }
  }




 }
}
