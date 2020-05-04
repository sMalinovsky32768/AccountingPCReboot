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
        SqlDataAdapter DataAdapter;

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
                    view.ItemsSource = set.Tables[0].DefaultView;
                    view.Columns[0].Visibility = Visibility.Collapsed;
                    typeDevice = TypeDevice.Notebook;
                    break;
                case 2:
                    adapter = new SqlDataAdapter("SELECT * FROM dbo.GetAllMonitor()", connectionString);
                    set = new DataSet();
                    adapter.Fill(set);
                    view.ItemsSource = set.Tables[0].DefaultView;
                    view.Columns[0].Visibility = Visibility.Collapsed;
                    typeDevice = TypeDevice.Monitor;
                    break;
                case 3:
                    adapter = new SqlDataAdapter("SELECT * FROM dbo.GetAllProjector()", connectionString);
                    set = new DataSet();
                    adapter.Fill(set);
                    view.ItemsSource = set.Tables[0].DefaultView;
                    view.Columns[0].Visibility = Visibility.Collapsed;
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
                                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt32(inventoryNumberPC.Text)));
                                command.Parameters.Add(new SqlParameter("@Name", namePC.Text));
                                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToUInt32(costPC.Text)));
                                command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoicePC));
                                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRow)locationPC.SelectedItem)[0]));
                                command.Parameters.Add(new SqlParameter("@CPU", cpuPC.Text));
                                command.Parameters.Add(new SqlParameter("@RAM", Convert.ToUInt32(ramPC.Text)));
                                command.Parameters.Add(new SqlParameter("@HDD", Convert.ToUInt32(hddPC.Text)));
                                command.Parameters.Add(new SqlParameter("@Video", vCardPC.Text));
                                command.Parameters.Add(new SqlParameter("@OSName", osPC.Text));
                                command.Parameters.Add(new SqlParameter("@MB", motherboardPC.Text));
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
                                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt32(inventoryNumberPC.Text)));
                                command.Parameters.Add(new SqlParameter("@Name", namePC.Text));
                                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToUInt32(costPC.Text)));
                                command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoicePC.Text));
                                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRow)locationPC.SelectedItem)[0]));
                                command.Parameters.Add(new SqlParameter("@CPU", cpuPC.Text));
                                command.Parameters.Add(new SqlParameter("@RAM", Convert.ToUInt32(ramPC.Text)));
                                command.Parameters.Add(new SqlParameter("@HDD", Convert.ToUInt32(hddPC.Text)));
                                command.Parameters.Add(new SqlParameter("@Video", vCardPC.Text));
                                command.Parameters.Add(new SqlParameter("@OSName", osPC.Text));
                                command.Parameters.Add(new SqlParameter("@MB", motherboardPC.Text));
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

        private void ComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            GetComboBoxSourcePC(sender);
        }

        private void GetComboBoxSourcePC(object sender)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            if (((ComboBox)sender).Name == "cpuPC")
            {

            }
            switch (((ComboBox)sender).Name)
            {
                case "namePC":
                    adapter = new SqlDataAdapter($"SELECT distinct Name from PC Where Name LIKE N'{namePC.Text}%'", connectionString);
                    set = new DataSet();
                    adapter.Fill(set);
                    namePC.ItemsSource = set.Tables[0].DefaultView;
                    namePC.DisplayMemberPath = "Name";
                    namePC.IsDropDownOpen = true;
                    break;
                case "motherboardPC":
                    adapter = new SqlDataAdapter($"SELECT distinct Motherboard from PC Where Motherboard LIKE N'{motherboardPC.Text}%'", connectionString);
                    set = new DataSet();
                    adapter.Fill(set);
                    motherboardPC.ItemsSource = set.Tables[0].DefaultView;
                    motherboardPC.DisplayMemberPath = "Motherboard";
                    motherboardPC.IsDropDownOpen = true;
                    break;
                case "cpuPC":
                    adapter = new SqlDataAdapter($"SELECT distinct CPUModel from PC Where CPUModel LIKE N'{cpuPC.Text}%'", connectionString);
                    set = new DataSet();
                    adapter.Fill(set);
                    cpuPC.ItemsSource = set.Tables[0].DefaultView;
                    cpuPC.DisplayMemberPath = "CPUModel";
                    cpuPC.IsDropDownOpen = true;
                    break;
            }
        }

        private void changePopup_Opened(object sender, EventArgs e)
        {
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
                                            inventoryNumberPC.Text = pc.InventoryNumber.ToString();
                                            namePC.Text = pc.Name;
                                            costPC.Text = pc.Cost.ToString();
                                            motherboardPC.Text = pc.Motherboard;
                                            cpuPC.Text = pc.CPU;
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
                                    notebookGrid.Visibility = Visibility.Hidden;
                                    pcGrid.Visibility = Visibility.Visible;
                                    dataAdapter = new SqlDataAdapter("SELECT * FROM GetAllLocationByTypeDeviceID(1)", connectionString);
                                    dataSet = new DataSet();
                                    dataAdapter.Fill(dataSet);
                                    locationPC.ItemsSource = dataSet.Tables[0].DefaultView;
                                    locationPC.DisplayMemberPath = "Place";
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

        private void GetValueVideoConnectors(object sender)
        {

        }
    }
}
