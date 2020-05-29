using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace AccountingPC
{
    public partial class AccountingPCWindow
    {
        private void UpdatePCData()
        {
            pcDataAdapter = new SqlDataAdapter("SELECT * FROM dbo.GetAllPC()", ConnectionString);
            pcDataSet = new DataSet();
            pcDataAdapter.Fill(pcDataSet);
            pcDataSet.Tables[0].Columns.Add("Видеоразъемы");
            //foreach (DataRow row in pcDataSet.Tables[0].Rows)
            //{
            //    row[20] = row[16].GetType() == typeof(int) ? GetVideoConnectors(Convert.ToInt32(row[16])) : row[16];
            //}
            foreach (DataRow row in pcDataSet.Tables[0].Rows)
            {
                row["Видеоразъемы"] = row["VideoConnectors"].GetType() == typeof(int) ? 
                    GetVideoConnectors(Convert.ToInt32(row["VideoConnectors"])) : row["VideoConnectors"];
            }
            int i = pcDataSet.Tables[0].Columns.IndexOf("VideoConnectors");
            int ii = pcDataSet.Tables[0].Columns.IndexOf("Видеоразъемы");
            pcDataSet.Tables[0].Columns["Видеоразъемы"].SetOrdinal(i);
            pcDataSet.Tables[0].Columns["VideoConnectors"].SetOrdinal(ii);
        }

        private void UpdateNotebookData()
        {
            notebookDataAdapter = new SqlDataAdapter("SELECT * FROM dbo.GetAllNotebook()", ConnectionString);
            notebookDataSet = new DataSet();
            notebookDataAdapter.Fill(notebookDataSet);
            notebookDataSet.Tables[0].Columns.Add("Видеоразъемы");
            foreach (DataRow row in notebookDataSet.Tables[0].Rows)
            {
                row["Видеоразъемы"] = row["VideoConnectors"].GetType() == typeof(int) ? 
                    GetVideoConnectors(Convert.ToInt32(row["VideoConnectors"])) : row["VideoConnectors"];
            }
            //int i = pcDataSet.Tables[0].Columns.IndexOf("VideoConnectors");
            //int ii = pcDataSet.Tables[0].Columns.IndexOf("Видеоразъемы");
            //pcDataSet.Tables[0].Columns["Видеоразъемы"].SetOrdinal(i);
            //pcDataSet.Tables[0].Columns["VideoConnectors"].SetOrdinal(ii);
            int i = notebookDataSet.Tables[0].DefaultView.Table.Columns.IndexOf("VideoConnectors");
            int ii = notebookDataSet.Tables[0].DefaultView.Table.Columns.IndexOf("Видеоразъемы");
            notebookDataSet.Tables[0].DefaultView.Table.Columns["Видеоразъемы"].SetOrdinal(i);
            notebookDataSet.Tables[0].DefaultView.Table.Columns["VideoConnectors"].SetOrdinal(ii);
        }

        private void UpdateMonitorData()
        {
            monitorDataAdapter = new SqlDataAdapter("SELECT * FROM dbo.GetAllMonitor()", ConnectionString);
            monitorDataSet = new DataSet();
            monitorDataAdapter.Fill(monitorDataSet);
            monitorDataSet.Tables[0].Columns.Add("Видеоразъемы");
            foreach (DataRow row in monitorDataSet.Tables[0].Rows)
            {
                row["Видеоразъемы"] = row["VideoConnectors"].GetType() == typeof(int) ?
                    GetVideoConnectors(Convert.ToInt32(row["VideoConnectors"])) : row["VideoConnectors"];
            }
            int i = monitorDataSet.Tables[0].DefaultView.Table.Columns.IndexOf("VideoConnectors");
            int ii = monitorDataSet.Tables[0].DefaultView.Table.Columns.IndexOf("Видеоразъемы");
            monitorDataSet.Tables[0].DefaultView.Table.Columns["Видеоразъемы"].SetOrdinal(i);
            monitorDataSet.Tables[0].DefaultView.Table.Columns["VideoConnectors"].SetOrdinal(ii);
        }

        private void UpdateProjectorData()
        {
            projectorDataAdapter = new SqlDataAdapter("SELECT * FROM dbo.GetAllProjector()", ConnectionString);
            projectorDataSet = new DataSet();
            projectorDataAdapter.Fill(projectorDataSet);
            projectorDataSet.Tables[0].Columns.Add("Видеоразъемы");
            foreach (DataRow row in projectorDataSet.Tables[0].Rows)
            {
                row["Видеоразъемы"] = row["VideoConnectors"].GetType() == typeof(int) ?
                    GetVideoConnectors(Convert.ToInt32(row["VideoConnectors"])) : row["VideoConnectors"];
            }
            int i = projectorDataSet.Tables[0].DefaultView.Table.Columns.IndexOf("VideoConnectors");
            int ii = projectorDataSet.Tables[0].DefaultView.Table.Columns.IndexOf("Видеоразъемы");
            projectorDataSet.Tables[0].DefaultView.Table.Columns["Видеоразъемы"].SetOrdinal(i);
            projectorDataSet.Tables[0].DefaultView.Table.Columns["VideoConnectors"].SetOrdinal(ii);
        }

        private void UpdateInteractiveWhiteboardData()
        {
            boardDataAdapter = new SqlDataAdapter("SELECT * FROM dbo.GetAllBoard()", ConnectionString);
            boardDataSet = new DataSet();
            boardDataAdapter.Fill(boardDataSet);
        }

        private void UpdateProjectorScreenData()
        {
            projectorScreenDataAdapter = new SqlDataAdapter("SELECT * FROM dbo.GetAllScreen()", ConnectionString);
            projectorScreenDataSet = new DataSet();
            projectorScreenDataAdapter.Fill(projectorScreenDataSet);
        }

        private void UpdatePrinterScannerData()
        {
            printerScannerDataAdapter = new SqlDataAdapter("SELECT * FROM dbo.GetAllPrinterScanner()", ConnectionString);
            printerScannerDataSet = new DataSet();
            printerScannerDataAdapter.Fill(printerScannerDataSet);
        }

        private void UpdateNetworkSwitchData()
        {
            networkSwitchDataAdapter = new SqlDataAdapter("SELECT * FROM dbo.GetAllNetworkSwitch()", ConnectionString);
            networkSwitchDataSet = new DataSet();
            networkSwitchDataAdapter.Fill(networkSwitchDataSet);
        }

        private void UpdateOtherEquipmentData()
        {
            otherEquipmentDataAdapter = new SqlDataAdapter("SELECT * FROM dbo.GetAllOtherEquipment()", ConnectionString);
            otherEquipmentDataSet = new DataSet();
            otherEquipmentDataAdapter.Fill(otherEquipmentDataSet);
        }

        private void SetViewToPC()
        {
            equipmentView.ItemsSource = pcDataSet.Tables[0].DefaultView;
            if (equipmentView.Columns.Count > 0)
            {
                equipmentView.Columns[pcDataSet.Tables[0].DefaultView.Table.Columns.IndexOf("ID")].Visibility = Visibility.Collapsed;
                equipmentView.Columns[pcDataSet.Tables[0].DefaultView.Table.Columns.IndexOf("VideoConnectors")].Visibility = Visibility.Collapsed;
                equipmentView.Columns[pcDataSet.Tables[0].DefaultView.Table.Columns.IndexOf("ImageID")].Visibility = Visibility.Collapsed;
                ((DataGridTextColumn)equipmentView.Columns[pcDataSet.Tables[0].DefaultView.Table.
                    Columns.IndexOf("Дата приобретения")]).Binding.StringFormat = "dd.MM.yyyy";

                //equipmentView.Columns[equipmentView.Columns.Count - 2].Visibility = Visibility.Collapsed;
            }
            installedSoftware.Visibility = Visibility.Visible;
            softwareColumn.Width = new GridLength(1, GridUnitType.Star);
            TypeDevice = TypeDevice.PC;
        }

        private void SetViewToNotebook()
        {
            equipmentView.ItemsSource = notebookDataSet.Tables[0].DefaultView;

            equipmentView.Columns[notebookDataSet.Tables[0].DefaultView.Table.Columns.IndexOf("ID")].Visibility = Visibility.Collapsed;
            equipmentView.Columns[notebookDataSet.Tables[0].DefaultView.Table.Columns.IndexOf("VideoConnectors")].Visibility = Visibility.Collapsed;
            equipmentView.Columns[notebookDataSet.Tables[0].DefaultView.Table.Columns.IndexOf("ImageID")].Visibility = Visibility.Collapsed;
            ((DataGridTextColumn)equipmentView.Columns[notebookDataSet.Tables[0].DefaultView.Table.
                Columns.IndexOf("Дата приобретения")]).Binding.StringFormat = "dd.MM.yyyy";

            TypeDevice = TypeDevice.Notebook;
            installedSoftware.Visibility = Visibility.Visible;
            softwareColumn.Width = new GridLength(1, GridUnitType.Star);
        }

        private void SetViewToMonitor()
        {
            equipmentView.ItemsSource = monitorDataSet.Tables[0].DefaultView;

            equipmentView.Columns[monitorDataSet.Tables[0].DefaultView.Table.Columns.IndexOf("ID")].Visibility = Visibility.Collapsed;
            equipmentView.Columns[monitorDataSet.Tables[0].DefaultView.Table.Columns.IndexOf("VideoConnectors")].Visibility = Visibility.Collapsed;
            equipmentView.Columns[monitorDataSet.Tables[0].DefaultView.Table.Columns.IndexOf("ImageID")].Visibility = Visibility.Collapsed;
            ((DataGridTextColumn)equipmentView.Columns[monitorDataSet.Tables[0].DefaultView.Table.
                Columns.IndexOf("Дата приобретения")]).Binding.StringFormat = "dd.MM.yyyy";

            TypeDevice = TypeDevice.Monitor;
            installedSoftware.Visibility = Visibility.Collapsed;
            softwareColumn.Width = GridLength.Auto;
        }

        private void SetViewToProjector()
        {
            equipmentView.ItemsSource = projectorDataSet.Tables[0].DefaultView;

            equipmentView.Columns[projectorDataSet.Tables[0].DefaultView.Table.Columns.IndexOf("ID")].Visibility = Visibility.Collapsed;
            equipmentView.Columns[projectorDataSet.Tables[0].DefaultView.Table.Columns.IndexOf("VideoConnectors")].Visibility = Visibility.Collapsed;
            equipmentView.Columns[projectorDataSet.Tables[0].DefaultView.Table.Columns.IndexOf("ImageID")].Visibility = Visibility.Collapsed;
            ((DataGridTextColumn)equipmentView.Columns[projectorDataSet.Tables[0].DefaultView.Table.
                Columns.IndexOf("Дата приобретения")]).Binding.StringFormat = "dd.MM.yyyy";

            TypeDevice = TypeDevice.Projector;
            installedSoftware.Visibility = Visibility.Collapsed;
            softwareColumn.Width = GridLength.Auto;
        }

        private void SetViewToInteractiveWhiteboard()
        {
            equipmentView.ItemsSource = boardDataSet.Tables[0].DefaultView;

            equipmentView.Columns[boardDataSet.Tables[0].DefaultView.Table.Columns.IndexOf("ID")].Visibility = Visibility.Collapsed;
            equipmentView.Columns[boardDataSet.Tables[0].DefaultView.Table.Columns.IndexOf("ImageID")].Visibility = Visibility.Collapsed;
            ((DataGridTextColumn)equipmentView.Columns[boardDataSet.Tables[0].DefaultView.Table.
                Columns.IndexOf("Дата приобретения")]).Binding.StringFormat = "dd.MM.yyyy";

            TypeDevice = TypeDevice.InteractiveWhiteboard;
            installedSoftware.Visibility = Visibility.Collapsed;
            softwareColumn.Width = GridLength.Auto;
        }

        private void SetViewToProjectorScreen()
        {
            equipmentView.ItemsSource = projectorScreenDataSet.Tables[0].DefaultView;

            equipmentView.Columns[projectorScreenDataSet.Tables[0].DefaultView.Table.Columns.IndexOf("ID")].Visibility = Visibility.Collapsed;
            equipmentView.Columns[projectorScreenDataSet.Tables[0].DefaultView.Table.Columns.IndexOf("ImageID")].Visibility = Visibility.Collapsed;
            ((DataGridTextColumn)equipmentView.Columns[projectorScreenDataSet.Tables[0].DefaultView.Table.
                Columns.IndexOf("Дата приобретения")]).Binding.StringFormat = "dd.MM.yyyy";

            TypeDevice = TypeDevice.ProjectorScreen;
            installedSoftware.Visibility = Visibility.Collapsed;
            softwareColumn.Width = GridLength.Auto;
        }

        private void SetViewToPrinterScanner()
        {
            equipmentView.ItemsSource = printerScannerDataSet.Tables[0].DefaultView;

            equipmentView.Columns[printerScannerDataSet.Tables[0].DefaultView.Table.Columns.IndexOf("ID")].Visibility = Visibility.Collapsed;
            equipmentView.Columns[printerScannerDataSet.Tables[0].DefaultView.Table.Columns.IndexOf("ImageID")].Visibility = Visibility.Collapsed;
            ((DataGridTextColumn)equipmentView.Columns[printerScannerDataSet.Tables[0].DefaultView.Table.
                Columns.IndexOf("Дата приобретения")]).Binding.StringFormat = "dd.MM.yyyy";

            TypeDevice = TypeDevice.PrinterScanner;
            installedSoftware.Visibility = Visibility.Collapsed;
            softwareColumn.Width = GridLength.Auto;
        }

        private void SetViewToNetworkSwitch()
        {
            equipmentView.ItemsSource = networkSwitchDataSet.Tables[0].DefaultView;

            equipmentView.Columns[networkSwitchDataSet.Tables[0].DefaultView.Table.Columns.IndexOf("ID")].Visibility = Visibility.Collapsed;
            equipmentView.Columns[networkSwitchDataSet.Tables[0].DefaultView.Table.Columns.IndexOf("ImageID")].Visibility = Visibility.Collapsed;
            ((DataGridTextColumn)equipmentView.Columns[networkSwitchDataSet.Tables[0].DefaultView.Table.
                Columns.IndexOf("Дата приобретения")]).Binding.StringFormat = "dd.MM.yyyy";

            TypeDevice = TypeDevice.NetworkSwitch;
            installedSoftware.Visibility = Visibility.Collapsed;
            softwareColumn.Width = GridLength.Auto;
        }

        private void SetViewToOtherEquipment()
        {
            equipmentView.ItemsSource = otherEquipmentDataSet.Tables[0].DefaultView;

            equipmentView.Columns[otherEquipmentDataSet.Tables[0].DefaultView.Table.Columns.IndexOf("ID")].Visibility = Visibility.Collapsed;
            equipmentView.Columns[otherEquipmentDataSet.Tables[0].DefaultView.Table.Columns.IndexOf("ImageID")].Visibility = Visibility.Collapsed;
            ((DataGridTextColumn)equipmentView.Columns[otherEquipmentDataSet.Tables[0].DefaultView.Table.
                Columns.IndexOf("Дата приобретения")]).Binding.StringFormat = "dd.MM.yyyy";

            TypeDevice = TypeDevice.OtherEquipment;
            installedSoftware.Visibility = Visibility.Collapsed;
            softwareColumn.Width = GridLength.Auto;
        }
    }
}
