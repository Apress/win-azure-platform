//-----------------------------------------------------------------------
// <copyright file="RDFEGlobals.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.Samples.WindowsAzure.ServiceManagement.Tools
{
    using System;

    using Microsoft.Samples.WindowsAzure.ServiceManagement;
    using System.Diagnostics;
using System.IO;

    /// <summary>
    /// Implements GetResult Command for getting status of Asynchronous operation
    /// </summary>
    class GenerateCertificateCommand : RDFEClientCommand
    {
        string makeCertPath;
        string certFileName;
        const string MakeCertFileName = "makecert.exe";
        public override void Setup(CommandLineParser parser, ref Command<RDFEGlobals> command)
        {
            parser.DefineAliases("GenerateCertificate", "gc", "generate-cert", "gene-c");
            parser.DefineParameterSet("GenerateCertificate", ref command, this, "Performs GetResult call");
            this.makeCertPath = Utility.ReadString(parser, "MakeCertPath", "The path of the makecert.exe file");
            this.certFileName = Utility.ReadString(parser, "CertFileName", "Certificate File Name");
        }

        public override bool Validate()
        {
            return (!string.IsNullOrEmpty(certFileName));
        }

        protected override void PerformOperation(IServiceManagement channel)
        {
            string makeCertFile = Path.Combine(makeCertPath, MakeCertFileName);
            if (!File.Exists(makeCertFile))
            {
                Console.WriteLine("Can not find the makecert file at the specified path.");
                return;
            }

            Console.WriteLine("Calling GenerateCertificate");

            Process certMaker = new Process();
            certMaker.StartInfo.FileName = makeCertFile;
            certMaker.StartInfo.Arguments = string.Format("-sr CurrentUser -ss My -n \"CN=Windows Azure Authentication Certificate\" -r {0}", certFileName);
            certMaker.StartInfo.UseShellExecute = false;
            certMaker.StartInfo.RedirectStandardOutput = true;
            certMaker.Start();

            Console.WriteLine("Result: " + certMaker.StandardOutput.ReadToEnd());
            certMaker.WaitForExit();
        }
    }

}
