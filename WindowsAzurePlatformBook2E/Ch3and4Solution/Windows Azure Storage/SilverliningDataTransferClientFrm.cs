using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
//using Microsoft.Samples.ServiceHosting.StorageClient;
using System.Threading;
using System.Collections.Specialized;
using System.Reflection;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Mime;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Collections;
using System.Xml.Serialization;
using System.Drawing.Imaging;
using Microsoft.WindowsAzure.StorageClient;
using Microsoft.WindowsAzure;
using System.Threading.Tasks;
using ProAzureCommonLib;

namespace Windows_Azure_Storage
{
    public partial class WindowsAzureStorage : Form
    {
        private WAStorageHelper m_StorageHelper;
        private bool m_IsLocal = false;
        private ResultContinuation containerListContinuationToken;
        private ResultContinuation blobListContinuationToken;

        //private BindingSource queueGridBindingSource = new BindingSource();

        public WAStorageHelper StorageHelper
        {
            get
            {
                if (m_StorageHelper == null)
                    LoadStorageAccount();
                return m_StorageHelper;
            }
            set { m_StorageHelper = value; }
        }

        public bool IsLocal
        {
            get { return m_IsLocal; }
            set { m_IsLocal = value; }
        }
        public WindowsAzureStorage()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {

            string localAccountName = ConfigurationManager.AppSettings["LocalStorageAccountName"];
            if (!string.IsNullOrEmpty(localAccountName))
            {
                string accountName = ConfigurationManager.AppSettings["StorageAccountConnectionString"];
            
                txtAccountName.Text = accountName;
              

                //if (!string.IsNullOrEmpty(accountName))
                //{
                //    if (accountName == localAccountName)
                //        IsLocal = true;
                //}

                try
                {
                    StorageHelper = new WAStorageHelper(CloudStorageAccount.Parse(txtAccountName.Text));

                }
                catch (Exception ex)
                {

                    MessageBox.Show("Could not connect to the storage account. Please check the credentials.");
                }
                //queueGridBindingSource = new BindingSource();
                //dgvMessages.DataSource = queueGridBindingSource;
            }

        }

        private void LoadStorageAccount()
        {
            if (txtAccountName.Text.Length > 0 )
            {
                try
                {
                    statusLabel.Text = "";
                    //string localAccountName = ConfigurationManager.AppSettings["LocalStorageAccountName"];

                    //if (txtAccountName.Text == localAccountName)
                    //    IsLocal = true;


                    //string blobStorageEndPoint = ConfigurationManager.AppSettings["BlobStorageEndpoint"];
                    //string queueStorageEndpoint = ConfigurationManager.AppSettings["QueueStorageEndpoint"];
                    //string tableStorageEndpoint = ConfigurationManager.AppSettings["TableStorageEndpoint"];

                    //StorageHelper = new WindowsAzureStorageHelper(txtAccountName.Text,
                    //    txtKey.Text, IsLocal,
                    //    blobStorageEndPoint,
                    //    queueStorageEndpoint,
                    //    tableStorageEndpoint);


                   // StorageHelper = new WindowsAzureStorageHelperWA();

                
                    StorageHelper = new WAStorageHelper(CloudStorageAccount.Parse(txtAccountName.Text));

                    statusLabel.Text = "Connected successfully...";

                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }//if

        }

        //private void CheckConnection()
        //{
        //    if (StorageHelper == null)
        //        LoadStorageAccount();

        //}

        #region Blob Functions

        private void LoadContainers(object target)
        {

            try
            {


                if (cbGetAll.Checked)
                {
                    string[] blobContainers = StorageHelper.GetContainerNames(StorageHelper.GetContainers());

                    if (blobContainers != null)
                    {
                        if (lbContainers.InvokeRequired)
                        {
                            lbContainers.Invoke(new MethodInvoker(delegate { lbContainers.Items.AddRange(blobContainers); }));
                            statusLabel.Text = "Containers loaded successfully...";

                        }
                        if (cbDestinationContainer.InvokeRequired)
                        {

                            cbDestinationContainer.Invoke(new MethodInvoker(delegate { cbDestinationContainer.Items.AddRange(blobContainers); }));

                        }
                    }
                }
                else
                {
                    string prefix = txtPrefix.Text;
                    string marker = txtMarker.Text;
                    int maxResults = 100;
                    if (!string.IsNullOrEmpty(txtMaxResults.Text))
                    {

                        maxResults = int.Parse(txtMaxResults.Text.Trim());
                    }

                    //IList<BlobContainer> containers = StorageHelper.GetContainers(prefix, maxResults, ref marker);
                    var containers = StorageHelper.GetContainerSegmented(prefix, maxResults, containerListContinuationToken);

                    //if (txtMarker.InvokeRequired)
                    //{
                    //    txtMarker.Invoke(new MethodInvoker(delegate { txtMarker.Text = marker; }));

                    //}

                    //if (string.IsNullOrEmpty(marker))
                    if (containers.ContinuationToken == null)
                    {
                        statusLabel.Text = "All Containers retrieved successfully...";
                    }
                    else
                    {
                        statusLabel.Text = "Click on List Containers to retrieve more containers....";
                    }

                    containerListContinuationToken = containers.ContinuationToken;

                    foreach (CloudBlobContainer b in containers.Results)
                    {
                        if (lbContainers.InvokeRequired)
                        {
                            lbContainers.Invoke(new MethodInvoker(delegate { lbContainers.Items.Add(b.Name); }));
                        }
                        if (cbDestinationContainer.InvokeRequired)
                        {
                            cbDestinationContainer.Invoke(new MethodInvoker(delegate { cbDestinationContainer.Items.Add(b.Name); }));


                        }

                    }

                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnListContainers_Click(object sender, EventArgs e)
        {
            if (cbGetAll.Checked || string.IsNullOrEmpty(txtMarker.Text))
            {
                lbContainers.Items.Clear();
                cbDestinationContainer.Items.Clear();
            }
            //CheckConnection();

            ThreadPool.QueueUserWorkItem(new WaitCallback(LoadContainers));

           //var containers =  StorageHelper.ListContainersInSegmentsAsynchronously();

           //int l = containers.Count();

          
        }

        //private void btnConnect_Click(object sender, EventArgs e)
        //{
        //    LoadStorageAccount();
        //}

        private void cbGetAll_CheckedChanged(object sender, EventArgs e)
        {
            lbContainers.Items.Clear();
            txtMarker.Text = string.Empty;
        }

        private void lbContainers_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearMetadataFromUI();
            treeView1.Nodes.Clear();
            txtContainerName.Text = lbContainers.Text;
            //cbPublic.Checked = (StorageHelper.GetContainerACL(txtContainerName.Text) == ContainerAccessControl.Public);
            SetPublicChecked();


            //BlobContainerProperties containerProps = StorageHelper.GetContainerProperties(txtContainerName.Text);
            CloudBlobContainer container = StorageHelper.GetBlobContainer(txtContainerName.Text);
            AddContainerMetadataToControls(container);

        }

        private void SetPublicChecked()
        {
            cbPublic.Checked = (StorageHelper.GetContainerPermissions(txtContainerName.Text).PublicAccess == BlobContainerPublicAccessType.Container);
        }

        private void btnContainerFunction_Click(object sender, EventArgs e)
        {
            if (txtContainerName.Text.Length > 0)
            {
                try
                {
                    //CheckConnection();

                    ExecuteFunction(lbFunctions.Text);
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message, "Error executing function " + lbFunctions.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);

                }

            }
            else
            {

                MessageBox.Show("Please specify a Container name...");

            }


        }

        private void ExecuteFunction(string functionName)
        {
            switch (functionName)
            {
                case "Create Container":

                    BlobContainerPermissions permissions = new BlobContainerPermissions { PublicAccess = GetPublicAccess() };
                    NameValueCollection metadata = GetBlobMetaDataFromControls();
                    StorageHelper.CreateContainer(txtContainerName.Text, permissions, metadata);
                    //StorageHelper.CreateContainer(txtContainerName.Text,
                    //    ((cbPublic.Checked) ? ContainerAccessControl.Public : ContainerAccessControl.Private),
                    //    GetMetaDataFromControls());

                    lbContainers.Items.Clear();
                    ThreadPool.QueueUserWorkItem(new WaitCallback(LoadContainers));
                    MessageBox.Show("Container created successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    break;
                case "Get Container Properties":
                    ClearMetadataFromUI();
                    AddContainerMetadataToControls();

                    break;
                case "Set Container Metadata":
                    try
                    {
                        StorageHelper.SetContainerMetadata(txtContainerName.Text, GetMetaDataFromControls());

                        ClearMetadataFromUI();
                        //BlobContainerProperties containerProps1 = StorageHelper.GetContainerProperties(txtContainerName.Text);
                        AddContainerMetadataToControls();
                        MessageBox.Show("Metadata values set successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    catch (StorageClientException)
                    {
                        MessageBox.Show("Metadata was not set. Please trace the message.", "Metadata not set", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }


                    break;
                case "Get Container ACL":

                    SetPublicChecked();

                    break;
                case "Set Container ACL":
                    StorageHelper.SetContainerPermissions(txtContainerName.Text, new BlobContainerPermissions { PublicAccess = GetPublicAccess() });
                    SetPublicChecked();
                    //StorageHelper.SetContainerACL(txtContainerName.Text, ((cbPublic.Checked)?ContainerAccessControl.Public:ContainerAccessControl.Private));
                    //cbPublic.Checked = (StorageHelper.GetContainerACL(txtContainerName.Text) == ContainerAccessControl.Public);
                    MessageBox.Show("ACL value set successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);


                    break;
                case "Delete Container":
                    DialogResult dr = MessageBox.Show(String.Format("Are you sure you want to delete Container {0}?", txtContainerName.Text),
                         "Delete Container", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (dr == DialogResult.Yes)
                    {
                        try
                        {
                            StorageHelper.DeleteContainer(txtContainerName.Text);
                            ClearMetadataFromUI();
                            lbContainers.Items.Clear();
                            ThreadPool.QueueUserWorkItem(new WaitCallback(LoadContainers));

                            MessageBox.Show("Container deleted successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (StorageClientException)
                        {
                            // TODO: Log Error - where to? Local, Service bus?
                            MessageBox.Show("Delete operation failed.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        statusLabel.Text = String.Format("Delete Container {0} operation cancelled.", txtContainerName.Text);

                    }
                    break;
                case "List Blobs":

                    ListBlobs();
                    //ListBlobsStr();

                    break;

                case "Get Shared Access Signature":
                    GetSharedAccessSignature();
                    break;
                default:
                    return;



            };
            statusLabel.Text = "Method executed successfully";

        }

        private BlobContainerPublicAccessType GetPublicAccess()
        {
            return (cbPublic.Checked) ? BlobContainerPublicAccessType.Container : BlobContainerPublicAccessType.Off;
        }

        private void ListBlobs()
        {
            string prefix = txtBlobPrefix.Text;
            string marker = txtBlobMarker.Text;
            int maxResults = 100;
            if (!string.IsNullOrEmpty(txtBlobMaxResults.Text))
            {

                maxResults = int.Parse(txtBlobMaxResults.Text.Trim());
            }
            string containerName = txtContainerName.Text;

            treeView1.Nodes.Clear();
            TreeNode account = treeView1.Nodes.Add(txtAccountName.Text);

            if (account != null)
            {
                TreeNode container = account.Nodes.Add(txtContainerName.Text);
                DisplayBlobsInTree(containerName, prefix, maxResults, container);
                treeView1.ExpandAll();

                txtBlobMarker.Text = marker;
            }
            else
            {

                MessageBox.Show("Invalid Account Name", "Invalid Account Name", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void GetSharedAccessSignature()
        {
            int hrs = int.Parse(txtContainerExpirationHrs.Text);
            SharedAccessPermissions sh = SharedAccessPermissions.Read;

            if (cbShPer.SelectedIndex > -1)
            {

                string access = cbShPer.SelectedText;

                switch (access)
                {
                    case "Read":
                        sh = SharedAccessPermissions.Read;
                        break;
                    case "Write":
                        sh = SharedAccessPermissions.Write;
                        break;
                    case "List":
                        sh = SharedAccessPermissions.List;
                        break;
                    case "Delete":
                        sh = SharedAccessPermissions.Delete;
                        break;
                    case "None":
                        sh = SharedAccessPermissions.None;
                        break;

                    default:
                        sh = SharedAccessPermissions.Read;
                        break;
                };

            }

           
            string containerName = txtContainerName.Text;

          txtSharedAccessKey.Text = StorageHelper.GetSharedAccessSignatureForPackageContainer(containerName, System.Guid.NewGuid().ToString("N"), hrs, sh);

          
           
        }

        private void GetSharedAccessBlobUrl()
        {
            int mins = int.Parse(txtBlobExpirationMins.Text);
            SharedAccessPermissions sh = SharedAccessPermissions.Read;

            if (cbBlobShPerm.SelectedIndex > -1)
            {

                string access = cbBlobShPerm.SelectedText;

                switch (access)
                {
                    case "Read":
                        sh = SharedAccessPermissions.Read;
                        break;
                    case "Write":
                        sh = SharedAccessPermissions.Write;
                        break;
                    case "List":
                        sh = SharedAccessPermissions.List;
                        break;
                    case "Delete":
                        sh = SharedAccessPermissions.Delete;
                        break;
                    case "None":
                        sh = SharedAccessPermissions.None;
                        break;

                    default:
                        sh = SharedAccessPermissions.Read;
                        break;
                };

            }


            string blobName = txtBlobName.Text;
            string containerName = txtContainerName.Text;
           this.txtSharedAccessUrl.Text =StorageHelper.GetSharedAccessSignatureBlobUrl(containerName, blobName,sh,  mins);



        }
        //private void ListBlobs(string containerName, string blobPrefix, int maxResults, TreeNode tn)
        //{
        //    ResultSegment<IListBlobItem> blobList = StorageHelper.ListBlobs(containerName, blobPrefix, maxResults, blobListContinuationToken);
        //    if (blobList.ContinuationToken != null)
        //    {
        //        blobListContinuationToken = blobList.ContinuationToken;
        //        DisplayBlobsInTree(blobList.Results, tn);
        //    }
        //}

        private void DisplayBlobsInTree(string containerName, string blobPrefix, int maxResults, TreeNode tn)
        {
            ResultSegment<IListBlobItem> blobList;
            do
            {
                blobList = StorageHelper.ListBlobs(containerName, blobPrefix, maxResults, blobListContinuationToken);
                blobListContinuationToken = blobList.ContinuationToken;
                AddBlobsToTree(blobList.Results, tn);
            }
            while (blobList.ContinuationToken != null);
        }

        private void AddBlobsToTree(IEnumerable<IListBlobItem> blobs, TreeNode tn)
        {
            IEnumerable<string> names = WAStorageHelper.ParseBlobNames(blobs);
            foreach (var name in names)
            {
                TreeNode t2 = tn.Nodes.Add(name);
                t2.ToolTipText = name;
            }
        }

        //private void ListBlobs(string containerName, string blobPrefix, string delimiter, int maxResults, ref string marker, TreeNode tn)
        //{

        //    IList<object> blobList = new List<object>(StorageHelper.ListBlobs(txtContainerName.Text, blobPrefix, delimiter, maxResults, ref marker));


        //    foreach (object o in blobList)
        //    {
        //        if (o is BlobProperties)
        //        {

        //           BlobProperties bp = (BlobProperties)o;

        //            string[] structureArr = bp.Name.Split(char.Parse(delimiter));

        //            if (structureArr != null && structureArr.Length > 0)
        //            {

        //                if (tn != null)
        //                {
        //                    TreeNode t2 = tn.Nodes.Add(structureArr[structureArr.Length - 1]);
        //                    t2.ToolTipText = bp.Name;

        //                }


        //            }


        //        }
        //        else if (o is string)
        //        {
        //            string bPrefix = (string)o;
        //            string[] structureArr = bPrefix.Split(char.Parse(delimiter));

        //            if (structureArr != null && structureArr.Length > 0)
        //            {
        //                string node = string.Empty;
        //                TreeNode t1 = null;
        //                if (structureArr.Length > 1)
        //                {
        //                    node = structureArr[structureArr.Length - 2];
        //                    t1 = tn.Nodes.Add(node);
        //                }
        //                else
        //                {
        //                    node = structureArr[0];
        //                    t1 = tn.Nodes.Add(node);
        //                }


        //                ListBlobs(containerName, bPrefix, delimiter, maxResults, ref marker, t1);

        //            }



        //        }



        //    }
        //}


        //private void ListBlobsStr()
        //{
        //    string prefix = txtBlobPrefix.Text;
        //    string marker = txtBlobMarker.Text;
        //    int maxResults = 100;
        //    if (!string.IsNullOrEmpty(txtBlobMaxResults.Text))
        //    {

        //        maxResults = int.Parse(txtBlobMaxResults.Text.Trim());
        //    }
        //    string containerName = txtContainerName.Text;


        //    IList<string> blobs = new List<string>();
        //    string basePath = string.Empty;
        //        ListBlobsStr(containerName, prefix, txtDelimiter.Text, maxResults, ref marker, blobs, basePath);


        //        txtBlobMarker.Text = marker;


        //}

        //private void ListBlobsStr(string containerName, string blobPrefix, string delimiter, int maxResults, ref string marker, IList<string> tn, string basePath)
        //{

        //    IList<object> blobList = new List<object>(StorageHelper.ListBlobs(txtContainerName.Text, blobPrefix, delimiter, maxResults, ref marker));


        //    foreach (object o in blobList)
        //    {
        //        if (o is BlobProperties)
        //        {

        //            BlobProperties bp = (BlobProperties)o;

        //            string[] structureArr = bp.Name.Split(char.Parse(delimiter));

        //            if (structureArr != null && structureArr.Length > 0)
        //            {

        //                if (tn != null)
        //                {
        //                  tn.Add(  basePath + delimiter + (structureArr[structureArr.Length - 1]));

        //                }


        //            }


        //        }
        //        else if (o is string)
        //        {
        //            string bPrefix = (string)o;
        //            string[] structureArr = bPrefix.Split(char.Parse(delimiter));

        //            if (structureArr != null && structureArr.Length > 0)
        //            {
        //                string node = string.Empty;
        //                string t1 = null;
        //                if (structureArr.Length > 1)
        //                {
        //                    node = structureArr[structureArr.Length - 2];
        //                    t1 = node;
        //                }
        //                else
        //                {
        //                    node = structureArr[0];
        //                    t1 = node;
        //                }


        //                ListBlobsStr(containerName, bPrefix, delimiter, maxResults, ref marker, tn, t1);

        //            }



        //        }



        //    }
        //}
        private NameValueCollection GetMetaDataFromControls()
        {
            NameValueCollection metadata = new NameValueCollection();

            for (int i = 0; i < 3; i++)
            {
                string name = string.Empty;
                string value = string.Empty;
                switch (i)
                {

                    case 0:
                        name = textBox1.Text;
                        value = textBox2.Text;
                        break;
                    case 1:
                        name = textBox3.Text;
                        value = textBox4.Text;
                        break;

                    case 2:
                        name = textBox5.Text;
                        value = textBox6.Text;
                        break;

                    case 3:
                        name = textBox7.Text;
                        value = textBox8.Text;
                        break;

                    default:

                        break;


                };

                if (name.Length > 0 && value.Length > 0)
                {
                    metadata.Add(name, value);
                }


            }//for

            return metadata;

        }

        private void AddContainerMetadataToControls()
        {
            AddContainerMetadataToControls(txtContainerName.Text);
        }

        private void AddContainerMetadataToControls(string containerName)
        {
            CloudBlobContainer container = StorageHelper.GetBlobContainer(containerName);
            AddContainerMetadataToControls(container);
        }

        private void AddContainerMetadataToControls(CloudBlobContainer container)
        {
            container.FetchAttributes();
            BlobContainerProperties props = container.Properties;
            if (props != null)
            {
                lblLastModified.Text = String.Format("Last Modified (UTC):{0},  ETag:{1}", props.LastModifiedUtc.ToString("F"), props.ETag);
                lblURI.Text = container.Uri.ToString();
                NameValueCollection metadata = container.Metadata;

                if (metadata != null && metadata.Count > 0)
                {
                    for (int i = 0; i < metadata.Count; i++)
                    {
                        string name = metadata.GetKey(i);
                        string value = metadata[i];
                        switch (i)
                        {

                            case 0:
                                textBox1.Text = name;
                                textBox2.Text = value;
                                break;
                            case 1:
                                textBox3.Text = name; ;
                                textBox4.Text = value;
                                break;

                            case 2:
                                textBox5.Text = name; ;
                                textBox6.Text = value;
                                break;

                            case 3:
                                textBox7.Text = name; ;
                                textBox8.Text = value;
                                break;

                            default:

                                break;


                        };


                    }//for
                }//if
            }//if

        }

        private void ClearMetadataFromUI()
        {


            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;

            textBox3.Text = string.Empty;
            textBox4.Text = string.Empty;

            textBox5.Text = string.Empty;
            textBox6.Text = string.Empty;

            textBox7.Text = string.Empty;
            textBox8.Text = string.Empty;

            lblURI.Text = string.Empty;
            lblLastModified.Text = string.Empty;


        }

        private void btnClearContainerResults_Click(object sender, EventArgs e)
        {
            ClearMetadataFromUI();
            txtContainerName.Text = string.Empty;
            cbPublic.Checked = false;
            treeView1.Nodes.Clear();
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            ClearBlobPropertiesFromUI(true);
            ClearBlobMetadataFromUI();
            if (e.Node.ToolTipText != null && e.Node.ToolTipText.Length > 0)
            {
                txtBlobName.Text = e.Node.ToolTipText;
                txtDestinationBlobName.Text = Path.GetFileNameWithoutExtension(e.Node.ToolTipText) + "-copy" + Path.GetExtension(e.Node.ToolTipText);
                PopulateUIWithBlobProperties(txtContainerName.Text, txtBlobName.Text);
            }
        }

        private void btnExecuteBlobFunction_Click(object sender, EventArgs e)
        {
            if ((txtContainerName.Text.Length > 0 && txtBlobName.Text.Length > 0) || cbPutMultipleBlobs.Checked)
            {
                try
                {
                    //CheckConnection();


                    ExecuteBlobFunction(lbBlobFunctions.Text);
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message, "Error executing function " + lbBlobFunctions.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);

                }

            }
            else
            {

                MessageBox.Show("Please select a Container name and then a Blob Name...");

            }
        }

        private void ExecuteBlobFunction(string functionName)
        {
            switch (functionName)
            {
                case "Put Blob":

                    NameValueCollection nv = GetBlobMetaDataFromControls();
                    if (cbPutMultipleBlobs.Checked)
                    {
                        string[] sp = txtBlobPath.Text.Split(';');
                        if (sp != null && sp.Length > 0)
                        {

                            foreach (string s in sp)
                            {
                                try
                                {
                                    StorageHelper.PutBlobFromFile(txtContainerName.Text, Path.GetFileName(s), s, cbOverwrite.Checked, cbCompress.Checked, nv);
                                    statusLabel.Text = String.Format("Blob {0} written to Container {1}", s, txtContainerName.Text);
                                }
                                catch (StorageClientException ex)
                                {
                                    if (ex.ErrorCode == StorageErrorCode.BlobAlreadyExists)
                                        MessageBox.Show("Unable to write to Blob. Please try overwrite.");
                                    else
                                        throw ex;
                                }
                            }
                        }
                    }
                    else
                    {
                        try
                        {
                            StorageHelper.PutBlobFromFile(txtContainerName.Text, txtBlobName.Text, txtBlobPath.Text, cbOverwrite.Checked, cbCompress.Checked, nv);
                            ClearBlobPropertiesFromUI(false);
                            ClearBlobMetadataFromUI();
                            PopulateUIWithBlobProperties(txtContainerName.Text, txtBlobName.Text);
                            statusLabel.Text = String.Format("Blob {0} written to Container {1}", txtBlobName.Text, txtContainerName.Text);
                        }
                        catch (StorageClientException ex)
                        {
                            if (ex.ErrorCode == StorageErrorCode.BlobAlreadyExists)
                                MessageBox.Show("Unable to write to Blob. Please try overwrite.");
                            else
                                throw ex;
                        }
                    }
                    break;
                case "Get Blob":
                    SaveBlobLocally(txtContainerName.Text, txtBlobName.Text, StorageHelper.GetBlobContentsAsFile);

                    break;
                case "Get Blob Properties":
                    if (txtContainerName.Text.Length > 0 && txtBlobName.Text.Length > 0)
                    {
                        PopulateUIWithBlobProperties(txtContainerName.Text, txtBlobName.Text);
                    }
                    else
                    {
                        MessageBox.Show("Please enter a Container Name and Choose a Blob");
                    }

                    break;

                case "Set Blob Metadata":
                    StorageHelper.UpdateBlobMetadata(txtContainerName.Text, txtBlobName.Text, GetBlobMetaDataFromControls());
                    //SetBlobMetaData(txtContainerName.Text, txtBlobName.Text);
                    break;
                case "Get Block List":

                    break;
                case "Delete Blob":
                    if (ConfirmDelete())
                    {
                        StorageHelper.DeleteBlob(txtContainerName.Text, txtBlobName.Text);
                        statusLabel.Text = String.Format("Blob {0} deleted successfully", txtBlobName.Text);

                        ClearMetadataFromUI();
                        treeView1.Nodes.Clear();
                        ListBlobs();

                    }
                    else
                    {
                        statusLabel.Text = String.Format("Delete Blob {0} operation cancelled.", txtBlobName.Text);
                    }
                    break;
                case "Get Blob If Modified":
                    {
                        SaveBlobLocally(txtContainerName.Text, txtBlobName.Text, StorageHelper.GetBlobContentsAsFileIfModified);
                        break;
                    }
                case "Copy Blob":

                    string destinationContainerName = txtContainerName.Text;

                    if (cbDestinationContainer.Text != null && cbDestinationContainer.Text.Length > 0)
                        destinationContainerName = cbDestinationContainer.Text;

                    try
                    {
                        CloudBlob blob = StorageHelper.GetBlob(txtContainerName.Text, txtBlobName.Text);
                        StorageHelper.CopyBlob(blob, destinationContainerName, txtDestinationBlobName.Text, DateTime.UtcNow.ToString());
                        statusLabel.Text = String.Format("Blob {0} copied to Blob {1} successfully", txtBlobName.Text, txtDestinationBlobName.Text);
                    }
                    catch (StorageClientException)
                    {
                        // TODO: Log error. Locally? Service bus?
                        MessageBox.Show(String.Format("Could not copy Blob {0}  to Blob {1}.", txtBlobName.Text, txtDestinationBlobName.Text), "Error Copying",
                           MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    break;

                case "Get Shared Access Url":
                    GetSharedAccessBlobUrl();
                    break;
                default:
                    return;



            };


        }

        private bool ConfirmDelete()
        {
            DialogResult dr = MessageBox.Show(String.Format("Are you sure you want to delete Blob {0}?", txtBlobName.Text),
                 "Delete Blob", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            return (dr == DialogResult.Yes);
        }

        private void PopulateUIWithBlobProperties(string containerName, string blobName)
        {
            CloudBlob blob = StorageHelper.GetBlob(containerName, blobName);
            blob.FetchAttributes();
            BlobProperties bp = blob.Properties;

            txtBlobName.Text = blobName;
            txtContentEncoding.Text = bp.ContentEncoding;
            txtContentLanguage.Text = bp.ContentLanguage;
            txtContentLength.Text = bp.Length.ToString();
            txtContentType.Text = bp.ContentType;
            txtETag.Text = bp.ETag;
            txtLastModified.Text = bp.LastModifiedUtc.ToString("F");
            txtURI.Text = blob.Uri.ToString();

            AddBlobMetadataToControls(blob.Metadata);
        }

        private void AddBlobMetadataToControls(NameValueCollection metadata)
        {
            if (metadata != null && metadata.Count > 0)
            {
                for (int i = 0; i < metadata.Count; i++)
                {
                    string name = metadata.GetKey(i);
                    string value = metadata[i];
                    switch (i)
                    {

                        case 0:
                            textBox9.Text = name;
                            textBox10.Text = value;
                            break;
                        case 1:
                            textBox11.Text = name; ;
                            textBox12.Text = value;
                            break;

                        case 2:
                            textBox13.Text = name; ;
                            textBox14.Text = value;
                            break;

                        case 3:
                            textBox15.Text = name; ;
                            textBox16.Text = value;
                            break;
                        case 4:
                            textBox17.Text = name; ;
                            textBox18.Text = value;
                            break;
                        case 5:
                            textBox19.Text = name; ;
                            textBox20.Text = value;
                            break;


                        default:

                            break;


                    };


                }//for
            }//if
        }

        private NameValueCollection GetBlobMetaDataFromControls()
        {
            NameValueCollection metadata = new NameValueCollection();

            for (int i = 0; i < 6; i++)
            {
                string name = string.Empty;
                string value = string.Empty;
                switch (i)
                {

                    case 0:
                        name = textBox9.Text;
                        value = textBox10.Text;
                        break;
                    case 1:
                        name = textBox11.Text;
                        value = textBox12.Text;
                        break;

                    case 2:
                        name = textBox13.Text;
                        value = textBox14.Text;
                        break;

                    case 3:
                        name = textBox15.Text;
                        value = textBox16.Text;
                        break;
                    case 4:
                        name = textBox17.Text;
                        value = textBox18.Text;
                        break;
                    case 5:
                        name = textBox19.Text;
                        value = textBox20.Text;
                        break;


                    default:

                        break;


                };

                if (name.Length > 0 && value.Length > 0)
                {
                    metadata.Add(name, value);
                }


            }//for

            return metadata;

        }

        private void ClearBlobPropertiesFromUI(bool clearBlobName)
        {
            if (clearBlobName)
            {
                txtBlobName.Text = string.Empty;
            }
            txtContentEncoding.Text = string.Empty;
            txtContentLanguage.Text = string.Empty;
            txtContentLength.Text = string.Empty;
            txtContentType.Text = string.Empty;
            txtETag.Text = string.Empty;
            txtLastModified.Text = string.Empty;
            txtURI.Text = string.Empty;


        }
        private void ClearBlobMetadataFromUI()
        {


            textBox9.Text = string.Empty;
            textBox10.Text = string.Empty;

            textBox11.Text = string.Empty;
            textBox12.Text = string.Empty;

            textBox13.Text = string.Empty;
            textBox14.Text = string.Empty;

            textBox15.Text = string.Empty;
            textBox16.Text = string.Empty;

            textBox17.Text = string.Empty;
            textBox18.Text = string.Empty;

            textBox19.Text = string.Empty;
            textBox20.Text = string.Empty;



        }

        //private bool PutBlob(string containerName, string blobName, string fileName, bool overwrite, bool gZipCompression, NameValueCollection metadata)
        //{

        // BlobProperties blobProperties = new BlobProperties(blobName);
        // blobProperties.ContentType = WindowsAzureStorageHelper.GetContentTypeFromExtension(Path.GetExtension(fileName));
        // blobProperties.Metadata = metadata;


        // BlobContents blobContents = null;
        // bool ret = false;


        // using (FileStream fs = File.OpenRead(fileName))
        // {
        //  blobContents = new BlobContents(fs);
        // ret =  StorageHelper.CreateBlob(containerName, blobProperties, blobContents, overwrite);

        // }



        // return ret;

        //}

        //private BlobProperties GetBlob(string containerName, string blobName, bool transferAsChunks, out BlobContents blobContents)
        //{

        //    blobContents = new BlobContents(new MemoryStream());
        //    return StorageHelper.GetBlob(containerName, blobName, blobContents, transferAsChunks);


        //}

        //private void SaveBlobLocally(BlobProperties blobProperties, BlobContents blobContents)
        //{
        //    if (blobContents != null && blobContents.AsBytes() != null && blobContents.AsBytes().Length > 0)
        //    {
        //        SaveFileDialog save = new SaveFileDialog();
        //        save.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        //        save.Filter = "All files (*.*)|*.*"  ;
        //        if (blobProperties.Name.Contains<char>('/'))
        //        {
        //            save.FileName = blobProperties.Name.Substring(blobProperties.Name.LastIndexOf('/') + 1);
        //        }
        //        else
        //        {
        //            save.FileName = blobProperties.Name;
        //        }


        //        DialogResult dr = save.ShowDialog();
        //        if (dr == DialogResult.OK)
        //        {


        //            File.WriteAllBytes(save.FileName, blobContents.AsBytes());

        //            statusLabel.Text = String.Format("Blob {0} written to File {1}.", blobProperties.Name, save.FileName);

        //        }//if
        //    }//if
        //    else
        //    {

        //        MessageBox.Show("The retrieved contents were blank");

        //    }


        //}

        private void SaveBlobLocally(string containerName, string blobName, Action<string, string, string> saveAction)
        {
            //CloudBlob blob = StorageHelper.GetBlob(containerName, blobName);
            //if (blob.Properties.Length > 0)
            //{
            try
            {
                SaveFileDialog save = new SaveFileDialog();
                save.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                save.Filter = "All files (*.*)|*.*";
                save.FileName = blobName;
                DialogResult dr = save.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    //blob.DownloadToFile(save.FileName);
                    saveAction(containerName, blobName, save.FileName);
                    statusLabel.Text = String.Format("Blob {0} written to File {1}.", blobName, save.FileName);

                }//if
            }
            catch (StorageClientException ex)
            {
                //MessageBox.Show("The retrieved contents were blank");
                MessageBox.Show("Blob was not downloaded successfully: " + ex.Message);
            }
        }

        private void btnFileBrowse_Click(object sender, EventArgs e)
        {
            DialogResult dr = openFileDialog1.ShowDialog();


            if (dr == DialogResult.OK)
            {
                ClearBlobPropertiesFromUI(true);

                if (openFileDialog1.FileNames.Length > 1)
                {
                    cbPutMultipleBlobs.Checked = true;
                    txtBlobPath.Text = "";
                    foreach (string s in openFileDialog1.FileNames)
                    {
                        txtBlobPath.Text += s + ";";

                    }
                    txtBlobPath.Text = txtBlobPath.Text.Remove(txtBlobPath.Text.LastIndexOf(';'));
                }
                else
                {
                    cbPutMultipleBlobs.Checked = false;
                    txtBlobPath.Text = openFileDialog1.FileName;
                    txtBlobName.Text = openFileDialog1.SafeFileName;
                }


            }
        }


        //private void SetBlobMetaData(string containerName, string blobName)
        //{
        //    //BlobProperties props = StorageHelper.GetBlobProperties(containerName, blobName);

        //    //if (props != null)
        //    //{
        //    //    props.Metadata = GetBlobMetaDataFromControls();

        //    //    StorageHelper.UpdateBlobMetadata(containerName, props);

        //    //    props = StorageHelper.GetBlobProperties(containerName, blobName);

        //    //    AddBlobMetadataToControls(props.Metadata);

        //    //}


        //}

        //private bool CopyBlob(string accountName, string sourceContainerName, string blobName, string destinationContainerName, string destinationBlobName)
        //{
        //    BlobProperties sourceBlobProperties = StorageHelper.GetBlobProperties(sourceContainerName, blobName);
        //    sourceBlobProperties.Metadata = GetMetaDataFromControls();
        //    blobName = String.Format("/{0}/{1}/{2}", accountName, sourceContainerName, blobName);
        //    return StorageHelper.CopyBlob(destinationContainerName, sourceBlobProperties, blobName, destinationBlobName, "2009-04-14");

        //}
        #endregion

        #region Queue Functions

        private void btnExecuteQueueFunction_Click(object sender, EventArgs e)
        {
            if (txtQueueName.Text.Length > 0)
            {
                try
                {
                    //CheckConnection();


                    ExecuteQueueFunction(lbQueueFunctions.Text);
                    statusLabel.Text = String.Format("Operation {0} executed successfully.", lbQueueFunctions.Text);
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message, "Error executing function " + lbQueueFunctions.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);

                }

            }
            else
            {

                MessageBox.Show("Please select a Queue name...");

            }

        }

        private void btnListQueues_Click(object sender, EventArgs e)
        {
            try
            {
                ListQueues();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.StackTrace, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void ListQueues()
        {

            //CheckConnection();
            lbQueues.Items.Clear();
           // var queues = StorageHelper.ListQueues(txtlistQueuePrefix.Text);

            var queues = StorageHelper.ListQueuesAsync(txtlistQueuePrefix.Text);
            foreach (var q in queues)
            {
                lbQueues.Items.Add(q.Name);
            }
        }



        private void ExecuteQueueFunction(string functionName)
        {
            switch (functionName)
            {
                case "Create Queue":
                    if (StorageHelper.CreateQueue(txtQueueName.Text))
                    {
                        ListQueues();
                        MessageBox.Show(String.Format("Queue {0} created successfully.", txtQueueName.Text));
                    }

                    else
                    {
                        MessageBox.Show("Unable to create queue. Already exists.");

                    }
                    break;
                case "Delete Queue":
                    try
                    {
                        StorageHelper.DeleteQueue(txtQueueName.Text);
                        ListQueues();
                    }
                    catch (StorageClientException)
                    {
                        MessageBox.Show(String.Format("Unable to delete queue {0}.", txtQueueName.Text));
                    }
                    break;

                case "Get Queue Metadata":

                    GetQueueProperties();
                    break;
                case "Set Queue Metadata":
                    SetQueueProperties();
                    break;
                case "Get Messages":
                    GetMessages();
                    break;
                case "Put Message":
                    PutMessage();
                    break;
                case "Clear Messages":
                    ClearMessages();
                    break;
                case "Peek Messages":
                    PeekMessages();
                    break;
                case "Delete Selected Messages":
                    DeleteSelectedMessages();

                    break;
                default:
                    return;



            };


        }

        private void GetQueueProperties()
        {
            CloudQueue queue = StorageHelper.GetQueue(txtQueueName.Text);

          
            AddQueueMetadataToControls(queue);
        }

        private void SetQueueProperties()
        {
            try
            {
                StorageHelper.SetQueueMetadata(txtQueueName.Text, GetQueueMetaDataFromControls());
                MessageBox.Show("Queue properties set succesfully.");
            }
            catch (StorageClientException)
            {
                // TODO: Log
                MessageBox.Show("Unable to set Queue properties.");
            }
        }

        private void AddQueueMetadataToControls(CloudQueue queue)
        {
            queue.FetchAttributes();
            NameValueCollection metadata = queue.Attributes.Metadata;
            if (metadata != null && metadata.Count > 0)
            {
                for (int i = 0; i < metadata.Count; i++)
                {
                    string name = metadata.GetKey(i);
                    string value = metadata[i];
                    switch (i)
                    {

                        case 0:
                            txtQM1.Text = name;
                            txtQM2.Text = value;
                            break;
                        case 1:
                            txtQM3.Text = name; ;
                            txtQM4.Text = value;
                            break;

                        case 2:
                            txtQM5.Text = name; ;
                            txtQM6.Text = value;
                            break;

                        case 3:
                            txtQM7.Text = name; ;
                            txtQM8.Text = value;
                            break;


                        default:

                            break;


                    };


                }//for
            }//if
        }

        private NameValueCollection GetQueueMetaDataFromControls()
        {
            NameValueCollection metadata = new NameValueCollection();

            for (int i = 0; i < 6; i++)
            {
                string name = string.Empty;
                string value = string.Empty;
                switch (i)
                {

                    case 0:
                        name = txtQM1.Text;
                        value = txtQM2.Text;
                        break;
                    case 1:
                        name = txtQM3.Text;
                        value = txtQM4.Text;
                        break;

                    case 2:
                        name = txtQM5.Text;
                        value = txtQM6.Text;
                        break;

                    case 3:
                        name = txtQM7.Text;
                        value = txtQM8.Text;
                        break;


                    default:

                        break;


                };

                if (name.Length > 0 && value.Length > 0)
                {
                    metadata.Add(name, value);
                }


            }//for

            return metadata;

        }

        private void lbQueues_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearQueueMetadata();
            txtQueueName.Text = lbQueues.Text;
            CloudQueue queue = StorageHelper.GetQueue(txtQueueName.Text);
            AddQueueMetadataToControls(queue);
            lblQueueURI.Text = queue.Uri.ToString();


        }

        private void ClearQueueMetadata()
        {


            txtQM1.Text = string.Empty;
            txtQM2.Text = string.Empty;

            txtQM3.Text = string.Empty;
            txtQM4.Text = string.Empty;

            txtQM5.Text = string.Empty;
            txtQM6.Text = string.Empty;

            txtQM7.Text = string.Empty;
            txtQM8.Text = string.Empty;

            lblURI.Text = string.Empty;
            lblLastModified.Text = string.Empty;


        }

        private void btnClearQueueResults_Click(object sender, EventArgs e)
        {
            ClearQueueMetadata();
            txtQueueName.Text = string.Empty;
            dgvMessages.Rows.Clear();

        }

        private void GetMessages()
        {
            dgvMessages.Rows.Clear();
            IEnumerable<CloudQueueMessage> msgs = StorageHelper.GetMessagesAsync(txtQueueName.Text, int.Parse(txtNumberOfMessages.Text), int.Parse(txtVisibilityTimeoutSecs.Text));

            if (msgs != null)
            {
                PopulateMessagesDataGridView(msgs.ToList());
            }
        }

        private void PeekMessages()
        {
            dgvMessages.Rows.Clear();
            IEnumerable<CloudQueueMessage> msgs = StorageHelper.PeekMessages(txtQueueName.Text, int.Parse(txtNumberOfMessages.Text));
            if (msgs != null)
            {
                PopulateMessagesDataGridView(msgs.ToList());
            }

        }

        private void PopulateMessagesDataGridView(IList<CloudQueueMessage> messages)
        {
            // don't try this in parallel - the DataGridView will throw a fit about threading
            foreach (CloudQueueMessage m in messages)
            {
                queueGridBindingSource.Add(m);
            }
        }

        private void PopulateSingleMessageDataGridView(object message)
        {
            CloudQueueMessage m = message as CloudQueueMessage;
            queueGridBindingSource.Add(m);
        }

        private void PutMessage()
        {
            int count = 1;

            if (txtNumberOfMessages.Text.Length > 0)
            {
                int.TryParse(txtNumberOfMessages.Text, out count);
                if (count == 0) count = 1;

            }

            string messageBody = string.Empty;
            if (txtMessageBody.Text.Length > 0)
            {
                messageBody = txtMessageBody.Text;

            }
            else
            {
                messageBody = String.Format("Message from Windows Azure Storage Operations", System.Guid.NewGuid().ToString("N"));
            }
            int ttlsecs = 300;
            if (txtTimeTolive.Text.Length > 0)
            {
                ttlsecs = int.Parse(txtTimeTolive.Text);
            }

            for (int i = 0; i < count; i++)
            {
                StorageHelper.AddMessageAsync(txtQueueName.Text, new CloudQueueMessage(messageBody), ttlsecs);
                statusLabel.Text = String.Format("Message {0} sent successfully to queue {1}. Count:{2}", messageBody, txtQueueName.Text, count);
            }
        }

        private void ClearMessages()
        {
            StorageHelper.ClearMessages(txtQueueName.Text);
            dgvMessages.Rows.Clear();

        }

        private void DeleteSelectedMessages()
        {
            DataGridViewSelectedRowCollection rows = dgvMessages.SelectedRows;
            if (rows.Count > 0)
            {
                // project into messages collection
                IEnumerable<CloudQueueMessage> messages = from DataGridViewRow row in rows
                                                          select row.DataBoundItem as CloudQueueMessage;
                // delete from storage
                StorageHelper.DeleteMessages(txtQueueName.Text, messages);
                // clean up UI
                foreach (CloudQueueMessage message in messages)
                {
                    queueGridBindingSource.Remove(message);
                }

            }
            else
                MessageBox.Show("Please select a message from the Message Grid and hit the Delete button on your keyboard to delete the message");


        }

        private void DeleteMessage(CloudQueueMessage message)
        {
            StorageHelper.DeleteMessage(txtQueueName.Text, message);
        }


        private void dgvMessages_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            CloudQueueMessage message = (CloudQueueMessage)e.Row.DataBoundItem;
            DeleteMessage(message);
            //string messageId = e.Row.Cells[0].Value.ToString();
            //string messageBody = e.Row.Cells[1].Value.ToString();
            //string popReceipt = e.Row.Cells[4].Value.ToString();
            //try
            //{
            //    DeleteMessage(txtQueueName.Text, messageBody, messageId, popReceipt);
            //    MessageBox.Show(String.Format("Message {0} deleted successfully.", messageId));
            //}
            //catch (StorageClientException)
            //{
            //    //TODO: Log
            //    MessageBox.Show(String.Format("Cound not delete Message {0}.", messageId));
            //}
        }

        private void cbStartReceivingMessages_CheckedChanged(object sender, EventArgs e)
        {

            if (txtQueueName.Text.Length > 0)
            {
                CloudQueue queue = StorageHelper.GetQueue(txtQueueName.Text);
                if (queue.Exists())
                {
                    QueueListener listener = new QueueListener(queue);
                    //Microsoft.Samples.ServiceHosting.StorageClient.MessageQueue mq = StorageHelper.GetQueue(txtQueueName.Text);

                    if (cbStartReceivingMessages.Checked)
                    {
                        listener.MessageReceived += new MessageReceivedEventHandler(listener_MessageReceived);
                        listener.PollInterval = 10000;
                        if (listener.StartReceiving())
                            statusLabel.Text = "Automatic Message receiving started...";
                        else
                            statusLabel.Text = "Could not start automatic Message receiving...";
                        //mq.MessageReceived += new MessageReceivedEventHandler(mq_MessageReceived);
                        //mq.PollInterval = 10000;
                        //if (mq.StartReceiving())
                        //{
                        //    statusLabel.Text = "Automatic Message receiving started...";

                        //}
                        //else
                        //{
                        //    statusLabel.Text = "Could not start automatic Message receiving...";
                        //}

                    }
                    else
                    {
                        //mq.StopReceiving();
                        //mq.MessageReceived -= new MessageReceivedEventHandler(mq_MessageReceived);
                        listener.StopReceiving();
                        listener.MessageReceived -= new MessageReceivedEventHandler(listener_MessageReceived);

                    }
                }
            }//if
            else
            {
                MessageBox.Show("Please select a Queue.");

            }
        }

        void listener_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            //ThreadPool.QueueUserWorkItem(new WaitCallback(PopulateSingleMessageDataGridView), e.Message);
            CloudQueueMessage m = e.Message as CloudQueueMessage;
            if (dgvMessages.InvokeRequired)
            {
                dgvMessages.Invoke(new MethodInvoker(delegate { queueGridBindingSource.Add(m); }));
            }

        }

        //void mq_MessageReceived(object sender, MessageReceivedEventArgs e)
        //{
        //    ThreadPool.QueueUserWorkItem(new WaitCallback(PopulateSingleMessageDataGridView), e.Message);

        //}

        #endregion

        private void dgvMessages_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {

        }







        private void btnSendQueueMessage3P_Click(object sender, EventArgs e)
        {
            try
            {
                //  SendQueueMessage3P(txtContainerName3P.Text, txtBlobName3P.Text, txtCorrId.Text);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error sending queue message", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void SendQueueMessage3P(string containerName, string blobFileName, string correlationId)
        {

            string queueName = ConfigurationManager.AppSettings["inputfilequeuename"];
            //CheckConnection();
            //bool alreadyExists;
            StorageHelper.CreateQueue(queueName);

            string messageBody = String.Format("{0}:{1}:{2}", correlationId, containerName, blobFileName);
            int ttlsecs = 300;

            StorageHelper.AddMessage(queueName, new CloudQueueMessage(messageBody), ttlsecs);
            this.statusLabel.Text = String.Format("Message {0} sent successfully to queue {1}", messageBody, queueName);
        }




        private void AddLog(string message)
        {
            //txtLog.AppendText(String.Format(">{0}{1}", message, Environment.NewLine));
            statusLabel.Text = String.Format(">{0}{1}", message, Environment.NewLine);

        }

      

      

     

       

      
    }
}
