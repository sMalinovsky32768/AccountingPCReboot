using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace AccountingPC
{
    public partial class AccountingPCWindow
    {
        private void UpdatePCData()
        {
            PcDataSet = new DataSet();
            new SqlDataAdapter("SELECT * FROM dbo.GetAllPC()", ConnectionString).Fill(PcDataSet);
            PcDataSet.Tables[0].Columns.Add("Видеоразъемы");
            foreach (DataRow row in PcDataSet.Tables[0].Rows)
            {
                row["Видеоразъемы"] = row["VideoConnectors"].GetType() == typeof(int) ?
                    GetVideoConnectors(Convert.ToInt32(row["VideoConnectors"])) : row["VideoConnectors"];
            }
            int i = PcDataSet.Tables[0].DefaultView.Table.Columns.IndexOf("VideoConnectors");
            int ii = PcDataSet.Tables[0].DefaultView.Table.Columns.IndexOf("Видеоразъемы");
            PcDataSet.Tables[0].DefaultView.Table.Columns["Видеоразъемы"].SetOrdinal(i);
            PcDataSet.Tables[0].DefaultView.Table.Columns["VideoConnectors"].SetOrdinal(ii);
        }

        private void UpdateNotebookData()
        {
            NotebookDataSet = new DataSet();
            new SqlDataAdapter("SELECT * FROM dbo.GetAllNotebook()", ConnectionString).Fill(NotebookDataSet);
            NotebookDataSet.Tables[0].Columns.Add("Видеоразъемы");
            foreach (DataRow row in NotebookDataSet.Tables[0].Rows)
            {
                row["Видеоразъемы"] = row["VideoConnectors"].GetType() == typeof(int) ?
                    GetVideoConnectors(Convert.ToInt32(row["VideoConnectors"])) : row["VideoConnectors"];
            }
            int i = NotebookDataSet.Tables[0].DefaultView.Table.Columns.IndexOf("VideoConnectors");
            int ii = NotebookDataSet.Tables[0].DefaultView.Table.Columns.IndexOf("Видеоразъемы");
            NotebookDataSet.Tables[0].DefaultView.Table.Columns["Видеоразъемы"].SetOrdinal(i);
            NotebookDataSet.Tables[0].DefaultView.Table.Columns["VideoConnectors"].SetOrdinal(ii);
        }

        private void UpdateMonitorData()
        {
            MonitorDataSet = new DataSet();
            new SqlDataAdapter("SELECT * FROM dbo.GetAllMonitor()", ConnectionString).Fill(MonitorDataSet);
            MonitorDataSet.Tables[0].Columns.Add("Видеоразъемы");
            foreach (DataRow row in MonitorDataSet.Tables[0].Rows)
            {
                row["Видеоразъемы"] = row["VideoConnectors"].GetType() == typeof(int) ?
                    GetVideoConnectors(Convert.ToInt32(row["VideoConnectors"])) : row["VideoConnectors"];
            }
            int i = MonitorDataSet.Tables[0].DefaultView.Table.Columns.IndexOf("VideoConnectors");
            int ii = MonitorDataSet.Tables[0].DefaultView.Table.Columns.IndexOf("Видеоразъемы");
            MonitorDataSet.Tables[0].DefaultView.Table.Columns["Видеоразъемы"].SetOrdinal(i);
            MonitorDataSet.Tables[0].DefaultView.Table.Columns["VideoConnectors"].SetOrdinal(ii);
        }

        private void UpdateProjectorData()
        {
            ProjectorDataSet = new DataSet();
            new SqlDataAdapter("SELECT * FROM dbo.GetAllProjector()", ConnectionString).Fill(ProjectorDataSet);
            ProjectorDataSet.Tables[0].Columns.Add("Видеоразъемы");
            foreach (DataRow row in ProjectorDataSet.Tables[0].Rows)
            {
                row["Видеоразъемы"] = row["VideoConnectors"].GetType() == typeof(int) ?
                    GetVideoConnectors(Convert.ToInt32(row["VideoConnectors"])) : row["VideoConnectors"];
            }
            int i = ProjectorDataSet.Tables[0].DefaultView.Table.Columns.IndexOf("VideoConnectors");
            int ii = ProjectorDataSet.Tables[0].DefaultView.Table.Columns.IndexOf("Видеоразъемы");
            ProjectorDataSet.Tables[0].DefaultView.Table.Columns["Видеоразъемы"].SetOrdinal(i);
            ProjectorDataSet.Tables[0].DefaultView.Table.Columns["VideoConnectors"].SetOrdinal(ii);
        }

        private void UpdateInteractiveWhiteboardData()
        {
            BoardDataSet = new DataSet();
            new SqlDataAdapter("SELECT * FROM dbo.GetAllBoard()", ConnectionString).Fill(BoardDataSet);
        }

        private void UpdateProjectorScreenData()
        {
            ProjectorScreenDataSet = new DataSet();
            new SqlDataAdapter("SELECT * FROM dbo.GetAllScreen()", ConnectionString).Fill(ProjectorScreenDataSet);
        }

        private void UpdatePrinterScannerData()
        {
            PrinterScannerDataSet = new DataSet();
            new SqlDataAdapter("SELECT * FROM dbo.GetAllPrinterScanner()", ConnectionString).Fill(PrinterScannerDataSet);
        }

        private void UpdateNetworkSwitchData()
        {
            NetworkSwitchDataSet = new DataSet();
            new SqlDataAdapter("SELECT * FROM dbo.GetAllNetworkSwitch()", ConnectionString).Fill(NetworkSwitchDataSet);
        }

        private void UpdateOtherEquipmentData()
        {
            OtherEquipmentDataSet = new DataSet();
            new SqlDataAdapter("SELECT * FROM dbo.GetAllOtherEquipment()", ConnectionString).Fill(OtherEquipmentDataSet);
        }

        private void SetViewToPC()
        {
            equipmentView.ItemsSource = PcDataSet.Tables[0].DefaultView;

            installedSoftware.Visibility = Visibility.Visible;
            softwareColumn.Width = new GridLength(1, GridUnitType.Star);
            TypeDevice = TypeDevice.PC;
        }

        private void SetViewToNotebook()
        {
            equipmentView.ItemsSource = NotebookDataSet.Tables[0].DefaultView;

            TypeDevice = TypeDevice.Notebook;
            installedSoftware.Visibility = Visibility.Visible;
            softwareColumn.Width = new GridLength(1, GridUnitType.Star);
        }

        private void SetViewToMonitor()
        {
            equipmentView.ItemsSource = MonitorDataSet.Tables[0].DefaultView;

            TypeDevice = TypeDevice.Monitor;
            installedSoftware.Visibility = Visibility.Collapsed;
            softwareColumn.Width = GridLength.Auto;
        }

        private void SetViewToProjector()
        {
            equipmentView.ItemsSource = ProjectorDataSet.Tables[0].DefaultView;

            TypeDevice = TypeDevice.Projector;
            installedSoftware.Visibility = Visibility.Collapsed;
            softwareColumn.Width = GridLength.Auto;
        }

        private void SetViewToInteractiveWhiteboard()
        {
            equipmentView.ItemsSource = BoardDataSet.Tables[0].DefaultView;

            TypeDevice = TypeDevice.InteractiveWhiteboard;
            installedSoftware.Visibility = Visibility.Collapsed;
            softwareColumn.Width = GridLength.Auto;
        }

        private void SetViewToProjectorScreen()
        {
            equipmentView.ItemsSource = ProjectorScreenDataSet.Tables[0].DefaultView;

            TypeDevice = TypeDevice.ProjectorScreen;
            installedSoftware.Visibility = Visibility.Collapsed;
            softwareColumn.Width = GridLength.Auto;
        }

        private void SetViewToPrinterScanner()
        {
            equipmentView.ItemsSource = PrinterScannerDataSet.Tables[0].DefaultView;

            TypeDevice = TypeDevice.PrinterScanner;
            installedSoftware.Visibility = Visibility.Collapsed;
            softwareColumn.Width = GridLength.Auto;
        }

        private void SetViewToNetworkSwitch()
        {
            equipmentView.ItemsSource = NetworkSwitchDataSet.Tables[0].DefaultView;

            TypeDevice = TypeDevice.NetworkSwitch;
            installedSoftware.Visibility = Visibility.Collapsed;
            softwareColumn.Width = GridLength.Auto;
        }

        private void SetViewToOtherEquipment()
        {
            equipmentView.ItemsSource = OtherEquipmentDataSet.Tables[0].DefaultView;

            TypeDevice = TypeDevice.OtherEquipment;
            installedSoftware.Visibility = Visibility.Collapsed;
            softwareColumn.Width = GridLength.Auto;
        }
    }
}
