using System;
using System.Collections.Generic;
using System.Configuration;
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
            pcSoftwareDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetSoftwareOnPC({DeviceID})", ConnectionString);
            pcSoftwareDataSet = new DataSet();
            pcSoftwareDataAdapter.Fill(pcSoftwareDataSet);
            softwareOnDevice.ItemsSource = pcSoftwareDataSet.Tables[0].DefaultView;
            pcSoftware = new List<InstalledSoftware>();
            foreach (DataRow row in pcSoftwareDataSet.Tables[0].Rows)
            {
                pcSoftware.Add(new InstalledSoftware()
                {
                    ID = Convert.ToInt32(row[0]),
                    Name = row[1].ToString(),
                    CountInstalled = Convert.ToInt32(row[2].GetType() != typeof(DBNull) ? row[2] : 0)
                });
            }
            softwareOnDevice.ItemsSource = pcSoftware;
        }

        private void UpdateSoftwareOnNotebook()
        {
            notebookSoftwareDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetSoftwareOnNotebook({DeviceID})", ConnectionString);
            notebookSoftwareDataSet = new DataSet();
            notebookSoftwareDataAdapter.Fill(notebookSoftwareDataSet);
            notebookSoftware = new List<InstalledSoftware>();
            foreach (DataRow row in notebookSoftwareDataSet.Tables[0].Rows)
            {
                notebookSoftware.Add(new InstalledSoftware()
                {
                    ID = Convert.ToInt32(row[0]),
                    Name = row[1].ToString(),
                    CountInstalled = Convert.ToInt32(row[2].GetType() != typeof(DBNull) ? row[2] : 0)
                });
            }
            softwareOnDevice.ItemsSource = notebookSoftware;
        }

        private void UpdateNotInstalledSoftwareOnPC()
        {
            pcNotInstalledSoftwareDataAdapter = new SqlDataAdapter($"select * From dbo.GetNotInstalledOnPC({DeviceID})", ConnectionString);
            pcNotInstalledSoftwareDataSet = new DataSet();
            pcNotInstalledSoftwareDataAdapter.Fill(pcNotInstalledSoftwareDataSet);
            pcNotInstalledSoftware = new List<InstalledSoftware>();
            foreach (DataRow row in pcNotInstalledSoftwareDataSet.Tables[0].Rows)
            {
                pcNotInstalledSoftware.Add(new InstalledSoftware()
                {
                    ID = Convert.ToInt32(row[0]),
                    Name = row[1].ToString(),
                    CountInstalled = Convert.ToInt32(row[2].GetType() != typeof(DBNull) ? row[2] : 0)
                });
            }
            addSoftware.ContextMenu.ItemsSource = pcNotInstalledSoftware;
        }

        private void UpdateNotInstalledSoftwareOnNotebook()
        {
            notebookNotInstalledSoftwareDataAdapter = new SqlDataAdapter($"select * From dbo.GetNotInstalledOnNotebook({DeviceID})", ConnectionString);
            notebookNotInstalledSoftwareDataSet = new DataSet();
            notebookNotInstalledSoftwareDataAdapter.Fill(notebookNotInstalledSoftwareDataSet);
            notebookNotInstalledSoftware = new List<InstalledSoftware>();
            foreach (DataRow row in notebookNotInstalledSoftwareDataSet.Tables[0].Rows)
            {
                notebookNotInstalledSoftware.Add(new InstalledSoftware()
                {
                    ID = Convert.ToInt32(row[0]),
                    Name = row[1].ToString(),
                    CountInstalled = Convert.ToInt32(row[2].GetType() != typeof(DBNull) ? row[2] : 0)
                });
            }
            addSoftware.ContextMenu.ItemsSource = notebookNotInstalledSoftware;
        }
    }
}
