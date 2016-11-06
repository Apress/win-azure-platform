using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.ServiceModel;
using ProAzureDemResContract;

namespace DemResServer
{
 static class Program
 {
 
  static void Main()
  {
   ServiceHost host = new ServiceHost(typeof(DemResService));

   Application.EnableVisualStyles();
   Application.SetCompatibleTextRenderingDefault(false);

   DemResServer form = new DemResServer();
   host.Open();

   Application.Run(form);

   host.Close();
  }
 }
}
