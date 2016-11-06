using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceModel;
using Microsoft.ServiceBus;
using EventPoint.Common;
using Microsoft.ServiceBus.Messaging;
using Microsoft.Samples.ServiceBusMessaging;

namespace EventPoint.Monitor
{


    public partial class Form1 : Form
    {
        ServiceHost host;
        SubscriptionClient subscription;

        public Form1()
        {
            InitializeComponent();

            EventMessageFactory factory = new EventMessageFactory();
            factory.CreateSubscription("CriticalEventsMonitor", "0"); // 0 is critical priority per the data generator
            subscription = factory.CreateSubscriptionClient("CriticalEventsMonitor");
            btnListen.Text = "Don't Listen";
            subscription.StartListener(message => this.UpdateUI(message.GetBody<EventMessage>().Message));
        }

        public void UpdateUI(string msg)
        {
            this.txtAlerts.Text += String.Format("[{0}] {1}", DateTime.Now.ToLongTimeString(), msg) + Environment.NewLine;
            this.lblLastCheckedTime.Text = DateTime.Now.ToLongTimeString();
        }


       private void btnListen_Click(object sender, EventArgs e)
       {
           try
           {
               if (btnListen.Text == "Listen")
               {
                   subscription.StartListener(message => Publish(message));
                   btnListen.Text = "Don't Listen";
               }
               else
               {
                   subscription.StopListener();
                   btnListen.Text = "Listen";
               }

           }
           catch (Exception ex)
           {
               MessageBox.Show(string.Format("Exception has occured: {0}", ex.Message), "Exception", MessageBoxButtons.OK);
               throw;
           }
       }

       private void Publish(BrokeredMessage message)
       {
           try
           {
               this.UpdateUI(message.GetBody<EventMessage>().Message);
               message.Complete();
           }
           catch (Exception)
           {
               message.Abandon();
           }
           finally
           {
               message.Dispose();
           }
       }

    }
}
