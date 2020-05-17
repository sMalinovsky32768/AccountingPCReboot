using AccountingPC.Properties;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Controls.Primitives;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Media;

namespace AccountingPC
{
    enum TypeChange
    {
        Add,
        Change,
    }

    enum View
    {
        Equipment,
        Software,
        Location,
    }

    /// <summary>
    /// Логика взаимодействия для AccountingPCWindow.xaml
    /// </summary>
    public partial class AccountingPCWindow : Window
    {
        private View nowView;

        private Dictionary<int, byte[]> images;
        ToolTip inventoryNumberToolTip;
        //public static readonly DependencyProperty AvailableInvNProperty;
        //public Boolean AvailableInvN
        //{
        //    get { return (bool)GetValue(AvailableInvNProperty); }
        //    set { SetValue(AvailableInvNProperty, value); }
        //}
        public double lastHeight;
        public double lastWidth;
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
        public static readonly RoutedCommand SelectViewLocationCommand = new RoutedUICommand(
            "SelectViewLocation", "SelectViewLocationCommand", typeof(AccountingPCWindow),
            new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.L, ModifierKeys.Alt) }));

        public static readonly RoutedCommand UpdateViewCommand = new RoutedUICommand(
            "UpdateView", "UpdateViewCommand", typeof(AccountingPCWindow),
            new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.F5), new KeyGesture(Key.R, ModifierKeys.Control) }));
        TypeDevice typeDevice;
        TypeChange typeChange;
        Int32 deviceID;
        bool isPreOpenPopup; 
        List<ListBoxItem> videoConnectorsItems;

        SqlDataAdapter aspectRatioDataAdapter;
        SqlDataAdapter cpuDataAdapter;
        SqlDataAdapter softwareDataAdapter;
        SqlDataAdapter pcSoftwareDataAdapter;
        SqlDataAdapter notebookSoftwareDataAdapter;
        SqlDataAdapter osDataAdapter;
        SqlDataAdapter screenInstalledDataAdapter;
        SqlDataAdapter typeDeviceDataAdapter;
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
        SqlDataAdapter typeLicenseDataAdapter;
        SqlDataAdapter videoConnectorsDataAdapter;
        SqlDataAdapter wifiFrequencyDataAdapter;
        SqlDataAdapter nameDataAdapter;
        SqlDataAdapter motherboardDataAdapter;
        SqlDataAdapter vCardDataAdapter;

        DataSet aspectRatioDataSet;
        DataSet cpuDataSet;
        DataSet softwareDataSet;
        DataSet pcSoftwareDataSet;
        DataSet notebookSoftwareDataSet;
        DataSet osDataSet;
        DataSet screenInstalledDataSet;
        DataSet typeDeviceDataSet;
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
        DataSet typeLicenseDataSet;
        DataSet videoConnectorsDataSet;
        DataSet wifiFrequencyDataSet;
        DataSet nameDataSet;
        DataSet motherboardDataSet;
        DataSet vCardDataSet;

        static AccountingPCWindow()
        {
            //AvailableInvNProperty = DependencyProperty.Register("AvailableInvN", typeof(bool), typeof(AccountingPCWindow));
        }
        public AccountingPCWindow()
        {
            InitializeComponent();
            lastHeight = Height;
            lastWidth = Width;
            UpdateAllData();
            UpdateImages();
            //equipmentCategoryList.SelectedIndex = 0;
            isPreOpenPopup = false;
            nowView = View.Equipment;
            //ValidationRule rule = Validation.
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
            if (WindowState == WindowState.Maximized)
            {
                ((System.Windows.Shapes.Path)buttonMaximized.Template.FindName("Maximize", buttonMaximized)).Visibility = Visibility.Collapsed;
                ((System.Windows.Shapes.Path)buttonMaximized.Template.FindName("Restore", buttonMaximized)).Visibility = Visibility.Visible;
            }
            else if (WindowState == WindowState.Normal)
            {
                ((System.Windows.Shapes.Path)buttonMaximized.Template.FindName("Maximize", buttonMaximized)).Visibility = Visibility.Visible;
                ((System.Windows.Shapes.Path)buttonMaximized.Template.FindName("Restore", buttonMaximized)).Visibility = Visibility.Collapsed;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Height = AccountingPCWindowSettings.Default.Height > MinHeight ? AccountingPCWindowSettings.Default.Height : MinHeight;
            Width = AccountingPCWindowSettings.Default.Width > MinWidth ? AccountingPCWindowSettings.Default.Width : MinWidth;
            lastHeight = AccountingPCWindowSettings.Default.lastHeignt;
            lastWidth = AccountingPCWindowSettings.Default.lastWidth;
            WindowState = AccountingPCWindowSettings.Default.WindowState != string.Empty ? 
                (WindowState)Enum.Parse(typeof(WindowState), AccountingPCWindowSettings.Default.WindowState) : WindowState.Normal;
            if (WindowState == WindowState.Maximized)
            {
                ((System.Windows.Shapes.Path)buttonMaximized.Template.FindName("Maximize", buttonMaximized)).Visibility = Visibility.Collapsed;
                ((System.Windows.Shapes.Path)buttonMaximized.Template.FindName("Restore", buttonMaximized)).Visibility = Visibility.Visible;
            }
            else if (WindowState == WindowState.Normal)
            {
                ((System.Windows.Shapes.Path)buttonMaximized.Template.FindName("Maximize", buttonMaximized)).Visibility = Visibility.Visible;
                ((System.Windows.Shapes.Path)buttonMaximized.Template.FindName("Restore", buttonMaximized)).Visibility = Visibility.Collapsed;
            }
            changePopup.Height = Height - 200;
            changePopup.Width = Width - 400;
            equipmentGrid.Visibility = Visibility.Visible;
            equipmentCategoryList.SelectedIndex = 0;
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
            switch (nowView)
            {
                case View.Equipment:
                    //UpdateData();
                    ChangeView();
                    break;
                case View.Software:
                    break;
                case View.Location:
                    break;
            }
        }

        private void AddDevice(object sender, RoutedEventArgs e)
        {
            typeChange = TypeChange.Add;
            changePopup.IsOpen = true;
        }

        private void ChangeDevice(object sender, RoutedEventArgs e)
        {
            DataRow row = ((DataRowView)equipmentView.SelectedItem).Row;
            deviceID = Convert.ToInt32(row[0]);
            typeChange = TypeChange.Change;
            changePopup.IsOpen = true;
        }

        private void DeleteDevice(object sender, RoutedEventArgs e)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                foreach (object obj in equipmentView.SelectedItems)
                {
                    DataRow row = ((DataRowView)obj).Row;
                    Int32 id = Convert.ToInt32(row[0]);
                    SqlCommand command = new SqlCommand($"Delete{typeDevice.ToString()}ByID", connection);
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
            UpdateData();
        }

        private void UpdateData()
        {
            String connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            switch (typeDevice)
            {
                case TypeDevice.PC:
                    pcDataAdapter = new SqlDataAdapter("SELECT * FROM dbo.GetAllPC()", connectionString);
                    pcDataSet = new DataSet();
                    pcDataAdapter.Fill(pcDataSet);
                    pcDataSet.Tables[0].Columns.Add("Видеоразъемы");
                    foreach (DataRow row in pcDataSet.Tables[0].Rows)
                    {
                        row[20] = row[16].GetType() == typeof(int) ? GetVideoConnectors(Convert.ToInt32(row[16])) : row[16];
                    }
                    break;
                case TypeDevice.Notebook:
                    notebookDataAdapter = new SqlDataAdapter("SELECT * FROM dbo.GetAllNotebook()", connectionString);
                    notebookDataSet = new DataSet();
                    notebookDataAdapter.Fill(notebookDataSet);
                    notebookDataSet.Tables[0].Columns.Add("Видеоразъемы");
                    foreach (DataRow row in notebookDataSet.Tables[0].Rows)
                    {
                        row[24] = row[19].GetType() == typeof(int) ? GetVideoConnectors(Convert.ToInt32(row[19])) : row[19];
                    }
                    break;
                case TypeDevice.Monitor:
                    monitorDataAdapter = new SqlDataAdapter("SELECT * FROM dbo.GetAllMonitor()", connectionString);
                    monitorDataSet = new DataSet();
                    monitorDataAdapter.Fill(monitorDataSet);
                    monitorDataSet.Tables[0].Columns.Add("Видеоразъемы");
                    foreach (DataRow row in monitorDataSet.Tables[0].Rows)
                    {
                        row[12] = row[9].GetType() == typeof(int) ? GetVideoConnectors(Convert.ToInt32(row[9])) : row[9];
                    }
                    break;
                case TypeDevice.Projector:
                    projectorDataAdapter = new SqlDataAdapter("SELECT * FROM dbo.GetAllProjector()", connectionString);
                    projectorDataSet = new DataSet();
                    projectorDataAdapter.Fill(projectorDataSet);
                    projectorDataSet.Tables[0].Columns.Add("Видеоразъемы");
                    foreach (DataRow row in projectorDataSet.Tables[0].Rows)
                    {
                        row[11] = row[8].GetType() == typeof(int) ? GetVideoConnectors(Convert.ToInt32(row[8])) : row[8];
                    }
                    break;
                case TypeDevice.InteractiveWhiteboard:
                    boardDataAdapter = new SqlDataAdapter("SELECT * FROM dbo.GetAllBoard()", connectionString);
                    boardDataSet = new DataSet();
                    boardDataAdapter.Fill(boardDataSet);
                    break;
                case TypeDevice.ProjectorScreen:
                    projectorScreenDataAdapter = new SqlDataAdapter("SELECT * FROM dbo.GetAllScreen()", connectionString);
                    projectorScreenDataSet = new DataSet();
                    projectorScreenDataAdapter.Fill(projectorScreenDataSet);
                    break;
                case TypeDevice.PrinterScanner:
                    printerScannerDataAdapter = new SqlDataAdapter("SELECT * FROM dbo.GetAllPrinterScanner()", connectionString);
                    printerScannerDataSet = new DataSet();
                    printerScannerDataAdapter.Fill(printerScannerDataSet);
                    break;
                case TypeDevice.NetworkSwitch:
                    networkSwitchDataAdapter = new SqlDataAdapter("SELECT * FROM dbo.GetAllNetworkSwitch()", connectionString);
                    networkSwitchDataSet = new DataSet();
                    networkSwitchDataAdapter.Fill(networkSwitchDataSet);
                    break;
                case TypeDevice.OtherEquipment:
                    otherEquipmentDataAdapter = new SqlDataAdapter("SELECT * FROM dbo.GetAllOtherEquipment()", connectionString);
                    otherEquipmentDataSet = new DataSet();
                    otherEquipmentDataAdapter.Fill(otherEquipmentDataSet);
                    break;
            }
        }

        private void UpdateAllData()
        {
            String connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            pcDataAdapter = new SqlDataAdapter("SELECT * FROM dbo.GetAllPC()", connectionString);
            pcDataSet = new DataSet();
            pcDataAdapter.Fill(pcDataSet);
            pcDataSet.Tables[0].Columns.Add("Видеоразъемы");
            foreach (DataRow row in pcDataSet.Tables[0].Rows)
            {
                row[20] = row[16].GetType() == typeof(int) ? GetVideoConnectors(Convert.ToInt32(row[16])) : row[16];
            }

            notebookDataAdapter = new SqlDataAdapter("SELECT * FROM dbo.GetAllNotebook()", connectionString);
            notebookDataSet = new DataSet();
            notebookDataAdapter.Fill(notebookDataSet);
            notebookDataSet.Tables[0].Columns.Add("Видеоразъемы");
            foreach (DataRow row in notebookDataSet.Tables[0].Rows)
            {
                row[24] = row[19].GetType() == typeof(int) ? GetVideoConnectors(Convert.ToInt32(row[19])) : row[19];
            }

            monitorDataAdapter = new SqlDataAdapter("SELECT * FROM dbo.GetAllMonitor()", connectionString);
            monitorDataSet = new DataSet();
            monitorDataAdapter.Fill(monitorDataSet);
            monitorDataSet.Tables[0].Columns.Add("Видеоразъемы");
            foreach (DataRow row in monitorDataSet.Tables[0].Rows)
            {
                row[12] = row[9].GetType() == typeof(int) ? GetVideoConnectors(Convert.ToInt32(row[9])) : row[9];
            }

            projectorDataAdapter = new SqlDataAdapter("SELECT * FROM dbo.GetAllProjector()", connectionString);
            projectorDataSet = new DataSet();
            projectorDataAdapter.Fill(projectorDataSet);
            projectorDataSet.Tables[0].Columns.Add("Видеоразъемы");
            foreach (DataRow row in projectorDataSet.Tables[0].Rows)
            {
                row[11] = row[8].GetType() == typeof(int) ? GetVideoConnectors(Convert.ToInt32(row[8])) : row[8];
            }

            boardDataAdapter = new SqlDataAdapter("SELECT * FROM dbo.GetAllBoard()", connectionString);
            boardDataSet = new DataSet();
            boardDataAdapter.Fill(boardDataSet);

            projectorScreenDataAdapter = new SqlDataAdapter("SELECT * FROM dbo.GetAllScreen()", connectionString);
            projectorScreenDataSet = new DataSet();
            projectorScreenDataAdapter.Fill(projectorScreenDataSet);

            printerScannerDataAdapter = new SqlDataAdapter("SELECT * FROM dbo.GetAllPrinterScanner()", connectionString);
            printerScannerDataSet = new DataSet();
            printerScannerDataAdapter.Fill(printerScannerDataSet);

            networkSwitchDataAdapter = new SqlDataAdapter("SELECT * FROM dbo.GetAllNetworkSwitch()", connectionString);
            networkSwitchDataSet = new DataSet();
            networkSwitchDataAdapter.Fill(networkSwitchDataSet);

            otherEquipmentDataAdapter = new SqlDataAdapter("SELECT * FROM dbo.GetAllOtherEquipment()", connectionString);
            otherEquipmentDataSet = new DataSet();
            otherEquipmentDataAdapter.Fill(otherEquipmentDataSet);
        }

        private void ChangeView()
        {
            switch (equipmentCategoryList.SelectedIndex)
            {
                case 0:
                    equipmentView.ItemsSource = pcDataSet.Tables[0].DefaultView;
                    if (equipmentView.Columns.Count > 0)
                    {
                        equipmentView.Columns[0].Visibility = Visibility.Collapsed;
                        equipmentView.Columns[16].Visibility = Visibility.Collapsed;
                        //equipmentView.Columns[equipmentView.Columns.Count - 1].Visibility = Visibility.Collapsed;
                        equipmentView.Columns[equipmentView.Columns.Count - 2].Visibility = Visibility.Collapsed;
                    }
                    typeDevice = TypeDevice.PC;
                    break;
                case 1:
                    equipmentView.ItemsSource = notebookDataSet.Tables[0].DefaultView;
                    equipmentView.Columns[0].Visibility = Visibility.Collapsed;
                    equipmentView.Columns[19].Visibility = Visibility.Collapsed;
                    //equipmentView.Columns[equipmentView.Columns.Count - 1].Visibility = Visibility.Collapsed;
                    equipmentView.Columns[equipmentView.Columns.Count - 2].Visibility = Visibility.Collapsed;
                    typeDevice = TypeDevice.Notebook;
                    break;
                case 2:
                    equipmentView.ItemsSource = monitorDataSet.Tables[0].DefaultView;
                    equipmentView.Columns[0].Visibility = Visibility.Collapsed;
                    equipmentView.Columns[9].Visibility = Visibility.Collapsed;
                    //equipmentView.Columns[equipmentView.Columns.Count - 1].Visibility = Visibility.Collapsed;
                    equipmentView.Columns[equipmentView.Columns.Count - 2].Visibility = Visibility.Collapsed;
                    typeDevice = TypeDevice.Monitor;
                    break;
                case 3:
                    equipmentView.ItemsSource = projectorDataSet.Tables[0].DefaultView;
                    equipmentView.Columns[0].Visibility = Visibility.Collapsed;
                    equipmentView.Columns[8].Visibility = Visibility.Collapsed;
                    //equipmentView.Columns[equipmentView.Columns.Count - 1].Visibility = Visibility.Collapsed;
                    equipmentView.Columns[equipmentView.Columns.Count - 2].Visibility = Visibility.Collapsed;
                    typeDevice = TypeDevice.Projector;
                    break;
                case 4:
                    equipmentView.ItemsSource = boardDataSet.Tables[0].DefaultView;
                    equipmentView.Columns[0].Visibility = Visibility.Collapsed;
                    equipmentView.Columns[equipmentView.Columns.Count - 1].Visibility = Visibility.Collapsed;
                    typeDevice = TypeDevice.InteractiveWhiteboard;
                    break;
                case 5:
                    equipmentView.ItemsSource = projectorScreenDataSet.Tables[0].DefaultView;
                    equipmentView.Columns[0].Visibility = Visibility.Collapsed;
                    equipmentView.Columns[equipmentView.Columns.Count - 1].Visibility = Visibility.Collapsed;
                    typeDevice = TypeDevice.ProjectorScreen;
                    break;
                case 6:
                    equipmentView.ItemsSource = printerScannerDataSet.Tables[0].DefaultView;
                    equipmentView.Columns[0].Visibility = Visibility.Collapsed;
                    equipmentView.Columns[equipmentView.Columns.Count - 1].Visibility = Visibility.Collapsed;
                    typeDevice = TypeDevice.PrinterScanner;
                    break;
                case 7:
                    equipmentView.ItemsSource = networkSwitchDataSet.Tables[0].DefaultView;
                    equipmentView.Columns[0].Visibility = Visibility.Collapsed;
                    equipmentView.Columns[equipmentView.Columns.Count - 1].Visibility = Visibility.Collapsed;
                    typeDevice = TypeDevice.NetworkSwitch;
                    break;
                case 8:
                    equipmentView.ItemsSource = otherEquipmentDataSet.Tables[0].DefaultView;
                    equipmentView.Columns[0].Visibility = Visibility.Collapsed;
                    equipmentView.Columns[equipmentView.Columns.Count - 1].Visibility = Visibility.Collapsed;
                    typeDevice = TypeDevice.OtherEquipment;
                    break;
            }
        }

        /* Управление изменением устройств */

        public CustomPopupPlacement[] ChangePopupPlacement(Size popupSize, Size targetSize, Point offset)
        {
            CustomPopupPlacement placement1 =
               new CustomPopupPlacement(new Point(0, 0), PopupPrimaryAxis.Vertical);

            CustomPopupPlacement[] ttplaces =
                    new CustomPopupPlacement[] { placement1 };
            return ttplaces;
        }

        private void SaveChanges(object sender, RoutedEventArgs e)
        {
            Task task;
            String connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                String commandString;
                SqlCommand command;
                int temp;
                switch (typeChange)
                {
                    case TypeChange.Add:
                        switch (typeDevice)
                        {
                            case TypeDevice.PC:
                                commandString = "AddPC";
                                command = new SqlCommand(commandString, connection);
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt32(inventoryNumber.Text)));
                                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToInt32(cost.Text)));
                                command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text == String.Empty ? null : invoice.Text));
                                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?[0]));
                                command.Parameters.Add(new SqlParameter("@CPU", cpu.Text == String.Empty ? null : cpu.Text));
                                temp = Convert.ToInt32(cores.Text);
                                if (temp > 0)
                                    command.Parameters.Add(new SqlParameter("@Cores", temp));
                                temp = Convert.ToInt32(frequency.Text);
                                if (temp > 0)
                                    command.Parameters.Add(new SqlParameter("@Frequency", temp));
                                temp = Convert.ToInt32(maxFrequency.Text);
                                if (temp > 0)
                                    command.Parameters.Add(new SqlParameter("@MaxFrequency", temp));
                                temp = Convert.ToInt32(ram.Text);
                                if (temp > 0)
                                    command.Parameters.Add(new SqlParameter("@RAM", temp));
                                temp = Convert.ToInt32(ramFrequency.Text);
                                if (temp > 0)
                                    command.Parameters.Add(new SqlParameter("@FrequencyRAM", temp));
                                temp = Convert.ToInt32(ssd.Text);
                                if (temp > 0)
                                    command.Parameters.Add(new SqlParameter("@SSD", temp));
                                temp = Convert.ToInt32(hdd.Text);
                                if (temp > 0)
                                    command.Parameters.Add(new SqlParameter("@HDD", temp));
                                command.Parameters.Add(new SqlParameter("@Video", vCard.Text == String.Empty ? null : vCard.Text));
                                temp = Convert.ToInt32(videoram.Text);
                                if (temp > 0)
                                    command.Parameters.Add(new SqlParameter("@VRAM",temp));
                                command.Parameters.Add(new SqlParameter("@OSID", ((DataRowView)os?.SelectedItem)?[0]));
                                command.Parameters.Add(new SqlParameter("@MB", motherboard.Text));
                                command.Parameters.Add(new SqlParameter("@VConnectors", GetValueVideoConnectors(vConnectors)));
                                command.Parameters.Add(new SqlParameter("@Image", LoadImage(imageFilename.Text)));
                                command.ExecuteNonQuery();
                                break;
                            case TypeDevice.Notebook:
                                commandString = "AddNotebook";
                                command = new SqlCommand(commandString, connection);
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt32(inventoryNumber.Text)));
                                command.Parameters.Add(new SqlParameter("@Type", ((DataRowView)type?.SelectedItem)?[0]));
                                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToInt32(cost.Text)));
                                command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text == String.Empty ? null : invoice.Text));
                                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?[0]));
                                command.Parameters.Add(new SqlParameter("@Diagonal", Convert.ToSingle(diagonal.Text)));
                                command.Parameters.Add(new SqlParameter("@CPU", cpu.Text));
                                temp = Convert.ToInt32(cores.Text);
                                if (temp > 0)
                                    command.Parameters.Add(new SqlParameter("@Cores", temp));
                                temp = Convert.ToInt32(frequency.Text);
                                if (temp > 0)
                                    command.Parameters.Add(new SqlParameter("@Frequency", temp));
                                temp = Convert.ToInt32(maxFrequency.Text);
                                if (temp > 0)
                                    command.Parameters.Add(new SqlParameter("@MaxFrequency", temp));
                                temp = Convert.ToInt32(ram.Text);
                                if (temp > 0)
                                    command.Parameters.Add(new SqlParameter("@RAM", temp));
                                temp = Convert.ToInt32(ramFrequency.Text);
                                if (temp > 0)
                                    command.Parameters.Add(new SqlParameter("@FrequencyRAM", temp));
                                temp = Convert.ToInt32(ssd.Text);
                                if (temp > 0)
                                    command.Parameters.Add(new SqlParameter("@SSD", temp));
                                temp = Convert.ToInt32(hdd.Text);
                                if (temp > 0)
                                    command.Parameters.Add(new SqlParameter("@HDD", temp));
                                command.Parameters.Add(new SqlParameter("@Video", vCard.Text == String.Empty ? null : vCard.Text));
                                temp = Convert.ToInt32(videoram.Text);
                                if (temp > 0)
                                    command.Parameters.Add(new SqlParameter("@VRAM", temp));
                                command.Parameters.Add(new SqlParameter("@OSID", ((DataRowView)os?.SelectedItem)?[0]));
                                command.Parameters.Add(new SqlParameter("@ResolutionID", ((DataRowView)resolution?.SelectedItem)?[0]));
                                command.Parameters.Add(new SqlParameter("@FrequencyID", ((DataRowView)screenFrequency?.SelectedItem)?[0]));
                                command.Parameters.Add(new SqlParameter("@MatrixID", ((DataRowView)matrixTechnology?.SelectedItem)?[0]));
                                command.Parameters.Add(new SqlParameter("@VConnectors", GetValueVideoConnectors(vConnectors)));
                                command.Parameters.Add(new SqlParameter("@Image", LoadImage(imageFilename.Text)));
                                command.ExecuteNonQuery();
                                break;
                            case TypeDevice.Monitor:
                                commandString = "AddMonitor";
                                command = new SqlCommand(commandString, connection);
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt32(inventoryNumber.Text)));
                                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToInt32(cost.Text)));
                                command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text == String.Empty ? null : invoice.Text));
                                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?[0]));
                                command.Parameters.Add(new SqlParameter("@Diagonal", Convert.ToSingle(diagonal.Text)));
                                command.Parameters.Add(new SqlParameter("@ResolutionID", ((DataRowView)resolution?.SelectedItem)?[0]));
                                command.Parameters.Add(new SqlParameter("@FrequencyID", ((DataRowView)screenFrequency?.SelectedItem)?[0]));
                                command.Parameters.Add(new SqlParameter("@MatrixID", ((DataRowView)matrixTechnology?.SelectedItem)?[0]));
                                command.Parameters.Add(new SqlParameter("@VConnectors", GetValueVideoConnectors(vConnectors)));
                                command.Parameters.Add(new SqlParameter("@Image", LoadImage(imageFilename.Text)));
                                command.ExecuteNonQuery();
                                break;
                            case TypeDevice.NetworkSwitch:
                                commandString = "AddNetworkSwitch";
                                command = new SqlCommand(commandString, connection);
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt32(inventoryNumber.Text)));
                                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToInt32(cost.Text)));
                                command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text == String.Empty ? null : invoice.Text));
                                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?[0]));
                                temp = Convert.ToInt32(ports.Text);
                                if (temp > 0)
                                    command.Parameters.Add(new SqlParameter("@Ports", temp));
                                command.Parameters.Add(new SqlParameter("@TypeID", ((DataRowView)type?.SelectedItem)?[0]));
                                command.Parameters.Add(new SqlParameter("@Frequency", ((DataRowView)wifiFrequency?.SelectedItem)?[0]));
                                command.Parameters.Add(new SqlParameter("@Image", LoadImage(imageFilename.Text)));
                                command.ExecuteNonQuery();
                                break;
                            case TypeDevice.InteractiveWhiteboard:
                                commandString = "AddInteractiveWhiteboard";
                                command = new SqlCommand(commandString, connection);
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt32(inventoryNumber.Text)));
                                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToInt32(cost.Text)));
                                command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text == String.Empty ? null : invoice.Text));
                                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?[0]));
                                command.Parameters.Add(new SqlParameter("@Diagonal", Convert.ToSingle(diagonal.Text)));
                                command.Parameters.Add(new SqlParameter("@Image", LoadImage(imageFilename.Text)));
                                command.ExecuteNonQuery();
                                break;
                            case TypeDevice.PrinterScanner:
                                commandString = "AddPrinterScanner";
                                command = new SqlCommand(commandString, connection);
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt32(inventoryNumber.Text)));
                                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToInt32(cost.Text)));
                                command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text == String.Empty ? null : invoice.Text));
                                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?[0]));
                                command.Parameters.Add(new SqlParameter("@TypeID", ((DataRowView)type?.SelectedItem)?[0]));
                                command.Parameters.Add(new SqlParameter("@PaperSizeID", ((DataRowView)paperSize?.SelectedItem)?[0]));
                                command.Parameters.Add(new SqlParameter("@Image", LoadImage(imageFilename.Text)));
                                command.ExecuteNonQuery();
                                break;
                            case TypeDevice.Projector:
                                commandString = "AddProjector";
                                command = new SqlCommand(commandString, connection);
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt32(inventoryNumber.Text)));
                                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToInt32(cost.Text)));
                                command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text == String.Empty ? null : invoice.Text));
                                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?[0]));
                                command.Parameters.Add(new SqlParameter("@TechnologyID", ((DataRowView)projectorTechnology?.SelectedItem)?[0]));
                                command.Parameters.Add(new SqlParameter("@Diagonal", Convert.ToSingle(diagonal.Text)));
                                command.Parameters.Add(new SqlParameter("@ResolutionID", ((DataRowView)resolution?.SelectedItem)?[0]));
                                command.Parameters.Add(new SqlParameter("@VConnectors", GetValueVideoConnectors(vConnectors)));
                                command.Parameters.Add(new SqlParameter("@Image", LoadImage(imageFilename.Text)));
                                command.ExecuteNonQuery();
                                break;
                            case TypeDevice.ProjectorScreen:
                                commandString = "AddProjectorScreen";
                                command = new SqlCommand(commandString, connection);
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt32(inventoryNumber.Text)));
                                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToInt32(cost.Text)));
                                command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text == String.Empty ? null : invoice.Text));
                                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?[0]));
                                command.Parameters.Add(new SqlParameter("@Diagonal", Convert.ToSingle(diagonal.Text)));
                                command.Parameters.Add(new SqlParameter("@IsElectronic", Convert.ToBoolean(isEDrive.IsChecked)));
                                command.Parameters.Add(new SqlParameter("@AspectRatioID", ((DataRowView)aspectRatio?.SelectedItem)?[0]));
                                command.Parameters.Add(new SqlParameter("@InstalledID", ((DataRowView)screenInstalled?.SelectedItem)?[0]));
                                command.Parameters.Add(new SqlParameter("@Image", LoadImage(imageFilename.Text)));
                                command.ExecuteNonQuery();
                                break;
                            case TypeDevice.OtherEquipment:
                                commandString = "AddProjectorScreen";
                                command = new SqlCommand(commandString, connection);
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt32(inventoryNumber.Text)));
                                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToInt32(cost.Text)));
                                command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text == String.Empty ? null : invoice.Text));
                                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?[0]));
                                command.Parameters.Add(new SqlParameter("@Image", LoadImage(imageFilename.Text)));
                                command.ExecuteNonQuery();
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
                                commandString = "UpdatePCByID";
                                command = new SqlCommand(commandString, connection);
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.Add(new SqlParameter("@ID", deviceID));
                                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt32(inventoryNumber.Text)));
                                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToInt32(cost.Text)));
                                command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text));
                                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?[0]));
                                command.Parameters.Add(new SqlParameter("@CPU", cpu.Text));
                                command.Parameters.Add(new SqlParameter("@Cores", Convert.ToInt32(cores.Text)));
                                command.Parameters.Add(new SqlParameter("@Frequency", Convert.ToInt32(frequency.Text)));
                                command.Parameters.Add(new SqlParameter("@MaxFrequency", Convert.ToInt32(maxFrequency.Text)));
                                command.Parameters.Add(new SqlParameter("@RAM", Convert.ToInt32(ram.Text)));
                                command.Parameters.Add(new SqlParameter("@FrequencyRAM", Convert.ToInt32(ramFrequency.Text)));
                                command.Parameters.Add(new SqlParameter("@SSD", Convert.ToInt32(ssd.Text)));
                                command.Parameters.Add(new SqlParameter("@HDD", Convert.ToInt32(hdd.Text)));
                                command.Parameters.Add(new SqlParameter("@Video", vCard.Text));
                                command.Parameters.Add(new SqlParameter("@VRAM", Convert.ToInt32(videoram.Text)));
                                command.Parameters.Add(new SqlParameter("@OSID", ((DataRowView)os?.SelectedItem)?[0]));
                                command.Parameters.Add(new SqlParameter("@MB", motherboard.Text));
                                command.Parameters.Add(new SqlParameter("@VConnectors", GetValueVideoConnectors(vConnectors)));
                                command.Parameters.Add(new SqlParameter("@Image", LoadImage(imageFilename.Text)));
                                command.ExecuteNonQuery();
                                break;
                            case TypeDevice.Notebook:
                                commandString = "UpdateNotebookByID";
                                command = new SqlCommand(commandString, connection);
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.Add(new SqlParameter("@ID", deviceID));
                                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt32(inventoryNumber.Text)));
                                command.Parameters.Add(new SqlParameter("@Type", ((DataRowView)type?.SelectedItem)?[0]));
                                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToInt32(cost.Text)));
                                command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text));
                                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?[0]));
                                command.Parameters.Add(new SqlParameter("@Diagonal", Convert.ToSingle(diagonal.Text)));
                                command.Parameters.Add(new SqlParameter("@CPU", cpu.Text));
                                command.Parameters.Add(new SqlParameter("@Cores", Convert.ToInt32(cores.Text)));
                                command.Parameters.Add(new SqlParameter("@Frequency", Convert.ToInt32(frequency.Text)));
                                command.Parameters.Add(new SqlParameter("@MaxFrequency", Convert.ToInt32(maxFrequency.Text)));
                                command.Parameters.Add(new SqlParameter("@RAM", Convert.ToInt32(ram.Text)));
                                command.Parameters.Add(new SqlParameter("@FrequencyRAM", Convert.ToInt32(ramFrequency.Text)));
                                command.Parameters.Add(new SqlParameter("@SSD", Convert.ToInt32(ssd.Text)));
                                command.Parameters.Add(new SqlParameter("@HDD", Convert.ToInt32(hdd.Text)));
                                command.Parameters.Add(new SqlParameter("@Video", vCard.Text));
                                command.Parameters.Add(new SqlParameter("@VRAM", Convert.ToInt32(videoram.Text)));
                                command.Parameters.Add(new SqlParameter("@OSID", ((DataRowView)os?.SelectedItem)?[0]));
                                command.Parameters.Add(new SqlParameter("@ResolutionID", ((DataRowView)resolution?.SelectedItem)?[0]));
                                command.Parameters.Add(new SqlParameter("@FrequencyID", ((DataRowView)screenFrequency?.SelectedItem)?[0]));
                                command.Parameters.Add(new SqlParameter("@MatrixID", ((DataRowView)matrixTechnology?.SelectedItem)?[0]));
                                command.Parameters.Add(new SqlParameter("@VConnectors", GetValueVideoConnectors(vConnectors)));
                                command.Parameters.Add(new SqlParameter("@Image", LoadImage(imageFilename.Text)));
                                command.ExecuteNonQuery();
                                break;
                            case TypeDevice.Monitor:
                                commandString = "UpdateMonitorByID";
                                command = new SqlCommand(commandString, connection);
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.Add(new SqlParameter("@ID", deviceID));
                                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt32(inventoryNumber.Text)));
                                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToInt32(cost.Text)));
                                command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text));
                                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?[0]));
                                command.Parameters.Add(new SqlParameter("@Diagonal", Convert.ToSingle(diagonal.Text)));
                                command.Parameters.Add(new SqlParameter("@ResolutionID", ((DataRowView)resolution?.SelectedItem)?[0]));
                                command.Parameters.Add(new SqlParameter("@FrequencyID", ((DataRowView)screenFrequency?.SelectedItem)?[0]));
                                command.Parameters.Add(new SqlParameter("@MatrixID", ((DataRowView)matrixTechnology?.SelectedItem)?[0]));
                                command.Parameters.Add(new SqlParameter("@VConnectors", GetValueVideoConnectors(vConnectors)));
                                command.Parameters.Add(new SqlParameter("@Image", LoadImage(imageFilename.Text)));
                                command.ExecuteNonQuery();
                                break;
                            case TypeDevice.NetworkSwitch:
                                commandString = "UpdateNetworkSwitchByID";
                                command = new SqlCommand(commandString, connection);
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.Add(new SqlParameter("@ID", deviceID));
                                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt32(inventoryNumber.Text)));
                                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToInt32(cost.Text)));
                                command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text == String.Empty ? null : invoice.Text));
                                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?[0]));
                                command.Parameters.Add(new SqlParameter("@Ports", Convert.ToInt32(ports.Text)));
                                command.Parameters.Add(new SqlParameter("@TypeID", ((DataRowView)type?.SelectedItem)?[0]));
                                command.Parameters.Add(new SqlParameter("@Frequency", ((DataRowView)wifiFrequency?.SelectedItem)?[0]));
                                command.Parameters.Add(new SqlParameter("@Image", LoadImage(imageFilename.Text)));
                                command.ExecuteNonQuery();
                                break;
                            case TypeDevice.InteractiveWhiteboard:
                                commandString = "UpdateInteractiveWhiteboardByID";
                                command = new SqlCommand(commandString, connection);
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.Add(new SqlParameter("@ID", deviceID));
                                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt32(inventoryNumber.Text)));
                                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToInt32(cost.Text)));
                                command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text == String.Empty ? null : invoice.Text));
                                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?[0]));
                                command.Parameters.Add(new SqlParameter("@Diagonal", Convert.ToSingle(diagonal.Text)));
                                command.Parameters.Add(new SqlParameter("@Image", LoadImage(imageFilename.Text)));
                                command.ExecuteNonQuery();
                                break;
                            case TypeDevice.PrinterScanner:
                                commandString = "UpdatePrinterScannerByID";
                                command = new SqlCommand(commandString, connection);
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.Add(new SqlParameter("@ID", deviceID));
                                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt32(inventoryNumber.Text)));
                                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToInt32(cost.Text)));
                                command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text == String.Empty ? null : invoice.Text));
                                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?[0]));
                                command.Parameters.Add(new SqlParameter("@TypeID", ((DataRowView)type?.SelectedItem)?[0]));
                                command.Parameters.Add(new SqlParameter("@PaperSizeID", ((DataRowView)paperSize?.SelectedItem)?[0]));
                                command.Parameters.Add(new SqlParameter("@Image", LoadImage(imageFilename.Text)));
                                command.ExecuteNonQuery();
                                break;
                            case TypeDevice.Projector:
                                commandString = "UpdateProjectorByID";
                                command = new SqlCommand(commandString, connection);
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.Add(new SqlParameter("@ID", deviceID));
                                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt32(inventoryNumber.Text)));
                                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToInt32(cost.Text)));
                                command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text == String.Empty ? null : invoice.Text));
                                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?[0]));
                                command.Parameters.Add(new SqlParameter("@TechnologyID", ((DataRowView)projectorTechnology?.SelectedItem)?[0]));
                                command.Parameters.Add(new SqlParameter("@Diagonal", Convert.ToSingle(diagonal.Text)));
                                command.Parameters.Add(new SqlParameter("@ResolutionID", ((DataRowView)resolution?.SelectedItem)?[0]));
                                command.Parameters.Add(new SqlParameter("@VConnectors", GetValueVideoConnectors(vConnectors)));
                                command.Parameters.Add(new SqlParameter("@Image", LoadImage(imageFilename.Text)));
                                command.ExecuteNonQuery();
                                break;
                            case TypeDevice.ProjectorScreen:
                                commandString = "UpdateProjectorScreenByID";
                                command = new SqlCommand(commandString, connection);
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.Add(new SqlParameter("@ID", deviceID));
                                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt32(inventoryNumber.Text)));
                                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToInt32(cost.Text)));
                                command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text == String.Empty ? null : invoice.Text));
                                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?[0]));
                                command.Parameters.Add(new SqlParameter("@Diagonal", Convert.ToSingle(diagonal.Text)));
                                command.Parameters.Add(new SqlParameter("@IsElectronic", Convert.ToBoolean(isEDrive.IsChecked)));
                                command.Parameters.Add(new SqlParameter("@AspectRatioID", ((DataRowView)aspectRatio?.SelectedItem)?[0]));
                                command.Parameters.Add(new SqlParameter("@InstalledID", ((DataRowView)screenInstalled?.SelectedItem)?[0]));
                                command.Parameters.Add(new SqlParameter("@Image", LoadImage(imageFilename.Text)));
                                command.ExecuteNonQuery();
                                break;
                            case TypeDevice.OtherEquipment:
                                commandString = "UpdateOtherEquipmentByID";
                                command = new SqlCommand(commandString, connection);
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.Add(new SqlParameter("@ID", deviceID));
                                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt32(inventoryNumber.Text)));
                                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToInt32(cost.Text)));
                                command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text == String.Empty ? null : invoice.Text));
                                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?[0]));
                                command.Parameters.Add(new SqlParameter("@Image", LoadImage(imageFilename.Text)));
                                command.ExecuteNonQuery();
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
                        UpdateData();
                        UpdateImages();
                        ChangeView();
                        break;
                }
                //int res = command.ExecuteNonQuery();
            }
        }

        private void ChangePopup_Opened(object sender, EventArgs e)
        {
            if (!isPreOpenPopup)
            {
                InitializePopup();
                viewGrid.IsEnabled = false;
                menu.IsEnabled = false;
                String commandString;
                SqlDataReader reader;
                String connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                switch (typeChange)
                {
                    case TypeChange.Change:
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            switch (typeDevice)
                            {
                                case TypeDevice.PC:
                                    commandString = $"SELECT * FROM dbo.GetPCByID({deviceID})";
                                    reader = new SqlCommand(commandString, connection).ExecuteReader();
                                    if (reader.HasRows)
                                    {
                                        if (reader.Read())
                                        {
                                            device = new PC()
                                            {
                                                InventoryNumber = Convert.ToUInt32(reader["InventoryNumber"]),
                                                Name = reader["Name"].ToString(),
                                                Cost = Convert.ToUInt32(reader["Cost"]),
                                                Motherboard = reader["MotherBoard"].ToString(),
                                                CPU = reader["CPUModel"].ToString(),
                                                Cores = Convert.ToUInt32(reader["NumberOfCores"].GetType() != typeof(DBNull) ? reader["NumberOfCores"] : 0),
                                                Frequency = Convert.ToUInt32(reader["FrequencyProcessor"].GetType() != typeof(DBNull) ? reader["FrequencyProcessor"] : 0),
                                                MaxFrequency = Convert.ToUInt32(reader["MaxFrequencyProcessor"].GetType() != typeof(DBNull) ? reader["MaxFrequencyProcessor"] : 0),
                                                VCard = reader["VideoCard"].ToString(),
                                                VideoRAM = Convert.ToUInt32(reader["VideoRAMGB"].GetType() != typeof(DBNull) ? reader["VideoRAMGB"] : 0),
                                                RAM = Convert.ToUInt32(reader["RAMGB"].GetType() != typeof(DBNull) ? reader["RAMGB"] : 0),
                                                FrequencyRAM = Convert.ToUInt32(reader["FrequencyRAM"].GetType() != typeof(DBNull) ? reader["FrequencyRAM"] : 0),
                                                SSD = Convert.ToUInt32(reader["SSDCapacityGB"].GetType() != typeof(DBNull) ? reader["SSDCapacityGB"] : 0),
                                                HDD = Convert.ToUInt32(reader["HDDCapacityGB"].GetType() != typeof(DBNull) ? reader["HDDCapacityGB"] : 0),
                                                OSID = Convert.ToUInt32(reader["OSID"].GetType() != typeof(DBNull) ? reader["OSID"] : 0),
                                                //InvoiceID = Convert.ToUInt32(reader["InvoiceID"].GetType() != typeof(DBNull) ? reader["InvoiceID"] : 0),
                                                InvoiceNumber = reader["InvoiceID"].ToString(),
                                                PlaceID = Convert.ToUInt32(reader["PlaceID"].GetType() != typeof(DBNull) ? reader["PlaceID"] : 0),
                                                VideoConnectorsValue = Convert.ToInt32(reader["VideoConnectors"].GetType() != typeof(DBNull) ? reader["VideoConnectors"] : 0),
                                            };
                                            PC pc = device as PC;
                                            pc.VideoConnectors = GetListVideoConnectors(pc.VideoConnectorsValue);

                                            inventoryNumber.Text = device.InventoryNumber.ToString();
                                            name.Text = device.Name;
                                            cost.Text = device.Cost.ToString();
                                            motherboard.Text = pc.Motherboard;
                                            cpu.Text = pc.CPU;
                                            cores.Text = pc.Cores.ToString();
                                            frequency.Text = pc.Frequency.ToString();
                                            maxFrequency.Text = pc.MaxFrequency.ToString();
                                            vCard.Text = pc.VCard;
                                            videoram.Text = pc.VideoRAM.ToString();
                                            hdd.Text = pc.HDD.ToString();
                                            ssd.Text = pc.SSD.ToString();
                                            ram.Text = pc.RAM.ToString();
                                            ramFrequency.Text = pc.FrequencyRAM.ToString();
                                            foreach (object obj in os.ItemsSource)
                                            {
                                                DataRowView row;
                                                row = (obj as DataRowView);
                                                if (Convert.ToUInt32(row.Row[0]) == Convert.ToUInt32(pc.OSID))
                                                {
                                                    os.SelectedItem = row;
                                                    break;
                                                }
                                            }
                                            foreach (object obj in location.ItemsSource)
                                            {
                                                DataRowView row;
                                                row = (obj as DataRowView);
                                                if (Convert.ToUInt32(row.Row[0]) == Convert.ToUInt32(pc.PlaceID))
                                                {
                                                    location.SelectedItem = row;
                                                    break;
                                                }
                                            }
                                            foreach (object obj in vConnectors.Items)
                                            {
                                                ListBoxItem item = obj as ListBoxItem;
                                                foreach (string connector in pc.VideoConnectors)
                                                {
                                                    if (item.Content.ToString() == connector)
                                                    {
                                                        item.IsSelected = true;
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    break;
                                case TypeDevice.Notebook:
                                    commandString = $"SELECT * FROM dbo.GetNotebookByID({deviceID})";
                                    reader = new SqlCommand(commandString, connection).ExecuteReader();
                                    if (reader.HasRows)
                                    {
                                        if (reader.Read())
                                        {
                                            device = new Notebook()
                                            {
                                                InventoryNumber = Convert.ToUInt32(reader["InventoryNumber"]),
                                                Name = reader["Name"].ToString(),
                                                Cost = Convert.ToUInt32(reader["Cost"]),
                                                TypeID = Convert.ToUInt32(reader["TypeNotebookID"]),
                                                CPU = reader["CPUModel"].ToString(),
                                                Cores = Convert.ToUInt32(reader["NumberOfCores"].GetType() != typeof(DBNull) ? reader["NumberOfCores"] : 0),
                                                Frequency = Convert.ToUInt32(reader["FrequencyProcessor"].GetType() != typeof(DBNull) ? reader["FrequencyProcessor"] : 0),
                                                MaxFrequency = Convert.ToUInt32(reader["MaxFrequencyProcessor"].GetType() != typeof(DBNull) ? reader["MaxFrequencyProcessor"] : 0),
                                                VCard = reader["VideoCard"].ToString(),
                                                VideoRAM = Convert.ToUInt32(reader["VideoRAMGB"].GetType() != typeof(DBNull) ? reader["VideoRAMGB"] : 0),
                                                RAM = Convert.ToUInt32(reader["RAMGB"].GetType() != typeof(DBNull) ? reader["RAMGB"] : 0),
                                                FrequencyRAM = Convert.ToUInt32(reader["FrequencyRAM"].GetType() != typeof(DBNull) ? reader["FrequencyRAM"] : 0),
                                                SSD = Convert.ToUInt32(reader["SSDCapacityGB"].GetType() != typeof(DBNull) ? reader["SSDCapacityGB"] : 0),
                                                HDD = Convert.ToUInt32(reader["HDDCapacityGB"].GetType() != typeof(DBNull) ? reader["HDDCapacityGB"] : 0),
                                                OSID = Convert.ToUInt32(reader["OSID"].GetType() != typeof(DBNull) ? reader["OSID"] : 0),
                                                InvoiceNumber = reader["InvoiceID"].ToString(),
                                                PlaceID = Convert.ToUInt32(reader["PlaceID"].GetType() != typeof(DBNull) ? reader["PlaceID"] : 0),
                                                Diagonal = Convert.ToSingle(reader["ScreenDiagonal"].GetType() != typeof(DBNull) ? reader["ScreenDiagonal"] : 0),
                                                ResolutionID = Convert.ToUInt32(reader["ResolutionID"].GetType() != typeof(DBNull) ? reader["ResolutionID"] : 0),
                                                FrequencyID = Convert.ToUInt32(reader["FrequencyID"].GetType() != typeof(DBNull) ? reader["FrequencyID"] : 0),
                                                MatrixTechnologyID = Convert.ToUInt32(reader["MatrixTechnologyID"].GetType() != typeof(DBNull) ? reader["MatrixTechnologyID"] : 0),
                                                VideoConnectorsValue = Convert.ToInt32(reader["VideoConnectors"].GetType() != typeof(DBNull) ? reader["VideoConnectors"] : 0),
                                            };
                                            Notebook notebook = device as Notebook;
                                            notebook.VideoConnectors = GetListVideoConnectors(notebook.VideoConnectorsValue);

                                            inventoryNumber.Text = device.InventoryNumber.ToString();
                                            name.Text = device.Name;
                                            cost.Text = device.Cost.ToString();
                                            cpu.Text = notebook.CPU;
                                            cores.Text = notebook.Cores.ToString();
                                            frequency.Text = notebook.Frequency.ToString();
                                            maxFrequency.Text = notebook.MaxFrequency.ToString();
                                            vCard.Text = notebook.VCard;
                                            videoram.Text = notebook.VideoRAM.ToString();
                                            ssd.Text = notebook.SSD.ToString();
                                            hdd.Text = notebook.HDD.ToString();
                                            ram.Text = notebook.RAM.ToString();
                                            ramFrequency.Text = notebook.FrequencyRAM.ToString();
                                            diagonal.Text = notebook.Diagonal.ToString();
                                            foreach (object obj in type.ItemsSource)
                                            {
                                                DataRowView row;
                                                row = (obj as DataRowView);
                                                if (Convert.ToUInt32(row.Row[0]) == Convert.ToUInt32(notebook.TypeID))
                                                {
                                                    type.SelectedItem = row;
                                                    break;
                                                }
                                            }
                                            foreach (object obj in os.ItemsSource)
                                            {
                                                DataRowView row;
                                                row = (obj as DataRowView);
                                                if (Convert.ToUInt32(row.Row[0]) == Convert.ToUInt32(notebook.OSID))
                                                {
                                                    os.SelectedItem = row;
                                                    break;
                                                }
                                            }
                                            foreach (object obj in resolution.ItemsSource)
                                            {
                                                DataRowView row;
                                                row = (obj as DataRowView);
                                                if (Convert.ToUInt32(row.Row[0]) == Convert.ToUInt32(notebook.ResolutionID))
                                                {
                                                    resolution.SelectedItem = row;
                                                    break;
                                                }
                                            }
                                            foreach (object obj in screenFrequency.ItemsSource)
                                            {
                                                DataRowView row;
                                                row = (obj as DataRowView);
                                                if (Convert.ToUInt32(row.Row[0]) == Convert.ToUInt32(notebook.FrequencyID))
                                                {
                                                    screenFrequency.SelectedItem = row;
                                                    break;
                                                }
                                            }
                                            foreach (object obj in matrixTechnology.ItemsSource)
                                            {
                                                DataRowView row;
                                                row = (obj as DataRowView);
                                                if (Convert.ToUInt32(row.Row[0]) == Convert.ToUInt32(notebook.MatrixTechnologyID))
                                                {
                                                    matrixTechnology.SelectedItem = row;
                                                    break;
                                                }
                                            }
                                            foreach (object obj in vConnectors.Items)
                                            {
                                                ListBoxItem item = obj as ListBoxItem;
                                                foreach (string connector in notebook.VideoConnectors)
                                                {
                                                    if (item.Content.ToString() == connector)
                                                    {
                                                        item.IsSelected = true;
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    break;
                                case TypeDevice.Monitor:
                                    commandString = $"SELECT * FROM dbo.GetMonitorByID({deviceID})";
                                    reader = new SqlCommand(commandString, connection).ExecuteReader();
                                    if (reader.HasRows)
                                    {
                                        if (reader.Read())
                                        {
                                            device = new Monitor()
                                            {
                                                InventoryNumber = Convert.ToUInt32(reader["InventoryNumber"]),
                                                Name = reader["Name"].ToString(),
                                                Cost = Convert.ToUInt32(reader["Cost"]),
                                                InvoiceNumber = reader["InvoiceID"].ToString(),
                                                PlaceID = Convert.ToUInt32(reader["PlaceID"].GetType() != typeof(DBNull) ? reader["PlaceID"] : 0),
                                                Diagonal = Convert.ToSingle(reader["ScreenDiagonal"].GetType() != typeof(DBNull) ? reader["ScreenDiagonal"] : 0),
                                                ResolutionID = Convert.ToUInt32(reader["ResolutionID"].GetType() != typeof(DBNull) ? reader["ResolutionID"] : 0),
                                                FrequencyID = Convert.ToUInt32(reader["FrequencyID"].GetType() != typeof(DBNull) ? reader["FrequencyID"] : 0),
                                                MatrixTechnologyID = Convert.ToUInt32(reader["MatrixTechnologyID"].GetType() != typeof(DBNull) ? reader["MatrixTechnologyID"] : 0),
                                                VideoConnectorsValue = Convert.ToInt32(reader["VideoConnectors"].GetType() != typeof(DBNull) ? reader["VideoConnectors"] : 0),
                                            };
                                            Monitor monitor = device as Monitor;
                                            monitor.VideoConnectors = GetListVideoConnectors(monitor.VideoConnectorsValue);

                                            inventoryNumber.Text = device.InventoryNumber.ToString();
                                            name.Text = device.Name;
                                            cost.Text = device.Cost.ToString();
                                            diagonal.Text = monitor.Diagonal.ToString();
                                            foreach (object obj in resolution.ItemsSource)
                                            {
                                                DataRowView row;
                                                row = (obj as DataRowView);
                                                if (Convert.ToUInt32(row.Row[0]) == Convert.ToUInt32(monitor.ResolutionID))
                                                {
                                                    resolution.SelectedItem = row;
                                                    break;
                                                }
                                            }
                                            foreach (object obj in screenFrequency.ItemsSource)
                                            {
                                                DataRowView row;
                                                row = (obj as DataRowView);
                                                if (Convert.ToUInt32(row.Row[0]) == Convert.ToUInt32(monitor.FrequencyID))
                                                {
                                                    screenFrequency.SelectedItem = row;
                                                    break;
                                                }
                                            }
                                            foreach (object obj in matrixTechnology.ItemsSource)
                                            {
                                                DataRowView row;
                                                row = (obj as DataRowView);
                                                if (Convert.ToUInt32(row.Row[0]) == Convert.ToUInt32(monitor.MatrixTechnologyID))
                                                {
                                                    matrixTechnology.SelectedItem = row;
                                                    break;
                                                }
                                            }
                                            foreach (object obj in vConnectors.Items)
                                            {
                                                ListBoxItem item = obj as ListBoxItem;
                                                foreach (string connector in monitor.VideoConnectors)
                                                {
                                                    if (item.Content.ToString() == connector)
                                                    {
                                                        item.IsSelected = true;
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    break;
                                case TypeDevice.Projector:
                                    commandString = $"SELECT * FROM dbo.GetProjectorByID({deviceID})";
                                    reader = new SqlCommand(commandString, connection).ExecuteReader();
                                    if (reader.HasRows)
                                    {
                                        if (reader.Read())
                                        {
                                            device = new Projector()
                                            {
                                                InventoryNumber = Convert.ToUInt32(reader["InventoryNumber"]),
                                                Name = reader["Name"].ToString(),
                                                Cost = Convert.ToUInt32(reader["Cost"]),
                                                InvoiceNumber = reader["InvoiceID"].ToString(),
                                                PlaceID = Convert.ToUInt32(reader["PlaceID"].GetType() != typeof(DBNull) ? reader["PlaceID"] : 0),
                                                Diagonal = Convert.ToSingle(reader["MaxDiagonal"].GetType() != typeof(DBNull) ? reader["MaxDiagonal"] : 0),
                                                ResolutionID = Convert.ToUInt32(reader["ResolutionID"].GetType() != typeof(DBNull) ? reader["ResolutionID"] : 0),
                                                ProjectorTechnologyID = Convert.ToUInt32(reader["ProjectorTechnologyID"].GetType() != typeof(DBNull) ? reader["ProjectorTechnologyID"] : 0),
                                                VideoConnectorsValue = Convert.ToInt32(reader["VideoConnectors"].GetType() != typeof(DBNull) ? reader["VideoConnectors"] : 0),
                                            };
                                            Projector projector = device as Projector;
                                            projector.VideoConnectors = GetListVideoConnectors(projector.VideoConnectorsValue);

                                            inventoryNumber.Text = device.InventoryNumber.ToString();
                                            name.Text = device.Name;
                                            cost.Text = device.Cost.ToString();
                                            diagonal.Text = projector.Diagonal.ToString();
                                            foreach (object obj in resolution.ItemsSource)
                                            {
                                                DataRowView row;
                                                row = (obj as DataRowView);
                                                if (Convert.ToUInt32(row.Row[0]) == Convert.ToUInt32(projector.ResolutionID))
                                                {
                                                    resolution.SelectedItem = row;
                                                    break;
                                                }
                                            }
                                            foreach (object obj in projectorTechnology.ItemsSource)
                                            {
                                                DataRowView row;
                                                row = (obj as DataRowView);
                                                if (Convert.ToUInt32(row.Row[0]) == Convert.ToUInt32(projector.ProjectorTechnologyID))
                                                {
                                                    projectorTechnology.SelectedItem = row;
                                                    break;
                                                }
                                            }
                                            foreach (object obj in vConnectors.Items)
                                            {
                                                ListBoxItem item = obj as ListBoxItem;
                                                foreach (string connector in projector.VideoConnectors)
                                                {
                                                    if (item.Content.ToString() == connector)
                                                    {
                                                        item.IsSelected = true;
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    break;
                                case TypeDevice.InteractiveWhiteboard:
                                    commandString = $"SELECT * FROM dbo.GetBoardByID({deviceID})";
                                    reader = new SqlCommand(commandString, connection).ExecuteReader();
                                    if (reader.HasRows)
                                    {
                                        if (reader.Read())
                                        {
                                            device = new InteractiveWhiteboard()
                                            {
                                                InventoryNumber = Convert.ToUInt32(reader["InventoryNumber"]),
                                                Name = reader["Name"].ToString(),
                                                Cost = Convert.ToUInt32(reader["Cost"]),
                                                InvoiceNumber = reader["InvoiceID"].ToString(),
                                                PlaceID = Convert.ToUInt32(reader["PlaceID"].GetType() != typeof(DBNull) ? reader["PlaceID"] : 0),
                                                Diagonal = Convert.ToSingle(reader["MaxDiagonal"].GetType() != typeof(DBNull) ? reader["MaxDiagonal"] : 0),
                                            };
                                            InteractiveWhiteboard board = device as InteractiveWhiteboard;

                                            inventoryNumber.Text = device.InventoryNumber.ToString();
                                            name.Text = device.Name;
                                            cost.Text = device.Cost.ToString();
                                            diagonal.Text = board.Diagonal.ToString();
                                        }
                                    }
                                    break;
                                case TypeDevice.ProjectorScreen:
                                    commandString = $"SELECT * FROM dbo.GetScreenByID({deviceID})";
                                    reader = new SqlCommand(commandString, connection).ExecuteReader();
                                    if (reader.HasRows)
                                    {
                                        if (reader.Read())
                                        {
                                            device = new ProjectorScreen()
                                            {
                                                InventoryNumber = Convert.ToUInt32(reader["InventoryNumber"]),
                                                Name = reader["Name"].ToString(),
                                                Cost = Convert.ToUInt32(reader["Cost"]),
                                                InvoiceNumber = reader["InvoiceID"].ToString(),
                                                PlaceID = Convert.ToUInt32(reader["PlaceID"].GetType() != typeof(DBNull) ? reader["PlaceID"] : 0),
                                                Diagonal = Convert.ToSingle(reader["MaxDiagonal"].GetType() != typeof(DBNull) ? reader["MaxDiagonal"] : 0),
                                                AspectRatioID = Convert.ToUInt32(reader["AspectRatioID"].GetType() != typeof(DBNull) ? reader["AspectRatioID"] : 0),
                                                ScreenInstalledID = Convert.ToUInt32(reader["ScreenInstalledID"].GetType() != typeof(DBNull) ? reader["ScreenInstalledID"] : 0),
                                                IsElectronicDrive = Convert.ToBoolean(reader["IsElectronicDrive"].GetType() != typeof(DBNull) ? reader["IsElectronicDrive"] : 0),
                                            };
                                            ProjectorScreen screen = device as ProjectorScreen;

                                            inventoryNumber.Text = device.InventoryNumber.ToString();
                                            name.Text = device.Name;
                                            cost.Text = device.Cost.ToString();
                                            diagonal.Text = screen.Diagonal.ToString();
                                            isEDrive.IsChecked = screen.IsElectronicDrive;
                                            foreach (object obj in aspectRatio.ItemsSource)
                                            {
                                                DataRowView row;
                                                row = (obj as DataRowView);
                                                if (Convert.ToUInt32(row.Row[0]) == screen.AspectRatioID)
                                                {
                                                    aspectRatio.SelectedItem = row;
                                                    break;
                                                }
                                            }
                                            foreach (object obj in screenInstalled.ItemsSource)
                                            {
                                                DataRowView row;
                                                row = (obj as DataRowView);
                                                if (Convert.ToUInt32(row.Row[0]) == screen.ScreenInstalledID)
                                                {
                                                    screenInstalled.SelectedItem = row;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    break;
                                case TypeDevice.PrinterScanner:
                                    commandString = $"SELECT * FROM dbo.GetPrinterScannerByID({deviceID})";
                                    reader = new SqlCommand(commandString, connection).ExecuteReader();
                                    if (reader.HasRows)
                                    {
                                        if (reader.Read())
                                        {
                                            device = new PrinterScanner()
                                            {
                                                InventoryNumber = Convert.ToUInt32(reader["InventoryNumber"]),
                                                TypeID = Convert.ToUInt32(reader["TypePrinterID"]),
                                                Name = reader["Name"].ToString(),
                                                Cost = Convert.ToUInt32(reader["Cost"]),
                                                InvoiceNumber = reader["InvoiceID"].ToString(),
                                                PlaceID = Convert.ToUInt32(reader["PlaceID"].GetType() != typeof(DBNull) ? reader["PlaceID"] : 0),
                                                PaperSizeID = Convert.ToUInt32(reader["PaperSizeID"].GetType() != typeof(DBNull) ? reader["PaperSizeID"] : 0),
                                            };
                                            PrinterScanner printerScanner = device as PrinterScanner;

                                            inventoryNumber.Text = device.InventoryNumber.ToString();
                                            name.Text = device.Name;
                                            cost.Text = device.Cost.ToString();
                                            foreach (object obj in type.ItemsSource)
                                            {
                                                DataRowView row;
                                                row = (obj as DataRowView);
                                                if (Convert.ToUInt32(row.Row[0]) == printerScanner.TypeID)
                                                {
                                                    type.SelectedItem = row;
                                                    break;
                                                }
                                            }
                                            foreach (object obj in paperSize.ItemsSource)
                                            {
                                                DataRowView row;
                                                row = (obj as DataRowView);
                                                if (Convert.ToUInt32(row.Row[0]) == printerScanner.PaperSizeID)
                                                {
                                                    paperSize.SelectedItem = row;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    break;
                                case TypeDevice.NetworkSwitch:
                                    commandString = $"SELECT * FROM dbo.GetNetworkSwitchByID({deviceID})";
                                    reader = new SqlCommand(commandString, connection).ExecuteReader();
                                    if (reader.HasRows)
                                    {
                                        if (reader.Read())
                                        {
                                            device = new NetworkSwitch()
                                            {
                                                InventoryNumber = Convert.ToUInt32(reader["InventoryNumber"]),
                                                TypeID = Convert.ToUInt32(reader["TypeID"]),
                                                Name = reader["Name"].ToString(),
                                                Cost = Convert.ToUInt32(reader["Cost"]),
                                                InvoiceNumber = reader["InvoiceID"].ToString(),
                                                PlaceID = Convert.ToUInt32(reader["PlaceID"].GetType() != typeof(DBNull) ? reader["PlaceID"] : 0),
                                                Ports = Convert.ToUInt32(reader["NumberOfPorts"].GetType() != typeof(DBNull) ? reader["PaperSizeID"] : 0),
                                                WiFiFrequencyID = Convert.ToUInt32(reader["WiFiFrequencyID"].GetType() != typeof(DBNull) ? reader["WiFiFrequencyID"] : 0),
                                            };
                                            NetworkSwitch networkSwitch = device as NetworkSwitch;

                                            inventoryNumber.Text = device.InventoryNumber.ToString();
                                            name.Text = device.Name;
                                            cost.Text = device.Cost.ToString();
                                            ports.Text = networkSwitch.Ports.ToString();
                                            foreach (object obj in type.ItemsSource)
                                            {
                                                DataRowView row;
                                                row = (obj as DataRowView);
                                                if (Convert.ToUInt32(row.Row[0]) == networkSwitch.TypeID)
                                                {
                                                    type.SelectedItem = row;
                                                    break;
                                                }
                                            }
                                            foreach (object obj in wifiFrequency.ItemsSource)
                                            {
                                                DataRowView row;
                                                row = (obj as DataRowView);
                                                if (Convert.ToUInt32(row.Row[0]) == networkSwitch.WiFiFrequencyID)
                                                {
                                                    wifiFrequency.SelectedItem = row;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    break;
                                case TypeDevice.OtherEquipment:
                                    commandString = $"SELECT * FROM dbo.GetOtherEquipmentByID({deviceID})";
                                    reader = new SqlCommand(commandString, connection).ExecuteReader();
                                    if (reader.HasRows)
                                    {
                                        if (reader.Read())
                                        {
                                            device = new OtherEquipment()
                                            {
                                                InventoryNumber = Convert.ToUInt32(reader["InventoryNumber"]),
                                                Name = reader["Name"].ToString(),
                                                Cost = Convert.ToUInt32(reader["Cost"]),
                                                InvoiceNumber = reader["InvoiceID"].ToString(),
                                                PlaceID = Convert.ToUInt32(reader["PlaceID"].GetType() != typeof(DBNull) ? reader["PlaceID"] : 0),
                                            };

                                            inventoryNumber.Text = device.InventoryNumber.ToString();
                                            name.Text = device.Name;
                                            cost.Text = device.Cost.ToString();
                                        }
                                    }
                                    break;
                            }
                            foreach (object obj in location.ItemsSource)
                            {
                                DataRowView row;
                                row = (obj as DataRowView);
                                if (Convert.ToUInt32(row.Row[0]) == device.PlaceID)
                                {
                                    location.SelectedItem = row;
                                    break;
                                }
                            }
                            invoice.Text = device.InvoiceNumber;
                        }
                        break;
                    case TypeChange.Add:
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            switch (typeDevice)
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
                        }
                        break;
                }
            }
        }

        private void PopupClose(object sender, ExecutedRoutedEventArgs e)
        {
            changePopup.IsOpen = false;
            isPreOpenPopup = false;
            viewGrid.IsEnabled = true;
            menu.IsEnabled = true;
            UpdateData();
            UpdateImages();
            ChangeView();
        }

        private String GetVideoConnectors(Int32 value)
        {
            List<String> arr = new List<String>();
            String connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlDataReader reader = new SqlCommand("Select * from dbo.GetAllVideoConnector()" +
                    "Order by value desc", connection).ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Int32 connectorValue = Convert.ToInt32(reader["Value"]);
                        if (value >= connectorValue)
                        {
                            value -= connectorValue;
                            arr.Add(reader["Name"].ToString());
                        }
                    }
                }
            }
            String res = String.Empty;
            for (int i = 0; i < arr.Count; i++)
            {
                res += $"{arr[i]}";
                if (i < arr.Count - 1)
                    res += ", ";
            }
            return res;
        }

        private List<String> GetListVideoConnectors(Int32 value)
        {
            List<String> arr = new List<String>();
            String connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlDataReader reader = new SqlCommand("Select * from dbo.GetAllVideoConnector()" +
                    "Order by value desc", connection).ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Int32 connectorValue = Convert.ToInt32(reader["Value"]);
                        if (value >= connectorValue)
                        {
                            value -= connectorValue;
                            arr.Add(reader["Name"].ToString());
                        }
                    }
                }
            }
            return arr;
        }

        private Int32 GetValueVideoConnectors(ListBox list)
        {
            Int32 value = 0;
            foreach(var obj in list.SelectedItems)
            {
                foreach(DataRowView row in videoConnectorsDataSet.Tables[0].DefaultView)
                {
                    //uint v = (uint)((obj as DataRow)["Value"]);
                    string s = (obj as ListBoxItem).Content.ToString();
                    if (row.Row[1].ToString() == s)
                        value += Convert.ToInt32(row.Row[2]);
                }
            }
            return value;
        }

        private void InitializePopup()
        {
            if (typeChange == TypeChange.Add)
            {
                autoInvN.Visibility = Visibility.Visible;
            }
            else if (typeChange == TypeChange.Change)
            {
                autoInvN.Visibility = Visibility.Collapsed;
                autoInvN.IsChecked = false;
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
            switch (typeDevice)
            {
                case TypeDevice.InteractiveWhiteboard:
                    invoiceGrid.Visibility = Visibility.Visible;
                    locationGrid.Visibility = Visibility.Visible;
                    diagonalGrid.Visibility = Visibility.Visible;
                    GridPlacement(diagonalGrid, 0, 1, 2);
                    GridPlacement(invoiceGrid, 2, 1, 3);
                    GridPlacement(locationGrid, 5, 1, 7);
                    break;
                case TypeDevice.Monitor:
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
                    break;
                case TypeDevice.NetworkSwitch:
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
                    break;
                case TypeDevice.Notebook:
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
                    break;
                case TypeDevice.OtherEquipment:
                    invoiceGrid.Visibility = Visibility.Visible;
                    locationGrid.Visibility = Visibility.Visible;
                    GridPlacement(invoiceGrid, 0, 1, 3);
                    GridPlacement(locationGrid, 3, 1, 9);
                    break;
                case TypeDevice.PC:
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
                    break;
                case TypeDevice.PrinterScanner:
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
                    break;
                case TypeDevice.Projector:
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
                    break;
                case TypeDevice.ProjectorScreen:
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
                    break;
            }
            UpdatePopupSource();
        }

        private void GridPlacement(UIElement element, int column, int row, int colSpan, int rowSpan = 1)
        {
            Grid.SetColumn(element, column);
            Grid.SetRow(element, row);
            Grid.SetColumnSpan(element, colSpan);
            Grid.SetRowSpan(element, rowSpan);
        }

        private void UpdatePopupSource()
        {
            String connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            aspectRatioDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllAspectRatio()", connectionString);
            aspectRatioDataSet = new DataSet();
            aspectRatioDataAdapter.Fill(aspectRatioDataSet);
            aspectRatio.ItemsSource = aspectRatioDataSet.Tables[0].DefaultView;
            aspectRatio.DisplayMemberPath = "Name";

            osDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllOS()", connectionString);
            osDataSet = new DataSet();
            osDataAdapter.Fill(osDataSet);
            os.ItemsSource = osDataSet.Tables[0].DefaultView;
            os.DisplayMemberPath = "Name";

            screenInstalledDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllScreenInstalled()", connectionString);
            screenInstalledDataSet = new DataSet();
            screenInstalledDataAdapter.Fill(screenInstalledDataSet);
            screenInstalled.ItemsSource = screenInstalledDataSet.Tables[0].DefaultView;
            screenInstalled.DisplayMemberPath = "Name";

            switch (typeDevice)
            {
                case TypeDevice.InteractiveWhiteboard:
                    nameDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllBoardName()", connectionString);
                    nameDataSet = new DataSet();
                    nameDataAdapter.Fill(nameDataSet);
                    name.ItemsSource = nameDataSet.Tables[0].DefaultView;
                    name.DisplayMemberPath = "Name";

                    locationDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllLocationByTypeDeviceID(4)", connectionString);
                    break;
                case TypeDevice.Monitor:
                    nameDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllMonitorName()", connectionString);
                    nameDataSet = new DataSet();
                    nameDataAdapter.Fill(nameDataSet);
                    name.ItemsSource = nameDataSet.Tables[0].DefaultView;
                    name.DisplayMemberPath = "Name";

                    locationDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllLocationByTypeDeviceID(6)", connectionString);
                    break;
                case TypeDevice.NetworkSwitch:
                    nameDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllNetworkSwitchName()", connectionString);
                    nameDataSet = new DataSet();
                    nameDataAdapter.Fill(nameDataSet);
                    name.ItemsSource = nameDataSet.Tables[0].DefaultView;
                    name.DisplayMemberPath = "Name";

                    typeNetworkSwitchDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllTypeNetworkSwitch()", connectionString);
                    typeNetworkSwitchDataSet = new DataSet();
                    typeNetworkSwitchDataAdapter.Fill(typeNetworkSwitchDataSet);
                    type.ItemsSource = typeNetworkSwitchDataSet.Tables[0].DefaultView;
                    type.DisplayMemberPath = "Name";

                    locationDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllLocationByTypeDeviceID(5)", connectionString);
                    break;
                case TypeDevice.Notebook:
                    nameDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllNotebookName()", connectionString);
                    nameDataSet = new DataSet();
                    nameDataAdapter.Fill(nameDataSet);
                    name.ItemsSource = nameDataSet.Tables[0].DefaultView;
                    name.DisplayMemberPath = "Name";

                    typeNotebookDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllTypeNotebook()", connectionString);
                    typeNotebookDataSet = new DataSet();
                    typeNotebookDataAdapter.Fill(typeNotebookDataSet);
                    type.ItemsSource = typeNotebookDataSet.Tables[0].DefaultView;
                    type.DisplayMemberPath = "Name";

                    cpuDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllNotebookCPU()", connectionString);
                    cpuDataSet = new DataSet();
                    cpuDataAdapter.Fill(cpuDataSet);
                    cpu.ItemsSource = cpuDataSet.Tables[0].DefaultView;
                    cpu.DisplayMemberPath = "CPUModel";

                    vCardDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllNotebookvCard()", connectionString);
                    vCardDataSet = new DataSet();
                    vCardDataAdapter.Fill(vCardDataSet);
                    vCard.ItemsSource = vCardDataSet.Tables[0].DefaultView;
                    vCard.DisplayMemberPath = "VideoCard";

                    locationDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllLocationByTypeDeviceID(2)", connectionString);
                    break;
                case TypeDevice.OtherEquipment:
                    nameDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllOtherEquipmentName()", connectionString);
                    nameDataSet = new DataSet();
                    nameDataAdapter.Fill(nameDataSet);
                    name.ItemsSource = nameDataSet.Tables[0].DefaultView;
                    name.DisplayMemberPath = "Name";

                    locationDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllLocationByTypeDeviceID(9)", connectionString);
                    break;
                case TypeDevice.PC:
                    nameDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllPCName()", connectionString);
                    nameDataSet = new DataSet();
                    nameDataAdapter.Fill(nameDataSet);
                    name.ItemsSource = nameDataSet.Tables[0].DefaultView;
                    name.DisplayMemberPath = "Name";

                    cpuDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllPCCPU()", connectionString);
                    cpuDataSet = new DataSet();
                    cpuDataAdapter.Fill(cpuDataSet);
                    cpu.ItemsSource = cpuDataSet.Tables[0].DefaultView;
                    cpu.DisplayMemberPath = "CPUModel";

                    vCardDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllPCvCard()", connectionString);
                    vCardDataSet = new DataSet();
                    vCardDataAdapter.Fill(vCardDataSet);
                    vCard.ItemsSource = vCardDataSet.Tables[0].DefaultView;
                    vCard.DisplayMemberPath = "VideoCard";

                    locationDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllLocationByTypeDeviceID(1)", connectionString);
                    break;
                case TypeDevice.PrinterScanner:
                    nameDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllPrinterScannerName()", connectionString);
                    nameDataSet = new DataSet();
                    nameDataAdapter.Fill(nameDataSet);
                    name.ItemsSource = nameDataSet.Tables[0].DefaultView;
                    name.DisplayMemberPath = "Name";

                    typePrinterDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllTypePrinter()", connectionString);
                    typePrinterDataSet = new DataSet();
                    typePrinterDataAdapter.Fill(typePrinterDataSet);
                    type.ItemsSource = typePrinterDataSet.Tables[0].DefaultView;
                    type.DisplayMemberPath = "Name";

                    locationDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllLocationByTypeDeviceID(3)", connectionString);
                    break;
                case TypeDevice.Projector:
                    nameDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllProjectorName()", connectionString);
                    nameDataSet = new DataSet();
                    nameDataAdapter.Fill(nameDataSet);
                    name.ItemsSource = nameDataSet.Tables[0].DefaultView;
                    name.DisplayMemberPath = "Name";

                    locationDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllLocationByTypeDeviceID(7)", connectionString);
                    break;
                case TypeDevice.ProjectorScreen:
                    nameDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllProjectorScreenName()", connectionString);
                    nameDataSet = new DataSet();
                    nameDataAdapter.Fill(nameDataSet);
                    name.ItemsSource = nameDataSet.Tables[0].DefaultView;
                    name.DisplayMemberPath = "Name";

                    locationDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllLocationByTypeDeviceID(8)", connectionString);
                    break;
            }

            locationDataSet = new DataSet();
            locationDataAdapter.Fill(locationDataSet);
            location.ItemsSource = locationDataSet.Tables[0].DefaultView;
            location.DisplayMemberPath = "Place";

            frequencyDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllFrequency()", connectionString);
            frequencyDataSet = new DataSet();
            frequencyDataAdapter.Fill(frequencyDataSet);
            screenFrequency.ItemsSource = frequencyDataSet.Tables[0].DefaultView;
            screenFrequency.DisplayMemberPath = "Name";

            matrixTechnologyDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllMatrixTechnology()", connectionString);
            matrixTechnologyDataSet = new DataSet();
            matrixTechnologyDataAdapter.Fill(matrixTechnologyDataSet);
            matrixTechnology.ItemsSource = matrixTechnologyDataSet.Tables[0].DefaultView;
            matrixTechnology.DisplayMemberPath = "Name";

            paperSizeDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllPaperSize()", connectionString);
            paperSizeDataSet = new DataSet();
            paperSizeDataAdapter.Fill(paperSizeDataSet);
            paperSize.ItemsSource = paperSizeDataSet.Tables[0].DefaultView;
            paperSize.DisplayMemberPath = "Name";

            projectorTechnologyDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllProjectorTechnology()", connectionString);
            projectorTechnologyDataSet = new DataSet();
            projectorTechnologyDataAdapter.Fill(projectorTechnologyDataSet);
            projectorTechnology.ItemsSource = projectorTechnologyDataSet.Tables[0].DefaultView;
            projectorTechnology.DisplayMemberPath = "Name";

            resolutionDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllResolution()", connectionString);
            resolutionDataSet = new DataSet();
            resolutionDataAdapter.Fill(resolutionDataSet);
            resolution.ItemsSource = resolutionDataSet.Tables[0].DefaultView;
            resolution.DisplayMemberPath = "Name";

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

            wifiFrequencyDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllWiFiFrequency()", connectionString);
            wifiFrequencyDataSet = new DataSet();
            wifiFrequencyDataAdapter.Fill(wifiFrequencyDataSet);
            wifiFrequency.ItemsSource = wifiFrequencyDataSet.Tables[0].DefaultView;
            wifiFrequency.DisplayMemberPath = "Name";

            motherboardDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllMotherboard()", connectionString);
            motherboardDataSet = new DataSet();
            motherboardDataAdapter.Fill(motherboardDataSet);
            motherboard.ItemsSource = motherboardDataSet.Tables[0].DefaultView;
            motherboard.DisplayMemberPath = "Name";
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

        private void ChangePopupPreClose()
        {
            if (changePopup.IsOpen)
            {
                isPreOpenPopup = true;
                changePopup.IsOpen = false;
                //changePopup.Visibility = Visibility.Collapsed;
            }
        }

        private void ChangePopupPostClose()
        {
            changePopup.Height = Height - 200;
            changePopup.Width = Width - 400;
            if (isPreOpenPopup)
            {
                changePopup.IsOpen = true;
                isPreOpenPopup = false;
                //changePopup.Visibility = Visibility.Visible;
            }
        }

        private void autoInvN_Checked(object sender, RoutedEventArgs e)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                inventoryNumber.Text = new SqlCommand("SELECT dbo.GetNextInventoryNumber()", connection).ExecuteScalar().ToString();
            }
        }

        private void cpu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRow row = ((DataRowView)cpu.SelectedItem)?.Row;
            frequency.Text = row?[1].ToString();
            maxFrequency.Text = row?[2].ToString();
            cores.Text = row?[3].ToString();
        }

        private void SelectViewEquipment(object sender, ExecutedRoutedEventArgs e)
        {
            nowView = View.Equipment;
            equipmentGrid.Visibility = Visibility.Visible;
            softwareGrid.Visibility = Visibility.Collapsed;
            locationManagementGrid.Visibility = Visibility.Collapsed;
        }

        private void SelectViewSoftware(object sender, ExecutedRoutedEventArgs e)
        {
            nowView = View.Software;
            equipmentGrid.Visibility = Visibility.Collapsed;
            softwareGrid.Visibility = Visibility.Visible;
            locationManagementGrid.Visibility = Visibility.Collapsed;
        }

        private void SelectViewLocation(object sender, ExecutedRoutedEventArgs e)
        {
            nowView = View.Location;
            equipmentGrid.Visibility = Visibility.Collapsed;
            softwareGrid.Visibility = Visibility.Collapsed;
            locationManagementGrid.Visibility = Visibility.Visible;
        }

        private void AddSoftware(object sender, RoutedEventArgs e)
        {

        }

        private void ChangeSoftware(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteSoftware(object sender, RoutedEventArgs e)
        {

        }

        private void EquipmentView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BitmapFrame frame = null;
            //deviceImage.Source = BitmapFrame.Create();
            int col;
            switch (typeDevice)
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
        }

        /*private int saveImage(string filename)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "AddImage";
                command.Parameters.Add("@Image", SqlDbType.VarBinary);
                command.Parameters.Add("@ID", SqlDbType.Int).Direction = ParameterDirection.Output;

                byte[] imageData;
                using (FileStream fs = new FileStream(filename, FileMode.Open))
                {
                    imageData = new byte[fs.Length];
                    fs.Read(imageData, 0, imageData.Length);
                }
                command.Parameters["@Image"].Value = imageData;

                command.ExecuteNonQuery();
                return Convert.ToInt32(command.Parameters["@ID"].Value);
            }
        }*/

        private Dictionary<int, byte[]> GetImages()
        {
            Dictionary<int, byte[]> temp = new Dictionary<int, byte[]>();

            String connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlDataReader reader = new SqlCommand("SELECT * FROM dbo.GetAllImages()", connection).ExecuteReader();

                while (reader.Read())
                {
                    temp.Add(reader.GetInt32(0), (byte[])reader.GetValue(1));
                }
            }

            return temp;
        }

        public void UpdateImages()
        {
            images = GetImages();
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

        private byte[] LoadImage(string path)
        {
            if (path != "")
            {
                byte[] data;
                try
                {
                    using (FileStream fs = new FileStream(path, FileMode.Open))
                    {
                        data = new byte[fs.Length];
                        fs.Read(data, 0, data.Length);
                    }
                    return data;
                }
                catch
                {
                    return null;
                }
            }
            else
            {
                int col;
                switch (typeDevice)
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
                object obj = ((DataRowView)equipmentView.SelectedItems?[0]).Row[col];
                int id = Convert.ToInt32(obj.GetType() == typeof(DBNull) ? null : obj);
                if (id != 0)
                {
                    return images[id];
                }
                return null;
            }
        }

        private void UpdateView_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            UpdateAllData();
            ChangeView();
            UpdateImages();
        }

        private void inventoryNumber_MouseEnter(object sender, MouseEventArgs e)
        {
            //Task task;
            //if (inventoryNumberToolTip != null)
            //{
            //    inventoryNumberToolTip.IsOpen = true;
            //    task = new Task(() =>
            //    {
            //        try
            //        {
            //            for (int i = 0; i < 3; i++)
            //            {
            //                i++;
            //                Thread.Sleep(1000);
            //            }
            //            Dispatcher.Invoke(() => {
            //                try
            //                {
            //                    if (inventoryNumberToolTip != null)
            //                        inventoryNumberToolTip.IsOpen = false;
            //                }
            //                catch { }
            //            });

            //        }
            //        catch { }
            //    });
            //    task.Start();
            //}
        }
    }
}
