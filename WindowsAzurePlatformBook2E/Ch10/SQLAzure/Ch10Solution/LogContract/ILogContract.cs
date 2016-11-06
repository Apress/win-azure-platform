using System;
using System.ServiceModel;
using System.Collections.Generic;


namespace AzureSample
{
    [ServiceContract(Name = "ILogContract", Namespace = "http://tempuri.org/")]

    public interface ILogContract
    {

        [OperationContract]
        void WriteToLog(DateTime eventDt, string source, string text);

    }


    public interface ILogChannel : ILogContract, IClientChannel { }

}
