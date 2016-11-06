﻿//---------------------------------------------------------------------------------
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
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Collections.ObjectModel;

namespace Microsoft.Samples.WindowsAzure.ServiceManagement
{
    /// <summary>
    /// List of operating systems.
    /// </summary>
    [CollectionDataContract(Name = "OperatingSystems", ItemName = "OperatingSystem", Namespace = Constants.ServiceManagementNS)]
    public class OperatingSystemList : List<OperatingSystem>
    {
        public OperatingSystemList()
        {
        }

        public OperatingSystemList(IEnumerable<OperatingSystem> operatingSystem)
            : base(operatingSystem)
        {
        }
    }

    /// <summary>
    /// An operating system supported in Windows Azure.
    /// </summary>
    [DataContract(Namespace = Constants.ServiceManagementNS)]
    public class OperatingSystem : IExtensibleDataObject
    {
        [DataMember(Order = 1)]
        public string Version { get; set; }

        [DataMember(Order = 2, EmitDefaultValue = false)]
        public string Label { get; set; }

        [DataMember(Order = 3)]
        public bool IsDefault { get; set; }

        [DataMember(Order = 4)]
        public bool IsActive { get; set; }

        [DataMember(Order = 5)]
        public string Family { get; set; }

        [DataMember(Order = 6)]
        public string FamilyLabel { get; set; }

        public ExtensionDataObject ExtensionData { get; set; }
    }

    /// <summary>
    /// The operating-system-specific interface of the resource model service.
    /// </summary>
    public partial interface IServiceManagement
    {
        #region List Operating Systems

        /// <summary>
        /// Lists all available operating systems.
        /// </summary>
        [OperationContract(AsyncPattern = true)]
        [WebInvoke(Method = "GET", UriTemplate = @"{subscriptionId}/operatingsystems")]
        IAsyncResult BeginListOperatingSystems(string subscriptionId, AsyncCallback callback, object state);
        OperatingSystemList EndListOperatingSystems(IAsyncResult asyncResult);

        #endregion
    }
    
    /// <summary>
    /// Extensions of the IServiceManagement interface that allows clients to call operations synchronously.
    /// </summary>
    public static partial class ServiceManagementExtensionMethods
    {
        public static OperatingSystemList ListOperatingSystems(this IServiceManagement proxy, string subscriptionId)
        {
            return proxy.EndListOperatingSystems(proxy.BeginListOperatingSystems(subscriptionId, null, null));
        }
    }     
}
