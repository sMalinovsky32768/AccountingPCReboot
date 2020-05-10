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
            list.SelectedIndex = 0;
            UpdateDataGrid();
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
            if (changePopup.IsOpen)
            {
                isPreOpenPopup = true;
                changePopup.IsOpen = false;
            }
            DragMove();// Для перемещение ока
            changePopup.Height = Height - 200;
            changePopup.Width = Width - 400;
            if (isPreOpenPopup)
            {
                changePopup.IsOpen = true;
                isPreOpenPopup = false;
            }
            if (WindowState == WindowState.Maximized)
            {
                
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
            try
            {
                UpdateDataGrid();
            }
            catch { }
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
            UpdateDataGrid();
        }

        private void UpdateDataGrid()
        {
            String connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            switch (list.SelectedIndex)
            {
                case 0:
                    adapter = new SqlDataAdapter("SELECT * FROM dbo.GetAllPC()", connectionString);
                    set = new DataSet();
                    adapter.Fill(set);
                    set.Tables[0].Columns.Add("Видеоразъемы");
                    foreach (DataRow row in set.Tables[0].Rows)
                    {
                        //row[15] = row[15].GetType()==typeof(DBNull)?row[15]:GetListVideoConnectors(Convert.ToInt32(row[15]));
                        row[18] = row[15].GetType()== typeof(int)?GetListVideoConnectors(Convert.ToInt32(row[15])):row[15];
                    }
                    view.ItemsSource = set.Tables[0].DefaultView;
                    if (view.Columns.Count > 0)
                    {
                        view.Columns[0].Visibility = Visibility.Collapsed;
                        view.Columns[15].Visibility = Visibility.Collapsed;
                    }
                    typeDevice = TypeDevice.PC;
                    break;
                case 1:
                    adapter = new SqlDataAdapter("SELECT * FROM dbo.GetAllNotebook()", connectionString);
                    set = new DataSet();
                    adapter.Fill(set);
                    set.Tables[0].Columns.Add("Видеоразъемы");
                    foreach (DataRow row in set.Tables[0].Rows)
                    {
                        row[22] = row[18].GetType() == typeof(int) ? GetListVideoConnectors(Convert.ToInt32(row[18])) : row[18];
                    }
                    view.ItemsSource = set.Tables[0].DefaultView;
                    view.Columns[0].Visibility = Visibility.Collapsed;
                    view.Columns[18].Visibility = Visibility.Collapsed;
                    typeDevice = TypeDevice.Notebook;
                    break;
                case 2:
                    adapter = new SqlDataAdapter("SELECT * FROM dbo.GetAllMonitor()", connectionString);
                    set = new DataSet();
                    adapter.Fill(set);
                    set.Tables[0].Columns.Add("Видеоразъемы");
                    foreach (DataRow row in set.Tables[0].Rows)
                    {
                        row[11] = row[9].GetType() == typeof(int) ? GetListVideoConnectors(Convert.ToInt32(row[9])) : row[9];
                    }
                    view.ItemsSource = set.Tables[0].DefaultView;
                    view.Columns[0].Visibility = Visibility.Collapsed;
                    view.Columns[9].Visibility = Visibility.Collapsed;
                    typeDevice = TypeDevice.Monitor;
                    break;
                case 3:
                    adapter = new SqlDataAdapter("SELECT * FROM dbo.GetAllProjector()", connectionString);
                    set = new DataSet();
                    adapter.Fill(set);
                    set.Tables[0].Columns.Add("Видеоразъемы");
                    foreach (DataRow row in set.Tables[0].Rows)
                    {
                        row[10] = row[8].GetType() == typeof(int) ? GetListVideoConnectors(Convert.ToInt32(row[8])) : row[8];
                    }
                    view.ItemsSource = set.Tables[0].DefaultView;
                    view.Columns[0].Visibility = Visibility.Collapsed;
                    view.Columns[8].Visibility = Visibility.Collapsed;
                    typeDevice = TypeDevice.Projector;
                    break;
                case 4:
                    adapter = new SqlDataAdapter("SELECT * FROM dbo.GetAllBoard()", connectionString);
                    set = new DataSet();
                    adapter.Fill(set);
                    view.ItemsSource = set.Tables[0].DefaultView;
                    view.Columns[0].Visibility = Visibility.Collapsed;
                    typeDevice = TypeDevice.InteractiveWhiteboard;
                    break;
                case 5:
                    adapter = new SqlDataAdapter("SELECT * FROM dbo.GetAllScreen()", connectionString);
                    set = new DataSet();
                    adapter.Fill(set);
                    view.ItemsSource = set.Tables[0].DefaultView;
                    view.Columns[0].Visibility = Visibility.Collapsed;
                    typeDevice = TypeDevice.ProjectorScreen;
                    break;
                case 6:
                    adapter = new SqlDataAdapter("SELECT * FROM dbo.GetAllPrinterScanner()", connectionString);
                    set = new DataSet();
                    adapter.Fill(set);
                    view.ItemsSource = set.Tables[0].DefaultView;
                    view.Columns[0].Visibility = Visibility.Collapsed;
                    typeDevice = TypeDevice.PrinterScanner;
                    break;
                case 7:
                    adapter = new SqlDataAdapter("SELECT * FROM dbo.GetAllNetworkSwitch()", connectionString);
                    set = new DataSet();
                    adapter.Fill(set);
                    view.ItemsSource = set.Tables[0].DefaultView;
                    view.Columns[0].Visibility = Visibility.Collapsed;
                    typeDevice = TypeDevice.NetworkSwitch;
                    break;
                case 8:
                    adapter = new SqlDataAdapter("SELECT * FROM dbo.GetAllOtherEquipment()", connectionString);
                    set = new DataSet();
                    adapter.Fill(set);
                    view.ItemsSource = set.Tables[0].DefaultView;
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
                                            pc = new PC()
                                            {
                                                InventoryNumber = Convert.ToUInt32(reader["Инвентарный номер"]),
                                                Name = reader["Наименование"].ToString(),
                                                Cost = Convert.ToUInt32(reader["Цена"]),
                                                Motherboard = reader["Материнская плата"].ToString(),
                                                CPU = reader["Процессор"].ToString(),
                                                VCard = reader["Видеокарта"].ToString(),
                                                RAM = Convert.ToUInt32(reader["ОЗУ"].GetType() != typeof(DBNull) ? reader["ОЗУ"] : 0),
                                                HDD = Convert.ToUInt32(reader["Объем HDD"]),
                                                OS = reader["Операционная система"].ToString(),
                                                Invoice = reader["Номер накладной"].ToString(),
                                                Location = { 
                                                    ID = Convert.ToUInt32(reader["PlaceID"]),
                                                    Name = reader["Расположение"].ToString() 
                                                }
                                            };
                                            inventoryNumber.Text = pc.InventoryNumber.ToString();
                                            name.Text = pc.Name;
                                            cost.Text = pc.Cost.ToString();
                                            motherboard.Text = pc.Motherboard;
                                            cpu.Text = pc.CPU;
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

                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case TypeChange.Add:
                        //PC pcAdd = new PC();
                        //pcGrid.DataContext = pcAdd;
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            SqlDataAdapter dataAdapter;
                            DataSet dataSet;
                            connection.Open();
                            switch (typeDevice)
                            {
                                case TypeDevice.PC:
                                    dataAdapter = new SqlDataAdapter("SELECT * FROM GetAllLocationByTypeDeviceID(1)", connectionString);
                                    dataSet = new DataSet();
                                    dataAdapter.Fill(dataSet);
                                    location.ItemsSource = dataSet.Tables[0].DefaultView;
                                    location.DisplayMemberPath = "Place";
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
            UpdateDataGrid();
        }

        private String GetListVideoConnectors(Int32 value)
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

        private List<String> GetListVideoConnectors(Int32 value, object obj)
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
                    break;
                case TypeDevice.Monitor:
                    nameDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllMonitorName()", connectionString);
                    nameDataSet = new DataSet();
                    nameDataAdapter.Fill(nameDataSet);
                    name.ItemsSource = nameDataSet.Tables[0].DefaultView;
                    name.DisplayMemberPath = "Name";
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
                    break;
                case TypeDevice.OtherEquipment:
                    nameDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllOtherEquipmentName()", connectionString);
                    nameDataSet = new DataSet();
                    nameDataAdapter.Fill(nameDataSet);
                    name.ItemsSource = nameDataSet.Tables[0].DefaultView;
                    name.DisplayMemberPath = "Name";
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
                    break;
                case TypeDevice.Projector:
                    nameDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllProjectorName()", connectionString);
                    nameDataSet = new DataSet();
                    nameDataAdapter.Fill(nameDataSet);
                    name.ItemsSource = nameDataSet.Tables[0].DefaultView;
                    name.DisplayMemberPath = "Name";
                    break;
                case TypeDevice.ProjectorScreen:
                    nameDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllProjectorScreenName()", connectionString);
                    nameDataSet = new DataSet();
                    nameDataAdapter.Fill(nameDataSet);
                    name.ItemsSource = nameDataSet.Tables[0].DefaultView;
                    name.DisplayMemberPath = "Name";
                    break;
            }

            frequencyDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllFrequency()", connectionString);
            frequencyDataSet = new DataSet();
            frequencyDataAdapter.Fill(frequencyDataSet);
            screenFrequency.ItemsSource = frequencyDataSet.Tables[0].DefaultView;
            screenFrequency.DisplayMemberPath = "Name";

            locationDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllLocation()", connectionString);
            locationDataSet = new DataSet();
            locationDataAdapter.Fill(locationDataSet);
            location.ItemsSource = locationDataSet.Tables[0].DefaultView;
            location.DisplayMemberPath = "Name";

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

            videoConnectorsDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllVideoConnector()", connectionString);
            videoConnectorsDataSet = new DataSet();
            videoConnectorsDataAdapter.Fill(videoConnectorsDataSet);
            vConnectors.ItemsSource = videoConnectorsDataSet.Tables[0].DefaultView;
            vConnectors.DisplayMemberPath = "Name";

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
    }
}
