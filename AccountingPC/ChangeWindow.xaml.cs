using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace AccountingPC
{
    /// <summary>
    /// Interaction logic for ChangeWindow.xaml
    /// </summary>
    public partial class ChangeWindow : Window
    {
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public static String ConnectionString => connectionString;
        public static readonly RoutedCommand CloseCommand = new RoutedUICommand(
            "Close", "CloseCommand", typeof(AccountingPCWindow),
            new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.Escape) }));
        private AccountingPCWindow accounting;

        private bool IsChangeAnalog { get; set; }
        public AccountingPCWindow Accounting { get => accounting; private set => accounting = value; }

        internal List<ListBoxItem> videoConnectorsItems;

        private Binding invNBinding;

        SqlDataAdapter aspectRatioDataAdapter;
        SqlDataAdapter cpuDataAdapter;
        SqlDataAdapter osDataAdapter;
        SqlDataAdapter screenInstalledDataAdapter;
        SqlDataAdapter frequencyDataAdapter;
        SqlDataAdapter locationDataAdapter;
        SqlDataAdapter matrixTechnologyDataAdapter;
        SqlDataAdapter paperSizeDataAdapter;
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
        SqlDataAdapter typeLicenseDataAdapter;
        SqlDataAdapter invoiceDataAdapter;

        DataSet aspectRatioDataSet;
        DataSet cpuDataSet;
        DataSet osDataSet;
        DataSet screenInstalledDataSet;
        DataSet frequencyDataSet;
        DataSet locationDataSet;
        DataSet matrixTechnologyDataSet;
        DataSet paperSizeDataSet;
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
        DataSet typeLicenseDataSet;
        DataSet invoiceDataSet;

        public ChangeWindow(AccountingPCWindow window)
        {
            InitializeComponent();
            Owner = window;
            Accounting = window;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            switch (Accounting.NowView)
            {
                case View.Equipment:
                    changeSoftwareGrid.Visibility = Visibility.Collapsed;
                    changeEquipmentGrid.Visibility = Visibility.Visible;
                    if (!Accounting.IsPreOpenEquipmentPopup)
                    {
                        Initialize();
                        switch (Accounting.TypeChange)
                        {
                            case TypeChange.Change:
                                switch (Accounting.TypeDevice)
                                {
                                    case TypeDevice.PC:
                                        GetPC(device, Accounting.DeviceID);
                                        break;
                                    case TypeDevice.Notebook:
                                        GetNotebook(device, Accounting.DeviceID);
                                        break;
                                    case TypeDevice.Monitor:
                                        GetMonitor(device, Accounting.DeviceID);
                                        break;
                                    case TypeDevice.Projector:
                                        GetProjector(device, Accounting.DeviceID);
                                        break;
                                    case TypeDevice.InteractiveWhiteboard:
                                        GetInteractiveWhiteboard(device, Accounting.DeviceID);
                                        break;
                                    case TypeDevice.ProjectorScreen:
                                        GetProjectorScreen(device, Accounting.DeviceID);
                                        break;
                                    case TypeDevice.PrinterScanner:
                                        GetPrinterScanner(device, Accounting.DeviceID);
                                        break;
                                    case TypeDevice.NetworkSwitch:
                                        GetNetworkSwitch(device, Accounting.DeviceID);
                                        break;
                                    case TypeDevice.OtherEquipment:
                                        GetOtherEquipment(device, Accounting.DeviceID);
                                        break;
                                }
                                SetDeviceLocationAndInvoice(device);
                                break;
                            case TypeChange.Add:
                                switch (Accounting.TypeDevice)
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
                    break;
                case View.Software:
                    changeEquipmentGrid.Visibility = Visibility.Collapsed;
                    changeSoftwareGrid.Visibility = Visibility.Visible;
                    if (!Accounting.IsPreOpenSoftwarePopup)
                    {
                        Initialize();
                        switch (Accounting.TypeChange)
                        {
                            case TypeChange.Add:
                                switch (Accounting.TypeSoft)
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
                                switch (Accounting.TypeSoft)
                                {
                                    case TypeSoft.LicenseSoftware:
                                        GetLicenseSoftware(soft, Accounting.SoftwareID);
                                        //AddSoftware();
                                        break;
                                    case TypeSoft.OS:
                                        GetOS(soft, Accounting.SoftwareID);
                                        //AddOS();
                                        break;
                                }
                                break;
                        }
                    }
                    break;
            }
        }

        private void Initialize()
        {
            switch (Accounting.NowView)
            {
                case View.Equipment:
                    ResetEquipmentGrid();
                    switch (Accounting.TypeDevice)
                    {
                        case TypeDevice.InteractiveWhiteboard:
                            InitializeForInteractiveWhiteboard();
                            break;
                        case TypeDevice.Monitor:
                            InitializeForMonitor();
                            break;
                        case TypeDevice.NetworkSwitch:
                            InitializeForNetworkSwitch();
                            break;
                        case TypeDevice.Notebook:
                            InitializeForNotebook();
                            break;
                        case TypeDevice.OtherEquipment:
                            InitializeForOtherEquipment();
                            break;
                        case TypeDevice.PC:
                            InitializeForPC();
                            break;
                        case TypeDevice.PrinterScanner:
                            InitializeForPrinterScanner();
                            break;
                        case TypeDevice.Projector:
                            InitializeForProjector();
                            break;
                        case TypeDevice.ProjectorScreen:
                            InitializeForProjectorScreen();
                            break;
                    }
                    break;
                case View.Software:
                    switch (Accounting.TypeSoft)
                    {
                        case TypeSoft.LicenseSoftware:
                            InitializeForSoftware();
                            break;
                        case TypeSoft.OS:
                            InitializeForOS();
                            break;
                    }
                    break;
            }
            UpdateSource();
        }

        private void GetPC(Device d, int id)
        {
            String commandString;
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = $"SELECT * FROM dbo.GetPCByID({id})";
                reader = new SqlCommand(commandString, connection).ExecuteReader();
                if (reader.HasRows)
                {
                    if (reader.Read())
                    {
                        d = new PC()
                        {
                            InventoryNumber = Convert.ToUInt32(reader["InventoryNumber"]),
                            Name = reader["Name"].ToString(),
                            Cost = Convert.ToSingle(reader["Cost"]),
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
                            InvoiceNumber = reader["InvoiceNumber"].ToString(),
                            PlaceID = Convert.ToUInt32(reader["PlaceID"].GetType() != typeof(DBNull) ? reader["PlaceID"] : 0),
                            VideoConnectorsValue = Convert.ToInt32(reader["VideoConnectors"].GetType() != typeof(DBNull) ? reader["VideoConnectors"] : 0),
                        };
                        PC pc = d as PC;
                        pc.VideoConnectors = GetListVideoConnectors(pc.VideoConnectorsValue);

                        inventoryNumber.Text = d.InventoryNumber.ToString();
                        name.Text = d.Name;
                        cost.Text = d.Cost.ToString();
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
            }
        }

        private void GetNotebook(Device d, int id)
        {
            String commandString;
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = $"SELECT * FROM dbo.GetNotebookByID({id})";
                reader = new SqlCommand(commandString, connection).ExecuteReader();
                if (reader.HasRows)
                {
                    if (reader.Read())
                    {
                        d = new Notebook()
                        {
                            InventoryNumber = Convert.ToUInt32(reader["InventoryNumber"]),
                            Name = reader["Name"].ToString(),
                            Cost = Convert.ToSingle(reader["Cost"]),
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
                            InvoiceNumber = reader["InvoiceNumber"].ToString(),
                            PlaceID = Convert.ToUInt32(reader["PlaceID"].GetType() != typeof(DBNull) ? reader["PlaceID"] : 0),
                            Diagonal = Convert.ToSingle(reader["ScreenDiagonal"].GetType() != typeof(DBNull) ? reader["ScreenDiagonal"] : 0),
                            ResolutionID = Convert.ToUInt32(reader["ResolutionID"].GetType() != typeof(DBNull) ? reader["ResolutionID"] : 0),
                            FrequencyID = Convert.ToUInt32(reader["FrequencyID"].GetType() != typeof(DBNull) ? reader["FrequencyID"] : 0),
                            MatrixTechnologyID = Convert.ToUInt32(reader["MatrixTechnologyID"].GetType() != typeof(DBNull) ? reader["MatrixTechnologyID"] : 0),
                            VideoConnectorsValue = Convert.ToInt32(reader["VideoConnectors"].GetType() != typeof(DBNull) ? reader["VideoConnectors"] : 0),
                        };
                        Notebook notebook = d as Notebook;
                        notebook.VideoConnectors = GetListVideoConnectors(notebook.VideoConnectorsValue);

                        inventoryNumber.Text = d.InventoryNumber.ToString();
                        name.Text = d.Name;
                        cost.Text = d.Cost.ToString();
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
            }
        }

        private void GetMonitor(Device d, int id)
        {
            String commandString;
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = $"SELECT * FROM dbo.GetMonitorByID({id})";
                reader = new SqlCommand(commandString, connection).ExecuteReader();
                if (reader.HasRows)
                {
                    if (reader.Read())
                    {
                        d = new Monitor()
                        {
                            InventoryNumber = Convert.ToUInt32(reader["InventoryNumber"]),
                            Name = reader["Name"].ToString(),
                            Cost = Convert.ToSingle(reader["Cost"]),
                            InvoiceNumber = reader["InvoiceNumber"].ToString(),
                            PlaceID = Convert.ToUInt32(reader["PlaceID"].GetType() != typeof(DBNull) ? reader["PlaceID"] : 0),
                            Diagonal = Convert.ToSingle(reader["ScreenDiagonal"].GetType() != typeof(DBNull) ? reader["ScreenDiagonal"] : 0),
                            ResolutionID = Convert.ToUInt32(reader["ResolutionID"].GetType() != typeof(DBNull) ? reader["ResolutionID"] : 0),
                            FrequencyID = Convert.ToUInt32(reader["FrequencyID"].GetType() != typeof(DBNull) ? reader["FrequencyID"] : 0),
                            MatrixTechnologyID = Convert.ToUInt32(reader["MatrixTechnologyID"].GetType() != typeof(DBNull) ? reader["MatrixTechnologyID"] : 0),
                            VideoConnectorsValue = Convert.ToInt32(reader["VideoConnectors"].GetType() != typeof(DBNull) ? reader["VideoConnectors"] : 0),
                        };
                        Monitor monitor = d as Monitor;
                        monitor.VideoConnectors = GetListVideoConnectors(monitor.VideoConnectorsValue);

                        inventoryNumber.Text = d.InventoryNumber.ToString();
                        name.Text = d.Name;
                        cost.Text = d.Cost.ToString();
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
            }
        }

        private void GetProjector(Device d, int id)
        {
            String commandString;
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = $"SELECT * FROM dbo.GetProjectorByID({id})";
                reader = new SqlCommand(commandString, connection).ExecuteReader();
                if (reader.HasRows)
                {
                    if (reader.Read())
                    {
                        d = new Projector()
                        {
                            InventoryNumber = Convert.ToUInt32(reader["InventoryNumber"]),
                            Name = reader["Name"].ToString(),
                            Cost = Convert.ToSingle(reader["Cost"]),
                            InvoiceNumber = reader["InvoiceNumber"].ToString(),
                            PlaceID = Convert.ToUInt32(reader["PlaceID"].GetType() != typeof(DBNull) ? reader["PlaceID"] : 0),
                            Diagonal = Convert.ToSingle(reader["MaxDiagonal"].GetType() != typeof(DBNull) ? reader["MaxDiagonal"] : 0),
                            ResolutionID = Convert.ToUInt32(reader["ResolutionID"].GetType() != typeof(DBNull) ? reader["ResolutionID"] : 0),
                            ProjectorTechnologyID = Convert.ToUInt32(reader["ProjectorTechnologyID"].GetType() != typeof(DBNull) ? reader["ProjectorTechnologyID"] : 0),
                            VideoConnectorsValue = Convert.ToInt32(reader["VideoConnectors"].GetType() != typeof(DBNull) ? reader["VideoConnectors"] : 0),
                        };
                        Projector projector = d as Projector;
                        projector.VideoConnectors = GetListVideoConnectors(projector.VideoConnectorsValue);

                        inventoryNumber.Text = d.InventoryNumber.ToString();
                        name.Text = d.Name;
                        cost.Text = d.Cost.ToString();
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
            }
        }

        private void GetInteractiveWhiteboard(Device d, int id)
        {
            String commandString;
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = $"SELECT * FROM dbo.GetBoardByID({id})";
                reader = new SqlCommand(commandString, connection).ExecuteReader();
                if (reader.HasRows)
                {
                    if (reader.Read())
                    {
                        d = new InteractiveWhiteboard()
                        {
                            InventoryNumber = Convert.ToUInt32(reader["InventoryNumber"]),
                            Name = reader["Name"].ToString(),
                            Cost = Convert.ToSingle(reader["Cost"]),
                            InvoiceNumber = reader["InvoiceNumber"].ToString(),
                            PlaceID = Convert.ToUInt32(reader["PlaceID"].GetType() != typeof(DBNull) ? reader["PlaceID"] : 0),
                            Diagonal = Convert.ToSingle(reader["Diagonal"].GetType() != typeof(DBNull) ? reader["Diagonal"] : 0),
                        };
                        InteractiveWhiteboard board = d as InteractiveWhiteboard;

                        inventoryNumber.Text = d.InventoryNumber.ToString();
                        name.Text = d.Name;
                        cost.Text = d.Cost.ToString();
                        diagonal.Text = board.Diagonal.ToString();
                    }
                }
            }
        }

        private void GetProjectorScreen(Device d, int id)
        {
            String commandString;
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = $"SELECT * FROM dbo.GetScreenByID({id})";
                reader = new SqlCommand(commandString, connection).ExecuteReader();
                if (reader.HasRows)
                {
                    if (reader.Read())
                    {
                        d = new ProjectorScreen()
                        {
                            InventoryNumber = Convert.ToUInt32(reader["InventoryNumber"]),
                            Name = reader["Name"].ToString(),
                            Cost = Convert.ToSingle(reader["Cost"]),
                            InvoiceNumber = reader["InvoiceNumber"].ToString(),
                            PlaceID = Convert.ToUInt32(reader["PlaceID"].GetType() != typeof(DBNull) ? reader["PlaceID"] : 0),
                            Diagonal = Convert.ToSingle(reader["Diagonal"].GetType() != typeof(DBNull) ? reader["Diagonal"] : 0),
                            AspectRatioID = Convert.ToUInt32(reader["AspectRatioID"].GetType() != typeof(DBNull) ? reader["AspectRatioID"] : 0),
                            ScreenInstalledID = Convert.ToUInt32(reader["ScreenInstalledID"].GetType() != typeof(DBNull) ? reader["ScreenInstalledID"] : 0),
                            IsElectronicDrive = Convert.ToBoolean(reader["IsElectronicDrive"].GetType() != typeof(DBNull) ? reader["IsElectronicDrive"] : 0),
                        };
                        ProjectorScreen screen = d as ProjectorScreen;

                        inventoryNumber.Text = d.InventoryNumber.ToString();
                        name.Text = d.Name;
                        cost.Text = d.Cost.ToString();
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
            }
        }

        private void GetPrinterScanner(Device d, int id)
        {
            String commandString;
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = $"SELECT * FROM dbo.GetPrinterScannerByID({id})";
                reader = new SqlCommand(commandString, connection).ExecuteReader();
                if (reader.HasRows)
                {
                    if (reader.Read())
                    {
                        d = new PrinterScanner()
                        {
                            InventoryNumber = Convert.ToUInt32(reader["InventoryNumber"]),
                            TypeID = Convert.ToUInt32(reader["TypePrinterID"]),
                            Name = reader["Name"].ToString(),
                            Cost = Convert.ToSingle(reader["Cost"]),
                            InvoiceNumber = reader["InvoiceNumber"].ToString(),
                            PlaceID = Convert.ToUInt32(reader["PlaceID"].GetType() != typeof(DBNull) ? reader["PlaceID"] : 0),
                            PaperSizeID = Convert.ToUInt32(reader["PaperSizeID"].GetType() != typeof(DBNull) ? reader["PaperSizeID"] : 0),
                        };
                        PrinterScanner printerScanner = d as PrinterScanner;

                        inventoryNumber.Text = d.InventoryNumber.ToString();
                        name.Text = d.Name;
                        cost.Text = d.Cost.ToString();
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
            }
        }

        private void GetNetworkSwitch(Device d, int id)
        {
            String commandString;
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = $"SELECT * FROM dbo.GetNetworkSwitchByID({id})";
                reader = new SqlCommand(commandString, connection).ExecuteReader();
                if (reader.HasRows)
                {
                    if (reader.Read())
                    {
                        d = new NetworkSwitch()
                        {
                            InventoryNumber = Convert.ToUInt32(reader["InventoryNumber"]),
                            TypeID = Convert.ToUInt32(reader["TypeID"]),
                            Name = reader["Name"].ToString(),
                            Cost = Convert.ToSingle(reader["Cost"]),
                            InvoiceNumber = reader["InvoiceNumber"].ToString(),
                            PlaceID = Convert.ToUInt32(reader["PlaceID"].GetType() != typeof(DBNull) ? reader["PlaceID"] : 0),
                            Ports = Convert.ToUInt32(reader["NumberOfPorts"].GetType() != typeof(DBNull) ? reader["NumberOfPorts"] : 0),
                            WiFiFrequencyID = Convert.ToUInt32(reader["WiFiFrequencyID"].GetType() != typeof(DBNull) ? reader["WiFiFrequencyID"] : 0),
                        };
                        NetworkSwitch networkSwitch = d as NetworkSwitch;

                        inventoryNumber.Text = d.InventoryNumber.ToString();
                        name.Text = d.Name;
                        cost.Text = d.Cost.ToString();
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
            }
        }

        private void GetOtherEquipment(Device d, int id)
        {
            String commandString;
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = $"SELECT * FROM dbo.GetOtherEquipmentByID({id})";
                reader = new SqlCommand(commandString, connection).ExecuteReader();
                if (reader.HasRows)
                {
                    if (reader.Read())
                    {
                        d = new OtherEquipment()
                        {
                            InventoryNumber = Convert.ToUInt32(reader["InventoryNumber"]),
                            Name = reader["Name"].ToString(),
                            Cost = Convert.ToSingle(reader["Cost"]),
                            InvoiceNumber = reader["InvoiceNumber"].ToString(),
                            PlaceID = Convert.ToUInt32(reader["PlaceID"].GetType() != typeof(DBNull) ? reader["PlaceID"] : 0),
                        };

                        inventoryNumber.Text = d.InventoryNumber.ToString();
                        name.Text = d.Name;
                        cost.Text = d.Cost.ToString();
                    }
                }
            }
        }

        private void SetDeviceLocationAndInvoice(Device d)
        {
            foreach (object obj in location.ItemsSource)
            {
                DataRowView row;
                row = (obj as DataRowView);
                if (Convert.ToUInt32(row.Row[0]) == d.PlaceID)
                {
                    location.SelectedItem = row;
                    break;
                }
            }
            invoice.Text = d.InvoiceNumber;
        }

        private void ResetEquipmentGrid()
        {
            imageFilename.Text = string.Empty;
            if (Accounting.TypeChange == TypeChange.Add)
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
            else if (Accounting.TypeChange == TypeChange.Change)
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

        private void InitializeForInteractiveWhiteboard()
        {
            invoiceGrid.Visibility = Visibility.Visible;
            locationGrid.Visibility = Visibility.Visible;
            diagonalGrid.Visibility = Visibility.Visible;
            GridPlacement(diagonalGrid, 0, 1, 2);
            GridPlacement(invoiceGrid, 2, 1, 3);
            GridPlacement(locationGrid, 5, 1, 7);
        }

        private void InitializeForMonitor()
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

        private void InitializeForNetworkSwitch()
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

        private void InitializeForNotebook()
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

        private void InitializeForOtherEquipment()
        {
            invoiceGrid.Visibility = Visibility.Visible;
            locationGrid.Visibility = Visibility.Visible;
            GridPlacement(invoiceGrid, 0, 1, 3);
            GridPlacement(locationGrid, 3, 1, 9);
        }

        private void InitializeForPC()
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

        private void InitializeForPrinterScanner()
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

        private void InitializeForProjector()
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

        private void InitializeForProjectorScreen()
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

        private void InitializeForSoftware()
        {
            softwareNameGrid.Visibility = Visibility.Visible;
            softwareCostGrid.Visibility = Visibility.Visible;
            softwareCountGrid.Visibility = Visibility.Visible;
            softwareInvoiceGrid.Visibility = Visibility.Visible;
            typeLicenseGrid.Visibility = Visibility.Visible;
            GridPlacement(softwareNameGrid, 0, 0, 4);
            GridPlacement(typeLicenseGrid, 4, 0, 2);
            GridPlacement(softwareCostGrid, 0, 1, 2);
            GridPlacement(softwareCountGrid, 2, 1, 2);
            GridPlacement(softwareInvoiceGrid, 4, 1, 2);
        }

        private void InitializeForOS()
        {
            softwareNameGrid.Visibility = Visibility.Visible;
            softwareCostGrid.Visibility = Visibility.Visible;
            softwareCountGrid.Visibility = Visibility.Visible;
            softwareInvoiceGrid.Visibility = Visibility.Visible;
            typeLicenseGrid.Visibility = Visibility.Hidden;
            GridPlacement(softwareNameGrid, 0, 0, 6);
            GridPlacement(softwareCostGrid, 0, 1, 2);
            GridPlacement(softwareCountGrid, 2, 1, 2);
            GridPlacement(softwareInvoiceGrid, 4, 1, 2);
        }

        private void GridPlacement(UIElement element, int column, int row, int colSpan, int rowSpan = 1)
        {
            Grid.SetColumn(element, column);
            Grid.SetRow(element, row);
            Grid.SetColumnSpan(element, colSpan);
            Grid.SetRowSpan(element, rowSpan);
        }

        public static String GetVideoConnectors(Int32 value)
        {
            List<String> arr = GetListVideoConnectors(value);
            String res = String.Empty;
            for (int i = 0; i < arr.Count; i++)
            {
                res += $"{arr[i]}";
                if (i < arr.Count - 1)
                    res += ", ";
            }
            return res;
        }

        private static List<String> GetListVideoConnectors(Int32 value)
        {
            List<String> arr = new List<String>();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlDataReader reader = new SqlCommand("Select * from dbo.GetAllVideoConnector() Order by value desc", connection).ExecuteReader();
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
            foreach (var obj in list.SelectedItems)
            {
                foreach (DataRowView row in videoConnectorsDataSet.Tables[0].DefaultView)
                {
                    string s = (obj as ListBoxItem).Content.ToString();
                    if (row.Row[1].ToString() == s)
                        value += Convert.ToInt32(row.Row[2]);
                }
            }
            return value;
        }

        private void DisabledRepeatInvN_Checked()
        {
            invNBinding = new Binding();
            invNBinding.Path = new PropertyPath("InventoryNumber");
            invNBinding.ValidationRules.Clear();
            invNBinding.ValidationRules.Add(new DataErrorValidationRule());
            invNBinding.ValidationRules.Add(new InventoryNumberValidationRule());
            inventoryNumber.SetBinding(TextBox.TextProperty, invNBinding);
        }

        private void DisabledRepeatInvN_Unchecked()
        {
            invNBinding = new Binding();
            invNBinding.Path = new PropertyPath("InventoryNumber");
            invNBinding.ValidationRules.Clear();
            invNBinding.ValidationRules.Add(new DataErrorValidationRule());
            inventoryNumber.SetBinding(TextBox.TextProperty, invNBinding);
        }

        private void UpdateSource()
        {
            switch (Accounting.NowView)
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
                    UpdateSourceForInvoice();
                    break;
                case View.Software:
                    UpdateSourceForTypeLicense();
                    break;
            }
        }

        private void SaveOrUpdateEquipmentDB()
        {
            Task task;
            switch (Accounting.TypeChange)
            {
                case TypeChange.Add:
                    switch (Accounting.TypeDevice)
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
                    Accounting.statusItem1.Content = "Успешно добавлено";
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
                            Dispatcher.Invoke(() =>
                            {
                                Accounting.statusItem1.Content = string.Empty;
                                statusItem1.Content = string.Empty;
                            });

                        }
                        catch { }
                    });
                    task.Start();
                    break;
                case TypeChange.Change:
                    switch (Accounting.TypeDevice)
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
                    Accounting.statusItem1.Content = "Успешно изменено";
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
                            Dispatcher.Invoke(() =>
                            {
                                Accounting.statusItem1.Content = string.Empty;
                                statusItem1.Content = string.Empty;
                            });

                        }
                        catch { }
                    });
                    task.Start();
                    //changeEquipmentPopup.IsOpen = false;
                    Accounting.IsPreOpenEquipmentPopup = false;
                    Accounting.viewGrid.IsEnabled = true;
                    Accounting.menu.IsEnabled = true;
                    break;
            }
        }

        private void SaveOrUpdateSoftwareDB()
        {
            Task task;
            switch (Accounting.TypeChange)
            {
                case TypeChange.Add:
                    switch (Accounting.TypeSoft)
                    {
                        case TypeSoft.LicenseSoftware:
                            AddSoftware();
                            break;
                        case TypeSoft.OS:
                            AddOS();
                            break;
                    }
                    Accounting.statusItem1.Content = "Успешно добавлено";
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
                            Dispatcher.Invoke(() =>
                            {
                                Accounting.statusItem1.Content = string.Empty;
                                statusItem1.Content = string.Empty;
                            });

                        }
                        catch { }
                    });
                    task.Start();
                    break;
                case TypeChange.Change:
                    switch (Accounting.TypeSoft)
                    {
                        case TypeSoft.LicenseSoftware:
                            UpdateSoftware();
                            break;
                        case TypeSoft.OS:
                            UpdateOS();
                            break;
                    }
                    Accounting.statusItem1.Content = "Успешно изменено";
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
                            Dispatcher.Invoke(() =>
                            {
                                Accounting.statusItem1.Content = string.Empty;
                                statusItem1.Content = string.Empty;
                            });

                        }
                        catch { }
                    });
                    task.Start();
                    //changeEquipmentPopup.IsOpen = false;
                    Accounting.IsPreOpenEquipmentPopup = false;
                    Accounting.viewGrid.IsEnabled = true;
                    Accounting.menu.IsEnabled = true;
                    break;
            }
        }

        private void UpdateSourceForAspectRatio()
        {
            aspectRatioDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllAspectRatio()", ConnectionString);
            aspectRatioDataSet = new DataSet();
            aspectRatioDataAdapter.Fill(aspectRatioDataSet);
            aspectRatio.ItemsSource = aspectRatioDataSet.Tables[0].DefaultView;
            aspectRatio.DisplayMemberPath = "Name";
        }

        private void UpdateSourceForOS()
        {
            osDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllOS()", ConnectionString);
            osDataSet = new DataSet();
            osDataAdapter.Fill(osDataSet);
            os.ItemsSource = osDataSet.Tables[0].DefaultView;
            os.DisplayMemberPath = "Наименование";
        }

        private void UpdateSourceForScreenInstalled()
        {
            screenInstalledDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllScreenInstalled()", ConnectionString);
            screenInstalledDataSet = new DataSet();
            screenInstalledDataAdapter.Fill(screenInstalledDataSet);
            screenInstalled.ItemsSource = screenInstalledDataSet.Tables[0].DefaultView;
            screenInstalled.DisplayMemberPath = "Name";
        }

        private void UpdateSourceForName()
        {
            switch (Accounting.TypeDevice)
            {
                case TypeDevice.InteractiveWhiteboard:
                    nameDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllBoardName()", ConnectionString);
                    break;
                case TypeDevice.Monitor:
                    nameDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllMonitorName()", ConnectionString);
                    break;
                case TypeDevice.NetworkSwitch:
                    nameDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllNetworkSwitchName()", ConnectionString);
                    break;
                case TypeDevice.Notebook:
                    nameDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllNotebookName()", ConnectionString);
                    break;
                case TypeDevice.OtherEquipment:
                    nameDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllOtherEquipmentName()", ConnectionString);
                    break;
                case TypeDevice.PC:
                    nameDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllPCName()", ConnectionString);
                    break;
                case TypeDevice.PrinterScanner:
                    nameDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllPrinterScannerName()", ConnectionString);
                    break;
                case TypeDevice.Projector:
                    nameDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllProjectorName()", ConnectionString);
                    break;
                case TypeDevice.ProjectorScreen:
                    nameDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllProjectorScreenName()", ConnectionString);
                    break;
            }

            nameDataSet = new DataSet();
            nameDataAdapter.Fill(nameDataSet);
            name.ItemsSource = nameDataSet.Tables[0].DefaultView;
            name.DisplayMemberPath = "Name";
        }

        private void UpdateSourceForLocation()
        {
            switch (Accounting.TypeDevice)
            {
                case TypeDevice.InteractiveWhiteboard:
                    locationDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.[GetAllCanUsedLocationByTypeDeviceID](4)", ConnectionString);
                    break;
                case TypeDevice.Monitor:
                    locationDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.[GetAllCanUsedLocationByTypeDeviceID](6)", ConnectionString);
                    break;
                case TypeDevice.NetworkSwitch:
                    locationDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.[GetAllCanUsedLocationByTypeDeviceID](5)", ConnectionString);
                    break;
                case TypeDevice.Notebook:
                    locationDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.[GetAllCanUsedLocationByTypeDeviceID](2)", ConnectionString);
                    break;
                case TypeDevice.OtherEquipment:
                    locationDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.[GetAllCanUsedLocationByTypeDeviceID](9)", ConnectionString);
                    break;
                case TypeDevice.PC:
                    locationDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.[GetAllCanUsedLocationByTypeDeviceID](1)", ConnectionString);
                    break;
                case TypeDevice.PrinterScanner:
                    locationDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.[GetAllCanUsedLocationByTypeDeviceID](3)", ConnectionString);
                    break;
                case TypeDevice.Projector:
                    locationDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.[GetAllCanUsedLocationByTypeDeviceID](7)", ConnectionString);
                    break;
                case TypeDevice.ProjectorScreen:
                    locationDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.[GetAllCanUsedLocationByTypeDeviceID](8)", ConnectionString);
                    break;
            }

            locationDataSet = new DataSet();
            locationDataAdapter.Fill(locationDataSet);
            location.ItemsSource = locationDataSet.Tables[0].DefaultView;
            location.DisplayMemberPath = "Place";
        }

        private void UpdateSourceForCPU()
        {
            switch (Accounting.TypeDevice)
            {
                case TypeDevice.Notebook:
                    cpuDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllNotebookCPU()", ConnectionString);
                    cpuDataSet = new DataSet();
                    cpuDataAdapter.Fill(cpuDataSet);
                    cpu.ItemsSource = cpuDataSet.Tables[0].DefaultView;
                    cpu.DisplayMemberPath = "CPUModel";
                    break;
                case TypeDevice.PC:
                    cpuDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllPCCPU()", ConnectionString);
                    cpuDataSet = new DataSet();
                    cpuDataAdapter.Fill(cpuDataSet);
                    cpu.ItemsSource = cpuDataSet.Tables[0].DefaultView;
                    cpu.DisplayMemberPath = "CPUModel";
                    break;
            }
        }

        private void UpdateSourceForVideoCard()
        {
            switch (Accounting.TypeDevice)
            {
                case TypeDevice.Notebook:
                    vCardDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllNotebookvCard()", ConnectionString);
                    vCardDataSet = new DataSet();
                    vCardDataAdapter.Fill(vCardDataSet);
                    vCard.ItemsSource = vCardDataSet.Tables[0].DefaultView;
                    vCard.DisplayMemberPath = "VideoCard";
                    break;
                case TypeDevice.PC:
                    vCardDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllPCvCard()", ConnectionString);
                    vCardDataSet = new DataSet();
                    vCardDataAdapter.Fill(vCardDataSet);
                    vCard.ItemsSource = vCardDataSet.Tables[0].DefaultView;
                    vCard.DisplayMemberPath = "VideoCard";
                    break;
            }
        }

        private void UpdateSourceForType()
        {
            switch (Accounting.TypeDevice)
            {
                case TypeDevice.NetworkSwitch:
                    typeNetworkSwitchDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllTypeNetworkSwitch()", ConnectionString);
                    typeNetworkSwitchDataSet = new DataSet();
                    typeNetworkSwitchDataAdapter.Fill(typeNetworkSwitchDataSet);
                    type.ItemsSource = typeNetworkSwitchDataSet.Tables[0].DefaultView;
                    type.DisplayMemberPath = "Name";
                    break;
                case TypeDevice.Notebook:
                    typeNotebookDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllTypeNotebook()", ConnectionString);
                    typeNotebookDataSet = new DataSet();
                    typeNotebookDataAdapter.Fill(typeNotebookDataSet);
                    type.ItemsSource = typeNotebookDataSet.Tables[0].DefaultView;
                    type.DisplayMemberPath = "Name";
                    break;
                case TypeDevice.PrinterScanner:
                    typePrinterDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllTypePrinter()", ConnectionString);
                    typePrinterDataSet = new DataSet();
                    typePrinterDataAdapter.Fill(typePrinterDataSet);
                    type.ItemsSource = typePrinterDataSet.Tables[0].DefaultView;
                    type.DisplayMemberPath = "Name";
                    break;
            }
        }

        private void UpdateSourceForFrequency()
        {
            frequencyDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllFrequency()", ConnectionString);
            frequencyDataSet = new DataSet();
            frequencyDataAdapter.Fill(frequencyDataSet);
            screenFrequency.ItemsSource = frequencyDataSet.Tables[0].DefaultView;
            screenFrequency.DisplayMemberPath = "Name";
        }

        private void UpdateSourceForMatrixTechology()
        {
            matrixTechnologyDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllMatrixTechnology()", ConnectionString);
            matrixTechnologyDataSet = new DataSet();
            matrixTechnologyDataAdapter.Fill(matrixTechnologyDataSet);
            matrixTechnology.ItemsSource = matrixTechnologyDataSet.Tables[0].DefaultView;
            matrixTechnology.DisplayMemberPath = "Name";
        }

        private void UpdateSourceForPaperSize()
        {
            paperSizeDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllPaperSize()", ConnectionString);
            paperSizeDataSet = new DataSet();
            paperSizeDataAdapter.Fill(paperSizeDataSet);
            paperSize.ItemsSource = paperSizeDataSet.Tables[0].DefaultView;
            paperSize.DisplayMemberPath = "Name";
        }

        private void UpdateSourceForProjectorTechnology()
        {
            projectorTechnologyDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllProjectorTechnology()", ConnectionString);
            projectorTechnologyDataSet = new DataSet();
            projectorTechnologyDataAdapter.Fill(projectorTechnologyDataSet);
            projectorTechnology.ItemsSource = projectorTechnologyDataSet.Tables[0].DefaultView;
            projectorTechnology.DisplayMemberPath = "Name";
        }

        private void UpdateSourceForResolution()
        {
            resolutionDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllResolution()", ConnectionString);
            resolutionDataSet = new DataSet();
            resolutionDataAdapter.Fill(resolutionDataSet);
            resolution.ItemsSource = resolutionDataSet.Tables[0].DefaultView;
            resolution.DisplayMemberPath = "Name";
        }

        private void UpdateSourceForVideoConnectors()
        {
            videoConnectorsDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllVideoConnector()", ConnectionString);
            videoConnectorsDataSet = new DataSet();
            videoConnectorsDataAdapter.Fill(videoConnectorsDataSet);
            videoConnectorsItems = new List<ListBoxItem>();
            foreach (DataRowView row in videoConnectorsDataSet.Tables[0].DefaultView)
                videoConnectorsItems.Add(new ListBoxItem() { Content = row.Row[1].ToString() });
            vConnectors.ItemsSource = videoConnectorsItems;
        }

        private void UpdateSourceForWifiFrequency()
        {
            wifiFrequencyDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllWiFiFrequency()", ConnectionString);
            wifiFrequencyDataSet = new DataSet();
            wifiFrequencyDataAdapter.Fill(wifiFrequencyDataSet);
            wifiFrequency.ItemsSource = wifiFrequencyDataSet.Tables[0].DefaultView;
            wifiFrequency.DisplayMemberPath = "Name";
        }

        private void UpdateSourceForInvoice()
        {
            invoiceDataAdapter = new SqlDataAdapter($"SELECT [Number] FROM dbo.GetAllInvoice()", ConnectionString);
            invoiceDataSet = new DataSet();
            invoiceDataAdapter.Fill(invoiceDataSet);
            invoice.ItemsSource = invoiceDataSet.Tables[0].DefaultView;
            invoice.DisplayMemberPath = "Number";
            softwareInvoice.ItemsSource = invoiceDataSet.Tables[0].DefaultView;
            softwareInvoice.DisplayMemberPath = "Number";
        }

        private void UpdateSourceForMotherboard()
        {
            motherboardDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllMotherboard()", ConnectionString);
            motherboardDataSet = new DataSet();
            motherboardDataAdapter.Fill(motherboardDataSet);
            motherboard.ItemsSource = motherboardDataSet.Tables[0].DefaultView;
            motherboard.DisplayMemberPath = "Motherboard";
        }

        private void UpdateSourceForTypeLicense()
        {
            typeLicenseDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllTypeSoftLicense()", ConnectionString);
            typeLicenseDataSet = new DataSet();
            typeLicenseDataAdapter.Fill(typeLicenseDataSet);
            typeLicense.ItemsSource = typeLicenseDataSet.Tables[0].DefaultView;
            typeLicense.DisplayMemberPath = "Name";
        }

        private void AddPC()
        {
            String commandString;
            SqlCommand command;
            int temp;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = "AddPC";
                command = new SqlCommand(commandString, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt32(inventoryNumber.Text)));
                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToSingle(cost.Text)));
                command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text == String.Empty ? null : invoice.Text));
                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?.Row?[0]));
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
                    command.Parameters.Add(new SqlParameter("@VRAM", temp));
                command.Parameters.Add(new SqlParameter("@OSID", ((DataRowView)os?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@MB", motherboard.Text));
                command.Parameters.Add(new SqlParameter("@VConnectors", GetValueVideoConnectors(vConnectors)));
                byte[] bytes = LoadImage(imageFilename.Text);
                if (bytes != null)
                    command.Parameters.Add(new SqlParameter("@Image", bytes));
                else
                    command.Parameters.Add(new SqlParameter("@Image", SqlDbType.VarBinary) { SqlValue = null });
                command.ExecuteNonQuery();
            }
        }

        private void AddNotebook()
        {
            String commandString;
            SqlCommand command;
            int temp;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = "AddNotebook";
                command = new SqlCommand(commandString, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt32(inventoryNumber.Text)));
                command.Parameters.Add(new SqlParameter("@Type", ((DataRowView)type?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToSingle(cost.Text)));
                command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text == String.Empty ? null : invoice.Text));
                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?.Row?[0]));
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
                byte[] bytes = LoadImage(imageFilename.Text);
                if (bytes != null)
                    command.Parameters.Add(new SqlParameter("@Image", bytes));
                command.ExecuteNonQuery();
            }
        }

        private void AddMonitor()
        {
            String commandString;
            SqlCommand command;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = "AddMonitor";
                command = new SqlCommand(commandString, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt32(inventoryNumber.Text)));
                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToSingle(cost.Text)));
                command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text == String.Empty ? null : invoice.Text));
                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?.Row?[0]));
                command.Parameters.Add(new SqlParameter("@Diagonal", Convert.ToSingle(diagonal.Text)));
                command.Parameters.Add(new SqlParameter("@ResolutionID", ((DataRowView)resolution?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@FrequencyID", ((DataRowView)screenFrequency?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@MatrixID", ((DataRowView)matrixTechnology?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@VConnectors", GetValueVideoConnectors(vConnectors)));
                byte[] bytes = LoadImage(imageFilename.Text);
                if (bytes != null)
                    command.Parameters.Add(new SqlParameter("@Image", bytes));
                command.ExecuteNonQuery();
            }
        }

        private void AddNetworkSwitch()
        {
            String commandString;
            SqlCommand command;
            int temp;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = "AddNetworkSwitch";
                command = new SqlCommand(commandString, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt32(inventoryNumber.Text)));
                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToSingle(cost.Text)));
                command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text == String.Empty ? null : invoice.Text));
                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?.Row?[0]));
                temp = Convert.ToInt32(ports.Text);
                if (temp > 0)
                    command.Parameters.Add(new SqlParameter("@Ports", temp));
                command.Parameters.Add(new SqlParameter("@TypeID", ((DataRowView)type?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@Frequency", ((DataRowView)wifiFrequency?.SelectedItem)?[0]));
                byte[] bytes = LoadImage(imageFilename.Text);
                if (bytes != null)
                    command.Parameters.Add(new SqlParameter("@Image", bytes));
                command.ExecuteNonQuery();
            }
        }

        private void AddInteractiveWhiteboard()
        {
            String commandString;
            SqlCommand command;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = "AddInteractiveWhiteboard";
                command = new SqlCommand(commandString, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt32(inventoryNumber.Text)));
                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToSingle(cost.Text)));
                command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text == String.Empty ? null : invoice.Text));
                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?.Row?[0]));
                command.Parameters.Add(new SqlParameter("@Diagonal", Convert.ToSingle(diagonal.Text)));
                byte[] bytes = LoadImage(imageFilename.Text);
                if (bytes != null)
                    command.Parameters.Add(new SqlParameter("@Image", bytes));
                command.ExecuteNonQuery();
            }
        }

        private void AddPrinterScanner()
        {
            String commandString;
            SqlCommand command;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = "AddPrinterScanner";
                command = new SqlCommand(commandString, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt32(inventoryNumber.Text)));
                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToSingle(cost.Text)));
                command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text == String.Empty ? null : invoice.Text));
                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?.Row?[0]));
                command.Parameters.Add(new SqlParameter("@TypeID", ((DataRowView)type?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@PaperSizeID", ((DataRowView)paperSize?.SelectedItem)?[0]));
                byte[] bytes = LoadImage(imageFilename.Text);
                if (bytes != null)
                    command.Parameters.Add(new SqlParameter("@Image", bytes));
                command.ExecuteNonQuery();
            }
        }

        private void AddProjector()
        {
            String commandString;
            SqlCommand command;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = "AddProjector";
                command = new SqlCommand(commandString, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt32(inventoryNumber.Text)));
                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToSingle(cost.Text)));
                command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text == String.Empty ? null : invoice.Text));
                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?.Row?[0]));
                command.Parameters.Add(new SqlParameter("@TechnologyID", ((DataRowView)projectorTechnology?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@Diagonal", Convert.ToSingle(diagonal.Text)));
                command.Parameters.Add(new SqlParameter("@ResolutionID", ((DataRowView)resolution?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@VConnectors", GetValueVideoConnectors(vConnectors)));
                byte[] bytes = LoadImage(imageFilename.Text);
                if (bytes != null)
                    command.Parameters.Add(new SqlParameter("@Image", bytes));
                command.ExecuteNonQuery();
            }
        }

        private void AddProjectorScreen()
        {
            String commandString;
            SqlCommand command;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = "AddProjectorScreen";
                command = new SqlCommand(commandString, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt32(inventoryNumber.Text)));
                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToSingle(cost.Text)));
                command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text == String.Empty ? null : invoice.Text));
                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?.Row?[0]));
                command.Parameters.Add(new SqlParameter("@Diagonal", Convert.ToSingle(diagonal.Text)));
                command.Parameters.Add(new SqlParameter("@IsElectronic", Convert.ToBoolean(isEDrive.IsChecked)));
                command.Parameters.Add(new SqlParameter("@AspectRatioID", ((DataRowView)aspectRatio?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@InstalledID", ((DataRowView)screenInstalled?.SelectedItem)?[0]));
                byte[] bytes = LoadImage(imageFilename.Text);
                if (bytes != null)
                    command.Parameters.Add(new SqlParameter("@Image", bytes));
                command.ExecuteNonQuery();
            }
        }

        private void AddOtherEquipment()
        {
            String commandString;
            SqlCommand command;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = "AddOtherEquipment";
                command = new SqlCommand(commandString, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt32(inventoryNumber.Text)));
                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToSingle(cost.Text)));
                command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text == String.Empty ? null : invoice.Text));
                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?.Row?[0]));
                //command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?[0]));
                byte[] bytes = LoadImage(imageFilename.Text);
                if (bytes != null)
                    command.Parameters.Add(new SqlParameter("@Image", bytes));
                command.ExecuteNonQuery();
            }
        }

        private void UpdatePC()
        {
            String commandString;
            SqlCommand command;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = "UpdatePCByID";
                command = new SqlCommand(commandString, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@ID", Accounting.DeviceID));
                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt32(inventoryNumber.Text)));
                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToSingle(cost.Text)));
                command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text));
                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?.Row?[0]));
                //command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?[0]));
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
                byte[] bytes = LoadImage(imageFilename.Text);
                if (bytes != null)
                    command.Parameters.Add(new SqlParameter("@Image", bytes));
                command.Parameters.Add(new SqlParameter("@IsChangeAnalog", IsChangeAnalog));
                command.ExecuteNonQuery();
            }
        }

        private void UpdateNotebook()
        {
            String commandString;
            SqlCommand command;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = "UpdateNotebookByID";
                command = new SqlCommand(commandString, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@ID", Accounting.DeviceID));
                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt32(inventoryNumber.Text)));
                command.Parameters.Add(new SqlParameter("@Type", ((DataRowView)type?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToSingle(cost.Text)));
                command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text));
                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?.Row?[0]));
                //command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?[0]));
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
                byte[] bytes = LoadImage(imageFilename.Text);
                if (bytes != null)
                    command.Parameters.Add(new SqlParameter("@Image", bytes));
                command.Parameters.Add(new SqlParameter("@IsChangeAnalog", IsChangeAnalog));
                command.ExecuteNonQuery();
            }
        }

        private void UpdateMonitor()
        {
            String commandString;
            SqlCommand command;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = "UpdateMonitorByID";
                command = new SqlCommand(commandString, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@ID", Accounting.DeviceID));
                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt32(inventoryNumber.Text)));
                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToSingle(cost.Text)));
                command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text));
                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?.Row?[0]));
                //command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@Diagonal", Convert.ToSingle(diagonal.Text)));
                command.Parameters.Add(new SqlParameter("@ResolutionID", ((DataRowView)resolution?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@FrequencyID", ((DataRowView)screenFrequency?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@MatrixID", ((DataRowView)matrixTechnology?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@VConnectors", GetValueVideoConnectors(vConnectors)));
                byte[] bytes = LoadImage(imageFilename.Text);
                if (bytes != null)
                    command.Parameters.Add(new SqlParameter("@Image", bytes));
                command.Parameters.Add(new SqlParameter("@IsChangeAnalog", IsChangeAnalog));
                command.ExecuteNonQuery();
            }
        }

        private void UpdateNetworkSwitch()
        {
            String commandString;
            SqlCommand command;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = "UpdateNetworkSwitchByID";
                command = new SqlCommand(commandString, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@ID", Accounting.DeviceID));
                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt32(inventoryNumber.Text)));
                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToSingle(cost.Text)));
                command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text == String.Empty ? null : invoice.Text));
                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?.Row?[0]));
                //command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@NumberOfPorts", Convert.ToInt32(ports.Text)));
                command.Parameters.Add(new SqlParameter("@TypeID", ((DataRowView)type?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@Frequency", ((DataRowView)wifiFrequency?.SelectedItem)?[0]));
                byte[] bytes = LoadImage(imageFilename.Text);
                if (bytes != null)
                    command.Parameters.Add(new SqlParameter("@Image", bytes));
                command.Parameters.Add(new SqlParameter("@IsChangeAnalog", IsChangeAnalog));
                command.ExecuteNonQuery();
            }
        }

        private void UpdateInteractiveWhiteboard()
        {
            String commandString;
            SqlCommand command;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = "UpdateInteractiveWhiteboardByID";
                command = new SqlCommand(commandString, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@ID", Accounting.DeviceID));
                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt32(inventoryNumber.Text)));
                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToSingle(cost.Text)));
                command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text == String.Empty ? null : invoice.Text));
                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?.Row?[0]));
                //command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@Diagonal", Convert.ToSingle(diagonal.Text)));
                byte[] bytes = LoadImage(imageFilename.Text);
                if (bytes != null)
                    command.Parameters.Add(new SqlParameter("@Image", bytes));
                command.Parameters.Add(new SqlParameter("@IsChangeAnalog", IsChangeAnalog));
                command.ExecuteNonQuery();
            }
        }

        private void UpdatePrinterScanner()
        {
            String commandString;
            SqlCommand command;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = "UpdatePrinterScannerByID";
                command = new SqlCommand(commandString, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@ID", Accounting.DeviceID));
                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt32(inventoryNumber.Text)));
                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToSingle(cost.Text)));
                command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text == String.Empty ? null : invoice.Text));
                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?.Row?[0]));
                //command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@TypeID", ((DataRowView)type?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@PaperSizeID", ((DataRowView)paperSize?.SelectedItem)?[0]));
                byte[] bytes = LoadImage(imageFilename.Text);
                if (bytes != null)
                    command.Parameters.Add(new SqlParameter("@Image", bytes));
                command.Parameters.Add(new SqlParameter("@IsChangeAnalog", IsChangeAnalog));
                command.ExecuteNonQuery();
            }
        }

        private void UpdateProjector()
        {
            String commandString;
            SqlCommand command;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = "UpdateProjectorByID";
                command = new SqlCommand(commandString, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@ID", Accounting.DeviceID));
                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt32(inventoryNumber.Text)));
                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToSingle(cost.Text)));
                command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text == String.Empty ? null : invoice.Text));
                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?.Row?[0]));
                //command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@TechnologyID", ((DataRowView)projectorTechnology?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@Diagonal", Convert.ToSingle(diagonal.Text)));
                command.Parameters.Add(new SqlParameter("@ResolutionID", ((DataRowView)resolution?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@VConnectors", GetValueVideoConnectors(vConnectors)));
                byte[] bytes = LoadImage(imageFilename.Text);
                if (bytes != null)
                    command.Parameters.Add(new SqlParameter("@Image", bytes));
                command.Parameters.Add(new SqlParameter("@IsChangeAnalog", IsChangeAnalog));
                command.ExecuteNonQuery();
            }
        }

        private void UpdateProjectorScreen()
        {
            String commandString;
            SqlCommand command;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = "UpdateProjectorScreenByID";
                command = new SqlCommand(commandString, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@ID", Accounting.DeviceID));
                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt32(inventoryNumber.Text)));
                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToSingle(cost.Text)));
                command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text == String.Empty ? null : invoice.Text));
                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?.Row?[0]));
                //command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@Diagonal", Convert.ToSingle(diagonal.Text)));
                command.Parameters.Add(new SqlParameter("@IsEDrive", Convert.ToBoolean(isEDrive.IsChecked)));
                command.Parameters.Add(new SqlParameter("@AspectRatioID", ((DataRowView)aspectRatio?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@InstalledID", ((DataRowView)screenInstalled?.SelectedItem)?[0]));
                byte[] bytes = LoadImage(imageFilename.Text);
                if (bytes != null)
                    command.Parameters.Add(new SqlParameter("@Image", bytes));
                command.Parameters.Add(new SqlParameter("@IsChangeAnalog", IsChangeAnalog));
                command.ExecuteNonQuery();
            }
        }

        private void UpdateOtherEquipment()
        {
            String commandString;
            SqlCommand command;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = "UpdateOtherEquipmentByID";
                command = new SqlCommand(commandString, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@ID", Accounting.DeviceID));
                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt32(inventoryNumber.Text)));
                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToSingle(cost.Text)));
                command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text == String.Empty ? null : invoice.Text));
                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?.Row?[0]));
                //command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?[0]));
                byte[] bytes = LoadImage(imageFilename.Text);
                if (bytes != null)
                    command.Parameters.Add(new SqlParameter("@Image", bytes));
                command.Parameters.Add(new SqlParameter("@IsChangeAnalog", IsChangeAnalog));
                command.ExecuteNonQuery();
            }
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
            else if (Accounting.TypeChange==TypeChange.Change)
            {
                object obj = ((DataRowView)Accounting.equipmentView.SelectedItems?[0]).Row["ImageID"];
                int id = Convert.ToInt32(obj.GetType() == typeof(DBNull) ? null : obj);
                if (id != 0)
                {
                    return Accounting.images[id];
                }
                return null;
            }
            return null;
        }

        private void AddSoftware()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = null;
                command = new SqlCommand($"AddLicenseSoftware", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@Name", softwareName.Text));
                command.Parameters.Add(new SqlParameter("@Price", Convert.ToSingle(softwareCost.Text)));
                command.Parameters.Add(new SqlParameter("@Count", Convert.ToInt32(softwareCount.Text)));
                command.Parameters.Add(new SqlParameter("@InvoiceNumber", softwareInvoice.Text));
                command.Parameters.Add(new SqlParameter("@Type", ((DataRowView)typeLicense.SelectedItem).Row[0]));
                command?.ExecuteNonQuery();
            }
        }

        private void AddOS()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = null;
                command = new SqlCommand($"AddOS", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@Name", softwareName.Text));
                command.Parameters.Add(new SqlParameter("@Price", Convert.ToSingle(softwareCost.Text)));
                command.Parameters.Add(new SqlParameter("@Count", Convert.ToInt32(softwareCount.Text)));
                command.Parameters.Add(new SqlParameter("@InvoiceNumber", softwareInvoice.Text));
                command?.ExecuteNonQuery();
            }
        }

        private void UpdateSoftware()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = null;
                command = new SqlCommand($"UpdateLicenseSoftware", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@ID", Accounting.SoftwareID));
                command.Parameters.Add(new SqlParameter("@Name", softwareName.Text));
                command.Parameters.Add(new SqlParameter("@Price", Convert.ToSingle(softwareCost.Text)));
                command.Parameters.Add(new SqlParameter("@Count", Convert.ToInt32(softwareCount.Text)));
                command.Parameters.Add(new SqlParameter("@InvoiceNumber", softwareInvoice.Text));
                command.Parameters.Add(new SqlParameter("@Type", ((DataRowView)typeLicense.SelectedItem).Row[0]));
                command?.ExecuteNonQuery();
            }
        }

        private void UpdateOS()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = null;
                command = new SqlCommand($"UpdateOS", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@ID", Accounting.SoftwareID));
                command.Parameters.Add(new SqlParameter("@Name", softwareName.Text));
                command.Parameters.Add(new SqlParameter("@Price", Convert.ToSingle(softwareCost.Text)));
                command.Parameters.Add(new SqlParameter("@Count", Convert.ToInt32(softwareCount.Text)));
                command.Parameters.Add(new SqlParameter("@InvoiceNumber", softwareInvoice.Text));
                command?.ExecuteNonQuery();
            }
        }

        private void GetLicenseSoftware(Software software, int id)
        {
            String commandString;
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = $"SELECT * FROM dbo.GetLicenseSoftwareByID({id})";
                reader = new SqlCommand(commandString, connection).ExecuteReader();
                if (reader.HasRows)
                {
                    if (reader.Read())
                    {
                        software = new LicenseSoftware()
                        {
                            Name = reader["Name"].ToString(),
                            Cost = Convert.ToSingle(reader["Cost"]),
                            Count = Convert.ToInt32(reader["Count"]),
                            InvoiceNumber = reader["InvoiceNumber"].ToString(),
                            Type = Convert.ToInt32(reader["Type"]),
                        };

                        softwareName.Text = software.Name;
                        softwareCost.Text = software.Cost.ToString();
                        softwareCount.Text = software.Count.ToString();
                        softwareInvoice.Text = software.InvoiceNumber;
                        foreach (object obj in typeLicense.ItemsSource)
                        {
                            DataRowView row;
                            row = (obj as DataRowView);
                            if (Convert.ToUInt32(row.Row[0]) == (software as LicenseSoftware).Type)
                            {
                                typeLicense.SelectedItem = row;
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void GetOS(Software software, int id)
        {
            String commandString;
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = $"SELECT * FROM dbo.GetOSByID({id})";
                reader = new SqlCommand(commandString, connection).ExecuteReader();
                if (reader.HasRows)
                {
                    if (reader.Read())
                    {
                        software = new OS()
                        {
                            Name = reader["Name"].ToString(),
                            Cost = Convert.ToSingle(reader["Cost"]),
                            Count = Convert.ToInt32(reader["Count"]),
                            InvoiceNumber = reader["InvoiceNumber"].ToString(),
                        };

                        softwareName.Text = software.Name;
                        softwareCost.Text = software.Cost.ToString();
                        softwareCount.Text = software.Count.ToString();
                        softwareInvoice.Text = software.InvoiceNumber;
                    }
                }
            }
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

        private void CloseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            switch (Accounting.NowView)
            {
                case View.Equipment:
                    //changeEquipmentPopup.IsOpen = false;
                    Accounting.IsPreOpenEquipmentPopup = false;
                    Accounting.UpdateEquipmentData();
                    Accounting.UpdateImages();
                    Accounting.ChangeEquipmentView();
                    break;
                case View.Software:
                    //changeSoftwarePopup.IsOpen = false;
                    Accounting.IsPreOpenSoftwarePopup = false;
                    Accounting.UpdateSoftwareData();
                    Accounting.ChangeSoftwareView();
                    break;
            }
            Accounting.viewGrid.IsEnabled = true;
            Accounting.menu.IsEnabled = true;
            Close();
        }

        private void Cpu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRow row = ((DataRowView)cpu.SelectedItem)?.Row;
            frequency.Text = row?[1].ToString();
            maxFrequency.Text = row?[2].ToString();
            cores.Text = row?[3].ToString();
        }

        private void ImageLoad_Click(object sender, RoutedEventArgs e)
        {
            //PreClose();
            System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();
            dialog.Filter = "Image Files(*.BMP;*.PNG;*.JPG;*.GIF)|*.BMP;*.PNG;*.JPG;*.GIF";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                return;
            //PostClose();
            imageFilename.Text = dialog.FileName;
        }

        private void PreClose()
        {
            Hide();
            switch (Accounting.NowView)
            {
                case View.Equipment:
                    Accounting.IsPreOpenEquipmentPopup = true;
                    break;
                case View.Software:
                    Accounting.IsPreOpenSoftwarePopup = true;
                    break;
            }
            //if (Accounting.changeEquipmentPopup.IsOpen)
            //{
            //    IsPreOpenEquipmentPopup = true;
            //    changeEquipmentPopup.IsOpen = false;
            //}
            //if (changeSoftwarePopup.IsOpen)
            //{
            //    IsPreOpenSoftwarePopup = true;
            //    changeSoftwarePopup.IsOpen = false;
            //}
        }

        private void PostClose()
        {
            Show();
            //ShowDialog();
            //if (IsPreOpenEquipmentPopup)
            //{
            //    changeEquipmentPopup.Height = Height - 200;
            //    changeEquipmentPopup.Width = Width - 400;
            //    changeEquipmentPopup.IsOpen = true;
            //    IsPreOpenEquipmentPopup = false;
            //}
            //if (IsPreOpenSoftwarePopup)
            //{
            //    changeSoftwarePopup.Height = Height - 200;
            //    changeSoftwarePopup.Width = Width - 400;
            //    changeSoftwarePopup.IsOpen = true;
            //    IsPreOpenSoftwarePopup = false;
            //}
        }

        private void SaveChangesForSoftware(object sender, RoutedEventArgs e)
        {
            SaveOrUpdateSoftwareDB();
            Accounting.UpdateSoftwareData();
            Accounting.ChangeSoftwareView();
        }

        private void SaveChanges(object sender, RoutedEventArgs e)
        {
            SaveOrUpdateEquipmentDB();
            Accounting.UpdateEquipmentData();
            Accounting.UpdateImages();
            Accounting.ChangeEquipmentView();
        }

        private void MoveWindow(object sender, MouseButtonEventArgs e)
        {
            //DragEnter += (obj, args) => Accounting.DragMove();
            DragMove();

            //Widhth 900
            //Height 500
            //Accounting.Width 1366
            //Accounting.Height 768

            //Left 510
            //Top 290
            //Accounting.Left 277
            //Accounting.Top 156

            //ΔWidth 466
            //ΔHeight 268
            //ΔLeft 233
            //ΔTop 124

            //double w = SystemParameters.PrimaryScreenWidth;//1920
            //double h = SystemParameters.PrimaryScreenHeight;//1080
            if (Accounting != null)
            {
                Accounting.Left = Left - (Accounting.Width - Width) / 2;
                Accounting.Top = Top - (Accounting.Height - Height) / 2;
            }
            //Accounting.DragMove();
        }

        private void Window_DragOver(object sender, DragEventArgs e)
        {
            if (Accounting != null)
            {
                Accounting.Left = Left - (Accounting.Width - Width) / 2;
                Accounting.Top = Top - (Accounting.Height - Height) / 2;
            }
        }
    }
}
