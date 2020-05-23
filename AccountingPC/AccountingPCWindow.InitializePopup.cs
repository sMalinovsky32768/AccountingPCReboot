using System.Windows;

namespace AccountingPC
{
    public partial class AccountingPCWindow
    {
        private void ResetPopup()
        {
            imageFilename.Text = string.Empty;
            if (TypeChange == TypeChange.Add)
            {
                autoInvN.Visibility = Visibility.Visible;
                if (disabledRepeatInvN.IsChecked == true)
                {
                    DisabledRepeatInvN_Checked();
                }
                else
                {
                    DisabledRepeatInvN_Unchecked();
                }
            }
            else if (TypeChange == TypeChange.Change)
            {
                autoInvN.Visibility = Visibility.Collapsed;
                autoInvN.IsChecked = false;
                DisabledRepeatInvN_Unchecked();
            }
            inventoryNumberGrid.Visibility = Visibility.Visible;
            deviceNameGrid.Visibility = Visibility.Visible;
            costGrid.Visibility = Visibility.Visible;
            invoiceGrid.Visibility = Visibility.Hidden;
            locationGrid.Visibility = Visibility.Hidden;
            motherboardGrid.Visibility = Visibility.Hidden;
            cpuGrid.Visibility = Visibility.Hidden;
            coresGrid.Visibility = Visibility.Hidden;
            frequencyGrid.Visibility = Visibility.Hidden;
            maxFrequencyGrid.Visibility = Visibility.Hidden;
            vCardGrid.Visibility = Visibility.Hidden;
            videoramGrid.Visibility = Visibility.Hidden;
            ssdGrid.Visibility = Visibility.Hidden;
            hddGrid.Visibility = Visibility.Hidden;
            ramGrid.Visibility = Visibility.Hidden;
            ramFrequencyGrid.Visibility = Visibility.Hidden;
            osGrid.Visibility = Visibility.Hidden;
            vConnectorsGrid.Visibility = Visibility.Hidden;
            resolutionGrid.Visibility = Visibility.Hidden;
            screenFrequencyGrid.Visibility = Visibility.Hidden;
            matrixTechnologyGrid.Visibility = Visibility.Hidden;
            typeGrid.Visibility = Visibility.Hidden;
            diagonalGrid.Visibility = Visibility.Hidden;
            projectorTechnologyGrid.Visibility = Visibility.Hidden;
            isEDriveGrid.Visibility = Visibility.Hidden;
            aspectRatioGrid.Visibility = Visibility.Hidden;
            screenInstalledGrid.Visibility = Visibility.Hidden;
            paperSizeGrid.Visibility = Visibility.Hidden;
            portsGrid.Visibility = Visibility.Hidden;
            wifiFrequencyGrid.Visibility = Visibility.Hidden;
            GridPlacement(inventoryNumberGrid, 0, 0, 3);
            GridPlacement(deviceNameGrid, 3, 0, 7);
            GridPlacement(costGrid, 10, 0, 2);
            GridPlacement(imageLoadGrid, 0, 6, 12);
        }

        private void InitializePopupForInteractiveWhiteboard()
        {
            invoiceGrid.Visibility = Visibility.Visible;
            locationGrid.Visibility = Visibility.Visible;
            diagonalGrid.Visibility = Visibility.Visible;
            GridPlacement(diagonalGrid, 0, 1, 2);
            GridPlacement(invoiceGrid, 2, 1, 3);
            GridPlacement(locationGrid, 5, 1, 7);
        }

        private void InitializePopupForMonitor()
        {
            vConnectorsGrid.Visibility = Visibility.Visible;
            resolutionGrid.Visibility = Visibility.Visible;
            screenFrequencyGrid.Visibility = Visibility.Visible;
            matrixTechnologyGrid.Visibility = Visibility.Visible;
            diagonalGrid.Visibility = Visibility.Visible;
            invoiceGrid.Visibility = Visibility.Visible;
            locationGrid.Visibility = Visibility.Visible;
            GridPlacement(diagonalGrid, 0, 1, 2);
            GridPlacement(resolutionGrid, 2, 1, 3);
            GridPlacement(screenFrequencyGrid, 5, 1, 2);
            GridPlacement(matrixTechnologyGrid, 7, 1, 3);
            GridPlacement(invoiceGrid, 0, 2, 3);
            GridPlacement(locationGrid, 3, 2, 7);
            GridPlacement(vConnectorsGrid, 10, 1, 2, 2);
        }

        private void InitializePopupForNetworkSwitch()
        {
            typeGrid.Visibility = Visibility.Visible;
            invoiceGrid.Visibility = Visibility.Visible;
            locationGrid.Visibility = Visibility.Visible;
            portsGrid.Visibility = Visibility.Visible;
            wifiFrequencyGrid.Visibility = Visibility.Visible;
            GridPlacement(inventoryNumberGrid, 0, 0, 3);
            GridPlacement(typeGrid, 3, 0, 2);
            GridPlacement(deviceNameGrid, 5, 0, 5);
            GridPlacement(costGrid, 10, 0, 2);
            GridPlacement(portsGrid, 0, 1, 3);
            GridPlacement(wifiFrequencyGrid, 3, 1, 3);
            GridPlacement(invoiceGrid, 6, 1, 6);
            GridPlacement(locationGrid, 0, 2, 12);
        }

        private void InitializePopupForNotebook()
        {
            typeGrid.Visibility = Visibility.Visible;
            invoiceGrid.Visibility = Visibility.Visible;
            locationGrid.Visibility = Visibility.Visible;
            cpuGrid.Visibility = Visibility.Visible;
            coresGrid.Visibility = Visibility.Visible;
            frequencyGrid.Visibility = Visibility.Visible;
            maxFrequencyGrid.Visibility = Visibility.Visible;
            vCardGrid.Visibility = Visibility.Visible;
            videoramGrid.Visibility = Visibility.Visible;
            ssdGrid.Visibility = Visibility.Visible;
            hddGrid.Visibility = Visibility.Visible;
            ramGrid.Visibility = Visibility.Visible;
            ramFrequencyGrid.Visibility = Visibility.Visible;
            osGrid.Visibility = Visibility.Visible;
            vConnectorsGrid.Visibility = Visibility.Visible;
            diagonalGrid.Visibility = Visibility.Visible;
            resolutionGrid.Visibility = Visibility.Visible;
            screenFrequencyGrid.Visibility = Visibility.Visible;
            matrixTechnologyGrid.Visibility = Visibility.Visible;
            GridPlacement(inventoryNumberGrid, 0, 0, 3);
            GridPlacement(typeGrid, 3, 0, 2);
            GridPlacement(deviceNameGrid, 5, 0, 5);
            GridPlacement(costGrid, 10, 0, 2);
            GridPlacement(cpuGrid, 0, 1, 4);
            GridPlacement(coresGrid, 4, 1, 2);
            GridPlacement(frequencyGrid, 6, 1, 3);
            GridPlacement(maxFrequencyGrid, 9, 1, 3);
            GridPlacement(diagonalGrid, 0, 2, 2);
            GridPlacement(resolutionGrid, 2, 2, 3);
            GridPlacement(screenFrequencyGrid, 5, 2, 2);
            GridPlacement(matrixTechnologyGrid, 7, 2, 3);
            GridPlacement(ssdGrid, 0, 3, 2);
            GridPlacement(hddGrid, 2, 3, 2);
            GridPlacement(vCardGrid, 4, 3, 4);
            GridPlacement(videoramGrid, 8, 3, 2);
            GridPlacement(ramGrid, 0, 4, 2);
            GridPlacement(ramFrequencyGrid, 2, 4, 2);
            GridPlacement(osGrid, 4, 4, 6);
            GridPlacement(invoiceGrid, 0, 5, 4);
            GridPlacement(locationGrid, 4, 5, 6);
            GridPlacement(vConnectorsGrid, 10, 2, 2, 4);
        }

        private void InitializePopupForOtherEquipment()
        {
            invoiceGrid.Visibility = Visibility.Visible;
            locationGrid.Visibility = Visibility.Visible;
            GridPlacement(invoiceGrid, 0, 1, 3);
            GridPlacement(locationGrid, 3, 1, 9);
        }

        private void InitializePopupForPC()
        {
            motherboardGrid.Visibility = Visibility.Visible;
            cpuGrid.Visibility = Visibility.Visible;
            coresGrid.Visibility = Visibility.Visible;
            frequencyGrid.Visibility = Visibility.Visible;
            maxFrequencyGrid.Visibility = Visibility.Visible;
            vCardGrid.Visibility = Visibility.Visible;
            videoramGrid.Visibility = Visibility.Visible;
            ssdGrid.Visibility = Visibility.Visible;
            hddGrid.Visibility = Visibility.Visible;
            ramGrid.Visibility = Visibility.Visible;
            ramFrequencyGrid.Visibility = Visibility.Visible;
            osGrid.Visibility = Visibility.Visible;
            invoiceGrid.Visibility = Visibility.Visible;
            locationGrid.Visibility = Visibility.Visible;
            osGrid.Visibility = Visibility.Visible;
            vConnectorsGrid.Visibility = Visibility.Visible;
            GridPlacement(motherboardGrid, 0, 1, 3);
            GridPlacement(cpuGrid, 3, 1, 3);
            GridPlacement(coresGrid, 6, 1, 2);
            GridPlacement(frequencyGrid, 8, 1, 2);
            GridPlacement(maxFrequencyGrid, 10, 1, 2);
            GridPlacement(vCardGrid, 0, 2, 4);
            GridPlacement(videoramGrid, 4, 2, 2);
            GridPlacement(ramGrid, 6, 2, 2);
            GridPlacement(ramFrequencyGrid, 8, 2, 2);
            GridPlacement(ssdGrid, 0, 3, 2);
            GridPlacement(hddGrid, 2, 3, 2);
            GridPlacement(osGrid, 4, 3, 6);
            GridPlacement(invoiceGrid, 0, 4, 4);
            GridPlacement(locationGrid, 4, 4, 6);
            GridPlacement(vConnectorsGrid, 10, 2, 2, 3);
        }

        private void InitializePopupForPrinterScanner()
        {
            paperSizeGrid.Visibility = Visibility.Visible;
            typeGrid.Visibility = Visibility.Visible;
            invoiceGrid.Visibility = Visibility.Visible;
            locationGrid.Visibility = Visibility.Visible;
            GridPlacement(inventoryNumberGrid, 0, 0, 3);
            GridPlacement(typeGrid, 3, 0, 2);
            GridPlacement(deviceNameGrid, 5, 0, 5);
            GridPlacement(costGrid, 10, 0, 2);
            GridPlacement(paperSizeGrid, 0, 1, 2);
            GridPlacement(invoiceGrid, 2, 1, 3);
            GridPlacement(locationGrid, 5, 1, 7);
        }

        private void InitializePopupForProjector()
        {
            vConnectorsGrid.Visibility = Visibility.Visible;
            diagonalGrid.Visibility = Visibility.Visible;
            resolutionGrid.Visibility = Visibility.Visible;
            projectorTechnologyGrid.Visibility = Visibility.Visible;
            invoiceGrid.Visibility = Visibility.Visible;
            locationGrid.Visibility = Visibility.Visible;
            GridPlacement(diagonalGrid, 0, 1, 2);
            GridPlacement(resolutionGrid, 2, 1, 4);
            GridPlacement(projectorTechnologyGrid, 6, 1, 4);
            GridPlacement(invoiceGrid, 0, 2, 3);
            GridPlacement(locationGrid, 3, 2, 7);
            GridPlacement(vConnectorsGrid, 10, 1, 2, 2);
        }

        private void InitializePopupForProjectorScreen()
        {
            diagonalGrid.Visibility = Visibility.Visible;
            isEDriveGrid.Visibility = Visibility.Visible;
            aspectRatioGrid.Visibility = Visibility.Visible;
            screenInstalledGrid.Visibility = Visibility.Visible;
            invoiceGrid.Visibility = Visibility.Visible;
            locationGrid.Visibility = Visibility.Visible;
            GridPlacement(diagonalGrid, 0, 1, 2);
            GridPlacement(aspectRatioGrid, 2, 1, 2);
            GridPlacement(isEDriveGrid, 4, 1, 2);
            GridPlacement(screenInstalledGrid, 6, 1, 6);
            GridPlacement(invoiceGrid, 0, 2, 3);
            GridPlacement(locationGrid, 3, 2, 9);
        }
    }
}
