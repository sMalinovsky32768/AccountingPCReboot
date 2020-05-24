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
            switch (TypeDevice)
            {
                case TypeDevice.PC:
                    UpdatePCData();
                    break;
                case TypeDevice.Notebook:
                    UpdateNotebookData();
                    break;
                case TypeDevice.Monitor:
                    UpdateMonitorData();
                    break;
                case TypeDevice.Projector:
                    UpdateProjectorData();
                    break;
                case TypeDevice.InteractiveWhiteboard:
                    UpdateInteractiveWhiteboardData();
                    break;
                case TypeDevice.ProjectorScreen:
                    UpdateProjectorScreenData();
                    break;
                case TypeDevice.PrinterScanner:
                    UpdatePrinterScannerData();
                    break;
                case TypeDevice.NetworkSwitch:
                    UpdateNetworkSwitchData();
                    break;
                case TypeDevice.OtherEquipment:
                    UpdateOtherEquipmentData();
                    break;
            }
        }

        private void UpdateAllEquipmentData()
        {
            UpdatePCData();
            UpdateNotebookData();
            UpdateMonitorData();
            UpdateProjectorData();
            UpdateInteractiveWhiteboardData();
            UpdateProjectorScreenData();
            UpdatePrinterScannerData();
            UpdateNetworkSwitchData();
            UpdateOtherEquipmentData();
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
            ResetEquipmentPopup();
            switch (NowView)
            {
                case View.Equipment:
                    switch (TypeDevice)
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
                    break;
                case View.Software:
                    switch (TypeSoft)
                    {
                        case TypeSoft.LicenseSoftware:
                            InitializePopupForSoftware();
                            break;
                        case TypeSoft.OS:
                            InitializePopupForOS();
                            break;
                    }
                    break;
            }
            UpdatePopupSource();
        }

        private void UpdatePopupSource()
        {
            switch (NowView)
            {
                case View.Equipment:
                    UpdateSourceForAspectRatio();
                    UpdateSourceForOS();
                    UpdateSourceForScreenInstalled();
                    UpdateSourceForName();
                    UpdateSourceForLocation();
                    UpdateSourceForCPU();
                    UpdateSourceForVideoCard();
                    UpdateSourceForType();
                    UpdateSourceForFrequency();
                    UpdateSourceForMatrixTechology();
                    UpdateSourceForPaperSize();
                    UpdateSourceForProjectorTechnology();
                    UpdateSourceForResolution();
                    UpdateSourceForVideoConnectors();
                    UpdateSourceForWifiFrequency();
                    UpdateSourceForMotherboard();
                    break;
                case View.Software:
                    UpdateSourceForTypeLicense();
                    break;
            }
        }

        private void SaveOrUpdateEquipmentDB()
        {
            Task task;
            switch (TypeChange)
            {
                case TypeChange.Add:
                    switch (TypeDevice)
                    {
                        case TypeDevice.PC:
                            AddPC();
                            break;
                        case TypeDevice.Notebook:
                            AddNotebook();
                            break;
                        case TypeDevice.Monitor:
                            AddMonitor();
                            break;
                        case TypeDevice.NetworkSwitch:
                            AddNetworkSwitch();
                            break;
                        case TypeDevice.InteractiveWhiteboard:
                            AddInteractiveWhiteboard();
                            break;
                        case TypeDevice.PrinterScanner:
                            AddPrinterScanner();
                            break;
                        case TypeDevice.Projector:
                            AddProjector();
                            break;
                        case TypeDevice.ProjectorScreen:
                            AddProjectorScreen();
                            break;
                        case TypeDevice.OtherEquipment:
                            AddOtherEquipment();
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
                    switch (TypeDevice)
                    {
                        case TypeDevice.PC:
                            UpdatePC();
                            break;
                        case TypeDevice.Notebook:
                            UpdateNotebook();
                            break;
                        case TypeDevice.Monitor:
                            UpdateMonitor();
                            break;
                        case TypeDevice.NetworkSwitch:
                            UpdateNetworkSwitch();
                            break;
                        case TypeDevice.InteractiveWhiteboard:
                            UpdateInteractiveWhiteboard();
                            break;
                        case TypeDevice.PrinterScanner:
                            UpdatePrinterScanner();
                            break;
                        case TypeDevice.Projector:
                            UpdateProjector();
                            break;
                        case TypeDevice.ProjectorScreen:
                            UpdateProjectorScreen();
                            break;
                        case TypeDevice.OtherEquipment:
                            UpdateOtherEquipment();
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
                    changeEquipmentPopup.IsOpen = false;
                    IsPreOpenEquipmentPopup = false;
                    viewGrid.IsEnabled = true;
                    menu.IsEnabled = true;
                    break;
            }
        }

        private void SaveOrUpdateSoftwareDB()
        {
            Task task;
            switch (TypeChange)
            {
                case TypeChange.Add:
                    switch (TypeSoft)
                    {
                        case TypeSoft.LicenseSoftware:
                            AddSoftware();
                            break;
                        case TypeSoft.OS:
                            AddOS();
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
                    switch (TypeSoft)
                    {
                        case TypeSoft.LicenseSoftware:
                            UpdateSoftware();
                            break;
                        case TypeSoft.OS:
                            UpdateOS();
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
                    changeEquipmentPopup.IsOpen = false;
                    IsPreOpenEquipmentPopup = false;
                    viewGrid.IsEnabled = true;
                    menu.IsEnabled = true;
                    break;
            }
        }
    }
}
