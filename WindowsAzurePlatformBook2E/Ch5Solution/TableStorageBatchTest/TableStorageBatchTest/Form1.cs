using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.Services.Client;
using Microsoft.WindowsAzure.StorageClient;
using System.Diagnostics;

namespace TableStorageBatchTest
{
    public partial class Form1 : Form
    {
        TableStorageHelper TableStorageClient = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                TableStorageClient = new TableStorageHelper(txtStorageAcc.Text);

                TableStorageClient.CreateTable(DeploymentTracking.TABLE_NAME);

                Stopwatch s = new Stopwatch();
                s.Start();
                TableServiceContext context = TableStorageClient.GetTableServiceContext();

                long count = int.Parse(txtNumberEntities.Text);
              
                    for (long i = 0; i < count; i++)
                    {
                        if (i > 0 && (i % 100) == 0)
                        {
                            //Finish first batch
                            DataServiceResponse resp = context.SaveChangesWithRetries(SaveChangesOptions.Batch);
                            //start second batch
                            context.AddObject(DeploymentTracking.TABLE_NAME,
                              new DeploymentTracking()
                              {
                                  Created = DateTime.UtcNow,
                                  CreatedBy = Environment.MachineName,
                                  IsSuccessful = true,
                                  Operation = "Test Operation" + i,
                                  ProvId = 1,
                                  RetryNumber = 1,
                                  StatusCode = "Status Code " + i,
                                  StatusMessage = "Test Message" + i,
                                 // RowKey = System.Guid.NewGuid().ToString("N")
                                 RowKey = (i+1).ToString()
                              });
                        }
                        else
                        {
                            //continue
                            context.AddObject(DeploymentTracking.TABLE_NAME,
                              new DeploymentTracking()
                              {
                                  Created = DateTime.UtcNow,
                                  CreatedBy = Environment.MachineName,
                                  IsSuccessful = true,
                                  Operation = "Test Operation" + i,
                                  ProvId = 1,
                                  RetryNumber = 1,
                                  StatusCode = "Status Code " + i,
                                  StatusMessage = "Test Message" + i,
                                 // RowKey = System.Guid.NewGuid().ToString("N")
                                  RowKey = (i + 1).ToString()
                              });

                        }

                    }

                    DataServiceResponse resp1 = context.SaveChangesWithRetries(SaveChangesOptions.Batch);
                    s.Stop();
                    MessageBox.Show("Entities Inserted. Time Required i milliseconds. " + s.ElapsedMilliseconds);
           

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

      

        private void btnDeleteTable_Click(object sender, EventArgs e)
        {
            try
            {
                TableStorageClient = new TableStorageHelper(txtStorageAcc.Text);
                TableStorageClient.DeleteTable(DeploymentTracking.TABLE_NAME);
                MessageBox.Show("Table deleted.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }
    }
}
