using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Microsoft.ServiceBus;

namespace EnergyServiceContract
{
 [ServiceBehavior(Name = "MulticastGatewayOperations", Namespace = "http://proazure/ServiceBus/energyservice/")]
 public class MulticastGatewayOperations : EnergyServiceContract.IMulticastGatewayOperations
 {
  #region IMulticastGatewayOperations Members

  public void Online(string gatewayId, string serviceUri, DateTime utcTime)
  {
   Console.WriteLine(String.Format("{0}>ONLINE Uri:{1} @ {2}", gatewayId, serviceUri, utcTime.ToString("s")));

  }

  public void GoingOffline(string gatewayId, string serviceUri, DateTime utcTime)
  {
      Console.WriteLine(String.Format("{0}>OFFLINE Uri:{1} @ {2}", gatewayId, serviceUri, utcTime.ToString("s")));

  }

  #endregion
 }
}
