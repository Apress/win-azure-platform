namespace NetOnewayRelayClient
{
    partial class EnergyManagementDevice
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EnergyManagementDevice));
            this.cbAutoSend = new System.Windows.Forms.CheckBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tslbl = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.tsSolutionName = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.tsSolutionPassword = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.tsDeviceId = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel5 = new System.Windows.Forms.ToolStripLabel();
            this.tsServicePath = new System.Windows.Forms.ToolStripTextBox();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel4 = new System.Windows.Forms.ToolStripLabel();
            this.tsSolutionToConnect = new System.Windows.Forms.ToolStripTextBox();
            this.tsConnect = new System.Windows.Forms.ToolStripButton();
            this.txtSetPoint = new System.Windows.Forms.TextBox();
            this.txtCurrentTemperature = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnHeatCool = new System.Windows.Forms.Button();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnHVAC = new System.Windows.Forms.Button();
            this.btnKwh = new System.Windows.Forms.Button();
            this.btnLightBulb = new System.Windows.Forms.Button();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.button1 = new System.Windows.Forms.Button();
            this.txtOff = new System.Windows.Forms.TextBox();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // cbAutoSend
            // 
            this.cbAutoSend.AutoSize = true;
            this.cbAutoSend.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbAutoSend.Location = new System.Drawing.Point(576, 372);
            this.cbAutoSend.Name = "cbAutoSend";
            this.cbAutoSend.Size = new System.Drawing.Size(190, 21);
            this.cbAutoSend.TabIndex = 2;
            this.cbAutoSend.Text = "Start sending KWh values";
            this.cbAutoSend.UseVisualStyleBackColor = true;
            this.cbAutoSend.CheckedChanged += new System.EventHandler(this.cbAutoSend_CheckedChanged);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tslbl});
            this.statusStrip1.Location = new System.Drawing.Point(0, 693);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(843, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tslbl
            // 
            this.tslbl.BackColor = System.Drawing.Color.Azure;
            this.tslbl.Name = "tslbl";
            this.tslbl.Size = new System.Drawing.Size(828, 17);
            this.tslbl.Spring = true;
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Aquamarine;
            this.toolStrip1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.tsSolutionName,
            this.toolStripLabel3,
            this.tsSolutionPassword,
            this.toolStripLabel2,
            this.tsDeviceId,
            this.toolStripLabel5,
            this.tsServicePath});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(843, 25);
            this.toolStrip1.TabIndex = 4;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(69, 22);
            this.toolStripLabel1.Text = "IssuerName";
            // 
            // tsSolutionName
            // 
            this.tsSolutionName.BackColor = System.Drawing.Color.White;
            this.tsSolutionName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tsSolutionName.Name = "tsSolutionName";
            this.tsSolutionName.Size = new System.Drawing.Size(100, 25);
            this.tsSolutionName.Text = "proazure";
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(56, 22);
            this.toolStripLabel3.Text = "IssuerKey";
            // 
            // tsSolutionPassword
            // 
            this.tsSolutionPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tsSolutionPassword.Name = "tsSolutionPassword";
            this.tsSolutionPassword.Size = new System.Drawing.Size(100, 25);
            this.tsSolutionPassword.Text = "proazure";
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(62, 22);
            this.toolStripLabel2.Text = "GatewayId";
            // 
            // tsDeviceId
            // 
            this.tsDeviceId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tsDeviceId.Name = "tsDeviceId";
            this.tsDeviceId.Size = new System.Drawing.Size(100, 25);
            this.tsDeviceId.Text = "MyHome";
            // 
            // toolStripLabel5
            // 
            this.toolStripLabel5.Name = "toolStripLabel5";
            this.toolStripLabel5.Size = new System.Drawing.Size(71, 22);
            this.toolStripLabel5.Text = "Service Path";
            // 
            // tsServicePath
            // 
            this.tsServicePath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tsServicePath.Name = "tsServicePath";
            this.tsServicePath.Size = new System.Drawing.Size(200, 25);
            this.tsServicePath.Text = "OnewayEnergyServiceOperations";
            // 
            // txtLog
            // 
            this.txtLog.AcceptsReturn = true;
            this.txtLog.BackColor = System.Drawing.Color.White;
            this.txtLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLog.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtLog.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLog.ForeColor = System.Drawing.Color.DarkRed;
            this.txtLog.Location = new System.Drawing.Point(0, 434);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(843, 259);
            this.txtLog.TabIndex = 5;
            this.txtLog.Text = "Messages";
            // 
            // toolStrip2
            // 
            this.toolStrip2.BackColor = System.Drawing.Color.Aquamarine;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel4,
            this.tsSolutionToConnect,
            this.tsConnect});
            this.toolStrip2.Location = new System.Drawing.Point(0, 25);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(843, 25);
            this.toolStrip2.TabIndex = 6;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // toolStripLabel4
            // 
            this.toolStripLabel4.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.toolStripLabel4.Name = "toolStripLabel4";
            this.toolStripLabel4.Size = new System.Drawing.Size(86, 22);
            this.toolStripLabel4.Text = "Solution Name";
            // 
            // tsSolutionToConnect
            // 
            this.tsSolutionToConnect.BackColor = System.Drawing.Color.White;
            this.tsSolutionToConnect.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tsSolutionToConnect.Name = "tsSolutionToConnect";
            this.tsSolutionToConnect.Size = new System.Drawing.Size(100, 25);
            this.tsSolutionToConnect.Text = "proazure-1";
            // 
            // tsConnect
            // 
            this.tsConnect.BackColor = System.Drawing.SystemColors.MenuBar;
            this.tsConnect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsConnect.Font = new System.Drawing.Font("Segoe UI", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            this.tsConnect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsConnect.Name = "tsConnect";
            this.tsConnect.Size = new System.Drawing.Size(57, 22);
            this.tsConnect.Text = "Connect";
            this.tsConnect.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.tsConnect.ToolTipText = "Connect to Service Bus";
            this.tsConnect.Click += new System.EventHandler(this.tsConnect_Click);
            // 
            // txtSetPoint
            // 
            this.txtSetPoint.BackColor = System.Drawing.Color.LightGray;
            this.txtSetPoint.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSetPoint.Font = new System.Drawing.Font("Stencil", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSetPoint.ForeColor = System.Drawing.Color.Black;
            this.txtSetPoint.Location = new System.Drawing.Point(119, 352);
            this.txtSetPoint.Name = "txtSetPoint";
            this.txtSetPoint.ReadOnly = true;
            this.txtSetPoint.Size = new System.Drawing.Size(36, 36);
            this.txtSetPoint.TabIndex = 12;
            this.txtSetPoint.Text = "55";
            // 
            // txtCurrentTemperature
            // 
            this.txtCurrentTemperature.BackColor = System.Drawing.Color.Gainsboro;
            this.txtCurrentTemperature.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCurrentTemperature.Font = new System.Drawing.Font("Stencil", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCurrentTemperature.ForeColor = System.Drawing.Color.Black;
            this.txtCurrentTemperature.Location = new System.Drawing.Point(237, 236);
            this.txtCurrentTemperature.Name = "txtCurrentTemperature";
            this.txtCurrentTemperature.Size = new System.Drawing.Size(27, 30);
            this.txtCurrentTemperature.TabIndex = 14;
            this.txtCurrentTemperature.Text = "55";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(155, 273);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 19;
            // 
            // toolTip1
            // 
            this.toolTip1.ToolTipTitle = "Set Point Value";
            // 
            // btnHeatCool
            // 
            this.btnHeatCool.Image = ((System.Drawing.Image)(resources.GetObject("btnHeatCool.Image")));
            this.btnHeatCool.Location = new System.Drawing.Point(224, 267);
            this.btnHeatCool.Name = "btnHeatCool";
            this.btnHeatCool.Size = new System.Drawing.Size(41, 36);
            this.btnHeatCool.TabIndex = 21;
            this.btnHeatCool.UseVisualStyleBackColor = true;
            // 
            // btnDown
            // 
            this.btnDown.Image = ((System.Drawing.Image)(resources.GetObject("btnDown.Image")));
            this.btnDown.Location = new System.Drawing.Point(68, 352);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(37, 36);
            this.btnDown.TabIndex = 18;
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnUp
            // 
            this.btnUp.Image = ((System.Drawing.Image)(resources.GetObject("btnUp.Image")));
            this.btnUp.Location = new System.Drawing.Point(25, 352);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(37, 36);
            this.btnUp.TabIndex = 16;
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnHVAC
            // 
            this.btnHVAC.BackColor = System.Drawing.Color.White;
            this.btnHVAC.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnHVAC.Image = ((System.Drawing.Image)(resources.GetObject("btnHVAC.Image")));
            this.btnHVAC.Location = new System.Drawing.Point(26, 116);
            this.btnHVAC.Name = "btnHVAC";
            this.btnHVAC.Size = new System.Drawing.Size(238, 150);
            this.btnHVAC.TabIndex = 7;
            this.btnHVAC.UseVisualStyleBackColor = false;
            // 
            // btnKwh
            // 
            this.btnKwh.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnKwh.Image = global::NetOnewayRelayClient.Properties.Resources.kwh;
            this.btnKwh.Location = new System.Drawing.Point(576, 116);
            this.btnKwh.Name = "btnKwh";
            this.btnKwh.Size = new System.Drawing.Size(238, 250);
            this.btnKwh.TabIndex = 1;
            this.btnKwh.UseVisualStyleBackColor = true;
            this.btnKwh.Click += new System.EventHandler(this.btnKwh_Click);
            // 
            // btnLightBulb
            // 
            this.btnLightBulb.BackColor = System.Drawing.Color.White;
            this.btnLightBulb.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnLightBulb.Image = global::NetOnewayRelayClient.Properties.Resources.off;
            this.btnLightBulb.Location = new System.Drawing.Point(301, 116);
            this.btnLightBulb.Name = "btnLightBulb";
            this.btnLightBulb.Size = new System.Drawing.Size(238, 250);
            this.btnLightBulb.TabIndex = 0;
            this.btnLightBulb.UseVisualStyleBackColor = false;
            this.btnLightBulb.Click += new System.EventHandler(this.btnLightBulb_Click);
            // 
            // trackBar1
            // 
            this.trackBar1.LargeChange = 1;
            this.trackBar1.Location = new System.Drawing.Point(26, 301);
            this.trackBar1.Maximum = 2;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(239, 45);
            this.trackBar1.TabIndex = 23;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // button1
            // 
            this.button1.Image = ((System.Drawing.Image)(resources.GetObject("button1.Image")));
            this.button1.Location = new System.Drawing.Point(122, 267);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(41, 36);
            this.button1.TabIndex = 24;
            this.button1.UseVisualStyleBackColor = true;
            // 
            // txtOff
            // 
            this.txtOff.BackColor = System.Drawing.Color.LightGray;
            this.txtOff.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtOff.Font = new System.Drawing.Font("Stencil", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOff.ForeColor = System.Drawing.Color.Black;
            this.txtOff.Location = new System.Drawing.Point(26, 267);
            this.txtOff.Name = "txtOff";
            this.txtOff.ReadOnly = true;
            this.txtOff.Size = new System.Drawing.Size(36, 28);
            this.txtOff.TabIndex = 25;
            this.txtOff.Text = "OFF";
            // 
            // EnergyManagementDevice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Azure;
            this.ClientSize = new System.Drawing.Size(843, 715);
            this.Controls.Add(this.txtOff);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnHeatCool);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnDown);
            this.Controls.Add(this.btnUp);
            this.Controls.Add(this.txtCurrentTemperature);
            this.Controls.Add(this.txtSetPoint);
            this.Controls.Add(this.btnHVAC);
            this.Controls.Add(this.toolStrip2);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.cbAutoSend);
            this.Controls.Add(this.btnKwh);
            this.Controls.Add(this.btnLightBulb);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "EnergyManagementDevice";
            this.Text = "ProAzure Energy Management";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EnergyManagementDevice_FormClosing);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnLightBulb;
        private System.Windows.Forms.Button btnKwh;
        private System.Windows.Forms.CheckBox cbAutoSend;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tslbl;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripTextBox tsSolutionName;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripTextBox tsDeviceId;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripTextBox tsSolutionPassword;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel4;
        private System.Windows.Forms.ToolStripTextBox tsSolutionToConnect;
        private System.Windows.Forms.Button btnHVAC;
        private System.Windows.Forms.TextBox txtSetPoint;
        private System.Windows.Forms.TextBox txtCurrentTemperature;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btnHeatCool;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ToolStripButton tsConnect;
        private System.Windows.Forms.TextBox txtOff;
        private System.Windows.Forms.ToolStripLabel toolStripLabel5;
        private System.Windows.Forms.ToolStripTextBox tsServicePath;
    }
}

