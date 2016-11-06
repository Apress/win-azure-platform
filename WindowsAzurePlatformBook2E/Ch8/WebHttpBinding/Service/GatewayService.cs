

namespace RESTGatewayServer
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using Microsoft.ServiceBus.Web;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using EnergyServiceContract;

    [ServiceBehavior(Name = "GatewayService", Namespace = "http://proazure/ServiceBus/energyservice/gateway/")]
    public class GatewayService : IRESTLightswitch, IRESTEnergyMeter
    {
         const string ON_FILE = "on.jpg";
         const string OFF_FILE = "off.jpg";
         Image on, off;
         static int LIGHT_BULB_STATE = 0;

      

        public GatewayService()
        {
            on = Image.FromFile(ON_FILE);
            off = Image.FromFile(OFF_FILE);
        }

      

        #region IGatewayContract Members

        public Message GetLightswitchState()
        {

            Message m = Message.CreateMessage(OperationContext.Current.IncomingMessageVersion, "GETRESPONSE", "ON");
            return m;
        }

        #endregion

        #region IRESTGatewayContract Members

        System.ServiceModel.Channels.Message IRESTLightswitch.GetLightswitchState()
        {
            System.ServiceModel.Channels.Message response = StreamMessageHelper.CreateMessage(OperationContext.Current.IncomingMessageVersion, "GETRESPONSE", this.WriteImageToStream);

            HttpResponseMessageProperty responseProperty = new HttpResponseMessageProperty();
            responseProperty.Headers.Add("Content-Type", "image/jpeg");
            response.Properties.Add(HttpResponseMessageProperty.Name, responseProperty);
            Console.WriteLine("Sent Light switch state to the caller");

            return response;
        }


        public void WriteImageToStream(System.IO.Stream stream)
        {
            Image i = (LIGHT_BULB_STATE == 0) ? off : on;
            i.Save(stream, ImageFormat.Jpeg);
            if (LIGHT_BULB_STATE == 0)
            {
                LIGHT_BULB_STATE = 1;
            }
            else
            {
                LIGHT_BULB_STATE = 0;
            }
        }

        System.ServiceModel.Channels.Message IRESTEnergyMeter.GetKWhValue()
        {
            Random r = new Random();
            double kwhValue = double.Parse(String.Format("{0:0.00}", (r.NextDouble() * 100)));
            System.ServiceModel.Channels.Message m = System.ServiceModel.Channels.Message.CreateMessage(OperationContext.Current.IncomingMessageVersion, "GETRESPONSE", String.Format("{0:00}", kwhValue));
            return m;
        }


      
        #endregion
    }
}
