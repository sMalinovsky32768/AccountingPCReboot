using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Controls;

namespace AccountingPC
{
    public partial class AccountingPCWindow
    {
        private void UpdateSourceForAspectRatio(String connectionString)
        {
            aspectRatioDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllAspectRatio()", connectionString);
            aspectRatioDataSet = new DataSet();
            aspectRatioDataAdapter.Fill(aspectRatioDataSet);
            aspectRatio.ItemsSource = aspectRatioDataSet.Tables[0].DefaultView;
            aspectRatio.DisplayMemberPath = "Name";
        }

        private void UpdateSourceForOS(String connectionString)
        {
            osDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllOS()", connectionString);
            osDataSet = new DataSet();
            osDataAdapter.Fill(osDataSet);
            os.ItemsSource = osDataSet.Tables[0].DefaultView;
            os.DisplayMemberPath = "Наименование";
        }

        private void UpdateSourceForScreenInstalled(String connectionString)
        {
            screenInstalledDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllScreenInstalled()", connectionString);
            screenInstalledDataSet = new DataSet();
            screenInstalledDataAdapter.Fill(screenInstalledDataSet);
            screenInstalled.ItemsSource = screenInstalledDataSet.Tables[0].DefaultView;
            screenInstalled.DisplayMemberPath = "Name";
        }

        private void UpdateSourceForName(String connectionString)
        {
            switch (typeDevice)
            {
                case TypeDevice.InteractiveWhiteboard:
                    nameDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllBoardName()", connectionString);
                    break;
                case TypeDevice.Monitor:
                    nameDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllMonitorName()", connectionString);
                    break;
                case TypeDevice.NetworkSwitch:
                    nameDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllNetworkSwitchName()", connectionString);
                    break;
                case TypeDevice.Notebook:
                    nameDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllNotebookName()", connectionString);
                    break;
                case TypeDevice.OtherEquipment:
                    nameDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllOtherEquipmentName()", connectionString);
                    break;
                case TypeDevice.PC:
                    nameDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllPCName()", connectionString);
                    break;
                case TypeDevice.PrinterScanner:
                    nameDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllPrinterScannerName()", connectionString);
                    break;
                case TypeDevice.Projector:
                    nameDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllProjectorName()", connectionString);
                    break;
                case TypeDevice.ProjectorScreen:
                    nameDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllProjectorScreenName()", connectionString);
                    break;
            }

            nameDataSet = new DataSet();
            nameDataAdapter.Fill(nameDataSet);
            name.ItemsSource = nameDataSet.Tables[0].DefaultView;
            name.DisplayMemberPath = "Name";
        }

        private void UpdateSourceForLocation(String connectionString)
        {
            switch (typeDevice)
            {
                case TypeDevice.InteractiveWhiteboard:
                    locationDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllLocationByTypeDeviceID(4)", connectionString);
                    break;
                case TypeDevice.Monitor:
                    locationDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllLocationByTypeDeviceID(6)", connectionString);
                    break;
                case TypeDevice.NetworkSwitch:
                    locationDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllLocationByTypeDeviceID(5)", connectionString);
                    break;
                case TypeDevice.Notebook:
                    locationDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllLocationByTypeDeviceID(2)", connectionString);
                    break;
                case TypeDevice.OtherEquipment:
                    locationDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllLocationByTypeDeviceID(9)", connectionString);
                    break;
                case TypeDevice.PC:
                    locationDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllLocationByTypeDeviceID(1)", connectionString);
                    break;
                case TypeDevice.PrinterScanner:
                    locationDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllLocationByTypeDeviceID(3)", connectionString);
                    break;
                case TypeDevice.Projector:
                    locationDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllLocationByTypeDeviceID(7)", connectionString);
                    break;
                case TypeDevice.ProjectorScreen:
                    locationDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllLocationByTypeDeviceID(8)", connectionString);
                    break;
            }

            locationDataSet = new DataSet();
            locationDataAdapter.Fill(locationDataSet);
            location.ItemsSource = locationDataSet.Tables[0].DefaultView;
            location.DisplayMemberPath = "Place";
        }

        private void UpdateSourceForCPU(String connectionString)
        {
            switch (typeDevice)
            {
                case TypeDevice.Notebook:
                    cpuDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllNotebookCPU()", connectionString);
                    cpuDataSet = new DataSet();
                    cpuDataAdapter.Fill(cpuDataSet);
                    cpu.ItemsSource = cpuDataSet.Tables[0].DefaultView;
                    cpu.DisplayMemberPath = "CPUModel";
                    break;
                case TypeDevice.PC:
                    cpuDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllPCCPU()", connectionString);
                    cpuDataSet = new DataSet();
                    cpuDataAdapter.Fill(cpuDataSet);
                    cpu.ItemsSource = cpuDataSet.Tables[0].DefaultView;
                    cpu.DisplayMemberPath = "CPUModel";
                    break;
            }
        }

        private void UpdateSourceForVideoCard(String connectionString)
        {
            switch (typeDevice)
            {
                case TypeDevice.Notebook:
                    vCardDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllNotebookvCard()", connectionString);
                    vCardDataSet = new DataSet();
                    vCardDataAdapter.Fill(vCardDataSet);
                    vCard.ItemsSource = vCardDataSet.Tables[0].DefaultView;
                    vCard.DisplayMemberPath = "VideoCard";
                    break;
                case TypeDevice.PC:
                    vCardDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllPCvCard()", connectionString);
                    vCardDataSet = new DataSet();
                    vCardDataAdapter.Fill(vCardDataSet);
                    vCard.ItemsSource = vCardDataSet.Tables[0].DefaultView;
                    vCard.DisplayMemberPath = "VideoCard";
                    break;
            }
        }

        private void UpdateSourceForType(String connectionString)
        {
            switch (typeDevice)
            {
                case TypeDevice.NetworkSwitch:
                    typeNetworkSwitchDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllTypeNetworkSwitch()", connectionString);
                    typeNetworkSwitchDataSet = new DataSet();
                    typeNetworkSwitchDataAdapter.Fill(typeNetworkSwitchDataSet);
                    type.ItemsSource = typeNetworkSwitchDataSet.Tables[0].DefaultView;
                    type.DisplayMemberPath = "Name";
                    break;
                case TypeDevice.Notebook:
                    typeNotebookDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllTypeNotebook()", connectionString);
                    typeNotebookDataSet = new DataSet();
                    typeNotebookDataAdapter.Fill(typeNotebookDataSet);
                    type.ItemsSource = typeNotebookDataSet.Tables[0].DefaultView;
                    type.DisplayMemberPath = "Name";
                    break;
                case TypeDevice.PrinterScanner:
                    typePrinterDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllTypePrinter()", connectionString);
                    typePrinterDataSet = new DataSet();
                    typePrinterDataAdapter.Fill(typePrinterDataSet);
                    type.ItemsSource = typePrinterDataSet.Tables[0].DefaultView;
                    type.DisplayMemberPath = "Name";
                    break;
            }
        }

        private void UpdateSourceForFrequency(String connectionString)
        {
            frequencyDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllFrequency()", connectionString);
            frequencyDataSet = new DataSet();
            frequencyDataAdapter.Fill(frequencyDataSet);
            screenFrequency.ItemsSource = frequencyDataSet.Tables[0].DefaultView;
            screenFrequency.DisplayMemberPath = "Name";
        }

        private void UpdateSourceForMatrixTechology(String connectionString)
        {
            matrixTechnologyDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllMatrixTechnology()", connectionString);
            matrixTechnologyDataSet = new DataSet();
            matrixTechnologyDataAdapter.Fill(matrixTechnologyDataSet);
            matrixTechnology.ItemsSource = matrixTechnologyDataSet.Tables[0].DefaultView;
            matrixTechnology.DisplayMemberPath = "Name";
        }

        private void UpdateSourceForPaperSize(String connectionString)
        {
            paperSizeDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllPaperSize()", connectionString);
            paperSizeDataSet = new DataSet();
            paperSizeDataAdapter.Fill(paperSizeDataSet);
            paperSize.ItemsSource = paperSizeDataSet.Tables[0].DefaultView;
            paperSize.DisplayMemberPath = "Name";
        }

        private void UpdateSourceForProjectorTechnology(String connectionString)
        {
            projectorTechnologyDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllProjectorTechnology()", connectionString);
            projectorTechnologyDataSet = new DataSet();
            projectorTechnologyDataAdapter.Fill(projectorTechnologyDataSet);
            projectorTechnology.ItemsSource = projectorTechnologyDataSet.Tables[0].DefaultView;
            projectorTechnology.DisplayMemberPath = "Name";
        }

        private void UpdateSourceForResolution(String connectionString)
        {
            resolutionDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllResolution()", connectionString);
            resolutionDataSet = new DataSet();
            resolutionDataAdapter.Fill(resolutionDataSet);
            resolution.ItemsSource = resolutionDataSet.Tables[0].DefaultView;
            resolution.DisplayMemberPath = "Name";
        }

        private void UpdateSourceForVideoConnectors(String connectionString)
        {
            //videoConnectorsDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllVideoConnector()", connectionString);
            //videoConnectorsDataSet = new DataSet();
            //videoConnectorsDataAdapter.Fill(videoConnectorsDataSet);
            //vConnectors.ItemsSource = videoConnectorsDataSet.Tables[0].DefaultView;
            //vConnectors.DisplayMemberPath = "Name";

            videoConnectorsDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllVideoConnector()", connectionString);
            videoConnectorsDataSet = new DataSet();
            videoConnectorsDataAdapter.Fill(videoConnectorsDataSet);
            videoConnectorsItems = new List<ListBoxItem>();
            foreach (DataRowView row in videoConnectorsDataSet.Tables[0].DefaultView)
                videoConnectorsItems.Add(new ListBoxItem() { Content = row.Row[1].ToString() });
            vConnectors.ItemsSource = videoConnectorsItems;
            //vConnectors.ItemsSource = videoConnectorsDataSet.Tables[0].DefaultView;
            //vConnectors.DisplayMemberPath = "Name";
        }

        private void UpdateSourceForWifiFrequency(String connectionString)
        {
            wifiFrequencyDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllWiFiFrequency()", connectionString);
            wifiFrequencyDataSet = new DataSet();
            wifiFrequencyDataAdapter.Fill(wifiFrequencyDataSet);
            wifiFrequency.ItemsSource = wifiFrequencyDataSet.Tables[0].DefaultView;
            wifiFrequency.DisplayMemberPath = "Name";
        }

        private void UpdateSourceForMotherboard(String connectionString)
        {
            motherboardDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllMotherboard()", connectionString);
            motherboardDataSet = new DataSet();
            motherboardDataAdapter.Fill(motherboardDataSet);
            motherboard.ItemsSource = motherboardDataSet.Tables[0].DefaultView;
            motherboard.DisplayMemberPath = "Name";
        }
    }
}
