using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.WindowsAzure.StorageClient;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using System.IO;
using System.Data;
using System.Collections;
using ProAzureCommonLib;

namespace SLWebService.silverlining
{
    public partial class DrivesManger : System.Web.UI.Page
    {

     
        public const string LOCAL_STORAGE_NAME = "SilverliningLocalStorage";
      
        private string LocalStoragePath
        {
            get { return WindowsAzureSystemHelper.GetLocalStorageRootPath(LOCAL_STORAGE_NAME); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ListAllMountedDrives();
            //if (!this.IsPostBack)
            //{
            //    FillTree();
            //}
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                MountDrive();
            }
            catch (Exception ex)
            {

                lblMsg.Text = ex.Message;
            }
        }

        private void MountDrive()
        {
            string storageAccountName = txtStorageAccountName.Text;
            string storageAccountKey = txtStorageAccountKey.Text;


            try
            {

                LocalResource resource = WindowsAzureSystemHelper.GetLocalStorageResource(LOCAL_STORAGE_NAME);
                int cacheSize = int.Parse(txtCacheSize.Text);
                if (resource.MaximumSizeInMegabytes < cacheSize)
                {
                    throw new Exception(String.Format("Cache size {0} MB cannot be more than maximum permitted size {1} MB", cacheSize, resource.MaximumSizeInMegabytes));
                }
                bool wasAlreadyMounted;
                string driveName = WADriveHelper.MountAzureDrive(storageAccountName, storageAccountKey, txtContainerName.Text, txtPageBlobName.Text, LocalStoragePath + txtCacheDirName.Text,
                     int.Parse(txtCacheSize.Text),
                     int.Parse(txtDriveSize.Text),
                     DriveMountOptions.None, out wasAlreadyMounted);

                lblMsg.Text = String.Format("Mounted Blob {0} to Drive {1}", txtPageBlobName.Text, driveName);

                if (cbCreateTestFile.Checked)
                {
                    try
                    {
                        WADriveHelper.WriteTestFile(driveName + System.Guid.NewGuid().ToString("N") + "silverlining-test.txt", "test");
                    }
                    catch (Exception ey)
                    {
                        lblMsg.Text = "Could not write test file to the drive " + ey.Message;
                    }
                }
                ListAllMountedDrives();
                FillTree();
            }
            catch (Exception ex)
            {

                lblMsg.Text = ex.Message;
            }
        }

        private void MountSnapshot(string snapshotUri)
        {
            string storageAccountName = txtStorageAccountName.Text;
            string storageAccountKey = txtStorageAccountKey.Text;


            try
            {

                LocalResource resource = WindowsAzureSystemHelper.GetLocalStorageResource(LOCAL_STORAGE_NAME);
                int cacheSize = int.Parse(txtCacheSize.Text);
                if (resource.MaximumSizeInMegabytes < cacheSize)
                {
                    throw new Exception(String.Format("Cache size {0} MB cannot be more than maximum permitted size {1} MB", cacheSize, resource.MaximumSizeInMegabytes));
                }
                bool wasAlreadyMounted;
                string driveName = WADriveHelper.MountAzureDrive(storageAccountName, storageAccountKey, new Uri(snapshotUri), LocalStoragePath + txtCacheDirName.Text + ".snapshot",
                     int.Parse(txtCacheSize.Text),
                     int.Parse(txtDriveSize.Text),
                     DriveMountOptions.None, out wasAlreadyMounted);

                lblMsg.Text = String.Format("Mounted Snapshot {0} to Drive {1}", snapshotUri, driveName);

              
                ListAllMountedDrives();
            }
            catch (Exception ex)
            {

                lblMsg.Text = ex.Message;
            }
        }

        public void DeleteDrive()
        {
            TreeView1.Nodes.Clear();
            if (lstDrives.SelectedIndex > -1)
            {
                // string[] split = lstMountedDrives.SelectedItem.Text.Split(new string[]{">"}, StringSplitOptions.RemoveEmptyEntries);
                string[] split = lstDrives.SelectedItem.Text.Split('>');
                string driveName = split[0].Trim();
                string uriString = split[1].Trim();

                string storageAccountName = txtStorageAccountName.Text;
                string storageAccountKey = txtStorageAccountKey.Text;


                try
                {

                    WADriveHelper.DeleteAzureDrive(storageAccountName, storageAccountKey, uriString);

                    lblMsg.Text = String.Format("Deleted drive {0} with blob Uri {1}.", driveName, uriString);
                    lstDrives.Items.Clear();
                    ListAllMountedDrives();
                }
                catch (Exception ex)
                {

                    lblMsg.Text = ex.Message;
                }
            }
            else
            {

                lblMsg.Text = "Please select a drive.";
            }
        }
        public void ListAllMountedDrives()
        {
            try
            {
               
               IDictionary<string, Uri> drives = CloudDrive.GetMountedDrives();

               if (drives != null)
               {
                  IEnumerator<KeyValuePair<string, Uri>> e =  drives.GetEnumerator();

                  while (e.MoveNext())
                  {
                      string item = string.Format("{0}>{1}", e.Current.Key, e.Current.Value);
                      if (!lstDrives.Items.Contains(new ListItem(item)))
                      {
                          lstDrives.Items.Add(new ListItem(string.Format("{0}>{1}", e.Current.Key, e.Current.Value)));
                      }

                  }
                   
               }
            }
            catch (Exception ex)
            {

                //lblMsg.Text = ex.Message;
            }

        }

        protected void btnUnmount_Click(object sender, EventArgs e)
        {
            try
            {
                UnmountDrive();
            }
            catch (Exception ex)
            {

                lblMsg.Text = ex.Message;
            }
        }

        private void UnmountDrive()
        {
            TreeView1.Nodes.Clear();
            if (lstDrives.SelectedIndex > -1)
            {
                // string[] split = lstMountedDrives.SelectedItem.Text.Split(new string[]{">"}, StringSplitOptions.RemoveEmptyEntries);
                string[] split = lstDrives.SelectedItem.Text.Split('>');
                string driveName = split[0].Trim();
                string uriString = split[1].Trim();

                string storageAccountName = txtStorageAccountName.Text;
                string storageAccountKey = txtStorageAccountKey.Text;


                try
                {

                    WADriveHelper.UnmountAzureDrive(storageAccountName, storageAccountKey, uriString);

                    lblMsg.Text = String.Format("Unmounted drive {0} with blob Uri {1}.", driveName, uriString);
                    lstDrives.Items.Clear();
                    ListAllMountedDrives();
                    FillTree();
                }
                catch (Exception ex)
                {

                    lblMsg.Text = ex.Message;
                }
            }
            else
            {

                lblMsg.Text = "Please select a drive.";
            }

        }

        public void PopulateTreeView(string directoryValue, TreeNode parentNode)
        {
            

            try
            {
                string[] directoryArray =
                Directory.GetDirectories(directoryValue);
                if (directoryArray.Length != 0)
                {
                   
                    foreach (string directory in directoryArray)
                    {

                        DirectoryInfo d = new DirectoryInfo(directory);

                        TreeNode myNode = new TreeNode(d.Name);

                        parentNode.ChildNodes.Add(myNode);

                        string[] files = Directory.GetFiles(directory, "*.*");
                        foreach (string f in files)
                        {

                            myNode.ChildNodes.Add(new TreeNode(Path.GetFileName(f)));
                        }


                        PopulateTreeView(directory, myNode);
                    }
                }
                else
                {

                    //string[] files = Directory.GetFiles(directoryValue, "*.*");
                    //foreach (string f in files)
                    //{

                    //    parentNode.ChildNodes.Add(new TreeNode(Path.GetFileName(f)));
                    //}
                }



            }
            catch (UnauthorizedAccessException)
            {
                //parentNode.ChildNodes.Add("Access denied");
            } // end catch
        }

        protected void btnViewFiles_Click(object sender, EventArgs e)
        {
            FillTree();
        }

        private void FillTree()
        {
            TreeView1.Nodes.Clear();
            TreeNode rootNode = new TreeNode(System.Environment.MachineName);
            rootNode.Expanded = true;
            TreeView1.Nodes.Add(rootNode);
            if (lstDrives.Items.Count > 0)
            {
               
                foreach (ListItem li in lstDrives.Items)
                {

                    // string[] split = lstMountedDrives.SelectedItem.Text.Split(new string[] { "-->" }, StringSplitOptions.None);
                    string[] split = li.Text.Split('>');
                    string driveName = split[0].Trim();
                    string uriString = split[1].Trim();

                    String path = driveName;
                    TreeNode driveNode = new TreeNode(path);
                    rootNode.ChildNodes.Add(driveNode);
                    string[] files = Directory.GetFiles(path, "*.*");
                    foreach (string f in files)
                    {

                        driveNode.ChildNodes.Add(new TreeNode(Path.GetFileName(f)));
                    }
                    PopulateTreeView(path, driveNode);
                }
                TreeView1.Visible = true;
            }
            lstDrives.Items.Clear();
            ListAllMountedDrives();
        }

        protected void btnSnapshot_Click(object sender, EventArgs e)
        {
            TreeView1.Nodes.Clear();
            if (lstDrives.SelectedIndex > -1)
            {
                // string[] split = lstMountedDrives.SelectedItem.Text.Split(new string[]{">"}, StringSplitOptions.RemoveEmptyEntries);
                string[] split = lstDrives.SelectedItem.Text.Split('>');
                string driveName = split[0].Trim();
                string uriString = split[1].Trim();

                string storageAccountName = txtStorageAccountName.Text;
                string storageAccountKey = txtStorageAccountKey.Text;


                try
                {

                   string snapshotUri = WADriveHelper.SnapshotAzureDrive(storageAccountName, storageAccountKey, uriString);

                    lblMsg.Text = String.Format("Snapshot of drive {0} has Uri {1}.", driveName, snapshotUri);

                    MountSnapshot(snapshotUri);

                    lstDrives.Items.Clear();
                    ListAllMountedDrives();
                    FillTree();
                }
                catch (Exception ex)
                {

                    lblMsg.Text = ex.Message;
                }
            }
            else
            {

                lblMsg.Text = "Please select a drive.";
            }

        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteDrive();
        }

             

       

        protected void btnUploadFileToDrive_Click(object sender, EventArgs e)
        {
            try
            {
                UploadFiles("images");
                UploadFiles(null);
                lblMsg.Text = "File uploaded to the drive.";
                FillTree();
            }
            catch (Exception ex)
            {

                lblMsg.Text = ex.Message;
            }
        }

        private void UploadFiles(string directoryName)
        {
            try
            {
                // Get the HttpFileCollection
                if (FileUpload1.HasFile)
                {
                    if (lstDrives.SelectedIndex > -1)
                    {
                        // string[] split = lstMountedDrives.SelectedItem.Text.Split(new string[]{">"}, StringSplitOptions.RemoveEmptyEntries);
                        string[] split = lstDrives.SelectedItem.Text.Split('>');
                        string driveName = split[0].Trim();
                        string uriString = split[1].Trim();
                        string filePath = driveName;
                        if(!string.IsNullOrEmpty(directoryName))
                        {
                            if (!Directory.Exists(driveName + directoryName))
                            {
                                Directory.CreateDirectory(driveName + directoryName);
                            }
                            filePath = driveName + directoryName + "\\";
                        }
                        filePath = filePath + FileUpload1.FileName;
                        FileUpload1.SaveAs(filePath);
                    }
                    else
                    {
                        lblMsg.Text = "Please select a drive to upload the file to.";

                    }


                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
        }
    }
}