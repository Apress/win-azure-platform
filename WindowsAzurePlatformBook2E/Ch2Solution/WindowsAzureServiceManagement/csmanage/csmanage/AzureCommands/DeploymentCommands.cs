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
    using System.ServiceModel;
    using System.Globalization;

    public partial class CSManageCommand
    {
        public static string DeploymentName { get; set; }
        public static bool ValidateDeploymentName()
        {
            if (string.IsNullOrEmpty(DeploymentName))
            {
                Console.WriteLine("DeploymentName is null or empty.");
                return false;
            }
            return true;
        }

        public static string DeploymentSlot { get; set; }
        public static bool ValidateDeploymentSlot()
        {
            if (string.IsNullOrEmpty(DeploymentSlot))
            {
                Console.WriteLine("DeploymentSlot is null or empty.");
                return false;
            }
            return true;
        }

        public static bool ValidateDeploymentNameOrSlot()
        {
            if (string.IsNullOrEmpty(DeploymentSlot) && string.IsNullOrEmpty(DeploymentName))
            {
                Console.WriteLine("Both DeploymentName and DeploymentSlot are null or empty.");
                return false;
            }
            return true;
        }

        public static string DeploymentLabel { get; set; }
        public static bool ValidateDeploymentLabel()
        {
            if (string.IsNullOrEmpty(DeploymentLabel))
            {
                Console.WriteLine("DeploymentLabel is null or empty.");
                return false;
            }
            return true;
        }

        public static string DeploymentStatus { get; set; }
        public static bool ValidateDeploymentStatus()
        {
            if (string.IsNullOrEmpty(DeploymentStatus))
            {
                Console.WriteLine("DeploymentStatus is null or empty.");
                return false;
            }
            return true;
        }

        public static string PackageLocation { get; set; }
        public static bool ValidatePackageLocation()
        {
            if (string.IsNullOrEmpty(PackageLocation))
            {
                Console.WriteLine("PackageUrl is null or empty.");
                return false;
            }
            else
            {
                if (!Uri.IsWellFormedUriString(PackageLocation, UriKind.Absolute))
                {
                    Console.WriteLine("PackageUrl format error. It cannot be casted to Uri.");
                    return false;
                }
            }
            return true;
        }

        public static string ConfigFileLocation { get; set; }
        public static bool ValidateConfigFileLocation()
        {
            if (string.IsNullOrEmpty(ConfigFileLocation))
            {
                Console.WriteLine("ConfigFileLocation is null or empty.");
                return false;
            }
            else
            {
                if (!File.Exists(ConfigFileLocation))
                {
                    Console.WriteLine("The file {0} cannot be found or the caller does not have sufficient permissions to read it. ", ConfigFileLocation);
                    return false;
                }
            }

            return true;
        }

        public static string RoleToUpgrade { get; set; }
        public static bool ValidateRoleToUpgrade()
        {
            if (string.IsNullOrEmpty(RoleToUpgrade))
            {
                Console.WriteLine("RoleToUpgrade is null or empty.");
                return false;
            }
            return true;
        }

        public static string Mode { get; set; }
        public static bool ValidateMode()
        {
            if (string.IsNullOrEmpty(Mode))
            {
                Console.WriteLine("Mode is null or empty.");
                return false;
            }
            return true;
        }

        public static string UpgradeDomainString { get; set; }
        public static bool ValidateUpgradeDomainString()
        {
            if (string.IsNullOrEmpty(UpgradeDomainString))
            {
                Console.WriteLine("UpgradeDomain is null or empty.");
                return false;
            }
            else
            {
                Int32 domain;
                if (!Int32.TryParse(UpgradeDomainString, out domain))
                {
                    Console.WriteLine("UpgradeDomain should be an integer.");
                }
            }
            return true;
        }

        public static string SourceDeploymentName { get; set; }
        public static bool ValidateSourceDeploymentName()
        {
            if (string.IsNullOrEmpty(SourceDeploymentName))
            {
                Console.WriteLine("The name of the source deployment is null or empty.");
                return false;
            }
            return true;
        }

        public static string DeploymentNameInProduction { get; set; }
    }

    class CreateDeploymentCommand : CSManageCommand
    {
        public override bool Validate()
        {
            return ValidateHostedServiceName() && ValidateDeploymentName() && ValidateDeploymentLabel() && ValidateDeploymentSlot() && ValidateConfigFileLocation() && ValidatePackageLocation();
        }

        protected override void PerformOperation(IServiceManagement channel)
        {
            CreateDeploymentInput input = new CreateDeploymentInput
            {
                Name = DeploymentName,
                Configuration = Utility.GetSettings(ConfigFileLocation),
            };

            if(!string.IsNullOrEmpty(PackageLocation))
            {
                input.PackageUrl = new Uri(PackageLocation);
            }

            if(!string.IsNullOrEmpty(DeploymentLabel))
            {
                input.Label = ServiceManagementHelper.EncodeToBase64String(DeploymentLabel);
            }
            Console.WriteLine("Creating Deployment... Name: {0}, Label: {1}", DeploymentName, DeploymentLabel);
            channel.CreateOrUpdateDeployment(SubscriptionId, HostedServiceName, DeploymentSlot, input);
        }
    }

    class GetDeploymentCommand : CSManageCommand
    {
        public override bool Validate()
        {
            return ValidateHostedServiceName() && ValidateDeploymentNameOrSlot();
        }

        protected override void PerformOperation(IServiceManagement channel)
        {
            Console.WriteLine("Getting Deployment");
            Deployment deployment = null;

            if (!string.IsNullOrEmpty(DeploymentName))
            {
                deployment = channel.GetDeployment(SubscriptionId, HostedServiceName, DeploymentName);
            }
            else if (!string.IsNullOrEmpty(DeploymentSlot))
            {
                deployment = channel.GetDeploymentBySlot(SubscriptionId, HostedServiceName, DeploymentSlot);
                
            }
            Utility.LogObject(deployment);
        }
    }

    class ChangeDeploymentConfigCommand : CSManageCommand
    {

        public override bool Validate()
        {
            return ValidateHostedServiceName() && ValidateDeploymentNameOrSlot() && ValidateConfigFileLocation();
        }

        protected override void PerformOperation(IServiceManagement channel)
        {
            var input = new ChangeConfigurationInput();
            input.Configuration = Utility.GetSettings(ConfigFileLocation);
            Console.WriteLine("Updating Deployment");
            if (!string.IsNullOrEmpty(DeploymentName))
            {
                channel.ChangeConfiguration(SubscriptionId, HostedServiceName, DeploymentName, input);
            }
            else if (!string.IsNullOrEmpty(DeploymentSlot))
            {
                channel.ChangeConfigurationBySlot(SubscriptionId, HostedServiceName, DeploymentSlot, input);
            }
        }
    }

    class DeleteDeploymentCommand : CSManageCommand
    {
        public override bool Validate()
        {
            return ValidateHostedServiceName() && ValidateDeploymentNameOrSlot();
        }

        protected override void PerformOperation(IServiceManagement channel)
        {
            Console.WriteLine("Deleting Deployment");
            if (!string.IsNullOrEmpty(DeploymentName))
            {
                channel.DeleteDeployment(SubscriptionId, HostedServiceName, DeploymentName);
            }
            else if (!string.IsNullOrEmpty(DeploymentSlot))
            {
                channel.DeleteDeploymentBySlot(SubscriptionId, HostedServiceName, DeploymentSlot);
            }
        }
    }

    class UpdateDeploymentStatusCommand : CSManageCommand
    {

        public override bool Validate()
        {
            return ValidateHostedServiceName() && ValidateDeploymentNameOrSlot() && ValidateDeploymentStatus();
        }

        protected override void PerformOperation(IServiceManagement channel)
        {
            var input = new UpdateDeploymentStatusInput()
            {
                Status = DeploymentStatus
            };
            Console.WriteLine("Updating DeploymentStatus");
            if (!string.IsNullOrEmpty(DeploymentName))
            {
                channel.UpdateDeploymentStatus(SubscriptionId, HostedServiceName, DeploymentName, input);
            }
            else if (!string.IsNullOrEmpty(DeploymentSlot))
            {
                channel.UpdateDeploymentStatusBySlot(SubscriptionId, HostedServiceName, DeploymentSlot, input);
            }
        }
    }

    class SwapDeploymentCommand : CSManageCommand
    {
        public override bool Validate()
        {
            return ValidateHostedServiceName() && ValidateSourceDeploymentName();
        }

        protected override void PerformOperation(IServiceManagement channel)
        {
            SwapDeploymentInput input = new SwapDeploymentInput
            {
                Production = DeploymentNameInProduction,
                SourceDeployment = SourceDeploymentName
            };
            Console.WriteLine("Swapping Deployment");
            channel.SwapDeployment(SubscriptionId, HostedServiceName, input);
        }
    }

    class UpgradeDeploymentCommand : CSManageCommand
    {
        public override bool Validate()
        {
            return ValidateHostedServiceName() && ValidateDeploymentNameOrSlot() && ValidateDeploymentLabel() && ValidateConfigFileLocation() && ValidatePackageLocation();
        }

        protected override void PerformOperation(IServiceManagement channel)
        {
            UpgradeDeploymentInput input = new UpgradeDeploymentInput
            {
                Configuration = Utility.GetSettings(ConfigFileLocation),
                Mode = Mode,
                RoleToUpgrade = RoleToUpgrade,
            };

            if (!string.IsNullOrEmpty(PackageLocation))
            {
                input.PackageUrl = new Uri(PackageLocation);
            }

            if (!string.IsNullOrEmpty(DeploymentLabel))
            {
                input.Label = ServiceManagementHelper.EncodeToBase64String(DeploymentLabel);
            }
            Console.WriteLine("Upgrading Deployment");
            if (!string.IsNullOrEmpty(DeploymentName))
            {
                channel.UpgradeDeployment(SubscriptionId, HostedServiceName, DeploymentName, input);
            }
            else if (!string.IsNullOrEmpty(DeploymentSlot))
            {
                channel.UpgradeDeploymentBySlot(SubscriptionId, HostedServiceName, DeploymentSlot, input);
            }
        }
    }

    class WalkUpgradeDomainCommand : CSManageCommand
    {
        public override bool Validate()
        {
            return ValidateHostedServiceName() && ValidateDeploymentNameOrSlot() && ValidateUpgradeDomainString();
        }

        protected override void PerformOperation(IServiceManagement channel)
        {
            int upgradeDomain = -1;
            if (!string.IsNullOrEmpty(UpgradeDomainString))
            {
                upgradeDomain = int.Parse(UpgradeDomainString, CultureInfo.CurrentCulture);
            }

            WalkUpgradeDomainInput input = new WalkUpgradeDomainInput
            {
                UpgradeDomain = upgradeDomain
            };

            Console.WriteLine("Walking upgrade domain:" + input.UpgradeDomain);
            if (!string.IsNullOrEmpty(DeploymentName))
            {
                channel.WalkUpgradeDomain(SubscriptionId, HostedServiceName, DeploymentName, input);
            }
            else if (!string.IsNullOrEmpty(DeploymentSlot))
            {
                channel.WalkUpgradeDomainBySlot(SubscriptionId, HostedServiceName, DeploymentSlot, input);
            }
        }
    }
}
