namespace EventPoint_GenerateData
{
    partial class Form1
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
            this.btnSendBulk = new System.Windows.Forms.Button();
            this.txtNumberOfMessages = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.btnSendCritical = new System.Windows.Forms.Button();
            this.lblSent = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnSendBulk
            // 
            this.btnSendBulk.Location = new System.Drawing.Point(13, 35);
            this.btnSendBulk.Name = "btnSendBulk";
            this.btnSendBulk.Size = new System.Drawing.Size(75, 23);
            this.btnSendBulk.TabIndex = 0;
            this.btnSendBulk.Text = "Send Bulk";
            this.btnSendBulk.UseVisualStyleBackColor = true;
            this.btnSendBulk.Click += new System.EventHandler(this.btnSendBulk_Click);
            // 
            // txtNumberOfMessages
            // 
            this.txtNumberOfMessages.Location = new System.Drawing.Point(218, 38);
            this.txtNumberOfMessages.Name = "txtNumberOfMessages";
            this.txtNumberOfMessages.Size = new System.Drawing.Size(43, 20);
            this.txtNumberOfMessages.TabIndex = 1;
            this.txtNumberOfMessages.Text = "5";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(104, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Number of Messages:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 138);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Status";
            // 
            // txtStatus
            // 
            this.txtStatus.Location = new System.Drawing.Point(21, 154);
            this.txtStatus.Multiline = true;
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.ReadOnly = true;
            this.txtStatus.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtStatus.Size = new System.Drawing.Size(290, 229);
            this.txtStatus.TabIndex = 4;
            this.txtStatus.Text = "(none)";
            // 
            // btnSendCritical
            // 
            this.btnSendCritical.Location = new System.Drawing.Point(13, 82);
            this.btnSendCritical.Name = "btnSendCritical";
            this.btnSendCritical.Size = new System.Drawing.Size(75, 23);
            this.btnSendCritical.TabIndex = 6;
            this.btnSendCritical.Text = "Send Critical";
            this.btnSendCritical.UseVisualStyleBackColor = true;
            this.btnSendCritical.Click += new System.EventHandler(this.btnSendCritical_Click);
            // 
            // lblSent
            // 
            this.lblSent.AutoSize = true;
            this.lblSent.Location = new System.Drawing.Point(226, 92);
            this.lblSent.Name = "lblSent";
            this.lblSent.Size = new System.Drawing.Size(0, 13);
            this.lblSent.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(104, 92);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Sent:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(605, 395);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblSent);
            this.Controls.Add(this.btnSendCritical);
            this.Controls.Add(this.txtStatus);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtNumberOfMessages);
            this.Controls.Add(this.btnSendBulk);
            this.Name = "Form1";
            this.Text = "EventPoint Message Generator";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSendBulk;
        private System.Windows.Forms.TextBox txtNumberOfMessages;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtStatus;
        private System.Windows.Forms.Button btnSendCritical;
        private System.Windows.Forms.Label lblSent;
        private System.Windows.Forms.Label label4;
    }
}

