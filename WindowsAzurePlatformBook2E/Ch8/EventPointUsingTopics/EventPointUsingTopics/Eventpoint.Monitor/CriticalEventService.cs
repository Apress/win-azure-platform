using System;
using System.ServiceModel;
using System.Windows.Forms;


namespace EventPoint.Monitor
{

    [ServiceBehavior(Name = "CriticalEvent", Namespace="http://eventpoint/relay")]
    public class CriticalEventService : EventPoint.Common.ICriticalEvent
    {
        public void RegisterAlert(EventPoint.Common.EventMessage alertMsg)
        {
            Form1 f = (Form1) Application.OpenForms[0];
            f.UpdateUI(alertMsg.Message);
        }
    }

}
