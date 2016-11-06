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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace Microsoft.Samples.WindowsAzure.ServiceManagement
{
    /// <summary>
    /// List of storage services
    /// </summary>
    [CollectionDataContract(Name = "StorageServices", ItemName = "StorageService", Namespace = Constants.ServiceManagementNS)]
    public class StorageServiceList : List<StorageService>
    {
        public StorageServiceList()
        {
        }

        public StorageServiceList(IEnumerable<StorageService> storageServices)
            : base(storageServices)
        {
        }
    }

    [DataContract(Namespace = Constants.ServiceManagementNS)]
    public class StorageService : IExtensibleDataObject
    {
        [DataMember(Order = 1)]
        public Uri Url { get; set; }

        [DataMember(Order = 2, EmitDefaultValue = false)]
        public string ServiceName { get; set; }

        [DataMember(Order = 3, EmitDefaultValue = false)]
        public StorageServiceProperties StorageServiceProperties { get; set; }

        [DataMember(Order = 4, EmitDefaultValue = false)]
        public StorageServiceKeys StorageServiceKeys { get; set; }

        public ExtensionDataObject ExtensionData { get; set; }
    }

    [DataContract(Namespace = Constants.ServiceManagementNS)]
    public class StorageServiceProperties : IExtensibleDataObject
    {
        [DataMember(Order = 1)]
        public string Description { get; set; }

        [DataMember(Order = 2, EmitDefaultValue = false)]
        public string AffinityGroup { get; set; }

        [DataMember(Order = 3, EmitDefaultValue = false)]
        public string Location { get; set; }

        [DataMember(Order = 4)]
        public string Label { get; set; }

        public ExtensionDataObject ExtensionData { get; set; }
    }

    [DataContract(Namespace = Constants.ServiceManagementNS)]
    public class StorageServiceKeys : IExtensibleDataObject
    {
        [DataMember(Order = 1)]
        public string Primary { get; set; }

        [DataMember(Order = 2)]
        public string Secondary { get; set; }

        public ExtensionDataObject ExtensionData { get; set; }
    }

    [DataContract(Namespace = Constants.ServiceManagementNS)]
    public class RegenerateKeys: IExtensibleDataObject
    {
        [DataMember(Order = 1)]
        public string KeyType { get; set; }

        public ExtensionDataObject ExtensionData { get; set; }
    }

    [DataContract(Name = "CreateStorageServiceInput", Namespace = Constants.ServiceManagementNS)]
    public class CreateStorageServiceInput : IExtensibleDataObject
    {
        [DataMember(Order = 1)]
        public string ServiceName { get; set; }

        [DataMember(Order = 2)]
        public string Description { get; set; }

        [DataMember(Order = 3)]
        public string Label { get; set; }

        [DataMember(Order = 4, EmitDefaultValue = false)]
        public string AffinityGroup { get; set; }
        [DataMember(Order = 5, EmitDefaultValue = false)]
        public string Location { get; set; }


        public ExtensionDataObject ExtensionData { get; set; }
    }

    [DataContract(Name = "UpdateStorageServiceInput", Namespace = Constants.ServiceManagementNS)]
    public class UpdateStorageServiceInput : IExtensibleDataObject
    {

        [DataMember(Order = 1, EmitDefaultValue = false)]
        public string Description { get; set; }
        [DataMember(Order = 2, EmitDefaultValue = false)]
        public string Label { get; set; }

        public ExtensionDataObject ExtensionData { get; set; }
    }
    /// <summary>
    /// The storage service-related part of the API
    /// </summary>
    public partial interface IServiceManagement
    {
        #region Create Storage Service

        /// <summary>
        /// Creates a hosted service.
        /// </summary>
        [OperationContract(AsyncPattern = true)]
        [WebInvoke(Method = "POST", UriTemplate = @"{subscriptionId}/services/storageservices")]
        IAsyncResult BeginCreateStorageService(string subscriptionId, CreateStorageServiceInput input, AsyncCallback callback, object state);
        void EndCreateStorageService(IAsyncResult asyncResult);

        #endregion

        #region Delete Storage Service

        /// <summary>
        /// Creates a hosted service.
        /// </summary>
        [OperationContract(AsyncPattern = true)]
        [WebInvoke(Method = "DELETE", UriTemplate = @"{subscriptionId}/services/storageservices/{serviceName}")]
        IAsyncResult BeginDeleteStorageService(string subscriptionId, string serviceName, AsyncCallback callback, object state);
        void EndDeleteStorageService(IAsyncResult asyncResult);

        #endregion

        #region Update Storage Service

        /// <summary>
        /// Creates a hosted service.
        /// </summary>
        [OperationContract(AsyncPattern = true)]
        [WebInvoke(Method = "POST", UriTemplate = @"{subscriptionId}/services/storageservices/{serviceName}")]
        IAsyncResult BeginUpdateStorageService(string subscriptionId, string serviceName, UpdateStorageServiceInput input, AsyncCallback callback, object state);
        void EndUpdateStorageService(IAsyncResult asyncResult);

        #endregion
        /// <summary>
        /// Lists the storage services associated with a given subscription.
        /// </summary>
        [OperationContract(AsyncPattern = true)]
        [WebGet(UriTemplate = @"{subscriptionId}/services/storageservices")]
        IAsyncResult BeginListStorageServices(string subscriptionId, AsyncCallback callback, object state);
        StorageServiceList EndListStorageServices(IAsyncResult asyncResult);

        /// <summary>
        /// Gets a storage service.
        /// </summary>
        [OperationContract(AsyncPattern = true)]
        [WebGet(UriTemplate = @"{subscriptionId}/services/storageservices/{serviceName}")]
        IAsyncResult BeginGetStorageService(string subscriptionId, string serviceName, AsyncCallback callback, object state);
        StorageService EndGetStorageService(IAsyncResult asyncResult);

        /// <summary>
        /// Gets the key of a storage service.
        /// </summary>
        [OperationContract(AsyncPattern = true)]
        [WebGet(UriTemplate = @"{subscriptionId}/services/storageservices/{serviceName}/keys")]
        IAsyncResult BeginGetStorageKeys(string subscriptionId, string serviceName, AsyncCallback callback, object state);
        StorageService EndGetStorageKeys(IAsyncResult asyncResult);

        /// <summary>
        /// Regenerates keys associated with a storage service.
        /// </summary>
        [OperationContract(AsyncPattern = true)]
        [WebInvoke(Method = "POST", UriTemplate = @"{subscriptionId}/services/storageservices/{serviceName}/keys?action=regenerate")]
        IAsyncResult BeginRegenerateStorageServiceKeys(string subscriptionId, string serviceName, RegenerateKeys regenerateKeys, AsyncCallback callback, object state);
        StorageService EndRegenerateStorageServiceKeys(IAsyncResult asyncResult);

   }

    public static partial class ServiceManagementExtensionMethods
    {
        public static void CreateStorageService(this IServiceManagement proxy, string subscriptionId, CreateStorageServiceInput input)
        {
            proxy.EndCreateStorageService(proxy.BeginCreateStorageService(subscriptionId, input, null, null));
        }

        public static void DeleteStorageService(this IServiceManagement proxy, string subscriptionId, string serviceName)
        {
            proxy.EndDeleteStorageService(proxy.BeginDeleteStorageService(subscriptionId, serviceName, null, null));
        }

        public static void UpdateStorageService(this IServiceManagement proxy, string subscriptionId, string serviceName, UpdateStorageServiceInput input)
        {
            //TODO: Update HostedService is returning Bad Request. Need to recheck.
            proxy.EndUpdateStorageService(proxy.BeginUpdateStorageService(subscriptionId, serviceName, input, null, null));
        }

        public static StorageServiceList ListStorageServices(this IServiceManagement proxy, string subscriptionId)
        {
            return proxy.EndListStorageServices(proxy.BeginListStorageServices(subscriptionId, null, null));
        }

        public static StorageService GetStorageService(this IServiceManagement proxy, string subscriptionId, string name)
        {
            return proxy.EndGetStorageService(proxy.BeginGetStorageService(subscriptionId, name, null, null));
        }

        public static StorageService GetStorageKeys(this IServiceManagement proxy, string subscriptionId, string name)
        {
            return proxy.EndGetStorageKeys(proxy.BeginGetStorageKeys(subscriptionId, name, null, null));
        }

        public static StorageService RegenerateStorageServiceKeys(this IServiceManagement proxy, string subscriptionId, string name, RegenerateKeys regenerateKeys)
        {
            return proxy.EndRegenerateStorageServiceKeys(proxy.BeginRegenerateStorageServiceKeys(subscriptionId, name, regenerateKeys, null, null));
        }
    }
}
