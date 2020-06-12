using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;

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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateSoftwareOnPC()
        {
            DefaultDataSet.Tables["PCSoftware"].Clear();
            new SqlDataAdapter($"SELECT * FROM dbo.GetSoftwareOnPC({DeviceID})", ConnectionString).Fill(DefaultDataSet, "PCSoftware");
            PcSoftware = new List<InstalledSoftware>
            {
                Capacity = 128
            };
            for (int i = 0; i < DefaultDataSet.Tables["PCSoftware"].Rows.Count; i++)
            {
                DataRow row = DefaultDataSet.Tables["PCSoftware"].Rows[i];
                PcSoftware?.Add(new InstalledSoftware()
                {
                    ID = Convert.ToInt32(row[0]),
                    Name = row[1].ToString(),
                    CountInstalled = Convert.ToInt32(row[2].GetType() != typeof(DBNull) ? row[2] : 0)
                });
            }
            softwareOnDevice.ItemsSource = PcSoftware;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateSoftwareOnNotebook()
        {
            DefaultDataSet.Tables["NotebookSoftware"].Clear();
            new SqlDataAdapter($"SELECT * FROM dbo.GetSoftwareOnNotebook({DeviceID})", ConnectionString).Fill(DefaultDataSet, "NotebookSoftware");
            NotebookSoftware = new List<InstalledSoftware>
            {
                Capacity = 128
            };
            for (int i = 0; i < DefaultDataSet.Tables["NotebookSoftware"].Rows.Count; i++)
            {
                DataRow row = DefaultDataSet.Tables["NotebookSoftware"].Rows[i];
                NotebookSoftware?.Add(new InstalledSoftware()
                {
                    ID = Convert.ToInt32(row[0]),
                    Name = row[1].ToString(),
                    CountInstalled = Convert.ToInt32(row[2].GetType() != typeof(DBNull) ? row[2] : 0)
                });
            }
            softwareOnDevice.ItemsSource = NotebookSoftware;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateNotInstalledSoftwareOnPC()
        {
            DefaultDataSet.Tables["PCNotInstalledSoftware"].Clear();
            new SqlDataAdapter($"select * From dbo.GetNotInstalledOnPC({DeviceID})", ConnectionString).Fill(DefaultDataSet, "PCNotInstalledSoftware");
            PcNotInstalledSoftware = new List<InstalledSoftware>
            {
                Capacity = 128
            };
            for (int i = 0; i < DefaultDataSet.Tables["PCNotInstalledSoftware"].Rows.Count; i++)
            {
                DataRow row = DefaultDataSet.Tables["PCNotInstalledSoftware"].Rows[i];
                PcNotInstalledSoftware?.Add(new InstalledSoftware()
                {
                    ID = Convert.ToInt32(row[0]),
                    Name = row[1].ToString(),
                    CountInstalled = Convert.ToInt32(row[2].GetType() != typeof(DBNull) ? row[2] : 0)
                });
            }
            addSoftware.ContextMenu.ItemsSource = PcNotInstalledSoftware;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateNotInstalledSoftwareOnNotebook()
        {
            DefaultDataSet.Tables["NotebookNotInstalledSoftware"].Clear();
            new SqlDataAdapter($"select * From dbo.GetNotInstalledOnNotebook({DeviceID})", ConnectionString).Fill(DefaultDataSet, "NotebookNotInstalledSoftware");
            NotebookNotInstalledSoftware = new List<InstalledSoftware>
            {
                Capacity = 128
            };
            for (int i = 0; i < DefaultDataSet.Tables["NotebookNotInstalledSoftware"].Rows.Count; i++)
            {
                DataRow row = DefaultDataSet.Tables["NotebookNotInstalledSoftware"].Rows[i];
                NotebookNotInstalledSoftware?.Add(new InstalledSoftware()
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
