namespace ProAzureDemResDbApp
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
      this.txtServerName = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.txtResult = new System.Windows.Forms.TextBox();
      this.txtUserName = new System.Windows.Forms.TextBox();
      this.txtPassword = new System.Windows.Forms.TextBox();
      this.txtDatabase = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.txtDomainName = new System.Windows.Forms.TextBox();
      this.btnCreateTable = new System.Windows.Forms.Button();
      this.btnDropTable = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // txtServerName
      // 
      this.txtServerName.Location = new System.Drawing.Point(114, 13);
      this.txtServerName.Name = "txtServerName";
      this.txtServerName.Size = new System.Drawing.Size(147, 20);
      this.txtServerName.TabIndex = 0;
      this.txtServerName.Text = "vej8xaf2av";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(13, 13);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(69, 13);
      this.label1.TabIndex = 1;
      this.label1.Text = "Server Name";
      // 
      // txtResult
      // 
      this.txtResult.AcceptsReturn = true;
      this.txtResult.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.txtResult.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.txtResult.ForeColor = System.Drawing.Color.Maroon;
      this.txtResult.Location = new System.Drawing.Point(0, 195);
      this.txtResult.Multiline = true;
      this.txtResult.Name = "txtResult";
      this.txtResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.txtResult.Size = new System.Drawing.Size(500, 171);
      this.txtResult.TabIndex = 3;
      // 
      // txtUserName
      // 
      this.txtUserName.Location = new System.Drawing.Point(114, 39);
      this.txtUserName.Name = "txtUserName";
      this.txtUserName.Size = new System.Drawing.Size(147, 20);
      this.txtUserName.TabIndex = 4;
      this.txtUserName.Text = "proazureadmin";
      // 
      // txtPassword
      // 
      this.txtPassword.Location = new System.Drawing.Point(114, 65);
      this.txtPassword.Name = "txtPassword";
      this.txtPassword.Size = new System.Drawing.Size(147, 20);
      this.txtPassword.TabIndex = 5;
      this.txtPassword.Text = "pass@word1";
      this.txtPassword.UseSystemPasswordChar = true;
      // 
      // txtDatabase
      // 
      this.txtDatabase.Location = new System.Drawing.Point(114, 91);
      this.txtDatabase.Name = "txtDatabase";
      this.txtDatabase.Size = new System.Drawing.Size(147, 20);
      this.txtDatabase.TabIndex = 6;
      this.txtDatabase.Text = "proazuredemres";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(13, 39);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(60, 13);
      this.label2.TabIndex = 7;
      this.label2.Text = "User Name";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(13, 65);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(53, 13);
      this.label3.TabIndex = 8;
      this.label3.Text = "Password";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(13, 91);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(84, 13);
      this.label4.TabIndex = 9;
      this.label4.Text = "Database Name";
      // 
      // txtDomainName
      // 
      this.txtDomainName.Location = new System.Drawing.Point(267, 13);
      this.txtDomainName.Name = "txtDomainName";
      this.txtDomainName.Size = new System.Drawing.Size(147, 20);
      this.txtDomainName.TabIndex = 10;
      this.txtDomainName.Text = "database.windows.net";
      // 
      // btnCreateTable
      // 
      this.btnCreateTable.Location = new System.Drawing.Point(114, 129);
      this.btnCreateTable.Name = "btnCreateTable";
      this.btnCreateTable.Size = new System.Drawing.Size(116, 23);
      this.btnCreateTable.TabIndex = 11;
      this.btnCreateTable.Text = "Create Sample Data";
      this.btnCreateTable.UseVisualStyleBackColor = true;
      this.btnCreateTable.Click += new System.EventHandler(this.btnCreateTable_Click);
      // 
      // btnDropTable
      // 
      this.btnDropTable.Location = new System.Drawing.Point(114, 158);
      this.btnDropTable.Name = "btnDropTable";
      this.btnDropTable.Size = new System.Drawing.Size(116, 23);
      this.btnDropTable.TabIndex = 13;
      this.btnDropTable.Text = "Drop Data";
      this.btnDropTable.UseVisualStyleBackColor = true;
      this.btnDropTable.Click += new System.EventHandler(this.btnDropTable_Click);
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(500, 366);
      this.Controls.Add(this.btnDropTable);
      this.Controls.Add(this.btnCreateTable);
      this.Controls.Add(this.txtDomainName);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.txtDatabase);
      this.Controls.Add(this.txtPassword);
      this.Controls.Add(this.txtUserName);
      this.Controls.Add(this.txtResult);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.txtServerName);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "Form1";
      this.Text = "Pro Azure Demand Response Sample Data";
      this.ResumeLayout(false);
      this.PerformLayout();

  }

  #endregion

  private System.Windows.Forms.TextBox txtServerName;
  private System.Windows.Forms.Label label1;
  private System.Windows.Forms.TextBox txtResult;
  private System.Windows.Forms.TextBox txtUserName;
  private System.Windows.Forms.TextBox txtPassword;
  private System.Windows.Forms.TextBox txtDatabase;
  private System.Windows.Forms.Label label2;
  private System.Windows.Forms.Label label3;
  private System.Windows.Forms.Label label4;
  private System.Windows.Forms.TextBox txtDomainName;
  private System.Windows.Forms.Button btnCreateTable;
  private System.Windows.Forms.Button btnDropTable;
 }
}

