namespace DemResGateway
{
 partial class Gateway
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
   this.btnSendkWhValue = new System.Windows.Forms.Button();
   this.txtGatewayId = new System.Windows.Forms.TextBox();
   this.label1 = new System.Windows.Forms.Label();
   this.lblkWh = new System.Windows.Forms.Label();
   this.SuspendLayout();
   // 
   // btnSendkWhValue
   // 
   this.btnSendkWhValue.Location = new System.Drawing.Point(100, 36);
   this.btnSendkWhValue.Name = "btnSendkWhValue";
   this.btnSendkWhValue.Size = new System.Drawing.Size(97, 23);
   this.btnSendkWhValue.TabIndex = 0;
   this.btnSendkWhValue.Text = "Send kWh Value";
   this.btnSendkWhValue.UseVisualStyleBackColor = true;
   this.btnSendkWhValue.Click += new System.EventHandler(this.btnSendkWhValue_Click);
   // 
   // txtGatewayId
   // 
   this.txtGatewayId.Location = new System.Drawing.Point(100, 13);
   this.txtGatewayId.Name = "txtGatewayId";
   this.txtGatewayId.Size = new System.Drawing.Size(97, 20);
   this.txtGatewayId.TabIndex = 1;
   this.txtGatewayId.Text = "MyGateway10001";
   // 
   // label1
   // 
   this.label1.AutoSize = true;
   this.label1.Location = new System.Drawing.Point(29, 13);
   this.label1.Name = "label1";
   this.label1.Size = new System.Drawing.Size(58, 13);
   this.label1.TabIndex = 2;
   this.label1.Text = "GatewayId";
   // 
   // lblkWh
   // 
   this.lblkWh.AutoSize = true;
   this.lblkWh.Location = new System.Drawing.Point(203, 36);
   this.lblkWh.Name = "lblkWh";
   this.lblkWh.Size = new System.Drawing.Size(0, 13);
   this.lblkWh.TabIndex = 3;
   // 
   // Gateway
   // 
   this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
   this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
   this.ClientSize = new System.Drawing.Size(317, 71);
   this.Controls.Add(this.lblkWh);
   this.Controls.Add(this.label1);
   this.Controls.Add(this.txtGatewayId);
   this.Controls.Add(this.btnSendkWhValue);
   this.Name = "Gateway";
   this.Text = "Demand-Response Gateway";
   this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Gateway_FormClosing);
   this.ResumeLayout(false);
   this.PerformLayout();

  }

  #endregion

  private System.Windows.Forms.Button btnSendkWhValue;
  private System.Windows.Forms.TextBox txtGatewayId;
  private System.Windows.Forms.Label label1;
  private System.Windows.Forms.Label lblkWh;
 }
}

