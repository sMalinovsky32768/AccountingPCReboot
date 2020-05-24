using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Controls;

namespace AccountingPC
{
    public partial class AccountingPCWindow
    {
        private void UpdateSourceForAspectRatio()
        {
            aspectRatioDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllAspectRatio()", ConnectionString);
            aspectRatioDataSet = new DataSet();
            aspectRatioDataAdapter.Fill(aspectRatioDataSet);
            aspectRatio.ItemsSource = aspectRatioDataSet.Tables[0].DefaultView;
            aspectRatio.DisplayMemberPath = "Name";
        }

        private void UpdateSourceForOS()
        {
            osDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllOS()", ConnectionString);
            osDataSet = new DataSet();
            osDataAdapter.Fill(osDataSet);
            os.ItemsSource = osDataSet.Tables[0].DefaultView;
            os.DisplayMemberPath = "Наименование";
        }

        private void UpdateSourceForScreenInstalled()
        {
            screenInstalledDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllScreenInstalled()", ConnectionString);
            screenInstalledDataSet = new DataSet();
            screenInstalledDataAdapter.Fill(screenInstalledDataSet);
            screenInstalled.ItemsSource = screenInstalledDataSet.Tables[0].DefaultView;
            screenInstalled.DisplayMemberPath = "Name";
        }

        private void UpdateSourceForName()
        {
            switch (TypeDevice)
            {
                case TypeDevice.InteractiveWhiteboard:
                    nameDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllBoardName()", ConnectionString);
                    break;
                case TypeDevice.Monitor:
                    nameDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllMonitorName()", ConnectionString);
                    break;
                case TypeDevice.NetworkSwitch:
                    nameDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllNetworkSwitchName()", ConnectionString);
                    break;
                case TypeDevice.Notebook:
                    nameDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllNotebookName()", ConnectionString);
                    break;
                case TypeDevice.OtherEquipment:
                    nameDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllOtherEquipmentName()", ConnectionString);
                    break;
                case TypeDevice.PC:
                    nameDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllPCName()", ConnectionString);
                    break;
                case TypeDevice.PrinterScanner:
                    nameDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllPrinterScannerName()", ConnectionString);
                    break;
                case TypeDevice.Projector:
                    nameDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllProjectorName()", ConnectionString);
                    break;
                case TypeDevice.ProjectorScreen:
                    nameDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllProjectorScreenName()", ConnectionString);
                    break;
            }

            nameDataSet = new DataSet();
            nameDataAdapter.Fill(nameDataSet);
            name.ItemsSource = nameDataSet.Tables[0].DefaultView;
            name.DisplayMemberPath = "Name";
        }

        private void UpdateSourceForLocation()
        {
            switch (TypeDevice)
            {
                case TypeDevice.InteractiveWhiteboard:
                    locationDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllLocationByTypeDeviceID(4)", ConnectionString);
                    break;
                case TypeDevice.Monitor:
                    locationDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllLocationByTypeDeviceID(6)", ConnectionString);
                    break;
                case TypeDevice.NetworkSwitch:
                    locationDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllLocationByTypeDeviceID(5)", ConnectionString);
                    break;
                case TypeDevice.Notebook:
                    locationDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllLocationByTypeDeviceID(2)", ConnectionString);
                    break;
                case TypeDevice.OtherEquipment:
                    locationDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllLocationByTypeDeviceID(9)", ConnectionString);
                    break;
                case TypeDevice.PC:
                    locationDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllLocationByTypeDeviceID(1)", ConnectionString);
                    break;
                case TypeDevice.PrinterScanner:
                    locationDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllLocationByTypeDeviceID(3)", ConnectionString);
                    break;
                case TypeDevice.Projector:
                    locationDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllLocationByTypeDeviceID(7)", ConnectionString);
                    break;
                case TypeDevice.ProjectorScreen:
                    locationDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllLocationByTypeDeviceID(8)", ConnectionString);
                    break;
            }

            locationDataSet = new DataSet();
            locationDataAdapter.Fill(locationDataSet);
            location.ItemsSource = locationDataSet.Tables[0].DefaultView;
            location.DisplayMemberPath = "Place";
        }

        private void UpdateSourceForCPU()
        {
            switch (TypeDevice)
            {
                case TypeDevice.Notebook:
                    cpuDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllNotebookCPU()", ConnectionString);
                    cpuDataSet = new DataSet();
                    cpuDataAdapter.Fill(cpuDataSet);
                    cpu.ItemsSource = cpuDataSet.Tables[0].DefaultView;
                    cpu.DisplayMemberPath = "CPUModel";
                    break;
                case TypeDevice.PC:
                    cpuDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllPCCPU()", ConnectionString);
                    cpuDataSet = new DataSet();
                    cpuDataAdapter.Fill(cpuDataSet);
                    cpu.ItemsSource = cpuDataSet.Tables[0].DefaultView;
                    cpu.DisplayMemberPath = "CPUModel";
                    break;
            }
        }

        private void UpdateSourceForVideoCard()
        {
            switch (TypeDevice)
            {
                case TypeDevice.Notebook:
                    vCardDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllNotebookvCard()", ConnectionString);
                    vCardDataSet = new DataSet();
                    vCardDataAdapter.Fill(vCardDataSet);
                    vCard.ItemsSource = vCardDataSet.Tables[0].DefaultView;
                    vCard.DisplayMemberPath = "VideoCard";
                    break;
                case TypeDevice.PC:
                    vCardDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllPCvCard()", ConnectionString);
                    vCardDataSet = new DataSet();
                    vCardDataAdapter.Fill(vCardDataSet);
                    vCard.ItemsSource = vCardDataSet.Tables[0].DefaultView;
                    vCard.DisplayMemberPath = "VideoCard";
                    break;
            }
        }

        private void UpdateSourceForType()
        {
            switch (TypeDevice)
            {
                case TypeDevice.NetworkSwitch:
                    typeNetworkSwitchDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllTypeNetworkSwitch()", ConnectionString);
                    typeNetworkSwitchDataSet = new DataSet();
                    typeNetworkSwitchDataAdapter.Fill(typeNetworkSwitchDataSet);
                    type.ItemsSource = typeNetworkSwitchDataSet.Tables[0].DefaultView;
                    type.DisplayMemberPath = "Name";
                    break;
                case TypeDevice.Notebook:
                    typeNotebookDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllTypeNotebook()", ConnectionString);
                    typeNotebookDataSet = new DataSet();
                    typeNotebookDataAdapter.Fill(typeNotebookDataSet);
                    type.ItemsSource = typeNotebookDataSet.Tables[0].DefaultView;
                    type.DisplayMemberPath = "Name";
                    break;
                case TypeDevice.PrinterScanner:
                    typePrinterDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllTypePrinter()", ConnectionString);
                    typePrinterDataSet = new DataSet();
                    typePrinterDataAdapter.Fill(typePrinterDataSet);
                    type.ItemsSource = typePrinterDataSet.Tables[0].DefaultView;
                    type.DisplayMemberPath = "Name";
                    break;
            }
        }

        private void UpdateSourceForFrequency()
        {
            frequencyDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllFrequency()", ConnectionString);
            frequencyDataSet = new DataSet();
            frequencyDataAdapter.Fill(frequencyDataSet);
            screenFrequency.ItemsSource = frequencyDataSet.Tables[0].DefaultView;
            screenFrequency.DisplayMemberPath = "Name";
        }

        private void UpdateSourceForMatrixTechology()
        {
            matrixTechnologyDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllMatrixTechnology()", ConnectionString);
            matrixTechnologyDataSet = new DataSet();
            matrixTechnologyDataAdapter.Fill(matrixTechnologyDataSet);
            matrixTechnology.ItemsSource = matrixTechnologyDataSet.Tables[0].DefaultView;
            matrixTechnology.DisplayMemberPath = "Name";
        }

        private void UpdateSourceForPaperSize()
        {
            paperSizeDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllPaperSize()", ConnectionString);
            paperSizeDataSet = new DataSet();
            paperSizeDataAdapter.Fill(paperSizeDataSet);
            paperSize.ItemsSource = paperSizeDataSet.Tables[0].DefaultView;
            paperSize.DisplayMemberPath = "Name";
        }

        private void UpdateSourceForProjectorTechnology()
        {
            projectorTechnologyDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllProjectorTechnology()", ConnectionString);
            projectorTechnologyDataSet = new DataSet();
            projectorTechnologyDataAdapter.Fill(projectorTechnologyDataSet);
            projectorTechnology.ItemsSource = projectorTechnologyDataSet.Tables[0].DefaultView;
            projectorTechnology.DisplayMemberPath = "Name";
        }

        private void UpdateSourceForResolution()
        {
            resolutionDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllResolution()", ConnectionString);
            resolutionDataSet = new DataSet();
            resolutionDataAdapter.Fill(resolutionDataSet);
            resolution.ItemsSource = resolutionDataSet.Tables[0].DefaultView;
            resolution.DisplayMemberPath = "Name";
        }

        private void UpdateSourceForVideoConnectors()
        {
            //videoConnectorsDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllVideoConnector()", connectionString);
            //videoConnectorsDataSet = new DataSet();
            //videoConnectorsDataAdapter.Fill(videoConnectorsDataSet);
            //vConnectors.ItemsSource = videoConnectorsDataSet.Tables[0].DefaultView;
            //vConnectors.DisplayMemberPath = "Name";

            videoConnectorsDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllVideoConnector()", ConnectionString);
            videoConnectorsDataSet = new DataSet();
            videoConnectorsDataAdapter.Fill(videoConnectorsDataSet);
            videoConnectorsItems = new List<ListBoxItem>();
            foreach (DataRowView row in videoConnectorsDataSet.Tables[0].DefaultView)
                videoConnectorsItems.Add(new ListBoxItem() { Content = row.Row[1].ToString() });
            vConnectors.ItemsSource = videoConnectorsItems;
            //vConnectors.ItemsSource = videoConnectorsDataSet.Tables[0].DefaultView;
            //vConnectors.DisplayMemberPath = "Name";
        }

        private void UpdateSourceForWifiFrequency()
        {
            wifiFrequencyDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllWiFiFrequency()", ConnectionString);
            wifiFrequencyDataSet = new DataSet();
            wifiFrequencyDataAdapter.Fill(wifiFrequencyDataSet);
            wifiFrequency.ItemsSource = wifiFrequencyDataSet.Tables[0].DefaultView;
            wifiFrequency.DisplayMemberPath = "Name";
        }

        private void UpdateSourceForMotherboard()
        {
            motherboardDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllMotherboard()", ConnectionString);
            motherboardDataSet = new DataSet();
            motherboardDataAdapter.Fill(motherboardDataSet);
            motherboard.ItemsSource = motherboardDataSet.Tables[0].DefaultView;
            motherboard.DisplayMemberPath = "Name";
        }

        private void UpdateSourceForTypeLicense()
        {
            typeLicenseDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllTypeSoftLicense()", ConnectionString);
            typeLicenseDataSet = new DataSet();
            typeLicenseDataAdapter.Fill(typeLicenseDataSet);
            typeLicense.ItemsSource = typeLicenseDataSet.Tables[0].DefaultView;
            typeLicense.DisplayMemberPath = "Name";
        }
    }
}
