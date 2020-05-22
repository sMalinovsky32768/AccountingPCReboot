using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace AccountingPC
{
    public partial class AccountingPCWindow
    {
        private void UpdatePCData(String connectionString)
        {
            pcDataAdapter = new SqlDataAdapter("SELECT * FROM dbo.GetAllPC()", connectionString);
            pcDataSet = new DataSet();
            pcDataAdapter.Fill(pcDataSet);
            pcDataSet.Tables[0].Columns.Add("Видеоразъемы");
            foreach (DataRow row in pcDataSet.Tables[0].Rows)
            {
                row[20] = row[16].GetType() == typeof(int) ? GetVideoConnectors(Convert.ToInt32(row[16])) : row[16];
            }
        }

        private void UpdateNotebookData(String connectionString)
        {
            notebookDataAdapter = new SqlDataAdapter("SELECT * FROM dbo.GetAllNotebook()", connectionString);
            notebookDataSet = new DataSet();
            notebookDataAdapter.Fill(notebookDataSet);
            notebookDataSet.Tables[0].Columns.Add("Видеоразъемы");
            foreach (DataRow row in notebookDataSet.Tables[0].Rows)
            {
                row[24] = row[19].GetType() == typeof(int) ? GetVideoConnectors(Convert.ToInt32(row[19])) : row[19];
            }
        }

        private void UpdateMonitorData(String connectionString)
        {
            monitorDataAdapter = new SqlDataAdapter("SELECT * FROM dbo.GetAllMonitor()", connectionString);
            monitorDataSet = new DataSet();
            monitorDataAdapter.Fill(monitorDataSet);
            monitorDataSet.Tables[0].Columns.Add("Видеоразъемы");
            foreach (DataRow row in monitorDataSet.Tables[0].Rows)
            {
                row[12] = row[9].GetType() == typeof(int) ? GetVideoConnectors(Convert.ToInt32(row[9])) : row[9];
            }
        }

        private void UpdateProjectorData(String connectionString)
        {
            projectorDataAdapter = new SqlDataAdapter("SELECT * FROM dbo.GetAllProjector()", connectionString);
            projectorDataSet = new DataSet();
            projectorDataAdapter.Fill(projectorDataSet);
            projectorDataSet.Tables[0].Columns.Add("Видеоразъемы");
            foreach (DataRow row in projectorDataSet.Tables[0].Rows)
            {
                row[11] = row[8].GetType() == typeof(int) ? GetVideoConnectors(Convert.ToInt32(row[8])) : row[8];
            }
        }

        private void UpdateInteractiveWhiteboardData(String connectionString)
        {
            boardDataAdapter = new SqlDataAdapter("SELECT * FROM dbo.GetAllBoard()", connectionString);
            boardDataSet = new DataSet();
            boardDataAdapter.Fill(boardDataSet);
        }

        private void UpdateProjectorScreenData(String connectionString)
        {
            projectorScreenDataAdapter = new SqlDataAdapter("SELECT * FROM dbo.GetAllScreen()", connectionString);
            projectorScreenDataSet = new DataSet();
            projectorScreenDataAdapter.Fill(projectorScreenDataSet);
        }

        private void UpdatePrinterScannerData(String connectionString)
        {
            printerScannerDataAdapter = new SqlDataAdapter("SELECT * FROM dbo.GetAllPrinterScanner()", connectionString);
            printerScannerDataSet = new DataSet();
            printerScannerDataAdapter.Fill(printerScannerDataSet);
        }

        private void UpdateNetworkSwitchData(String connectionString)
        {
            networkSwitchDataAdapter = new SqlDataAdapter("SELECT * FROM dbo.GetAllNetworkSwitch()", connectionString);
            networkSwitchDataSet = new DataSet();
            networkSwitchDataAdapter.Fill(networkSwitchDataSet);
        }

        private void UpdateOtherEquipmentData(String connectionString)
        {
            otherEquipmentDataAdapter = new SqlDataAdapter("SELECT * FROM dbo.GetAllOtherEquipment()", connectionString);
            otherEquipmentDataSet = new DataSet();
            otherEquipmentDataAdapter.Fill(otherEquipmentDataSet);
        }

        private void SetViewToPC()
        {
            equipmentView.ItemsSource = pcDataSet.Tables[0].DefaultView;
            if (equipmentView.Columns.Count > 0)
            {
                equipmentView.Columns[0].Visibility = Visibility.Collapsed;
                equipmentView.Columns[16].Visibility = Visibility.Collapsed;
                //equipmentView.Columns[equipmentView.Columns.Count - 1].Visibility = Visibility.Collapsed;
                equipmentView.Columns[equipmentView.Columns.Count - 2].Visibility = Visibility.Collapsed;
            }
            installedSoftware.Visibility = Visibility.Visible;
            softwareColumn.Width = new GridLength(1, GridUnitType.Star);
            typeDevice = TypeDevice.PC;
        }

        private void SetViewToNotebook()
        {
            equipmentView.ItemsSource = notebookDataSet.Tables[0].DefaultView;
            equipmentView.Columns[0].Visibility = Visibility.Collapsed;
            equipmentView.Columns[19].Visibility = Visibility.Collapsed;
            //equipmentView.Columns[equipmentView.Columns.Count - 1].Visibility = Visibility.Collapsed;
            equipmentView.Columns[equipmentView.Columns.Count - 2].Visibility = Visibility.Collapsed;
            typeDevice = TypeDevice.Notebook;
            installedSoftware.Visibility = Visibility.Visible;
            softwareColumn.Width = new GridLength(1, GridUnitType.Star);
        }

        private void SetViewToMonitor()
        {
            equipmentView.ItemsSource = monitorDataSet.Tables[0].DefaultView;
            equipmentView.Columns[0].Visibility = Visibility.Collapsed;
            equipmentView.Columns[9].Visibility = Visibility.Collapsed;
            //equipmentView.Columns[equipmentView.Columns.Count - 1].Visibility = Visibility.Collapsed;
            equipmentView.Columns[equipmentView.Columns.Count - 2].Visibility = Visibility.Collapsed;
            typeDevice = TypeDevice.Monitor;
            installedSoftware.Visibility = Visibility.Hidden;
            softwareColumn.Width = GridLength.Auto;
        }

        private void SetViewToProjector()
        {
            equipmentView.ItemsSource = projectorDataSet.Tables[0].DefaultView;
            equipmentView.Columns[0].Visibility = Visibility.Collapsed;
            equipmentView.Columns[8].Visibility = Visibility.Collapsed;
            //equipmentView.Columns[equipmentView.Columns.Count - 1].Visibility = Visibility.Collapsed;
            equipmentView.Columns[equipmentView.Columns.Count - 2].Visibility = Visibility.Collapsed;
            typeDevice = TypeDevice.Projector;
            installedSoftware.Visibility = Visibility.Hidden;
            softwareColumn.Width = GridLength.Auto;
        }

        private void SetViewToInteractiveWhiteboard()
        {
            equipmentView.ItemsSource = boardDataSet.Tables[0].DefaultView;
            equipmentView.Columns[0].Visibility = Visibility.Collapsed;
            equipmentView.Columns[equipmentView.Columns.Count - 1].Visibility = Visibility.Collapsed;
            typeDevice = TypeDevice.InteractiveWhiteboard;
            installedSoftware.Visibility = Visibility.Hidden;
            softwareColumn.Width = GridLength.Auto;
        }

        private void SetViewToProjectorScreen()
        {
            equipmentView.ItemsSource = projectorScreenDataSet.Tables[0].DefaultView;
            equipmentView.Columns[0].Visibility = Visibility.Collapsed;
            equipmentView.Columns[equipmentView.Columns.Count - 1].Visibility = Visibility.Collapsed;
            typeDevice = TypeDevice.ProjectorScreen;
            installedSoftware.Visibility = Visibility.Hidden;
            softwareColumn.Width = GridLength.Auto;
        }

        private void SetViewToPrinterScanner()
        {
            equipmentView.ItemsSource = printerScannerDataSet.Tables[0].DefaultView;
            equipmentView.Columns[0].Visibility = Visibility.Collapsed;
            equipmentView.Columns[equipmentView.Columns.Count - 1].Visibility = Visibility.Collapsed;
            typeDevice = TypeDevice.PrinterScanner;
            installedSoftware.Visibility = Visibility.Hidden;
            softwareColumn.Width = GridLength.Auto;
        }

        private void SetViewToNetworkSwitch()
        {
            equipmentView.ItemsSource = networkSwitchDataSet.Tables[0].DefaultView;
            equipmentView.Columns[0].Visibility = Visibility.Collapsed;
            equipmentView.Columns[equipmentView.Columns.Count - 1].Visibility = Visibility.Collapsed;
            typeDevice = TypeDevice.NetworkSwitch;
            installedSoftware.Visibility = Visibility.Hidden;
            softwareColumn.Width = GridLength.Auto;
        }

        private void SetViewToOtherEquipment()
        {
            equipmentView.ItemsSource = otherEquipmentDataSet.Tables[0].DefaultView;
            equipmentView.Columns[0].Visibility = Visibility.Collapsed;
            equipmentView.Columns[equipmentView.Columns.Count - 1].Visibility = Visibility.Collapsed;
            typeDevice = TypeDevice.OtherEquipment;
            installedSoftware.Visibility = Visibility.Hidden;
            softwareColumn.Width = GridLength.Auto;
        }
    }
}
