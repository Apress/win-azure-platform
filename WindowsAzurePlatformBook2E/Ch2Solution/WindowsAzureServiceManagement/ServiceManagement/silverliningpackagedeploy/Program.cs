using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace silverliningpackagedeploy
{
    class Program
    {
        /*
        static void Main(string[] args)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = localFilePath;
            startInfo.CreateNoWindow = true;
            startInfo.ErrorDialog = false;
            startInfo.UseShellExecute = false;

            // startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;
            //Add parameters for finalization or another exe for finalization
            startInfo.Arguments = exeArguments;

            Process p1 = new Process();
            p1.OutputDataReceived += new DataReceivedEventHandler(p1_OutputDataReceived);
            p1.ErrorDataReceived += new DataReceivedEventHandler(p1_ErrorDataReceived);
            p1.Disposed += new EventHandler(p1_Disposed);
            p1.Exited += new EventHandler(p1_Exited);
            p1.StartInfo = startInfo;
            if (p1.Start())
            {

                TimeSpan waitTime = TimeSpan.FromMinutes(maxExeProcessWaitTimeInMins);

                if (!p1.WaitForExit((int)waitTime.TotalMilliseconds))
                {

                    throw new Exception("Process timed out after minutes:" + maxExeProcessWaitTimeInMins);

                }
                Console.WriteLine(p1.StandardOutput.ReadToEnd());
                
              

            }//if


            
        }

        static void p1_Exited(object sender, EventArgs e)
        {
            Console.WriteLine("Exited");

        }

        static void p1_Disposed(object sender, EventArgs e)
        {
            Console.WriteLine("Disposed");
        }

        static void p1_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine("Error>" + e.Data);
        }

        static void p1_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine("Output>" + e.Data);
        }
         * */
    }
}
