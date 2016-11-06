using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.ServiceBus;
using System.ServiceModel;
using System.Security.Principal;
using System.Threading;
using System.ServiceModel.Description;
using EnergyServiceContract;
using System.Net;
using System.Web;
using System.IO;
using System.ServiceModel.Syndication;
using System.Xml;


namespace ServiceBusUtils
{
    public sealed class ServiceBusHelper
    {
     #region NetOnewayRelay Binding
     public static ChannelFactory<IOnewayEnergyServiceChannel> GetOnewayChannelFactory(string solutionName)
        {
           

           //return new ChannelFactory<IOnewayEnergyServiceChannel>("RelayEndpoint", new EndpointAddress(serviceUri));
            return new ChannelFactory<IOnewayEnergyServiceChannel>("RelayEndpoint");

        
        }

        public static IOnewayEnergyServiceChannel GetOneWayEnergyChannel(ChannelFactory<IOnewayEnergyServiceChannel> channelFactory)
        {

            IOnewayEnergyServiceChannel channel = channelFactory.CreateChannel();
            Console.WriteLine("Opening Channel.");
            channel.Open();
            return channel;

        }

        public static void CloseoneWayChannelFactoryAndChannel(ChannelFactory<IOnewayEnergyServiceChannel> channelFactory, IOnewayEnergyServiceChannel channel)
        {

             channel.Close();

            channelFactory.Close();

        }

        public static void SendLightingValue(IOnewayEnergyServiceChannel netOnewayChannel, string gatewayId, string switchId, int lightingValue)
        {
         netOnewayChannel.SendLightingValue(gatewayId, switchId, lightingValue, DateTime.UtcNow);

        }

        public static void SendKwhValue(IOnewayEnergyServiceChannel netOnewayChannel, string gatewayId, string meterId, double kWhValue)
        {
         netOnewayChannel.SendKwhValue(gatewayId, meterId, kWhValue, DateTime.UtcNow);

        }

        public static void SendHVACModeValue(IOnewayEnergyServiceChannel netOnewayChannel, string gatewayId, string hvacId, int hvacValue)
        {
            netOnewayChannel.SendHVACMode(gatewayId, hvacId, hvacValue, DateTime.UtcNow);

        }

        public static void SendHVACSetPointValue(IOnewayEnergyServiceChannel netOnewayChannel, string gatewayId, string hvacId, int setPointValue)
        {
            netOnewayChannel.SendHVACSetPoint(gatewayId, hvacId, setPointValue, DateTime.UtcNow);

        }

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
     #endregion

     #region Relay Authentication
        public static TransportClientEndpointBehavior GetUsernamePasswordBehavior(string issuerName, string issuerKey)
        {
            TransportClientEndpointBehavior sharedSecretServiceBusCredential = new TransportClientEndpointBehavior();
            sharedSecretServiceBusCredential.TokenProvider = SharedSecretTokenProvider.CreateSharedSecretTokenProvider(issuerName, issuerKey);

            return sharedSecretServiceBusCredential;
        }

        public static string GetServiceBusSolutionName()
        {
         string serviceBusSolutionName = Environment.GetEnvironmentVariable("RELAYSAMPLESOLUTION");
         while (string.IsNullOrEmpty(serviceBusSolutionName))
         {
          Console.Write("Please enter the Service Namespace name to use for this sample:");
          serviceBusSolutionName = Console.ReadLine();
         }
         return serviceBusSolutionName;
        }

        public static string ReadPassword()
        {
         StringBuilder sbPassword = new StringBuilder();

         ConsoleKeyInfo info = Console.ReadKey(true);
         while (info.Key != ConsoleKey.Enter)
         {
          if (info.Key == ConsoleKey.Backspace)
          {
           if (sbPassword.Length != 0)
           {
            sbPassword.Remove(sbPassword.Length - 1, 1);
            Console.Write("\b \b");     // erase last char
           }
          }
          else if (info.KeyChar >= ' ')           // no control chars
          {
           sbPassword.Append(info.KeyChar);
           Console.Write("*");
          }
          info = Console.ReadKey(true);
         }

         Console.WriteLine();

         return sbPassword.ToString();
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
          creds.AppendFormat( "Token identity: {0}" +  
                     "Principal identity: {1} with {2} Security context is null", 
                     WindowsIdentity.GetCurrent().Name + Environment.NewLine,
                     cp.Identity.GetType(),
                     (cp.Identity.Name == "" ? "Blank identity" : cp.Identity.Name + Environment.NewLine))
                     ;
         }
         else
         {
          creds.AppendFormat("Token identity: {0}" +
                     "Principal identity: {1} with {2} Security context is null" + Environment.NewLine +
                     "Security context primary identity: {3} with {4}" +
                     "Security context Windows identity: {5} with {6}",
                     WindowsIdentity.GetCurrent().Name + Environment.NewLine,
                     cp.Identity.GetType(),
                     (cp.Identity.Name == "" ? "Blank identity" : cp.Identity.Name + Environment.NewLine),
                     currentContext.PrimaryIdentity.GetType(),
                     (currentContext.PrimaryIdentity.Name == "" ? "Blank identity" : currentContext.PrimaryIdentity.Name) + Environment.NewLine,
                     currentContext.WindowsIdentity.GetType(),
                    (currentContext.WindowsIdentity.Name == "" ? "Blank identity" : currentContext.WindowsIdentity.Name)
                     );

         }
         return creds.ToString();
        }
        #endregion

        public static string GetGatewayServicePath(string gatewayId)
        {

            return String.Format("Gateway/{0}", gatewayId);

        }

     #region Queue Helper
        public static string GetRESTAuthenticationTokenFromACS(string solutionName, string password)
        {
         string acsUri = string.Format(
             "https://{0}/issuetoken.aspx?u={1}&p={2}",
             ServiceBusEnvironment.DefaultIdentityHostName,
             HttpUtility.UrlEncode(solutionName),
             HttpUtility.UrlEncode(password));

         HttpWebRequest acsRequest = (HttpWebRequest)WebRequest.Create(acsUri);
         using (var acsResponse = acsRequest.GetResponse())
         {
          using (Stream acsStream = acsResponse.GetResponseStream())
          {
           byte[] token = new byte[500];
           int tokenBodyLength = acsStream.Read(token, 0, 500);
           return Encoding.UTF8.GetString(token, 0, tokenBodyLength);
          }
         }
        }

       
     #endregion
    }
}
