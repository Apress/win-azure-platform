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

using System;
using System.Diagnostics;
using System.IO;
using System.Configuration;
using System.Net.Security;
namespace Microsoft.Samples.WindowsAzure.ServiceManagement.Tools
{
    class Program : ParseArgs
    {
        CSManageCommand command = new CSManageCommand();
        Program()
        {            
        }
       
        bool hasErrors = false;

        protected void LogMessage(string message, params object[] args)
        {
            Console.Out.WriteLine(message, args);
        }

        protected override void LogError(string message, params object[] args)
        {
            Console.Error.WriteLine(message, args);
            hasErrors = true;
        }

        public bool ExecuteActions(string[] args)
        {
            try
            {
                ParseArguments(args);
                if (hasErrors)
                {
                    LogMessage("Use /? for command line syntax.");
                }
            }
            catch (Exception e)
            {
                LogError("Encountered and unexpected error {0} {1}.", e.Message, e.StackTrace);
            }
            return hasErrors;
        }

        #region Argument Parsing Code
        private void ParseArguments(string[] args)
        {
            FlagTokens rest = null;
            bool noArgs = true;
            foreach (var tok in GetTokens(args))
            {
                noArgs = false;
                if (tok.flag == null)
                {
                    rest = tok;
                }
                else
                {
                    switch (tok.flag.ToLowerInvariant())
                    {
                        case "/?":
                        case "/help":
                            command.Usage();
                            return;
                        case "/hosted-service":
                            {
                                CSManageCommand.HostedServiceName = tok.args[0];
                                break;
                            }
                        case "/storage-service":
                            {
                                CSManageCommand.StorageServiceName = tok.args[0];
                                break;
                            }
                        case "/affinity-group":
                            {
                                CSManageCommand.AffinityGroupName = tok.args[0];
                                break;
                            }
                        case "/name":
                            {
                                CSManageCommand.DeploymentName = tok.args[0];
                                break;
                            }
                        case "/show-deployments":
                            {
                                CSManageCommand.ShowDeploymentString = tok.args[0];
                                break;
                            }
                        case "/slot":
                            {
                                CSManageCommand.DeploymentSlot = tok.args[0];
                                break;
                            }
                        case "/key-type":
                            {
                                CSManageCommand.KeyType = tok.args[0];
                                break;
                            }
                        case "/config":
                            {
                                CSManageCommand.ConfigFileLocation = tok.args[0];
                                break;
                            }
                        case "/package":
                            {
                                CSManageCommand.PackageLocation = tok.args[0];
                                break;
                            }
                        case "/status":
                            {
                                CSManageCommand.DeploymentStatus = tok.args[0];
                                break;
                            }
                        case "/mode":
                            {
                                CSManageCommand.Mode = tok.args[0];
                                break;
                            }
                        case "/label":
                            {
                                CSManageCommand.DeploymentLabel = tok.args[0];
                                break;
                            }
                        case "/role":
                            {
                                CSManageCommand.RoleToUpgrade = tok.args[0];
                                break;
                            }
                        case "/domain":
                            {
                                CSManageCommand.UpgradeDomainString = tok.args[0];
                                break;
                            }
                        case "/source-deployment":
                            {
                                CSManageCommand.SourceDeploymentName = tok.args[0];
                                break;
                            }
                        case "/production-slot":
                            {
                                CSManageCommand.DeploymentNameInProduction = tok.args[0];
                                break;
                            }
                        case "/op-id":
                            {
                                CSManageCommand.OperationId = tok.args[0];
                                break;
                            }
                        case "/cert-file":
                            {
                                CSManageCommand.Cert_file = tok.args[0];
                                break;
                            }
                        case "/cert-format":
                            {
                                CSManageCommand.Cert_format = tok.args[0];
                                break;
                            }
                        case "/cert-password":
                            {
                                CSManageCommand.Cert_Password = tok.args[0];
                                break;
                            }
                        case "/cert-thumbprint":
                            {
                                CSManageCommand.Cert_Thumbprint = tok.args[0];
                                break;
                            }
                        case "/cert-algorithm":
                            {
                                CSManageCommand.Cert_Algorithm = tok.args[0];
                                break;
                            }
                    }
                }
            }
            if (noArgs)
            {
                command.Usage();
                return;
            }
            foreach (var tok in GetTokens(args))
            {
                if (tok.flag == null)
                {
                    continue;
                }
                switch (tok.flag.ToLowerInvariant())
                {
                    case "/view-properties":
                        {
                            ((new ViewPropertiesCommand())).Run();
                            return;
                        }
                    case "/list-hosted-services":
                        {
                            (new ListHostedServicesCommand()).Run();
                            return;
                        }
                    case "/list-affinity-groups":
                        {
                            (new ListAffinityGroupsCommmand()).Run();
                            return;
                        }
                    case "/list-storage-services":
                        {
                            (new ListStorageServicesCommmand()).Run();
                            return;
                        }
                    case "/view-keys":
                        {
                            (new GetStorageKeysCommmand()).Run();
                            return;
                        }
                    case "/regenerate-key":
                        {
                            (new RegenerateStorageServiceKeysCommmand()).Run();
                            return;
                        }
                    case "/view-deployment":
                        {
                            (new GetDeploymentCommand()).Run();
                            return;
                        }
                    case "/create-deployment":
                        {
                            (new CreateDeploymentCommand()).Run();
                            return;
                        }
                    case "/delete-deployment":
                        {
                            (new DeleteDeploymentCommand()).Run();
                            return;
                        }
                    case "/swap-deployment":
                        {
                            (new SwapDeploymentCommand()).Run();
                            return;
                        }
                    case "/change-deployment-config":
                        {
                            (new ChangeDeploymentConfigCommand()).Run();
                            return;
                        }
                    case "/update-deployment":
                        {
                            (new UpdateDeploymentStatusCommand()).Run();
                            return;
                        }
                    case "/upgrade-deployment":
                        {
                            (new UpgradeDeploymentCommand()).Run();
                            return;
                        }
                    case "/walk-upgrade-domain":
                        {
                            (new WalkUpgradeDomainCommand()).Run();
                            return;
                        }
                    case "/get-operation-status":
                        {
                            (new GetResultCommand()).Run();
                            return;
                        }
                    case "/add-certificate":
                        {
                            (new AddCertificatesCommand()).Run();
                            return;
                        }
                    case "/view-certificate":
                        {
                            (new GetCertificateCommand()).Run();
                            return;
                        }
                    case "/list-certificates":
                        {
                            (new ListCertificatesCommand()).Run();
                            return;
                        }
                    case "/delete-certificate":
                        {
                            (new DeleteCertificateCommand()).Run();
                            return;
                        }
                    case "/list-operating-systems":
                        {
                            (new ListOperatingSystemsCommmand()).Run();
                            return;
                        }
                    default:
                        {
                            LogError("Unknown Flag {0}", tok.flag);
                            return;
                        }
                }
            }
            LogError("No command specified");
            return;
        }
        #endregion
       
        static int Main(string[] args)
        {
            ProcessCheckServerCertificate();
            
            Program prgm = new Program();

            if (prgm.ExecuteActions(args))
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        static void ProcessCheckServerCertificate()
        {
            var CheckServerCertificateString = Utility.TryGetConfigurationSetting("CheckServerCertificate");

            bool checkServerCertificate = true;

            if (!string.IsNullOrEmpty(CheckServerCertificateString))
            {
                if(!Boolean.TryParse(CheckServerCertificateString, out checkServerCertificate))
                {
                    Console.WriteLine("The value of CheckServerCertificate cannot be recognized. Using true as the default value.");
                }
            }

            System.Net.ServicePointManager.ServerCertificateValidationCallback =
               ((sender, certificate, chain, sslPolicyErrors) => 
                {
                    if(!checkServerCertificate)
                        return true;
                    if (sslPolicyErrors == SslPolicyErrors.None)
                        return true;

                    Console.WriteLine("  Certificate error: {0}", sslPolicyErrors);

                    // Do not allow this client to communicate with unauthenticated servers.
                    return false;
                }
               );

        }
    }
}
