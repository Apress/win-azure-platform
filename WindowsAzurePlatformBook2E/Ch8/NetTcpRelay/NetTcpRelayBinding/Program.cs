using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using Microsoft.ServiceBus;
using ServiceBusUtils;
using EnergyServiceContract;

namespace NetEventRelayServer
{
 class Program
 {
  static void Main(string[] args)
  {
   try
   {
       string serviceNamespace = ServiceBusHelper.GetServiceBusSolutionName();
   

    string input = string.Empty;

    do
    {
     try
     {
      Console.WriteLine("Press enter exit to exit");
      Console.WriteLine("Enter a gatewayId to turn everything off");
      Console.Write("Enter GatewayID>");
      input = Console.ReadLine();
      TurnEverythingOff(serviceNamespace, input.Trim());
     }
     catch (Exception ex)
     {
      Console.WriteLine(ex.Message);

     }
        
    } while (input != "exit");

    
   }
   catch (Exception ex)
   {
    Console.WriteLine(ex.Message);

   }
  }

   static void TurnEverythingOff(string serviceNamespace, string gatewayId)
  {
      
      ChannelFactory<IEnergyServiceGatewayOperationsChannel> netTcpRelayChannelFactory = null;
      IEnergyServiceGatewayOperationsChannel netTcpRelayChannel = null;
      try
      {
          Uri serviceUri = ServiceBusEnvironment.CreateServiceUri("sb", serviceNamespace, ServiceBusHelper.GetGatewayServicePath(gatewayId));
          //For WS2207HttpRelayBinding
         // Uri serviceUri = ServiceBusEnvironment.CreateServiceUri("http", serviceNamespace, ServiceBusHelper.GetGatewayServicePath(gatewayId));

          netTcpRelayChannelFactory = new ChannelFactory<IEnergyServiceGatewayOperationsChannel>("RelayTcpEndpoint", new EndpointAddress(serviceUri));
          netTcpRelayChannel = netTcpRelayChannelFactory.CreateChannel();
          netTcpRelayChannel.Open();

          Console.WriteLine("Connected to " + serviceUri.ToString());
          Console.WriteLine("Light switch is:" + netTcpRelayChannel.GetLightingValue(gatewayId, "Lighting-1"));
          netTcpRelayChannel.SetLightingValue(gatewayId, "Lighting-1", 0);
          Console.WriteLine("Light switch turned OFF");

          Console.WriteLine("Current Temperature:" + netTcpRelayChannel.GetCurrentTemp(gatewayId, "HVAC-1"));
          Console.WriteLine("Current Set Point:" + netTcpRelayChannel.GetHVACSetpoint(gatewayId, "HVAC-1"));
          Console.WriteLine("Current HVAC Mode:" + netTcpRelayChannel.GetHVACMode(gatewayId, "HVAC-1"));
          netTcpRelayChannel.SetHVACMode(gatewayId, "HVAC-1", 0);
          netTcpRelayChannel.SetHVACSetpoint(gatewayId, "HVAC-1", 78);
          Console.WriteLine("Set HVAC mode to OFF");
          Console.WriteLine("Set everything to off on " + gatewayId);

      }
      catch (Exception ex)
      {
          Console.WriteLine(ex.Message);


      }
      finally
      {

          if (netTcpRelayChannel != null && netTcpRelayChannelFactory != null)
          {

              netTcpRelayChannel.Close();

              netTcpRelayChannelFactory.Close();

          }

      }

  }


 }
}
