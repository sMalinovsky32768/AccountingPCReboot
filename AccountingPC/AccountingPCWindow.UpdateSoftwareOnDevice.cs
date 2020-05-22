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
            String connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            switch (typeDevice)
            {
                case TypeDevice.PC:
                    UpdateSoftwareOnPC(connectionString);
                    UpdateNotInstalledSoftwareOnPC(connectionString);
                    break;
                case TypeDevice.Notebook:
                    UpdateSoftwareOnNotebook(connectionString);
                    UpdateNotInstalledSoftwareOnNotebook(connectionString);
                    break;
            }
        }

        private void UpdateSoftwareOnPC(String connectionString)
        {
            pcSoftwareDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetSoftwareOnPC({deviceID})", connectionString);
            pcSoftwareDataSet = new DataSet();
            pcSoftwareDataAdapter.Fill(pcSoftwareDataSet);
            softwareOnDevice.ItemsSource = pcSoftwareDataSet.Tables[0].DefaultView;
            pcSoftware = new List<Software>();
            foreach (DataRow row in pcSoftwareDataSet.Tables[0].Rows)
            {
                pcSoftware.Add(new Software()
                {
                    ID = Convert.ToInt32(row[0]),
                    Name = row[1].ToString(),
                    CountInstalled = Convert.ToInt32(row[2].GetType() != typeof(DBNull) ? row[2] : 0)
                });
            }
            softwareOnDevice.ItemsSource = pcSoftware;
        }

        private void UpdateSoftwareOnNotebook(String connectionString)
        {
            notebookSoftwareDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetSoftwareOnNotebook({deviceID})", connectionString);
            notebookSoftwareDataSet = new DataSet();
            notebookSoftwareDataAdapter.Fill(notebookSoftwareDataSet);
            notebookSoftware = new List<Software>();
            foreach (DataRow row in notebookSoftwareDataSet.Tables[0].Rows)
            {
                notebookSoftware.Add(new Software()
                {
                    ID = Convert.ToInt32(row[0]),
                    Name = row[1].ToString(),
                    CountInstalled = Convert.ToInt32(row[2].GetType() != typeof(DBNull) ? row[2] : 0)
                });
            }
            softwareOnDevice.ItemsSource = notebookSoftware;
        }

        private void UpdateNotInstalledSoftwareOnPC(String connectionString)
        {
            pcNotInstalledSoftwareDataAdapter = new SqlDataAdapter($"select * From dbo.GetNotInstalledOnNotebook({deviceID})", connectionString);
            pcNotInstalledSoftwareDataSet = new DataSet();
            pcNotInstalledSoftwareDataAdapter.Fill(pcNotInstalledSoftwareDataSet);
            pcNotInstalledSoftware = new List<Software>();
            foreach (DataRow row in pcNotInstalledSoftwareDataSet.Tables[0].Rows)
            {
                pcNotInstalledSoftware.Add(new Software()
                {
                    ID = Convert.ToInt32(row[0]),
                    Name = row[1].ToString(),
                    CountInstalled = Convert.ToInt32(row[2].GetType() != typeof(DBNull) ? row[2] : 0)
                });
            }
            addSoftware.ContextMenu.ItemsSource = pcNotInstalledSoftware;
        }

        private void UpdateNotInstalledSoftwareOnNotebook(String connectionString)
        {
            notebookNotInstalledSoftwareDataAdapter = new SqlDataAdapter($"select * From dbo.GetNotInstalledOnNotebook({deviceID})", connectionString);
            notebookNotInstalledSoftwareDataSet = new DataSet();
            notebookNotInstalledSoftwareDataAdapter.Fill(notebookNotInstalledSoftwareDataSet);
            notebookNotInstalledSoftware = new List<Software>();
            foreach (DataRow row in notebookNotInstalledSoftwareDataSet.Tables[0].Rows)
            {
                notebookNotInstalledSoftware.Add(new Software()
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
