using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Samples.WindowsAzure.ServiceManagement;
using System.Net.Security;
using System.Net;

namespace ServiceManagement
{
    public partial class ServiceManagementForm : Form
    {
        IServiceManagement channel = null;
        private string currentConfig = "";

        public ServiceManagementForm()
        {
            InitializeComponent();
            ddlResourceType.SelectedIndex = 0;
            ddlSlotType.SelectedIndex = 0;
            txtLog.AppendText(Environment.NewLine);
            txtSubscriptionId.Text = ConfigurationManager.AppSettings["SubscriptionID"];
            txtThumbprint.Text = ConfigurationManager.AppSettings["CertificateThumbprint"];
            LoadCertificate();
        }

        private void LoadCertificate()
        {
            try
            {
                
                X509Certificate cert = ServiceManagementUtil.GetCertificateByThumbprint(txtThumbprint.Text);
                if (cert != null)
                {
                    AddLog("Loaded Certificate " + cert.Subject);
                }
                else
                {
                    AddLog("Cannot load certificate " + txtThumbprint.Text);

                }

            }
            catch (Exception ex)
            {
                AddLog(String.Format("Cannot load certificate {0}. Returned Error {1}.", txtThumbprint.Text, ex.Message));
                MessageBox.Show(String.Format("Cannot load certificate {0}. Returned Error {1}.", txtThumbprint.Text, ex.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnLoadCert_Click(object sender, EventArgs e)
        {
            LoadCertificate();
        }

        #region Display Results

        private void AddBeginTimestamp()
        {
            AddLog(String.Format("Begin Timestamp {0}", DateTime.Now.ToString("s")));
        }

        private void AddEndTimestamp()
        {
            AddLog(String.Format("End Timestamp {0}", DateTime.Now.ToString("s")));
        }
        private void AddLog(string message)
        {
            txtLog.AppendText(String.Format(">{0}{1}", message, Environment.NewLine));

        }

        internal void AddLog(AffinityGroup affinityGroup)
        {
            if (affinityGroup != null)
            {
                AddLog(String.Format("AffinityGroup Name:{0}", affinityGroup.Name));
                AddLog(String.Format("AffinityGroup Description:{0}", affinityGroup.Description));
                AddLog(String.Format("AffinityGroup Location:{0}", affinityGroup.Location));
                AddLog(affinityGroup.HostedServices);
                AddLog(affinityGroup.StorageServices);
            }
        }

        internal void AddLog(AffinityGroupList affinityGroupList)
        {
            if (affinityGroupList != null)
            {
                AddLog(String.Format("AffinityGroupList contains {0} item(s).", affinityGroupList.Count));
                foreach (AffinityGroup group in affinityGroupList)
                {
                    ddlAffinityGroups.Items.Add(group.Name);
                    AddLog(group);
                }
            }
        }

        internal void AddLog(LocationList locationList)
        {
            if (locationList != null)
            {
                AddLog(String.Format("Locationlist contains {0} item(s).", locationList.Count));
                ddlLocations.DataSource = locationList;
                ddlLocations.DisplayMember = "Name";
                
                foreach (Location group in locationList)
                {
                   //ddlLocations.Items.Add(group);
                   // ddlLocations.
                    AddLog(group);
                }
            }
        }

        internal void AddLog(Location location)
        {
            if (location != null)
            {
                AddLog(String.Format("{0}", location.Name));

            }
        }
        internal void AddLog(Operation operation)
        {
            if (operation != null)
            {
                AddLog(String.Format("Operation Status for {0} is {1}.", operation.OperationTrackingId, operation.Status));

            }
        }
        internal void AddLog(Deployment deployment)
        {
            if (deployment != null)
            {
                AddLog(String.Format("Name:{0}", deployment.Name));
                AddLog(String.Format("Label:{0}", ServiceManagementHelper.DecodeFromBase64String(deployment.Label)));
                lblDeploymentId.Text = deployment.PrivateID;
                AddLog(String.Format("Url:{0}", deployment.Url));
                AddLog(String.Format("Status:{0}", deployment.Status));
                AddLog(String.Format("DeploymentSlot:{0}", deployment.DeploymentSlot));
                AddLog(String.Format("PrivateID:{0}", deployment.PrivateID));
                AddLog(deployment.RoleInstanceList);
                AddLog(deployment.UpgradeStatus);
                AddLog("<----ServiceConfiguration---->");
                currentConfig = ServiceManagementHelper.DecodeFromBase64String(deployment.Configuration);
                AddLog(currentConfig);
                AddLog("</----ServiceConfiguration---->");
                foreach (Role r in deployment.RoleList)
                {
                    dataGridView1.Rows.Add(r.RoleName, ServiceManagementUtil.GetInstanceCountFromConfigByRole(currentConfig, r.RoleName));

                }
            }
        }

        internal void AddLog(DeploymentList deploymentList)
        {
            if (deploymentList != null)
            {
                AddLog(String.Format("DeploymentList contains {0} item(s).", deploymentList.Count));
                foreach (Deployment deployment in deploymentList)
                {
                    AddLog(deployment);
                }
            }
        }

        internal void AddLog(HostedService hostedService)
        {
            if (hostedService != null)
            {
                if (!string.IsNullOrEmpty(hostedService.ServiceName))
                {
                    AddLog(String.Format("StorageService Name:{0}", hostedService.ServiceName));
                }
                AddLog(String.Format("HostedService Url:{0}", hostedService.Url));
                AddLog(hostedService.HostedServiceProperties);
                AddLog(hostedService.Deployments);
            }
        }

        internal void AddLog(HostedServiceList hostedServiceList)
        {
            if (hostedServiceList != null)
            {
                AddLog(String.Format("HostedServiceList contains {0} item(s).", hostedServiceList.Count));
                foreach (HostedService service in hostedServiceList)
                {
                    AddLog(service);
                }
            }
        }

        internal void AddLog(HostedServiceProperties hostedServiceProperties)
        {
            if (hostedServiceProperties != null)
            {
                AddLog(String.Format("HostedService Label:{0}", ServiceManagementHelper.DecodeFromBase64String(hostedServiceProperties.Label)));
                AddLog(String.Format("HostedService Description:{0}", hostedServiceProperties.Description));
                if (!string.IsNullOrEmpty(hostedServiceProperties.AffinityGroup))
                {
                    AddLog(String.Format("HostedService AffinityGroupName:{0}", hostedServiceProperties.AffinityGroup));
                }
                if (!string.IsNullOrEmpty(hostedServiceProperties.Location))
                {
                    AddLog(String.Format("HostedService Location:{0}", hostedServiceProperties.Location));
                }
            }
        }

        internal void AddLog(RoleInstanceList roleInstanceList)
        {
            if (roleInstanceList != null)
            {
                foreach (RoleInstance instance in roleInstanceList)
                {
                    AddLog(String.Format("RoleName: {0}", instance.RoleName));
                    AddLog(String.Format("Role InstanceName: {0}", instance.InstanceName));
                    AddLog(String.Format("Role InstanceStatus: {0}", instance.InstanceStatus));
                }
            }
        }

        internal void AddLog(StorageService storageService)
        {
            if (storageService != null)
            {
                if (!string.IsNullOrEmpty(storageService.ServiceName))
                {
                    AddLog(String.Format("StorageService Name:{0}", storageService.ServiceName));
                    ddlStorage.Items.Add(storageService.ServiceName);
                }
                AddLog(String.Format("StorageService Url:{0}", storageService.Url));
                if (storageService.StorageServiceKeys != null)
                {
                    AddLog(String.Format("Primary key:{0}", storageService.StorageServiceKeys.Primary));
                    AddLog(String.Format("Secondary key:{0}", storageService.StorageServiceKeys.Secondary));
                }
               
            
            }
        }

        internal void AddLog(StorageServiceList storageServiceList)
        {
            
            if (storageServiceList != null)
            {
                AddLog(String.Format("StorageServiceList contains {0} item(s).", storageServiceList.Count));
              
                foreach (StorageService service in storageServiceList)
                {
                    AddLog(service);
                                   
                }

             

            }
        }

        internal void AddLog(UpgradeStatus upgradeStatus)
        {
            if (upgradeStatus != null)
            {
                AddLog(String.Format("UpgradeType: {0}", upgradeStatus.UpgradeType));
                AddLog(String.Format("CurrentUpgradeDomain: {0}", upgradeStatus.CurrentUpgradeDomain));
                AddLog(String.Format("CurrentUpgradeDomainState: {0}", upgradeStatus.CurrentUpgradeDomainState));
            }
        }

        internal void AddLog(Certificate certificate)
        {
            if (certificate == null)
                return;

            if (certificate.CertificateUrl != null)
            {
                AddLog(String.Format("Certificate Url:{0}", certificate.CertificateUrl.ToString()));
            }

            if (certificate.ThumbprintAlgorithm != null)
            {
                AddLog(String.Format("Certificate ThumbprintAlgorithm:{0}", certificate.ThumbprintAlgorithm));
            }

            if (certificate.Thumbprint != null)
            {
                AddLog(String.Format("Certificate Thumbprint:{0}", certificate.Thumbprint));
            }

            if (certificate.Data != null)
            {
                X509Certificate2 cert = new X509Certificate2(Convert.FromBase64String(certificate.Data));
                if (cert != null)
                {
                    AddLog(String.Format("Certificate FriendlyName:{0}", cert.FriendlyName));
                    AddLog(String.Format("Certificate Subject:{0}", cert.Subject));
                    AddLog(String.Format("Certificate Issuer:{0}", cert.Issuer));
                    AddLog(String.Format("Certificate SerialNumber:{0}", cert.SerialNumber));
                }
                AddLog(String.Format("Certificate Data:{0}", certificate.Data));
            }
        }

        internal void AddLog(CertificateList certificateList)
        {
            if (certificateList == null)
                return;

            AddLog(String.Format("CertificateList contains {0} item(s).", certificateList.Count));
            foreach (var item in certificateList)
            {
                AddLog(item);
            }
        }

        internal void AddLog(OperatingSystemList operatingSystemList)
        {
            if (operatingSystemList == null)
            {
                return;
            }

            AddLog(String.Format("OperatingSystemList contains {0} item(s).", operatingSystemList.Count));
            foreach (var item in operatingSystemList)
            {
                AddLog(item);
            }
        }

        internal void AddLog(Microsoft.Samples.WindowsAzure.ServiceManagement.OperatingSystem operatingSystem)
        {
            if (operatingSystem == null)
            {
                return;
            }

            AddLog(String.Format("Operating System Version:{0}", operatingSystem.Version));
            AddLog(String.Format("Operating System Label:{0}", ServiceManagementHelper.DecodeFromBase64String(operatingSystem.Label)));
            AddLog(String.Format("Operating System IsDefault:{0}", operatingSystem.IsDefault));
            AddLog(String.Format("Operating System IsActive:{0}", operatingSystem.IsActive));
        }

        #endregion




        private void btnExecute_Click(object sender, EventArgs e)
        {
            try
            {
                AddBeginTimestamp();
                switch (lstOperations.Text)
                {

                    case "Get Hosted Service":
                        GetHostedService();
                        break;
                    case "Get Storage Service":
                        GetStorageService();
                        break;
                    case "Get Affinity Group":
                        GetAffinityGroup();
                        break;
                    case "Get Operation Status":
                        GetOperationStatus();
                        break;

                    case "List Affinity Groups":
                        ListAffinityGroups();
                        break;
                    case "Create Storage Service":
                        CreateStorageService();
                        break;
                    case "Delete Storage Service":
                        DeleteStorageService();
                        break;
                    case "Update Storage Service":
                        UpdateStorageService();
                        break;
                    case "List Storage Services":
                        ListStorageServices();
                        break;
                    case "Get Storage Keys":
                        GetStorageKeys();
                        break;
                    case "Regenerate Keys":
                        RegenrateStorageServiceKeys();
                        break;
                    case "List Hosted Services":
                        ListHostedServices();
                        break;
                    case "Get Deployment":
                        GetDeployment();
                        break;
                    case "Create Deployment":
                        CreateDeployment();
                        break;
                    case "Swap Deployment":
                        SwapDeployment();
                        break;
                    case "Delete Deployment":
                        DeleteDeployment();
                        break;
                    case "Update Deployment Status":
                        UpdateDeploymentStatus();
                        break;
                    case "Change Deployment Config":
                        ChangeDeploymentConfiguration();
                        break;
                    case "List Certificates":
                        ListCertificates();
                        break;
                    case "Add Certificate":
                        AddCertificate();
                        break;
                   
                    case "Delete Certificate":
                        DeleteCertificate();
                        break;
                    case "List Operating Systems":
                        ListOperatingSystems();
                        break;
                    case "Walk Upgrade Domain":
                        WalkUpgradeDomain();
                        break;
                    case "Create Hosted Service":
                        CreateHostedService();
                        break;
                    case "Update Hosted Service":
                        UpdateHostedService();
                        break;
                    case "Delete Hosted Service":
                        DeleteHostedService();
                        break;
                    case "List Locations":
                         ListLocations();
                        break;
                    default:
                        break;


                }

                AddEndTimestamp();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);


            }

        }

        void ProcessCheckServerCertificate()
        {
            var CheckServerCertificateString = ServiceManagementUtil.TryGetConfigurationSetting("CheckServerCertificate");

            bool checkServerCertificate = true;

            if (!string.IsNullOrEmpty(CheckServerCertificateString))
            {
                if (!Boolean.TryParse(CheckServerCertificateString, out checkServerCertificate))
                {
                    AddLog("The value of CheckServerCertificate cannot be recognized. Using true as the default value.");
                }
            }

            System.Net.ServicePointManager.ServerCertificateValidationCallback =
               ((sender, certificate, chain, sslPolicyErrors) =>
               {
                   if (!checkServerCertificate)
                       return true;
                   if (sslPolicyErrors == SslPolicyErrors.None)
                       return true;

                   AddLog(String.Format("  Certificate error: {0}", sslPolicyErrors));

                   // Do not allow this client to communicate with unauthenticated servers.
                   return false;
               }
               );

        }


        #region Azure Function Calls
        private void ClearAllColoredTextboxes()
        {
            txtResourceName.BackColor = Color.White;
            txtServiceCertificate.BackColor = Color.White;
            txtCertPassword.BackColor = Color.White;
            txtServiceCertAlgo.BackColor = Color.White;
            txtServiceCertThumbprint.BackColor = Color.White;
            txtResourceName.BackColor = Color.White;
            ddlResourceType.BackColor = Color.White;
            ddlKeyType.BackColor = Color.White;
            ddlSlotType.BackColor = Color.White;
            txtDeploymentName.BackColor = Color.White;
            txtStagingDeploymentName.BackColor = Color.White;
            txtDeploymentLabel.BackColor = Color.White;
            txtPackageUri.BackColor = Color.White;
            txtConfigFilePath.BackColor = Color.White;
            ddlDeploymentStatus.BackColor = Color.White;
            ddlMode.BackColor = Color.White;
            txtDomainNumber.BackColor = Color.White;
            txtOpId.BackColor = Color.White;
            txtHostedServiceName.BackColor = Color.White;
            txtHostedServiceLabel.BackColor = Color.White;
            ddlLocations.BackColor = Color.White;
            ddlAffinityGroups.BackColor = Color.White;

        }

        private void ChangeControlsColor(params Control[] c)
        {

            foreach (Control i in c)
            {
                i.BackColor = Color.Yellow;
            }

        }
        public void ListHostedServices()
        {
            if (txtSubscriptionId.Text.Length > 0 && txtThumbprint.Text.Length > 0)
            {
                string opid;
                HttpStatusCode? nullable = null;
                string statusDescription = null;
                AddLog(ServiceManagementUtil.ListHostedServices(ref channel, txtSubscriptionId.Text, txtThumbprint.Text, out opid, out nullable, out statusDescription));
                txtOpId.Text = opid;
            }
            else
            {
                throw new Exception("Needs SubscriptionId and CertificatePath");
            }
        }

        
        public void ListStorageServices()
        {
            if (txtSubscriptionId.Text.Length > 0 && txtThumbprint.Text.Length > 0)
            {
                ddlStorage.Items.Clear();
                string opid;
                HttpStatusCode? nullable = null;
                string statusDescription = null;
                AddLog(ServiceManagementUtil.ListStorageServices(ref channel, txtSubscriptionId.Text, txtThumbprint.Text, out opid, out nullable, out statusDescription));
                txtOpId.Text = opid;
            }
            else
            {
                throw new Exception("Needs SubscriptionId and CertificatePath");
            }
        }

        public void ListAffinityGroups()
        {
            if (txtSubscriptionId.Text.Length > 0 && txtThumbprint.Text.Length > 0)
            {
                string opid;
                HttpStatusCode? nullable = null;
                string statusDescription = null;
                AddLog(ServiceManagementUtil.ListAffinityGroups(ref channel, txtSubscriptionId.Text, txtThumbprint.Text, out opid, out nullable, out statusDescription));
                txtOpId.Text = opid;
            }
            else
            {
                throw new Exception("Needs SubscriptionId and CertificatePath");
            }
        }

        public void ListLocations()
        {
            if (txtSubscriptionId.Text.Length > 0 && txtThumbprint.Text.Length > 0)
            {
                string opid;
                HttpStatusCode? nullable = null;
                string statusDescription = null;
                AddLog(ServiceManagementUtil.ListLocations(ref channel, txtSubscriptionId.Text, txtThumbprint.Text, out opid, out nullable, out statusDescription));
                txtOpId.Text = "";
            }
            else
            {
                throw new Exception("Needs SubscriptionId and CertificatePath");
            }
        }

        public void GetHostedService()
        {
            if (txtSubscriptionId.Text.Length > 0 && txtThumbprint.Text.Length > 0 && txtResourceName.Text.Length > 0)
            {
                string opid;
                HttpStatusCode? nullable = null;
                string statusDescription = null;
                AddLog(ServiceManagementUtil.GetHostedServiceWithDetails(ref channel, txtSubscriptionId.Text, txtResourceName.Text, true, txtThumbprint.Text, out opid, out nullable, out statusDescription));
                txtOpId.Text = opid;
            }
            else
            {
                throw new Exception("Needs SubscriptionId, CertificatePath and Resource Name");
            }
        }

        private void ShowHostedServiceControls()
        {
            ChangeControlsColor(txtResourceName);
            
        }

        public void GetStorageService()
        {
            if (txtSubscriptionId.Text.Length > 0 && txtThumbprint.Text.Length > 0 && txtResourceName.Text.Length > 0)
            {
                string opid;
                HttpStatusCode? nullable = null;
                string statusDescription = null;
                AddLog(ServiceManagementUtil.GetStorageService(ref channel, txtSubscriptionId.Text, txtResourceName.Text, txtThumbprint.Text, out opid, out nullable, out statusDescription));
                txtOpId.Text = opid;
            }
            else
            {
                throw new Exception("Needs SubscriptionId, CertificatePath and Resource Name");
            }
        }

        private void ShowStorageServiceControls()
        {
            ChangeControlsColor(txtResourceName);
        }
        public void GetStorageKeys()
        {
            if (txtSubscriptionId.Text.Length > 0 && txtThumbprint.Text.Length > 0 && txtResourceName.Text.Length > 0)
            {

                string opid;
                HttpStatusCode? nullable = null;
                string statusDescription = null;
                AddLog(ServiceManagementUtil.GetStorageKeys(ref channel, txtSubscriptionId.Text, txtResourceName.Text, txtThumbprint.Text, out opid, out nullable, out statusDescription));
                txtOpId.Text = opid;
            }
            else
            {
                throw new Exception("Needs SubscriptionId, CertificatePath and Resource Name");
            }
        }

        private void ShowStorageKeysControls()
        {
            ChangeControlsColor(txtResourceName);
        }

        public void RegenrateStorageServiceKeys()
        {
            if (txtSubscriptionId.Text.Length > 0 && txtThumbprint.Text.Length > 0 && txtResourceName.Text.Length > 0 && ddlKeyType.SelectedIndex > 0)
            {
                string opid;
                HttpStatusCode? nullable = null;
                string statusDescription = null;
                AddLog(ServiceManagementUtil.RegenerateStorageServiceKeys(ref channel, txtSubscriptionId.Text, txtResourceName.Text, (ddlKeyType.Text =="primary")?true:false, txtThumbprint.Text, out opid, out nullable, out statusDescription));
                txtOpId.Text = opid;
            }
            else
            {
                throw new Exception("Needs SubscriptionId, CertificatePath, Resource Name and Key Type");
            }
        }

        private void ShowRegenrateStorageServiceKeysControls()
        {
           ChangeControlsColor(txtResourceName, ddlKeyType);
        }
        public void GetAffinityGroup()
        {
            if (txtSubscriptionId.Text.Length > 0 && txtThumbprint.Text.Length > 0 && txtResourceName.Text.Length > 0)
            {
                string opid;
                HttpStatusCode? nullable = null;
                string statusDescription = null;
                AddLog(ServiceManagementUtil.GetAffinityGroup(ref channel, txtSubscriptionId.Text, txtResourceName.Text, txtThumbprint.Text, out opid, out nullable, out statusDescription));
                txtOpId.Text = opid;
            }
            else
            {
                throw new Exception("Needs SubscriptionId, CertificatePath, Resource Name ");
            }
        }

        private void ShowGetAffinityGroupControls()
        {
            ChangeControlsColor(txtResourceName);
        }
        public void GetOperationStatus()
        {
            if (txtSubscriptionId.Text.Length > 0 && txtThumbprint.Text.Length > 0 && txtOpId.Text.Length > 0)
            {
                string opid;
                HttpStatusCode? nullable = null;
                string statusDescription = null;
                AddLog(ServiceManagementUtil.GetOperationStatus(ref channel, txtSubscriptionId.Text, txtOpId.Text, txtThumbprint.Text, out opid, out nullable, out statusDescription));
                txtOpId.Text = opid;
            }
            else
            {
                throw new Exception("Needs SubscriptionId, CertificatePath, OPID ");
            }
        }

        private void ShowGetOperationStatusControls()
        {
            ChangeControlsColor(txtOpId);

        }

        public void GetDeployment()
        {
            dataGridView1.Rows.Clear();
            if (txtSubscriptionId.Text.Length > 0 && txtThumbprint.Text.Length > 0 && txtResourceName.Text.Length > 0)
            {
                if (txtDeploymentName.Text.Length > 0)
                {
                    string opid;
                    HttpStatusCode? nullable = null;
                    string statusDescription = null;
                    AddLog(ServiceManagementUtil.GetDeployment(ref channel, txtSubscriptionId.Text, txtResourceName.Text, txtDeploymentName.Text, txtThumbprint.Text, out opid, out nullable, out statusDescription));
                    txtOpId.Text = opid;
                }
                else
                {
                    string opid;
                    HttpStatusCode? nullable = null;
                    string statusDescription = null;
                    if (ddlSlotType.SelectedIndex > -1)
                    {
                        AddLog(ServiceManagementUtil.GetDeploymentBySlot(ref channel, txtSubscriptionId.Text, txtResourceName.Text, ddlSlotType.Text, txtThumbprint.Text, out opid, out nullable, out statusDescription));
                    }
                    else
                    {
                        AddLog(ServiceManagementUtil.GetDeploymentBySlot(ref channel, txtSubscriptionId.Text, txtResourceName.Text, "production", txtThumbprint.Text, out opid, out nullable, out statusDescription));


                    }
                    txtOpId.Text = opid;

                }
            }
            else
            {
                throw new Exception("Needs SubscriptionId, CertificatePath, Resource Name, Deployment Name or Slot Type ");
            }
        }

        private void ShowGetDeploymentControls()
        {
          
            ChangeControlsColor(txtResourceName, txtDeploymentName, ddlSlotType);
        }

        public void CreateDeployment()
        {
            if (txtSubscriptionId.Text.Length > 0 && txtThumbprint.Text.Length > 0 && txtResourceName.Text.Length > 0 && txtDeploymentName.Text.Length > 0 &&
                txtDeploymentLabel.Text.Length > 0 && txtConfigFilePath.Text.Length > 0 && ddlSlotType.SelectedIndex > -1 && txtPackageUri.Text.Length > 0)
            {
                string opid;
                HttpStatusCode? nullable = null;
                string statusDescription = null;
                ServiceManagementUtil.CreateOrUpdateDeployment(ref channel, txtSubscriptionId.Text, txtResourceName.Text, txtDeploymentName.Text, txtDeploymentLabel.Text,
                    txtConfigFilePath.Text, ddlSlotType.Text, txtPackageUri.Text, txtThumbprint.Text, true, out opid, out nullable, out statusDescription);
                txtOpId.Text = opid;
            }
            else
            {
                throw new Exception("Needs SubscriptionId, CertificatePath, Resource Name, Deployment Name, Deployment Label, Config File Path, Deployment Slot and Package Uri ");
            }
        }


      
        private void ShowCreateDeploymentControls()
        {
           
            ChangeControlsColor(txtResourceName, txtDeploymentName, txtDeploymentLabel, txtConfigFilePath, ddlSlotType, txtPackageUri);
                
        }

        public void CreateHostedService()
        {
            if (txtSubscriptionId.Text.Length > 0 && txtThumbprint.Text.Length > 0 && txtHostedServiceName.Text.Length > 0 && txtHostedServiceLabel.Text.Length > 0 &&
                ( ddlLocations.SelectedIndex > -1 || ddlAffinityGroups.SelectedIndex > -1))
            {
                string opid;
                HttpStatusCode? nullable = null;
                string statusDescription = null;
                ServiceManagementUtil.CreateHostedService(ref channel, txtSubscriptionId.Text, txtHostedServiceName.Text, txtHostedServiceLabel.Text, "Test Description", ddlLocations.Text, ddlAffinityGroups.Text,
                    txtThumbprint.Text, out opid, out nullable, out statusDescription);
                txtOpId.Text = opid;
                txtResourceName.Text = txtHostedServiceName.Text;
                AddLog("Service Created successfully. Now call Get Hosted Service to see the results.");
            }
            else
            {
                throw new Exception("Needs SubscriptionId, CertificatePath, Hosted Service Name, Hosted Service Label, Location");
            }
        }

        public void CreateStorageService()
        {
            if (txtSubscriptionId.Text.Length > 0 && txtThumbprint.Text.Length > 0 && txtHostedServiceName.Text.Length > 0 && txtHostedServiceLabel.Text.Length > 0 &&
                (ddlLocations.SelectedIndex > -1 || ddlAffinityGroups.SelectedIndex > -1))
            {
                string opid;
                HttpStatusCode? nullable = null;
                string statusDescription = null;
                ServiceManagementUtil.CreateStorageService(ref channel, txtSubscriptionId.Text, txtHostedServiceName.Text, txtHostedServiceLabel.Text, "Test Description", ddlLocations.Text, ddlAffinityGroups.Text,
                    txtThumbprint.Text, out opid, out nullable, out statusDescription);
                txtOpId.Text = opid;
                txtResourceName.Text = txtHostedServiceName.Text;
                AddLog("Storage Service Created successfully. Now call Get Hosted Service to see the results.");
            }
            else
            {
                throw new Exception("Needs SubscriptionId, CertificatePath, Service Name, Service Label, Location");
            }
        }

        private void ShowCreateHostedServiceControls()
        {

            ChangeControlsColor(txtHostedServiceName, txtHostedServiceLabel, ddlAffinityGroups, ddlLocations);

        }

        public void UpdateHostedService()
        {
            if (txtSubscriptionId.Text.Length > 0 && txtThumbprint.Text.Length > 0 && txtHostedServiceName.Text.Length > 0 && txtHostedServiceLabel.Text.Length > 0 )
            {
                string opid;
                HttpStatusCode? nullable = null;
                string statusDescription = null;
                ServiceManagementUtil.UpdateHostedService(ref channel, txtSubscriptionId.Text, txtHostedServiceName.Text, txtHostedServiceLabel.Text, "Test Update Description",
                    txtThumbprint.Text, out opid, out nullable, out statusDescription);
                txtOpId.Text = opid;

                txtResourceName.Text = txtHostedServiceName.Text;
                AddLog("Service Updated successfully. Now call Get Hosted Service to see the results.");
            }
            else
            {
                throw new Exception("Needs SubscriptionId, CertificatePath, Hosted Service Name, Hosted Service Label");
            }
        }


        public void UpdateStorageService()
        {
            if (txtSubscriptionId.Text.Length > 0 && txtThumbprint.Text.Length > 0 && txtHostedServiceName.Text.Length > 0 && txtHostedServiceLabel.Text.Length > 0)
            {
                string opid;
                HttpStatusCode? nullable = null;
                string statusDescription = null;
                ServiceManagementUtil.UpdateStorageService(ref channel, txtSubscriptionId.Text, txtHostedServiceName.Text, txtHostedServiceLabel.Text, "Test Update Description",
                    txtThumbprint.Text, out opid, out nullable, out statusDescription);
                txtOpId.Text = opid;

                txtResourceName.Text = txtHostedServiceName.Text;
                AddLog("Service Updated successfully. Now call Get Storage Service to see the results.");
            }
            else
            {
                throw new Exception("Needs SubscriptionId, CertificatePath, Service Name, Service Label");
            }
        }
        private void ShowUpdateHostedServiceControls()
        {

            ChangeControlsColor(txtHostedServiceName, txtHostedServiceLabel);

        }

        public void DeleteHostedService()
        {
            if (txtSubscriptionId.Text.Length > 0 && txtThumbprint.Text.Length > 0 && txtHostedServiceName.Text.Length > 0 && txtHostedServiceLabel.Text.Length > 0)
            {
                string opid;
                HttpStatusCode? nullable = null;
                string statusDescription = null;
                ServiceManagementUtil.DeleteHostedService(ref channel, txtSubscriptionId.Text, txtHostedServiceName.Text,
                    txtThumbprint.Text, out opid, out nullable, out statusDescription);
                txtOpId.Text = opid;

              
                AddLog("Service Deleted successfully. Now call Get Hosted Service to see the results.");
            }
            else
            {
                throw new Exception("Needs SubscriptionId, CertificatePath, Hosted Service Name");
            }
        }

        public void DeleteStorageService()
        {
            if (txtSubscriptionId.Text.Length > 0 && txtThumbprint.Text.Length > 0 && txtHostedServiceName.Text.Length > 0 && txtHostedServiceLabel.Text.Length > 0)
            {
                string opid;
                HttpStatusCode? nullable = null;
                string statusDescription = null;
                ServiceManagementUtil.DeleteStorageService(ref channel, txtSubscriptionId.Text, txtHostedServiceName.Text,
                    txtThumbprint.Text, out opid, out nullable, out statusDescription);
                txtOpId.Text = opid;


                AddLog("Service Deleted successfully. Now call Get Storage Service to see the results.");
            }
            else
            {
                throw new Exception("Needs SubscriptionId, CertificatePath, Service Name");
            }
        }
        private void ShowDeleteHostedServiceControls()
        {

            ChangeControlsColor(txtHostedServiceName);

        }
        public void SwapDeployment()
        {
            if (txtSubscriptionId.Text.Length > 0 && txtThumbprint.Text.Length > 0 && txtResourceName.Text.Length > 0 && txtDeploymentName.Text.Length > 0 && txtStagingDeploymentName.Text.Length > 0)
            {
                string opid;
                HttpStatusCode? nullable = null;
                string statusDescription = null;
                ServiceManagementUtil.SwapDeployment(ref channel, txtSubscriptionId.Text, txtResourceName.Text, txtDeploymentName.Text, txtStagingDeploymentName.Text, txtThumbprint.Text, out opid, out nullable, out statusDescription);
                txtOpId.Text = opid;
            }
            else
            {
                throw new Exception("Needs SubscriptionId, CertificatePath, Resource Name, Deployment Name, Staging Deployment Name ");
            }
        }

        private void ShowSwapDeploymentControls()
        {
           
            ChangeControlsColor(txtResourceName, txtDeploymentName, txtStagingDeploymentName);

        }
        public void DeleteDeployment()
        {
            if (txtSubscriptionId.Text.Length > 0 && txtThumbprint.Text.Length > 0 && txtResourceName.Text.Length > 0)
            {
                if (txtDeploymentName.Text.Length > 0)
                {
                    string opid;
                    HttpStatusCode? nullable = null;
                    string statusDescription = null;
                    ServiceManagementUtil.DeleteDeployment(ref channel, txtSubscriptionId.Text, txtResourceName.Text, txtDeploymentName.Text, txtThumbprint.Text, out opid, out nullable, out statusDescription);
                    txtOpId.Text = opid;
                }
                else
                {
                    string opid;
                    HttpStatusCode? nullable = null;
                    string statusDescription = null;
                    if (ddlSlotType.SelectedIndex > -1)
                    {
                        ServiceManagementUtil.DeleteDeploymentBySlot(ref channel, txtSubscriptionId.Text, txtResourceName.Text, ddlSlotType.Text, txtThumbprint.Text, out opid, out nullable, out statusDescription);
                    }
                    else
                    {
                        ServiceManagementUtil.DeleteDeploymentBySlot(ref channel, txtSubscriptionId.Text, txtResourceName.Text, "staging", txtThumbprint.Text, out opid, out nullable, out statusDescription);


                    }
                    txtOpId.Text = opid;

                }
            }
            else
            {
                throw new Exception("Needs SubscriptionId, CertificatePath, Resource Name, Deployment Name or Slot Type ");
            }
        }

        private void ShowDeleteDeploymentControls()
        {
           
            ChangeControlsColor(txtResourceName, txtDeploymentName, ddlSlotType);

        }
        public void UpdateDeploymentStatus()
        {
            if (txtSubscriptionId.Text.Length > 0 && txtThumbprint.Text.Length > 0 && txtResourceName.Text.Length > 0 && ddlDeploymentStatus.SelectedIndex > -1)
            {
                string opid;
                HttpStatusCode? nullable = null;
                string statusDescription = null;
                if (txtDeploymentName.Text.Length > -1)
                {
                    ServiceManagementUtil.UpdateDeploymentStatus(ref channel, txtSubscriptionId.Text, txtResourceName.Text, txtDeploymentName.Text, ddlDeploymentStatus.Text, txtThumbprint.Text, out opid, out nullable, out statusDescription);
                }
                else
                {
                    if (ddlSlotType.SelectedIndex > -1)
                    {
                        ServiceManagementUtil.UpdateDeploymentStatusBySlot(ref channel, txtSubscriptionId.Text, txtResourceName.Text, ddlSlotType.Text, ddlDeploymentStatus.Text, txtThumbprint.Text, out opid, out nullable, out statusDescription);
                    }
                    else
                    {
                        ServiceManagementUtil.UpdateDeploymentStatusBySlot(ref channel, txtSubscriptionId.Text, txtResourceName.Text, "staging", ddlDeploymentStatus.Text, txtThumbprint.Text, out opid, out nullable, out statusDescription);


                    }

                }
                txtOpId.Text = opid;
            }
            else
            {
                throw new Exception("Needs SubscriptionId, CertificatePath, Resource Name, Deployment Status, Deployment Name or Slot Type ");
            }
        }

        private void ShowUpdateDeploymentStatusControls()
        {
           
            ChangeControlsColor(txtResourceName, txtDeploymentName, ddlDeploymentStatus);


        }
        public void ChangeDeploymentConfiguration()
        {
            if (txtSubscriptionId.Text.Length > 0 && txtThumbprint.Text.Length > 0 && txtResourceName.Text.Length > 0 && txtConfigFilePath.Text.Length > 0)
            {
                string opid;
                HttpStatusCode? nullable = null;
                string statusDescription = null;
                if (txtDeploymentName.Text.Length > 0)
                {
                    ServiceManagementUtil.ChangeConfiguration(ref channel, txtSubscriptionId.Text, txtResourceName.Text, txtDeploymentName.Text, txtConfigFilePath.Text, txtThumbprint.Text, out opid, out nullable, out statusDescription);
                }
                else
                {
                    if (ddlSlotType.SelectedIndex > -1)
                    {
                        ServiceManagementUtil.ChangeConfigurationBySlot(ref channel, txtSubscriptionId.Text, txtResourceName.Text, ddlSlotType.Text, txtConfigFilePath.Text, txtThumbprint.Text, out opid, out nullable, out statusDescription);
                    }
                    else
                    {
                        ServiceManagementUtil.ChangeConfigurationBySlot(ref channel, txtSubscriptionId.Text, txtResourceName.Text, "staging", txtConfigFilePath.Text, txtThumbprint.Text, out opid, out nullable, out statusDescription);


                    }

                }
                txtOpId.Text = opid;
            }
            else
            {
                throw new Exception("Needs SubscriptionId, CertificatePath, Resource Name, Deployment Name or Slot Type ");
            }
        }


        public void ChangeDeploymentConfigurationByConfigString()
        {
            if (txtSubscriptionId.Text.Length > 0 && txtThumbprint.Text.Length > 0 && txtResourceName.Text.Length > 0 && currentConfig.Length > 0)
            {
                string opid;
                HttpStatusCode? nullable = null;
                string statusDescription = null;
                if (txtDeploymentName.Text.Length > 0)
                {
                    ServiceManagementUtil.ChangeConfigurationByConfigString(ref channel, txtSubscriptionId.Text, txtResourceName.Text, txtDeploymentName.Text, currentConfig, txtThumbprint.Text, out opid, out nullable, out statusDescription);
                }
                else
                {
                    if (ddlSlotType.SelectedIndex > -1)
                    {
                        ServiceManagementUtil.ChangeConfigurationByConfigStringBySlot(ref channel, txtSubscriptionId.Text, txtResourceName.Text, ddlSlotType.Text, currentConfig, txtThumbprint.Text, out opid, out nullable, out statusDescription);
                    }
                    else
                    {
                        ServiceManagementUtil.ChangeConfigurationByConfigStringBySlot(ref channel, txtSubscriptionId.Text, txtResourceName.Text, "staging", currentConfig, txtThumbprint.Text, out opid, out nullable, out statusDescription);


                    }

                }
                txtOpId.Text = opid;
            }
            else
            {
                throw new Exception("Needs SubscriptionId, CertificatePath, Resource Name, Deployment Name or Slot Type ");
            }
        }
        private void ShowChangeDeploymentConfigurationControls()
        {

            ChangeControlsColor(txtResourceName, txtDeploymentName, txtConfigFilePath, ddlSlotType);

        }

        public void ListCertificates()
        {
            if (txtSubscriptionId.Text.Length > 0 && txtThumbprint.Text.Length > 0 && txtResourceName.Text.Length > 0)
            {
                string opid;
                HttpStatusCode? nullable = null;
                string statusDescription = null;
                AddLog(ServiceManagementUtil.ListCertificates(ref channel, txtSubscriptionId.Text, txtThumbprint.Text, txtResourceName.Text, out opid, out nullable, out statusDescription));
                txtOpId.Text = opid;
            }
            else
            {
                throw new Exception("Needs SubscriptionId, CertificatePath, Resource Name, Thumbprint of the service mgmt certificate ");
            }
        }

        private void ShowListCertificatesControls()
        {

            ChangeControlsColor(txtResourceName);

        }
        public void AddCertificate()
        {
            if (txtSubscriptionId.Text.Length > 0 && txtThumbprint.Text.Length > 0 && txtResourceName.Text.Length > 0 && txtServiceCertificate.Text.Length > 0)
            {
                string opid;
                HttpStatusCode? nullable = null;
                string statusDescription = null;
                ServiceManagementUtil.AddCertificate(ref channel, txtSubscriptionId.Text, txtResourceName.Text, txtThumbprint.Text, File.ReadAllBytes( txtServiceCertificate.Text), "pfx", txtCertPassword.Text, out opid, out nullable, out statusDescription);
                txtOpId.Text = opid;
            }
            else
            {
                throw new Exception("Needs SubscriptionId, CertificatePath, Resource Name, Path of the service certificate, Thumbprint of the service mgmt ");
            }
        }

        private void ShowAddCertificateControls()
        {

            ChangeControlsColor(txtResourceName, txtServiceCertificate, txtCertPassword);

        }
        public void DeleteCertificate()
        {
            if (txtSubscriptionId.Text.Length > 0 && txtThumbprint.Text.Length > 0 && txtResourceName.Text.Length > 0 && txtServiceCertAlgo.Text.Length > 0 && txtServiceCertThumbprint.Text.Length > 0)
            {
                string opid;
                HttpStatusCode? nullable = null;
                string statusDescription = null;
                ServiceManagementUtil.DeleteCertificate(ref channel, txtSubscriptionId.Text, txtResourceName.Text, txtThumbprint.Text, txtServiceCertAlgo.Text, txtServiceCertThumbprint.Text, out opid, out nullable, out statusDescription);
                txtOpId.Text = opid;
            }
            else
            {
                throw new Exception("Needs SubscriptionId, CertificatePath, Resource Name, Thumbprint of the service certificate, Algorithm of the certificate. ");
            }
        }

        private void ShowDeleteCertificateControls()
        {

            ChangeControlsColor(txtResourceName, txtServiceCertAlgo, txtServiceCertThumbprint);

        }

        public void ListOperatingSystems()
        {
            if (txtSubscriptionId.Text.Length > 0 && txtThumbprint.Text.Length > 0)
            {
                string opid;
                HttpStatusCode? nullable = null;
                string statusDescription = null;
                AddLog(ServiceManagementUtil.ListOperatingSystems(ref channel, txtSubscriptionId.Text, txtThumbprint.Text, out opid, out nullable, out statusDescription));
               
            }
            else
            {
                throw new Exception("Needs SubscriptionId, Thumbprint");
            }
        }

       
        public void WalkUpgradeDomain()
        {
            if (txtSubscriptionId.Text.Length > 0 && txtThumbprint.Text.Length > 0 && txtResourceName.Text.Length > 0 && txtDomainNumber.Text.Length > 0)
            {
                string opid;
                HttpStatusCode? nullable = null;
                string statusDescription = null;
                if (txtDeploymentName.Text.Length > 0)
                {
                    ServiceManagementUtil.WalkUpgradeDomain(ref channel, txtSubscriptionId.Text, txtResourceName.Text, txtDeploymentName.Text, int.Parse(txtDomainNumber.Text), txtThumbprint.Text, out opid, out nullable, out statusDescription);
                }
                else
                {
                    if (ddlSlotType.SelectedIndex > -1)
                    {
                        ServiceManagementUtil.WalkUpgradeDomainBySlot(ref channel, txtSubscriptionId.Text, txtResourceName.Text, ddlSlotType.Text, int.Parse(txtDomainNumber.Text), txtThumbprint.Text, out opid, out nullable, out statusDescription);
                    }
                    else
                    {
                        ServiceManagementUtil.WalkUpgradeDomainBySlot(ref channel, txtSubscriptionId.Text, txtResourceName.Text, "staging", int.Parse(txtDomainNumber.Text), txtThumbprint.Text, out opid, out nullable, out statusDescription);


                    }

                }
                
            }
            else
            {
                throw new Exception("Needs SubscriptionId, CertificatePath, Resource Name, Deployment Name or Slot Type ");
            }
        }
        private void ShowWalkUpgradeDomainControls()
        {

            ChangeControlsColor(txtResourceName, txtDeploymentName, txtDomainNumber, ddlSlotType);

        }
        #endregion

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtLog.Clear();
        }



        private void btnBrowseConfigFile_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "All files (*.*)|*.*";
            openFileDialog1.InitialDirectory = Environment.CurrentDirectory;
            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                txtConfigFilePath.Text = openFileDialog1.FileName;

            }
        }

        private void btnBrowseServiceCertificate_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "All files (*.*)|*.*";
            openFileDialog1.InitialDirectory = Environment.CurrentDirectory;
            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                txtServiceCertificate.Text = openFileDialog1.FileName;

            }
        }

        private void lstOperations_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ClearAllColoredTextboxes();
                switch (lstOperations.Text)
                {

                    case "Get Hosted Service":
                        ShowHostedServiceControls();
                   
                        break;
                    case "Get Storage Service":
                        ShowStorageServiceControls();
                        break;
                    case "Get Affinity Group":
                        ShowGetAffinityGroupControls();
                        break;
                    case "Get Operation Status":
                        ShowGetOperationStatusControls();
                        break;

                    case "List Affinity Groups":
                      
                        break;

                    case "List Storage Services":
                        
                        break;
                    case "Get Storage Keys":
                        ShowStorageKeysControls();
                        break;
                    case "Regenerate Keys":
                        ShowRegenrateStorageServiceKeysControls();
                        break;
                    case "List Hosted Services":
                       
                        break;
                    case "Get Deployment":
                        ShowGetDeploymentControls();
                        break;
                    case "Create Deployment":
                        ShowCreateDeploymentControls();
                        break;
                    case "Swap Deployment":
                        ShowSwapDeploymentControls();
                        break;
                    case "Delete Deployment":
                        ShowDeleteDeploymentControls();
                        break;
                    case "Update Deployment Status":
                        ShowUpdateDeploymentStatusControls();
                        break;
                    case "Change Deployment Config":
                        ShowChangeDeploymentConfigurationControls();
                        break;
                    case "List Certificates":
                        ShowListCertificatesControls();
                        break;
                    case "Add Certificate":
                        ShowAddCertificateControls();
                        break;

                    case "Delete Certificate":
                        ShowDeleteCertificateControls();
                        break;
                    case "List Operating Systems":
                       
                        break;
                    case "Walk Upgrade Domain":
                        ShowWalkUpgradeDomainControls();
                        break;
                      
                    case "Create Hosted Service":
                        ShowCreateHostedServiceControls();
                        break;
                    case "Create Storage Service":
                        ShowCreateHostedServiceControls();
                        break;
                    case "Update Hosted Service":
                        ShowUpdateHostedServiceControls();
                        break;
                    case "Update Storage Service":
                        ShowUpdateHostedServiceControls();
                        break;
                    case "Delete Hosted Service":
                        ShowDeleteHostedServiceControls();
                        break;

                    case "Delete Storage Service":
                        ShowDeleteHostedServiceControls();
                        break;
                    case "List Locations":

                        break;

                    default:
                        break;


                }


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);


            }
        }

        private void btnChangeInstances_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                try
                {
                    bool changed = false;
                    foreach (DataGridViewRow r in dataGridView1.Rows)
                    {
                        object o = r.Cells["NewInstanceCount"].Value;

                        if (o != null)
                        {
                            int newInstanceCount = Convert.ToInt32(o);
                            int instances = Convert.ToInt32(r.Cells["Instances"].Value);
                            string roleName = r.Cells["RoleName"].Value.ToString();
                            if (instances != newInstanceCount)
                            {

                                currentConfig = ServiceManagementUtil.ChangeInstanceCount(currentConfig, roleName, newInstanceCount.ToString());
                                changed = true;
                            }

                        }
                    }

                    if (changed)
                    {
                        AddLog(currentConfig);
                        AddBeginTimestamp();
                        ChangeDeploymentConfigurationByConfigString();
                        AddEndTimestamp();
                        GetDeployment();

                    }
                    else
                    {
                        MessageBox.Show("No instance change detected.");

                    }



                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);


                }
            }
            else
            {

                MessageBox.Show("Please execute the GetDeployment method first.");
            }
        }

        private void btnGetPerfData_Click(object sender, EventArgs e)
        {
            try{
            if (ddlStorage.SelectedIndex > -1 && lblDeploymentId.Text.Length > 0)
            {
                string storageServiceName = ddlStorage.SelectedItem.ToString();
                //Get the storage key
                if (txtSubscriptionId.Text.Length > 0 && txtThumbprint.Text.Length > 0 && txtResourceName.Text.Length > 0)
                {

                    string opid;
                    HttpStatusCode? nullable = null;
                    string statusDescription = null;

                    StorageService s = ServiceManagementUtil.GetStorageKeys(ref channel, txtSubscriptionId.Text, storageServiceName, txtThumbprint.Text, out opid, out nullable, out statusDescription);
                    txtOpId.Text = opid;
                    string key = s.StorageServiceKeys.Primary;
                    int timeFrameInMinutes = 360;
                    txtAvgMemory.Text = ((int)DiagnosticsUtil.GetAverageMemoryUsageInMbytes(lblDeploymentId.Text, storageServiceName, key, timeFrameInMinutes)).ToString();
                    txtAvgCpu.Text = ((int)DiagnosticsUtil.GetAverageProcessorTime(lblDeploymentId.Text, storageServiceName, key, timeFrameInMinutes)).ToString(); 

                }
                else
                {
                    throw new Exception("Needs SubscriptionId, CertificatePath and Resource Name");
                }
            }
            else
            {
                MessageBox.Show("First execute GetDeployment(), then execute the ListStorageServices() operations and then select a storage service that stored the performance data of your compute service.");
            }

             }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);


            }
        }

        private void btnGuidName_Click(object sender, EventArgs e)
        {
            txtHostedServiceName.Text = System.Guid.NewGuid().ToString("N");
        }

      

    }

  
}
