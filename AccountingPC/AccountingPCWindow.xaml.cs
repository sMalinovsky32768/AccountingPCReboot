﻿using AccountingPC.Properties;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Data;
using AccountingPC.AccountingReport;
using System.Linq;

namespace AccountingPC
{
    /// <summary>
    /// Логика взаимодействия для AccountingPCWindow.xaml
    /// </summary>
    public partial class AccountingPCWindow : Window
    {

        public static String ConnectionString { get; private set; } = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public static readonly RoutedCommand ParametersCommand = new RoutedUICommand(
            "Parameters", "ParametersCommand", typeof(AccountingPCWindow),
            new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.F12) }));

        public static readonly RoutedCommand ExitCommand = new RoutedUICommand(
            "Exit", "ExitCommand", typeof(AccountingPCWindow),
            new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.F4, ModifierKeys.Alt) }));

        public static readonly RoutedCommand PopupCloseCommand = new RoutedUICommand(
            "PopupClose", "PopupCloseCommand", typeof(AccountingPCWindow),
            new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.Escape) }));

        public static readonly RoutedCommand SelectViewEquipmentCommand = new RoutedUICommand(
            "SelectViewEquipment", "SelectViewEquipmentCommand", typeof(AccountingPCWindow),
            new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.E, ModifierKeys.Alt) }));

        public static readonly RoutedCommand SelectViewSoftwareCommand = new RoutedUICommand(
            "SelectViewSoftware", "SelectViewSoftwareCommand", typeof(AccountingPCWindow),
            new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.S, ModifierKeys.Alt) }));

        public static readonly RoutedCommand SelectViewInvoiceCommand = new RoutedUICommand(
            "SelectViewInvoice", "SelectViewInvoiceCommand", typeof(AccountingPCWindow),
            new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.I, ModifierKeys.Alt) }));

        public static readonly RoutedCommand SelectViewLocationCommand = new RoutedUICommand(
            "SelectViewLocation", "SelectViewLocationCommand", typeof(AccountingPCWindow),
            new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.L, ModifierKeys.Alt) }));


        public static readonly RoutedCommand UpdateViewCommand = new RoutedUICommand(
            "UpdateView", "UpdateViewCommand", typeof(AccountingPCWindow),
            new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.F5), new KeyGesture(Key.R, ModifierKeys.Control) }));

        private View NowView { get; set; }
        private bool IsChangeAnalog { get; set; }
        private TypeDevice TypeDevice { get; set; }
        private TypeSoft TypeSoft { get; set; }
        private TypeChange TypeChange { get; set; }
        private int DeviceID { get; set; }
        private int SoftwareID { get; set; }
        private bool IsPreOpenEquipmentPopup { get; set; }
        private bool IsPreOpenSoftwarePopup { get; set; }

        private Binding invNBinding;

        private Dictionary<int, byte[]> images;

        List<ListBoxItem> videoConnectorsItems;

        List<InstalledSoftware> pcSoftware;
        List<InstalledSoftware> notebookSoftware;
        List<InstalledSoftware> pcNotInstalledSoftware;
        List<InstalledSoftware> notebookNotInstalledSoftware;

        public double lastHeight;
        public double lastWidth;

        SqlDataAdapter aspectRatioDataAdapter;
        SqlDataAdapter cpuDataAdapter;
        SqlDataAdapter softwareDataAdapter;
        SqlDataAdapter pcNotInstalledSoftwareDataAdapter;
        SqlDataAdapter pcSoftwareDataAdapter;
        SqlDataAdapter notebookNotInstalledSoftwareDataAdapter;
        SqlDataAdapter notebookSoftwareDataAdapter;
        SqlDataAdapter osDataAdapter;
        SqlDataAdapter screenInstalledDataAdapter;
        SqlDataAdapter boardDataAdapter;
        SqlDataAdapter frequencyDataAdapter;
        SqlDataAdapter locationDataAdapter;
        SqlDataAdapter matrixTechnologyDataAdapter;
        SqlDataAdapter monitorDataAdapter;
        SqlDataAdapter networkSwitchDataAdapter;
        SqlDataAdapter notebookDataAdapter;
        SqlDataAdapter otherEquipmentDataAdapter;
        SqlDataAdapter paperSizeDataAdapter;
        SqlDataAdapter pcDataAdapter;
        SqlDataAdapter printerScannerDataAdapter;
        SqlDataAdapter projectorDataAdapter;
        SqlDataAdapter projectorScreenDataAdapter;
        SqlDataAdapter projectorTechnologyDataAdapter;
        SqlDataAdapter resolutionDataAdapter;
        SqlDataAdapter typeNetworkSwitchDataAdapter;
        SqlDataAdapter typeNotebookDataAdapter;
        SqlDataAdapter typePrinterDataAdapter;
        SqlDataAdapter videoConnectorsDataAdapter;
        SqlDataAdapter wifiFrequencyDataAdapter;
        SqlDataAdapter nameDataAdapter;
        SqlDataAdapter motherboardDataAdapter;
        SqlDataAdapter vCardDataAdapter;
        SqlDataAdapter typeDeviceDataAdapter;
        SqlDataAdapter typeLicenseDataAdapter;

        DataSet aspectRatioDataSet;
        DataSet cpuDataSet;
        DataSet softwareDataSet;
        DataSet pcNotInstalledSoftwareDataSet;
        DataSet pcSoftwareDataSet;
        DataSet notebookNotInstalledSoftwareDataSet;
        DataSet notebookSoftwareDataSet;
        DataSet osDataSet;
        DataSet screenInstalledDataSet;
        DataSet boardDataSet;
        DataSet frequencyDataSet;
        DataSet locationDataSet;
        DataSet matrixTechnologyDataSet;
        DataSet monitorDataSet;
        DataSet networkSwitchDataSet;
        DataSet notebookDataSet;
        DataSet otherEquipmentDataSet;
        DataSet paperSizeDataSet;
        DataSet pcDataSet;
        DataSet printerScannerDataSet;
        DataSet projectorDataSet;
        DataSet projectorScreenDataSet;
        DataSet projectorTechnologyDataSet;
        DataSet resolutionDataSet;
        DataSet typeNetworkSwitchDataSet;
        DataSet typeNotebookDataSet;
        DataSet typePrinterDataSet;
        DataSet videoConnectorsDataSet;
        DataSet wifiFrequencyDataSet;
        DataSet nameDataSet;
        DataSet motherboardDataSet;
        DataSet vCardDataSet;
        DataSet typeDeviceDataSet;
        DataSet typeLicenseDataSet;

        static AccountingPCWindow()
        {
            //AvailableInvNProperty = DependencyProperty.Register("AvailableInvN", typeof(bool), typeof(AccountingPCWindow));
        }
        public AccountingPCWindow()
        {
            InitializeComponent();
            lastHeight = Height;
            lastWidth = Width;
            UpdateAllEquipmentData();
            UpdateAllSoftwareData();
            UpdateImages();
            //equipmentCategoryList.SelectedIndex = 0;
            IsPreOpenEquipmentPopup = false;
            NowView = View.Equipment;
            reportMenu.ItemContainerGenerator.StatusChanged += ItemContainerGenerator_StatusChanged;
            reportMenu.ItemsSource = AccountingReport.Report.ReportNamesCollection;
            reportMenu.DisplayMemberPath = "Name";
            //if (reportMenu.ItemContainerGenerator.Status ==
            //    System.Windows.Controls.Primitives.GeneratorStatus.ContainersGenerated)
            //{
            //    var containers = reportMenu.Items.Cast<object>().Select(
            //        item => (FrameworkElement)reportMenu.ItemContainerGenerator.ContainerFromItem(item));
            //    foreach (var container in containers)
            //    {
            //        (container as MenuItem).Click += CreateReport_Click;
            //    }
            //}
        }

        private void ItemContainerGenerator_StatusChanged(object sender, EventArgs e)
        {
            if (reportMenu.ItemContainerGenerator.Status ==
                System.Windows.Controls.Primitives.GeneratorStatus.ContainersGenerated)
            {
                var containers = reportMenu.Items.Cast<object>().Select(
                    item => (FrameworkElement)reportMenu.ItemContainerGenerator.ContainerFromItem(item));
                foreach (var container in containers)
                {
                    (container as MenuItem).Click += CreateReport_Click;
                }
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ChangePopupPreClose();
            ChangePopupPostClose();
        }

        private void OpenParameters(object sender, RoutedEventArgs e)
        {
            new ParametersWindow().ShowDialog();
        }

        private void ExitApp(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource.GetType() == typeof(MenuItem))
                if (((MenuItem)e.OriginalSource).Name == "menuExit")
                    if (Settings.Default.SHUTDOWN_ON_EXPLICIT)
                        App.Current.Shutdown();
                    else
                        Close();
                else { }
            else if (e.OriginalSource.GetType() == typeof(Button))
                if (((Button)e.OriginalSource).Name == "buttonExit")
                    Close();
                else { }
            else
                Close();
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ChangePopupPreClose();
            DragMove();// Для перемещение окна
            ChangePopupPostClose();
            ChangeWindowState();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadFromSettings();
            ChangeWindowState();
            SelectViewEquipment();
            changeEquipmentPopup.Height = Height - 200;
            changeEquipmentPopup.Width = Width - 400;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            AccountingPCWindowSettings.Default.WindowState = Enum.GetName(typeof(WindowState), WindowState);
            AccountingPCWindowSettings.Default.Width = Width;
            AccountingPCWindowSettings.Default.lastWidth = lastWidth;
            AccountingPCWindowSettings.Default.lastHeignt = lastHeight;
            AccountingPCWindowSettings.Default.Height = Height;
            AccountingPCWindowSettings.Default.Save();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (NowView)
            {
                case View.Equipment:
                    ChangeEquipmentView();
                    break;
                case View.Software:
                    ChangeSoftwareView();
                    break;
                case View.Location:
                    break;
            }
        }

        private void AddDevice(object sender, RoutedEventArgs e)
        {
            TypeChange = TypeChange.Add;
            changeEquipmentPopup.IsOpen = true;
        }

        private void ChangeDevice(object sender, RoutedEventArgs e)
        {
            DataRow row = ((DataRowView)equipmentView.SelectedItem).Row;
            DeviceID = Convert.ToInt32(row[0]);
            TypeChange = TypeChange.Change;
            changeEquipmentPopup.IsOpen = true;
        }

        private void DeleteDevice(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                foreach (object obj in equipmentView.SelectedItems)
                {
                    DataRow row = ((DataRowView)obj).Row;
                    Int32 id = Convert.ToInt32(row[0]);
                    SqlCommand command = new SqlCommand($"Delete{TypeDevice.ToString()}ByID", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@ID", id));
                    Int32 res = command.ExecuteNonQuery();
                }
            }
            statusItem1.Content = "Успешно удалено";
            Task task;
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
            UpdateEquipmentData();
        }

        /* Управление изменением устройств */

        private void SaveChanges(object sender, RoutedEventArgs e)
        {
            SaveOrUpdateEquipmentDB();
            UpdateEquipmentData();
            UpdateImages();
            ChangeEquipmentView();
        }

        private void ChangeEquipmentPopup_Opened(object sender, EventArgs e)
        {
            viewGrid.IsEnabled = false;
            menu.IsEnabled = false;
            if (!IsPreOpenEquipmentPopup)
            {
                InitializePopup();
                switch (TypeChange)
                {
                    case TypeChange.Change:
                        switch (TypeDevice)
                        {
                            case TypeDevice.PC:
                                GetPC(device, DeviceID);
                                break;
                            case TypeDevice.Notebook:
                                GetNotebook(device, DeviceID);
                                break;
                            case TypeDevice.Monitor:
                                GetMonitor(device, DeviceID);
                                break;
                            case TypeDevice.Projector:
                                GetProjector(device, DeviceID);
                                break;
                            case TypeDevice.InteractiveWhiteboard:
                                GetInteractiveWhiteboard(device, DeviceID);
                                break;
                            case TypeDevice.ProjectorScreen:
                                GetProjectorScreen(device, DeviceID);
                                break;
                            case TypeDevice.PrinterScanner:
                                GetPrinterScanner(device, DeviceID);
                                break;
                            case TypeDevice.NetworkSwitch:
                                GetNetworkSwitch(device, DeviceID);
                                break;
                            case TypeDevice.OtherEquipment:
                                GetOtherEquipment(device, DeviceID);
                                break;
                        }
                        SetDeviceLocationAndInvoice(device);
                        break;
                    case TypeChange.Add:
                        switch (TypeDevice)
                        {
                            case TypeDevice.PC:
                                device = new PC();
                                break;
                            case TypeDevice.Notebook:
                                device = new Notebook();
                                break;
                            case TypeDevice.Monitor:
                                device = new Monitor();
                                break;
                            case TypeDevice.Projector:
                                device = new Projector();
                                break;
                            case TypeDevice.InteractiveWhiteboard:
                                device = new InteractiveWhiteboard();
                                break;
                            case TypeDevice.ProjectorScreen:
                                device = new ProjectorScreen();
                                break;
                            case TypeDevice.PrinterScanner:
                                device = new PrinterScanner();
                                break;
                            case TypeDevice.NetworkSwitch:
                                device = new NetworkSwitch();
                                break;
                            case TypeDevice.OtherEquipment:
                                device = new OtherEquipment();
                                break;
                        }
                        break;
                }
            }
        }

        private void PopupClose(object sender, ExecutedRoutedEventArgs e)
        {
            switch (NowView)
            {
                case View.Equipment:
                    changeEquipmentPopup.IsOpen = false;
                    IsPreOpenEquipmentPopup = false;
                    UpdateEquipmentData();
                    UpdateImages();
                    ChangeEquipmentView();
                    break;
                case View.Software:
                    changeSoftwarePopup.IsOpen = false;
                    IsPreOpenSoftwarePopup = false;
                    UpdateSoftwareData();
                    ChangeSoftwareView();
                    break;
            }
            viewGrid.IsEnabled = true;
            menu.IsEnabled = true;
        }

        private void Window_LostFocus(object sender, RoutedEventArgs e)
        {
            //changePopupPreClose();
        }

        private void Window_GotFocus(object sender, RoutedEventArgs e)
        {
            //changePopupPostClose();
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            ChangePopupPreClose();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            ChangePopupPostClose();
        }

        private void AutoInvN_Checked(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                inventoryNumber.Text = new SqlCommand("SELECT dbo.GetNextInventoryNumber()", connection).ExecuteScalar().ToString();
                inventoryNumber.IsEnabled = false;
            }
        }

        private void autoInvN_Unchecked(object sender, RoutedEventArgs e)
        {
            inventoryNumber.IsEnabled = true;
        }

        private void Cpu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRow row = ((DataRowView)cpu.SelectedItem)?.Row;
            frequency.Text = row?[1].ToString();
            maxFrequency.Text = row?[2].ToString();
            cores.Text = row?[3].ToString();
        }

        private void SelectViewEquipment(object sender, ExecutedRoutedEventArgs e)
        {
            SelectViewEquipment();
        }

        private void SelectViewSoftware(object sender, ExecutedRoutedEventArgs e)
        {
            SelectViewSoftware();
        }

        private void SelectViewInvoice(object sender, ExecutedRoutedEventArgs e)
        {
            SelectViewInvoice();
        }

        private void SelectViewLocation(object sender, ExecutedRoutedEventArgs e)
        {
            SelectViewLocation();
        }

        private void AddSoftware(object sender, RoutedEventArgs e)
        {
            TypeChange = TypeChange.Add;
            changeSoftwarePopup.IsOpen = true;
        }

        private void ChangeSoftware(object sender, RoutedEventArgs e)
        {
            DataRow row = ((DataRowView)softwareView.SelectedItem).Row;
            DeviceID = Convert.ToInt32(row[0]);
            TypeChange = TypeChange.Change;
            changeSoftwarePopup.IsOpen = true;
        }

        private void DeleteSoftware(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                foreach (object obj in softwareView.SelectedItems)
                {
                    DataRow row = ((DataRowView)obj).Row;
                    Int32 id = Convert.ToInt32(row[0]);
                    SqlCommand command = new SqlCommand($"Delete{TypeSoft.ToString()}", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@ID", id));
                    Int32 res = command.ExecuteNonQuery();
                }
            }
            statusItem1.Content = "Успешно удалено";
            Task task;
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
            UpdateSoftwareData();
        }

        private void EquipmentView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DeviceID = Convert.ToInt32(((DataRowView)equipmentView?.SelectedItem)?.Row?[0]);
            BitmapFrame frame = null;
            //deviceImage.Source = BitmapFrame.Create();
            int col;
            switch (TypeDevice)
            {
                case TypeDevice.PC:
                case TypeDevice.Notebook:
                case TypeDevice.Monitor:
                case TypeDevice.Projector:
                    col = equipmentView.Columns.Count - 2;
                    break;
                default:
                    col = equipmentView.Columns.Count - 1;
                    break;
            }
            object obj = equipmentView.SelectedItems.Count > 0 ? (((DataRowView)equipmentView.SelectedItems?[0])?.Row[col]) : 0;
            int id = Convert.ToInt32(obj.GetType() == typeof(DBNull) ? 0 : obj);
            if (id != 0)
            {
                using (MemoryStream stream = new MemoryStream(images[id]))
                {
                    frame = BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                }
            }
            deviceImage.Source = frame;
            UpdateSoftwareOnDevice();
        }

        private void ImageLoad_Click(object sender, RoutedEventArgs e)
        {
            ChangePopupPreClose();
            //changePopup.Opacity = 1;
            System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();
            dialog.Filter = "Image Files(*.BMP;*.PNG;*.JPG;*.GIF)|*.BMP;*.PNG;*.JPG;*.GIF";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                return;
            ChangePopupPostClose();
            //changePopup.Opacity = 0;
            imageFilename.Text = dialog.FileName;
        }

        private void UpdateView_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            switch (NowView)
            {
                case View.Equipment:
                    UpdateAllEquipmentData();
                    ChangeEquipmentView();
                    UpdateImages();
                    break;
                case View.Software:
                    UpdateAllSoftwareData();
                    ChangeSoftwareView();
                    break;
                case View.Location:
                    break;
            }
        }

        private void InventoryNumber_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void InventoryNumber_Error(object sender, ValidationErrorEventArgs e)
        {
            return;
        }

        private void ChangeAnalog_Checked(object sender, RoutedEventArgs e)
        {
            IsChangeAnalog = true;
        }

        private void ChangeAnalog_Unchecked(object sender, RoutedEventArgs e)
        {
            IsChangeAnalog = false;
        }

        private void DisabledRepeatInvN_Checked(object sender, RoutedEventArgs e)
        {
            DisabledRepeatInvN_Checked();
        }

        private void DisabledRepeatInvN_Unchecked(object sender, RoutedEventArgs e)
        {
            DisabledRepeatInvN_Unchecked();
        }

        private void AddSoftware_Click(object sender, RoutedEventArgs e)
        {
            ((Button)sender).ContextMenu.IsOpen = true;
        }

        private void DelSoftware_Click(object sender, RoutedEventArgs e)
        {
            int licenseID = ((InstalledSoftware)softwareOnDevice.SelectedItem).ID;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = null;
                switch (TypeDevice)
                {
                    case TypeDevice.PC:
                        command = new SqlCommand($"DeleteInstalledSoftwarePC", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@PCID", DeviceID));
                        command.Parameters.Add(new SqlParameter("@LicenseID", licenseID));
                        break;
                    case TypeDevice.Notebook:
                        command = new SqlCommand($"DeleteInstalledSoftwareNotebook", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@NotebookID", DeviceID));
                        command.Parameters.Add(new SqlParameter("@LicenseID", licenseID));
                        break;
                }
                command?.ExecuteNonQuery();
            }
            UpdateSoftwareOnDevice();
        }

        private void AddSoftwareItem(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            InstalledSoftware software = (InstalledSoftware)button.DataContext;
            int licenseID = software.ID;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = null;
                switch (TypeDevice)
                {
                    case TypeDevice.PC:
                        command = new SqlCommand($"AddInstalledSoftwarePC", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@PCID", DeviceID));
                        command.Parameters.Add(new SqlParameter("@LicenseID", licenseID));
                        break;
                    case TypeDevice.Notebook:
                        command = new SqlCommand($"AddInstalledSoftwareNotebook", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@NotebookID", DeviceID));
                        command.Parameters.Add(new SqlParameter("@LicenseID", licenseID));
                        break;
                }
                command?.ExecuteNonQuery();
            }
            UpdateSoftwareOnDevice();
        }

        private void ChangeSoftwarePopup_Opened(object sender, EventArgs e)
        {
            viewGrid.IsEnabled = false;
            menu.IsEnabled = false;
            switch (TypeChange)
            {
                case TypeChange.Add:
                    switch (TypeSoft)
                    {
                        case TypeSoft.LicenseSoftware:
                            soft = new LicenseSoftware();
                            //AddSoftware();
                            break;
                        case TypeSoft.OS:
                            soft = new OS();
                            //AddOS();
                            break;
                    }
                    break;
                case TypeChange.Change:
                    switch (TypeSoft)
                    {
                        case TypeSoft.LicenseSoftware:
                            GetLicenseSoftware(soft, SoftwareID);
                            //AddSoftware();
                            break;
                        case TypeSoft.OS:
                            GetOS(soft, SoftwareID);
                            //AddOS();
                            break;
                    }
                    break;
            }
        }

        private void SoftwareView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SoftwareID = Convert.ToInt32(((DataRowView)softwareView?.SelectedItem)?.Row?[0]);
        }

        private void SaveChangesForSoftware(object sender, RoutedEventArgs e)
        {
            SaveOrUpdateSoftwareDB();
            UpdateSoftwareData();
            ChangeSoftwareView();
        }

        private void CreateReport_Click(object sender, RoutedEventArgs e)
        {
            //System.Windows.Forms.SaveFileDialog dialog = new System.Windows.Forms.SaveFileDialog();
            //dialog.Filter = "Excel | *.xlsx;*.xls";
            //dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            //dialog.FileName = $"Report_{DateTime.Now.ToString("dd-MM-yyyy__HH-mm-ss__g")}.xlsx";
            //if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
            //    return;
            //Report.SaveReport(dialog.FileName);
            ReportName reportName = ((sender as MenuItem).Header as ReportName);
            if (reportName!=null)
                new ConfiguringReportWindow(reportName.Type).ShowDialog();
            else
                new ConfiguringReportWindow().ShowDialog();
        }
    }
}
