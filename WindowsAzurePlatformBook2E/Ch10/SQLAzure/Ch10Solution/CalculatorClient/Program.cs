using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.ServiceModel;
using Microsoft.ServiceBus.Samples;
using AzureSample;

namespace CalculatorClient
{
    class Program
    {
        static void Main(string[] args)
        {
            string endPoint = "sb://proazure.servicebus.windows.net/sample/calc/";//ConfigurationSettings.AppSettings["EndPoint"];
            string userName = "proazure"; // ConfigurationSettings.AppSettings["UserName"];
            string password = "proazure";//  ConfigurationSettings.AppSettings["Password"];
            AzureSample.CalculatorClient c = new AzureSample.CalculatorClient(endPoint, userName, password);
            double a = c.Channel.Add(2.5, 2.5);
            Console.WriteLine("Ans:" + a);
            Console.ReadLine();
        }
    }
}
