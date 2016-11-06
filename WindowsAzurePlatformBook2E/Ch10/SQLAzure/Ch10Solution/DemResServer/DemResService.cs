using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using ProAzureDemResContract;

namespace DemResServer
{
 [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
 public class DemResService : IDemResOperations
 {

  #region IDemResOperations Members

  public void SendValue(string gatewayId, double value, DateTime gatewayTime)
  {
   //Update the database table with the new value
   IDemResCallback callback = OperationContext.Current.GetCallbackChannel<IDemResCallback>();
   //Get the value from the database and curtail if total price
   callback.Curtail(1);
  }

  #endregion
 }
}
