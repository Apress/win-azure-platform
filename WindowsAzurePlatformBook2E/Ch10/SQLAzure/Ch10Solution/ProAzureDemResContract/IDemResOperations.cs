using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Data;

namespace ProAzureDemResContract
{

 [ServiceContract(CallbackContract = typeof(IDemResCallback))]
 public interface IDemResOperations
 {
  [OperationContract]
  void SendValue(string gatewayId, double value, DateTime gatewayTime);
 }

 public interface IDemResCallback
 {
  [OperationContract]
  void Curtail(int setPointValue);
 }

  
 
}
