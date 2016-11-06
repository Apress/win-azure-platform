using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace ADONETConnection
{
 public partial class Form1 : Form
 {
 
  public Form1()
  {
   InitializeComponent();
  }

  private void btnConnect_Click(object sender, EventArgs e)
  {

  }

  private void CreateTable()
  {

   try
   {
    // Connect to the sample database and perform various operations
    using (SqlConnection conn = new SqlConnection(GetUserDbString()))
    {
     using (SqlCommand command = conn.CreateCommand())
     {
      conn.Open();

      // Create table
      command.CommandText = "CREATE TABLE MyTable1(Column1 int primary key clustered, Column2 varchar(50), Column3 datetime)";
      command.ExecuteNonQuery();

      // Insert records
      command.CommandText = String.Format("INSERT INTO MyTable1 (Column1, Column2, Column3) values ({0}, '{1}', '{2}')", 1, "TestData", DateTime.Now.ToString("s"));
      int rowsAdded = command.ExecuteNonQuery();

      DisplayResults(command);


      // Update a record
      command.CommandText = "UPDATE MyTable1 SET Column2='Updated String' WHERE Column1=1";
      command.ExecuteNonQuery();
      AddText("UPDATED RECORD");
      DisplayResults(command);

      // Delete a record
      command.CommandText = "DELETE FROM MyTable1 WHERE Column1=1";
      command.ExecuteNonQuery();

      AddText("DELETED RECORD");
      DisplayResults(command);


     }//using
    }

   }
   catch (Exception ex)
   {

    MessageBox.Show(ex.Message, "Error in Create Table", MessageBoxButtons.OK, MessageBoxIcon.Error);

   }

  }

  private void DisplayResults(SqlCommand command)
  {

   command.CommandText = "SELECT Column1, Column2, Column3 FROM MyTable1";

   using (SqlDataReader reader = command.ExecuteReader())
   {
    // Loop over the results
    while (reader.Read())
    {
     AddText(command.CommandText);
     AddText(String.Format("Column1: {0}, Column2: {1}, Column3: {2}",
                     reader["Column1"].ToString().Trim(),
                     reader["Column2"].ToString().Trim(),
                     reader["Column3"].ToString().Trim()));
     AddText("\n");
    }
   }
  }

  private void DropTable()
  {
   try
   {

    using (SqlConnection conn = new SqlConnection(GetUserDbString()))
    {
     using (SqlCommand command = conn.CreateCommand())
     {
      conn.Open();

      // Create table
      command.CommandText = "DROP TABLE MyTable1";
      command.ExecuteNonQuery();

     }//using
    }

    AddText("DROPPED TABLE");
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
   connString1Builder.DataSource = serverName + "." + domain;
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
   connString2Builder.DataSource = serverName + "." + domain; ;
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
   CreateTable();
  }

  private void btnDropTable_Click(object sender, EventArgs e)
  {
   DropTable();
  }
 }
}
