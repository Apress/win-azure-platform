using System;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace EventPoint.Common
{
    public class Config
    {
  
        public static string serviceNamespace;
        public static string criticalServiceNamespace;
        public static string issuerName;
        public static string issuerSecret;
        public static string dataConnectionString; 
        public static string sqlConnectionString;


        static Config()
        {
                serviceNamespace = ConfigurationManager.AppSettings["ServiceNamespace"];
                criticalServiceNamespace = ConfigurationManager.AppSettings["CriticalServiceNamespace"];
                issuerName = ConfigurationManager.AppSettings["IssuerName"];
                issuerSecret = ConfigurationManager.AppSettings["IssuerSecret"];
                dataConnectionString = ConfigurationManager.AppSettings["DataConnectionString"];
                sqlConnectionString = ConfigurationManager.AppSettings["sqlConnectionString"];            
        }

    }
}
