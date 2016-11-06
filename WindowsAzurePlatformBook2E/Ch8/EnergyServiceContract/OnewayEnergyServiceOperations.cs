using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Security.Principal;
using System.Threading;


namespace EnergyServiceContract
{


 [ServiceBehavior(Name = "OnewayEnergyServiceOperations", Namespace = "http://proazure/ServiceBus/energyservice/headend/")]
   public class OnewayEnergyServiceOperations : EnergyServiceContract.IOnewayEnergyServiceOperations
    {
     
     #region IOnewayEnergyServiceOperations Members

     public void SendKwhValue(string gatewayId, string meterId, double kwhValue, DateTime utcTime)
     {
         Console.WriteLine(String.Format("{0}>Energy Meter {1} value:{2:0.00} kWh @ {3}", gatewayId, meterId, kwhValue, utcTime.ToString("s")));
        // Console.WriteLine(GetClientCredentials());
     }

     public void SendLightingValue(string gatewayId, string switchId, int lightingValue, DateTime utcTime)
     {
         Console.WriteLine(String.Format("{0}>Changed lightbulb state of switch {1} to {2}", gatewayId, switchId, ((lightingValue == 1) ? "ON" : "OFF")));
        // Console.WriteLine(GetClientCredentials());
     }

     public void SendHVACSetPoint(string gatewayId, string hvacId, int setPointValue, DateTime utcTime)
     {
         Console.WriteLine(String.Format("{0}>HVAC {1} has SETPOINT value:{2:0} F @ {3}", gatewayId, hvacId, setPointValue, utcTime.ToString("s")));
        // Console.WriteLine(GetClientCredentials());
     }

     public void SendHVACMode(string gatewayId, string hvacId, int mode, DateTime utcTime)
     {
         Console.WriteLine(String.Format("{0}>HVAC {1} MODE is set to {2} @ {3}", gatewayId, hvacId, GetHVACModeString(mode), utcTime.ToString("s")));
        // Console.WriteLine(GetClientCredentials());
     }

     #endregion

     #region Message Security
     public static string GetClientCredentials()
     {
      StringBuilder creds = new StringBuilder();

      IPrincipal cp = Thread.CurrentPrincipal;
      ServiceSecurityContext currentContext = ServiceSecurityContext.Current;
      if (currentContext == null)
      {
       creds.AppendFormat("Token: {0}" +
                  "Principal: {1} Identity: {2} " +
                  "Security context is null",
                  WindowsIdentity.GetCurrent().Name + Environment.NewLine,
                  cp.Identity.GetType() + Environment.NewLine,
                  (cp.Identity.Name == "" ? "Identity is blank" : cp.Identity.Name + Environment.NewLine))
                  ;
      }
      else
      {
       creds.AppendFormat("Token: {0}" +
                  "Principal: {1} Identity: {2} " + Environment.NewLine +
                  "Primary identity: {3}  {4}" +
                  "Windows identity: {5}  {6}",
                  WindowsIdentity.GetCurrent().Name + Environment.NewLine,
                  cp.Identity.GetType(),
                  (cp.Identity.Name == "" ? "Identity is blank" : cp.Identity.Name + Environment.NewLine),
                  currentContext.PrimaryIdentity.GetType(),
                  (currentContext.PrimaryIdentity.Name == "" ? "Identity is blank" : currentContext.PrimaryIdentity.Name) + Environment.NewLine,
                  currentContext.WindowsIdentity.GetType(),
                 (currentContext.WindowsIdentity.Name == "" ? "Identity is blank" : currentContext.WindowsIdentity.Name)
                  );

      }
      return creds.ToString();
     }
     #endregion
     public static string GetHVACModeString(int hvacMode)
     {
         if (hvacMode == 0)
         {
             return "OFF";
         }
         else if (hvacMode == 1)
         {
             return "COLD";
         }
         else
         {
             return "HOT";

         }

     }
    }
}
