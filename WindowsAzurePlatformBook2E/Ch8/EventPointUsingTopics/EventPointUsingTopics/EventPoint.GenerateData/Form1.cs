using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;
using EventPoint.Common;
//using EventPoint.Data;


namespace EventPoint_GenerateData
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string topicName = "eventpoint";
        TopicDescription topicDescription;
        TopicClient eventTopic;
        NamespaceManager nsManager;
        MessagingFactory messageFactory;

        private string[] Messages = new string[] {  "*** Critical, unhandled exception ***", 
                                                    "Service timeout, retrying",
                                                    "Monitoring data at 90% of allocated quota",
                                                    "Queue depth at 80%",
                                                    "Number of calls greater than 30-day moving average"};
 
        private void Form1_Load(object sender, EventArgs e)
        
        {
            // default is 2, for throughput optimization, lets bump it up.
            ServicePointManager.DefaultConnectionLimit = 12;

            // another optimization, saves one round trip per msg by skipping the "I have some thing for you, is that ok?" handshake
            ServicePointManager.Expect100Continue = false;

            // Create Namespace Manager and messaging factory
            Uri serviceAddress = ServiceBusEnvironment.CreateServiceUri("sb", Config.serviceNamespace, string.Empty);
            nsManager = new NamespaceManager(serviceAddress, TokenProvider.CreateSharedSecretTokenProvider(Config.issuerName, Config.issuerSecret));
            messageFactory = MessagingFactory.Create(serviceAddress, TokenProvider.CreateSharedSecretTokenProvider(Config.issuerName, Config.issuerSecret));

            // set up the topic with batched operations, and time to live of 10 minutes. Subscriptions will not delete the message, since multiple clients are accessing the message
            // it will expire on its own after 10 minutes.
            topicDescription = new TopicDescription(topicName) { DefaultMessageTimeToLive = TimeSpan.FromMinutes(10), Path = topicName, EnableBatchedOperations = true };
            if (!nsManager.TopicExists(topicDescription.Path))
                nsManager.CreateTopic(topicDescription);

            // create client
            eventTopic = messageFactory.CreateTopicClient(topicName);
        }

        private void btnSendBulk_Click(object sender, EventArgs e)
        {
            
            try
            {

                txtStatus.Text = string.Empty;
                EventMessage evt = new EventMessage();
                int idprefix = new Random().Next(10000000);
                int index;
                for (int i = 0; i < Convert.ToInt32(txtNumberOfMessages.Text); i++)
                {
                    this.lblSent.Text = (i+1).ToString();
                    //evt.ID = idprefix + i;
                    evt.Link = "http://thelink.com";
                    index = new Random().Next(0, 5);
                    evt.Message = Messages[index];
                    evt.Originator = string.Format("EventPoint Generator - {0}", Guid.NewGuid());
                    evt.Title = "test msg";
                    evt.Priority = index.ToString();

                    BrokeredMessage brokeredMessage = new BrokeredMessage(evt);
                    brokeredMessage.CorrelationId = evt.Priority; // for CorrelationFilter in subscription
                    //brokeredMessage.Properties["priority"] = evt.Priority; // if SqlFilter is preferred
                    eventTopic.Send(brokeredMessage);

                }

                //txtStatus.Text += string.Format("Queue contains APPROXIMATELY {0} messages.", queue.RetrieveApproximateMessageCount());
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Exception has occured: {0}", ex.Message), "Exception", MessageBoxButtons.OK);
                throw;
            }
        }


        private void btnSendCritical_Click(object sender, EventArgs e)
        {
            ChannelFactory<ICriticalEvent> channelFactory = new ChannelFactory<ICriticalEvent>("sbrelay", new EndpointAddress("sb://eventpoint-critical.servicebus.windows.net"));
            ICriticalEvent ic = channelFactory.CreateChannel();
            EventPoint.Common.EventMessage eventMsg = new EventPoint.Common.EventMessage();
            eventMsg.Message = "Manually sent from generator";
            eventMsg.Priority = "0";
            eventMsg.Originator = Environment.UserName;
            BrokeredMessage brokeredMessage = new BrokeredMessage(eventMsg);
            brokeredMessage.CorrelationId = eventMsg.Priority; // for CorrelationFilter in subscription
            //brokeredMessage.Properties["priority"] = evt.Priority; // if SqlFilter is preferred
            eventTopic.Send(brokeredMessage);

        }



      }
}
