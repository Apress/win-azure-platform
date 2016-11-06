namespace EventPoint.Monitor
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
            this.txtAlerts = new System.Windows.Forms.TextBox();
            this.lblLastEvent = new System.Windows.Forms.Label();
            this.btnListen = new System.Windows.Forms.Button();
            this.lblLastCheckedTime = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtAlerts
            // 
            this.txtAlerts.Location = new System.Drawing.Point(12, 92);
            this.txtAlerts.Multiline = true;
            this.txtAlerts.Name = "txtAlerts";
            this.txtAlerts.Size = new System.Drawing.Size(473, 313);
            this.txtAlerts.TabIndex = 0;
            // 
            // lblLastEvent
            // 
            this.lblLastEvent.AutoSize = true;
            this.lblLastEvent.Location = new System.Drawing.Point(264, 68);
            this.lblLastEvent.Name = "lblLastEvent";
            this.lblLastEvent.Size = new System.Drawing.Size(60, 13);
            this.lblLastEvent.TabIndex = 1;
            this.lblLastEvent.Text = "Last event:";
            // 
            // btnListen
            // 
            this.btnListen.Location = new System.Drawing.Point(13, 63);
            this.btnListen.Name = "btnListen";
            this.btnListen.Size = new System.Drawing.Size(75, 23);
            this.btnListen.TabIndex = 3;
            this.btnListen.Text = "Starting...";
            this.btnListen.UseVisualStyleBackColor = true;
            this.btnListen.Click += new System.EventHandler(this.btnListen_Click);
            // 
            // lblLastCheckedTime
            // 
            this.lblLastCheckedTime.AutoSize = true;
            this.lblLastCheckedTime.Location = new System.Drawing.Point(330, 68);
            this.lblLastCheckedTime.Name = "lblLastCheckedTime";
            this.lblLastCheckedTime.Size = new System.Drawing.Size(40, 13);
            this.lblLastCheckedTime.TabIndex = 4;
            this.lblLastCheckedTime.Text = "(never)";
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(13, 12);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(472, 40);
            this.textBox1.TabIndex = 5;
            this.textBox1.Text = "This is an on-prem Winforms application that hosts a WCF endpoint and receives cr" +
                "itical alerts multi-cast through the ServiceBus";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(497, 425);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.lblLastCheckedTime);
            this.Controls.Add(this.btnListen);
            this.Controls.Add(this.lblLastEvent);
            this.Controls.Add(this.txtAlerts);
            this.Name = "Form1";
            this.Text = "Critical Event Monitor";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtAlerts;
        private System.Windows.Forms.Label lblLastEvent;
        private System.Windows.Forms.Button btnListen;
        private System.Windows.Forms.Label lblLastCheckedTime;
        private System.Windows.Forms.TextBox textBox1;
    }
}

