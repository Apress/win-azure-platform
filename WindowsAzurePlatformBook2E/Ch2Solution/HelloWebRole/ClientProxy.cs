using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.ServiceModel.Channels;


namespace HelloService
{
    [ServiceContract(Namespace = "http://proazure/helloservice")]
    interface IHelloService
    {
        [OperationContract]
        string GetMyIp();
        [OperationContract]
        string GetHostName();
        [OperationContract]
        int GetUpdateDomain();
        [OperationContract]
        int GetFaultDomain();

    }
    partial class ClientProxy : ClientBase<IHelloService>, IHelloService
    {
        public ClientProxy()
        { }
        public ClientProxy(string endpointName)
            : base(endpointName)
        { }
        public ClientProxy(Binding binding, EndpointAddress remoteAddress) :
            base(binding, remoteAddress)
        { }
        /* Additional constructors */
        public string GetMyIp()
        {
            return Channel.GetMyIp();
        }

        public string GetHostName()
        {
            return Channel.GetHostName();
        }
        public int GetUpdateDomain()
        {
            return Channel.GetUpdateDomain();
        }
        public int GetFaultDomain()
        {
            return Channel.GetFaultDomain();
        }

       
    }
}
