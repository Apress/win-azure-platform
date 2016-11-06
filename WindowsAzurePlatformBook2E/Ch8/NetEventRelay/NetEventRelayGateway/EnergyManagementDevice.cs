using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServiceBusUtils;
using EnergyServiceContract;
using System.ServiceModel;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Description;
using System.ServiceModel.Description;


namespace NetEventRelayGateway
{
 [ServiceBehavior(Name = "EnergyServiceGatewayOperations", 
     Namespace = "http://proazure/ServiceBus/energyservice/gateway",
     InstanceContextMode=InstanceContextMode.Single)]
 public partial class EnergyManagementDevice : Form, IEnergyServiceGatewayOperations 
    {
     #region Client Channels
     ChannelFactory<IOnewayEnergyServiceChannel> netOnewayChannelFactory;
     IOnewayEnergyServiceChannel netOnewayChannel;

     ChannelFactory<IMulticastGatewayChannel> netEventRelayChannelFactory;
     IMulticastGatewayChannel netEventRelayChannel;

     #endregion

     #region Servers
    // NetTcpRelayGatewayOperationsServer server;
     ServiceHost server;
     string serviceUri = string.Empty;
     #endregion
     short lightBulbState = 0;
     double kwh = 0;
        Timer kwhTimer = new Timer();

        int hvacMode = 0;//Off by default
        int currentTemperature = 55;
        static int setPoint = 55; 
         
        public EnergyManagementDevice()
        {
            InitializeComponent();
      
            
        }

        void kwhTimer_Tick(object sender, EventArgs e)
        {
            SendKwh();
            SendONLINEValue();
        }

        private void SendKwh()
        {
            Random r = new Random();
            kwh = double.Parse(String.Format("{0:0.00}", (r.NextDouble() * 100)));
            try
            {

             if (netOnewayChannel != null)
             {
                 ServiceBusHelper.SendKwhValue(netOnewayChannel, tsGatewayId.Text, "Meter-1", kwh);
              tslbl.Text = String.Format("Sent value {0} kWh @ {1}", kwh, DateTime.UtcNow.ToString("s"));
             }
             else
             {

              MessageBox.Show("netOnewayChannel is not initialized", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

             }
            }
            catch (Exception ex)
            {
             MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);


            }
        }

        private void btnLightBulb_Click(object sender, EventArgs e)
        {
            ChangeLightBulbState(true, 0);
        }

        private bool IsLightBulbOn()
        {

            if (lightBulbState == 1)
            {
            return true;
            }

            else 
            { 
                return false;
            }

        }

        private void ChangeLightBulbState(bool checkExistingState, short newState)
        {
         if (checkExistingState)
         {
          if (IsLightBulbOn())
          {
           this.btnLightBulb.Image = global::NetEventRelayGateway.Properties.Resources.off;
           lightBulbState = 0;

          }
          else
          {
           this.btnLightBulb.Image = global::NetEventRelayGateway.Properties.Resources.on;
           lightBulbState = 1;

          }
         }
         else
         {
          if (newState == 0)
          {
           this.btnLightBulb.Image = global::NetEventRelayGateway.Properties.Resources.off;
           lightBulbState = 0;

          }
          else
          {
           this.btnLightBulb.Image = global::NetEventRelayGateway.Properties.Resources.on;
           lightBulbState = 1;

          }


         }

            try
            {

             if (netOnewayChannel != null)
             {
              ServiceBusHelper.SendLightingValue(netOnewayChannel, tsGatewayId.Text, "LightSwitch-1", lightBulbState);
              AddLog(String.Format("Changed lightbulb state to {0}", ((lightBulbState == 1)?"ON":"OFF")));
             }
             else
             {

              MessageBox.Show("netOnewayChannel is not initialized", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

             }
            }
            catch (Exception ex)
            {
             MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);


            }

        }


        private void ChangeHVACMode()
        {
          
            try
            {

                if (netOnewayChannel != null)
                {
                    ServiceBusHelper.SendHVACModeValue(netOnewayChannel, tsGatewayId.Text, "HVAC-1", hvacMode);
                    AddLog(String.Format("HVAC Mode is set to {0}", ServiceBusHelper.GetHVACModeString(hvacMode)));
                }
                else
                {

                    MessageBox.Show("netOnewayChannel is not initialized", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);


            }

        }

        private void ChangeSetPointValue()
        {

            try
            {

                if (netOnewayChannel != null)
                {
                    ServiceBusHelper.SendHVACSetPointValue(netOnewayChannel, tsGatewayId.Text, "HVAC-1", setPoint);
                    AddLog(String.Format("Changed HVAC SetPoint to {0} degrees F", setPoint));
                }
                else
                {

                    MessageBox.Show("netOnewayChannel is not initialized", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);


            }

        }

        private void SendONLINEValue()
        {

         try
         {

          if (netEventRelayChannel != null)
          {

             
           netEventRelayChannel.Online(tsGatewayId.Text, serviceUri, DateTime.UtcNow);
           AddLog(String.Format("Sent ONLINE message @ {0} Uri:{1}", DateTime.UtcNow.ToString("s"), serviceUri));
          }
          else
          {

           MessageBox.Show("netEventRelayChannel is not initialized", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

          }
         }
         catch (Exception ex)
         {
          MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);


         }

        }


        private void btnKwh_Click(object sender, EventArgs e)
        {
            SendKwh();
        }

       

     #region Service Bus Methods

        //private void InitNetOnewayRelayClient()
        //{
        //    string solutionName = tsSolutionName.Text;
        //    string password = tsSolutionPassword.Text;
        //    string serviceUserName = tsSolutionToConnect.Text;
        //    try
        //    {
        //        TransportClientEndpointBehavior behavior = ServiceBusHelper.GetUsernamePasswordBehavior(solutionName, password);
        //        netOnewayChannelFactory = ServiceBusHelper.GetOnewayChannelFactory(serviceUserName);
        //        netOnewayChannelFactory.Credentials.UserName.UserName = solutionName;
        //        netOnewayChannelFactory.Credentials.UserName.Password = password;

        //        netOnewayChannelFactory.Endpoint.Behaviors.Add(behavior);

        //        netOnewayChannel = ServiceBusHelper.GetOneWayEnergyChannel(netOnewayChannelFactory);

        //        ClearLog();
        //        AddLog("Connected to " + netOnewayChannelFactory.Endpoint.Address.Uri.ToString());

        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);


        //    }

        //}

        private void InitNetOnewayRelayClient()
        {
            string issuerName = tsSolutionName.Text;
            string issuerKey = tsSolutionPassword.Text;
            string serviceNamespaceDomain = tsSolutionToConnect.Text;
            try
            {

                Uri address = ServiceBusEnvironment.CreateServiceUri("sb", serviceNamespaceDomain, "OnewayEnergyServiceOperations");

                netOnewayChannelFactory = new ChannelFactory<IOnewayEnergyServiceChannel>("RelayEndpoint", new EndpointAddress(address));

                netOnewayChannel = ServiceBusHelper.GetOneWayEnergyChannel(netOnewayChannelFactory);

                ClearLog();
                AddLog("Connected");
                AddLog(netOnewayChannelFactory.Endpoint.Address.Uri.ToString());


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);


            }

        }

        private void InitNetEventRelayClient()
        {
            string issuerName = tsSolutionName.Text;
            string issuerKey = tsSolutionPassword.Text;
            string serviceNamespaceDomain = tsSolutionToConnect.Text;
         try
         {
             TransportClientEndpointBehavior behavior = new TransportClientEndpointBehavior();
             string token = SharedSecretTokenProvider.ComputeSimpleWebTokenString(issuerName, issuerKey);
             behavior.TokenProvider = SimpleWebTokenProvider.CreateSimpleWebTokenProvider(token);
             Uri serviceUri = ServiceBusEnvironment.CreateServiceUri("sb", serviceNamespaceDomain, "Gateway/MulticastService/");

         // netEventRelayChannelFactory = new ChannelFactory<IMulticastGatewayChannel>("RelayMulticastEndpoint", new EndpointAddress(serviceUri));
          netEventRelayChannelFactory = new ChannelFactory<IMulticastGatewayChannel>("RelayMulticastEndpoint");
          netEventRelayChannelFactory.Endpoint.Behaviors.Add(behavior);
          netEventRelayChannel = netEventRelayChannelFactory.CreateChannel();
          netEventRelayChannel.Open();

          SendONLINEValue();
          AddLog("Connected to " + netEventRelayChannelFactory.Endpoint.Address.Uri.ToString());

         }
         catch (Exception ex)
         {
          MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);


         }

        }

        private void InitNetTcpRelayServer()
        {
            try
            {
                string issuerName = tsSolutionName.Text;
                string issuerKey = tsSolutionPassword.Text;
                string serviceNamespaceDomain = tsSolutionToConnect.Text;

              

                TransportClientEndpointBehavior behavior = new TransportClientEndpointBehavior();
                behavior.TokenProvider = SharedSecretTokenProvider.CreateSharedSecretTokenProvider(issuerName, Encoding.ASCII.GetBytes(issuerKey));

                Uri address = ServiceBusEnvironment.CreateServiceUri("sb", serviceNamespaceDomain, ServiceBusHelper.GetGatewayServicePath(tsGatewayId.Text));
                //For WS2207HttpRelayBinding
               // Uri address = ServiceBusEnvironment.CreateServiceUri("http", serviceNamespaceDomain, ServiceBusHelper.GetGatewayServicePath(tsGatewayId.Text));
                serviceUri = address.ToString();
                server = new ServiceHost(this, address);

                server.Description.Endpoints[0].Behaviors.Add(behavior);

                ServiceRegistrySettings settings = new ServiceRegistrySettings();
                settings.DiscoveryMode = DiscoveryType.Public;
                settings.DisplayName = address.ToString();
                foreach (ServiceEndpoint se in server.Description.Endpoints)
                    se.Behaviors.Add(settings);

                server.Open();
                AddLog("Gateway Server Running with ServiceUri:" + address.ToString());

                AddLog("Service registered for public discovery.");

            }
            catch (Exception ex)
            {
                MessageBox.Show( ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }


        private void CloseChannels()
        {
            if (this.netOnewayChannel != null && this.netOnewayChannelFactory != null)
            {
                ServiceBusHelper.CloseoneWayChannelFactoryAndChannel(this.netOnewayChannelFactory, this.netOnewayChannel);
            }

            if (netEventRelayChannel != null && netEventRelayChannelFactory != null)
            {
               
             try
             {

                 netEventRelayChannel.GoingOffline(tsGatewayId.Text, serviceUri, DateTime.UtcNow);
             }
             catch (Exception) { }
             netEventRelayChannel.Close();

             netEventRelayChannelFactory.Close();

            }
            if (server != null)
            {

                server.Close();

            }
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

        private void EnergyManagementDevice_FormClosing(object sender, FormClosingEventArgs e)
        {
         CloseChannels();
        }

        private void btnOnline_Click(object sender, EventArgs e)
        {

        }

        private void tsConnect_Click(object sender, EventArgs e)
        {
            InitNetOnewayRelayClient();

            InitNetEventRelayClient();

            InitNetTcpRelayServer();

            kwhTimer.Tick += new EventHandler(kwhTimer_Tick);
            kwhTimer.Interval = 10000;
            kwhTimer.Enabled = true;
            kwhTimer.Start();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            this.hvacMode = trackBar1.Value;
            ChangeHVACMode();
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            IncreaseOne();
        }
        private void btnDown_Click(object sender, EventArgs e)
        {
            DecreaseOne();
        }
        private void IncreaseOne()
        {
            setPoint = (int.Parse(txtSetPoint.Text) + 1);
            txtSetPoint.Text = setPoint.ToString();
            ChangeSetPointValue();
            SetTrackBar();
            ChangeHVACMode();
            

        }

        private void DecreaseOne()
        {
            setPoint = (int.Parse(txtSetPoint.Text) - 1);
            txtSetPoint.Text = setPoint.ToString();
            ChangeSetPointValue();
            SetTrackBar();
            ChangeHVACMode();

        }

        private void SetTrackBar()
        {
            if (setPoint > currentTemperature)
            {
                trackBar1.Value =hvacMode= 2;
            }
            else if (setPoint < currentTemperature)
            {
                trackBar1.Value = hvacMode = 1;
            }
            else
            {
                trackBar1.Value = hvacMode = 0;
            }


        }

        private void txtCurrentTemperature_TextChanged(object sender, EventArgs e)
        {
         currentTemperature = int.Parse(txtCurrentTemperature.Text);
        }

        private void cbOnline_CheckedChanged(object sender, EventArgs e)
        {
         if (cbOnline.Checked)
         {
          kwhTimer.Enabled = true;
          kwhTimer.Start();

         }
         else
         {
          kwhTimer.Stop();
          kwhTimer.Enabled = false;

         }
        }


    
        #region IEnergyServiceGatewayOperations Members
     

        public bool UpdateSoftware(string softwareUrl)
        {
         AddLog("UpdateSoftware:" + softwareUrl);
         return true;
        }

        public bool SetLightingValue(string gatewayId, string deviceId, short switchValue)
        {
         ChangeLightBulbState(false, switchValue);
         AddLog("SetLightingValue:" + switchValue);
         return true;
        }

        public bool SetHVACMode(string gatewayId, string deviceId, int hvMode)
        {
         hvacMode = hvMode;
         trackBar1.Value = hvacMode;
         ChangeHVACMode();
         AddLog("SetHVACMode:" + hvMode);
         return true;
        }

        public bool SetHVACSetpoint(string gatewayId, string deviceId, int spValue)
        {
         ChangeSetPointValue();
         AddLog("SetHVACSetpoint:" + spValue);

         return true;
        }

      
        public short GetLightingValue(string gatewayId, string deviceId)
        {
         AddLog("GetLightingValue:" + lightBulbState);

         return lightBulbState;
        }

        public int GetHVACMode(string gatewayId, string deviceId)
        {
         AddLog("GetHVACMode:" + hvacMode);

         return hvacMode;
        }
  

        public int GetHVACSetpoint(string gatewayId, string deviceId)
        {
         AddLog("GetHVACSetpoint:" + txtSetPoint.Text);
         return int.Parse(txtSetPoint.Text);
        }

        public int GetCurrentTemp(string gatewayId, string deviceId)
        {
         AddLog("GetCurrentTemp:" + txtCurrentTemperature.Text);
            return int.Parse(txtCurrentTemperature.Text);
        }


        public double GetKWhValue(string gatewayId, string deviceId)
        {
         AddLog("GetKWhValue:" + kwh);

            return kwh;
        }
        #endregion

        
    }
}
