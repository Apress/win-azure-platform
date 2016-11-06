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

    public partial class CSManageCommand
    {
        //[Pimary|Secondary]
        public static string KeyType { get; set; }
        public static bool ValidateKeyType()
        {
            if (string.IsNullOrEmpty(KeyType))
            {
                Console.WriteLine("KeyType is null or empty.");
                return false;
            }

            return true;
        }
    }

    class ListStorageServicesCommmand : CSManageCommand
    {
        public override bool Validate()
        {
            return true;
        }

        protected override void PerformOperation(IServiceManagement channel)
        {
            Console.WriteLine("Listing StorageServices");
            var storageServices = channel.ListStorageServices(SubscriptionId);
            Utility.LogObject(storageServices);
        }
    }

    class RegenerateStorageServiceKeysCommmand : CSManageCommand
    {
        public override bool Validate()
        {
            return ValidateKeyType();
        }

        protected override void PerformOperation(IServiceManagement channel)
        {
            Console.WriteLine("Regenerating Storage Key");
            var key = new RegenerateKeys()
            {
                KeyType = KeyType
            };
            var keys = channel.RegenerateStorageServiceKeys(SubscriptionId, StorageServiceName, key);
            Utility.LogObject(keys);
        }
    }

    class GetStorageKeysCommmand : CSManageCommand
    {
        public override bool Validate()
        {
            return ValidateStorageServiceName();
        }

        protected override void PerformOperation(IServiceManagement channel)
        {
            Console.WriteLine("Getting Storage Keys");
            var storageService = channel.GetStorageKeys(SubscriptionId, StorageServiceName);
            Utility.LogObject(storageService);
        }
    }
}