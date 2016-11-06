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
using System.ServiceModel.Channels;


namespace MessageBuffer
{
    public partial class Form1 : Form
    {
       
        public Form1()
        {
            InitializeComponent();
        }

        #region Message Buffer Methods

        private  MessageBufferPolicy GetMessagBufferPolicy(double bufferExpirationTime, int maxMessageCount)
        {
           

            MessageBufferPolicy policy = new MessageBufferPolicy
            {
                ExpiresAfter = TimeSpan.FromMinutes(bufferExpirationTime),
                MaxMessageCount = maxMessageCount,
                OverflowPolicy = OverflowPolicy.RejectIncomingMessage,
                Authorization = AuthorizationPolicy.Required,
                Discoverability = DiscoverabilityPolicy.Public,
                TransportProtection = TransportProtectionPolicy.None
            
               
            };
           

            return policy;
        }

        private  TransportClientEndpointBehavior GetACSSecurity(string issuerName, string issuerKey)
        {

            TransportClientEndpointBehavior behavior = new TransportClientEndpointBehavior();
            behavior.TokenProvider = SharedSecretTokenProvider.CreateSharedSecretTokenProvider(issuerName, Encoding.ASCII.GetBytes(issuerKey));

            return behavior;
        }

        private MessageBufferClient CreateMessageBuffer(string serviceNamespace, string messageBufferName, TransportClientEndpointBehavior behavior, MessageBufferPolicy policy)
        {
            MessageVersion messageVersion = MessageVersion.Soap12WSAddressing10;
            Uri messageBufferUri = GetMessageBufferUri();
            return MessageBufferClient.CreateMessageBuffer(behavior, messageBufferUri, policy, messageVersion);
        }

        private void SendMessage(string message, MessageBufferClient client)
        {
            System.ServiceModel.Channels.Message msg = System.ServiceModel.Channels.Message.CreateMessage(MessageVersion.Default, string.Empty, message);
            client.Send(msg);
            msg.Close();
        }

        private string RetrieveMessage(MessageBufferClient client)
        {
            System.ServiceModel.Channels.Message retrievedMessage = null; ;

            try
            {
                retrievedMessage = client.Retrieve();

                return retrievedMessage.GetBody<string>();
            }
            finally
            {
                if(retrievedMessage != null)
                retrievedMessage.Close();
            }
           
        }

        private string PeekAndRetrieveMessage(MessageBufferClient client)
        {
         
            System.ServiceModel.Channels.Message lockedMessage = client.PeekLock();
            try
            {
                client.DeleteLockedMessage(lockedMessage);
                return lockedMessage.GetBody<string>();
            }
            finally
            {
                if (lockedMessage != null)
                    lockedMessage.Close();
            }
        }

        private Uri GetMessageBufferUri()
        {
            return ServiceBusEnvironment.CreateServiceUri("http", txtServiceNamespace.Text, txtMessageBuffer.Text);

        }

        private MessageBufferClient GetMessageBufferClient()
        {
            TransportClientEndpointBehavior behavior = GetACSSecurity(txtIssuerName.Text, txtIssuerKey.Text);
            Uri messageBufferUri = GetMessageBufferUri();

            return MessageBufferClient.GetMessageBuffer(behavior, messageBufferUri);

        }
        public void SendMessages()
        {

            MessageBufferClient client = GetMessageBufferClient();
            AddLog(String.Format("Message buffer '{0}'.", client.MessageBufferUri));


            // send three messages to the message buffer
            for (int i = 1; i <= 10; ++i)
            {
                string msg = String.Format("{0}-{1}", txtMessage.Text, System.Guid.NewGuid().ToString("N"));
                SendMessage(msg, client);
                AddLog(msg + " sent.");
            }
        }

        public void RetrieveMessage()
        {
            MessageBufferClient client = GetMessageBufferClient();
            AddLog(String.Format("Message buffer '{0}'.", client.MessageBufferUri));
            AddLog("Retrieved Message:" + RetrieveMessage(client));

        }

        public void PeekAndRetrieveMessage()
        {
            MessageBufferClient client = GetMessageBufferClient();
            AddLog(String.Format("Message buffer '{0}'.", client.MessageBufferUri));
            AddLog("Peek and Retrieve Message:" + PeekAndRetrieveMessage(client));

        }

        public void DeleteMessageBuffer()
        {
            MessageBufferClient client = GetMessageBufferClient();
            // delete the message buffer
            client.DeleteMessageBuffer();
           AddLog(String.Format("Message buffer {0} deleted.", client.MessageBufferUri));
        }
    
        #endregion

        private void AddLog(string log)
        {
            txtLog.AppendText(String.Format("LOG>{0}{1}", log, Environment.NewLine));


        }
        private void ClearLog()
        {
            txtLog.Clear();
        }

        private void btnCreateBuffer_Click(object sender, EventArgs e)
        {
            try
            {
                TransportClientEndpointBehavior behavior = GetACSSecurity(txtIssuerName.Text, txtIssuerKey.Text);
                MessageBufferPolicy policy = GetMessagBufferPolicy(10d, 100);
                //  messageBufferUri = ServiceBusEnvironment.CreateServiceUri("http", txtServiceNamespace.Text, txtMessageBuffer.Text);
                MessageBufferClient client = CreateMessageBuffer(txtServiceNamespace.Text, txtMessageBuffer.Text, behavior, policy);
                AddLog(String.Format("Message Buffer {0} created", client.MessageBufferUri.ToString()));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK);

            }
        }

        private void btnSendMessages_Click(object sender, EventArgs e)
        {
            try
            {
                SendMessages();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK);

            }
        }

        private void btnRetrieve_Click(object sender, EventArgs e)
        {
            try
            {
                RetrieveMessage();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK);

            }
        }

        private void btnPeekAndRetrieve_Click(object sender, EventArgs e)
        {
            try
            {
                PeekAndRetrieveMessage();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK);

            }
        }

        private void btnDeleteBuffer_Click(object sender, EventArgs e)
        {
            try
            {
                DeleteMessageBuffer();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK);

            }
        }


    }
}
