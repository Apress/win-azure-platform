using System;
using System.ServiceModel;
using System.Collections.Generic;


namespace ProAzureDemResContract
{
    [ServiceContract(Name = "ILogContract", Namespace = "http://proazure.com/")]
    public interface ILogContract
    {

        [OperationContract]
        void WriteToLog(DateTime eventDt, string source, string text);

    }


    public interface ILogChannel : ILogContract, IClientChannel { }

}
