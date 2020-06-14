using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Windows;

namespace AccountingPC
{
    public partial class AccountingPCWindow
    {
        private void UpdateSoftwareOnDevice()
        {
            try
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
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, exception.GetType().Name, MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateSoftwareOnPC()
        {
            DefaultDataSet.Tables["PCSoftware"].Clear();
            new SqlDataAdapter($"SELECT * FROM dbo.GetSoftwareOnPC({DeviceID})", ConnectionString).Fill(DefaultDataSet,
                "PCSoftware");
            PcSoftware = new List<InstalledSoftware>
            {
                Capacity = 128
            };
            for (var i = 0; i < DefaultDataSet.Tables["PCSoftware"].Rows.Count; i++)
            {
                var row = DefaultDataSet.Tables["PCSoftware"].Rows[i];
                PcSoftware?.Add(new InstalledSoftware
                {
                    ID = Convert.ToInt32(row[0]),
                    Name = row[1].ToString(),
                    CountInstalled = Convert.ToInt32(row[2] is DBNull ? 0 : row[2])
                });
            }

            softwareOnDevice.ItemsSource = PcSoftware;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateSoftwareOnNotebook()
        {
            DefaultDataSet.Tables["NotebookSoftware"].Clear();
            new SqlDataAdapter($"SELECT * FROM dbo.GetSoftwareOnNotebook({DeviceID})", ConnectionString).Fill(
                DefaultDataSet, "NotebookSoftware");
            NotebookSoftware = new List<InstalledSoftware>
            {
                Capacity = 128
            };
            for (var i = 0; i < DefaultDataSet.Tables["NotebookSoftware"].Rows.Count; i++)
            {
                var row = DefaultDataSet.Tables["NotebookSoftware"].Rows[i];
                NotebookSoftware?.Add(new InstalledSoftware
                {
                    ID = Convert.ToInt32(row[0]),
                    Name = row[1].ToString(),
                    CountInstalled = Convert.ToInt32(row[2] is DBNull ? 0 : row[2])
                });
            }

            softwareOnDevice.ItemsSource = NotebookSoftware;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateNotInstalledSoftwareOnPC()
        {
            DefaultDataSet.Tables["PCNotInstalledSoftware"].Clear();
            new SqlDataAdapter($"select * From dbo.GetNotInstalledOnPC({DeviceID})", ConnectionString).Fill(
                DefaultDataSet, "PCNotInstalledSoftware");
            PcNotInstalledSoftware = new List<InstalledSoftware>
            {
                Capacity = 128
            };
            for (var i = 0; i < DefaultDataSet.Tables["PCNotInstalledSoftware"].Rows.Count; i++)
            {
                var row = DefaultDataSet.Tables["PCNotInstalledSoftware"].Rows[i];
                PcNotInstalledSoftware?.Add(new InstalledSoftware
                {
                    ID = Convert.ToInt32(row[0]),
                    Name = row[1].ToString(),
                    CountInstalled = Convert.ToInt32(row[2] is DBNull ? 0 : row[2])
                });
            }

            addSoftware.ContextMenu.ItemsSource = PcNotInstalledSoftware;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateNotInstalledSoftwareOnNotebook()
        {
            DefaultDataSet.Tables["NotebookNotInstalledSoftware"].Clear();
            new SqlDataAdapter($"select * From dbo.GetNotInstalledOnNotebook({DeviceID})", ConnectionString).Fill(
                DefaultDataSet, "NotebookNotInstalledSoftware");
            NotebookNotInstalledSoftware = new List<InstalledSoftware>
            {
                Capacity = 128
            };
            for (var i = 0; i < DefaultDataSet.Tables["NotebookNotInstalledSoftware"].Rows.Count; i++)
            {
                var row = DefaultDataSet.Tables["NotebookNotInstalledSoftware"].Rows[i];
                NotebookNotInstalledSoftware?.Add(new InstalledSoftware
                {
                    ID = Convert.ToInt32(row[0]),
                    Name = row[1].ToString(),
                    CountInstalled = Convert.ToInt32(row[2] is DBNull ? 0 : row[2])
                });
            }

            if (addSoftware.ContextMenu != null) addSoftware.ContextMenu.ItemsSource = NotebookNotInstalledSoftware;
        }
    }
}