using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using EventPoint.Common;

namespace EventPoint.CriticalPersister
{
    public static class DatabaseArtifacts
    {
        public static bool CreateDatabase()
        {

            SqlConnectionStringBuilder connStringBuilder;
            connStringBuilder = new SqlConnectionStringBuilder(Config.sqlConnectionString);
            connStringBuilder.InitialCatalog = "master";
            connStringBuilder.Encrypt = true;
            connStringBuilder.TrustServerCertificate = false;

            string sqlAzureUserName = connStringBuilder.UserID;
            string sqlAzureUserPassword = connStringBuilder.Password;
            string sqlAzureServerName = Config.sqlConnectionString;

            int svrPos = sqlAzureServerName.IndexOf("Server=");
            sqlAzureServerName = sqlAzureServerName.Substring(svrPos+7);
            sqlAzureServerName = sqlAzureServerName.Substring(sqlAzureServerName.IndexOf("tcp:")+4, sqlAzureServerName.IndexOf(".database")-4);


//<add key="SqlConnectionString" value="Server=tcp:x1krar7kep.database.windows.net;Initial Catalog=EventPoint;User ID=brloes@x1krar7kep;Password=pass@word1;Trusted_Connection=False;Encrypt=True"/>


            // Connect to the master database and create the sample database
            using (SqlConnection connection = new SqlConnection(connStringBuilder.ToString()))
            {
                using (SqlCommand command = connection.CreateCommand())
                {

                    connection.Open();

                    // Create the sample database
                    string cmdText = String.Format("CREATE DATABASE {0}","EventPoint");
                    command.CommandText = cmdText;
                    command.ExecuteNonQuery();

                    // create login
                    command.CommandText = string.Format("CREATE LOGIN [{0}] WITH PASSWORD=N'{1}'",sqlAzureUserName,sqlAzureUserPassword);
                    command.ExecuteNonQuery();

                    // assign role
                    command.CommandText = string.Format("EXEC sp_addrolemember 'db_owner', '{0}'",sqlAzureUserName);
                    command.ExecuteNonQuery();

                    connection.Close();
                }
            }


            connStringBuilder.InitialCatalog = "EventPoint";
            using (SqlConnection conn = new SqlConnection(connStringBuilder.ToString()))
            {
                using (SqlCommand command = conn.CreateCommand())
                {
                    conn.Open();

                    StringBuilder sb = new StringBuilder();

                    sb.Append("CREATE TABLE [dbo].[CriticalAlerts](");
                    sb.Append("[ID] [int] IDENTITY(1,1) NOT NULL,");
                    sb.Append("[Timestamp] [nvarchar](50) NULL,");
                    sb.Append("[Link] [nvarchar](50) NULL,");
                    sb.Append("[Message] [nvarchar](50) NULL,");
                    sb.Append("[Originator] [nvarchar](50) NULL,");
                    sb.Append("[Priority] [nvarchar](50) NULL,");
                    sb.Append("[Title] [nvarchar](50) NULL,");
                    sb.Append("CONSTRAINT [PK_dbo.CriticalAlerts] PRIMARY KEY CLUSTERED ");
                    sb.Append("(");
                    sb.Append("[ID] ASC");
                    sb.Append(")WITH (STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF)");
                    sb.Append(")");

                    command.CommandText = sb.ToString();
                    command.ExecuteNonQuery();

                }
            }



            //SqlCommand cmd = new SqlCommand();
            //cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Connection = connection;

            //try
            //{
            //    int ret = cmd.ExecuteNonQuery();
            //}
            //catch (Exception)
            //{
            //    int tries = 0;
            //    while (tries++ < 10)
            //    {
            //        if (connection.State == ConnectionState.Connecting)
            //            System.Threading.Thread.Sleep(1000);
            //    }

            //    Trace.WriteLine("SQL Connection error. Please check the connection string, and if using SQL Azure, ensure you have created a firewall rule for your current IP address");
            //    throw;
            //}


            return true;
        }
    }
}


#region sample


namespace Microsoft.SDS.Samples
{
    class Program
    {
        // Provide the following information
        private static string userName = "<ProvideUserName>";
        private static string password = "<ProvidePassword>";
        private static string dataSource = "<ProvideServerName>";
        private static string sampleDatabaseName = "<ProvideDatabaseName>";

        static void Main(string[] args)
        {
            // Create a connection string for the master database
            SqlConnectionStringBuilder connString1Builder;
            connString1Builder = new SqlConnectionStringBuilder();
            connString1Builder.DataSource = dataSource;
            connString1Builder.InitialCatalog = "master";
            connString1Builder.Encrypt = true;
            connString1Builder.TrustServerCertificate = false;
            connString1Builder.UserID = userName;
            connString1Builder.Password = password;

            // Create a connection string for the sample database
            SqlConnectionStringBuilder connString2Builder;
            connString2Builder = new SqlConnectionStringBuilder();
            connString2Builder.DataSource = dataSource;
            connString2Builder.InitialCatalog = sampleDatabaseName;
            connString2Builder.Encrypt = true;
            connString2Builder.TrustServerCertificate = false;
            connString2Builder.UserID = userName;
            connString2Builder.Password = password;

            // Connect to the master database and create the sample database
            using (SqlConnection conn = new SqlConnection(connString1Builder.ToString()))
            {
                using (SqlCommand command = conn.CreateCommand())
                {

                    conn.Open();

                    // Create the sample database
                    string cmdText = String.Format("CREATE DATABASE {0}",
                                                    sampleDatabaseName);
                    command.CommandText = cmdText;
                    command.ExecuteNonQuery();
                    conn.Close();
                }
            }

            // Connect to the sample database and perform various operations
            using (SqlConnection conn = new SqlConnection(connString2Builder.ToString()))
            {
                using (SqlCommand command = conn.CreateCommand())
                {
                    conn.Open();

                    // Create a table
                    command.CommandText = "CREATE TABLE T1(Col1 int primary key, Col2 varchar(20))";
                    command.ExecuteNonQuery();

                    // Insert sample records
                    command.CommandText = "INSERT INTO T1 (col1, col2) values (1, 'string 1'), (2, 'string 2'), (3, 'string 3')";
                    int rowsAdded = command.ExecuteNonQuery();

                    // Query the table and print the results
                    command.CommandText = "SELECT * FROM T1";

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Loop over the results
                        while (reader.Read())
                        {
                            Console.WriteLine("Col1: {0}, Col2: {1}",
                                            reader["Col1"].ToString().Trim(),
                                            reader["Col2"].ToString().Trim());
                        }
                    }

                    // Update a record
                    command.CommandText = "UPDATE T1 SET Col2='string 1111' WHERE Col1=1";
                    command.ExecuteNonQuery();

                    // Delete a record
                    command.CommandText = "DELETE FROM T1 WHERE Col1=2";
                    command.ExecuteNonQuery();

                    // Query the table and print the results

                    Console.WriteLine("\nAfter update/delete the table has these records...");

                    command.CommandText = "SELECT * FROM T1";

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Loop over the results
                        while (reader.Read())
                        {
                            Console.WriteLine("Col1: {0}, Col2: {1}",
                                            reader["Col1"].ToString().Trim(),
                                            reader["Col2"].ToString().Trim());
                        }
                    }

                    conn.Close();
                }
            }
            Console.WriteLine("Press enter to continue...");
            Console.ReadLine();
        }
    }
}


#endregion