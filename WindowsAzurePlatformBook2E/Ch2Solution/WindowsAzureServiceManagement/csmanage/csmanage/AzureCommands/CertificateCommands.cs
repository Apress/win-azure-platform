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

    using Microsoft.Samples.WindowsAzure.ServiceManagement;
    using System.Collections.Generic;
    using System.IO;

    public partial class CSManageCommand
    {
        public static string Cert_Algorithm { get; set; }
        protected bool ValidateCert_Algorithm()
        {
            if (string.IsNullOrEmpty(Cert_Algorithm))
            {
                Console.WriteLine("cert-algorithm is null or empty.");
                return false;
            }

            return true;
        }

        public static string Cert_Thumbprint { get; set; }
        protected bool ValidateCert_Thumbprint()
        {
            if (string.IsNullOrEmpty(Cert_Thumbprint))
            {
                Console.WriteLine("cert-thumbprint is null or empty.");
                return false;
            }

            return true;
        }

        public static string Cert_Password { get; set; }
        protected bool ValidateCert_Password()
        {
            if (string.IsNullOrEmpty(Cert_Password))
            {
                Console.WriteLine("cert-password is null or empty.");
                return false;
            }

            return true;
        }

        public static string Cert_file { get; set; }
        protected bool ValidateCert_filePath()
        {
            if (string.IsNullOrEmpty(Cert_file))
            {
                Console.WriteLine("Cert-file is null or empty.");
                return false;
            }
            else
            {
                try
                {
                    if (!File.Exists(Cert_file))
                    {
                        Console.WriteLine("The file {0} cannot be found. ", Cert_file);
                        return false;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Cert-file is invalid file path.");
                    return false;
                }
            }

            return true;
        }

        public static string Cert_format { get; set; }
        protected bool ValidateCert_format()
        {
            if (string.IsNullOrEmpty(Cert_format))
            {
                Console.WriteLine("Cert-format is null or empty.");
                return false;
            }

            return true;
        }
    }

    class ListCertificatesCommand : CSManageCommand
    {
        public override bool Validate()
        {
            return ValidateHostedServiceName();
        }

        protected override void PerformOperation(IServiceManagement channel)
        {
            Console.WriteLine("Listing Certificates");
            var obj = channel.ListCertificates(SubscriptionId, HostedServiceName);
            Utility.LogObject(obj);
        }
    }

    class GetCertificateCommand : CSManageCommand
    {
        public override bool Validate()
        {
            return ValidateHostedServiceName() && ValidateCert_Algorithm() && ValidateCert_Thumbprint();
        }

        protected override void PerformOperation(IServiceManagement channel)
        {
            Console.WriteLine("Getting Certificate");
            var obj = channel.GetCertificate(SubscriptionId, HostedServiceName, Cert_Algorithm, Cert_Thumbprint);
            Utility.LogObject(obj);
        }
    }

    class AddCertificatesCommand : CSManageCommand
    {
        public override bool Validate()
        {
            return ValidateHostedServiceName() && ValidateCert_filePath() && ValidateCert_format();
        }

        protected override void PerformOperation(IServiceManagement channel)
        {
            var input = new CertificateFile()
            {
                CertificateFormat = Cert_format,
                Password = Cert_Password,
            };

            input.Data = Convert.ToBase64String(File.ReadAllBytes(Cert_file));
            Console.WriteLine("Adding Certificate");
            channel.AddCertificates(SubscriptionId, HostedServiceName, input);
        }
    }

    class DeleteCertificateCommand : CSManageCommand
    {
        public override bool Validate()
        {
            return ValidateHostedServiceName() && ValidateCert_Algorithm() && ValidateCert_Thumbprint();
        }

        protected override void PerformOperation(IServiceManagement channel)
        {
            Console.WriteLine("Deleting certificate");
            channel.DeleteCertificate(SubscriptionId, HostedServiceName, Cert_Algorithm, Cert_Thumbprint);
        }
    }
}