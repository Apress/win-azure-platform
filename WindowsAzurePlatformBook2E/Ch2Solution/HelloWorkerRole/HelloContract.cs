using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Net;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace HelloService
{
    [ServiceContract (Namespace="http://proazure/helloservice")]
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
    [ServiceBehavior(AddressFilterMode=AddressFilterMode.Any)]
    public class HelloServiceImpl : IHelloService
    {



        #region IHelloService Members

        public string GetMyIp()
        {
            IPAddress[] ips = null;

            ips = Dns.GetHostAddresses(Dns.GetHostName());

            if (ips != null)
            {
                foreach (IPAddress i in ips)
                {
                    if(i.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                     return i.ToString(); ;
                }

            }

            return "";
        }

        #endregion

      


        public string GetHostName()
        {
            return Dns.GetHostName();
        }

       
        public int GetUpdateDomain()
        {
            return RoleEnvironment.CurrentRoleInstance.UpdateDomain;

        }
       
        public int GetFaultDomain()
        {
            return RoleEnvironment.CurrentRoleInstance.FaultDomain;

        }
    }
}
