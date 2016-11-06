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
    [CollectionDataContract(Name = "Locations", ItemName = "Location", Namespace = Constants.ServiceManagementNS)]
    public class LocationList : List<Location>
    {
        public LocationList()
        {
        }

        public LocationList(IEnumerable<Location> location)
            : base(location)
        {
        }
    }

    /// <summary>
    /// An operating system supported in Windows Azure.
    /// </summary>
    [DataContract(Namespace = Constants.ServiceManagementNS)]
    public class Location : IExtensibleDataObject
    {
        [DataMember(Order = 1)]
        public string Name { get; set; }

      

        public ExtensionDataObject ExtensionData { get; set; }
    }

    /// <summary>
    /// The location interface of the resource model service.
    /// </summary>
    public partial interface IServiceManagement
    {
        #region List Location

        /// <summary>
        /// Lists all available locations.
        /// </summary>
        [OperationContract(AsyncPattern = true)]
        [WebInvoke(Method = "GET", UriTemplate = @"{subscriptionId}/locations")]
        IAsyncResult BeginListLocations(string subscriptionId, AsyncCallback callback, object state);
        LocationList EndListLocations(IAsyncResult asyncResult);

        #endregion
    }

    /// <summary>
    /// Extensions of the IServiceManagement interface that allows clients to call operations synchronously.
    /// </summary>
    public static partial class ServiceManagementExtensionMethods
    {
        public static LocationList ListLocations(this IServiceManagement proxy, string subscriptionId)
        {
            return proxy.EndListLocations(proxy.BeginListLocations(subscriptionId, null, null));
        }
    }     
}
