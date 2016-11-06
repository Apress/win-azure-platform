using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace ProAzureDemResDbApp
{
 public partial class Form1 : Form
 {
     const int START_LOCATIONID = 95147;
     const int END_LOCATIONID = 95483;

     const string GATEWAYID_FORMAT = "MyGateway{0}";
     const int START_GATEWAYID = 1;
     const int END_GATEWAYID = 300;
     const string WEB_ADDRESS_FORMAT = "sb://proazure1.servicebus.windows.net/gateways/{0}/{1}";
     DateTime PRICINGCALENDAR_STARTDATE = DateTime.Now;
     DateTime PRICINGCALENDAR_ENDDATE = DateTime.Now.AddDays(120);

    
  public Form1()
  {
   InitializeComponent();
  }

  private void btnConnect_Click(object sender, EventArgs e)
  {

  }

  private void InsertPricingLocations()
  {

   try
   {
    // Connect to the sample database and perform various operations
    using (SqlConnection conn = new SqlConnection(GetUserDbString()))
    {
     conn.Open();

     for (int i = START_LOCATIONID; i < END_LOCATIONID; i++)
     {
      using (SqlCommand command = conn.CreateCommand())
      {




       // Insert records
       command.CommandText = "InsertPricingLocations";
       command.CommandType = CommandType.StoredProcedure;


       string lid = i.ToString();
       string desc = "Description-" + lid;
       SqlParameter locationId = command.CreateParameter();
       locationId.ParameterName = "@locationId";
       locationId.Value = lid;
       command.Parameters.Add(locationId);

       SqlParameter description = command.CreateParameter();
       description.ParameterName = "@description";
       description.Value = desc;
       command.Parameters.Add(description);

       int rowsAdded = command.ExecuteNonQuery();




      }//using
     }//for
    }
    AddText("Added Picing Locations");

   }
   catch (Exception ex)
   {

    MessageBox.Show(ex.Message, "Error in Create Table", MessageBoxButtons.OK, MessageBoxIcon.Error);

   }

  }

  private void InsertPricingCalendar()
  {

   try
   {
    // Connect to the sample database and perform various operations
    using (SqlConnection conn = new SqlConnection(GetUserDbString()))
    {
     conn.Open();
     
      for (int j = START_LOCATIONID; j < END_LOCATIONID; j++)
      {
       using (SqlCommand command = conn.CreateCommand())
       {
        
        // Insert records
        command.CommandText = "InsertPricingCalendar_kWh";
        command.CommandType = CommandType.StoredProcedure;

        
        string lid = j.ToString();
        Random r = new Random();
        double price = r.NextDouble();

        SqlParameter pricingStartDate = command.CreateParameter();
        pricingStartDate.ParameterName = "@pricingStartDate";
        pricingStartDate.Value = PRICINGCALENDAR_STARTDATE;
        command.Parameters.Add(pricingStartDate);

        SqlParameter pricingEndDate = command.CreateParameter();
        pricingEndDate.ParameterName = "@pricingEndDate";
        pricingEndDate.Value = PRICINGCALENDAR_ENDDATE;
        command.Parameters.Add(pricingEndDate);

        SqlParameter pricePerkWh = command.CreateParameter();
        pricePerkWh.ParameterName = "@pricePerkWh";
        pricePerkWh.Value = price;
        command.Parameters.Add(pricePerkWh);

        SqlParameter locationId = command.CreateParameter();
        locationId.ParameterName = "@locationId";
        locationId.Value = lid;
        command.Parameters.Add(locationId);

        command.ExecuteNonQuery();




       }//using
      
     }//for
    }
    AddText("Added Pricing Calendar");

   }
   catch (Exception ex)
   {

    MessageBox.Show(ex.Message, "Error in Create Table", MessageBoxButtons.OK, MessageBoxIcon.Error);

   }

  }
  private void InsertGateways()
  {

      try
      {
          // Connect to the sample database and perform various operations
          using (SqlConnection conn = new SqlConnection(GetUserDbString()))
          {
           conn.Open();
            int locid = START_LOCATIONID;
              for (int i = START_GATEWAYID; i < END_GATEWAYID; i++)
              {
                  using (SqlCommand command = conn.CreateCommand())
                  {
                      
                      // Insert records
                      command.CommandText = "InsertGateway";
                      command.CommandType = CommandType.StoredProcedure;
                      locid = locid + 1;

                      string lid = ((locid <= END_LOCATIONID) ? locid : START_LOCATIONID).ToString();
                      string gid = String.Format(GATEWAYID_FORMAT, i);
                   
                      string wa = String.Format(WEB_ADDRESS_FORMAT, lid, gid);

                      SqlParameter gatewayId = command.CreateParameter();
                      gatewayId.ParameterName = "@gatewayId";
                      gatewayId.Value = gid;
                      command.Parameters.Add(gatewayId);

                      SqlParameter locationId = command.CreateParameter();
                      locationId.ParameterName = "@locationId";
                      locationId.Value = lid;
                      command.Parameters.Add(locationId);

                      SqlParameter webAddress = command.CreateParameter();
                      webAddress.ParameterName = "@webAddress";
                      webAddress.Value = wa;
                      command.Parameters.Add(webAddress);

                      int rowsAdded = command.ExecuteNonQuery();

                      AddText(String.Format("Added Gateway GatewayId:{0}, LocationId:{1}, WebAddress:{2}\n", gid, lid, wa));



                  }//using
              }//for
          }

      }
      catch (Exception ex)
      {

          MessageBox.Show(ex.Message, "Error in Create Table", MessageBoxButtons.OK, MessageBoxIcon.Error);

      }

  }
  private void InsertEnergyMeterValues()
  {

   try
   {
    // Connect to the sample database and perform various operations
    using (SqlConnection conn = new SqlConnection(GetUserDbString()))
    {
     conn.Open();
     for (int i = START_GATEWAYID; i < END_GATEWAYID; i++)
     {
      using (SqlCommand command = conn.CreateCommand())
      {
       
       // Insert records
       command.CommandText = "InsertEnergyMeterValues";
       command.CommandType = CommandType.StoredProcedure;

       Random r = new Random();
       double kWh = r.NextDouble() * 5.2D;

       string gid = String.Format(GATEWAYID_FORMAT, i);
       

       SqlParameter gatewayId = command.CreateParameter();
       gatewayId.ParameterName = "@gatewayId";
       gatewayId.Value = gid;
       command.Parameters.Add(gatewayId);

       SqlParameter kWhValue = command.CreateParameter();
       kWhValue.ParameterName = "@kWhValue";
       kWhValue.Value = kWh;
       command.Parameters.Add(kWhValue);

       SqlParameter kWhFieldRecoredTime = command.CreateParameter();
       kWhFieldRecoredTime.ParameterName = "@kWhFieldRecoredTime";
       kWhFieldRecoredTime.Value = DateTime.Now;
       command.Parameters.Add(kWhFieldRecoredTime);

       SqlParameter kWhServerTime = command.CreateParameter();
       kWhServerTime.ParameterName = "@kWhServerTime";
       kWhServerTime.Value = DateTime.Now;
       command.Parameters.Add(kWhServerTime);

       int rowsAdded = command.ExecuteNonQuery();




      }//using
     }//for
    }

    AddText("Added Energy Meter Values");

   }
   catch (Exception ex)
   {

    MessageBox.Show(ex.Message, "Error in Create Table", MessageBoxButtons.OK, MessageBoxIcon.Error);

   }

  }

 
  private void DropData()
  {
   try
   {

    using (SqlConnection conn = new SqlConnection(GetUserDbString()))
    {
     conn.Open();
     using (SqlCommand command = conn.CreateCommand())
     {
      

      // Create table
      command.CommandText = "DELETE FROM EnergyMeterValues";
      command.ExecuteNonQuery();

      command.CommandText = "DELETE FROM Gateways";
      command.ExecuteNonQuery();

      command.CommandText = "DELETE FROM PricingCalendar_kWh";
      command.ExecuteNonQuery();

      command.CommandText = "DELETE FROM PricingLocations";
      command.ExecuteNonQuery();

     }//using
    }

    AddText("DROPPED DATA");
   }
   catch (Exception ex)
   {

    MessageBox.Show(ex.Message, "Error in Drop Table", MessageBoxButtons.OK, MessageBoxIcon.Error);

   }
  }

  private string GetMasterDbConnectionString()
  {
   SqlConnectionStringBuilder connString1Builder = new SqlConnectionStringBuilder();

   string serverName = txtServerName.Text;
   string domain = txtDomainName.Text;
   connString1Builder.DataSource = (!string.IsNullOrEmpty(domain) ? serverName + "." + domain : serverName);
   connString1Builder.InitialCatalog = "master";
   connString1Builder.Encrypt = true;
   connString1Builder.TrustServerCertificate = true;
   connString1Builder.UserID = txtUserName.Text;
   connString1Builder.Password = txtPassword.Text;

   return connString1Builder.ToString();

  }

  private string GetUserDbString()
  {
   // Create a connection string for the sample database
   SqlConnectionStringBuilder connString2Builder = new SqlConnectionStringBuilder();
   string serverName = txtServerName.Text;
   string domain = txtDomainName.Text;
   connString2Builder.DataSource = (!string.IsNullOrEmpty(domain) ? serverName + "." + domain : serverName);
   connString2Builder.InitialCatalog = txtDatabase.Text;
   connString2Builder.Encrypt = true;
   connString2Builder.TrustServerCertificate = true;
   connString2Builder.UserID = txtUserName.Text;
   connString2Builder.Password = txtPassword.Text;

   return connString2Builder.ToString();

  }


  private void AddText(string text)
  {

   txtResult.AppendText(text + "\n");
  }

  private void btnCreateTable_Click(object sender, EventArgs e)
  {
      InsertPricingLocations();
      InsertPricingCalendar();
      InsertGateways();
      InsertEnergyMeterValues();
  }

  private void btnDropTable_Click(object sender, EventArgs e)
  {
   DropData();
  }
 }
}
