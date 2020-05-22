using System;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;

namespace AccountingPC
{
    public partial class AccountingPCWindow
    {
        public void UpdateImages()
        {
            images = GetImages();
        }

        private void UpdateEquipmentData()
        {
            String connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            switch (typeDevice)
            {
                case TypeDevice.PC:
                    UpdatePCData(connectionString);
                    break;
                case TypeDevice.Notebook:
                    UpdateNotebookData(connectionString);
                    break;
                case TypeDevice.Monitor:
                    UpdateMonitorData(connectionString);
                    break;
                case TypeDevice.Projector:
                    UpdateProjectorData(connectionString);
                    break;
                case TypeDevice.InteractiveWhiteboard:
                    UpdateInteractiveWhiteboardData(connectionString);
                    break;
                case TypeDevice.ProjectorScreen:
                    UpdateProjectorScreenData(connectionString);
                    break;
                case TypeDevice.PrinterScanner:
                    UpdatePrinterScannerData(connectionString);
                    break;
                case TypeDevice.NetworkSwitch:
                    UpdateNetworkSwitchData(connectionString);
                    break;
                case TypeDevice.OtherEquipment:
                    UpdateOtherEquipmentData(connectionString);
                    break;
            }
        }

        private void UpdateAllEquipmentData()
        {
            String connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            UpdatePCData(connectionString);
            UpdateNotebookData(connectionString);
            UpdateMonitorData(connectionString);
            UpdateProjectorData(connectionString);
            UpdateInteractiveWhiteboardData(connectionString);
            UpdateProjectorScreenData(connectionString);
            UpdatePrinterScannerData(connectionString);
            UpdateNetworkSwitchData(connectionString);
            UpdateOtherEquipmentData(connectionString);
        }

        private void ChangeEquipmentView()
        {
            switch (equipmentCategoryList.SelectedIndex)
            {
                case 0:
                    SetViewToPC();
                    break;
                case 1:
                    SetViewToNotebook();
                    break;
                case 2:
                    SetViewToMonitor();
                    break;
                case 3:
                    SetViewToProjector();
                    break;
                case 4:
                    SetViewToInteractiveWhiteboard();
                    break;
                case 5:
                    SetViewToProjectorScreen();
                    break;
                case 6:
                    SetViewToPrinterScanner();
                    break;
                case 7:
                    SetViewToNetworkSwitch();
                    break;
                case 8:
                    SetViewToOtherEquipment();
                    break;
            }
        }

        private void InitializePopup()
        {
            ResetPopup();
            switch (typeDevice)
            {
                case TypeDevice.InteractiveWhiteboard:
                    InitializePopupForInteractiveWhiteboard();
                    break;
                case TypeDevice.Monitor:
                    InitializePopupForMonitor();
                    break;
                case TypeDevice.NetworkSwitch:
                    InitializePopupForNetworkSwitch();
                    break;
                case TypeDevice.Notebook:
                    InitializePopupForNotebook();
                    break;
                case TypeDevice.OtherEquipment:
                    InitializePopupForOtherEquipment();
                    break;
                case TypeDevice.PC:
                    InitializePopupForPC();
                    break;
                case TypeDevice.PrinterScanner:
                    InitializePopupForPrinterScanner();
                    break;
                case TypeDevice.Projector:
                    InitializePopupForProjector();
                    break;
                case TypeDevice.ProjectorScreen:
                    InitializePopupForProjectorScreen();
                    break;
            }
            UpdatePopupSource();
        }

        private void UpdatePopupSource()
        {
            String connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            UpdateSourceForAspectRatio(connectionString);
            UpdateSourceForOS(connectionString);
            UpdateSourceForScreenInstalled(connectionString);
            UpdateSourceForName(connectionString);
            UpdateSourceForLocation(connectionString);
            UpdateSourceForCPU(connectionString);
            UpdateSourceForVideoCard(connectionString);
            UpdateSourceForType(connectionString);
            UpdateSourceForFrequency(connectionString);
            UpdateSourceForMatrixTechology(connectionString);
            UpdateSourceForPaperSize(connectionString);
            UpdateSourceForProjectorTechnology(connectionString);
            UpdateSourceForResolution(connectionString);
            UpdateSourceForVideoConnectors(connectionString);
            UpdateSourceForWifiFrequency(connectionString);
            UpdateSourceForMotherboard(connectionString);
        }

        private void SaveOrUpdateDB()
        {
            Task task;
            String connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            switch (typeChange)
            {
                case TypeChange.Add:
                    switch (typeDevice)
                    {
                        case TypeDevice.PC:
                            AddPC(connectionString);
                            break;
                        case TypeDevice.Notebook:
                            AddNotebook(connectionString);
                            break;
                        case TypeDevice.Monitor:
                            AddMonitor(connectionString);
                            break;
                        case TypeDevice.NetworkSwitch:
                            AddNetworkSwitch(connectionString);
                            break;
                        case TypeDevice.InteractiveWhiteboard:
                            AddInteractiveWhiteboard(connectionString);
                            break;
                        case TypeDevice.PrinterScanner:
                            AddPrinterScanner(connectionString);
                            break;
                        case TypeDevice.Projector:
                            AddProjector(connectionString);
                            break;
                        case TypeDevice.ProjectorScreen:
                            AddProjectorScreen(connectionString);
                            break;
                        case TypeDevice.OtherEquipment:
                            AddOtherEquipment(connectionString);
                            break;
                    }
                    statusItem1.Content = "Успешно добавлено";
                    task = new Task(() =>
                    {
                        try
                        {
                            for (int i = 0; i < 10; i++)
                            {
                                i++;
                                Thread.Sleep(1000);
                            }
                            Dispatcher.Invoke(() => statusItem1.Content = string.Empty);

                        }
                        catch { }
                    });
                    task.Start();
                    break;
                case TypeChange.Change:
                    switch (typeDevice)
                    {
                        case TypeDevice.PC:
                            UpdatePC(connectionString);
                            break;
                        case TypeDevice.Notebook:
                            UpdateNotebook(connectionString);
                            break;
                        case TypeDevice.Monitor:
                            UpdateMonitor(connectionString);
                            break;
                        case TypeDevice.NetworkSwitch:
                            UpdateNetworkSwitch(connectionString);
                            break;
                        case TypeDevice.InteractiveWhiteboard:
                            UpdateInteractiveWhiteboard(connectionString);
                            break;
                        case TypeDevice.PrinterScanner:
                            UpdatePrinterScanner(connectionString);
                            break;
                        case TypeDevice.Projector:
                            UpdateProjector(connectionString);
                            break;
                        case TypeDevice.ProjectorScreen:
                            UpdateProjectorScreen(connectionString);
                            break;
                        case TypeDevice.OtherEquipment:
                            UpdateOtherEquipment(connectionString);
                            break;
                    }
                    statusItem1.Content = "Успешно изменено";
                    task = new Task(() =>
                    {
                        try
                        {
                            for (int i = 0; i < 10; i++)
                            {
                                i++;
                                Thread.Sleep(1000);
                            }
                            Dispatcher.Invoke(() => statusItem1.Content = string.Empty);

                        }
                        catch { }
                    });
                    task.Start();
                    changePopup.IsOpen = false;
                    isPreOpenPopup = false;
                    viewGrid.IsEnabled = true;
                    menu.IsEnabled = true;
                    break;
            }
        }
    }
}
