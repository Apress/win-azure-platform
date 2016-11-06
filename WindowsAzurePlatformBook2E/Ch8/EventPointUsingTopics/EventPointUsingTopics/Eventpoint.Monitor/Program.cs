using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.ServiceModel;

namespace EventPoint.Monitor
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form1 frm = new Form1();
            Application.Run(frm);
        }
    }
}
