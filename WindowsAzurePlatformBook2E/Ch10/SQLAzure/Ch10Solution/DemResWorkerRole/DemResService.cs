using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.ServiceModel;
using ProAzureDemResContract;
using Microsoft.WindowsAzure.ServiceRuntime;


namespace DemResWorkerRole
{
 [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
 public class DemResService : IDemResOperations
 {

  #region IDemResOperations Members

  public void SendValue(string gatewayId, double kWhValue, DateTime gatewayTime)
  {
   //Update the database table with the new value
   InsertEnergyMeterValues(gatewayId, kWhValue, gatewayTime);
   //Get the value from the database and curtail if total price > $1.0
   double cost = GetCostByGateway(gatewayId, kWhValue);

   //if (cost > 1.0)
   //{
    IDemResCallback callback = OperationContext.Current.GetCallbackChannel<IDemResCallback>();
    callback.Curtail(70);
   //}
   
  }

  private string GetUserDbString()
  {
   // Create a connection string for the sample database
   SqlConnectionStringBuilder connString2Builder = new SqlConnectionStringBuilder();
   string serverName = RoleEnvironment.GetConfigurationSettingValue("SQLAzure-ServerName"); 
   connString2Builder.DataSource = serverName;
   connString2Builder.InitialCatalog = RoleEnvironment.GetConfigurationSettingValue("SQLAzure-DatabaseName"); 
   connString2Builder.Encrypt = true;
   connString2Builder.TrustServerCertificate = true;
   connString2Builder.UserID = RoleEnvironment.GetConfigurationSettingValue("SQLAzure-UserName");
   connString2Builder.Password = RoleEnvironment.GetConfigurationSettingValue("SQLAzure-Password"); 

   return connString2Builder.ToString();

  }
  #endregion

  #region DB Functions
  private void InsertEnergyMeterValues(string gid, double kWh, DateTime gatewayTime)
  {

   try
   {
    // Connect to the sample database and perform various operations
    using (SqlConnection conn = new SqlConnection(GetUserDbString()))
    {
     conn.Open();
     
      using (SqlCommand command = conn.CreateCommand())
      {

       // Insert records
       command.CommandText = "InsertEnergyMeterValues";
       command.CommandType = CommandType.StoredProcedure;

      


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
       kWhFieldRecoredTime.Value = gatewayTime;
       command.Parameters.Add(kWhFieldRecoredTime);

       SqlParameter kWhServerTime = command.CreateParameter();
       kWhServerTime.ParameterName = "@kWhServerTime";
       kWhServerTime.Value = DateTime.Now;
       command.Parameters.Add(kWhServerTime);

       int rowsAdded = command.ExecuteNonQuery();




      }//using
   
    }

 

   }
   catch (Exception ex)
   {

    throw ex;

   }

  }
  private double GetCostByGateway(string gid, double kWh)
  {
   double cost = 0.0;
   try
   {
    // Connect to the sample database and perform various operations
    using (SqlConnection conn = new SqlConnection(GetUserDbString()))
    {
     conn.Open();

     using (SqlCommand command = conn.CreateCommand())
     {

      // Insert records
      command.CommandText = "GetEnergyCostByGatewayId";
      command.CommandType = CommandType.StoredProcedure;

      SqlParameter gatewayId = command.CreateParameter();
      gatewayId.ParameterName = "@gatewayId";
      gatewayId.Value = gid;
      command.Parameters.Add(gatewayId);

      using (IDataReader reader = command.ExecuteReader())
      {
       if (reader.Read())
       {

        if (!reader.IsDBNull(0))
        {
         cost = reader.GetDouble(0) * kWh;

        }

       }


      }//using




     }//using

    }


    return cost;
   }
   catch (Exception ex)
   {

    throw ex;

   }

  }
  #endregion
 }
}
