using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Net;
using System.Net.Security;
using Microsoft.Samples.WindowsAzure.ServiceManagement;
using System.Threading;
using System.IO;
using System.Configuration;
using System.Xml.Linq;

namespace ServiceManagement
{
    /// <summary>
    /// Helper class for calling service management API
    /// </summary>
    public class ServiceManagementUtil
    {
        public static int PollTimeoutInSeconds = 30;

        //public static string CertificateThumbprint { get; set; }
        //public static X509Certificate2 certificate { get; set; }

        static ServiceManagementUtil()
        {
            ServicePointManager.ServerCertificateValidationCallback = (RemoteCertificateValidationCallback)Delegate.Combine(ServicePointManager.ServerCertificateValidationCallback, new RemoteCertificateValidationCallback(ServiceManagementUtil.RemoteCertValidate));
        }

        //public static bool ValidateCertificate()
        //{

        //    string certificateThumbprint = TryGetConfigurationSetting("CertificateThumbprint");
        //    if (string.IsNullOrEmpty(certificateThumbprint))
        //    {

        //        throw new Exception("Certificate thumbprint not found.");
        //    }


        //    if (String.IsNullOrEmpty(certificateThumbprint))
        //    {

        //        return false;
        //    }

        //    X509Store certificateStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);
        //    certificateStore.Open(OpenFlags.ReadOnly);
        //    X509Certificate2Collection certs = certificateStore.Certificates.Find(X509FindType.FindByThumbprint, certificateThumbprint, false);
        //    if (certs.Count != 1)
        //    {
        //        Console.WriteLine("Client certificate cannot be found. Please check the config file. ");
        //        return false;
        //    }
        //    certificate = certs[0];
        //    return true;
        //}

        /// <summary>
        /// Sets the cert policy.
        /// </summary>
        public static void SetCertificatePolicy()
        {
            ServicePointManager.ServerCertificateValidationCallback
                       += RemoteCertificateValidate;
        }

        /// <summary>
        /// Remotes the certificate validate.
        /// </summary>
        private static bool RemoteCertificateValidate(
           object sender, X509Certificate cert,
            X509Chain chain, SslPolicyErrors error)
        {
            // trust any certificate!!!
            System.Console.WriteLine("Warning, trust any certificate");
            return true;
        }



        #region Certificate
        static public X509Certificate2 GetCertificateByThumbprintSettingName(string ThumbprintSettingName)
        {
            //GetConfigurationValue() doesn't seem to retrieve the value from Certificates
            //So I added it to the Settings tab
            string Thumbprint = ConfigurationManager.AppSettings[ThumbprintSettingName];

            return GetCertificateByThumbprint(Thumbprint);
        }
        public static X509Certificate2 GetCertificateByThumbprint(string Thumbprint)
        {

            X509Store certificateStore = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            certificateStore.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection certs = certificateStore.Certificates.Find(X509FindType.FindByThumbprint, Thumbprint, false);

            return certs[0];
        }
        public static X509Certificate2 GetCertificate(byte[] certificateBytes)
        {

            return new X509Certificate2(certificateBytes);


        }

        public static X509Certificate2 GetCertificate(byte[] certificateBytes, string password)
        {

            return new X509Certificate2(certificateBytes, password);


        }

        public static X509Certificate2 GetCertificate(string certificateFilePath)
        {

            return new X509Certificate2(certificateFilePath);


        }

        public static X509Certificate2 GetCertificate(string certificateFilePath, string password)
        {

            return new X509Certificate2(certificateFilePath, password);


        }

        private static bool RemoteCertValidate(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors error)
        {
            return true;
        }

        #endregion

        #region Channel

        private static IServiceManagement GetChannel(X509Certificate2 cert)
        {

            return ServiceManagementHelper.CreateServiceManagementChannel("WindowsAzureEndPoint", cert);
        }

        private static IServiceManagement GetChannel(byte[] certificateBytes, string password)
        {

            return ServiceManagementHelper.CreateServiceManagementChannel("WindowsAzureEndPoint", (!string.IsNullOrEmpty(password)) ? GetCertificate(certificateBytes, password) : GetCertificate(certificateBytes));
        }
        private static IServiceManagement GetChannel(byte[] certificateBytes)
        {

            return ServiceManagementHelper.CreateServiceManagementChannel("WindowsAzureEndPoint", GetCertificate(certificateBytes));
        }

        private static IServiceManagement GetChannel(string certificateFilePath)
        {

            return ServiceManagementHelper.CreateServiceManagementChannel("WindowsAzureEndPoint", GetCertificate(certificateFilePath));
        }

        private static IServiceManagement GetChannel(string certificateFilePath, string password)
        {

            return ServiceManagementHelper.CreateServiceManagementChannel("WindowsAzureEndPoint", (!string.IsNullOrEmpty(password)) ? GetCertificate(certificateFilePath, password) : GetCertificate(certificateFilePath));
        }
        private static IServiceManagement GetChannelByThumbprint(string certificateThumbprint)
        {

            return ServiceManagementHelper.CreateServiceManagementChannel("WindowsAzureEndPoint", GetCertificateByThumbprint(certificateThumbprint));
        }

        #endregion

        #region private methods

        public static string TryGetConfigurationSetting(string configName)
        {
            var setting = ConfigurationManager.AppSettings[configName];
            if (setting == null)
            {
                return null;
            }
            else
            {
                return setting.ToString();
            }
        }
        //private static void WaitForAsyncOperation(IServiceManagement service, string trackingId, string subscriptionId)
        //{
        //    Operation operation;
        //    int num = 0;
        //    do
        //    {
        //        operation = service.GetOperationStatus(subscriptionId, trackingId);
        //        Thread.Sleep(30000);
        //        Console.Write(".");
        //        num++;
        //        if ((PollTimeoutInSeconds > 0) && (num > PollTimeoutInSeconds))
        //        {
        //            Console.WriteLine();
        //            Console.WriteLine("Giving up after {0} seconds. Call GetResult manually to get the status", PollTimeoutInSeconds);
        //            break;
        //        }
        //    }
        //    while ((operation.Status != "Failed") && (operation.Status != "Succeeded"));
        //    if (operation.Status != "InProgress")
        //    {
        //        Console.WriteLine("Done");
        //    }
        //    Console.Write("Operation Status=" + operation.Status.ToString());
        //}

        #endregion

        #region Service Management methods


        #region Operation Status
        public static Operation GetOperationStatus(ref IServiceManagement channel, string subscriptionId, string operationId, byte[] certificateBytes, string password, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            nullable = null;
            statusDescription = null;
            opid = string.Empty;

            if (channel == null)
            {
                channel = GetChannel(certificateBytes, password);
            }



            return GetOperationStatus(ref channel, subscriptionId, operationId, out opid, out nullable, out statusDescription);

        }

        public static Operation GetOperationStatus(ref IServiceManagement channel, string subscriptionId, string operationId, string certificateThumbPrint, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;


            if (channel == null)
            {
                channel = GetChannelByThumbprint(certificateThumbPrint);
            }

            return GetOperationStatus(ref channel, subscriptionId, operationId, out opid, out nullable, out statusDescription);


        }

        public static Operation GetOperationStatus(ref IServiceManagement channel, string subscriptionId, string operationId, X509Certificate2 cert, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;


            if (channel == null)
            {
                channel = GetChannel(cert);
            }

            return GetOperationStatus(ref channel, subscriptionId, operationId, out opid, out nullable, out statusDescription);


        }
        private static Operation GetOperationStatus(ref IServiceManagement channel, string subscriptionId, string operationId, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            nullable = null;
            statusDescription = null;
            opid = string.Empty;
            Operation hl = null;
            try
            {


                using (new OperationContextScope((IContextChannel)channel))
                {
                    try
                    {
                        hl = channel.GetOperationStatus(subscriptionId, operationId);
                        if (WebOperationContext.Current.IncomingResponse != null)
                        {

                            opid = WebOperationContext.Current.IncomingResponse.Headers["x-ms-request-id"];
                            nullable = new HttpStatusCode?(WebOperationContext.Current.IncomingResponse.StatusCode);
                            statusDescription = WebOperationContext.Current.IncomingResponse.StatusDescription;

                        }
                    }
                    catch (CommunicationException exception)
                    {
                        throw exception;
                    }

                }

                //if (((opid != null) && nullable.HasValue) && (((HttpStatusCode)nullable) == HttpStatusCode.Accepted))
                //{

                //    WaitForAsyncOperation(channel, opid, subscriptionId);

                //}
            }
            catch (TimeoutException)
            {
                throw;
            }

            return hl;
        }

        #endregion

        #region Affinity Group
        /// <summary>
        /// 
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="subscriptionId"></param>
        /// <param name="certificateThumbPrint"></param>
        /// <param name="opid"></param>
        /// <param name="nullable"></param>
        /// <param name="statusDescription"></param>
        /// <returns></returns>
        public static AffinityGroupList ListAffinityGroups(ref IServiceManagement channel, string subscriptionId, string certificateThumbPrint, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            if (channel == null)
            {
                channel = GetChannelByThumbprint(certificateThumbPrint);
            }


            return ListAffinityGroups(ref channel, subscriptionId, out opid, out nullable, out statusDescription);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="subscriptionId"></param>
        /// <param name="certificateBytes"></param>
        /// <param name="password"></param>
        /// <param name="opid"></param>
        /// <param name="nullable"></param>
        /// <param name="statusDescription"></param>
        /// <returns></returns>
        public static AffinityGroupList ListAffinityGroups(ref IServiceManagement channel, string subscriptionId, byte[] certificateBytes, string password, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            if (channel == null)
            {
                channel = GetChannel(certificateBytes, password);
            }


            return ListAffinityGroups(ref channel, subscriptionId, out opid, out nullable, out statusDescription);
        }

        public static AffinityGroupList ListAffinityGroups(ref IServiceManagement channel, string subscriptionId, X509Certificate2 cert, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            if (channel == null)
            {
                channel = GetChannel(cert);
            }


            return ListAffinityGroups(ref channel, subscriptionId, out opid, out nullable, out statusDescription);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="subscriptionId"></param>
        /// <param name="opid"></param>
        /// <param name="nullable"></param>
        /// <param name="statusDescription"></param>
        /// <returns></returns>
        private static AffinityGroupList ListAffinityGroups(ref IServiceManagement channel, string subscriptionId, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;
            AffinityGroupList hl = null;
            try
            {

                using (new OperationContextScope((IContextChannel)channel))
                {
                    try
                    {
                        hl = channel.ListAffinityGroups(subscriptionId);
                        if (WebOperationContext.Current.IncomingResponse != null)
                        {

                            opid = WebOperationContext.Current.IncomingResponse.Headers["x-ms-request-id"];
                            nullable = new HttpStatusCode?(WebOperationContext.Current.IncomingResponse.StatusCode);
                            statusDescription = WebOperationContext.Current.IncomingResponse.StatusDescription;

                        }
                    }
                    catch (CommunicationException exception)
                    {
                        throw exception;
                    }

                }

                //if (((opid != null) && nullable.HasValue) && (((HttpStatusCode)nullable) == HttpStatusCode.Accepted))
                //{

                //    WaitForAsyncOperation(channel, opid, subscriptionId);

                //}
            }
            catch (TimeoutException)
            {
                throw;
            }

            return hl;
        }


        public static AffinityGroup GetAffinityGroup(ref IServiceManagement channel, string subscriptionId, string affinityGroupName, string certificateThumbPrint, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;
            AffinityGroup hl = null;
            try
            {
                if (channel == null)
                {
                    channel = GetChannelByThumbprint(certificateThumbPrint);
                }



                using (new OperationContextScope((IContextChannel)channel))
                {
                    try
                    {
                        hl = channel.GetAffinityGroup(subscriptionId, affinityGroupName);
                        if (WebOperationContext.Current.IncomingResponse != null)
                        {

                            opid = WebOperationContext.Current.IncomingResponse.Headers["x-ms-request-id"];
                            nullable = new HttpStatusCode?(WebOperationContext.Current.IncomingResponse.StatusCode);
                            statusDescription = WebOperationContext.Current.IncomingResponse.StatusDescription;

                        }
                    }
                    catch (CommunicationException exception)
                    {
                        throw exception;
                    }
                    finally
                    {
                        if (nullable.HasValue)
                        {
                            Console.WriteLine("HTTP Status Code: {0}", nullable);
                            Console.WriteLine("StatusDescription: {0}", statusDescription);
                        }
                    }
                }

                //if (((opid != null) && nullable.HasValue) && (((HttpStatusCode)nullable) == HttpStatusCode.Accepted))
                //{
                //    Console.WriteLine("Waiting for async operation to complete:");
                //    WaitForAsyncOperation(channel, opid, subscriptionId);
                //    Console.WriteLine();
                //}
            }
            catch (TimeoutException)
            {
                throw;
            }

            return hl;
        }


        #endregion

        #region Storage Service

        #region CreateStorageService
        public static void CreateStorageService(ref IServiceManagement channel, string subscriptionId, string serviceName, string serviceLabel, string description, string location, string affinityGroup, string certificateThumbPrint, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            if (channel == null)
            {
                channel = GetChannelByThumbprint(certificateThumbPrint);

            }
            CreateStorageService(ref channel, subscriptionId, serviceName, serviceLabel, description, location, affinityGroup, out opid, out nullable, out statusDescription);


        }

        public static void CreateStorageService(ref IServiceManagement channel, string subscriptionId, string serviceName, string serviceLabel, string description, string location, string affinityGroup, X509Certificate2 cert, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            if (channel == null)
            {
                channel = GetChannel(cert);

            }
            CreateStorageService(ref channel, subscriptionId, serviceName, serviceLabel, description, location, affinityGroup, out opid, out nullable, out statusDescription);


        }

        public static void CreateStorageService(ref IServiceManagement channel, string subscriptionId, string serviceName, string serviceLabel, string description, string location, string affinityGroup, byte[] certificateBytes, string password, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            if (channel == null)
            {
                channel = GetChannel(certificateBytes);

            }
            CreateStorageService(ref channel, subscriptionId, serviceName, serviceLabel, description, location, affinityGroup, out opid, out nullable, out statusDescription);
        }

        private static void CreateStorageService(ref IServiceManagement channel, string subscriptionId, string serviceName, string serviceLabel, string description, string location, string affinityGroup, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;
            try
            {

                CreateStorageServiceInput input2;


                using (new OperationContextScope((IContextChannel)channel))
                {


                    input2 = new CreateStorageServiceInput();
                    input2.ServiceName = serviceName;
                    input2.Description = description;
                    if (!string.IsNullOrEmpty(affinityGroup))
                    {
                        input2.AffinityGroup = affinityGroup;
                    }
                    else
                    {
                        input2.Location = location;
                    }

                    if (!string.IsNullOrEmpty(serviceLabel))
                    {
                        input2.Label = ServiceManagementHelper.EncodeToBase64String(serviceLabel);
                    }

                    channel.CreateStorageService(subscriptionId, input2);
                    if (WebOperationContext.Current.IncomingResponse != null)
                    {

                        opid = WebOperationContext.Current.IncomingResponse.Headers["x-ms-request-id"];
                        nullable = new HttpStatusCode?(WebOperationContext.Current.IncomingResponse.StatusCode);
                        statusDescription = WebOperationContext.Current.IncomingResponse.StatusDescription;

                    }



                }


                //if (((opid != null) && nullable.HasValue) && (((HttpStatusCode)nullable) == HttpStatusCode.Accepted))
                //{

                //    WaitForAsyncOperation(channel, opid, subscriptionId);

                //}
            }
            catch (TimeoutException)
            {
                throw;
            }
        }

        #endregion

        #region DeleteStorageService
        public static void DeleteStorageService(ref IServiceManagement channel, string subscriptionId, string serviceName, string certificateThumbPrint, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            if (channel == null)
            {
                channel = GetChannelByThumbprint(certificateThumbPrint);

            }


            DeleteStorageService(ref channel, subscriptionId, serviceName, out opid, out nullable, out statusDescription);
        }

        public static void DeleteStorageService(ref IServiceManagement channel, string subscriptionId, string serviceName, X509Certificate2 cert, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            if (channel == null)
            {
                channel = GetChannel(cert);

            }


            DeleteStorageService(ref channel, subscriptionId, serviceName, out opid, out nullable, out statusDescription);
        }
        public static void DeleteStorageService(ref IServiceManagement channel, string subscriptionId, string serviceName, byte[] certificateBytes, string password, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;


            if (channel == null)
            {
                channel = GetChannel(certificateBytes, password);

            }

            DeleteStorageService(ref channel, subscriptionId, serviceName, out opid, out nullable, out statusDescription);
        }

        private static void DeleteStorageService(ref IServiceManagement channel, string subscriptionId, string serviceName, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            try
            {


                using (new OperationContextScope((IContextChannel)channel))
                {


                    channel.DeleteStorageService(subscriptionId, serviceName);
                    if (WebOperationContext.Current.IncomingResponse != null)
                    {

                        opid = WebOperationContext.Current.IncomingResponse.Headers["x-ms-request-id"];
                        nullable = new HttpStatusCode?(WebOperationContext.Current.IncomingResponse.StatusCode);
                        statusDescription = WebOperationContext.Current.IncomingResponse.StatusDescription;

                    }



                }


                //if (((opid != null) && nullable.HasValue) && (((HttpStatusCode)nullable) == HttpStatusCode.Accepted))
                //{

                //    WaitForAsyncOperation(channel, opid, subscriptionId);

                //}
            }
            catch (TimeoutException)
            {
                throw;
            }
        }

        #endregion

        #region UpdateStorageService

        public static void UpdateStorageService(ref IServiceManagement channel, string subscriptionId, string serviceName, string serviceLabel, string description, string certificateThumbPrint, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            if (channel == null)
            {
                channel = GetChannelByThumbprint(certificateThumbPrint);

            }
            UpdateStorageService(ref channel, subscriptionId, serviceName, serviceLabel, description, out opid, out nullable, out statusDescription);


        }

        public static void UpdateStorageService(ref IServiceManagement channel, string subscriptionId, string serviceName, string serviceLabel, string description, X509Certificate2 cert, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            if (channel == null)
            {
                channel = GetChannel(cert);

            }
            UpdateStorageService(ref channel, subscriptionId, serviceName, serviceLabel, description, out opid, out nullable, out statusDescription);


        }

        public static void UpdateStorageService(ref IServiceManagement channel, string subscriptionId, string serviceName, string serviceLabel, string description, byte[] certificateBytes, string password, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            if (channel == null)
            {
                channel = GetChannel(certificateBytes, password);

            }
            UpdateStorageService(ref channel, subscriptionId, serviceName, serviceLabel, description, out opid, out nullable, out statusDescription);
        }

        private static void UpdateStorageService(ref IServiceManagement channel, string subscriptionId, string serviceName, string serviceLabel, string description, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            try
            {

                UpdateStorageServiceInput input2;


                using (new OperationContextScope((IContextChannel)channel))
                {


                    input2 = new UpdateStorageServiceInput();
                    input2.Description = description;


                    if (!string.IsNullOrEmpty(serviceLabel))
                    {
                        input2.Label = ServiceManagementHelper.EncodeToBase64String(serviceLabel);
                    }

                    channel.UpdateStorageService(subscriptionId, serviceName, input2);
                    if (WebOperationContext.Current.IncomingResponse != null)
                    {

                        opid = WebOperationContext.Current.IncomingResponse.Headers["x-ms-request-id"];
                        nullable = new HttpStatusCode?(WebOperationContext.Current.IncomingResponse.StatusCode);
                        statusDescription = WebOperationContext.Current.IncomingResponse.StatusDescription;

                    }



                }


                //if (((opid != null) && nullable.HasValue) && (((HttpStatusCode)nullable) == HttpStatusCode.Accepted))
                //{

                //    WaitForAsyncOperation(channel, opid, subscriptionId);

                //}
            }
            catch (TimeoutException)
            {
                throw;
            }
        }

        #endregion

        #region ListStorageServices
        public static StorageServiceList ListStorageServices(ref IServiceManagement channel, string subscriptionId, string certificateThumbPrint, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;


            if (channel == null)
            {
                channel = GetChannelByThumbprint(certificateThumbPrint);
            }

            return ListStorageServices(ref channel, subscriptionId, out opid, out nullable, out statusDescription);
        }

        public static StorageServiceList ListStorageServices(ref IServiceManagement channel, string subscriptionId, byte[] certBytes, string password, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;


            if (channel == null)
            {
                channel = GetChannel(certBytes, password);
            }

            return ListStorageServices(ref channel, subscriptionId, out opid, out nullable, out statusDescription);
        }

        public static StorageServiceList ListStorageServices(ref IServiceManagement channel, string subscriptionId, X509Certificate2 cert, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;


            if (channel == null)
            {
                channel = GetChannel(cert);
            }

            return ListStorageServices(ref channel, subscriptionId, out opid, out nullable, out statusDescription);
        }

        private static StorageServiceList ListStorageServices(ref IServiceManagement channel, string subscriptionId, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;
            StorageServiceList hl = null;
            try
            {


                using (new OperationContextScope((IContextChannel)channel))
                {
                    try
                    {
                        hl = channel.ListStorageServices(subscriptionId);
                        if (WebOperationContext.Current.IncomingResponse != null)
                        {

                            opid = WebOperationContext.Current.IncomingResponse.Headers["x-ms-request-id"];
                            nullable = new HttpStatusCode?(WebOperationContext.Current.IncomingResponse.StatusCode);
                            statusDescription = WebOperationContext.Current.IncomingResponse.StatusDescription;

                        }
                    }
                    catch (CommunicationException exception)
                    {
                        throw exception;
                    }
                    finally
                    {
                        if (nullable.HasValue)
                        {
                            Console.WriteLine("HTTP Status Code: {0}", nullable);
                            Console.WriteLine("StatusDescription: {0}", statusDescription);
                        }
                    }
                }

                //if (((opid != null) && nullable.HasValue) && (((HttpStatusCode)nullable) == HttpStatusCode.Accepted))
                //{
                //    Console.WriteLine("Waiting for async operation to complete:");
                //    WaitForAsyncOperation(channel, opid, subscriptionId);
                //    Console.WriteLine();
                //}
            }
            catch (TimeoutException)
            {
                throw;
            }

            return hl;
        }
        #endregion

        #region RegenerateStorageServiceKeys
        public static StorageService RegenerateStorageServiceKeys(ref IServiceManagement channel, string subscriptionId, string serviceName, bool isPrimaryKey, string certificateThumbPrint, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            if (channel == null)
            {
                channel = GetChannelByThumbprint(certificateThumbPrint);
            }

            return RegenerateStorageServiceKeys(ref channel, subscriptionId, serviceName, (isPrimaryKey) ? "primary" : "secondary", out opid, out nullable, out statusDescription);
        }

        public static StorageService RegenerateStorageServiceKeys(ref IServiceManagement channel, string subscriptionId, string serviceName, bool isPrimaryKey, byte[] certBytes, string password, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            if (channel == null)
            {
                channel = GetChannel(certBytes, password);
            }

            return RegenerateStorageServiceKeys(ref channel, subscriptionId, serviceName, (isPrimaryKey) ? "primary" : "secondary", out opid, out nullable, out statusDescription);
        }

        public static StorageService RegenerateStorageServiceKeys(ref IServiceManagement channel, string subscriptionId, string serviceName, bool isPrimaryKey, X509Certificate2 cert, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            if (channel == null)
            {
                channel = GetChannel(cert);
            }

            return RegenerateStorageServiceKeys(ref channel, subscriptionId, serviceName, (isPrimaryKey) ? "primary" : "secondary", out opid, out nullable, out statusDescription);
        }
        private static StorageService RegenerateStorageServiceKeys(ref IServiceManagement channel, string subscriptionId, string serviceName, string keyType, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            StorageService hl = null;
            try
            {


                using (new OperationContextScope((IContextChannel)channel))
                {
                    try
                    {
                        RegenerateKeys keys = new RegenerateKeys();
                        keys.KeyType = keyType;

                        hl = channel.RegenerateStorageServiceKeys(subscriptionId, serviceName, keys);
                        if (WebOperationContext.Current.IncomingResponse != null)
                        {

                            opid = WebOperationContext.Current.IncomingResponse.Headers["x-ms-request-id"];
                            nullable = new HttpStatusCode?(WebOperationContext.Current.IncomingResponse.StatusCode);
                            statusDescription = WebOperationContext.Current.IncomingResponse.StatusDescription;

                        }
                    }
                    catch (CommunicationException exception)
                    {
                        throw exception;
                    }
                    finally
                    {
                        if (nullable.HasValue)
                        {
                            Console.WriteLine("HTTP Status Code: {0}", nullable);
                            Console.WriteLine("StatusDescription: {0}", statusDescription);
                        }
                    }
                }

                //if (((opid != null) && nullable.HasValue) && (((HttpStatusCode)nullable) == HttpStatusCode.Accepted))
                //{

                //    WaitForAsyncOperation(channel, opid, subscriptionId);

                //}
            }
            catch (TimeoutException)
            {
                throw;
            }

            return hl;
        }
        #endregion

        #region GetStorageKeys
        public static StorageService GetStorageKeys(ref IServiceManagement channel, string subscriptionId, string serviceName, string certificateThumbPrint, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            if (channel == null)
            {
                channel = GetChannelByThumbprint(certificateThumbPrint);
            }


            return GetStorageKeys(ref channel, subscriptionId, serviceName, out opid, out nullable, out statusDescription);
        }

        public static StorageService GetStorageKeys(ref IServiceManagement channel, string subscriptionId, string serviceName, byte[] certBytes, string password, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            if (channel == null)
            {
                channel = GetChannel(certBytes, password);
            }


            return GetStorageKeys(ref channel, subscriptionId, serviceName, out opid, out nullable, out statusDescription);
        }

        public static StorageService GetStorageKeys(ref IServiceManagement channel, string subscriptionId, string serviceName, X509Certificate2 cert, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            if (channel == null)
            {
                channel = GetChannel(cert);
            }


            return GetStorageKeys(ref channel, subscriptionId, serviceName, out opid, out nullable, out statusDescription);
        }
        private static StorageService GetStorageKeys(ref IServiceManagement channel, string subscriptionId, string serviceName, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;
            StorageService hl = null;
            try
            {

                using (new OperationContextScope((IContextChannel)channel))
                {
                    try
                    {
                        hl = channel.GetStorageKeys(subscriptionId, serviceName);
                        if (WebOperationContext.Current.IncomingResponse != null)
                        {

                            opid = WebOperationContext.Current.IncomingResponse.Headers["x-ms-request-id"];
                            nullable = new HttpStatusCode?(WebOperationContext.Current.IncomingResponse.StatusCode);
                            statusDescription = WebOperationContext.Current.IncomingResponse.StatusDescription;

                        }
                    }
                    catch (CommunicationException exception)
                    {
                        throw exception;
                    }
                    finally
                    {
                        if (nullable.HasValue)
                        {
                            Console.WriteLine("HTTP Status Code: {0}", nullable);
                            Console.WriteLine("StatusDescription: {0}", statusDescription);
                        }
                    }
                }

                //if (((opid != null) && nullable.HasValue) && (((HttpStatusCode)nullable) == HttpStatusCode.Accepted))
                //{

                //    WaitForAsyncOperation(channel, opid, subscriptionId);

                //}
            }
            catch (TimeoutException)
            {
                throw;
            }

            return hl;
        }

        #endregion

        #region GetStorageService
        public static StorageService GetStorageService(ref IServiceManagement channel, string subscriptionId, string serviceName, string certificateThumbPrint, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;


            if (channel == null)
            {
                channel = GetChannelByThumbprint(certificateThumbPrint);
            }


            return GetStorageService(ref channel, subscriptionId, serviceName, out opid, out nullable, out statusDescription);
        }

        public static StorageService GetStorageService(ref IServiceManagement channel, string subscriptionId, string serviceName, byte[] certBytes, string password, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;


            if (channel == null)
            {
                channel = GetChannel(certBytes, password);
            }


            return GetStorageService(ref channel, subscriptionId, serviceName, out opid, out nullable, out statusDescription);
        }

        public static StorageService GetStorageService(ref IServiceManagement channel, string subscriptionId, string serviceName, X509Certificate2 cert, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;


            if (channel == null)
            {
                channel = GetChannel(cert);
            }


            return GetStorageService(ref channel, subscriptionId, serviceName, out opid, out nullable, out statusDescription);
        }
        private static StorageService GetStorageService(ref IServiceManagement channel, string subscriptionId, string serviceName, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;
            StorageService hl = null;
            try
            {

                using (new OperationContextScope((IContextChannel)channel))
                {
                    try
                    {
                        hl = channel.GetStorageService(subscriptionId, serviceName);
                        if (WebOperationContext.Current.IncomingResponse != null)
                        {

                            opid = WebOperationContext.Current.IncomingResponse.Headers["x-ms-request-id"];
                            nullable = new HttpStatusCode?(WebOperationContext.Current.IncomingResponse.StatusCode);
                            statusDescription = WebOperationContext.Current.IncomingResponse.StatusDescription;

                        }
                    }
                    catch (CommunicationException exception)
                    {
                        throw exception;
                    }
                    finally
                    {
                        if (nullable.HasValue)
                        {
                            Console.WriteLine("HTTP Status Code: {0}", nullable);
                            Console.WriteLine("StatusDescription: {0}", statusDescription);
                        }
                    }
                }

                //if (((opid != null) && nullable.HasValue) && (((HttpStatusCode)nullable) == HttpStatusCode.Accepted))
                //{
                //    WaitForAsyncOperation(channel, opid, subscriptionId);
                //}
            }
            catch (TimeoutException)
            {
                throw;
            }

            return hl;
        }

        #endregion

        #endregion

        #region Deployment

        #region CreateOrUpdateDeployment

        #region CreateOrUpdateDeployment
        public static void CreateOrUpdateDeployment(ref IServiceManagement channel, string subscriptionId, string serviceName, string deploymentName, string deploymentLabel, string configFileLocation, string deploymentSlot, string csPackageBlobUri, string certificateThumbPrint, bool startDeployment, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            if (channel == null)
            {
                channel = GetChannelByThumbprint(certificateThumbPrint);

            }

            CreateOrUpdateDeployment(ref channel, subscriptionId, serviceName, deploymentName, deploymentLabel, configFileLocation,
                    deploymentSlot, csPackageBlobUri, startDeployment, out opid, out nullable, out statusDescription);

        }

        public static void CreateOrUpdateDeployment(ref IServiceManagement channel, string subscriptionId, string serviceName, string deploymentName, string deploymentLabel, string configFileLocation, string deploymentSlot, string csPackageBlobUri, byte[] certBytes, string password, bool startDeployment, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            if (channel == null)
            {
                channel = GetChannel(certBytes, password);

            }

            CreateOrUpdateDeployment(ref channel, subscriptionId, serviceName, deploymentName, deploymentLabel, configFileLocation,
                    deploymentSlot, csPackageBlobUri, startDeployment, out opid, out nullable, out statusDescription);

        }

        public static void CreateOrUpdateDeployment(ref IServiceManagement channel, string subscriptionId, string serviceName, string deploymentName, string deploymentLabel, string configFileLocation, string deploymentSlot, string csPackageBlobUri, X509Certificate2 cert, bool startDeployment, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            if (channel == null)
            {
                channel = GetChannel(cert);

            }

            CreateOrUpdateDeployment(ref channel, subscriptionId, serviceName, deploymentName, deploymentLabel, configFileLocation,
                    deploymentSlot, csPackageBlobUri, startDeployment, out opid, out nullable, out statusDescription);

        }

        private static void CreateOrUpdateDeployment(ref IServiceManagement channel, string subscriptionId, string serviceName, string deploymentName, string deploymentLabel, string configFileLocation, string deploymentSlot, string csPackageBlobUri, bool startDeployment, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;
            try
            {

                CreateDeploymentInput input2;


                using (new OperationContextScope((IContextChannel)channel))
                {


                    input2 = new CreateDeploymentInput();
                    input2.Name = deploymentName;
                    input2.Configuration = GetEncodedConfiguration(configFileLocation);
                    input2.StartDeployment = startDeployment;
                    CreateDeploymentInput input = input2;
                    if (!string.IsNullOrEmpty(csPackageBlobUri))
                    {
                        input.PackageUrl = new Uri(csPackageBlobUri);
                    }
                    if (!string.IsNullOrEmpty(deploymentLabel))
                    {
                        input.Label = ServiceManagementHelper.EncodeToBase64String(deploymentLabel);
                    }

                    channel.CreateOrUpdateDeployment(subscriptionId, serviceName, deploymentSlot, input);
                    if (WebOperationContext.Current.IncomingResponse != null)
                    {

                        opid = WebOperationContext.Current.IncomingResponse.Headers["x-ms-request-id"];
                        nullable = new HttpStatusCode?(WebOperationContext.Current.IncomingResponse.StatusCode);
                        statusDescription = WebOperationContext.Current.IncomingResponse.StatusDescription;

                    }



                }
            }
            catch (TimeoutException)
            {
                throw;
            }

        }

        //public static void CreateOrUpdateDeployment(ref IServiceManagement channel, string subscriptionId, string serviceName, string deploymentName, string deploymentLabel, string configFileLocation, string deploymentSlot, string csPackageBlobUri, byte[] certificateBytes, string password, bool startDeployment, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        //{
        //    CreateOrUpdateDeploymentWithConfigContent(ref channel,
        //        subscriptionId,
        //         serviceName, deploymentName, deploymentLabel, GetEncodedConfiguration(configFileLocation), deploymentSlot, csPackageBlobUri, certificateBytes, password, startDeployment, out opid, out nullable, out statusDescription)
        //    ;
        //}

        #endregion

        #region CreateOrUpdateDeploymentWithConfigContent
        public static void CreateOrUpdateDeploymentWithConfigContent(ref IServiceManagement channel, string subscriptionId, string serviceName, string deploymentName, string deploymentLabel, string configFileContent, string deploymentSlot, string csPackageBlobUri, byte[] certificateBytes, string password, bool startDeployment, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            nullable = null;
            statusDescription = null;
            opid = string.Empty;

            if (channel == null)
            {
                channel = GetChannel(certificateBytes, password);

            }
            CreateOrUpdateDeploymentWithConfigContent(ref channel, subscriptionId, serviceName, deploymentName, deploymentLabel, configFileContent, deploymentSlot, csPackageBlobUri, startDeployment, out opid, out nullable, out statusDescription);
        }

        public static void CreateOrUpdateDeploymentWithConfigContent(ref IServiceManagement channel, string subscriptionId, string serviceName, string deploymentName, string deploymentLabel, string configFileContent, string deploymentSlot, string csPackageBlobUri, string certificateThumbprint, bool startDeployment, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            nullable = null;
            statusDescription = null;
            opid = string.Empty;

            if (channel == null)
            {
                channel = GetChannelByThumbprint(certificateThumbprint);

            }
            CreateOrUpdateDeploymentWithConfigContent(ref channel, subscriptionId, serviceName, deploymentName, deploymentLabel, configFileContent, deploymentSlot, csPackageBlobUri, startDeployment, out opid, out nullable, out statusDescription);
        }


        public static void CreateOrUpdateDeploymentWithConfigContent(ref IServiceManagement channel, string subscriptionId, string serviceName, string deploymentName, string deploymentLabel, string configFileContent, string deploymentSlot, string csPackageBlobUri, X509Certificate2 cert, bool startDeployment, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            nullable = null;
            statusDescription = null;
            opid = string.Empty;

            if (channel == null)
            {
                channel = GetChannel(cert);

            }
            CreateOrUpdateDeploymentWithConfigContent(ref channel, subscriptionId, serviceName, deploymentName, deploymentLabel, configFileContent, deploymentSlot, csPackageBlobUri, startDeployment, out opid, out nullable, out statusDescription);
        }

        private static void CreateOrUpdateDeploymentWithConfigContent(ref IServiceManagement channel, string subscriptionId, string serviceName, string deploymentName, string deploymentLabel, string configFileContent, string deploymentSlot, string csPackageBlobUri, bool startDeployment, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            nullable = null;
            statusDescription = null;
            opid = string.Empty;
            try
            {

                CreateDeploymentInput input2;


                using (new OperationContextScope((IContextChannel)channel))
                {


                    input2 = new CreateDeploymentInput();
                    input2.Name = deploymentName;
                    input2.Configuration = ServiceManagementHelper.EncodeToBase64String(configFileContent);
                    input2.StartDeployment = startDeployment;
                    CreateDeploymentInput input = input2;
                    if (!string.IsNullOrEmpty(csPackageBlobUri))
                    {
                        input.PackageUrl = new Uri(csPackageBlobUri);
                    }
                    if (!string.IsNullOrEmpty(deploymentLabel))
                    {
                        input.Label = ServiceManagementHelper.EncodeToBase64String(deploymentLabel);
                    }

                    channel.CreateOrUpdateDeployment(subscriptionId, serviceName, deploymentSlot, input);
                    if (WebOperationContext.Current.IncomingResponse != null)
                    {

                        opid = WebOperationContext.Current.IncomingResponse.Headers["x-ms-request-id"];
                        nullable = new HttpStatusCode?(WebOperationContext.Current.IncomingResponse.StatusCode);
                        statusDescription = WebOperationContext.Current.IncomingResponse.StatusDescription;

                    }



                }
            }
            catch (TimeoutException)
            {
                throw;
            }
        }


        #endregion
        #endregion


        #region Delete Deployment


        public static void DeleteDeployment(ref IServiceManagement channel, string subscriptionId, string serviceName, string deploymentName, byte[] certificateBytes, string password, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            nullable = null;
            statusDescription = null;
            opid = string.Empty;

            if (channel == null)
            {
                channel = GetChannel(certificateBytes, password);
            }

            DeleteDeployment(ref channel, subscriptionId, serviceName, deploymentName, out opid, out nullable, out statusDescription);

        }


        public static void DeleteDeployment(ref IServiceManagement channel, string subscriptionId, string serviceName, string deploymentName, string certThumbprint, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            nullable = null;
            statusDescription = null;
            opid = string.Empty;

            if (channel == null)
            {
                channel = GetChannelByThumbprint(certThumbprint);
            }

            DeleteDeployment(ref channel, subscriptionId, serviceName, deploymentName, out opid, out nullable, out statusDescription);

        }

        public static void DeleteDeployment(ref IServiceManagement channel, string subscriptionId, string serviceName, string deploymentName, X509Certificate2 cert, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            nullable = null;
            statusDescription = null;
            opid = string.Empty;

            if (channel == null)
            {
                channel = GetChannel(cert);
            }

            DeleteDeployment(ref channel, subscriptionId, serviceName, deploymentName, out opid, out nullable, out statusDescription);

        }
        private static void DeleteDeployment(ref IServiceManagement channel, string subscriptionId, string serviceName, string deploymentName, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            nullable = null;
            statusDescription = null;
            opid = string.Empty;
            try
            {


                using (new OperationContextScope((IContextChannel)channel))
                {
                    try
                    {
                        channel.DeleteDeployment(subscriptionId, serviceName, deploymentName);
                        if (WebOperationContext.Current.IncomingResponse != null)
                        {

                            opid = WebOperationContext.Current.IncomingResponse.Headers["x-ms-request-id"];
                            nullable = new HttpStatusCode?(WebOperationContext.Current.IncomingResponse.StatusCode);
                            statusDescription = WebOperationContext.Current.IncomingResponse.StatusDescription;

                        }
                    }
                    catch (CommunicationException exception)
                    {
                        throw exception;
                    }

                }

                //if (((opid != null) && nullable.HasValue) && (((HttpStatusCode)nullable) == HttpStatusCode.Accepted))
                //{
                //    Console.WriteLine("Waiting for async operation to complete:");
                //    WaitForAsyncOperation(channel, opid, subscriptionId);
                //    Console.WriteLine();
                //}
            }
            catch (TimeoutException)
            {
                throw;
            }


        }
        #endregion

        #region DeleteDeploymentBySlot

        public static void DeleteDeploymentBySlot(ref IServiceManagement channel, string subscriptionId, string serviceName, string deploymentSlot, string certificateThumbPrint, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            if (channel == null)
            {
                channel = GetChannelByThumbprint(certificateThumbPrint);
            }

            DeleteDeploymentBySlot(ref channel, subscriptionId, serviceName, deploymentSlot, out opid, out nullable, out statusDescription);
        }


        public static void DeleteDeploymentBySlot(ref IServiceManagement channel, string subscriptionId, string serviceName, string deploymentSlot, byte[] certBytes, string password, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            if (channel == null)
            {
                channel = GetChannel(certBytes, password);
            }

            DeleteDeploymentBySlot(ref channel, subscriptionId, serviceName, deploymentSlot, out opid, out nullable, out statusDescription);
        }

        public static void DeleteDeploymentBySlot(ref IServiceManagement channel, string subscriptionId, string serviceName, string deploymentSlot, X509Certificate2 cert, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            if (channel == null)
            {
                channel = GetChannel(cert);
            }

            DeleteDeploymentBySlot(ref channel, subscriptionId, serviceName, deploymentSlot, out opid, out nullable, out statusDescription);
        }
        private static void DeleteDeploymentBySlot(ref IServiceManagement channel, string subscriptionId, string serviceName, string deploymentSlot, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;
            try
            {

                using (new OperationContextScope((IContextChannel)channel))
                {
                    try
                    {
                        channel.DeleteDeploymentBySlot(subscriptionId, serviceName, deploymentSlot);
                        if (WebOperationContext.Current.IncomingResponse != null)
                        {

                            opid = WebOperationContext.Current.IncomingResponse.Headers["x-ms-request-id"];
                            nullable = new HttpStatusCode?(WebOperationContext.Current.IncomingResponse.StatusCode);
                            statusDescription = WebOperationContext.Current.IncomingResponse.StatusDescription;

                        }
                    }
                    catch (CommunicationException exception)
                    {
                        throw exception;
                    }

                }

                //if (((opid != null) && nullable.HasValue) && (((HttpStatusCode)nullable) == HttpStatusCode.Accepted))
                //{
                //    Console.WriteLine("Waiting for async operation to complete:");
                //    WaitForAsyncOperation(channel, opid, subscriptionId);
                //    Console.WriteLine();
                //}
            }
            catch (TimeoutException)
            {
                throw;
            }
        }
        #endregion

        #region SwapDeployment
        public static void SwapDeployment(ref IServiceManagement channel, string subscriptionId, string serviceName, string productionDeploymentName, string sourceDeploymentName, string certificateThumbPrint, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            if (channel == null)
            {
                channel = GetChannelByThumbprint(certificateThumbPrint);
            }

            SwapDeployment(ref channel, subscriptionId, serviceName, productionDeploymentName, sourceDeploymentName, out opid, out nullable, out statusDescription);

        }

        public static void SwapDeployment(ref IServiceManagement channel, string subscriptionId, string serviceName, string productionDeploymentName, string sourceDeploymentName, byte[] certBytes, string password, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            if (channel == null)
            {
                channel = GetChannel(certBytes, password);
            }

            SwapDeployment(ref channel, subscriptionId, serviceName, productionDeploymentName, sourceDeploymentName, out opid, out nullable, out statusDescription);

        }

        public static void SwapDeployment(ref IServiceManagement channel, string subscriptionId, string serviceName, string productionDeploymentName, string sourceDeploymentName, X509Certificate2 cert, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            if (channel == null)
            {
                channel = GetChannel(cert);
            }

            SwapDeployment(ref channel, subscriptionId, serviceName, productionDeploymentName, sourceDeploymentName, out opid, out nullable, out statusDescription);

        }

        private static void SwapDeployment(ref IServiceManagement channel, string subscriptionId, string serviceName, string productionDeploymentName, string sourceDeploymentName, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            try
            {


                using (new OperationContextScope((IContextChannel)channel))
                {
                    try
                    {
                        SwapDeploymentInput input = new SwapDeploymentInput();
                        input.Production = productionDeploymentName;
                        input.SourceDeployment = sourceDeploymentName;
                        channel.SwapDeployment(subscriptionId, serviceName, input);
                        if (WebOperationContext.Current.IncomingResponse != null)
                        {

                            opid = WebOperationContext.Current.IncomingResponse.Headers["x-ms-request-id"];
                            nullable = new HttpStatusCode?(WebOperationContext.Current.IncomingResponse.StatusCode);
                            statusDescription = WebOperationContext.Current.IncomingResponse.StatusDescription;

                        }

                    }
                    catch (CommunicationException exception)
                    {
                        throw exception;
                    }
                    finally
                    {
                        if (nullable.HasValue)
                        {
                            Console.WriteLine("HTTP Status Code: {0}", nullable);
                            Console.WriteLine("StatusDescription: {0}", statusDescription);
                        }
                    }
                }

                //if (((opid != null) && nullable.HasValue) && (((HttpStatusCode)nullable) == HttpStatusCode.Accepted))
                //{

                //    WaitForAsyncOperation(channel, opid, subscriptionId);

                //}
            }
            catch (TimeoutException)
            {
                throw;
            }


        }

        #endregion

        #region GetDeployment


        public static Deployment GetDeployment(ref IServiceManagement channel, string subscriptionId, string serviceName, string deploymentName, byte[] certificateBytes, string password, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            nullable = null;
            statusDescription = null;
            opid = string.Empty;


            if (channel == null)
            {
                channel = GetChannel(certificateBytes, password);
            }

            return GetDeployment(ref channel, subscriptionId, serviceName, deploymentName, out opid, out nullable, out statusDescription);
        }

        public static Deployment GetDeployment(ref IServiceManagement channel, string subscriptionId, string serviceName, string deploymentName, string certThumbprint, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            nullable = null;
            statusDescription = null;
            opid = string.Empty;


            if (channel == null)
            {
                channel = GetChannelByThumbprint(certThumbprint);
            }

            return GetDeployment(ref channel, subscriptionId, serviceName, deploymentName, out opid, out nullable, out statusDescription);
        }

        public static Deployment GetDeployment(ref IServiceManagement channel, string subscriptionId, string serviceName, string deploymentName, X509Certificate2 cert, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            nullable = null;
            statusDescription = null;
            opid = string.Empty;


            if (channel == null)
            {
                channel = GetChannel(cert);
            }

            return GetDeployment(ref channel, subscriptionId, serviceName, deploymentName, out opid, out nullable, out statusDescription);
        }
        private static Deployment GetDeployment(ref IServiceManagement channel, string subscriptionId, string serviceName, string deploymentName, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            nullable = null;
            statusDescription = null;
            opid = string.Empty;
            Deployment hl = null;
            try
            {


                using (new OperationContextScope((IContextChannel)channel))
                {
                    try
                    {
                        hl = channel.GetDeployment(subscriptionId, serviceName, deploymentName);
                        if (WebOperationContext.Current.IncomingResponse != null)
                        {

                            opid = WebOperationContext.Current.IncomingResponse.Headers["x-ms-request-id"];
                            nullable = new HttpStatusCode?(WebOperationContext.Current.IncomingResponse.StatusCode);
                            statusDescription = WebOperationContext.Current.IncomingResponse.StatusDescription;

                        }
                    }
                    catch (CommunicationException exception)
                    {
                        throw exception;
                    }

                }

                //if (((opid != null) && nullable.HasValue) && (((HttpStatusCode)nullable) == HttpStatusCode.Accepted))
                //{
                //    Console.WriteLine("Waiting for async operation to complete:");
                //    WaitForAsyncOperation(channel, opid, subscriptionId);
                //    Console.WriteLine();
                //}
            }
            catch (TimeoutException)
            {
                throw;
            }

            return hl;
        }

        #endregion

        #region GetDeploymentBySlot
        public static Deployment GetDeploymentBySlot(ref IServiceManagement channel, string subscriptionId, string serviceName, string deploymentSlot, string certificateThumbPrint, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            if (channel == null)
            {
                channel = GetChannelByThumbprint(certificateThumbPrint);
            }

            return GetDeploymentBySlot(ref channel, subscriptionId, serviceName, deploymentSlot, out opid, out nullable, out statusDescription);

        }

        public static Deployment GetDeploymentBySlot(ref IServiceManagement channel, string subscriptionId, string serviceName, string deploymentSlot, X509Certificate2 cert, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            if (channel == null)
            {
                channel = GetChannel(cert);
            }

            return GetDeploymentBySlot(ref channel, subscriptionId, serviceName, deploymentSlot, out opid, out nullable, out statusDescription);

        }

        public static Deployment GetDeploymentBySlot(ref IServiceManagement channel, string subscriptionId, string serviceName, string deploymentSlot, byte[] certificateBytes, string password, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            nullable = null;
            statusDescription = null;
            opid = string.Empty;


            if (channel == null)
            {
                channel = GetChannel(certificateBytes);
            }

            return GetDeploymentBySlot(ref channel, subscriptionId, serviceName, deploymentSlot, out opid, out nullable, out statusDescription);

        }

        private static Deployment GetDeploymentBySlot(ref IServiceManagement channel, string subscriptionId, string serviceName, string deploymentSlot, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            nullable = null;
            statusDescription = null;
            opid = string.Empty;
            Deployment hl = null;
            try
            {

                using (new OperationContextScope((IContextChannel)channel))
                {
                    try
                    {
                        hl = channel.GetDeploymentBySlot(subscriptionId, serviceName, deploymentSlot);
                        if (WebOperationContext.Current.IncomingResponse != null)
                        {

                            opid = WebOperationContext.Current.IncomingResponse.Headers["x-ms-request-id"];
                            nullable = new HttpStatusCode?(WebOperationContext.Current.IncomingResponse.StatusCode);
                            statusDescription = WebOperationContext.Current.IncomingResponse.StatusDescription;

                        }
                    }
                    catch (CommunicationException exception)
                    {
                        throw exception;
                    }

                }

                //if (((opid != null) && nullable.HasValue) && (((HttpStatusCode)nullable) == HttpStatusCode.Accepted))
                //{
                //    Console.WriteLine("Waiting for async operation to complete:");
                //    WaitForAsyncOperation(channel, opid, subscriptionId);
                //    Console.WriteLine();
                //}
            }
            catch (TimeoutException)
            {
                throw;
            }

            return hl;

        }

        #endregion

        #region UpdateDeploymentStatus
        public static void UpdateDeploymentStatus(ref IServiceManagement channel, string subscriptionId, string serviceName, string deploymentName, string deploymentStatus, string certificateThumbPrint, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            if (channel == null)
            {
                channel = GetChannelByThumbprint(certificateThumbPrint);
            }

            UpdateDeploymentStatus(ref channel, subscriptionId, serviceName, deploymentName, deploymentStatus, out opid, out nullable, out statusDescription);

        }

        public static void UpdateDeploymentStatus(ref IServiceManagement channel, string subscriptionId, string serviceName, string deploymentName, string deploymentStatus, X509Certificate2 cert, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            if (channel == null)
            {
                channel = GetChannel(cert);
            }

            UpdateDeploymentStatus(ref channel, subscriptionId, serviceName, deploymentName, deploymentStatus, out opid, out nullable, out statusDescription);

        }
        public static void UpdateDeploymentStatus(ref IServiceManagement channel, string subscriptionId, string serviceName, string deploymentName, string deploymentStatus, byte[] certificateBytes, string password, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            nullable = null;
            statusDescription = null;
            opid = string.Empty;

            if (channel == null)
            {
                channel = GetChannel(certificateBytes, password);
            }

            UpdateDeploymentStatus(ref channel, subscriptionId, serviceName, deploymentName, deploymentStatus, out opid, out nullable, out statusDescription);

        }

        private static void UpdateDeploymentStatus(ref IServiceManagement channel, string subscriptionId, string serviceName, string deploymentName, string deploymentStatus, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            nullable = null;
            statusDescription = null;
            opid = string.Empty;

            try
            {


                using (new OperationContextScope((IContextChannel)channel))
                {
                    try
                    {
                        UpdateDeploymentStatusInput input2 = new UpdateDeploymentStatusInput();
                        input2.Status = deploymentStatus;
                        UpdateDeploymentStatusInput input = input2;
                        channel.UpdateDeploymentStatus(subscriptionId, serviceName, deploymentName, input);


                        if (WebOperationContext.Current.IncomingResponse != null)
                        {

                            opid = WebOperationContext.Current.IncomingResponse.Headers["x-ms-request-id"];
                            nullable = new HttpStatusCode?(WebOperationContext.Current.IncomingResponse.StatusCode);
                            statusDescription = WebOperationContext.Current.IncomingResponse.StatusDescription;

                        }

                    }
                    catch (CommunicationException exception)
                    {
                        throw exception;
                    }

                }

                //if (((opid != null) && nullable.HasValue) && (((HttpStatusCode)nullable) == HttpStatusCode.Accepted))
                //{

                //    WaitForAsyncOperation(channel, opid, subscriptionId);

                //}
            }
            catch (TimeoutException)
            {
                throw;
            }

        }

        #endregion

        #region UpdateDeploymentStatusBySlot
        public static void UpdateDeploymentStatusBySlot(ref IServiceManagement channel, string subscriptionId, string serviceName, string deploymentSlot, string deploymentStatus, string certificateThumbPrint, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;


            if (channel == null)
            {
                channel = GetChannelByThumbprint(certificateThumbPrint);
            }

            UpdateDeploymentStatusBySlot(ref channel, subscriptionId, serviceName, deploymentSlot, deploymentStatus, out opid, out nullable, out statusDescription);

        }

        public static void UpdateDeploymentStatusBySlot(ref IServiceManagement channel, string subscriptionId, string serviceName, string deploymentSlot, string deploymentStatus, X509Certificate2 cert, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;


            if (channel == null)
            {
                channel = GetChannel(cert);
            }

            UpdateDeploymentStatusBySlot(ref channel, subscriptionId, serviceName, deploymentSlot, deploymentStatus, out opid, out nullable, out statusDescription);

        }

        public static void UpdateDeploymentStatusBySlot(ref IServiceManagement channel, string subscriptionId, string serviceName, string deploymentSlot, string deploymentStatus, byte[] certificateBytes, string password, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            nullable = null;
            statusDescription = null;
            opid = string.Empty;


            if (channel == null)
            {
                channel = GetChannel(certificateBytes, password);
            }

            UpdateDeploymentStatusBySlot(ref channel, subscriptionId, serviceName, deploymentSlot, deploymentStatus, out opid, out nullable, out statusDescription);

        }

        private static void UpdateDeploymentStatusBySlot(ref IServiceManagement channel, string subscriptionId, string serviceName, string deploymentSlot, string deploymentStatus, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            nullable = null;
            statusDescription = null;
            opid = string.Empty;

            try
            {


                using (new OperationContextScope((IContextChannel)channel))
                {
                    try
                    {
                        UpdateDeploymentStatusInput input2 = new UpdateDeploymentStatusInput();
                        input2.Status = deploymentStatus;
                        UpdateDeploymentStatusInput input = input2;
                        channel.UpdateDeploymentStatusBySlot(subscriptionId, serviceName, deploymentSlot, input);


                        if (WebOperationContext.Current.IncomingResponse != null)
                        {

                            opid = WebOperationContext.Current.IncomingResponse.Headers["x-ms-request-id"];
                            nullable = new HttpStatusCode?(WebOperationContext.Current.IncomingResponse.StatusCode);
                            statusDescription = WebOperationContext.Current.IncomingResponse.StatusDescription;

                        }

                    }
                    catch (CommunicationException exception)
                    {
                        throw exception;
                    }

                }

                //if (((opid != null) && nullable.HasValue) && (((HttpStatusCode)nullable) == HttpStatusCode.Accepted))
                //{

                //    WaitForAsyncOperation(channel, opid, subscriptionId);

                //}
            }
            catch (TimeoutException)
            {
                throw;
            }

        }

        #endregion
        #endregion

        #region Certificates

        #region AddCertificate
        public static void AddCertificate(ref IServiceManagement channel, string subscriptionId, string serviceName, string certificateThumbPrint, byte[] newCertBytes, string cert_format, string cert_password, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {

            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            if (channel == null)
            {
                channel = GetChannelByThumbprint(certificateThumbPrint);
            }

            AddCertificate(ref channel, subscriptionId, serviceName, newCertBytes, cert_format, cert_password, out opid, out nullable, out statusDescription);

        }


        public static void AddCertificate(ref IServiceManagement channel, string subscriptionId, string serviceName, byte[] apiCertBytes, string apiCertPassword, byte[] newCertBytes, string cert_format, string cert_password, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {

            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            if (channel == null)
            {
                channel = GetChannel(apiCertBytes, apiCertPassword);
            }

            AddCertificate(ref channel, subscriptionId, serviceName, newCertBytes, cert_format, cert_password, out opid, out nullable, out statusDescription);

        }

        public static void AddCertificate(ref IServiceManagement channel, string subscriptionId, string serviceName, X509Certificate2 apiCert, byte[] newCertBytes, string cert_format, string cert_password, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {

            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            if (channel == null)
            {
                channel = GetChannel(apiCert);
            }

            AddCertificate(ref channel, subscriptionId, serviceName, newCertBytes, cert_format, cert_password, out opid, out nullable, out statusDescription);

        }
        private static void AddCertificate(ref IServiceManagement channel, string subscriptionId, string serviceName, byte[] newServiceCertificateBytes, string cert_format, string cert_password, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {

            opid = string.Empty;
            nullable = null;
            statusDescription = null;
            try
            {


                using (new OperationContextScope((IContextChannel)channel))
                {
                    try
                    {
                        CertificateFile file2 = new CertificateFile();
                        file2.CertificateFormat = cert_format;
                        file2.Password = cert_password;
                        CertificateFile input = file2;
                        // input.Data = Convert.ToBase64String(File.ReadAllBytes(newServiceCertificatePath));
                        input.Data = Convert.ToBase64String(newServiceCertificateBytes);
                        channel.AddCertificates(subscriptionId, serviceName, input);

                        if (WebOperationContext.Current.IncomingResponse != null)
                        {

                            opid = WebOperationContext.Current.IncomingResponse.Headers["x-ms-request-id"];
                            nullable = new HttpStatusCode?(WebOperationContext.Current.IncomingResponse.StatusCode);
                            statusDescription = WebOperationContext.Current.IncomingResponse.StatusDescription;

                        }
                    }
                    catch (CommunicationException exception)
                    {
                        throw exception;
                    }
                    finally
                    {
                        if (nullable.HasValue)
                        {
                            Console.WriteLine("HTTP Status Code: {0}", nullable);
                            Console.WriteLine("StatusDescription: {0}", statusDescription);
                        }
                    }
                }

                //if (((opid != null) && nullable.HasValue) && (((HttpStatusCode)nullable) == HttpStatusCode.Accepted))
                //{
                //    Console.WriteLine("Waiting for async operation to complete:");
                //    WaitForAsyncOperation(channel, opid, subscriptionId);
                //    Console.WriteLine();
                //}
            }
            catch (TimeoutException)
            {
                throw;
            }


        }

        #endregion

        #region DeleteCertificate
        public static void DeleteCertificate(ref IServiceManagement channel, string subscriptionId, string serviceName, string apiCertThumpprint, string cert_algorithm, string certificateToDelete, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {

            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            if (channel == null)
            {
                channel = GetChannelByThumbprint(apiCertThumpprint);
            }

            DeleteCertificate(ref channel, subscriptionId, serviceName, cert_algorithm, certificateToDelete, out opid, out nullable, out statusDescription);


        }


        public static void DeleteCertificate(ref IServiceManagement channel, string subscriptionId, string serviceName, byte[] apiCertBytes, string apiCertPassword, string cert_algorithm, string certificateToDelete, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {

            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            if (channel == null)
            {
                channel = GetChannel(apiCertBytes, apiCertPassword);
            }

            DeleteCertificate(ref channel, subscriptionId, serviceName, cert_algorithm, certificateToDelete, out opid, out nullable, out statusDescription);


        }

        public static void DeleteCertificate(ref IServiceManagement channel, string subscriptionId, string serviceName, X509Certificate2 apiCert, string cert_algorithm, string certificateToDelete, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {

            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            if (channel == null)
            {
                channel = GetChannel(apiCert);
            }

            DeleteCertificate(ref channel, subscriptionId, serviceName, cert_algorithm, certificateToDelete, out opid, out nullable, out statusDescription);


        }
        private static void DeleteCertificate(ref IServiceManagement channel, string subscriptionId, string serviceName, string cert_algorithm, string certificateToDelete, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {

            opid = string.Empty;
            nullable = null;
            statusDescription = null;
            try
            {


                using (new OperationContextScope((IContextChannel)channel))
                {
                    try
                    {
                        channel.DeleteCertificate(subscriptionId, serviceName, cert_algorithm, certificateToDelete);


                        if (WebOperationContext.Current.IncomingResponse != null)
                        {

                            opid = WebOperationContext.Current.IncomingResponse.Headers["x-ms-request-id"];
                            nullable = new HttpStatusCode?(WebOperationContext.Current.IncomingResponse.StatusCode);
                            statusDescription = WebOperationContext.Current.IncomingResponse.StatusDescription;

                        }
                    }
                    catch (CommunicationException exception)
                    {
                        throw exception;
                    }
                    finally
                    {
                        if (nullable.HasValue)
                        {
                            Console.WriteLine("HTTP Status Code: {0}", nullable);
                            Console.WriteLine("StatusDescription: {0}", statusDescription);
                        }
                    }
                }

                //if (((opid != null) && nullable.HasValue) && (((HttpStatusCode)nullable) == HttpStatusCode.Accepted))
                //{
                //    Console.WriteLine("Waiting for async operation to complete:");
                //    WaitForAsyncOperation(channel, opid, subscriptionId);
                //    Console.WriteLine();
                //}
            }
            catch (TimeoutException)
            {
                throw;
            }


        }

        #endregion

        #region ListCertificates
        public static CertificateList ListCertificates(ref IServiceManagement channel, string subscriptionId, string certificateThumbPrint, string hostedServiceName, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;


            if (channel == null)
            {
                channel = GetChannelByThumbprint(certificateThumbPrint);
            }

            return ListCertificates(ref channel, subscriptionId, hostedServiceName, out opid, out nullable, out statusDescription);
        }

        public static CertificateList ListCertificates(ref IServiceManagement channel, string subscriptionId, byte[] certBytes, string password, string hostedServiceName, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;


            if (channel == null)
            {
                channel = GetChannel(certBytes, password);
            }

            return ListCertificates(ref channel, subscriptionId, hostedServiceName, out opid, out nullable, out statusDescription);
        }

        public static CertificateList ListCertificates(ref IServiceManagement channel, string subscriptionId, X509Certificate2 cert, string hostedServiceName, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;


            if (channel == null)
            {
                channel = GetChannel(cert);
            }

            return ListCertificates(ref channel, subscriptionId, hostedServiceName, out opid, out nullable, out statusDescription);
        }

        private static CertificateList ListCertificates(ref IServiceManagement channel, string subscriptionId, string hostedServiceName, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;
            CertificateList hl = null;
            try
            {


                using (new OperationContextScope((IContextChannel)channel))
                {
                    try
                    {
                        hl = channel.ListCertificates(subscriptionId, hostedServiceName);
                        if (WebOperationContext.Current.IncomingResponse != null)
                        {

                            opid = WebOperationContext.Current.IncomingResponse.Headers["x-ms-request-id"];
                            nullable = new HttpStatusCode?(WebOperationContext.Current.IncomingResponse.StatusCode);
                            statusDescription = WebOperationContext.Current.IncomingResponse.StatusDescription;

                        }
                    }
                    catch (CommunicationException exception)
                    {
                        throw exception;
                    }
                    finally
                    {
                        if (nullable.HasValue)
                        {
                            Console.WriteLine("HTTP Status Code: {0}", nullable);
                            Console.WriteLine("StatusDescription: {0}", statusDescription);
                        }
                    }
                }

                //if (((opid != null) && nullable.HasValue) && (((HttpStatusCode)nullable) == HttpStatusCode.Accepted))
                //{

                //    WaitForAsyncOperation(channel, opid, subscriptionId);

                //}
            }
            catch (TimeoutException)
            {
                throw;
            }

            return hl;
        }

        #endregion
        #endregion

        #region OS List
        public static OperatingSystemList ListOperatingSystems(ref IServiceManagement channel, string subscriptionId, string certificateThumbPrint, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;


            if (channel == null)
            {
                channel = GetChannelByThumbprint(certificateThumbPrint);
            }
            return ListOperatingSystems(ref channel, subscriptionId, out opid, out nullable, out statusDescription);
        }

        public static OperatingSystemList ListOperatingSystems(ref IServiceManagement channel, string subscriptionId, byte[] certBytes, string password, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;


            if (channel == null)
            {
                channel = GetChannel(certBytes, password);
            }
            return ListOperatingSystems(ref channel, subscriptionId, out opid, out nullable, out statusDescription);
        }

        public static OperatingSystemList ListOperatingSystems(ref IServiceManagement channel, string subscriptionId, X509Certificate2 cert, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;


            if (channel == null)
            {
                channel = GetChannel(cert);
            }
            return ListOperatingSystems(ref channel, subscriptionId, out opid, out nullable, out statusDescription);
        }
        private static OperatingSystemList ListOperatingSystems(ref IServiceManagement channel, string subscriptionId, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;
            OperatingSystemList ol = null;


            using (new OperationContextScope((IContextChannel)channel))
            {
                try
                {
                    ol = channel.ListOperatingSystems(subscriptionId);
                    if (WebOperationContext.Current.IncomingResponse != null)
                    {

                        opid = WebOperationContext.Current.IncomingResponse.Headers["x-ms-request-id"];
                        nullable = new HttpStatusCode?(WebOperationContext.Current.IncomingResponse.StatusCode);
                        statusDescription = WebOperationContext.Current.IncomingResponse.StatusDescription;

                    }
                }
                catch (CommunicationException exception)
                {
                    throw exception;
                }

            }

            //if (((opid != null) && nullable.HasValue) && (((HttpStatusCode)nullable) == HttpStatusCode.Accepted))
            //{

            //    WaitForAsyncOperation(channel, opid, subscriptionId);

            //}

            return ol;

        }
        #endregion

        #region Location List
        public static LocationList ListLocations(ref IServiceManagement channel, string subscriptionId, string certificateThumbPrint, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;


            if (channel == null)
            {
                channel = GetChannelByThumbprint(certificateThumbPrint);
            }
            return ListLocations(ref channel, subscriptionId, out opid, out nullable, out statusDescription);
        }

        public static LocationList ListLocations(ref IServiceManagement channel, string subscriptionId, X509Certificate2 cert, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;


            if (channel == null)
            {
                channel = GetChannel(cert);
            }
            return ListLocations(ref channel, subscriptionId, out opid, out nullable, out statusDescription);
        }

        public static LocationList ListLocations(ref IServiceManagement channel, string subscriptionId, byte[] certificateBytes, string password, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            if (channel == null)
            {
                channel = GetChannel(certificateBytes, password);
            }

            return ListLocations(ref channel, subscriptionId, out opid, out nullable, out statusDescription);
        }

        private static LocationList ListLocations(ref IServiceManagement channel, string subscriptionId, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;
            LocationList ll = null;

            using (new OperationContextScope((IContextChannel)channel))
            {
                try
                {
                    ll = channel.ListLocations(subscriptionId);
                    if (WebOperationContext.Current.IncomingResponse != null)
                    {

                        opid = WebOperationContext.Current.IncomingResponse.Headers["x-ms-request-id"];
                        nullable = new HttpStatusCode?(WebOperationContext.Current.IncomingResponse.StatusCode);
                        statusDescription = WebOperationContext.Current.IncomingResponse.StatusDescription;

                    }
                }
                catch (CommunicationException exception)
                {
                    throw exception;
                }

            }

            //if (((opid != null) && nullable.HasValue) && (((HttpStatusCode)nullable) == HttpStatusCode.Accepted))
            //{

            //    WaitForAsyncOperation(channel, opid, subscriptionId);

            //}


            return ll;

        }
        #endregion

        #region Walk Upgrade Domain

        #region WalkUpgradeDomain
        public static void WalkUpgradeDomain(ref IServiceManagement channel, string subscriptionId, string serviceName, string deploymentName, int domainNumber, string certificateThumbPrint, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {

            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            if (channel == null)
            {
                channel = GetChannelByThumbprint(certificateThumbPrint);
            }
            WalkUpgradeDomain(ref channel, subscriptionId, serviceName, deploymentName, domainNumber, out opid, out nullable, out statusDescription);
        }

        public static void WalkUpgradeDomain(ref IServiceManagement channel, string subscriptionId, string serviceName, string deploymentName, int domainNumber, byte[] certBytes, string password, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {

            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            if (channel == null)
            {
                channel = GetChannel(certBytes, password);
            }
            WalkUpgradeDomain(ref channel, subscriptionId, serviceName, deploymentName, domainNumber, out opid, out nullable, out statusDescription);
        }

        public static void WalkUpgradeDomain(ref IServiceManagement channel, string subscriptionId, string serviceName, string deploymentName, int domainNumber, X509Certificate2 cert, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {

            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            if (channel == null)
            {
                channel = GetChannel(cert);
            }
            WalkUpgradeDomain(ref channel, subscriptionId, serviceName, deploymentName, domainNumber, out opid, out nullable, out statusDescription);
        }
        private static void WalkUpgradeDomain(ref IServiceManagement channel, string subscriptionId, string serviceName, string deploymentName, int domainNumber, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {

            opid = string.Empty;
            nullable = null;
            statusDescription = null;
            try
            {

                using (new OperationContextScope((IContextChannel)channel))
                {
                    channel.WalkUpgradeDomain(subscriptionId, serviceName, deploymentName
                     , new WalkUpgradeDomainInput() { UpgradeDomain = domainNumber });
                    if (WebOperationContext.Current.IncomingResponse != null)
                    {

                        opid = WebOperationContext.Current.IncomingResponse.Headers["x-ms-request-id"];
                        nullable = new HttpStatusCode?(WebOperationContext.Current.IncomingResponse.StatusCode);
                        statusDescription = WebOperationContext.Current.IncomingResponse.StatusDescription;

                    }
                }//using

            }
            catch (Exception)
            {
                throw;
            }

            //if (((opid != null) && nullable.HasValue) && (((HttpStatusCode)nullable) == HttpStatusCode.Accepted))
            //{

            //    WaitForAsyncOperation(channel, opid, subscriptionId);

            //}
        }
        #endregion
        #region WalkUpgradeDomainBySlot
        public static void WalkUpgradeDomainBySlot(ref IServiceManagement channel, string subscriptionId, string serviceName, string deploymentSlot, int domainNumber, string certificateThumbPrint, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            if (channel == null)
            {
                channel = GetChannelByThumbprint(certificateThumbPrint);
            }
            WalkUpgradeDomainBySlot(ref channel, subscriptionId, serviceName, deploymentSlot, domainNumber, out opid, out nullable, out statusDescription);
        }

        public static void WalkUpgradeDomainBySlot(ref IServiceManagement channel, string subscriptionId, string serviceName, string deploymentSlot, int domainNumber, byte[] certBytes, string password, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            if (channel == null)
            {
                channel = GetChannel(certBytes, password);
            }
            WalkUpgradeDomainBySlot(ref channel, subscriptionId, serviceName, deploymentSlot, domainNumber, out opid, out nullable, out statusDescription);
        }

        public static void WalkUpgradeDomainBySlot(ref IServiceManagement channel, string subscriptionId, string serviceName, string deploymentSlot, int domainNumber, X509Certificate2 cert, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            if (channel == null)
            {
                channel = GetChannel(cert);
            }
            WalkUpgradeDomainBySlot(ref channel, subscriptionId, serviceName, deploymentSlot, domainNumber, out opid, out nullable, out statusDescription);
        }

        private static void WalkUpgradeDomainBySlot(ref IServiceManagement channel, string subscriptionId, string serviceName, string deploymentSlot, int domainNumber, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;
            try
            {

                using (new OperationContextScope((IContextChannel)channel))
                {
                    channel.WalkUpgradeDomainBySlot(subscriptionId, serviceName, deploymentSlot
                        , new WalkUpgradeDomainInput() { UpgradeDomain = domainNumber });
                    if (WebOperationContext.Current.IncomingResponse != null)
                    {

                        opid = WebOperationContext.Current.IncomingResponse.Headers["x-ms-request-id"];
                        nullable = new HttpStatusCode?(WebOperationContext.Current.IncomingResponse.StatusCode);
                        statusDescription = WebOperationContext.Current.IncomingResponse.StatusDescription;

                    }
                }//using
            }
            catch (Exception)
            {
                throw;
            }


            //if (((opid != null) && nullable.HasValue) && (((HttpStatusCode)nullable) == HttpStatusCode.Accepted))
            //{

            //    WaitForAsyncOperation(channel, opid, subscriptionId);

            //}
        }

        #endregion
        #endregion

        #region Hosted Service

        #region CreateHostedService
        public static void CreateHostedService(ref IServiceManagement channel, string subscriptionId, string serviceName, string serviceLabel, string description, string location, string affinityGroup, string certificateThumbPrint, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            if (channel == null)
            {
                channel = GetChannelByThumbprint(certificateThumbPrint);

            }
            CreateHostedService(ref channel, subscriptionId, serviceName, serviceLabel, description, location, affinityGroup, out opid, out nullable, out statusDescription);


        }

        public static void CreateHostedService(ref IServiceManagement channel, string subscriptionId, string serviceName, string serviceLabel, string description, string location, string affinityGroup, X509Certificate2 cert, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            if (channel == null)
            {
                channel = GetChannel(cert);

            }
            CreateHostedService(ref channel, subscriptionId, serviceName, serviceLabel, description, location, affinityGroup, out opid, out nullable, out statusDescription);


        }

        public static void CreateHostedService(ref IServiceManagement channel, string subscriptionId, string serviceName, string serviceLabel, string description, string location, string affinityGroup, byte[] certificateBytes, string password, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            if (channel == null)
            {
                channel = GetChannel(certificateBytes);

            }
            CreateHostedService(ref channel, subscriptionId, serviceName, serviceLabel, description, location, affinityGroup, out opid, out nullable, out statusDescription);
        }

        private static void CreateHostedService(ref IServiceManagement channel, string subscriptionId, string serviceName, string serviceLabel, string description, string location, string affinityGroup, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;
            try
            {

                CreateHostedServiceInput input2;


                using (new OperationContextScope((IContextChannel)channel))
                {


                    input2 = new CreateHostedServiceInput();
                    input2.ServiceName = serviceName;
                    input2.Description = description;
                    if (!string.IsNullOrEmpty(affinityGroup))
                    {
                        input2.AffinityGroup = affinityGroup;
                    }
                    else
                    {
                        input2.Location = location;
                    }

                    if (!string.IsNullOrEmpty(serviceLabel))
                    {
                        input2.Label = ServiceManagementHelper.EncodeToBase64String(serviceLabel);
                    }

                    channel.CreateHostedService(subscriptionId, input2);
                    if (WebOperationContext.Current.IncomingResponse != null)
                    {

                        opid = WebOperationContext.Current.IncomingResponse.Headers["x-ms-request-id"];
                        nullable = new HttpStatusCode?(WebOperationContext.Current.IncomingResponse.StatusCode);
                        statusDescription = WebOperationContext.Current.IncomingResponse.StatusDescription;

                    }



                }


                //if (((opid != null) && nullable.HasValue) && (((HttpStatusCode)nullable) == HttpStatusCode.Accepted))
                //{

                //    WaitForAsyncOperation(channel, opid, subscriptionId);

                //}
            }
            catch (TimeoutException)
            {
                throw;
            }
        }

        #endregion

        #region UpdateHostedService

        public static void UpdateHostedService(ref IServiceManagement channel, string subscriptionId, string serviceName, string serviceLabel, string description, string certificateThumbPrint, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            if (channel == null)
            {
                channel = GetChannelByThumbprint(certificateThumbPrint);

            }
            UpdateHostedService(ref channel, subscriptionId, serviceName, serviceLabel, description, out opid, out nullable, out statusDescription);


        }

        public static void UpdateHostedService(ref IServiceManagement channel, string subscriptionId, string serviceName, string serviceLabel, string description, X509Certificate2 cert, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            if (channel == null)
            {
                channel = GetChannel(cert);

            }
            UpdateHostedService(ref channel, subscriptionId, serviceName, serviceLabel, description, out opid, out nullable, out statusDescription);


        }

        public static void UpdateHostedService(ref IServiceManagement channel, string subscriptionId, string serviceName, string serviceLabel, string description, byte[] certificateBytes, string password, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            if (channel == null)
            {
                channel = GetChannel(certificateBytes, password);

            }
            UpdateHostedService(ref channel, subscriptionId, serviceName, serviceLabel, description, out opid, out nullable, out statusDescription);
        }

        private static void UpdateHostedService(ref IServiceManagement channel, string subscriptionId, string serviceName, string serviceLabel, string description, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            try
            {

                UpdateHostedServiceInput input2;


                using (new OperationContextScope((IContextChannel)channel))
                {


                    input2 = new UpdateHostedServiceInput();
                    input2.Description = description;


                    if (!string.IsNullOrEmpty(serviceLabel))
                    {
                        input2.Label = ServiceManagementHelper.EncodeToBase64String(serviceLabel);
                    }

                    channel.UpdateHostedService(subscriptionId, serviceName, input2);
                    if (WebOperationContext.Current.IncomingResponse != null)
                    {

                        opid = WebOperationContext.Current.IncomingResponse.Headers["x-ms-request-id"];
                        nullable = new HttpStatusCode?(WebOperationContext.Current.IncomingResponse.StatusCode);
                        statusDescription = WebOperationContext.Current.IncomingResponse.StatusDescription;

                    }



                }


                //if (((opid != null) && nullable.HasValue) && (((HttpStatusCode)nullable) == HttpStatusCode.Accepted))
                //{

                //    WaitForAsyncOperation(channel, opid, subscriptionId);

                //}
            }
            catch (TimeoutException)
            {
                throw;
            }
        }

        #endregion

        #region DeleteHostedService
        public static void DeleteHostedService(ref IServiceManagement channel, string subscriptionId, string serviceName, string certificateThumbPrint, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            if (channel == null)
            {
                channel = GetChannelByThumbprint(certificateThumbPrint);

            }


            DeleteHostedService(ref channel, subscriptionId, serviceName, out opid, out nullable, out statusDescription);
        }

        public static void DeleteHostedService(ref IServiceManagement channel, string subscriptionId, string serviceName, X509Certificate2 cert, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            if (channel == null)
            {
                channel = GetChannel(cert);

            }


            DeleteHostedService(ref channel, subscriptionId, serviceName, out opid, out nullable, out statusDescription);
        }
        public static void DeleteHostedService(ref IServiceManagement channel, string subscriptionId, string serviceName, byte[] certificateBytes, string password, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;


            if (channel == null)
            {
                channel = GetChannel(certificateBytes, password);

            }

            DeleteHostedService(ref channel, subscriptionId, serviceName, out opid, out nullable, out statusDescription);
        }

        private static void DeleteHostedService(ref IServiceManagement channel, string subscriptionId, string serviceName, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            try
            {


                using (new OperationContextScope((IContextChannel)channel))
                {


                    channel.DeleteHostedService(subscriptionId, serviceName);
                    if (WebOperationContext.Current.IncomingResponse != null)
                    {

                        opid = WebOperationContext.Current.IncomingResponse.Headers["x-ms-request-id"];
                        nullable = new HttpStatusCode?(WebOperationContext.Current.IncomingResponse.StatusCode);
                        statusDescription = WebOperationContext.Current.IncomingResponse.StatusDescription;

                    }



                }


                //if (((opid != null) && nullable.HasValue) && (((HttpStatusCode)nullable) == HttpStatusCode.Accepted))
                //{

                //    WaitForAsyncOperation(channel, opid, subscriptionId);

                //}
            }
            catch (TimeoutException)
            {
                throw;
            }
        }

        #endregion

        #region GetHostedService
        public static HostedService GetHostedService(ref IServiceManagement channel, string subscriptionId, string serviceName, string certificateThumbPrint, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            if (channel == null)
            {
                channel = GetChannelByThumbprint(certificateThumbPrint);
            }


            return GetHostedService(ref channel, subscriptionId, serviceName, out opid, out nullable, out statusDescription);
        }

        public static HostedService GetHostedService(ref IServiceManagement channel, string subscriptionId, string serviceName, X509Certificate2 cert, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            if (channel == null)
            {
                channel = GetChannel(cert);
            }


            return GetHostedService(ref channel, subscriptionId, serviceName, out opid, out nullable, out statusDescription);
        }


        public static HostedService GetHostedService(ref IServiceManagement channel, string subscriptionId, string serviceName, byte[] certificateBytes, string password, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            if (channel == null)
            {
                channel = GetChannel(certificateBytes, password);
            }

            return GetHostedService(ref channel, subscriptionId, serviceName, out opid, out nullable, out statusDescription);
        }

        private static HostedService GetHostedService(ref IServiceManagement channel, string subscriptionId, string serviceName, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;
            HostedService hl = null;
            try
            {


                using (new OperationContextScope((IContextChannel)channel))
                {
                    try
                    {
                        hl = channel.GetHostedService(subscriptionId, serviceName);
                        if (WebOperationContext.Current.IncomingResponse != null)
                        {

                            opid = WebOperationContext.Current.IncomingResponse.Headers["x-ms-request-id"];
                            nullable = new HttpStatusCode?(WebOperationContext.Current.IncomingResponse.StatusCode);
                            statusDescription = WebOperationContext.Current.IncomingResponse.StatusDescription;

                        }
                    }
                    catch (CommunicationException exception)
                    {
                        throw exception;
                    }
                    finally
                    {
                        if (nullable.HasValue)
                        {
                            Console.WriteLine("HTTP Status Code: {0}", nullable);
                            Console.WriteLine("StatusDescription: {0}", statusDescription);
                        }
                    }
                }

                //if (((opid != null) && nullable.HasValue) && (((HttpStatusCode)nullable) == HttpStatusCode.Accepted))
                //{
                //    Console.WriteLine("Waiting for async operation to complete:");
                //    WaitForAsyncOperation(channel, opid, subscriptionId);
                //    Console.WriteLine();
                //}
            }
            catch (TimeoutException)
            {
                Console.WriteLine("There was an error processing this command.");
            }

            return hl;
        }

        #endregion

        #region GetHostedServiceWithDetails
        public static HostedService GetHostedServiceWithDetails(ref IServiceManagement channel, string subscriptionId, string serviceName, bool embed_detail, string certificateThumbPrint, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            if (channel == null)
            {
                channel = GetChannelByThumbprint(certificateThumbPrint);
            }

            return GetHostedServiceWithDetails(ref channel, subscriptionId, serviceName, embed_detail, out opid, out nullable, out statusDescription);
        }
        public static HostedService GetHostedServiceWithDetails(ref IServiceManagement channel, string subscriptionId, string serviceName, bool embed_detail, byte[] certificateBytes, string password, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            if (channel == null)
            {
                channel = GetChannel(certificateBytes, password);
            }

            return GetHostedServiceWithDetails(ref channel, subscriptionId, serviceName, embed_detail, out opid, out nullable, out statusDescription);
        }

        public static HostedService GetHostedServiceWithDetails(ref IServiceManagement channel, string subscriptionId, string serviceName, bool embed_detail, X509Certificate2 cert, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            if (channel == null)
            {
                channel = GetChannel(cert);
            }

            return GetHostedServiceWithDetails(ref channel, subscriptionId, serviceName, embed_detail, out opid, out nullable, out statusDescription);
        }
        private static HostedService GetHostedServiceWithDetails(ref IServiceManagement channel, string subscriptionId, string serviceName, bool embed_detail, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;
            HostedService hl = null;
            try
            {

                using (new OperationContextScope((IContextChannel)channel))
                {
                    try
                    {
                        hl = channel.GetHostedServiceWithDetails(subscriptionId, serviceName, embed_detail);
                        if (WebOperationContext.Current.IncomingResponse != null)
                        {

                            opid = WebOperationContext.Current.IncomingResponse.Headers["x-ms-request-id"];
                            nullable = new HttpStatusCode?(WebOperationContext.Current.IncomingResponse.StatusCode);
                            statusDescription = WebOperationContext.Current.IncomingResponse.StatusDescription;

                        }
                    }
                    catch (CommunicationException exception)
                    {
                        throw exception;
                    }
                    finally
                    {
                        if (nullable.HasValue)
                        {
                            Console.WriteLine("HTTP Status Code: {0}", nullable);
                            Console.WriteLine("StatusDescription: {0}", statusDescription);
                        }
                    }
                }

                //if (((opid != null) && nullable.HasValue) && (((HttpStatusCode)nullable) == HttpStatusCode.Accepted))
                //{
                //    WaitForAsyncOperation(channel, opid, subscriptionId);
                //    Console.WriteLine();
                //}
            }
            catch (TimeoutException)
            {
                throw;
            }

            return hl;
        }


        #endregion

        #region ListHostedServices
        public static HostedServiceList ListHostedServices(ref IServiceManagement channel, string subscriptionId, string certificateThumbPrint, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;


            if (channel == null)
            {
                channel = GetChannelByThumbprint(certificateThumbPrint);
            }

            return ListHostedServices(ref channel, subscriptionId, out opid, out nullable, out statusDescription);
        }

        public static HostedServiceList ListHostedServices(ref IServiceManagement channel, string subscriptionId, byte[] certBytes, string password, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;


            if (channel == null)
            {
                channel = GetChannel(certBytes, password);
            }

            return ListHostedServices(ref channel, subscriptionId, out opid, out nullable, out statusDescription);
        }

        public static HostedServiceList ListHostedServices(ref IServiceManagement channel, string subscriptionId, X509Certificate2 cert, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;


            if (channel == null)
            {
                channel = GetChannel(cert);
            }

            return ListHostedServices(ref channel, subscriptionId, out opid, out nullable, out statusDescription);
        }

        private static HostedServiceList ListHostedServices(ref IServiceManagement channel, string subscriptionId, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;
            HostedServiceList hl = null;
            try
            {


                using (new OperationContextScope((IContextChannel)channel))
                {
                    try
                    {
                        hl = channel.ListHostedServices(subscriptionId);
                        if (WebOperationContext.Current.IncomingResponse != null)
                        {

                            opid = WebOperationContext.Current.IncomingResponse.Headers["x-ms-request-id"];
                            nullable = new HttpStatusCode?(WebOperationContext.Current.IncomingResponse.StatusCode);
                            statusDescription = WebOperationContext.Current.IncomingResponse.StatusDescription;

                        }
                    }
                    catch (CommunicationException exception)
                    {
                        throw exception;
                    }
                    finally
                    {
                        if (nullable.HasValue)
                        {
                            Console.WriteLine("HTTP Status Code: {0}", nullable);
                            Console.WriteLine("StatusDescription: {0}", statusDescription);
                        }
                    }
                }

                //if (((opid != null) && nullable.HasValue) && (((HttpStatusCode)nullable) == HttpStatusCode.Accepted))
                //{
                //    Console.WriteLine("Waiting for async operation to complete:");
                //    WaitForAsyncOperation(channel, opid, subscriptionId);
                //    Console.WriteLine();
                //}
            }
            catch (TimeoutException)
            {
                Console.WriteLine("There was an error processing this command.");
            }

            return hl;
        }

        #endregion

        #endregion

        #region Change Config

        #region ChangeConfiguration
        public static void ChangeConfiguration(ref IServiceManagement channel, string subscriptionId, string serviceName, string deploymentName, string configFilePath, string certificateThumbPrint, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            if (channel == null)
            {
                channel = GetChannelByThumbprint(certificateThumbPrint);
            }

            ChangeConfiguration(ref channel, subscriptionId, serviceName, deploymentName, GetEncodedConfiguration(configFilePath), out opid, out nullable, out statusDescription);

        }

        public static void ChangeConfiguration(ref IServiceManagement channel, string subscriptionId, string serviceName, string deploymentName, string configFilePath, byte[] certBytes, string password, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            if (channel == null)
            {
                channel = GetChannel(certBytes, password);
            }

            ChangeConfiguration(ref channel, subscriptionId, serviceName, deploymentName, GetEncodedConfiguration(configFilePath), out opid, out nullable, out statusDescription);

        }

        public static void ChangeConfiguration(ref IServiceManagement channel, string subscriptionId, string serviceName, string deploymentName, string configFilePath, X509Certificate2 cert, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            if (channel == null)
            {
                channel = GetChannel(cert);
            }

            ChangeConfiguration(ref channel, subscriptionId, serviceName, deploymentName, GetEncodedConfiguration(configFilePath), out opid, out nullable, out statusDescription);

        }

        private static void ChangeConfiguration(ref IServiceManagement channel, string subscriptionId, string serviceName, string deploymentName, string encodedFileContent, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;
            try
            {


                using (new OperationContextScope((IContextChannel)channel))
                {
                    try
                    {
                        ChangeConfigurationInput input = new ChangeConfigurationInput();
                        input.Configuration = encodedFileContent;

                        channel.ChangeConfiguration(subscriptionId, serviceName, deploymentName, input);


                        if (WebOperationContext.Current.IncomingResponse != null)
                        {

                            opid = WebOperationContext.Current.IncomingResponse.Headers["x-ms-request-id"];
                            nullable = new HttpStatusCode?(WebOperationContext.Current.IncomingResponse.StatusCode);
                            statusDescription = WebOperationContext.Current.IncomingResponse.StatusDescription;

                        }
                    }
                    catch (CommunicationException exception)
                    {
                        throw exception;
                    }
                    finally
                    {
                        if (nullable.HasValue)
                        {
                            Console.WriteLine("HTTP Status Code: {0}", nullable);
                            Console.WriteLine("StatusDescription: {0}", statusDescription);
                        }
                    }
                }

                //if (((opid != null) && nullable.HasValue) && (((HttpStatusCode)nullable) == HttpStatusCode.Accepted))
                //{
                //    Console.WriteLine("Waiting for async operation to complete:");
                //    WaitForAsyncOperation(channel, opid, subscriptionId);
                //    Console.WriteLine();
                //}
            }
            catch (TimeoutException)
            {
                throw;
            }

        }

        #endregion

        #region ChangeConfigurationBySlot
        public static void ChangeConfigurationBySlot(ref IServiceManagement channel, string subscriptionId, string serviceName, string deploymentSlot, string configFilePath, string certificateThumbPrint, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            if (channel == null)
            {
                channel = GetChannelByThumbprint(certificateThumbPrint);
            }
            ChangeConfigurationBySlot(ref channel, subscriptionId, serviceName, deploymentSlot, GetEncodedConfiguration(configFilePath), out opid, out nullable, out statusDescription);
        }

        public static void ChangeConfigurationBySlot(ref IServiceManagement channel, string subscriptionId, string serviceName, string deploymentSlot, string configFilePath, byte[] certBytes, string password, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            if (channel == null)
            {
                channel = GetChannel(certBytes, password);
            }
            ChangeConfigurationBySlot(ref channel, subscriptionId, serviceName, deploymentSlot, GetEncodedConfiguration(configFilePath), out opid, out nullable, out statusDescription);
        }

        public static void ChangeConfigurationBySlot(ref IServiceManagement channel, string subscriptionId, string serviceName, string deploymentSlot, string configFilePath, X509Certificate2 cert, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            if (channel == null)
            {
                channel = GetChannel(cert);
            }
            ChangeConfigurationBySlot(ref channel, subscriptionId, serviceName, deploymentSlot, GetEncodedConfiguration(configFilePath), out opid, out nullable, out statusDescription);
        }

        private static void ChangeConfigurationBySlot(ref IServiceManagement channel, string subscriptionId, string serviceName, string deploymentSlot, string encodedConfigFileContent, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;
            try
            {


                using (new OperationContextScope((IContextChannel)channel))
                {
                    try
                    {
                        ChangeConfigurationInput input = new ChangeConfigurationInput();
                        input.Configuration = encodedConfigFileContent;

                        channel.ChangeConfigurationBySlot(subscriptionId, serviceName, deploymentSlot, input);


                        if (WebOperationContext.Current.IncomingResponse != null)
                        {

                            opid = WebOperationContext.Current.IncomingResponse.Headers["x-ms-request-id"];
                            nullable = new HttpStatusCode?(WebOperationContext.Current.IncomingResponse.StatusCode);
                            statusDescription = WebOperationContext.Current.IncomingResponse.StatusDescription;

                        }
                    }
                    catch (CommunicationException exception)
                    {
                        throw exception;
                    }
                    finally
                    {
                        if (nullable.HasValue)
                        {
                            Console.WriteLine("HTTP Status Code: {0}", nullable);
                            Console.WriteLine("StatusDescription: {0}", statusDescription);
                        }
                    }
                }

                //if (((opid != null) && nullable.HasValue) && (((HttpStatusCode)nullable) == HttpStatusCode.Accepted))
                //{
                //    Console.WriteLine("Waiting for async operation to complete:");
                //    WaitForAsyncOperation(channel, opid, subscriptionId);
                //    Console.WriteLine();
                //}
            }
            catch (TimeoutException)
            {
                throw;
            }

        }

        #endregion

        #region ChangeConfigurationByConfigString
        public static void ChangeConfigurationByConfigString(ref IServiceManagement channel, string subscriptionId, string serviceName, string deploymentName, string configString, string certificateThumbPrint, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            if (channel == null)
            {
                channel = GetChannelByThumbprint(certificateThumbPrint);
            }

            ChangeConfigurationByConfigString(ref channel, subscriptionId, serviceName, deploymentName, configString, out opid, out nullable, out statusDescription);
        }

        public static void ChangeConfigurationByConfigString(ref IServiceManagement channel, string subscriptionId, string serviceName, string deploymentName, string configString, byte[] certBytes, string password, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            if (channel == null)
            {
                channel = GetChannel(certBytes, password);
            }

            ChangeConfigurationByConfigString(ref channel, subscriptionId, serviceName, deploymentName, configString, out opid, out nullable, out statusDescription);
        }

        public static void ChangeConfigurationByConfigString(ref IServiceManagement channel, string subscriptionId, string serviceName, string deploymentName, string configString, X509Certificate2 cert, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            if (channel == null)
            {
                channel = GetChannel(cert);
            }

            ChangeConfigurationByConfigString(ref channel, subscriptionId, serviceName, deploymentName, configString, out opid, out nullable, out statusDescription);
        }
        private static void ChangeConfigurationByConfigString(ref IServiceManagement channel, string subscriptionId, string serviceName, string deploymentName, string configString, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            string encodedConfig = ServiceManagementHelper.EncodeToBase64String(configString);
            ChangeConfiguration(ref channel, subscriptionId, serviceName, deploymentName, encodedConfig, out opid, out nullable, out statusDescription);

        }

        #endregion

        #region ChangeConfigurationByConfigStringBySlot
        public static void ChangeConfigurationByConfigStringBySlot(ref IServiceManagement channel, string subscriptionId, string serviceName, string deploymentSlot, string configString, string certificateThumbPrint, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            if (channel == null)
            {
                channel = GetChannelByThumbprint(certificateThumbPrint);
            }



            ChangeConfigurationByConfigStringBySlot(ref channel, subscriptionId, serviceName, deploymentSlot, configString, out opid, out nullable, out statusDescription);

        }

        public static void ChangeConfigurationByConfigStringBySlot(ref IServiceManagement channel, string subscriptionId, string serviceName, string deploymentSlot, string configString, byte[] certBytes, string password, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            if (channel == null)
            {
                channel = GetChannel(certBytes, password);
            }



            ChangeConfigurationByConfigStringBySlot(ref channel, subscriptionId, serviceName, deploymentSlot, configString, out opid, out nullable, out statusDescription);

        }

        public static void ChangeConfigurationByConfigStringBySlot(ref IServiceManagement channel, string subscriptionId, string serviceName, string deploymentSlot, string configString, X509Certificate2 cert, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            if (channel == null)
            {
                channel = GetChannel(cert);
            }



            ChangeConfigurationByConfigStringBySlot(ref channel, subscriptionId, serviceName, deploymentSlot, configString, out opid, out nullable, out statusDescription);

        }

        private static void ChangeConfigurationByConfigStringBySlot(ref IServiceManagement channel, string subscriptionId, string serviceName, string deploymentSlot, string nonencodedConfigString, out string opid, out HttpStatusCode? nullable, out string statusDescription)
        {
            opid = string.Empty;
            nullable = null;
            statusDescription = null;

            string encodedConfigFileContent = ServiceManagementHelper.EncodeToBase64String(nonencodedConfigString);

            ChangeConfigurationBySlot(ref channel, subscriptionId, serviceName, deploymentSlot, encodedConfigFileContent, out opid, out nullable, out statusDescription);

        }

        #endregion

        #region GetConfiguration and Change Instances
        internal static string GetEncodedConfiguration(string configurationFilePath)
        {
            string configValue = null;
            try
            {
                configValue = string.Join("", File.ReadAllLines(Path.GetFullPath(configurationFilePath)));

            }
            catch (Exception)
            {

                throw;
            }
            return ServiceManagementHelper.EncodeToBase64String(configValue);
        }


        static public string GetServiceConfig(string deploymentInfoXML)
        {
            //get the service configuration out of the deployment configuration
            XElement deploymentInfo = XElement.Parse(deploymentInfoXML);
            string encodedServiceConfig = (from element in deploymentInfo.Elements()
                                           where element.Name.LocalName.Trim().ToLower() == "configuration"
                                           select (string)element.Value).Single();

            string CurrentServiceConfigText = System.Text.ASCIIEncoding.ASCII.GetString(System.Convert.FromBase64String(encodedServiceConfig));

            return CurrentServiceConfigText;
        }

        static public string GetInstanceCountFromConfigByRole(string serviceConfigXML, string roleName)
        {
            //make the service config queryable
            XElement xServiceConfig = XElement.Parse(serviceConfigXML);

            XElement webRoleElement = (from element in xServiceConfig.Elements()
                                       where element.Attribute("name").Value == roleName
                                       select element).Single();

            string currentInstanceCount = (from childelement in webRoleElement.Elements()
                                           where childelement.Name.LocalName.Trim().ToLower() == "instances"
                                           select (string)childelement.Attribute("count").Value).FirstOrDefault();
            return currentInstanceCount;

        }

        public static string ChangeInstanceCount(string serviceConfigXML, string roleName, string newCount)
        {
            string returnConfig = default(string);
            XElement XServiceConfig = XElement.Parse(serviceConfigXML);

            XElement WebRoleElement = (from element in XServiceConfig.Elements()
                                       where element.Attribute("name").Value == roleName
                                       select element).Single();

            XElement InstancesElement = (from childelement in WebRoleElement.Elements()
                                         where childelement.Name.LocalName.Trim().ToLower() == "instances"
                                         select childelement).Single();

            InstancesElement.SetAttributeValue("count", newCount.ToString());

            StringBuilder xml = new StringBuilder();
            XServiceConfig.Save(new StringWriter(xml));
            returnConfig = xml.ToString();

            return returnConfig;
        }

        #endregion

        #endregion
        #endregion
    }
}
