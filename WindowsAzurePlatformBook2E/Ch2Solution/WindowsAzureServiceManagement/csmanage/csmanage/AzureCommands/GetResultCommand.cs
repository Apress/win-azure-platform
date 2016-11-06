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
        public static string OperationId { get; set; }
        public static bool ValidateOperationId()
        {
            if (string.IsNullOrEmpty(OperationId))
            {
                Console.WriteLine("OperationId is null or empty.");
                return false;
            }

            return true;
        }
    }

    /// <summary>
    /// Implements GetResult Command for getting status of Asynchronous operation
    /// </summary>
    public class GetResultCommand : CSManageCommand
    {
        public override bool Validate()
        {
            return ValidateOperationId();
        }

        protected override void PerformOperation(IServiceManagement channel)
        {
            Console.WriteLine("Getting Operation Status");
            var operation = channel.GetOperationStatus(SubscriptionId, OperationId);
            Console.WriteLine("Requested Status={0}", operation.Status);
        }
    }
}

