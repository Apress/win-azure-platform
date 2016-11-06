//---------------------------------------------------------------------------------
// Microsoft (R) Windows Azure SDK
// Software Development Kit
// 
// Copyright (c) Microsoft Corporation. All rights reserved.  
//
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE. 
//---------------------------------------------------------------------------------

namespace Microsoft.Samples.WindowsAzure.ServiceManagement.Tools
{
    using System;
    using System.Net;
    using System.Net.Security;
    using System.Security.Cryptography.X509Certificates;
    using System.ServiceModel;
    using System.ServiceModel.Web;
    using Microsoft.Samples.WindowsAzure.ServiceManagement;
    using System.IO;
    using System.Configuration;

    public partial class CSManageCommand
    {
        public const int PollTimeoutInSeconds = 1800;

        public CSManageCommand()
        {
        }

        //Common parameters for different commands
        public static string SubscriptionId { get; set; }
        public static bool ValidateSubscriptionId()
        {
            if (string.IsNullOrEmpty(SubscriptionId))
            {
                Console.WriteLine("SubscriptionId is null or empty.");
                return false;
            }
            return true;
        }

        public static string CertificateThumbprint { get; set; }
        public static X509Certificate2 Certificate { get; set; }
        public static bool ValidateCertificate()
        {
            if (String.IsNullOrEmpty(CertificateThumbprint))
            {
                Console.WriteLine("CertificateThumbprint cannot be found. Please check the config file. ");
                return false;
            }

            X509Store certificateStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            certificateStore.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection certs = certificateStore.Certificates.Find(X509FindType.FindByThumbprint, CertificateThumbprint, false);
            if (certs.Count != 1)
            {
                Console.WriteLine("Client certificate cannot be found. Please check the config file. ");
                return false;
            }
            Certificate = certs[0];
            return true;
        }

        public virtual void Usage()
        {
            Console.WriteLine(ResourceFile.Usage);
        }

        public virtual bool Validate() { return false; }
        
        protected virtual void PerformOperation(IServiceManagement channel) {}
        
        public void Run()
        {
            if (!ReadFromConfigFile() || !ValidateSubscriptionId() || !ValidateCertificate() || !this.Validate())
            {
                Console.WriteLine("There was an error processing this command!");
                return;
            }

            var serviceManagement = ServiceManagementHelper.CreateServiceManagementChannel("WindowsAzureEndPoint", CSManageCommand.Certificate);
            Console.WriteLine("Using certificate: " + CSManageCommand.Certificate.SubjectName.Name);

            try
            {
                string trackingId = null;
                HttpStatusCode? statusCode = null;
                string statusDescription = null;

                using (OperationContextScope scope = new OperationContextScope((IContextChannel)serviceManagement))
                {
                    try
                    {
                        this.PerformOperation(serviceManagement);
                        if (WebOperationContext.Current.IncomingResponse != null)
                        {
                            trackingId = WebOperationContext.Current.IncomingResponse.Headers[Constants.OperationTrackingIdHeader];
                            statusCode = WebOperationContext.Current.IncomingResponse.StatusCode;
                            statusDescription = WebOperationContext.Current.IncomingResponse.StatusDescription;
                            Console.WriteLine("Operation ID: {0}", trackingId);
                        }
                    }
                    catch (CommunicationException ce)
                    {
                        ServiceManagementError error = null;
                        HttpStatusCode httpStatusCode = 0;
                        string operationId;
                        ServiceManagementHelper.TryGetExceptionDetails(ce, out error, out httpStatusCode, out operationId);
                        if (error == null)
                        {
                            Console.WriteLine(ce.Message);
                        }
                        else
                        {
                            Console.WriteLine("HTTP Status Code: {0}", httpStatusCode);
                            Console.WriteLine("Error Message: {0}", error.Message);
                            Console.WriteLine("Operation Id: {0}", operationId);
                        } 
                    }
                    finally
                    {
                        if (statusCode != null)
                        {
                            Console.WriteLine("HTTP Status Code: {0}", statusCode);
                            Console.WriteLine("StatusDescription: {0}", statusDescription);
                        }
                    }
                }
                if (trackingId != null && statusCode != null && statusCode == HttpStatusCode.Accepted)
                {
                    Console.WriteLine("Waiting for async operation to complete:");
                    WaitForAsyncOperation(serviceManagement, trackingId);
                }
            }
            catch (TimeoutException)
            {
                Console.WriteLine("There was an error processing this command.");
            }          
        }

        private static void WaitForAsyncOperation(IServiceManagement service, string trackingId)
        {
            Operation tracking;
            int count = 0;
            do
            {                
                tracking = service.GetOperationStatus(SubscriptionId, trackingId);
                System.Threading.Thread.Sleep(1000);
                Console.Write(".");
                count++;
                if (CSManageCommand.PollTimeoutInSeconds > 0 && count > CSManageCommand.PollTimeoutInSeconds)
                {
                    Console.WriteLine();
                    Console.WriteLine("Giving up after {0} seconds. Call GetResult manually to get the status", CSManageCommand.PollTimeoutInSeconds);
                    break;
                }
            }
            while (tracking.Status != OperationState.Failed && tracking.Status != OperationState.Succeeded);
            
            if (tracking.Status != OperationState.InProgress)
                Console.WriteLine("Done");
            
            Console.WriteLine("Operation Status=" + tracking.Status.ToString());

            if (tracking.Status == OperationState.Failed)
            {
                Console.WriteLine("Error Message: {0}", tracking.Error.Message);
            }
        }

        private static bool ReadFromConfigFile()
        {
            CSManageCommand.CertificateThumbprint = Utility.TryGetConfigurationSetting("CertificateThumbprint");
            if (string.IsNullOrEmpty(CSManageCommand.CertificateThumbprint))
            {
                Console.WriteLine("Cannot find CertificateThumbprint in the config file");
                return false;
            }

            CSManageCommand.SubscriptionId = Utility.TryGetConfigurationSetting("SubscriptionId");
            if (string.IsNullOrEmpty(CSManageCommand.SubscriptionId))
            {
                Console.WriteLine("Cannot find SubscriptionId in the config file");
                return false;
            }

            return true;
        }

        
    }
}