using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace AccountingPC
{
    public partial class AccountingPCWindow
    {
        private void UpdateSoftwareOnDevice()
        {
            switch (TypeDevice)
            {
                case TypeDevice.PC:
                    UpdateSoftwareOnPC();
                    UpdateNotInstalledSoftwareOnPC();
                    break;
                case TypeDevice.Notebook:
                    UpdateSoftwareOnNotebook();
                    UpdateNotInstalledSoftwareOnNotebook();
                    break;
            }
        }

        private void UpdateSoftwareOnPC()
        {
            //PcSoftwareDataSet = new DataSet();
            //new SqlDataAdapter($"SELECT * FROM dbo.GetSoftwareOnPC({DeviceID})", ConnectionString).Fill(PcSoftwareDataSet);
            //softwareOnDevice.ItemsSource = PcSoftwareDataSet.Tables[0].DefaultView;

            new SqlDataAdapter($"SELECT * FROM dbo.GetSoftwareOnPC({DeviceID})", ConnectionString).Fill(DefaultDataSet, "PCSoftware");
            //softwareOnDevice.ItemsSource = DefaultDataSet.Tables["PCSoftware"].DefaultView;
            PcSoftware = new List<InstalledSoftware>
            {
                Capacity = 128
            };
            foreach (DataRow row in DefaultDataSet.Tables["PCSoftware"].Rows)
            {
                PcSoftware.Add(new InstalledSoftware()
                {
                    ID = Convert.ToInt32(row[0]),
                    Name = row[1].ToString(),
                    CountInstalled = Convert.ToInt32(row[2].GetType() != typeof(DBNull) ? row[2] : 0)
                });
            }
            softwareOnDevice.ItemsSource = PcSoftware;
        }

        private void UpdateSoftwareOnNotebook()
        {
            //NotebookSoftwareDataSet = new DataSet();
            //new SqlDataAdapter($"SELECT * FROM dbo.GetSoftwareOnNotebook({DeviceID})", ConnectionString).Fill(NotebookSoftwareDataSet);
            new SqlDataAdapter($"SELECT * FROM dbo.GetSoftwareOnNotebook({DeviceID})", ConnectionString).Fill(DefaultDataSet, "NotebookSoftware");
            NotebookSoftware = new List<InstalledSoftware>
            {
                Capacity = 128
            };
            foreach (DataRow row in DefaultDataSet.Tables["NotebookSoftware"].Rows)
            {
                NotebookSoftware.Add(new InstalledSoftware()
                {
                    ID = Convert.ToInt32(row[0]),
                    Name = row[1].ToString(),
                    CountInstalled = Convert.ToInt32(row[2].GetType() != typeof(DBNull) ? row[2] : 0)
                });
            }
            softwareOnDevice.ItemsSource = NotebookSoftware;
        }

        private void UpdateNotInstalledSoftwareOnPC()
        {
            //PcNotInstalledSoftwareDataSet = new DataSet();
            //new SqlDataAdapter($"select * From dbo.GetNotInstalledOnPC({DeviceID})", ConnectionString).Fill(PcNotInstalledSoftwareDataSet);
            new SqlDataAdapter($"select * From dbo.GetNotInstalledOnPC({DeviceID})", ConnectionString).Fill(DefaultDataSet, "PCNotInstalledSoftware");
            PcNotInstalledSoftware = new List<InstalledSoftware>
            {
                Capacity = 128
            };
            foreach (DataRow row in DefaultDataSet.Tables["PCNotInstalledSoftware"].Rows)
            {
                PcNotInstalledSoftware.Add(new InstalledSoftware()
                {
                    ID = Convert.ToInt32(row[0]),
                    Name = row[1].ToString(),
                    CountInstalled = Convert.ToInt32(row[2].GetType() != typeof(DBNull) ? row[2] : 0)
                });
            }
            addSoftware.ContextMenu.ItemsSource = PcNotInstalledSoftware;
        }

        private void UpdateNotInstalledSoftwareOnNotebook()
        {
            //NotebookNotInstalledSoftwareDataSet = new DataSet();
            //new SqlDataAdapter($"select * From dbo.GetNotInstalledOnNotebook({DeviceID})", ConnectionString).Fill(NotebookNotInstalledSoftwareDataSet);
            new SqlDataAdapter($"select * From dbo.GetNotInstalledOnNotebook({DeviceID})", ConnectionString).Fill(DefaultDataSet, "NotebookNotInstalledSoftware");
            NotebookNotInstalledSoftware = new List<InstalledSoftware>
            {
                Capacity = 128
            };
            foreach (DataRow row in DefaultDataSet.Tables["NotebookNotInstalledSoftware"].Rows)
            {
                NotebookNotInstalledSoftware.Add(new InstalledSoftware()
                {
                    ID = Convert.ToInt32(row[0]),
                    Name = row[1].ToString(),
                    CountInstalled = Convert.ToInt32(row[2].GetType() != typeof(DBNull) ? row[2] : 0)
                });
            }
            addSoftware.ContextMenu.ItemsSource = NotebookNotInstalledSoftware;
        }
    }
}
