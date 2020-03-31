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
        Int32 DeviceID;
        bool isPreOpenPopup;

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
            DeviceID = Convert.ToInt32(row[0]);
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
                    view.ItemsSource = set.Tables[0].DefaultView;
                    if (view.Columns.Count > 0)
                    {
                        view.Columns[0].Visibility = Visibility.Collapsed;
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
                String commandString = $"Update {typeDevice} set InventoryNumber = {Convert.ToUInt32(inventoryNumberPC.Text)}, " +
                    $"Name = N'{namePC.Text}', Cost = {Convert.ToUInt32(costPC.Text)} where ID={DeviceID}";
                SqlCommand command = new SqlCommand(commandString, connection);
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
                case "motherboardPC":
                    adapter = new SqlDataAdapter($"SELECT Motherboard from PC Where Motherboard LIKE N'{motherboardPC.Text}%'", connectionString);
                    set = new DataSet();
                    adapter.Fill(set);
                    motherboardPC.ItemsSource = set.Tables[0].DefaultView;
                    motherboardPC.DisplayMemberPath = "Motherboard";
                    motherboardPC.IsDropDownOpen = true;
                    break;
                case "cpuPC":
                    adapter = new SqlDataAdapter($"SELECT CPUModel from PC Where CPUModel LIKE N'{cpuPC.Text}%'", connectionString);
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
            if (!isPreOpenPopup)
            {
                switch (typeChange)
                {
                    case TypeChange.Change:
                        String commandString;
                        SqlDataReader reader;
                        String connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            switch (typeDevice)
                            {
                                case TypeDevice.PC:
                                    commandString = $"SELECT * FROM dbo.GetPCByID({DeviceID})";
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
                                                Location = reader["Расположение"].ToString()
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
                                    commandString = $"SELECT * FROM dbo.GetNotebookByID({DeviceID})";
                                    reader = new SqlCommand(commandString, connection).ExecuteReader();
                                    if (reader.HasRows)
                                    {
                                        if (reader.Read())
                                        {

                                        }
                                    }
                                    break;
                                case TypeDevice.Monitor:
                                    commandString = $"SELECT * FROM dbo.GetMonitorByID({DeviceID})";
                                    reader = new SqlCommand(commandString, connection).ExecuteReader();
                                    if (reader.HasRows)
                                    {
                                        if (reader.Read())
                                        {

                                        }
                                    }
                                    break;
                                case TypeDevice.Projector:
                                    commandString = $"SELECT * FROM dbo.GetProjectorByID({DeviceID})";
                                    reader = new SqlCommand(commandString, connection).ExecuteReader();
                                    if (reader.HasRows)
                                    {
                                        if (reader.Read())
                                        {

                                        }
                                    }
                                    break;
                                case TypeDevice.InteractiveWhiteboard:
                                    commandString = $"SELECT * FROM dbo.GetBoardByID({DeviceID})";
                                    reader = new SqlCommand(commandString, connection).ExecuteReader();
                                    if (reader.HasRows)
                                    {
                                        if (reader.Read())
                                        {

                                        }
                                    }
                                    break;
                                case TypeDevice.ProjectorScreen:
                                    commandString = $"SELECT * FROM dbo.GetScreenByID({DeviceID})";
                                    reader = new SqlCommand(commandString, connection).ExecuteReader();
                                    if (reader.HasRows)
                                    {
                                        if (reader.Read())
                                        {

                                        }
                                    }
                                    break;
                                case TypeDevice.PrinterScanner:
                                    commandString = $"SELECT * FROM dbo.GetPrinterScannerByID({DeviceID})";
                                    reader = new SqlCommand(commandString, connection).ExecuteReader();
                                    if (reader.HasRows)
                                    {
                                        if (reader.Read())
                                        {

                                        }
                                    }
                                    break;
                                case TypeDevice.NetworkSwitch:
                                    commandString = $"SELECT * FROM dbo.GetNetworkSwitchByID({DeviceID})";
                                    reader = new SqlCommand(commandString, connection).ExecuteReader();
                                    if (reader.HasRows)
                                    {
                                        if (reader.Read())
                                        {

                                        }
                                    }
                                    break;
                                case TypeDevice.OtherEquipment:
                                    commandString = $"SELECT * FROM dbo.GetOtherEquipmentByID({DeviceID})";
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
                        PC pcAdd = new PC();
                        pcGrid.DataContext = pcAdd;
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
    }
}
