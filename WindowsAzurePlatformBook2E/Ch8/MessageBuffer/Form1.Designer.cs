namespace MessageBuffer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.txtLog = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtIssuerName = new System.Windows.Forms.TextBox();
            this.txtIssuerKey = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtServiceNamespace = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnCreateBuffer = new System.Windows.Forms.Button();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtMessageBuffer = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnSendMessages = new System.Windows.Forms.Button();
            this.btnRetrieve = new System.Windows.Forms.Button();
            this.btnPeekAndRetrieve = new System.Windows.Forms.Button();
            this.btnDeleteBuffer = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtLog
            // 
            this.txtLog.AcceptsReturn = true;
            this.txtLog.BackColor = System.Drawing.Color.White;
            this.txtLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLog.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtLog.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLog.ForeColor = System.Drawing.Color.DarkRed;
            this.txtLog.Location = new System.Drawing.Point(0, 211);
            this.txtLog.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(747, 316);
            this.txtLog.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 16);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 17);
            this.label1.TabIndex = 7;
            this.label1.Text = "Issuer Name";
            // 
            // txtIssuerName
            // 
            this.txtIssuerName.Location = new System.Drawing.Point(169, 16);
            this.txtIssuerName.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtIssuerName.Name = "txtIssuerName";
            this.txtIssuerName.Size = new System.Drawing.Size(225, 23);
            this.txtIssuerName.TabIndex = 8;
            this.txtIssuerName.Text = "owner";
            // 
            // txtIssuerKey
            // 
            this.txtIssuerKey.Location = new System.Drawing.Point(169, 48);
            this.txtIssuerKey.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtIssuerKey.Name = "txtIssuerKey";
            this.txtIssuerKey.Size = new System.Drawing.Size(225, 23);
            this.txtIssuerKey.TabIndex = 9;
            this.txtIssuerKey.Text = "wJBJaobUmarWn6kqv7QpaaRh3ttNVr3w1OjiotVEOL4=";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 48);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 17);
            this.label2.TabIndex = 10;
            this.label2.Text = "Issuer Key";
            // 
            // txtServiceNamespace
            // 
            this.txtServiceNamespace.Location = new System.Drawing.Point(169, 80);
            this.txtServiceNamespace.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtServiceNamespace.Name = "txtServiceNamespace";
            this.txtServiceNamespace.Size = new System.Drawing.Size(225, 23);
            this.txtServiceNamespace.TabIndex = 11;
            this.txtServiceNamespace.Text = "proazure-1";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 80);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(134, 17);
            this.label3.TabIndex = 12;
            this.label3.Text = "Service Namespace";
            // 
            // btnCreateBuffer
            // 
            this.btnCreateBuffer.Location = new System.Drawing.Point(475, 16);
            this.btnCreateBuffer.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCreateBuffer.Name = "btnCreateBuffer";
            this.btnCreateBuffer.Size = new System.Drawing.Size(156, 28);
            this.btnCreateBuffer.TabIndex = 13;
            this.btnCreateBuffer.Text = "Create Buffer";
            this.btnCreateBuffer.UseVisualStyleBackColor = true;
            this.btnCreateBuffer.Click += new System.EventHandler(this.btnCreateBuffer_Click);
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(169, 144);
            this.txtMessage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(225, 23);
            this.txtMessage.TabIndex = 14;
            this.txtMessage.Text = "proazure-1";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 112);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(148, 17);
            this.label4.TabIndex = 15;
            this.label4.Text = "Message Buffer Name";
            // 
            // txtMessageBuffer
            // 
            this.txtMessageBuffer.Location = new System.Drawing.Point(169, 112);
            this.txtMessageBuffer.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtMessageBuffer.Name = "txtMessageBuffer";
            this.txtMessageBuffer.Size = new System.Drawing.Size(225, 23);
            this.txtMessageBuffer.TabIndex = 16;
            this.txtMessageBuffer.Text = "proazurebuffer";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 144);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(116, 17);
            this.label5.TabIndex = 17;
            this.label5.Text = "Message to send";
            // 
            // btnSendMessages
            // 
            this.btnSendMessages.Location = new System.Drawing.Point(475, 52);
            this.btnSendMessages.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSendMessages.Name = "btnSendMessages";
            this.btnSendMessages.Size = new System.Drawing.Size(156, 28);
            this.btnSendMessages.TabIndex = 18;
            this.btnSendMessages.Text = "Send Messages";
            this.btnSendMessages.UseVisualStyleBackColor = true;
            this.btnSendMessages.Click += new System.EventHandler(this.btnSendMessages_Click);
            // 
            // btnRetrieve
            // 
            this.btnRetrieve.Location = new System.Drawing.Point(475, 87);
            this.btnRetrieve.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnRetrieve.Name = "btnRetrieve";
            this.btnRetrieve.Size = new System.Drawing.Size(156, 28);
            this.btnRetrieve.TabIndex = 19;
            this.btnRetrieve.Text = "Retrieve ";
            this.btnRetrieve.UseVisualStyleBackColor = true;
            this.btnRetrieve.Click += new System.EventHandler(this.btnRetrieve_Click);
            // 
            // btnPeekAndRetrieve
            // 
            this.btnPeekAndRetrieve.Location = new System.Drawing.Point(475, 123);
            this.btnPeekAndRetrieve.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnPeekAndRetrieve.Name = "btnPeekAndRetrieve";
            this.btnPeekAndRetrieve.Size = new System.Drawing.Size(156, 28);
            this.btnPeekAndRetrieve.TabIndex = 20;
            this.btnPeekAndRetrieve.Text = "Peek and Retrieve";
            this.btnPeekAndRetrieve.UseVisualStyleBackColor = true;
            this.btnPeekAndRetrieve.Click += new System.EventHandler(this.btnPeekAndRetrieve_Click);
            // 
            // btnDeleteBuffer
            // 
            this.btnDeleteBuffer.Location = new System.Drawing.Point(475, 159);
            this.btnDeleteBuffer.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnDeleteBuffer.Name = "btnDeleteBuffer";
            this.btnDeleteBuffer.Size = new System.Drawing.Size(156, 28);
            this.btnDeleteBuffer.TabIndex = 21;
            this.btnDeleteBuffer.Text = "Delete Buffer";
            this.btnDeleteBuffer.UseVisualStyleBackColor = true;
            this.btnDeleteBuffer.Click += new System.EventHandler(this.btnDeleteBuffer_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(747, 527);
            this.Controls.Add(this.btnDeleteBuffer);
            this.Controls.Add(this.btnPeekAndRetrieve);
            this.Controls.Add(this.btnRetrieve);
            this.Controls.Add(this.btnSendMessages);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtMessageBuffer);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.btnCreateBuffer);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtServiceNamespace);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtIssuerKey);
            this.Controls.Add(this.txtIssuerName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtLog);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form1";
            this.Text = "Message Buffer Test";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtIssuerName;
        private System.Windows.Forms.TextBox txtIssuerKey;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtServiceNamespace;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnCreateBuffer;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtMessageBuffer;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnSendMessages;
        private System.Windows.Forms.Button btnRetrieve;
        private System.Windows.Forms.Button btnPeekAndRetrieve;
        private System.Windows.Forms.Button btnDeleteBuffer;
    }
}

