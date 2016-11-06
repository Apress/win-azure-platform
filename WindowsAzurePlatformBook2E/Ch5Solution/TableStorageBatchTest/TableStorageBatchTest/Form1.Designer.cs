namespace TableStorageBatchTest
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
            this.txtStorageAcc = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtNumberEntities = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnDeleteTable = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtStorageAcc
            // 
            this.txtStorageAcc.Location = new System.Drawing.Point(119, 12);
            this.txtStorageAcc.Name = "txtStorageAcc";
            this.txtStorageAcc.Size = new System.Drawing.Size(488, 20);
            this.txtStorageAcc.TabIndex = 0;
            this.txtStorageAcc.Text = "UseDevelopmentStorage=true";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(-1, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "StorageAccount String";
            // 
            // txtNumberEntities
            // 
            this.txtNumberEntities.Location = new System.Drawing.Point(119, 39);
            this.txtNumberEntities.Name = "txtNumberEntities";
            this.txtNumberEntities.Size = new System.Drawing.Size(100, 20);
            this.txtNumberEntities.TabIndex = 2;
            this.txtNumberEntities.Text = "100";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(-1, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Batch Entity Count";
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(119, 66);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 4;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnDeleteTable
            // 
            this.btnDeleteTable.Location = new System.Drawing.Point(200, 66);
            this.btnDeleteTable.Name = "btnDeleteTable";
            this.btnDeleteTable.Size = new System.Drawing.Size(101, 23);
            this.btnDeleteTable.TabIndex = 5;
            this.btnDeleteTable.Text = "Delete Table";
            this.btnDeleteTable.UseVisualStyleBackColor = true;
            this.btnDeleteTable.Click += new System.EventHandler(this.btnDeleteTable_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(619, 124);
            this.Controls.Add(this.btnDeleteTable);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtNumberEntities);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtStorageAcc);
            this.Name = "Form1";
            this.Text = "Table Storage Batch Update Test";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtStorageAcc;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtNumberEntities;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnDeleteTable;
    }
}

