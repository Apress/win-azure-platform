namespace ServiceManagement
{
    partial class ServiceManagementForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServiceManagementForm));
            this.txtLog = new System.Windows.Forms.TextBox();
            this.btnLoadCert = new System.Windows.Forms.Button();
            this.lstOperations = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnGuidName = new System.Windows.Forms.Button();
            this.label27 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.ddlLocations = new System.Windows.Forms.ComboBox();
            this.ddlAffinityGroups = new System.Windows.Forms.ComboBox();
            this.txtHostedServiceLabel = new System.Windows.Forms.TextBox();
            this.txtHostedServiceName = new System.Windows.Forms.TextBox();
            this.lblDeploymentId = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.txtServiceCertThumbprint = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.txtServiceCertAlgo = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.txtCertPassword = new System.Windows.Forms.TextBox();
            this.btnBrowseServiceCertificate = new System.Windows.Forms.Button();
            this.txtServiceCertificate = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtThumbprint = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnExecute = new System.Windows.Forms.Button();
            this.txtOpId = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.txtDomainNumber = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.ddlMode = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.ddlDeploymentStatus = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtStagingDeploymentName = new System.Windows.Forms.TextBox();
            this.btnBrowseConfigFile = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.txtConfigFilePath = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtPackageUri = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtDeploymentLabel = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtDeploymentName = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.ddlSlotType = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.ddlKeyType = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.ddlResourceType = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtResourceName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSubscriptionId = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.label20 = new System.Windows.Forms.Label();
            this.btnChangeInstances = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.RoleName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Instances = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NewInstanceCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ddlStorage = new System.Windows.Forms.ComboBox();
            this.label21 = new System.Windows.Forms.Label();
            this.txtAvgCpu = new System.Windows.Forms.TextBox();
            this.txtAvgMemory = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.btnGetPerfData = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // txtLog
            // 
            this.txtLog.AcceptsReturn = true;
            this.txtLog.BackColor = System.Drawing.SystemColors.HighlightText;
            this.txtLog.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtLog.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLog.ForeColor = System.Drawing.Color.MidnightBlue;
            this.txtLog.Location = new System.Drawing.Point(0, 636);
            this.txtLog.Margin = new System.Windows.Forms.Padding(4);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(973, 226);
            this.txtLog.TabIndex = 0;
            this.txtLog.Text = "---------------------Output Window--------------------------";
            // 
            // btnLoadCert
            // 
            this.btnLoadCert.Location = new System.Drawing.Point(468, 66);
            this.btnLoadCert.Margin = new System.Windows.Forms.Padding(4);
            this.btnLoadCert.Name = "btnLoadCert";
            this.btnLoadCert.Size = new System.Drawing.Size(116, 32);
            this.btnLoadCert.TabIndex = 1;
            this.btnLoadCert.Text = "Load Certificate";
            this.btnLoadCert.UseVisualStyleBackColor = true;
            this.btnLoadCert.Click += new System.EventHandler(this.btnLoadCert_Click);
            // 
            // lstOperations
            // 
            this.lstOperations.BackColor = System.Drawing.SystemColors.Info;
            this.lstOperations.ForeColor = System.Drawing.Color.Brown;
            this.lstOperations.FormattingEnabled = true;
            this.lstOperations.ItemHeight = 16;
            this.lstOperations.Items.AddRange(new object[] {
            "--Service Management Operations--",
            "List Affinity Groups",
            "Get Affinity Group",
            "Create Storage Service",
            "Delete Storage Service",
            "Update Storage Service",
            "List Storage Services",
            "Get Storage Service",
            "Get Storage Keys",
            "Regenerate Keys",
            "List Hosted Services",
            "Create Hosted Service",
            "Get Hosted Service",
            "Update Hosted Service",
            "Delete Hosted Service",
            "Get Operation Status",
            "Get Deployment",
            "Create Deployment",
            "Swap Deployment",
            "Delete Deployment",
            "Update Deployment Status",
            "Change Deployment Config",
            "Upgrade Deployment",
            "Walk Upgrade Domain",
            "List Certificates",
            "Add Certificate",
            "Delete Certificate",
            "List Operating Systems",
            "List Locations"});
            this.lstOperations.Location = new System.Drawing.Point(0, 0);
            this.lstOperations.Margin = new System.Windows.Forms.Padding(4);
            this.lstOperations.Name = "lstOperations";
            this.lstOperations.Size = new System.Drawing.Size(353, 404);
            this.lstOperations.TabIndex = 3;
            this.lstOperations.SelectedIndexChanged += new System.EventHandler(this.lstOperations_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnGuidName);
            this.groupBox1.Controls.Add(this.label27);
            this.groupBox1.Controls.Add(this.label26);
            this.groupBox1.Controls.Add(this.label25);
            this.groupBox1.Controls.Add(this.label24);
            this.groupBox1.Controls.Add(this.ddlLocations);
            this.groupBox1.Controls.Add(this.ddlAffinityGroups);
            this.groupBox1.Controls.Add(this.txtHostedServiceLabel);
            this.groupBox1.Controls.Add(this.txtHostedServiceName);
            this.groupBox1.Controls.Add(this.lblDeploymentId);
            this.groupBox1.Controls.Add(this.label19);
            this.groupBox1.Controls.Add(this.txtServiceCertThumbprint);
            this.groupBox1.Controls.Add(this.label18);
            this.groupBox1.Controls.Add(this.txtServiceCertAlgo);
            this.groupBox1.Controls.Add(this.label17);
            this.groupBox1.Controls.Add(this.txtCertPassword);
            this.groupBox1.Controls.Add(this.btnBrowseServiceCertificate);
            this.groupBox1.Controls.Add(this.txtServiceCertificate);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtThumbprint);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.btnClear);
            this.groupBox1.Controls.Add(this.btnExecute);
            this.groupBox1.Controls.Add(this.txtOpId);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.txtDomainNumber);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.ddlMode);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.ddlDeploymentStatus);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.txtStagingDeploymentName);
            this.groupBox1.Controls.Add(this.btnBrowseConfigFile);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.txtConfigFilePath);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.txtPackageUri);
            this.groupBox1.Controls.Add(this.btnLoadCert);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.txtDeploymentLabel);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.txtDeploymentName);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.ddlSlotType);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.ddlKeyType);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.ddlResourceType);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtResourceName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtSubscriptionId);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupBox1.Location = new System.Drawing.Point(353, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(620, 636);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Account Information";
            // 
            // btnGuidName
            // 
            this.btnGuidName.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGuidName.Location = new System.Drawing.Point(171, 564);
            this.btnGuidName.Margin = new System.Windows.Forms.Padding(4);
            this.btnGuidName.Name = "btnGuidName";
            this.btnGuidName.Size = new System.Drawing.Size(50, 27);
            this.btnGuidName.TabIndex = 66;
            this.btnGuidName.Text = "GUID";
            this.btnGuidName.UseVisualStyleBackColor = true;
            this.btnGuidName.Click += new System.EventHandler(this.btnGuidName_Click);
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(386, 516);
            this.label27.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(94, 17);
            this.label27.TabIndex = 65;
            this.label27.Text = "Service Label";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(9, 539);
            this.label26.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(96, 17);
            this.label26.TabIndex = 64;
            this.label26.Text = "Service Name";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(386, 468);
            this.label25.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(69, 17);
            this.label25.TabIndex = 63;
            this.label25.Text = "Locations";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(386, 418);
            this.label24.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(101, 17);
            this.label24.TabIndex = 62;
            this.label24.Text = "Affinity Groups";
            // 
            // ddlLocations
            // 
            this.ddlLocations.BackColor = System.Drawing.SystemColors.Window;
            this.ddlLocations.FormattingEnabled = true;
            this.ddlLocations.Location = new System.Drawing.Point(389, 487);
            this.ddlLocations.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ddlLocations.Name = "ddlLocations";
            this.ddlLocations.Size = new System.Drawing.Size(207, 24);
            this.ddlLocations.TabIndex = 61;
            // 
            // ddlAffinityGroups
            // 
            this.ddlAffinityGroups.BackColor = System.Drawing.SystemColors.Window;
            this.ddlAffinityGroups.FormattingEnabled = true;
            this.ddlAffinityGroups.Location = new System.Drawing.Point(389, 442);
            this.ddlAffinityGroups.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ddlAffinityGroups.Name = "ddlAffinityGroups";
            this.ddlAffinityGroups.Size = new System.Drawing.Size(207, 24);
            this.ddlAffinityGroups.TabIndex = 60;
            // 
            // txtHostedServiceLabel
            // 
            this.txtHostedServiceLabel.BackColor = System.Drawing.SystemColors.Window;
            this.txtHostedServiceLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtHostedServiceLabel.Location = new System.Drawing.Point(389, 541);
            this.txtHostedServiceLabel.Margin = new System.Windows.Forms.Padding(4);
            this.txtHostedServiceLabel.Name = "txtHostedServiceLabel";
            this.txtHostedServiceLabel.Size = new System.Drawing.Size(207, 23);
            this.txtHostedServiceLabel.TabIndex = 59;
            this.txtHostedServiceLabel.Text = "testlabel";
            // 
            // txtHostedServiceName
            // 
            this.txtHostedServiceName.BackColor = System.Drawing.SystemColors.Window;
            this.txtHostedServiceName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtHostedServiceName.Location = new System.Drawing.Point(171, 541);
            this.txtHostedServiceName.Margin = new System.Windows.Forms.Padding(4);
            this.txtHostedServiceName.Name = "txtHostedServiceName";
            this.txtHostedServiceName.Size = new System.Drawing.Size(207, 23);
            this.txtHostedServiceName.TabIndex = 58;
            this.txtHostedServiceName.Text = "silverlining";
            // 
            // lblDeploymentId
            // 
            this.lblDeploymentId.AutoSize = true;
            this.lblDeploymentId.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDeploymentId.ForeColor = System.Drawing.Color.Navy;
            this.lblDeploymentId.Location = new System.Drawing.Point(386, 321);
            this.lblDeploymentId.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDeploymentId.Name = "lblDeploymentId";
            this.lblDeploymentId.Size = new System.Drawing.Size(0, 13);
            this.lblDeploymentId.TabIndex = 57;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(396, 169);
            this.label19.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(105, 17);
            this.label19.TabIndex = 47;
            this.label19.Text = "Resource Type";
            // 
            // txtServiceCertThumbprint
            // 
            this.txtServiceCertThumbprint.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtServiceCertThumbprint.Location = new System.Drawing.Point(171, 167);
            this.txtServiceCertThumbprint.Margin = new System.Windows.Forms.Padding(4);
            this.txtServiceCertThumbprint.Name = "txtServiceCertThumbprint";
            this.txtServiceCertThumbprint.Size = new System.Drawing.Size(207, 23);
            this.txtServiceCertThumbprint.TabIndex = 46;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(311, 138);
            this.label18.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(67, 17);
            this.label18.TabIndex = 45;
            this.label18.Text = "Algorithm";
            // 
            // txtServiceCertAlgo
            // 
            this.txtServiceCertAlgo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtServiceCertAlgo.Location = new System.Drawing.Point(399, 135);
            this.txtServiceCertAlgo.Margin = new System.Windows.Forms.Padding(4);
            this.txtServiceCertAlgo.Name = "txtServiceCertAlgo";
            this.txtServiceCertAlgo.Size = new System.Drawing.Size(37, 23);
            this.txtServiceCertAlgo.TabIndex = 44;
            this.txtServiceCertAlgo.Text = "md5";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(9, 135);
            this.label17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(99, 17);
            this.label17.TabIndex = 43;
            this.label17.Text = "Cert Password";
            // 
            // txtCertPassword
            // 
            this.txtCertPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCertPassword.Location = new System.Drawing.Point(171, 135);
            this.txtCertPassword.Margin = new System.Windows.Forms.Padding(4);
            this.txtCertPassword.Name = "txtCertPassword";
            this.txtCertPassword.Size = new System.Drawing.Size(125, 23);
            this.txtCertPassword.TabIndex = 42;
            this.txtCertPassword.UseSystemPasswordChar = true;
            // 
            // btnBrowseServiceCertificate
            // 
            this.btnBrowseServiceCertificate.Location = new System.Drawing.Point(468, 102);
            this.btnBrowseServiceCertificate.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnBrowseServiceCertificate.Name = "btnBrowseServiceCertificate";
            this.btnBrowseServiceCertificate.Size = new System.Drawing.Size(93, 30);
            this.btnBrowseServiceCertificate.TabIndex = 41;
            this.btnBrowseServiceCertificate.Text = "Browse...";
            this.btnBrowseServiceCertificate.UseVisualStyleBackColor = true;
            this.btnBrowseServiceCertificate.Click += new System.EventHandler(this.btnBrowseServiceCertificate_Click);
            // 
            // txtServiceCertificate
            // 
            this.txtServiceCertificate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtServiceCertificate.Location = new System.Drawing.Point(171, 106);
            this.txtServiceCertificate.Margin = new System.Windows.Forms.Padding(4);
            this.txtServiceCertificate.Name = "txtServiceCertificate";
            this.txtServiceCertificate.Size = new System.Drawing.Size(293, 23);
            this.txtServiceCertificate.TabIndex = 40;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 106);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(122, 17);
            this.label2.TabIndex = 39;
            this.label2.Text = "Service Certificate";
            // 
            // txtThumbprint
            // 
            this.txtThumbprint.BackColor = System.Drawing.Color.Yellow;
            this.txtThumbprint.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtThumbprint.Location = new System.Drawing.Point(171, 73);
            this.txtThumbprint.Margin = new System.Windows.Forms.Padding(4);
            this.txtThumbprint.Name = "txtThumbprint";
            this.txtThumbprint.Size = new System.Drawing.Size(293, 23);
            this.txtThumbprint.TabIndex = 38;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(9, 73);
            this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(156, 17);
            this.label16.TabIndex = 37;
            this.label16.Text = "Mgmt. Cert. Thumbprint";
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(342, 595);
            this.btnClear.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(107, 27);
            this.btnClear.TabIndex = 34;
            this.btnClear.Text = "Clear Output";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnExecute
            // 
            this.btnExecute.Location = new System.Drawing.Point(171, 595);
            this.btnExecute.Margin = new System.Windows.Forms.Padding(4);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(147, 27);
            this.btnExecute.TabIndex = 33;
            this.btnExecute.Text = "Execute Operation";
            this.btnExecute.UseVisualStyleBackColor = true;
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // txtOpId
            // 
            this.txtOpId.BackColor = System.Drawing.SystemColors.Window;
            this.txtOpId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtOpId.Location = new System.Drawing.Point(171, 505);
            this.txtOpId.Margin = new System.Windows.Forms.Padding(4);
            this.txtOpId.Name = "txtOpId";
            this.txtOpId.Size = new System.Drawing.Size(207, 23);
            this.txtOpId.TabIndex = 13;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 505);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 17);
            this.label5.TabIndex = 13;
            this.label5.Text = "OP-ID";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(9, 473);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(110, 17);
            this.label15.TabIndex = 32;
            this.label15.Text = "Domain Number";
            // 
            // txtDomainNumber
            // 
            this.txtDomainNumber.BackColor = System.Drawing.SystemColors.Window;
            this.txtDomainNumber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDomainNumber.Location = new System.Drawing.Point(171, 473);
            this.txtDomainNumber.Margin = new System.Windows.Forms.Padding(4);
            this.txtDomainNumber.Name = "txtDomainNumber";
            this.txtDomainNumber.Size = new System.Drawing.Size(207, 23);
            this.txtDomainNumber.TabIndex = 14;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(9, 442);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(102, 17);
            this.label14.TabIndex = 31;
            this.label14.Text = "Upgrade Mode";
            // 
            // ddlMode
            // 
            this.ddlMode.BackColor = System.Drawing.SystemColors.Window;
            this.ddlMode.FormattingEnabled = true;
            this.ddlMode.Items.AddRange(new object[] {
            "auto",
            "manual"});
            this.ddlMode.Location = new System.Drawing.Point(171, 442);
            this.ddlMode.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ddlMode.Name = "ddlMode";
            this.ddlMode.Size = new System.Drawing.Size(207, 24);
            this.ddlMode.TabIndex = 30;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(9, 411);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(127, 17);
            this.label13.TabIndex = 29;
            this.label13.Text = "Deployment Status";
            // 
            // ddlDeploymentStatus
            // 
            this.ddlDeploymentStatus.BackColor = System.Drawing.SystemColors.Window;
            this.ddlDeploymentStatus.FormattingEnabled = true;
            this.ddlDeploymentStatus.Items.AddRange(new object[] {
            "running",
            "suspended"});
            this.ddlDeploymentStatus.Location = new System.Drawing.Point(171, 411);
            this.ddlDeploymentStatus.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ddlDeploymentStatus.Name = "ddlDeploymentStatus";
            this.ddlDeploymentStatus.Size = new System.Drawing.Size(207, 24);
            this.ddlDeploymentStatus.TabIndex = 28;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(381, 265);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(176, 17);
            this.label12.TabIndex = 27;
            this.label12.Text = "Staging Deployment Name";
            // 
            // txtStagingDeploymentName
            // 
            this.txtStagingDeploymentName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtStagingDeploymentName.Location = new System.Drawing.Point(385, 288);
            this.txtStagingDeploymentName.Margin = new System.Windows.Forms.Padding(4);
            this.txtStagingDeploymentName.Name = "txtStagingDeploymentName";
            this.txtStagingDeploymentName.Size = new System.Drawing.Size(207, 23);
            this.txtStagingDeploymentName.TabIndex = 26;
            // 
            // btnBrowseConfigFile
            // 
            this.btnBrowseConfigFile.Location = new System.Drawing.Point(468, 375);
            this.btnBrowseConfigFile.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnBrowseConfigFile.Name = "btnBrowseConfigFile";
            this.btnBrowseConfigFile.Size = new System.Drawing.Size(93, 30);
            this.btnBrowseConfigFile.TabIndex = 25;
            this.btnBrowseConfigFile.Text = "Browse...";
            this.btnBrowseConfigFile.UseVisualStyleBackColor = true;
            this.btnBrowseConfigFile.Click += new System.EventHandler(this.btnBrowseConfigFile_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(9, 382);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(145, 17);
            this.label11.TabIndex = 24;
            this.label11.Text = "Local Config File Path";
            // 
            // txtConfigFilePath
            // 
            this.txtConfigFilePath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtConfigFilePath.Location = new System.Drawing.Point(171, 382);
            this.txtConfigFilePath.Margin = new System.Windows.Forms.Padding(4);
            this.txtConfigFilePath.Name = "txtConfigFilePath";
            this.txtConfigFilePath.Size = new System.Drawing.Size(293, 23);
            this.txtConfigFilePath.TabIndex = 23;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(9, 350);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(117, 17);
            this.label10.TabIndex = 22;
            this.label10.Text = "Package Blob Url";
            // 
            // txtPackageUri
            // 
            this.txtPackageUri.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPackageUri.Location = new System.Drawing.Point(171, 350);
            this.txtPackageUri.Margin = new System.Windows.Forms.Padding(4);
            this.txtPackageUri.Name = "txtPackageUri";
            this.txtPackageUri.Size = new System.Drawing.Size(421, 23);
            this.txtPackageUri.TabIndex = 21;
            this.txtPackageUri.Text = "http://proazurestorage.blob.core.windows.net/silverliningpackages/SilverliningWeb" +
    "Service.cspkg";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(9, 319);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(122, 17);
            this.label9.TabIndex = 20;
            this.label9.Text = "Deployment Label";
            // 
            // txtDeploymentLabel
            // 
            this.txtDeploymentLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDeploymentLabel.Location = new System.Drawing.Point(171, 319);
            this.txtDeploymentLabel.Margin = new System.Windows.Forms.Padding(4);
            this.txtDeploymentLabel.Name = "txtDeploymentLabel";
            this.txtDeploymentLabel.Size = new System.Drawing.Size(207, 23);
            this.txtDeploymentLabel.TabIndex = 19;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(9, 288);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(124, 17);
            this.label8.TabIndex = 18;
            this.label8.Text = "Deployment Name";
            // 
            // txtDeploymentName
            // 
            this.txtDeploymentName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDeploymentName.Location = new System.Drawing.Point(171, 288);
            this.txtDeploymentName.Margin = new System.Windows.Forms.Padding(4);
            this.txtDeploymentName.Name = "txtDeploymentName";
            this.txtDeploymentName.Size = new System.Drawing.Size(207, 23);
            this.txtDeploymentName.TabIndex = 17;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 257);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(68, 17);
            this.label7.TabIndex = 16;
            this.label7.Text = "Slot Type";
            // 
            // ddlSlotType
            // 
            this.ddlSlotType.BackColor = System.Drawing.Color.White;
            this.ddlSlotType.FormattingEnabled = true;
            this.ddlSlotType.Items.AddRange(new object[] {
            "staging",
            "production"});
            this.ddlSlotType.Location = new System.Drawing.Point(171, 257);
            this.ddlSlotType.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ddlSlotType.Name = "ddlSlotType";
            this.ddlSlotType.Size = new System.Drawing.Size(207, 24);
            this.ddlSlotType.TabIndex = 15;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 226);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(68, 17);
            this.label6.TabIndex = 14;
            this.label6.Text = "Key Type";
            // 
            // ddlKeyType
            // 
            this.ddlKeyType.BackColor = System.Drawing.SystemColors.Window;
            this.ddlKeyType.FormattingEnabled = true;
            this.ddlKeyType.Items.AddRange(new object[] {
            "primary",
            "secondary"});
            this.ddlKeyType.Location = new System.Drawing.Point(171, 226);
            this.ddlKeyType.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ddlKeyType.Name = "ddlKeyType";
            this.ddlKeyType.Size = new System.Drawing.Size(207, 24);
            this.ddlKeyType.TabIndex = 13;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 197);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(110, 17);
            this.label4.TabIndex = 12;
            this.label4.Text = "Resource Name";
            // 
            // ddlResourceType
            // 
            this.ddlResourceType.BackColor = System.Drawing.SystemColors.Window;
            this.ddlResourceType.FormattingEnabled = true;
            this.ddlResourceType.Items.AddRange(new object[] {
            "Hosted Service Name",
            "Storage Account Name",
            "Affinity Group Name"});
            this.ddlResourceType.Location = new System.Drawing.Point(399, 197);
            this.ddlResourceType.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ddlResourceType.Name = "ddlResourceType";
            this.ddlResourceType.Size = new System.Drawing.Size(207, 24);
            this.ddlResourceType.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 166);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(110, 17);
            this.label3.TabIndex = 6;
            this.label3.Text = "Cert Thumbprint";
            // 
            // txtResourceName
            // 
            this.txtResourceName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtResourceName.Location = new System.Drawing.Point(171, 197);
            this.txtResourceName.Margin = new System.Windows.Forms.Padding(4);
            this.txtResourceName.Name = "txtResourceName";
            this.txtResourceName.Size = new System.Drawing.Size(207, 23);
            this.txtResourceName.TabIndex = 5;
            this.txtResourceName.Text = "silverlining";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 36);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "SubscriptionId";
            // 
            // txtSubscriptionId
            // 
            this.txtSubscriptionId.BackColor = System.Drawing.Color.Yellow;
            this.txtSubscriptionId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSubscriptionId.Location = new System.Drawing.Point(171, 36);
            this.txtSubscriptionId.Margin = new System.Windows.Forms.Padding(4);
            this.txtSubscriptionId.Name = "txtSubscriptionId";
            this.txtSubscriptionId.Size = new System.Drawing.Size(207, 23);
            this.txtSubscriptionId.TabIndex = 0;
            this.txtSubscriptionId.UseSystemPasswordChar = true;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.Location = new System.Drawing.Point(-1, 408);
            this.label20.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(127, 17);
            this.label20.TabIndex = 48;
            this.label20.Text = "Dynamic Scaling";
            // 
            // btnChangeInstances
            // 
            this.btnChangeInstances.Location = new System.Drawing.Point(206, 505);
            this.btnChangeInstances.Margin = new System.Windows.Forms.Padding(4);
            this.btnChangeInstances.Name = "btnChangeInstances";
            this.btnChangeInstances.Size = new System.Drawing.Size(147, 28);
            this.btnChangeInstances.TabIndex = 48;
            this.btnChangeInstances.Text = "Change Instances";
            this.btnChangeInstances.UseVisualStyleBackColor = true;
            this.btnChangeInstances.Click += new System.EventHandler(this.btnChangeInstances_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.RoleName,
            this.Instances,
            this.NewInstanceCount});
            this.dataGridView1.Location = new System.Drawing.Point(2, 427);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(351, 75);
            this.dataGridView1.TabIndex = 52;
            // 
            // RoleName
            // 
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RoleName.DefaultCellStyle = dataGridViewCellStyle1;
            this.RoleName.HeaderText = "Role Name";
            this.RoleName.Name = "RoleName";
            this.RoleName.ReadOnly = true;
            this.RoleName.Width = 103;
            // 
            // Instances
            // 
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Instances.DefaultCellStyle = dataGridViewCellStyle2;
            this.Instances.HeaderText = "Instances";
            this.Instances.Name = "Instances";
            this.Instances.ReadOnly = true;
            this.Instances.Width = 93;
            // 
            // NewInstanceCount
            // 
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.Yellow;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.Format = "N0";
            dataGridViewCellStyle3.NullValue = null;
            this.NewInstanceCount.DefaultCellStyle = dataGridViewCellStyle3;
            this.NewInstanceCount.HeaderText = "New Count";
            this.NewInstanceCount.Name = "NewInstanceCount";
            this.NewInstanceCount.ToolTipText = "Click in the cell to add new instance count";
            this.NewInstanceCount.Width = 101;
            // 
            // ddlStorage
            // 
            this.ddlStorage.BackColor = System.Drawing.SystemColors.Window;
            this.ddlStorage.FormattingEnabled = true;
            this.ddlStorage.Location = new System.Drawing.Point(146, 538);
            this.ddlStorage.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ddlStorage.Name = "ddlStorage";
            this.ddlStorage.Size = new System.Drawing.Size(207, 24);
            this.ddlStorage.TabIndex = 48;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(8, 535);
            this.label21.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(118, 17);
            this.label21.TabIndex = 48;
            this.label21.Text = "Perf data storage";
            // 
            // txtAvgCpu
            // 
            this.txtAvgCpu.BackColor = System.Drawing.SystemColors.Window;
            this.txtAvgCpu.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAvgCpu.Location = new System.Drawing.Point(137, 599);
            this.txtAvgCpu.Margin = new System.Windows.Forms.Padding(4);
            this.txtAvgCpu.Name = "txtAvgCpu";
            this.txtAvgCpu.ReadOnly = true;
            this.txtAvgCpu.Size = new System.Drawing.Size(48, 23);
            this.txtAvgCpu.TabIndex = 48;
            // 
            // txtAvgMemory
            // 
            this.txtAvgMemory.BackColor = System.Drawing.SystemColors.Window;
            this.txtAvgMemory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAvgMemory.Location = new System.Drawing.Point(297, 599);
            this.txtAvgMemory.Margin = new System.Windows.Forms.Padding(4);
            this.txtAvgMemory.Name = "txtAvgMemory";
            this.txtAvgMemory.ReadOnly = true;
            this.txtAvgMemory.Size = new System.Drawing.Size(48, 23);
            this.txtAvgMemory.TabIndex = 53;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(8, 601);
            this.label22.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(121, 17);
            this.label22.TabIndex = 54;
            this.label22.Text = "%Processor (avg)";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(193, 601);
            this.label23.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(88, 17);
            this.label23.TabIndex = 55;
            this.label23.Text = "Memory(MB)";
            // 
            // btnGetPerfData
            // 
            this.btnGetPerfData.Location = new System.Drawing.Point(206, 563);
            this.btnGetPerfData.Margin = new System.Windows.Forms.Padding(4);
            this.btnGetPerfData.Name = "btnGetPerfData";
            this.btnGetPerfData.Size = new System.Drawing.Size(147, 28);
            this.btnGetPerfData.TabIndex = 56;
            this.btnGetPerfData.Text = "Get Perf Data";
            this.btnGetPerfData.UseVisualStyleBackColor = true;
            this.btnGetPerfData.Click += new System.EventHandler(this.btnGetPerfData_Click);
            // 
            // ServiceManagementForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Azure;
            this.ClientSize = new System.Drawing.Size(973, 862);
            this.Controls.Add(this.btnGetPerfData);
            this.Controls.Add(this.label23);
            this.Controls.Add(this.label22);
            this.Controls.Add(this.txtAvgMemory);
            this.Controls.Add(this.txtAvgCpu);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.ddlStorage);
            this.Controls.Add(this.btnChangeInstances);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lstOperations);
            this.Controls.Add(this.txtLog);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ServiceManagementForm";
            this.Text = "Windows Azure Service Management API";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Button btnLoadCert;
        private System.Windows.Forms.ListBox lstOperations;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSubscriptionId;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtResourceName;
        private System.Windows.Forms.ComboBox ddlResourceType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtOpId;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox ddlKeyType;
        private System.Windows.Forms.ComboBox ddlSlotType;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtDeploymentName;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtDeploymentLabel;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtPackageUri;
        private System.Windows.Forms.Button btnBrowseConfigFile;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtConfigFilePath;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtStagingDeploymentName;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox ddlDeploymentStatus;
        private System.Windows.Forms.TextBox txtDomainNumber;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ComboBox ddlMode;
        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.TextBox txtThumbprint;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Button btnBrowseServiceCertificate;
        private System.Windows.Forms.TextBox txtServiceCertificate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txtCertPassword;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox txtServiceCertAlgo;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox txtServiceCertThumbprint;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Button btnChangeInstances;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn RoleName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Instances;
        private System.Windows.Forms.DataGridViewTextBoxColumn NewInstanceCount;
        private System.Windows.Forms.ComboBox ddlStorage;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TextBox txtAvgCpu;
        private System.Windows.Forms.TextBox txtAvgMemory;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Button btnGetPerfData;
        private System.Windows.Forms.Label lblDeploymentId;
        private System.Windows.Forms.TextBox txtHostedServiceLabel;
        private System.Windows.Forms.TextBox txtHostedServiceName;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.ComboBox ddlLocations;
        private System.Windows.Forms.ComboBox ddlAffinityGroups;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Button btnGuidName;
    }
}

