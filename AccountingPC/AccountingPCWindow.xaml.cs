using AccountingPC.Properties;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using AccountingPC.LightTheme;
using System.Windows.Controls.Primitives;

namespace AccountingPC
{
    enum TypeChange
    {
        Add,
        Change,
    }

    /// <summary>
    /// Логика взаимодействия для AccountingPCWindow.xaml
    /// </summary>
    public partial class AccountingPCWindow : Window
    {
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
        SqlDataAdapter adapter;
        DataSet set;
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

        SqlDataAdapter DataAdapter;

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

        DataSet DataSet;

        public AccountingPCWindow()
        {
            InitializeComponent();
            lastHeight = Height;
            lastWidth = Width;
            UpdateAllData();
            list.SelectedIndex = 0;
            isPreOpenPopup = false;
        }

        private void window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            changePopup.Height = Height - 200;
            changePopup.Width = Width - 400;
            if (changePopup.IsOpen)
            {
                isPreOpenPopup = true;
                changePopup.IsOpen = false;
                changePopup.IsOpen = true;
                isPreOpenPopup = false;
            }
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
            changePopupPreClose();
            DragMove();// Для перемещение ока
            changePopup.Height = Height - 200;
            changePopup.Width = Width - 400;
            changePopupPostClose();
            if (WindowState == WindowState.Maximized)
            {
                ((Path)buttonMaximized.Template.FindName("Maximize", buttonMaximized)).Visibility = Visibility.Collapsed;
                ((Path)buttonMaximized.Template.FindName("Restore", buttonMaximized)).Visibility = Visibility.Visible;
            }
            else if (WindowState == WindowState.Normal)
            {
                ((Path)buttonMaximized.Template.FindName("Maximize", buttonMaximized)).Visibility = Visibility.Visible;
                ((Path)buttonMaximized.Template.FindName("Restore", buttonMaximized)).Visibility = Visibility.Collapsed;
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
                ((Path)buttonMaximized.Template.FindName("Maximize", buttonMaximized)).Visibility = Visibility.Collapsed;
                ((Path)buttonMaximized.Template.FindName("Restore", buttonMaximized)).Visibility = Visibility.Visible;
            }
            else if (WindowState == WindowState.Normal)
            {
                ((Path)buttonMaximized.Template.FindName("Maximize", buttonMaximized)).Visibility = Visibility.Visible;
                ((Path)buttonMaximized.Template.FindName("Restore", buttonMaximized)).Visibility = Visibility.Collapsed;
            }
            changePopup.Height = Height - 200;
            changePopup.Width = Width - 400;
            UpdateData();
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
            ChangeView();
        }

        private void AddDevice(object sender, RoutedEventArgs e)
        {
            typeChange = TypeChange.Add;
            changePopup.IsOpen = true;
        }

        private void ChangeDevice(object sender, RoutedEventArgs e)
        {
            DataRow row = ((DataRowView)view.SelectedItem).Row;
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
                foreach (object obj in view.SelectedItems)
                {
                    DataRow row = ((DataRowView)obj).Row;
                    Int32 id = Convert.ToInt32(row[0]);
                    SqlCommand command = new SqlCommand($"Delete{typeDevice.ToString()}ByID", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@ID", id));
                    Int32 res = command.ExecuteNonQuery();
                }
            }
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
                        row[18] = row[15].GetType() == typeof(int) ? GetVideoConnectors(Convert.ToInt32(row[15])) : row[15];
                    }
                    break;
                case TypeDevice.Notebook:
                    notebookDataAdapter = new SqlDataAdapter("SELECT * FROM dbo.GetAllNotebook()", connectionString);
                    notebookDataSet = new DataSet();
                    notebookDataAdapter.Fill(notebookDataSet);
                    notebookDataSet.Tables[0].Columns.Add("Видеоразъемы");
                    foreach (DataRow row in notebookDataSet.Tables[0].Rows)
                    {
                        row[22] = row[18].GetType() == typeof(int) ? GetVideoConnectors(Convert.ToInt32(row[18])) : row[18];
                    }
                    break;
                case TypeDevice.Monitor:
                    monitorDataAdapter = new SqlDataAdapter("SELECT * FROM dbo.GetAllMonitor()", connectionString);
                    monitorDataSet = new DataSet();
                    monitorDataAdapter.Fill(monitorDataSet);
                    monitorDataSet.Tables[0].Columns.Add("Видеоразъемы");
                    foreach (DataRow row in monitorDataSet.Tables[0].Rows)
                    {
                        row[11] = row[9].GetType() == typeof(int) ? GetVideoConnectors(Convert.ToInt32(row[9])) : row[9];
                    }
                    break;
                case TypeDevice.Projector:
                    projectorDataAdapter = new SqlDataAdapter("SELECT * FROM dbo.GetAllProjector()", connectionString);
                    projectorDataSet = new DataSet();
                    projectorDataAdapter.Fill(projectorDataSet);
                    projectorDataSet.Tables[0].Columns.Add("Видеоразъемы");
                    foreach (DataRow row in projectorDataSet.Tables[0].Rows)
                    {
                        row[10] = row[8].GetType() == typeof(int) ? GetVideoConnectors(Convert.ToInt32(row[8])) : row[8];
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
                row[18] = row[15].GetType() == typeof(int) ? GetVideoConnectors(Convert.ToInt32(row[15])) : row[15];
            }

            notebookDataAdapter = new SqlDataAdapter("SELECT * FROM dbo.GetAllNotebook()", connectionString);
            notebookDataSet = new DataSet();
            notebookDataAdapter.Fill(notebookDataSet);
            notebookDataSet.Tables[0].Columns.Add("Видеоразъемы");
            foreach (DataRow row in notebookDataSet.Tables[0].Rows)
            {
                row[22] = row[18].GetType() == typeof(int) ? GetVideoConnectors(Convert.ToInt32(row[18])) : row[18];
            }

            monitorDataAdapter = new SqlDataAdapter("SELECT * FROM dbo.GetAllMonitor()", connectionString);
            monitorDataSet = new DataSet();
            monitorDataAdapter.Fill(monitorDataSet);
            monitorDataSet.Tables[0].Columns.Add("Видеоразъемы");
            foreach (DataRow row in monitorDataSet.Tables[0].Rows)
            {
                row[11] = row[9].GetType() == typeof(int) ? GetVideoConnectors(Convert.ToInt32(row[9])) : row[9];
            }

            projectorDataAdapter = new SqlDataAdapter("SELECT * FROM dbo.GetAllProjector()", connectionString);
            projectorDataSet = new DataSet();
            projectorDataAdapter.Fill(projectorDataSet);
            projectorDataSet.Tables[0].Columns.Add("Видеоразъемы");
            foreach (DataRow row in projectorDataSet.Tables[0].Rows)
            {
                row[10] = row[8].GetType() == typeof(int) ? GetVideoConnectors(Convert.ToInt32(row[8])) : row[8];
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
            switch (list.SelectedIndex)
            {
                case 0:
                    view.ItemsSource = pcDataSet.Tables[0].DefaultView;
                    if (view.Columns.Count > 0)
                    {
                        view.Columns[0].Visibility = Visibility.Collapsed;
                        view.Columns[15].Visibility = Visibility.Collapsed;
                    }
                    typeDevice = TypeDevice.PC;
                    break;
                case 1:
                    view.ItemsSource = notebookDataSet.Tables[0].DefaultView;
                    view.Columns[0].Visibility = Visibility.Collapsed;
                    view.Columns[18].Visibility = Visibility.Collapsed;
                    typeDevice = TypeDevice.Notebook;
                    break;
                case 2:
                    view.ItemsSource = monitorDataSet.Tables[0].DefaultView;
                    view.Columns[0].Visibility = Visibility.Collapsed;
                    view.Columns[9].Visibility = Visibility.Collapsed;
                    typeDevice = TypeDevice.Monitor;
                    break;
                case 3:
                    view.ItemsSource = projectorDataSet.Tables[0].DefaultView;
                    view.Columns[0].Visibility = Visibility.Collapsed;
                    view.Columns[8].Visibility = Visibility.Collapsed;
                    typeDevice = TypeDevice.Projector;
                    break;
                case 4:
                    view.ItemsSource = boardDataSet.Tables[0].DefaultView;
                    view.Columns[0].Visibility = Visibility.Collapsed;
                    typeDevice = TypeDevice.InteractiveWhiteboard;
                    break;
                case 5:
                    view.ItemsSource = projectorScreenDataSet.Tables[0].DefaultView;
                    view.Columns[0].Visibility = Visibility.Collapsed;
                    typeDevice = TypeDevice.ProjectorScreen;
                    break;
                case 6:
                    view.ItemsSource = printerScannerDataSet.Tables[0].DefaultView;
                    view.Columns[0].Visibility = Visibility.Collapsed;
                    typeDevice = TypeDevice.PrinterScanner;
                    break;
                case 7:
                    view.ItemsSource = networkSwitchDataSet.Tables[0].DefaultView;
                    view.Columns[0].Visibility = Visibility.Collapsed;
                    typeDevice = TypeDevice.NetworkSwitch;
                    break;
                case 8:
                    view.ItemsSource = otherEquipmentDataSet.Tables[0].DefaultView;
                    view.Columns[0].Visibility = Visibility.Collapsed;
                    typeDevice = TypeDevice.OtherEquipment;
                    break;
            }
        }

        /* Управление изменением устройств */

        public CustomPopupPlacement[] changePopupPlacement(Size popupSize, Size targetSize, Point offset)
        {
            CustomPopupPlacement placement1 =
               new CustomPopupPlacement(new Point(0, 0), PopupPrimaryAxis.Vertical);

            CustomPopupPlacement[] ttplaces =
                    new CustomPopupPlacement[] { placement1 };
            return ttplaces;
        }

        private void SaveChanges(object sender, RoutedEventArgs e)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                String commandString;
                SqlCommand command;
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
                                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToUInt32(cost.Text)));
                                command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice));
                                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRow)location.SelectedItem)[0]));
                                command.Parameters.Add(new SqlParameter("@CPU", cpu.Text));
                                command.Parameters.Add(new SqlParameter("@RAM", Convert.ToUInt32(ram.Text)));
                                command.Parameters.Add(new SqlParameter("@HDD", Convert.ToUInt32(hdd.Text)));
                                command.Parameters.Add(new SqlParameter("@Video", vCard.Text));
                                command.Parameters.Add(new SqlParameter("@OSName", os.Text));
                                command.Parameters.Add(new SqlParameter("@MB", motherboard.Text));
                                command.ExecuteNonQuery();
                                break;
                            case TypeDevice.Notebook:
                                break;
                            case TypeDevice.Monitor:
                                break;
                            case TypeDevice.NetworkSwitch:
                                break;
                            case TypeDevice.InteractiveWhiteboard:
                                break;
                            case TypeDevice.PrinterScanner:
                                break;
                            case TypeDevice.Projector:
                                break;
                            case TypeDevice.ProjectorScreen:
                                break;
                            case TypeDevice.OtherEquipment:
                                break;
                        }
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
                                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToUInt32(cost.Text)));
                                command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text));
                                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRow)location.SelectedItem)[0]));
                                command.Parameters.Add(new SqlParameter("@CPU", cpu.Text));
                                command.Parameters.Add(new SqlParameter("@RAM", Convert.ToUInt32(ram.Text)));
                                command.Parameters.Add(new SqlParameter("@HDD", Convert.ToUInt32(hdd.Text)));
                                command.Parameters.Add(new SqlParameter("@Video", vCard.Text));
                                command.Parameters.Add(new SqlParameter("@OSName", os.Text));
                                command.Parameters.Add(new SqlParameter("@MB", motherboard.Text));
                                command.ExecuteNonQuery();
                                break;
                            case TypeDevice.Notebook:
                                break;
                            case TypeDevice.Monitor:
                                break;
                            case TypeDevice.NetworkSwitch:
                                break;
                            case TypeDevice.InteractiveWhiteboard:
                                break;
                            case TypeDevice.PrinterScanner:
                                break;
                            case TypeDevice.Projector:
                                break;
                            case TypeDevice.ProjectorScreen:
                                break;
                            case TypeDevice.OtherEquipment:
                                break;
                        }
                        break;
                }
                //int res = command.ExecuteNonQuery();
            }
        }

        private void changePopup_Opened(object sender, EventArgs e)
        {
            InitializePopup();
            viewGrid.IsEnabled = false;
            menu.IsEnabled = false;
            String commandString;
            SqlDataReader reader;
            String connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            if (!isPreOpenPopup)
            {
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
                                            hdd.Text = notebook.HDD.ToString();
                                            ram.Text = notebook.RAM.ToString();
                                            ramFrequency.Text = notebook.FrequencyRAM.ToString();
                                            diagonal.Text = notebook.Diagonal.ToString();
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

        private UInt32 GetValueVideoConnectors()
        {
            UInt32 value = 0;
            foreach(var obj in vConnectors.SelectedItems)
            {
                value += (uint)((obj as DataRow)["Value"]);
            }
            return value;
        }

        private void InitializePopup()
        {
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
                    GridPlacement(frequencyGrid, 6, 1, 2);
                    GridPlacement(maxFrequencyGrid, 8, 1, 2);
                    GridPlacement(hddGrid, 10, 1, 2);
                    GridPlacement(diagonalGrid, 0, 2, 2);
                    GridPlacement(resolutionGrid, 2, 2, 3);
                    GridPlacement(screenFrequencyGrid, 5, 2, 2);
                    GridPlacement(matrixTechnologyGrid, 7, 2, 3);
                    GridPlacement(vCardGrid, 0, 3, 4);
                    GridPlacement(videoramGrid, 4, 3, 2);
                    GridPlacement(ramGrid, 6, 3, 2);
                    GridPlacement(ramFrequencyGrid, 8, 3, 2);
                    GridPlacement(osGrid, 0, 4, 6);
                    GridPlacement(invoiceGrid, 6, 4, 4);
                    GridPlacement(locationGrid, 0, 5, 10);
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
                    GridPlacement(hddGrid, 0, 3, 2);
                    GridPlacement(osGrid, 2, 3, 4);
                    GridPlacement(invoiceGrid, 6, 3, 4);
                    GridPlacement(locationGrid, 0, 4, 10);
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

        private void window_LostFocus(object sender, RoutedEventArgs e)
        {
            changePopupPreClose();
        }

        private void window_GotFocus(object sender, RoutedEventArgs e)
        {
            changePopupPostClose();
        }

        private void window_Deactivated(object sender, EventArgs e)
        {
            changePopupPreClose();
        }

        private void window_Activated(object sender, EventArgs e)
        {
            changePopupPostClose();
        }

        private void changePopupPreClose()
        {
            if (changePopup.IsOpen)
            {
                isPreOpenPopup = true;
                changePopup.IsOpen = false;
            }
        }

        private void changePopupPostClose()
        {
            changePopup.Height = Height - 200;
            changePopup.Width = Width - 400;
            if (isPreOpenPopup)
            {
                changePopup.IsOpen = true;
                isPreOpenPopup = false;
            }
        }
    }
}
