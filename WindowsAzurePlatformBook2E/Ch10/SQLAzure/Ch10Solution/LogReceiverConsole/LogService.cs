using System;
using System.ServiceModel;
using System.Text;
using System.Diagnostics;

namespace AzureSample
{


    [ServiceBehavior(Name = "LogService", Namespace = "http://tempuri.org/")]



    class LogService : ILogContract
    {



        public void WriteToLog(DateTime eventDt, string source, string text)
        {


            string logMessage = string.Format("From {0} at {1}: {2}", eventDt.ToString(), source, text);
            Console.WriteLine(logMessage);
            Trace.WriteLine(logMessage);

        }


    }


}

