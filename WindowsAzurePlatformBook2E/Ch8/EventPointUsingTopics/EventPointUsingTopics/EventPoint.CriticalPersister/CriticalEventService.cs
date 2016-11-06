using System;
using System.ServiceModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using System.Data;
using EventPoint.Common; 


namespace EventPoint.CriticalPersister
{

    [ServiceBehavior(Name = "CriticalEvent", Namespace="sb://eventpoint/relay")]
    public class CriticalEventService : EventPoint.Common.ICriticalEvent
    {
        static SqlConnection connection;

        static void OpenConnection() 
        {
            int sqlMaxRetries = 4;
            int sqlRetrySleep = 100;
            int sqlMaxSleep = 5000;
            int sqlMinSleep = 10;
            int retryCount = 0;

            while (retryCount < sqlMaxRetries)
            {
                try
                {
                    connection = new SqlConnection(Config.sqlConnectionString);
                    connection.Open();
                    break;
                }

                catch (SqlException)
                {
                    //SqlConnection.ClearPool(connection);

                    while (retryCount < sqlMaxRetries)
                    {

                        // Don't sleep on the first retry, as most SQL Azure retries work on the first retry with no sleep
                        if (retryCount > 0)
                        {
                            // wait longer between each retry
                            int sleep = (int)Math.Pow(retryCount + 1, 2.0) * sqlRetrySleep;

                            // limit to the min and max retry values
                            if (sleep > sqlMaxSleep)
                            {
                                sleep = sqlMaxSleep;
                            }
                            else if (sleep < sqlMinSleep)
                            {
                                sleep = sqlMinSleep;
                            }

                            // sleep
                            System.Threading.Thread.Sleep(sleep);
                        }
                    }

                }
            }
        }

        public void RegisterAlert(EventPoint.Common.EventMessage eventMsg)
        {
            if (connection == null)
            {
                OpenConnection();
            }

            Debug.WriteLine("***>>> RegisterAlert called...");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = connection;
            cmd.CommandText = "PersistAlert";
            cmd.Parameters.AddWithValue("@Link", eventMsg.Link);
            cmd.Parameters.AddWithValue("@Message", eventMsg.Message);
            cmd.Parameters.AddWithValue("@Originator", eventMsg.Originator);
            cmd.Parameters.AddWithValue("@Priority", eventMsg.Priority);
            cmd.Parameters.AddWithValue("@Title", eventMsg.Title);

            try
            {
                int ret = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                if (connection.State != ConnectionState.Open)
                {
                    OpenConnection();
                }

                int tries = 0;
                while (tries++ < 10)
                {
                    if (connection.State == ConnectionState.Connecting)
                        System.Threading.Thread.Sleep(3000);
                }

                if (connection.State == ConnectionState.Open)
                {
                    int ret = cmd.ExecuteNonQuery();
                }
                else
                {
                    Trace.WriteLine("SQL Connection error. Please check the connection string, and if using SQL Azure, ensure you have created a firewall rule for your current IP address");
                    Trace.WriteLine(string.Format("Error Message: {0}", ex.Message));
                    throw;
                }
            }

        }

    }

}
