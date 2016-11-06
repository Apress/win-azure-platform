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

namespace NetOnewayRelayClient
{
    public partial class EnergyManagementDevice : Form
    {
     ChannelFactory<IOnewayEnergyServiceChannel> netOnewayChannelFactory;
     IOnewayEnergyServiceChannel netOnewayChannel;

        short lightBulbState = 0;
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
        }

        private void SendKwh()
        {
            Random r = new Random();
            double kwh = double.Parse(String.Format("{0:0.00}", (r.NextDouble() * 100)));
            try
            {

             if (netOnewayChannel != null)
             {
                 ServiceBusHelper.SendKwhValue(netOnewayChannel, tsDeviceId.Text, "Meter-1", kwh);
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
            ChangeLightBulbState();
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

        private void ChangeLightBulbState()
        {
            if (IsLightBulbOn())
            {
                this.btnLightBulb.Image = global::NetOnewayRelayClient.Properties.Resources.off;
                lightBulbState = 0;

            }
            else
            {
                this.btnLightBulb.Image = global::NetOnewayRelayClient.Properties.Resources.on;
                lightBulbState = 1;

            }

            try
            {

             if (netOnewayChannel != null)
             {
              ServiceBusHelper.SendLightingValue(netOnewayChannel, tsDeviceId.Text, "LightSwitch-1", lightBulbState);
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
                    ServiceBusHelper.SendHVACModeValue(netOnewayChannel, tsDeviceId.Text, "HVAC-1", hvacMode);
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
                    ServiceBusHelper.SendHVACSetPointValue(netOnewayChannel, tsDeviceId.Text, "HVAC-1", setPoint);
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


        private void btnKwh_Click(object sender, EventArgs e)
        {
            SendKwh();
        }

        private void cbAutoSend_CheckedChanged(object sender, EventArgs e)
        {
            if (cbAutoSend.Checked)
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

     #region Service Bus Methods

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

        private void CloseChannels()
        {
            if (this.netOnewayChannel != null && this.netOnewayChannelFactory != null)
            {
                ServiceBusHelper.CloseoneWayChannelFactoryAndChannel(this.netOnewayChannelFactory, this.netOnewayChannel);
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
           
            kwhTimer.Tick += new EventHandler(kwhTimer_Tick);
            kwhTimer.Interval = 10000;
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

      
       
    }
}
