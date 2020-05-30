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

        internal void UpdateEquipmentData()
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

        internal void UpdateAllEquipmentData()
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
        
        internal void ChangeEquipmentView()
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
    }
}
