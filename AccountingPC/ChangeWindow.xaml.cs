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

        public static string ConnectionString => connectionString;
        public static readonly RoutedCommand CloseCommand = new RoutedUICommand(
            "Close", "CloseCommand", typeof(AccountingPCWindow),
            new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.Escape) }));
        private AccountingPCWindow accounting;

        private bool IsChangeAnalog { get; set; }
        public AccountingPCWindow Accounting { get => accounting; private set => accounting = value; }
        //public DataSet AspectRatioDataSet { get => aspectRatioDataSet; set => aspectRatioDataSet = value; }
        //public DataSet CpuDataSet { get => cpuDataSet; set => cpuDataSet = value; }
        //public DataSet OsDataSet { get => osDataSet; set => osDataSet = value; }
        //public DataSet ScreenInstalledDataSet { get => screenInstalledDataSet; set => screenInstalledDataSet = value; }
        //public DataSet FrequencyDataSet { get => frequencyDataSet; set => frequencyDataSet = value; }
        //public DataSet LocationDataSet { get => locationDataSet; set => locationDataSet = value; }
        //public DataSet MatrixTechnologyDataSet { get => matrixTechnologyDataSet; set => matrixTechnologyDataSet = value; }
        //public DataSet PaperSizeDataSet { get => paperSizeDataSet; set => paperSizeDataSet = value; }
        //public DataSet ProjectorTechnologyDataSet { get => projectorTechnologyDataSet; set => projectorTechnologyDataSet = value; }
        //public DataSet ResolutionDataSet { get => resolutionDataSet; set => resolutionDataSet = value; }
        //public DataSet TypeNetworkSwitchDataSet { get => typeNetworkSwitchDataSet; set => typeNetworkSwitchDataSet = value; }
        //public DataSet TypeNotebookDataSet { get => typeNotebookDataSet; set => typeNotebookDataSet = value; }
        //public DataSet TypePrinterDataSet { get => typePrinterDataSet; set => typePrinterDataSet = value; }
        //public DataSet VideoConnectorsDataSet { get => videoConnectorsDataSet; set => videoConnectorsDataSet = value; }
        //public DataSet WifiFrequencyDataSet { get => wifiFrequencyDataSet; set => wifiFrequencyDataSet = value; }
        //public DataSet NameDataSet { get => nameDataSet; set => nameDataSet = value; }
        //public DataSet MotherboardDataSet { get => motherboardDataSet; set => motherboardDataSet = value; }
        //public DataSet VCardDataSet { get => vCardDataSet; set => vCardDataSet = value; }
        //public DataSet TypeLicenseDataSet { get => typeLicenseDataSet; set => typeLicenseDataSet = value; }
        //public DataSet InvoiceDataSet { get => invoiceDataSet; set => invoiceDataSet = value; }

        public DataSet DefaultDataSet { get; set; }

        internal List<ListBoxItem> videoConnectorsItems;

        private Binding invNBinding;
        //private DataSet aspectRatioDataSet;
        //private DataSet cpuDataSet;
        //private DataSet osDataSet;
        //private DataSet screenInstalledDataSet;
        //private DataSet frequencyDataSet;
        //private DataSet locationDataSet;
        //private DataSet matrixTechnologyDataSet;
        //private DataSet paperSizeDataSet;
        //private DataSet projectorTechnologyDataSet;
        //private DataSet resolutionDataSet;
        //private DataSet typeNetworkSwitchDataSet;
        //private DataSet typeNotebookDataSet;
        //private DataSet typePrinterDataSet;
        //private DataSet videoConnectorsDataSet;
        //private DataSet wifiFrequencyDataSet;
        //private DataSet nameDataSet;
        //private DataSet motherboardDataSet;
        //private DataSet vCardDataSet;
        //private DataSet typeLicenseDataSet;
        //private DataSet invoiceDataSet;

        public ChangeWindow(AccountingPCWindow window)
        {
            DefaultDataSet = new DataSet("Default DataSet");
            DefaultDataSet.Tables.Add("AspectRatio");
            DefaultDataSet.Tables.Add("CPU");
            DefaultDataSet.Tables.Add("OS");
            DefaultDataSet.Tables.Add("ScreenInstalled");
            DefaultDataSet.Tables.Add("Frequency");
            DefaultDataSet.Tables.Add("Location");
            DefaultDataSet.Tables.Add("MatrixTechnology");
            DefaultDataSet.Tables.Add("PaperSize");
            DefaultDataSet.Tables.Add("ProjectorTechnology");
            DefaultDataSet.Tables.Add("Resolution");
            DefaultDataSet.Tables.Add("TypeNetworkSwitch");
            DefaultDataSet.Tables.Add("TypeNotebook");
            DefaultDataSet.Tables.Add("TypePrinter");
            DefaultDataSet.Tables.Add("VideoConnectors");
            DefaultDataSet.Tables.Add("WifiFrequency");
            DefaultDataSet.Tables.Add("Name");
            DefaultDataSet.Tables.Add("Motherboard");
            DefaultDataSet.Tables.Add("VCard");
            DefaultDataSet.Tables.Add("TypeLicense");
            DefaultDataSet.Tables.Add("Invoice");

            InitializeComponent();
            Owner = window;
            Accounting = window;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Accounting.Hide();
            Accounting.IsHitTestVisible = false;
            ChangeView();
        }

        private void ChangeView()
        {
            switch (Accounting.NowView)
            {
                case View.Equipment:
                    //Width = 1100;
                    //Height = 700;
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
                    //Width = 600;
                    //Height = 300;
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
            string commandString;
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
            string commandString;
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
            string commandString;
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
            string commandString;
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
            string commandString;
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
            string commandString;
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
            string commandString;
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
            string commandString;
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
            string commandString;
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

        public static string GetVideoConnectors(int value)
        {
            List<string> arr = GetListVideoConnectors(value);
            string res = string.Empty;
            for (int i = 0; i < arr.Count; i++)
            {
                res += $"{arr[i]}";
                if (i < arr.Count - 1)
                {
                    res += ", ";
                }
            }
            return res;
        }

        private static List<string> GetListVideoConnectors(int value)
        {
            List<string> arr = new List<string>
            {
                Capacity = 32
            };
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlDataReader reader = new SqlCommand("Select * from dbo.GetAllVideoConnector() Order by value desc", connection).ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int connectorValue = Convert.ToInt32(reader["Value"]);
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

        private int GetValueVideoConnectors(ListBox list)
        {
            int value = 0;
            foreach (object obj in list.SelectedItems)
            {
                //foreach (DataRowView row in VideoConnectorsDataSet.Tables[0].DefaultView)
                foreach (DataRowView row in DefaultDataSet.Tables["VideoConnectors"].DefaultView) 
                {
                    string s = (obj as ListBoxItem).Content.ToString();
                    if (row.Row[1].ToString() == s)
                    {
                        value += Convert.ToInt32(row.Row[2]);
                    }
                }
            }
            return value;
        }

        private void DisabledRepeatInvN_Checked()
        {
            invNBinding = new Binding
            {
                Path = new PropertyPath("InventoryNumber")
            };
            invNBinding.ValidationRules.Clear();
            invNBinding.ValidationRules.Add(new DataErrorValidationRule());
            invNBinding.ValidationRules.Add(new InventoryNumberValidationRule());
            inventoryNumber.SetBinding(TextBox.TextProperty, invNBinding);
        }

        private void DisabledRepeatInvN_Unchecked()
        {
            invNBinding = new Binding
            {
                Path = new PropertyPath("InventoryNumber")
            };
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
            //AspectRatioDataSet = new DataSet();
            //new SqlDataAdapter($"SELECT * FROM dbo.GetAllAspectRatio()", ConnectionString).Fill(AspectRatioDataSet);
            //aspectRatio.ItemsSource = AspectRatioDataSet.Tables[0].DefaultView;
            new SqlDataAdapter($"SELECT * FROM dbo.GetAllAspectRatio()", ConnectionString).Fill(DefaultDataSet, "AspectRatio");
            aspectRatio.ItemsSource = DefaultDataSet.Tables["AspectRatio"].DefaultView;
            aspectRatio.DisplayMemberPath = "Name";
        }

        private void UpdateSourceForOS()
        {
            //OsDataSet = new DataSet();
            //new SqlDataAdapter($"SELECT * FROM dbo.GetAllOS()", ConnectionString).Fill(OsDataSet);
            //os.ItemsSource = OsDataSet.Tables[0].DefaultView;
            new SqlDataAdapter($"SELECT * FROM dbo.GetAllOS()", ConnectionString).Fill(DefaultDataSet, "OS");
            os.ItemsSource = DefaultDataSet.Tables["OS"].DefaultView;
            os.DisplayMemberPath = "Наименование";
        }

        private void UpdateSourceForScreenInstalled()
        {
            //ScreenInstalledDataSet = new DataSet();
            //new SqlDataAdapter($"SELECT * FROM dbo.GetAllScreenInstalled()", ConnectionString).Fill(ScreenInstalledDataSet);
            //screenInstalled.ItemsSource = ScreenInstalledDataSet.Tables[0].DefaultView;
            new SqlDataAdapter($"SELECT * FROM dbo.GetAllScreenInstalled()", ConnectionString).Fill(DefaultDataSet, "ScreenInstalled");
            screenInstalled.ItemsSource = DefaultDataSet.Tables["ScreenInstalled"].DefaultView;
            screenInstalled.DisplayMemberPath = "Name";
        }

        private void UpdateSourceForName()
        {
            SqlDataAdapter NameDataAdapter = null;
            switch (Accounting.TypeDevice)
            {
                case TypeDevice.InteractiveWhiteboard:
                    NameDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllBoardName()", ConnectionString);
                    break;
                case TypeDevice.Monitor:
                    NameDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllMonitorName()", ConnectionString);
                    break;
                case TypeDevice.NetworkSwitch:
                    NameDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllNetworkSwitchName()", ConnectionString);
                    break;
                case TypeDevice.Notebook:
                    NameDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllNotebookName()", ConnectionString);
                    break;
                case TypeDevice.OtherEquipment:
                    NameDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllOtherEquipmentName()", ConnectionString);
                    break;
                case TypeDevice.PC:
                    NameDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllPCName()", ConnectionString);
                    break;
                case TypeDevice.PrinterScanner:
                    NameDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllPrinterScannerName()", ConnectionString);
                    break;
                case TypeDevice.Projector:
                    NameDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllProjectorName()", ConnectionString);
                    break;
                case TypeDevice.ProjectorScreen:
                    NameDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.GetAllProjectorScreenName()", ConnectionString);
                    break;
            }

            //NameDataSet = new DataSet();
            //NameDataAdapter.Fill(NameDataSet);
            //name.ItemsSource = NameDataSet.Tables[0].DefaultView;
            NameDataAdapter?.Fill(DefaultDataSet, "Name");
            name.ItemsSource = DefaultDataSet.Tables["Name"].DefaultView;
            name.DisplayMemberPath = "Name";
        }

        private void UpdateSourceForLocation()
        {
            SqlDataAdapter LocationDataAdapter = null;
            switch (Accounting.TypeDevice)
            {
                case TypeDevice.InteractiveWhiteboard:
                    LocationDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.[GetAllCanUsedLocationByTypeDeviceID](4)", ConnectionString);
                    break;
                case TypeDevice.Monitor:
                    LocationDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.[GetAllCanUsedLocationByTypeDeviceID](6)", ConnectionString);
                    break;
                case TypeDevice.NetworkSwitch:
                    LocationDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.[GetAllCanUsedLocationByTypeDeviceID](5)", ConnectionString);
                    break;
                case TypeDevice.Notebook:
                    LocationDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.[GetAllCanUsedLocationByTypeDeviceID](2)", ConnectionString);
                    break;
                case TypeDevice.OtherEquipment:
                    LocationDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.[GetAllCanUsedLocationByTypeDeviceID](9)", ConnectionString);
                    break;
                case TypeDevice.PC:
                    LocationDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.[GetAllCanUsedLocationByTypeDeviceID](1)", ConnectionString);
                    break;
                case TypeDevice.PrinterScanner:
                    LocationDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.[GetAllCanUsedLocationByTypeDeviceID](3)", ConnectionString);
                    break;
                case TypeDevice.Projector:
                    LocationDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.[GetAllCanUsedLocationByTypeDeviceID](7)", ConnectionString);
                    break;
                case TypeDevice.ProjectorScreen:
                    LocationDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.[GetAllCanUsedLocationByTypeDeviceID](8)", ConnectionString);
                    break;
            }

            //LocationDataSet = new DataSet();
            //LocationDataAdapter.Fill(LocationDataSet);
            //location.ItemsSource = LocationDataSet.Tables[0].DefaultView;
            LocationDataAdapter?.Fill(DefaultDataSet, "Location");
            location.ItemsSource = DefaultDataSet.Tables["Location"].DefaultView;
            location.DisplayMemberPath = "Place";
        }

        private void UpdateSourceForCPU()
        {
            switch (Accounting.TypeDevice)
            {
                case TypeDevice.Notebook:
                    //CpuDataSet = new DataSet();
                    //new SqlDataAdapter($"SELECT * FROM dbo.GetAllNotebookCPU()", ConnectionString).Fill(CpuDataSet);
                    //cpu.ItemsSource = CpuDataSet.Tables[0].DefaultView;
                    new SqlDataAdapter($"SELECT * FROM dbo.GetAllNotebookCPU()", ConnectionString).Fill(DefaultDataSet, "CPU");
                    cpu.ItemsSource = DefaultDataSet.Tables["CPU"].DefaultView;
                    cpu.DisplayMemberPath = "CPUModel";
                    break;
                case TypeDevice.PC:
                    //CpuDataSet = new DataSet();
                    //new SqlDataAdapter($"SELECT * FROM dbo.GetAllPCCPU()", ConnectionString).Fill(CpuDataSet);
                    //cpu.ItemsSource = CpuDataSet.Tables[0].DefaultView;
                    new SqlDataAdapter($"SELECT * FROM dbo.GetAllPCCPU()", ConnectionString).Fill(DefaultDataSet, "CPU");
                    cpu.ItemsSource = DefaultDataSet.Tables["CPU"].DefaultView;
                    cpu.DisplayMemberPath = "CPUModel";
                    break;
            }
        }

        private void UpdateSourceForVideoCard()
        {
            switch (Accounting.TypeDevice)
            {
                case TypeDevice.Notebook:
                    //VCardDataSet = new DataSet();
                    //new SqlDataAdapter($"SELECT * FROM dbo.GetAllNotebookvCard()", ConnectionString).Fill(VCardDataSet);
                    //vCard.ItemsSource = VCardDataSet.Tables[0].DefaultView;
                    new SqlDataAdapter($"SELECT * FROM dbo.GetAllNotebookvCard()", ConnectionString).Fill(DefaultDataSet, "VCard");
                    vCard.ItemsSource = DefaultDataSet.Tables["VCard"].DefaultView;
                    vCard.DisplayMemberPath = "VideoCard";
                    break;
                case TypeDevice.PC:
                    //VCardDataSet = new DataSet();
                    //new SqlDataAdapter($"SELECT * FROM dbo.GetAllPCvCard()", ConnectionString).Fill(VCardDataSet);
                    //vCard.ItemsSource = VCardDataSet.Tables[0].DefaultView;
                    new SqlDataAdapter($"SELECT * FROM dbo.GetAllPCvCard()", ConnectionString).Fill(DefaultDataSet, "VCard");
                    vCard.ItemsSource = DefaultDataSet.Tables["VCard"].DefaultView;
                    vCard.DisplayMemberPath = "VideoCard";
                    break;
            }
        }

        private void UpdateSourceForType()
        {
            switch (Accounting.TypeDevice)
            {
                case TypeDevice.NetworkSwitch:
                    //TypeNetworkSwitchDataSet = new DataSet();
                    //new SqlDataAdapter($"SELECT * FROM dbo.GetAllTypeNetworkSwitch()", ConnectionString).Fill(TypeNetworkSwitchDataSet);
                    //type.ItemsSource = TypeNetworkSwitchDataSet.Tables[0].DefaultView;
                    new SqlDataAdapter($"SELECT * FROM dbo.GetAllTypeNetworkSwitch()", ConnectionString).Fill(DefaultDataSet, "TypeNetworkSwitch");
                    type.ItemsSource = DefaultDataSet.Tables["TypeNetworkSwitch"].DefaultView;
                    type.DisplayMemberPath = "Name";
                    break;
                case TypeDevice.Notebook:
                    //TypeNotebookDataSet = new DataSet();
                    //new SqlDataAdapter($"SELECT * FROM dbo.GetAllTypeNotebook()", ConnectionString).Fill(TypeNotebookDataSet);
                    //type.ItemsSource = TypeNotebookDataSet.Tables[0].DefaultView;
                    new SqlDataAdapter($"SELECT * FROM dbo.GetAllTypeNotebook()", ConnectionString).Fill(DefaultDataSet, "TypeNotebook");
                    type.ItemsSource = DefaultDataSet.Tables["TypeNotebook"].DefaultView;
                    type.DisplayMemberPath = "Name";
                    break;
                case TypeDevice.PrinterScanner:
                    //TypePrinterDataSet = new DataSet();
                    new SqlDataAdapter($"SELECT * FROM dbo.GetAllTypePrinter()", ConnectionString).Fill(DefaultDataSet, "TypePrinter");
                    type.ItemsSource = DefaultDataSet.Tables["TypePrinter"].DefaultView;
                    type.DisplayMemberPath = "Name";
                    break;
            }
        }

        private void UpdateSourceForFrequency()
        {
            //FrequencyDataSet = new DataSet();
            //new SqlDataAdapter($"SELECT * FROM dbo.GetAllFrequency()", ConnectionString).Fill(FrequencyDataSet);
            //screenFrequency.ItemsSource = FrequencyDataSet.Tables[0].DefaultView;
            new SqlDataAdapter($"SELECT * FROM dbo.GetAllFrequency()", ConnectionString).Fill(DefaultDataSet, "Frequency");
            screenFrequency.ItemsSource = DefaultDataSet.Tables["Frequency"].DefaultView;
            screenFrequency.DisplayMemberPath = "Name";
        }

        private void UpdateSourceForMatrixTechology()
        {
            //MatrixTechnologyDataSet = new DataSet();
            //new SqlDataAdapter($"SELECT * FROM dbo.GetAllMatrixTechnology()", ConnectionString).Fill(MatrixTechnologyDataSet);
            //matrixTechnology.ItemsSource = MatrixTechnologyDataSet.Tables[0].DefaultView;
            new SqlDataAdapter($"SELECT * FROM dbo.GetAllMatrixTechnology()", ConnectionString).Fill(DefaultDataSet, "MatrixTechnology");
            matrixTechnology.ItemsSource = DefaultDataSet.Tables["MatrixTechnology"].DefaultView;
            matrixTechnology.DisplayMemberPath = "Name";
        }

        private void UpdateSourceForPaperSize()
        {
            //PaperSizeDataSet = new DataSet();
            //new SqlDataAdapter($"SELECT * FROM dbo.GetAllPaperSize()", ConnectionString).Fill(PaperSizeDataSet);
            //paperSize.ItemsSource = PaperSizeDataSet.Tables[0].DefaultView;
            new SqlDataAdapter($"SELECT * FROM dbo.GetAllPaperSize()", ConnectionString).Fill(DefaultDataSet, "PaperSize");
            paperSize.ItemsSource = DefaultDataSet.Tables["PaperSize"].DefaultView;
            paperSize.DisplayMemberPath = "Name";
        }

        private void UpdateSourceForProjectorTechnology()
        {
            //ProjectorTechnologyDataSet = new DataSet();
            //new SqlDataAdapter($"SELECT * FROM dbo.GetAllProjectorTechnology()", ConnectionString).Fill(ProjectorTechnologyDataSet);
            //projectorTechnology.ItemsSource = ProjectorTechnologyDataSet.Tables[0].DefaultView;
            new SqlDataAdapter($"SELECT * FROM dbo.GetAllProjectorTechnology()", ConnectionString).Fill(DefaultDataSet, "ProjectorTechnology");
            projectorTechnology.ItemsSource = DefaultDataSet.Tables["ProjectorTechnology"].DefaultView;
            projectorTechnology.DisplayMemberPath = "Name";
        }

        private void UpdateSourceForResolution()
        {
            //ResolutionDataSet = new DataSet();
            //new SqlDataAdapter($"SELECT * FROM dbo.GetAllResolution()", ConnectionString).Fill(ResolutionDataSet);
            //resolution.ItemsSource = ResolutionDataSet.Tables[0].DefaultView;
            new SqlDataAdapter($"SELECT * FROM dbo.GetAllResolution()", ConnectionString).Fill(DefaultDataSet, "Resolution");
            resolution.ItemsSource = DefaultDataSet.Tables["Resolution"].DefaultView;
            resolution.DisplayMemberPath = "Name";
        }

        private void UpdateSourceForVideoConnectors()
        {
            //VideoConnectorsDataSet = new DataSet();
            //new SqlDataAdapter($"SELECT * FROM dbo.GetAllVideoConnector()", ConnectionString).Fill(VideoConnectorsDataSet);
            new SqlDataAdapter($"SELECT * FROM dbo.GetAllVideoConnector()", ConnectionString).Fill(DefaultDataSet, "VideoConnectors");
            videoConnectorsItems = new List<ListBoxItem>
            {
                Capacity = 32
            };
            //foreach (DataRowView row in VideoConnectorsDataSet.Tables[0].DefaultView)
            foreach (DataRowView row in DefaultDataSet.Tables["VideoConnectors"].DefaultView)
            {
                videoConnectorsItems.Add(new ListBoxItem() { Content = row.Row[1].ToString() });
            }

            vConnectors.ItemsSource = videoConnectorsItems;
        }

        private void UpdateSourceForWifiFrequency()
        {
            //WifiFrequencyDataSet = new DataSet();
            //new SqlDataAdapter($"SELECT * FROM dbo.GetAllWiFiFrequency()", ConnectionString).Fill(WifiFrequencyDataSet);
            //wifiFrequency.ItemsSource = WifiFrequencyDataSet.Tables[0].DefaultView;
            new SqlDataAdapter($"SELECT * FROM dbo.GetAllWiFiFrequency()", ConnectionString).Fill(DefaultDataSet, "WifiFrequency");
            wifiFrequency.ItemsSource = DefaultDataSet.Tables["WifiFrequency"].DefaultView;
            wifiFrequency.DisplayMemberPath = "Name";
        }

        private void UpdateSourceForInvoice()
        {
            //InvoiceDataSet = new DataSet();
            //new SqlDataAdapter($"SELECT [Number] FROM dbo.GetAllInvoice()", ConnectionString).Fill(InvoiceDataSet);
            //invoice.ItemsSource = InvoiceDataSet.Tables[0].DefaultView;
            //softwareInvoice.ItemsSource = InvoiceDataSet.Tables[0].DefaultView;
            new SqlDataAdapter($"SELECT [Number] FROM dbo.GetAllInvoice()", ConnectionString).Fill(DefaultDataSet, "Invoice");
            invoice.ItemsSource = DefaultDataSet.Tables["Invoice"].DefaultView;
            softwareInvoice.ItemsSource = DefaultDataSet.Tables["Invoice"].DefaultView;
            invoice.DisplayMemberPath = "Number";
            softwareInvoice.DisplayMemberPath = "Number";
        }

        private void UpdateSourceForMotherboard()
        {
            //MotherboardDataSet = new DataSet();
            //new SqlDataAdapter($"SELECT * FROM dbo.GetAllMotherboard()", ConnectionString).Fill(MotherboardDataSet);
            //motherboard.ItemsSource = MotherboardDataSet.Tables[0].DefaultView;
            new SqlDataAdapter($"SELECT * FROM dbo.GetAllMotherboard()", ConnectionString).Fill(DefaultDataSet, "Motherboard");
            motherboard.ItemsSource = DefaultDataSet.Tables["Motherboard"].DefaultView;
            motherboard.DisplayMemberPath = "Motherboard";
        }

        private void UpdateSourceForTypeLicense()
        {
            //TypeLicenseDataSet = new DataSet();
            //new SqlDataAdapter($"SELECT * FROM dbo.GetAllTypeSoftLicense()", ConnectionString).Fill(TypeLicenseDataSet);
            //typeLicense.ItemsSource = TypeLicenseDataSet.Tables[0].DefaultView;
            new SqlDataAdapter($"SELECT * FROM dbo.GetAllTypeSoftLicense()", ConnectionString).Fill(DefaultDataSet, "TypeLicense");
            typeLicense.ItemsSource = DefaultDataSet.Tables["TypeLicense"].DefaultView;
            typeLicense.DisplayMemberPath = "Name";
        }

        private void AddPC()
        {
            string commandString;
            SqlCommand command;
            int temp;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = "AddPC";
                command = new SqlCommand(commandString, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt32(inventoryNumber.Text)));
                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToSingle(cost.Text)));
                command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text == string.Empty ? null : invoice.Text));
                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?.Row?[0]));
                command.Parameters.Add(new SqlParameter("@CPU", cpu.Text == string.Empty ? null : cpu.Text));
                temp = Convert.ToInt32(cores.Text);
                if (temp > 0)
                {
                    command.Parameters.Add(new SqlParameter("@Cores", temp));
                }

                temp = Convert.ToInt32(frequency.Text);
                if (temp > 0)
                {
                    command.Parameters.Add(new SqlParameter("@Frequency", temp));
                }

                temp = Convert.ToInt32(maxFrequency.Text);
                if (temp > 0)
                {
                    command.Parameters.Add(new SqlParameter("@MaxFrequency", temp));
                }

                temp = Convert.ToInt32(ram.Text);
                if (temp > 0)
                {
                    command.Parameters.Add(new SqlParameter("@RAM", temp));
                }

                temp = Convert.ToInt32(ramFrequency.Text);
                if (temp > 0)
                {
                    command.Parameters.Add(new SqlParameter("@FrequencyRAM", temp));
                }

                temp = Convert.ToInt32(ssd.Text);
                if (temp > 0)
                {
                    command.Parameters.Add(new SqlParameter("@SSD", temp));
                }

                temp = Convert.ToInt32(hdd.Text);
                if (temp > 0)
                {
                    command.Parameters.Add(new SqlParameter("@HDD", temp));
                }

                command.Parameters.Add(new SqlParameter("@Video", vCard.Text == string.Empty ? null : vCard.Text));
                temp = Convert.ToInt32(videoram.Text);
                if (temp > 0)
                {
                    command.Parameters.Add(new SqlParameter("@VRAM", temp));
                }

                command.Parameters.Add(new SqlParameter("@OSID", ((DataRowView)os?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@MB", motherboard.Text));
                command.Parameters.Add(new SqlParameter("@VConnectors", GetValueVideoConnectors(vConnectors)));
                byte[] bytes = LoadImage(imageFilename.Text);
                if (bytes != null)
                {
                    command.Parameters.Add(new SqlParameter("@Image", bytes));
                }
                else
                {
                    command.Parameters.Add(new SqlParameter("@Image", SqlDbType.VarBinary) { SqlValue = null });
                }

                command.ExecuteNonQuery();
            }
        }

        private void AddNotebook()
        {
            string commandString;
            SqlCommand command;
            int temp;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = "AddNotebook";
                command = new SqlCommand(commandString, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt32(inventoryNumber.Text)));
                command.Parameters.Add(new SqlParameter("@Type", ((DataRowView)type?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToSingle(cost.Text)));
                command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text == string.Empty ? null : invoice.Text));
                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?.Row?[0]));
                command.Parameters.Add(new SqlParameter("@Diagonal", Convert.ToSingle(diagonal.Text)));
                command.Parameters.Add(new SqlParameter("@CPU", cpu.Text));
                temp = Convert.ToInt32(cores.Text);
                if (temp > 0)
                {
                    command.Parameters.Add(new SqlParameter("@Cores", temp));
                }

                temp = Convert.ToInt32(frequency.Text);
                if (temp > 0)
                {
                    command.Parameters.Add(new SqlParameter("@Frequency", temp));
                }

                temp = Convert.ToInt32(maxFrequency.Text);
                if (temp > 0)
                {
                    command.Parameters.Add(new SqlParameter("@MaxFrequency", temp));
                }

                temp = Convert.ToInt32(ram.Text);
                if (temp > 0)
                {
                    command.Parameters.Add(new SqlParameter("@RAM", temp));
                }

                temp = Convert.ToInt32(ramFrequency.Text);
                if (temp > 0)
                {
                    command.Parameters.Add(new SqlParameter("@FrequencyRAM", temp));
                }

                temp = Convert.ToInt32(ssd.Text);
                if (temp > 0)
                {
                    command.Parameters.Add(new SqlParameter("@SSD", temp));
                }

                temp = Convert.ToInt32(hdd.Text);
                if (temp > 0)
                {
                    command.Parameters.Add(new SqlParameter("@HDD", temp));
                }

                command.Parameters.Add(new SqlParameter("@Video", vCard.Text == string.Empty ? null : vCard.Text));
                temp = Convert.ToInt32(videoram.Text);
                if (temp > 0)
                {
                    command.Parameters.Add(new SqlParameter("@VRAM", temp));
                }

                command.Parameters.Add(new SqlParameter("@OSID", ((DataRowView)os?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@ResolutionID", ((DataRowView)resolution?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@FrequencyID", ((DataRowView)screenFrequency?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@MatrixID", ((DataRowView)matrixTechnology?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@VConnectors", GetValueVideoConnectors(vConnectors)));
                byte[] bytes = LoadImage(imageFilename.Text);
                if (bytes != null)
                {
                    command.Parameters.Add(new SqlParameter("@Image", bytes));
                }

                command.ExecuteNonQuery();
            }
        }

        private void AddMonitor()
        {
            string commandString;
            SqlCommand command;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = "AddMonitor";
                command = new SqlCommand(commandString, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt32(inventoryNumber.Text)));
                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToSingle(cost.Text)));
                command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text == string.Empty ? null : invoice.Text));
                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?.Row?[0]));
                command.Parameters.Add(new SqlParameter("@Diagonal", Convert.ToSingle(diagonal.Text)));
                command.Parameters.Add(new SqlParameter("@ResolutionID", ((DataRowView)resolution?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@FrequencyID", ((DataRowView)screenFrequency?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@MatrixID", ((DataRowView)matrixTechnology?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@VConnectors", GetValueVideoConnectors(vConnectors)));
                byte[] bytes = LoadImage(imageFilename.Text);
                if (bytes != null)
                {
                    command.Parameters.Add(new SqlParameter("@Image", bytes));
                }

                command.ExecuteNonQuery();
            }
        }

        private void AddNetworkSwitch()
        {
            string commandString;
            SqlCommand command;
            int temp;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = "AddNetworkSwitch";
                command = new SqlCommand(commandString, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt32(inventoryNumber.Text)));
                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToSingle(cost.Text)));
                command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text == string.Empty ? null : invoice.Text));
                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?.Row?[0]));
                temp = Convert.ToInt32(ports.Text);
                if (temp > 0)
                {
                    command.Parameters.Add(new SqlParameter("@Ports", temp));
                }

                command.Parameters.Add(new SqlParameter("@TypeID", ((DataRowView)type?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@Frequency", ((DataRowView)wifiFrequency?.SelectedItem)?[0]));
                byte[] bytes = LoadImage(imageFilename.Text);
                if (bytes != null)
                {
                    command.Parameters.Add(new SqlParameter("@Image", bytes));
                }

                command.ExecuteNonQuery();
            }
        }

        private void AddInteractiveWhiteboard()
        {
            string commandString;
            SqlCommand command;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = "AddInteractiveWhiteboard";
                command = new SqlCommand(commandString, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt32(inventoryNumber.Text)));
                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToSingle(cost.Text)));
                command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text == string.Empty ? null : invoice.Text));
                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?.Row?[0]));
                command.Parameters.Add(new SqlParameter("@Diagonal", Convert.ToSingle(diagonal.Text)));
                byte[] bytes = LoadImage(imageFilename.Text);
                if (bytes != null)
                {
                    command.Parameters.Add(new SqlParameter("@Image", bytes));
                }

                command.ExecuteNonQuery();
            }
        }

        private void AddPrinterScanner()
        {
            string commandString;
            SqlCommand command;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = "AddPrinterScanner";
                command = new SqlCommand(commandString, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt32(inventoryNumber.Text)));
                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToSingle(cost.Text)));
                command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text == string.Empty ? null : invoice.Text));
                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?.Row?[0]));
                command.Parameters.Add(new SqlParameter("@TypeID", ((DataRowView)type?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@PaperSizeID", ((DataRowView)paperSize?.SelectedItem)?[0]));
                byte[] bytes = LoadImage(imageFilename.Text);
                if (bytes != null)
                {
                    command.Parameters.Add(new SqlParameter("@Image", bytes));
                }

                command.ExecuteNonQuery();
            }
        }

        private void AddProjector()
        {
            string commandString;
            SqlCommand command;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = "AddProjector";
                command = new SqlCommand(commandString, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt32(inventoryNumber.Text)));
                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToSingle(cost.Text)));
                command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text == string.Empty ? null : invoice.Text));
                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?.Row?[0]));
                command.Parameters.Add(new SqlParameter("@TechnologyID", ((DataRowView)projectorTechnology?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@Diagonal", Convert.ToSingle(diagonal.Text)));
                command.Parameters.Add(new SqlParameter("@ResolutionID", ((DataRowView)resolution?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@VConnectors", GetValueVideoConnectors(vConnectors)));
                byte[] bytes = LoadImage(imageFilename.Text);
                if (bytes != null)
                {
                    command.Parameters.Add(new SqlParameter("@Image", bytes));
                }

                command.ExecuteNonQuery();
            }
        }

        private void AddProjectorScreen()
        {
            string commandString;
            SqlCommand command;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = "AddProjectorScreen";
                command = new SqlCommand(commandString, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt32(inventoryNumber.Text)));
                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToSingle(cost.Text)));
                command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text == string.Empty ? null : invoice.Text));
                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?.Row?[0]));
                command.Parameters.Add(new SqlParameter("@Diagonal", Convert.ToSingle(diagonal.Text)));
                command.Parameters.Add(new SqlParameter("@IsElectronic", Convert.ToBoolean(isEDrive.IsChecked)));
                command.Parameters.Add(new SqlParameter("@AspectRatioID", ((DataRowView)aspectRatio?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@InstalledID", ((DataRowView)screenInstalled?.SelectedItem)?[0]));
                byte[] bytes = LoadImage(imageFilename.Text);
                if (bytes != null)
                {
                    command.Parameters.Add(new SqlParameter("@Image", bytes));
                }

                command.ExecuteNonQuery();
            }
        }

        private void AddOtherEquipment()
        {
            string commandString;
            SqlCommand command;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = "AddOtherEquipment";
                command = new SqlCommand(commandString, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt32(inventoryNumber.Text)));
                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToSingle(cost.Text)));
                command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text == string.Empty ? null : invoice.Text));
                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?.Row?[0]));
                //command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?[0]));
                byte[] bytes = LoadImage(imageFilename.Text);
                if (bytes != null)
                {
                    command.Parameters.Add(new SqlParameter("@Image", bytes));
                }

                command.ExecuteNonQuery();
            }
        }

        private void UpdatePC()
        {
            string commandString;
            SqlCommand command;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = "UpdatePCByID";
                command = new SqlCommand(commandString, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
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
                {
                    command.Parameters.Add(new SqlParameter("@Image", bytes));
                }

                command.Parameters.Add(new SqlParameter("@IsChangeAnalog", IsChangeAnalog));
                command.ExecuteNonQuery();
            }
        }

        private void UpdateNotebook()
        {
            string commandString;
            SqlCommand command;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = "UpdateNotebookByID";
                command = new SqlCommand(commandString, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
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
                {
                    command.Parameters.Add(new SqlParameter("@Image", bytes));
                }

                command.Parameters.Add(new SqlParameter("@IsChangeAnalog", IsChangeAnalog));
                command.ExecuteNonQuery();
            }
        }

        private void UpdateMonitor()
        {
            string commandString;
            SqlCommand command;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = "UpdateMonitorByID";
                command = new SqlCommand(commandString, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
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
                {
                    command.Parameters.Add(new SqlParameter("@Image", bytes));
                }

                command.Parameters.Add(new SqlParameter("@IsChangeAnalog", IsChangeAnalog));
                command.ExecuteNonQuery();
            }
        }

        private void UpdateNetworkSwitch()
        {
            string commandString;
            SqlCommand command;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = "UpdateNetworkSwitchByID";
                command = new SqlCommand(commandString, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(new SqlParameter("@ID", Accounting.DeviceID));
                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt32(inventoryNumber.Text)));
                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToSingle(cost.Text)));
                command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text == string.Empty ? null : invoice.Text));
                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?.Row?[0]));
                //command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@NumberOfPorts", Convert.ToInt32(ports.Text)));
                command.Parameters.Add(new SqlParameter("@TypeID", ((DataRowView)type?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@Frequency", ((DataRowView)wifiFrequency?.SelectedItem)?[0]));
                byte[] bytes = LoadImage(imageFilename.Text);
                if (bytes != null)
                {
                    command.Parameters.Add(new SqlParameter("@Image", bytes));
                }

                command.Parameters.Add(new SqlParameter("@IsChangeAnalog", IsChangeAnalog));
                command.ExecuteNonQuery();
            }
        }

        private void UpdateInteractiveWhiteboard()
        {
            string commandString;
            SqlCommand command;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = "UpdateInteractiveWhiteboardByID";
                command = new SqlCommand(commandString, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(new SqlParameter("@ID", Accounting.DeviceID));
                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt32(inventoryNumber.Text)));
                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToSingle(cost.Text)));
                command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text == string.Empty ? null : invoice.Text));
                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?.Row?[0]));
                //command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@Diagonal", Convert.ToSingle(diagonal.Text)));
                byte[] bytes = LoadImage(imageFilename.Text);
                if (bytes != null)
                {
                    command.Parameters.Add(new SqlParameter("@Image", bytes));
                }

                command.Parameters.Add(new SqlParameter("@IsChangeAnalog", IsChangeAnalog));
                command.ExecuteNonQuery();
            }
        }

        private void UpdatePrinterScanner()
        {
            string commandString;
            SqlCommand command;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = "UpdatePrinterScannerByID";
                command = new SqlCommand(commandString, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(new SqlParameter("@ID", Accounting.DeviceID));
                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt32(inventoryNumber.Text)));
                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToSingle(cost.Text)));
                command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text == string.Empty ? null : invoice.Text));
                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?.Row?[0]));
                //command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@TypeID", ((DataRowView)type?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@PaperSizeID", ((DataRowView)paperSize?.SelectedItem)?[0]));
                byte[] bytes = LoadImage(imageFilename.Text);
                if (bytes != null)
                {
                    command.Parameters.Add(new SqlParameter("@Image", bytes));
                }

                command.Parameters.Add(new SqlParameter("@IsChangeAnalog", IsChangeAnalog));
                command.ExecuteNonQuery();
            }
        }

        private void UpdateProjector()
        {
            string commandString;
            SqlCommand command;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = "UpdateProjectorByID";
                command = new SqlCommand(commandString, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(new SqlParameter("@ID", Accounting.DeviceID));
                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt32(inventoryNumber.Text)));
                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToSingle(cost.Text)));
                command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text == string.Empty ? null : invoice.Text));
                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?.Row?[0]));
                //command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@TechnologyID", ((DataRowView)projectorTechnology?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@Diagonal", Convert.ToSingle(diagonal.Text)));
                command.Parameters.Add(new SqlParameter("@ResolutionID", ((DataRowView)resolution?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@VConnectors", GetValueVideoConnectors(vConnectors)));
                byte[] bytes = LoadImage(imageFilename.Text);
                if (bytes != null)
                {
                    command.Parameters.Add(new SqlParameter("@Image", bytes));
                }

                command.Parameters.Add(new SqlParameter("@IsChangeAnalog", IsChangeAnalog));
                command.ExecuteNonQuery();
            }
        }

        private void UpdateProjectorScreen()
        {
            string commandString;
            SqlCommand command;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = "UpdateProjectorScreenByID";
                command = new SqlCommand(commandString, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(new SqlParameter("@ID", Accounting.DeviceID));
                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt32(inventoryNumber.Text)));
                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToSingle(cost.Text)));
                command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text == string.Empty ? null : invoice.Text));
                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?.Row?[0]));
                //command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@Diagonal", Convert.ToSingle(diagonal.Text)));
                command.Parameters.Add(new SqlParameter("@IsEDrive", Convert.ToBoolean(isEDrive.IsChecked)));
                command.Parameters.Add(new SqlParameter("@AspectRatioID", ((DataRowView)aspectRatio?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@InstalledID", ((DataRowView)screenInstalled?.SelectedItem)?[0]));
                byte[] bytes = LoadImage(imageFilename.Text);
                if (bytes != null)
                {
                    command.Parameters.Add(new SqlParameter("@Image", bytes));
                }

                command.Parameters.Add(new SqlParameter("@IsChangeAnalog", IsChangeAnalog));
                command.ExecuteNonQuery();
            }
        }

        private void UpdateOtherEquipment()
        {
            string commandString;
            SqlCommand command;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = "UpdateOtherEquipmentByID";
                command = new SqlCommand(commandString, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(new SqlParameter("@ID", Accounting.DeviceID));
                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt32(inventoryNumber.Text)));
                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToSingle(cost.Text)));
                command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text == string.Empty ? null : invoice.Text));
                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?.Row?[0]));
                //command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?[0]));
                byte[] bytes = LoadImage(imageFilename.Text);
                if (bytes != null)
                {
                    command.Parameters.Add(new SqlParameter("@Image", bytes));
                }

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
            else if (Accounting.TypeChange == TypeChange.Change)
            {
                object obj = ((DataRowView)Accounting.equipmentView.SelectedItems?[0]).Row["ImageID"];
                int id = Convert.ToInt32(obj.GetType() == typeof(DBNull) ? null : obj);
                if (id != 0)
                {
                    return Accounting.Images[id];
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
                command = new SqlCommand($"AddLicenseSoftware", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
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
                command = new SqlCommand($"AddOS", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
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
                command = new SqlCommand($"UpdateLicenseSoftware", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
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
                command = new SqlCommand($"UpdateOS", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
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
            string commandString;
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
            string commandString;
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
            //Accounting.Show();
            Accounting.IsHitTestVisible = true;
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
            System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog
            {
                Filter = "Image Files(*.BMP;*.PNG;*.JPG;*.GIF)|*.BMP;*.PNG;*.JPG;*.GIF"
            };
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }
            //PostClose();
            imageFilename.Text = dialog.FileName;
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

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Accounting.menu.IsEnabled = false;
            Accounting.viewGrid.IsEnabled = false;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Accounting.menu.IsEnabled = true;
            Accounting.viewGrid.IsEnabled = true;
        }
    }
}
