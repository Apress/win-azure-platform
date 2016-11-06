using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.ServiceBus;
using System.ServiceModel;
using ProAzureDemResContract;

namespace DemResGateway
{
 [CallbackBehavior(UseSynchronizationContext = false)]
 public partial class Gateway : Form, IDemResCallback
 {
  GatewayClient client;

  public Gateway()
  {
   InitializeComponent();
   client = new GatewayClient(this);
   
  }

  private void btnSendkWhValue_Click(object sender, EventArgs e)
  {
   try
   {
    Random r = new Random();
    double kWh = r.NextDouble() * 0.6D;
    lblkWh.Text = kWh.ToString();
    client.SendValue(txtGatewayId.Text, kWh, DateTime.Now);
   }
   catch (Exception ex)
   {
    MessageBox.Show(ex.Message);

   }
   
  }

  public void Curtail(int setPointValue)
  {
   MessageBox.Show("New SetPoint Value sent by server:" + setPointValue.ToString(), "Callback Message", MessageBoxButtons.OK);

  }
  private void Gateway_FormClosing(object sender, FormClosingEventArgs e)
  {
   client.Close();
  }
 }
}
