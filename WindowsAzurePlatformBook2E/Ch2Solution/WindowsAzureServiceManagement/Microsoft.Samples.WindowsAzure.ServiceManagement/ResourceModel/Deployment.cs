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
    [DataContract(Name = "Swap", Namespace = Constants.ServiceManagementNS)]
    public class SwapDeploymentInput : IExtensibleDataObject
    {
        [DataMember(Order = 1)]
        public string Production { get; set; }

        [DataMember(Order = 2)]
        public string SourceDeployment { get; set; }

        public ExtensionDataObject ExtensionData { get; set; }
    }

    /// <summary>
    /// This class represents a deployment in our deployment-related operations.
    /// </summary>
    [DataContract(Namespace = Constants.ServiceManagementNS)]
    public class Deployment : IExtensibleDataObject
    {
        [DataMember(Order = 1, EmitDefaultValue = false)]
        public string Name { get; set; }

        [DataMember(Order = 2, EmitDefaultValue = false)]
        public string DeploymentSlot { get; set; }

        [DataMember(Order = 3, EmitDefaultValue = false)]
        public string PrivateID { get; set; }

        /// <summary>
        /// The class DeploymentStatus defines its possible values. 
        /// </summary>
        [DataMember(Order = 4, EmitDefaultValue = false)]
        public string Status { get; set; }

        [DataMember(Order = 5, EmitDefaultValue = false)]
        public string Label { get; set; }

        [DataMember(Order = 6, EmitDefaultValue = false)]
        public Uri Url { get; set; }

        [DataMember(Order = 7, EmitDefaultValue = false)]
        public string Configuration { get; set; }

        [DataMember(Order = 8, EmitDefaultValue = false)]
        public RoleInstanceList RoleInstanceList { get; set; }

        [DataMember(Order = 10, EmitDefaultValue = false)]
        public UpgradeStatus UpgradeStatus { get; set; }

        [DataMember(Order = 11, EmitDefaultValue = false)]
        public int UpgradeDomainCount;

        [DataMember(Order = 12, EmitDefaultValue = false)]
        public RoleList RoleList { get; set; }

        public ExtensionDataObject ExtensionData { get; set; }
    }

    [CollectionDataContract(Name = "RoleList", ItemName = "Role", Namespace = Constants.ServiceManagementNS)]
    public class RoleList : List<Role>
    {
        public RoleList()
        {
        }

        public RoleList(IEnumerable<Role> roles)
            : base(roles)
        {
        }
    }

    [DataContract(Namespace = Constants.ServiceManagementNS)]
    public class Role : IExtensibleDataObject
    {
        [DataMember(Order = 1)]
        public string RoleName { get; set; }

        [DataMember(Order = 2)]
        public string OsVersion { get; set; }

        public ExtensionDataObject ExtensionData { get; set; }
    }

    [CollectionDataContract(Name = "RoleInstanceList", ItemName = "RoleInstance", Namespace = Constants.ServiceManagementNS)]
    public class RoleInstanceList : List<RoleInstance>
    {
        public RoleInstanceList()
        {
        }

        public RoleInstanceList(IEnumerable<RoleInstance> roles)
            : base(roles)
        {
        }
    }

    // @todo: this should implement IExtensibleDataObject. Can we do this without destroying backwards compatibility???
    [DataContract(Namespace = Constants.ServiceManagementNS)]
    public class RoleInstance : IExtensibleDataObject
    {
        [DataMember(Order = 1)]
        public string RoleName { get; set; }

        [DataMember(Order = 2)]
        public string InstanceName { get; set; }

        [DataMember(Order = 3)]
        public string InstanceStatus { get; set; }

        public ExtensionDataObject ExtensionData { get; set; }
    }

    [DataContract(Name = "CreateDeployment", Namespace = Constants.ServiceManagementNS)]
    public class CreateDeploymentInput : IExtensibleDataObject
    {
        [DataMember(Order = 1)]
        public string Name { get; set; }

        [DataMember(Order = 2)]
        public Uri PackageUrl { get; set; }

        [DataMember(Order = 3)]
        public string Label { get; set; }

        [DataMember(Order = 4)]
        public string Configuration { get; set; }
         [DataMember(Order = 5)]
        public bool StartDeployment { get; set; }


        public ExtensionDataObject ExtensionData { get; set; }
    }

   

    [DataContract(Name = "ChangeConfiguration", Namespace = Constants.ServiceManagementNS)]
    public class ChangeConfigurationInput : IExtensibleDataObject
    {
        [DataMember(Order = 1)]
        public string Configuration { get; set; }
        
        public ExtensionDataObject ExtensionData { get; set; }
    }

    [DataContract(Name = "UpdateDeploymentStatus", Namespace = Constants.ServiceManagementNS)]
    public class UpdateDeploymentStatusInput : IExtensibleDataObject
    {
        [DataMember(Order = 1)]
        public string Status { get; set; }

        public ExtensionDataObject ExtensionData { get; set; }
    }

    [DataContract(Name = "UpgradeDeployment", Namespace = Constants.ServiceManagementNS)]
    public class UpgradeDeploymentInput : IExtensibleDataObject
    {
        [DataMember(Order = 1)]
        public string Mode { get; set; }

        [DataMember(Order = 2)]
        public Uri PackageUrl { get; set; }

        [DataMember(Order = 3)]
        public string Configuration { get; set; }
        
        [DataMember(Order = 4)]
        public string Label { get; set; }

        [DataMember(Order = 5)]
        public string RoleToUpgrade { get; set; }

        public ExtensionDataObject ExtensionData { get; set; }
    }

    [DataContract(Name = "WalkUpgradeDomain", Namespace = Constants.ServiceManagementNS)]
    public class WalkUpgradeDomainInput : IExtensibleDataObject
    {
        [DataMember(Order = 1)]
        public int UpgradeDomain { get; set; }

        public ExtensionDataObject ExtensionData { get; set; }
    }

    [DataContract(Namespace = Constants.ServiceManagementNS)]
    public class UpgradeStatus : IExtensibleDataObject
    {
        [DataMember(Order = 1)]
        public string UpgradeType { get; set; }

        [DataMember(Order = 2)]
        public string CurrentUpgradeDomainState { get; set; }

        [DataMember(Order = 3)]
        public int CurrentUpgradeDomain { get; set; }

        public ExtensionDataObject ExtensionData { get; set; }
    }

    /// <summary>
    /// The deployment-specific interface of the resource model service.
    /// </summary>
    public partial interface IServiceManagement
    {
        #region Swap Deployment

        /// <summary>
        /// Swaps the deployment to a production slot.
        /// </summary>
        [OperationContract(AsyncPattern = true)]
        [WebInvoke(Method = "POST", UriTemplate = @"{subscriptionId}/services/hostedservices/{serviceName}")]
        IAsyncResult BeginSwapDeployment(string subscriptionId, string serviceName, SwapDeploymentInput input, AsyncCallback callback, object state);
        void EndSwapDeployment(IAsyncResult asyncResult);

        #endregion

       

        #region Create Deployment
        /// <summary>
        /// Creates a deployment.
        /// </summary>
        [OperationContract(AsyncPattern = true)]
        [WebInvoke(Method = "POST", UriTemplate = @"{subscriptionId}/services/hostedservices/{serviceName}/deploymentslots/{deploymentSlot}")]
        IAsyncResult BeginCreateOrUpdateDeployment(string subscriptionId, string serviceName, string deploymentSlot, CreateDeploymentInput input, AsyncCallback callback, object state);
        void EndCreateOrUpdateDeployment(IAsyncResult asyncResult);
        #endregion

        #region Delete Deployment

        /// <summary>
        /// Deletes the specified deployment. This works against either through the slot or through the name.This is an asynchronous operation.
        /// Only implements deleting by deployment name right now. 
        /// </summary>
        [OperationContract(AsyncPattern = true)]
        [WebInvoke(Method = "DELETE", UriTemplate = @"{subscriptionId}/services/hostedservices/{serviceName}/deployments/{deploymentName}")]
        IAsyncResult BeginDeleteDeployment(string subscriptionId, string serviceName, string deploymentName, AsyncCallback callback, object state);
        void EndDeleteDeployment(IAsyncResult asyncResult);

        /// <summary>
        /// Deletes the specified deployment. This works against either through the slot or through the name.This is an asynchronous operation.
        /// Only implements deleting by deployment name right now. 
        /// </summary>
        [OperationContract(AsyncPattern = true)]
        [WebInvoke(Method = "DELETE", UriTemplate = @"{subscriptionId}/services/hostedservices/{serviceName}/deploymentslots/{deploymentSlot}")]
        IAsyncResult BeginDeleteDeploymentBySlot(string subscriptionId, string serviceName, string deploymentSlot, AsyncCallback callback, object state);
        void EndDeleteDeploymentBySlot(IAsyncResult asyncResult);

        #endregion

        #region Get Deployment

        /// <summary>
        /// Gets the specified deployment details.
        /// </summary>
        [OperationContract(AsyncPattern = true)]
        [WebGet(UriTemplate = @"{subscriptionId}/services/hostedservices/{serviceName}/deployments/{deploymentName}")]
        IAsyncResult BeginGetDeployment(string subscriptionId, string serviceName, string deploymentName, AsyncCallback callback, object state);
        Deployment EndGetDeployment(IAsyncResult asyncResult);

        /// <summary>
        /// Gets the specified deployment details.
        /// </summary>
        [OperationContract(AsyncPattern = true)]
        [WebGet(UriTemplate = @"{subscriptionId}/services/hostedservices/{serviceName}/deploymentslots/{deploymentSlot}")]
        IAsyncResult BeginGetDeploymentBySlot(string subscriptionId, string serviceName, string deploymentSlot, AsyncCallback callback, object state);
        Deployment EndGetDeploymentBySlot(IAsyncResult asyncResult);

        #endregion

        #region Change Deployment Config

        /// <summary>
        /// Initiates a change to the deployment. This works against through the deployment name.
        /// This is an asynchronous operation
        /// </summary>
        [OperationContract(AsyncPattern = true)]
        [WebInvoke(Method = "POST", UriTemplate = @"{subscriptionId}/services/hostedservices/{serviceName}/deployments/{deploymentName}/?comp=config")]
        IAsyncResult BeginChangeConfiguration(string subscriptionId, string serviceName, string deploymentName, ChangeConfigurationInput input, AsyncCallback callback, object state);
        void EndChangeConfiguration(IAsyncResult asyncResult);

        /// <summary>
        /// Initiates a change to the deployment. This works against through the slot name.
        /// This is an asynchronous operation
        /// </summary>
        [OperationContract(AsyncPattern = true)]
        [WebInvoke(Method = "POST", UriTemplate = @"{subscriptionId}/services/hostedservices/{serviceName}/deploymentslots/{deploymentSlot}/?comp=config")]
        IAsyncResult BeginChangeConfigurationBySlot(string subscriptionId, string serviceName, string deploymentSlot, ChangeConfigurationInput input, AsyncCallback callback, object state);
        void EndChangeConfigurationBySlot(IAsyncResult asyncResult);

        #endregion

        #region Update Deployment Status

        /// <summary>
        /// Initiates a change to the deployment. This works against through the deployment name.
        /// This is an asynchronous operation
        /// </summary>
        [OperationContract(AsyncPattern = true)]
        [WebInvoke(Method = "POST", UriTemplate = @"{subscriptionId}/services/hostedservices/{serviceName}/deployments/{deploymentName}/?comp=status")]
        IAsyncResult BeginUpdateDeploymentStatus(string subscriptionId, string serviceName, string deploymentName, UpdateDeploymentStatusInput input, AsyncCallback callback, object state);
        void EndUpdateDeploymentStatus(IAsyncResult asyncResult);

        /// <summary>
        /// Initiates a change to the deployment. This works against through the slot name.
        /// This is an asynchronous operation
        /// </summary>
        [OperationContract(AsyncPattern = true)]
        [WebInvoke(Method = "POST", UriTemplate = @"{subscriptionId}/services/hostedservices/{serviceName}/deploymentslots/{deploymentSlot}/?comp=status")]
        IAsyncResult BeginUpdateDeploymentStatusBySlot(string subscriptionId, string serviceName, string deploymentSlot, UpdateDeploymentStatusInput input, AsyncCallback callback, object state);
        void EndUpdateDeploymentStatusBySlot(IAsyncResult asyncResult);

        #endregion

        #region Upgrade Deployment

        /// <summary>
        /// Initiates an deployment upgrade.
        /// This is an asynchronous operation
        /// </summary>
        [OperationContract(AsyncPattern = true)]
        [WebInvoke(Method = "POST", UriTemplate = @"{subscriptionId}/services/hostedservices/{serviceName}/deployments/{deploymentName}/?comp=upgrade")]
        IAsyncResult BeginUpgradeDeployment(string subscriptionId, string serviceName, string deploymentName, UpgradeDeploymentInput input, AsyncCallback callback, object state);
        void EndUpgradeDeployment(IAsyncResult asyncResult);

        /// <summary>
        /// Initiates an deployment upgrade through the slot name.
        /// This is an asynchronous operation
        /// </summary>
        [OperationContract(AsyncPattern = true)]
        [WebInvoke(Method = "POST", UriTemplate = @"{subscriptionId}/services/hostedservices/{serviceName}/deploymentslots/{deploymentSlot}/?comp=upgrade")]
        IAsyncResult BeginUpgradeDeploymentBySlot(string subscriptionId, string serviceName, string deploymentSlot, UpgradeDeploymentInput input, AsyncCallback callback, object state);
        void EndUpgradeDeploymentBySlot(IAsyncResult asyncResult);

        #endregion

        #region Walk Upgrade Domain

        /// <summary>
        /// Initiates an deployment upgrade.
        /// This is an asynchronous operation
        /// </summary>
        [OperationContract(AsyncPattern = true)]
        [WebInvoke(Method = "POST", UriTemplate = @"{subscriptionId}/services/hostedservices/{serviceName}/deployments/{deploymentName}/?comp=walkupgradedomain")]
        IAsyncResult BeginWalkUpgradeDomain(string subscriptionId, string serviceName, string deploymentName, WalkUpgradeDomainInput input, AsyncCallback callback, object state);
        void EndWalkUpgradeDomain(IAsyncResult asyncResult);

        /// <summary>
        /// Initiates an deployment upgrade through the slot name.
        /// This is an asynchronous operation
        /// </summary>
        [OperationContract(AsyncPattern = true)]
        [WebInvoke(Method = "POST", UriTemplate = @"{subscriptionId}/services/hostedservices/{serviceName}/deploymentslots/{deploymentSlot}/?comp=walkupgradedomain")]
        IAsyncResult BeginWalkUpgradeDomainBySlot(string subscriptionId, string serviceName, string deploymentSlot, WalkUpgradeDomainInput input, AsyncCallback callback, object state);
        void EndWalkUpgradeDomainBySlot(IAsyncResult asyncResult);

        #endregion
    }

    public static partial class ServiceManagementExtensionMethods
    {
        public static void SwapDeployment(this IServiceManagement proxy, string subscriptionId, string serviceName, SwapDeploymentInput input)
        {
            proxy.EndSwapDeployment(proxy.BeginSwapDeployment(subscriptionId, serviceName, input,  null, null));
        }

        public static void CreateOrUpdateDeployment(this IServiceManagement proxy, string subscriptionId, string serviceName, string deploymentSlot, CreateDeploymentInput input)
        {
            proxy.EndCreateOrUpdateDeployment(proxy.BeginCreateOrUpdateDeployment(subscriptionId, serviceName, deploymentSlot, input, null, null));
        }

     
        public static void DeleteDeployment(this IServiceManagement proxy, string subscriptionId, string serviceName, string deploymentName)
        {
            proxy.EndDeleteDeployment(proxy.BeginDeleteDeployment(subscriptionId, serviceName, deploymentName, null, null));
        }

        public static void DeleteDeploymentBySlot(this IServiceManagement proxy, string subscriptionId, string serviceName, string deploymentSlot)
        {
            proxy.EndDeleteDeploymentBySlot(proxy.BeginDeleteDeploymentBySlot(subscriptionId, serviceName, deploymentSlot, null, null));
        }

        public static Deployment GetDeployment(this IServiceManagement proxy, string subscriptionId, string serviceName, string deploymentName)
        {
            return proxy.EndGetDeployment(proxy.BeginGetDeployment(subscriptionId, serviceName, deploymentName, null, null));
        }

        public static Deployment GetDeploymentBySlot(this IServiceManagement proxy, string subscriptionId, string serviceName, string deploymentSlot)
        {
            return proxy.EndGetDeploymentBySlot(proxy.BeginGetDeploymentBySlot(subscriptionId, serviceName, deploymentSlot, null, null));
        }

        public static void UpdateDeploymentStatus(this IServiceManagement proxy, string subscriptionId, string serviceName, string deploymentName, UpdateDeploymentStatusInput input)
        {
            proxy.EndUpdateDeploymentStatus(proxy.BeginUpdateDeploymentStatus(subscriptionId, serviceName, deploymentName, input, null, null));
        }

        public static void UpdateDeploymentStatusBySlot(this IServiceManagement proxy, string subscriptionId, string serviceName, string deploymentSlot, UpdateDeploymentStatusInput input)
        {
            proxy.EndUpdateDeploymentStatusBySlot(proxy.BeginUpdateDeploymentStatusBySlot(subscriptionId, serviceName, deploymentSlot, input, null, null));
        }

        public static void ChangeConfiguration(this IServiceManagement proxy, string subscriptionId, string serviceName, string deploymentName, ChangeConfigurationInput input)
        {
            proxy.EndChangeConfiguration(proxy.BeginChangeConfiguration(subscriptionId, serviceName, deploymentName, input, null, null));
        }

        public static void ChangeConfigurationBySlot(this IServiceManagement proxy, string subscriptionId, string serviceName, string deploymentSlot, ChangeConfigurationInput input)
        {
            proxy.EndChangeConfigurationBySlot(proxy.BeginChangeConfigurationBySlot(subscriptionId, serviceName, deploymentSlot, input, null, null));
        }

        public static void UpgradeDeployment(this IServiceManagement proxy, string subscriptionId, string serviceName, string deploymentName, UpgradeDeploymentInput input)
        {
            proxy.EndUpgradeDeployment(proxy.BeginUpgradeDeployment(subscriptionId, serviceName, deploymentName, input, null, null));
        }

        public static void UpgradeDeploymentBySlot(this IServiceManagement proxy, string subscriptionId, string serviceName, string deploymentSlot, UpgradeDeploymentInput input)
        {
            proxy.EndUpgradeDeploymentBySlot(proxy.BeginUpgradeDeploymentBySlot(subscriptionId, serviceName, deploymentSlot, input, null, null));
        }

        public static void WalkUpgradeDomain(this IServiceManagement proxy, string subscriptionId, string serviceName, string deploymentName, WalkUpgradeDomainInput input)
        {
            proxy.EndWalkUpgradeDomain(proxy.BeginWalkUpgradeDomain(subscriptionId, serviceName, deploymentName, input, null, null));
        }

        public static void WalkUpgradeDomainBySlot(this IServiceManagement proxy, string subscriptionId, string serviceName, string deploymentSlot, WalkUpgradeDomainInput input)
        {
            proxy.EndWalkUpgradeDomainBySlot(proxy.BeginWalkUpgradeDomainBySlot(subscriptionId, serviceName, deploymentSlot, input, null, null));
        }
    }
}
