using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Windows;

namespace AccountingPC
{
    public partial class AccountingPCWindow
    {
        private void UpdatePCData()
        {
            DefaultDataSet.Tables["PC"].Clear();
            new SqlDataAdapter("SELECT * FROM dbo.GetAllPC()", ConnectionString).Fill(DefaultDataSet, "PC");
            if (!DefaultDataSet.Tables["PC"].Columns.Contains("Видеоразъемы"))
                DefaultDataSet.Tables["PC"].Columns.Add("Видеоразъемы");
            for (int j = 0; j < DefaultDataSet.Tables["PC"].Rows.Count; j++) 
            {
                DataRow row = DefaultDataSet.Tables["PC"].Rows[j];
                row["Видеоразъемы"] = row["VideoConnectors"].GetType() == typeof(int) ?
                    GetVideoConnectors(Convert.ToInt32(row["VideoConnectors"])) : row["VideoConnectors"];
            }
            int i = DefaultDataSet.Tables["PC"].DefaultView.Table.Columns.IndexOf("VideoConnectors");
            int ii = DefaultDataSet.Tables["PC"].DefaultView.Table.Columns.IndexOf("Видеоразъемы");
            DefaultDataSet.Tables["PC"].DefaultView.Table.Columns["Видеоразъемы"].SetOrdinal(i);
            DefaultDataSet.Tables["PC"].DefaultView.Table.Columns["VideoConnectors"].SetOrdinal(ii);
        }

        private void UpdateNotebookData()
        {
            DefaultDataSet.Tables["Notebook"].Clear();
            new SqlDataAdapter("SELECT * FROM dbo.GetAllNotebook()", ConnectionString).Fill(DefaultDataSet, "Notebook");
            if (!DefaultDataSet.Tables["Notebook"].Columns.Contains("Видеоразъемы"))
                DefaultDataSet.Tables["Notebook"].Columns.Add("Видеоразъемы");
            for (int j = 0; j < DefaultDataSet.Tables["Notebook"].Rows.Count; j++)
            {
                DataRow row = DefaultDataSet.Tables["Notebook"].Rows[j];
                row["Видеоразъемы"] = row["VideoConnectors"].GetType() == typeof(int) ?
                    GetVideoConnectors(Convert.ToInt32(row["VideoConnectors"])) : row["VideoConnectors"];
            }
            int i = DefaultDataSet.Tables["Notebook"].DefaultView.Table.Columns.IndexOf("VideoConnectors");
            int ii = DefaultDataSet.Tables["Notebook"].DefaultView.Table.Columns.IndexOf("Видеоразъемы");
            DefaultDataSet.Tables["Notebook"].DefaultView.Table.Columns["Видеоразъемы"].SetOrdinal(i);
            DefaultDataSet.Tables["Notebook"].DefaultView.Table.Columns["VideoConnectors"].SetOrdinal(ii);
        }

        private void UpdateMonitorData()
        {
            DefaultDataSet.Tables["Monitor"].Clear();
            new SqlDataAdapter("SELECT * FROM dbo.GetAllMonitor()", ConnectionString).Fill(DefaultDataSet, "Monitor");
            if (!DefaultDataSet.Tables["Monitor"].Columns.Contains("Видеоразъемы"))
                DefaultDataSet.Tables["Monitor"].Columns.Add("Видеоразъемы");
            for (int j = 0; j < DefaultDataSet.Tables["Monitor"].Rows.Count; j++)
            {
                DataRow row = DefaultDataSet.Tables["Monitor"].Rows[j];
                row["Видеоразъемы"] = row["VideoConnectors"].GetType() == typeof(int) ?
                    GetVideoConnectors(Convert.ToInt32(row["VideoConnectors"])) : row["VideoConnectors"];
            }
            int i = DefaultDataSet.Tables["Monitor"].DefaultView.Table.Columns.IndexOf("VideoConnectors");
            int ii = DefaultDataSet.Tables["Monitor"].DefaultView.Table.Columns.IndexOf("Видеоразъемы");
            DefaultDataSet.Tables["Monitor"].DefaultView.Table.Columns["Видеоразъемы"].SetOrdinal(i);
            DefaultDataSet.Tables["Monitor"].DefaultView.Table.Columns["VideoConnectors"].SetOrdinal(ii);
        }

        private void UpdateProjectorData()
        {
            DefaultDataSet.Tables["Projector"].Clear();
            new SqlDataAdapter("SELECT * FROM dbo.GetAllProjector()", ConnectionString).Fill(DefaultDataSet, "Projector");
            if (!DefaultDataSet.Tables["Projector"].Columns.Contains("Видеоразъемы"))
            DefaultDataSet.Tables["Projector"].Columns.Add("Видеоразъемы");
            for (int j = 0; j < DefaultDataSet.Tables["Projector"].Rows.Count; j++)
            {
                DataRow row = DefaultDataSet.Tables["Projector"].Rows[j];
                row["Видеоразъемы"] = row["VideoConnectors"].GetType() == typeof(int) ?
                    GetVideoConnectors(Convert.ToInt32(row["VideoConnectors"])) : row["VideoConnectors"];
            }
            int i = DefaultDataSet.Tables["Projector"].DefaultView.Table.Columns.IndexOf("VideoConnectors");
            int ii = DefaultDataSet.Tables["Projector"].DefaultView.Table.Columns.IndexOf("Видеоразъемы");
            DefaultDataSet.Tables["Projector"].DefaultView.Table.Columns["Видеоразъемы"].SetOrdinal(i);
            DefaultDataSet.Tables["Projector"].DefaultView.Table.Columns["VideoConnectors"].SetOrdinal(ii);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateInteractiveWhiteboardData()
        {
            DefaultDataSet.Tables["Board"].Clear();
            new SqlDataAdapter("SELECT * FROM dbo.GetAllBoard()", ConnectionString).Fill(DefaultDataSet, "Board");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateProjectorScreenData()
        {
            DefaultDataSet.Tables["ProjectorScreen"].Clear();
            new SqlDataAdapter("SELECT * FROM dbo.GetAllScreen()", ConnectionString).Fill(DefaultDataSet, "ProjectorScreen");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdatePrinterScannerData()
        {
            DefaultDataSet.Tables["PrinterScanner"].Clear();
            new SqlDataAdapter("SELECT * FROM dbo.GetAllPrinterScanner()", ConnectionString).Fill(DefaultDataSet, "PrinterScanner");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateNetworkSwitchData()
        {
            DefaultDataSet.Tables["NetworkSwitch"].Clear();
            new SqlDataAdapter("SELECT * FROM dbo.GetAllNetworkSwitch()", ConnectionString).Fill(DefaultDataSet, "NetworkSwitch");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateOtherEquipmentData()
        {
            DefaultDataSet.Tables["OtherEquipment"].Clear();
            new SqlDataAdapter("SELECT * FROM dbo.GetAllOtherEquipment()", ConnectionString).Fill(DefaultDataSet, "OtherEquipment");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetViewToPC()
        {
            equipmentView.ItemsSource = DefaultDataSet.Tables["PC"].DefaultView;

            installedSoftware.Visibility = Visibility.Visible;
            softwareColumn.Width = new GridLength(1, GridUnitType.Star);
            TypeDevice = TypeDevice.PC;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetViewToNotebook()
        {
            equipmentView.ItemsSource = DefaultDataSet.Tables["Notebook"].DefaultView;

            TypeDevice = TypeDevice.Notebook;
            installedSoftware.Visibility = Visibility.Visible;
            softwareColumn.Width = new GridLength(1, GridUnitType.Star);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetViewToMonitor()
        {
            equipmentView.ItemsSource = DefaultDataSet.Tables["Monitor"].DefaultView;

            TypeDevice = TypeDevice.Monitor;
            installedSoftware.Visibility = Visibility.Collapsed;
            softwareColumn.Width = GridLength.Auto;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetViewToProjector()
        {
            equipmentView.ItemsSource = DefaultDataSet.Tables["Projector"].DefaultView;

            TypeDevice = TypeDevice.Projector;
            installedSoftware.Visibility = Visibility.Collapsed;
            softwareColumn.Width = GridLength.Auto;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetViewToInteractiveWhiteboard()
        {
            equipmentView.ItemsSource = DefaultDataSet.Tables["Board"].DefaultView;

            TypeDevice = TypeDevice.InteractiveWhiteboard;
            installedSoftware.Visibility = Visibility.Collapsed;
            softwareColumn.Width = GridLength.Auto;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetViewToProjectorScreen()
        {
            equipmentView.ItemsSource = DefaultDataSet.Tables["ProjectorScreen"].DefaultView;

            TypeDevice = TypeDevice.ProjectorScreen;
            installedSoftware.Visibility = Visibility.Collapsed;
            softwareColumn.Width = GridLength.Auto;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetViewToPrinterScanner()
        {
            equipmentView.ItemsSource = DefaultDataSet.Tables["PrinterScanner"].DefaultView;

            TypeDevice = TypeDevice.PrinterScanner;
            installedSoftware.Visibility = Visibility.Collapsed;
            softwareColumn.Width = GridLength.Auto;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetViewToNetworkSwitch()
        {
            equipmentView.ItemsSource = DefaultDataSet.Tables["NetworkSwitch"].DefaultView;

            TypeDevice = TypeDevice.NetworkSwitch;
            installedSoftware.Visibility = Visibility.Collapsed;
            softwareColumn.Width = GridLength.Auto;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetViewToOtherEquipment()
        {
            equipmentView.ItemsSource = DefaultDataSet.Tables["OtherEquipment"].DefaultView;

            TypeDevice = TypeDevice.OtherEquipment;
            installedSoftware.Visibility = Visibility.Collapsed;
            softwareColumn.Width = GridLength.Auto;
        }
    }
}
