using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace AccountingPC
{
    public partial class ChangeWindow : Window
    {
        public static readonly RoutedCommand CloseCommand = new RoutedUICommand(
            "Close", "CloseCommand", typeof(AccountingPCWindow),
            new InputGestureCollection(new InputGesture[] {new KeyGesture(Key.Escape)}));

        private Binding invNBinding;

        internal List<ListBoxItem> videoConnectorsItems;
        //internal ChangeRelation TargetChangeRelation { get; private set; }

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
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                var resourceName = assembly.GetManifestResourceNames().Single(s => s.EndsWith("icon.ico"));
                using (var stream = assembly.GetManifestResourceStream(resourceName))
                {
                    if (stream != null)
                        Icon = BitmapFrame.Create(stream);
                }
            }
            catch (Exception ex)
            {
                Clipboard.SetText(ex.ToString());
            }
            Owner = window;
            Accounting = window;
            DeviceID = Accounting.DeviceID;

            switch (Accounting.TypeDevice)
            {
                case TypeDevice.PC:
                    //TargetChangeRelation = new ChangeRelation()
                    //{
                    //    AddCommand = "AddPC",
                    //    UpdateCommand = "UpdatePC",
                    //    Columns = new List<ChangeColumnRelation>()
                    //    {
                    //        new ChangeColumnRelation()
                    //        {
                    //            ParameterName="@ID",
                    //            IsUpdateOnly = true,
                    //            Value = DeviceID,
                    //        },
                    //        new ChangeColumnRelation()
                    //        {
                    //            ParameterName="@Name",
                    //            Box=name,
                    //            Property=TextBox.TextProperty,
                    //        },
                    //    }
                    //}
                    break;
            }
        }

        public static string ConnectionString { get; } =
            ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        private bool IsChangeAnalog { get; set; }
        public AccountingPCWindow Accounting { get; private set; }
        public int DeviceID { get; set; }
        public DataSet DefaultDataSet { get; set; }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Accounting.IsHitTestVisible = false;
            switch (Accounting.TypeChange)
            {
                case TypeChange.Add:
                    saveButton.Content = "Добавить";
                    saveSoftButton.Content = "Добавить";
                    break;
                case TypeChange.Change:
                    saveButton.Content = "Изменить";
                    saveSoftButton.Content = "Изменить";
                    break;
            }

            try
            {

                ChangeView();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, exception.GetType().Name, MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ChangeView()
        {
            switch (Accounting.NowView)
            {
                case View.Equipment:
                    changeSoftwareGrid.Visibility = Visibility.Collapsed;
                    changeEquipmentGrid.Visibility = Visibility.Visible;
                    Initialize();
                    switch (Accounting.TypeChange)
                    {
                        case TypeChange.Change:
                            switch (Accounting.TypeDevice)
                            {
                                case TypeDevice.PC:
                                    GetPC(DeviceID);
                                    break;
                                case TypeDevice.Notebook:
                                    GetNotebook(DeviceID);
                                    break;
                                case TypeDevice.Monitor:
                                    GetMonitor(DeviceID);
                                    break;
                                case TypeDevice.Projector:
                                    GetProjector(DeviceID);
                                    break;
                                case TypeDevice.InteractiveWhiteboard:
                                    GetInteractiveWhiteboard(DeviceID);
                                    break;
                                case TypeDevice.ProjectorScreen:
                                    GetProjectorScreen(DeviceID);
                                    break;
                                case TypeDevice.PrinterScanner:
                                    GetPrinterScanner(DeviceID);
                                    break;
                                case TypeDevice.NetworkSwitch:
                                    GetNetworkSwitch(DeviceID);
                                    break;
                                case TypeDevice.OtherEquipment:
                                    GetOtherEquipment(DeviceID);
                                    break;
                            }

                            SetDeviceLocationAndInvoice();
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

                    break;
                case View.Software:
                    changeEquipmentGrid.Visibility = Visibility.Collapsed;
                    changeSoftwareGrid.Visibility = Visibility.Visible;
                    Initialize();
                    switch (Accounting.TypeChange)
                    {
                        case TypeChange.Add:
                            switch (Accounting.TypeSoft)
                            {
                                case TypeSoft.LicenseSoftware:
                                    soft = new LicenseSoftware();
                                    break;
                                case TypeSoft.OS:
                                    soft = new OS();
                                    break;
                            }

                            break;
                        case TypeChange.Change:
                            switch (Accounting.TypeSoft)
                            {
                                case TypeSoft.LicenseSoftware:
                                    GetLicenseSoftware(Accounting.SoftwareID);
                                    break;
                                case TypeSoft.OS:
                                    GetOS(Accounting.SoftwareID);
                                    break;
                            }

                            break;
                    }

                    break;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void GetPC(int id)
        {
            string commandString;
            SqlDataReader reader;
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = $"SELECT * FROM dbo.GetPCByID({id})";
                reader = new SqlCommand(commandString, connection).ExecuteReader();
                if (reader.HasRows)
                    if (reader.Read())
                    {
                        device = new PC
                        {
                            InventoryNumber = Convert.ToInt64(reader["InventoryNumber"]),
                            Name = reader["Name"].ToString(),
                            Cost = Convert.ToDecimal(!(reader["Cost"] is DBNull) ? reader["Cost"] : 0),
                            Motherboard = reader["MotherBoard"].ToString(),
                            CPU = reader["CPUModel"].ToString(),
                            Cores = Convert.ToUInt32(!(reader["NumberOfCores"] is DBNull)
                                ? reader["NumberOfCores"]
                                : 0),
                            Frequency = Convert.ToUInt32(!(reader["FrequencyProcessor"] is DBNull)
                                ? reader["FrequencyProcessor"]
                                : 0),
                            MaxFrequency = Convert.ToUInt32(!(reader["MaxFrequencyProcessor"] is DBNull)
                                ? reader["MaxFrequencyProcessor"]
                                : 0),
                            VCard = reader["VideoCard"].ToString(),
                            VideoRAM = Convert.ToUInt32(!(reader["VideoRAMGB"] is DBNull)
                                ? reader["VideoRAMGB"]
                                : 0),
                            RAM = Convert.ToUInt32(!(reader["RAMGB"] is DBNull) ? reader["RAMGB"] : 0),
                            FrequencyRAM = Convert.ToUInt32(!(reader["FrequencyRAM"] is DBNull)
                                ? reader["FrequencyRAM"]
                                : 0),
                            SSD = Convert.ToUInt32(!(reader["SSDCapacityGB"] is DBNull)
                                ? reader["SSDCapacityGB"]
                                : 0),
                            HDD = Convert.ToUInt32(!(reader["HDDCapacityGB"] is DBNull)
                                ? reader["HDDCapacityGB"]
                                : 0),
                            OSID = Convert.ToUInt32(!(reader["OSID"] is DBNull) ? reader["OSID"] : 0),
                            InvoiceNumber = reader["InvoiceNumber"].ToString(),
                            PlaceID = Convert.ToUInt32(!(reader["PlaceID"] is DBNull)
                                ? reader["PlaceID"]
                                : 0),
                            VideoConnectorsValue = Convert.ToInt32(!(reader["VideoConnectors"] is DBNull)
                                ? reader["VideoConnectors"]
                                : 0)
                        };
                        var pc = device as PC;
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
                        foreach (var obj in os.ItemsSource)
                        {
                            var row = obj as DataRowView;
                            if (Convert.ToUInt32(row.Row[0]) == Convert.ToUInt32(pc.OSID))
                            {
                                os.SelectedItem = row;
                                break;
                            }
                        }

                        foreach (var obj in location.ItemsSource)
                        {
                            DataRowView row;
                            row = obj as DataRowView;
                            if (Convert.ToUInt32(row.Row[0]) == Convert.ToUInt32(pc.PlaceID))
                            {
                                location.SelectedItem = row;
                                break;
                            }
                        }

                        foreach (var obj in vConnectors.Items)
                        {
                            var item = obj as ListBoxItem;
                            foreach (var connector in pc.VideoConnectors)
                                if (item.Content.ToString() == connector)
                                {
                                    item.IsSelected = true;
                                    break;
                                }
                        }
                    }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void GetNotebook(int id)
        {
            string commandString;
            SqlDataReader reader;
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = $"SELECT * FROM dbo.GetNotebookByID({id})";
                reader = new SqlCommand(commandString, connection).ExecuteReader();
                if (reader.HasRows)
                    if (reader.Read())
                    {
                        device = new Notebook
                        {
                            InventoryNumber = Convert.ToInt64(reader["InventoryNumber"]),
                            Name = reader["Name"].ToString(),
                            Cost = Convert.ToDecimal(!(reader["Cost"] is DBNull) ? reader["Cost"] : 0),
                            TypeID = Convert.ToUInt32(!(reader["TypeNotebookID"] is DBNull) ? reader["TypeNotebookID"] : 0),
                            CPU = reader["CPUModel"].ToString(),
                            Cores = Convert.ToUInt32(!(reader["NumberOfCores"] is DBNull)
                                ? reader["NumberOfCores"]
                                : 0),
                            Frequency = Convert.ToUInt32(!(reader["FrequencyProcessor"] is DBNull)
                                ? reader["FrequencyProcessor"]
                                : 0),
                            MaxFrequency = Convert.ToUInt32(!(reader["MaxFrequencyProcessor"] is DBNull)
                                ? reader["MaxFrequencyProcessor"]
                                : 0),
                            VCard = reader["VideoCard"].ToString(),
                            VideoRAM = Convert.ToUInt32(!(reader["VideoRAMGB"] is DBNull)
                                ? reader["VideoRAMGB"]
                                : 0),
                            RAM = Convert.ToUInt32(!(reader["RAMGB"] is DBNull) ? reader["RAMGB"] : 0),
                            FrequencyRAM = Convert.ToUInt32(!(reader["FrequencyRAM"] is DBNull)
                                ? reader["FrequencyRAM"]
                                : 0),
                            SSD = Convert.ToUInt32(!(reader["SSDCapacityGB"] is DBNull)
                                ? reader["SSDCapacityGB"]
                                : 0),
                            HDD = Convert.ToUInt32(!(reader["HDDCapacityGB"] is DBNull)
                                ? reader["HDDCapacityGB"]
                                : 0),
                            OSID = Convert.ToUInt32(!(reader["OSID"] is DBNull) ? reader["OSID"] : 0),
                            InvoiceNumber = reader["InvoiceNumber"].ToString(),
                            PlaceID = Convert.ToUInt32(!(reader["PlaceID"] is DBNull)
                                ? reader["PlaceID"]
                                : 0),
                            Diagonal = Convert.ToDecimal(!(reader["ScreenDiagonal"] is DBNull)
                                ? reader["ScreenDiagonal"]
                                : 0),
                            ResolutionID = Convert.ToUInt32(!(reader["ResolutionID"] is DBNull)
                                ? reader["ResolutionID"]
                                : 0),
                            FrequencyID = Convert.ToUInt32(!(reader["FrequencyID"] is DBNull)
                                ? reader["FrequencyID"]
                                : 0),
                            MatrixTechnologyID =
                                Convert.ToUInt32(!(reader["MatrixTechnologyID"] is DBNull)
                                    ? reader["MatrixTechnologyID"]
                                    : 0),
                            VideoConnectorsValue = Convert.ToInt32(!(reader["VideoConnectors"] is DBNull)
                                ? reader["VideoConnectors"]
                                : 0)
                        };
                        var notebook = device as Notebook;
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
                        foreach (var obj in type.ItemsSource)
                        {
                            var row = obj as DataRowView;
                            if (Convert.ToUInt32(row.Row[0]) == Convert.ToUInt32(notebook.TypeID))
                            {
                                type.SelectedItem = row;
                                break;
                            }
                        }

                        foreach (var obj in os.ItemsSource)
                        {
                            var row = obj as DataRowView;
                            if (Convert.ToUInt32(row.Row[0]) == Convert.ToUInt32(notebook.OSID))
                            {
                                os.SelectedItem = row;
                                break;
                            }
                        }

                        foreach (var obj in resolution.ItemsSource)
                        {
                            var row = obj as DataRowView;
                            if (Convert.ToUInt32(row.Row[0]) == Convert.ToUInt32(notebook.ResolutionID))
                            {
                                resolution.SelectedItem = row;
                                break;
                            }
                        }

                        foreach (var obj in screenFrequency.ItemsSource)
                        {
                            var row = obj as DataRowView;
                            if (Convert.ToUInt32(row.Row[0]) == Convert.ToUInt32(notebook.FrequencyID))
                            {
                                screenFrequency.SelectedItem = row;
                                break;
                            }
                        }

                        foreach (var obj in matrixTechnology.ItemsSource)
                        {
                            var row = obj as DataRowView;
                            if (Convert.ToUInt32(row.Row[0]) == Convert.ToUInt32(notebook.MatrixTechnologyID))
                            {
                                matrixTechnology.SelectedItem = row;
                                break;
                            }
                        }

                        foreach (var obj in vConnectors.Items)
                        {
                            var item = obj as ListBoxItem;
                            foreach (var connector in notebook.VideoConnectors)
                                if (item.Content.ToString() == connector)
                                {
                                    item.IsSelected = true;
                                    break;
                                }
                        }
                    }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void GetMonitor(int id)
        {
            string commandString;
            SqlDataReader reader;
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = $"SELECT * FROM dbo.GetMonitorByID({id})";
                reader = new SqlCommand(commandString, connection).ExecuteReader();
                if (reader.HasRows)
                    if (reader.Read())
                    {
                        device = new Monitor
                        {
                            InventoryNumber = Convert.ToInt64(reader["InventoryNumber"]),
                            Name = reader["Name"].ToString(),
                            Cost = Convert.ToDecimal(!(reader["Cost"] is DBNull) ? reader["Cost"] : 0),
                            InvoiceNumber = reader["InvoiceNumber"].ToString(),
                            PlaceID = Convert.ToUInt32(!(reader["PlaceID"] is DBNull)
                                ? reader["PlaceID"]
                                : 0),
                            Diagonal = Convert.ToDecimal(!(reader["ScreenDiagonal"] is DBNull)
                                ? reader["ScreenDiagonal"]
                                : 0),
                            ResolutionID = Convert.ToUInt32(!(reader["ResolutionID"] is DBNull)
                                ? reader["ResolutionID"]
                                : 0),
                            FrequencyID = Convert.ToUInt32(!(reader["FrequencyID"] is DBNull)
                                ? reader["FrequencyID"]
                                : 0),
                            MatrixTechnologyID =
                                Convert.ToUInt32(!(reader["MatrixTechnologyID"] is DBNull)
                                    ? reader["MatrixTechnologyID"]
                                    : 0),
                            VideoConnectorsValue = Convert.ToInt32(!(reader["VideoConnectors"] is DBNull)
                                ? reader["VideoConnectors"]
                                : 0)
                        };
                        if (device is Monitor monitor)
                        {
                            monitor.VideoConnectors = GetListVideoConnectors(monitor.VideoConnectorsValue);

                            inventoryNumber.Text = device.InventoryNumber.ToString();
                            name.Text = device.Name;
                            cost.Text = device.Cost.ToString();
                            diagonal.Text = monitor.Diagonal.ToString();
                            foreach (var obj in resolution.ItemsSource)
                            {
                                var row = obj as DataRowView;
                                if (Convert.ToUInt32(row.Row[0]) == Convert.ToUInt32(monitor.ResolutionID))
                                {
                                    resolution.SelectedItem = row;
                                    break;
                                }
                            }

                            foreach (var obj in screenFrequency.ItemsSource)
                            {
                                var row = obj as DataRowView;
                                if (Convert.ToUInt32(row.Row[0]) == Convert.ToUInt32(monitor.FrequencyID))
                                {
                                    screenFrequency.SelectedItem = row;
                                    break;
                                }
                            }

                            foreach (var obj in matrixTechnology.ItemsSource)
                            {
                                var row = obj as DataRowView;
                                if (Convert.ToUInt32(row.Row[0]) == Convert.ToUInt32(monitor.MatrixTechnologyID))
                                {
                                    matrixTechnology.SelectedItem = row;
                                    break;
                                }
                            }

                            foreach (var obj in vConnectors.Items)
                            {
                                var item = obj as ListBoxItem;
                                foreach (var connector in monitor.VideoConnectors)
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void GetProjector(int id)
        {
            string commandString;
            SqlDataReader reader;
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = $"SELECT * FROM dbo.GetProjectorByID({id})";
                reader = new SqlCommand(commandString, connection).ExecuteReader();
                if (reader.HasRows)
                    if (reader.Read())
                    {
                        device = new Projector
                        {
                            InventoryNumber = Convert.ToInt64(reader["InventoryNumber"]),
                            Name = reader["Name"].ToString(),
                            Cost = Convert.ToDecimal(!(reader["Cost"] is DBNull) ? reader["Cost"] : 0),
                            InvoiceNumber = reader["InvoiceNumber"].ToString(),
                            PlaceID = Convert.ToUInt32(!(reader["PlaceID"] is DBNull)
                                ? reader["PlaceID"]
                                : 0),
                            Diagonal = Convert.ToDecimal(!(reader["MaxDiagonal"] is DBNull)
                                ? reader["MaxDiagonal"]
                                : 0),
                            ResolutionID = Convert.ToUInt32(!(reader["ResolutionID"] is DBNull)
                                ? reader["ResolutionID"]
                                : 0),
                            ProjectorTechnologyID =
                                Convert.ToUInt32(!(reader["ProjectorTechnologyID"] is DBNull)
                                    ? reader["ProjectorTechnologyID"]
                                    : 0),
                            VideoConnectorsValue = Convert.ToInt32(!(reader["VideoConnectors"] is DBNull)
                                ? reader["VideoConnectors"]
                                : 0)
                        };
                        if (device is Projector projector)
                        {
                            projector.VideoConnectors = GetListVideoConnectors(projector.VideoConnectorsValue);

                            inventoryNumber.Text = device.InventoryNumber.ToString();
                            name.Text = device.Name;
                            cost.Text = device.Cost.ToString();
                            diagonal.Text = projector.Diagonal.ToString();
                            foreach (var obj in resolution.ItemsSource)
                            {
                                DataRowView row;
                                row = obj as DataRowView;
                                if (Convert.ToUInt32(row.Row[0]) == Convert.ToUInt32(projector.ResolutionID))
                                {
                                    resolution.SelectedItem = row;
                                    break;
                                }
                            }

                            foreach (var obj in projectorTechnology.ItemsSource)
                            {
                                DataRowView row;
                                row = obj as DataRowView;
                                if (Convert.ToUInt32(row.Row[0]) == Convert.ToUInt32(projector.ProjectorTechnologyID))
                                {
                                    projectorTechnology.SelectedItem = row;
                                    break;
                                }
                            }

                            foreach (var obj in vConnectors.Items)
                            {
                                var item = obj as ListBoxItem;
                                foreach (var connector in projector.VideoConnectors)
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void GetInteractiveWhiteboard(int id)
        {
            string commandString;
            SqlDataReader reader;
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = $"SELECT * FROM dbo.GetBoardByID({id})";
                reader = new SqlCommand(commandString, connection).ExecuteReader();
                if (reader.HasRows)
                    if (reader.Read())
                    {
                        device = new InteractiveWhiteboard
                        {
                            InventoryNumber = Convert.ToInt64(reader["InventoryNumber"]),
                            Name = reader["Name"].ToString(),
                            Cost = Convert.ToDecimal(!(reader["Cost"] is DBNull) ? reader["Cost"] : 0),
                            InvoiceNumber = reader["InvoiceNumber"].ToString(),
                            PlaceID = Convert.ToUInt32(!(reader["PlaceID"] is DBNull)
                                ? reader["PlaceID"]
                                : 0),
                            Diagonal = Convert.ToDecimal(!(reader["Diagonal"] is DBNull)
                                ? reader["Diagonal"]
                                : 0)
                        };
                        var board = device as InteractiveWhiteboard;

                        inventoryNumber.Text = device.InventoryNumber.ToString();
                        name.Text = device.Name;
                        cost.Text = device.Cost.ToString();
                        diagonal.Text = board?.Diagonal.ToString();
                    }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void GetProjectorScreen(int id)
        {
            string commandString;
            SqlDataReader reader;
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = $"SELECT * FROM dbo.GetScreenByID({id})";
                reader = new SqlCommand(commandString, connection).ExecuteReader();
                if (reader.HasRows)
                    if (reader.Read())
                    {
                        device = new ProjectorScreen
                        {
                            InventoryNumber = Convert.ToInt64(reader["InventoryNumber"]),
                            Name = reader["Name"].ToString(),
                            Cost = Convert.ToDecimal(!(reader["Cost"] is DBNull) ? reader["Cost"] : 0),
                            InvoiceNumber = reader["InvoiceNumber"].ToString(),
                            PlaceID = Convert.ToUInt32(!(reader["PlaceID"] is DBNull)
                                ? reader["PlaceID"]
                                : 0),
                            Diagonal = Convert.ToDecimal(!(reader["Diagonal"] is DBNull)
                                ? reader["Diagonal"]
                                : 0),
                            AspectRatioID = Convert.ToUInt32(!(reader["AspectRatioID"] is DBNull)
                                ? reader["AspectRatioID"]
                                : 0),
                            ScreenInstalledID = Convert.ToUInt32(!(reader["ScreenInstalledID"] is DBNull)
                                ? reader["ScreenInstalledID"]
                                : 0),
                            IsElectronicDrive =
                                Convert.ToBoolean(!(reader["IsElectronicDrive"] is DBNull)
                                    ? reader["IsElectronicDrive"]
                                    : 0)
                        };
                        var screen = device as ProjectorScreen;

                        inventoryNumber.Text = device.InventoryNumber.ToString();
                        name.Text = device.Name;
                        cost.Text = device.Cost.ToString();
                        diagonal.Text = screen?.Diagonal.ToString();
                        isEDrive.IsChecked = screen?.IsElectronicDrive;
                        foreach (var obj in aspectRatio.ItemsSource)
                        {
                            var row = obj as DataRowView;
                            if (Convert.ToUInt32(row.Row[0]) == screen.AspectRatioID)
                            {
                                aspectRatio.SelectedItem = row;
                                break;
                            }
                        }

                        foreach (var obj in screenInstalled.ItemsSource)
                        {
                            var row = obj as DataRowView;
                            if (Convert.ToUInt32(row.Row[0]) == screen.ScreenInstalledID)
                            {
                                screenInstalled.SelectedItem = row;
                                break;
                            }
                        }
                    }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void GetPrinterScanner(int id)
        {
            string commandString;
            SqlDataReader reader;
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = $"SELECT * FROM dbo.GetPrinterScannerByID({id})";
                reader = new SqlCommand(commandString, connection).ExecuteReader();
                if (reader.HasRows)
                    if (reader.Read())
                    {
                        device = new PrinterScanner
                        {
                            InventoryNumber = Convert.ToInt64(reader["InventoryNumber"]),
                            TypeID = Convert.ToUInt32(!(reader["TypePrinterID"] is DBNull)
                                ? reader["TypePrinterID"] : 0),
                            Name = reader["Name"].ToString(),
                            Cost = Convert.ToDecimal(!(reader["Cost"] is DBNull) ? reader["Cost"] : 0),
                            InvoiceNumber = reader["InvoiceNumber"].ToString(),
                            PlaceID = Convert.ToUInt32(!(reader["PlaceID"] is DBNull)
                                ? reader["PlaceID"]
                                : 0),
                            PaperSizeID = Convert.ToUInt32(!(reader["PaperSizeID"] is DBNull)
                                ? reader["PaperSizeID"]
                                : 0)
                        };
                        var printerScanner = device as PrinterScanner;

                        inventoryNumber.Text = device.InventoryNumber.ToString();
                        name.Text = device.Name;
                        cost.Text = device.Cost.ToString();
                        foreach (var obj in type.ItemsSource)
                        {
                            var row = obj as DataRowView;
                            if (Convert.ToUInt32(row?.Row[0]) == printerScanner?.TypeID)
                            {
                                type.SelectedItem = row;
                                break;
                            }
                        }

                        foreach (var obj in paperSize.ItemsSource)
                        {
                            var row = obj as DataRowView;
                            if (Convert.ToUInt32(row?.Row[0]) == printerScanner?.PaperSizeID)
                            {
                                paperSize.SelectedItem = row;
                                break;
                            }
                        }
                    }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void GetNetworkSwitch(int id)
        {
            string commandString;
            SqlDataReader reader;
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = $"SELECT * FROM dbo.GetNetworkSwitchByID({id})";
                reader = new SqlCommand(commandString, connection).ExecuteReader();
                if (reader.HasRows)
                    if (reader.Read())
                    {
                        device = new NetworkSwitch
                        {
                            InventoryNumber = Convert.ToInt64(reader["InventoryNumber"]),
                            TypeID = Convert.ToUInt32(!(reader["TypeID"] is DBNull) ? reader["TypeID"] : 0),
                            Name = reader["Name"].ToString(),
                            Cost = Convert.ToDecimal(!(reader["Cost"] is DBNull) ? reader["Cost"] : 0),
                            InvoiceNumber = reader["InvoiceNumber"].ToString(),
                            PlaceID = Convert.ToUInt32(!(reader["PlaceID"] is DBNull)
                                ? reader["PlaceID"]
                                : 0),
                            Ports = Convert.ToUInt32(!(reader["NumberOfPorts"] is DBNull)
                                ? reader["NumberOfPorts"]
                                : 0),
                            WiFiFrequencyID = Convert.ToUInt32(!(reader["WiFiFrequencyID"] is DBNull)
                                ? reader["WiFiFrequencyID"]
                                : 0)
                        };
                        var networkSwitch = device as NetworkSwitch;

                        inventoryNumber.Text = device.InventoryNumber.ToString();
                        name.Text = device.Name;
                        cost.Text = device.Cost.ToString();
                        ports.Text = networkSwitch?.Ports.ToString();
                        foreach (var obj in type.ItemsSource)
                        {
                            var row = obj as DataRowView;
                            if (Convert.ToUInt32(row?.Row[0]) == networkSwitch?.TypeID)
                            {
                                type.SelectedItem = row;
                                break;
                            }
                        }

                        foreach (var obj in wifiFrequency.ItemsSource)
                        {
                            var row = obj as DataRowView;
                            if (Convert.ToUInt32(row?.Row[0]) == networkSwitch?.WiFiFrequencyID)
                            {
                                wifiFrequency.SelectedItem = row;
                                break;
                            }
                        }
                    }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void GetOtherEquipment(int id)
        {
            string commandString;
            SqlDataReader reader;
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = $"SELECT * FROM dbo.GetOtherEquipmentByID({id})";
                reader = new SqlCommand(commandString, connection).ExecuteReader();
                if (reader.HasRows)
                    if (reader.Read())
                    {
                        device = new OtherEquipment
                        {
                            InventoryNumber = Convert.ToInt64(reader["InventoryNumber"]),
                            Name = reader["Name"].ToString(),
                            Cost = Convert.ToDecimal(!(reader["Cost"] is DBNull) ? reader["Cost"] : 0),
                            InvoiceNumber = reader["InvoiceNumber"].ToString(),
                            PlaceID = Convert.ToUInt32(!(reader["PlaceID"] is DBNull)
                                ? reader["PlaceID"]
                                : 0)
                        };

                        inventoryNumber.Text = device.InventoryNumber.ToString();
                        name.Text = device.Name;
                        cost.Text = device.Cost.ToString();
                    }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetDeviceLocationAndInvoice()
        {
            foreach (var obj in location.ItemsSource)
            {
                var row = obj as DataRowView;
                if (Convert.ToUInt32(row?.Row[0]) == device.PlaceID)
                {
                    location.SelectedItem = row;
                    break;
                }
            }

            invoice.Text = device.InvoiceNumber;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ResetEquipmentGrid()
        {
            imageFilename.Text = string.Empty;
            if (Accounting.TypeChange == TypeChange.Add)
            {
                autoInvN.Visibility = Visibility.Visible;
                if (disabledRepeatInvN.IsChecked == true)
                    DisabledRepeatInvN_Checked();
                else
                    DisabledRepeatInvN_Unchecked();
                changeAnalog.Visibility = Visibility.Collapsed;
            }
            else if (Accounting.TypeChange == TypeChange.Change)
            {
                autoInvN.Visibility = Visibility.Collapsed;
                autoInvN.IsChecked = false;
                DisabledRepeatInvN_Unchecked();
                changeAnalog.Visibility = Visibility.Visible;
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void InitializeForInteractiveWhiteboard()
        {
            invoiceGrid.Visibility = Visibility.Visible;
            locationGrid.Visibility = Visibility.Visible;
            diagonalGrid.Visibility = Visibility.Visible;
            GridPlacement(diagonalGrid, 0, 1, 2);
            GridPlacement(invoiceGrid, 2, 1, 3);
            GridPlacement(locationGrid, 5, 1, 7);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void InitializeForOtherEquipment()
        {
            invoiceGrid.Visibility = Visibility.Visible;
            locationGrid.Visibility = Visibility.Visible;
            GridPlacement(invoiceGrid, 0, 1, 3);
            GridPlacement(locationGrid, 3, 1, 9);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

        private static void GridPlacement(UIElement element, int column, int row, int colSpan, int rowSpan = 1)
        {
            Grid.SetColumn(element, column);
            Grid.SetRow(element, row);
            Grid.SetColumnSpan(element, colSpan);
            Grid.SetRowSpan(element, rowSpan);
        }

        private static List<string> GetListVideoConnectors(int value)
        {
            var arr = new List<string>
            {
                Capacity = 32
            };
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                var reader = new SqlCommand("Select * from dbo.GetAllVideoConnector() Order by value desc", connection)
                    .ExecuteReader();
                if (reader.HasRows)
                    while (reader.Read())
                    {
                        var connectorValue = Convert.ToInt32(reader["Value"]);
                        if (value >= connectorValue)
                        {
                            value -= connectorValue;
                            arr.Add(reader["Name"].ToString());
                        }
                    }
            }

            return arr;
        }

        private int GetValueVideoConnectors(ListBox list)
        {
            var value = 0;
            try
            {
                foreach (var obj in list.SelectedItems)
                foreach (DataRowView row in DefaultDataSet.Tables["VideoConnectors"].DefaultView)
                {
                    var s = (obj as ListBoxItem).Content.ToString();
                    if (row.Row[1].ToString() == s) value += Convert.ToInt32(row.Row[2]);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, e.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error);
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
                    UpdateSourceForMatrixTechnology();
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
            try
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
                                for (var i = 0; i < 10; i++)
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
                            catch
                            {
                            }
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
                                for (var i = 0; i < 10; i++)
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
                            catch
                            {
                            }
                        });
                        task.Start();
                        break;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, e.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveOrUpdateSoftwareDB()
        {
            try
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
                                for (var i = 0; i < 10; i++)
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
                            catch
                            {
                            }
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
                                for (var i = 0; i < 10; i++)
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
                            catch
                            {
                            }
                        });
                        task.Start();
                        break;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, e.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateSourceForAspectRatio()
        {
            DefaultDataSet.Tables["AspectRatio"].Clear();
            new SqlDataAdapter("SELECT * FROM dbo.GetAllAspectRatio()", ConnectionString).Fill(DefaultDataSet,
                "AspectRatio");
            aspectRatio.ItemsSource = DefaultDataSet.Tables["AspectRatio"].DefaultView;
            aspectRatio.DisplayMemberPath = "Name";
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateSourceForOS()
        {
            DefaultDataSet.Tables["OS"].Clear();
            new SqlDataAdapter("SELECT * FROM dbo.GetAllOS()", ConnectionString).Fill(DefaultDataSet, "OS");
            os.ItemsSource = DefaultDataSet.Tables["OS"].DefaultView;
            os.DisplayMemberPath = "Наименование";
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateSourceForScreenInstalled()
        {
            DefaultDataSet.Tables["ScreenInstalled"].Clear();
            new SqlDataAdapter("SELECT * FROM dbo.GetAllScreenInstalled()", ConnectionString).Fill(DefaultDataSet,
                "ScreenInstalled");
            screenInstalled.ItemsSource = DefaultDataSet.Tables["ScreenInstalled"].DefaultView;
            screenInstalled.DisplayMemberPath = "Name";
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateSourceForName()
        {
            SqlDataAdapter NameDataAdapter = null;
            switch (Accounting.TypeDevice)
            {
                case TypeDevice.InteractiveWhiteboard:
                    NameDataAdapter = new SqlDataAdapter("SELECT * FROM dbo.GetAllBoardName()", ConnectionString);
                    break;
                case TypeDevice.Monitor:
                    NameDataAdapter = new SqlDataAdapter("SELECT * FROM dbo.GetAllMonitorName()", ConnectionString);
                    break;
                case TypeDevice.NetworkSwitch:
                    NameDataAdapter =
                        new SqlDataAdapter("SELECT * FROM dbo.GetAllNetworkSwitchName()", ConnectionString);
                    break;
                case TypeDevice.Notebook:
                    NameDataAdapter = new SqlDataAdapter("SELECT * FROM dbo.GetAllNotebookName()", ConnectionString);
                    break;
                case TypeDevice.OtherEquipment:
                    NameDataAdapter =
                        new SqlDataAdapter("SELECT * FROM dbo.GetAllOtherEquipmentName()", ConnectionString);
                    break;
                case TypeDevice.PC:
                    NameDataAdapter = new SqlDataAdapter("SELECT * FROM dbo.GetAllPCName()", ConnectionString);
                    break;
                case TypeDevice.PrinterScanner:
                    NameDataAdapter =
                        new SqlDataAdapter("SELECT * FROM dbo.GetAllPrinterScannerName()", ConnectionString);
                    break;
                case TypeDevice.Projector:
                    NameDataAdapter = new SqlDataAdapter("SELECT * FROM dbo.GetAllProjectorName()", ConnectionString);
                    break;
                case TypeDevice.ProjectorScreen:
                    NameDataAdapter =
                        new SqlDataAdapter("SELECT * FROM dbo.GetAllProjectorScreenName()", ConnectionString);
                    break;
            }

            DefaultDataSet.Tables["Name"].Clear();
            NameDataAdapter?.Fill(DefaultDataSet, "Name");
            name.ItemsSource = DefaultDataSet.Tables["Name"].DefaultView;
            name.DisplayMemberPath = "Name";
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateSourceForLocation()
        {
            SqlDataAdapter LocationDataAdapter = null;
            switch (Accounting.TypeDevice)
            {
                case TypeDevice.InteractiveWhiteboard:
                    LocationDataAdapter =
                        new SqlDataAdapter("SELECT * FROM dbo.[GetAllCanUsedLocationByTypeDeviceID](4)",
                            ConnectionString);
                    break;
                case TypeDevice.Monitor:
                    LocationDataAdapter =
                        new SqlDataAdapter("SELECT * FROM dbo.[GetAllCanUsedLocationByTypeDeviceID](6)",
                            ConnectionString);
                    break;
                case TypeDevice.NetworkSwitch:
                    LocationDataAdapter =
                        new SqlDataAdapter("SELECT * FROM dbo.[GetAllCanUsedLocationByTypeDeviceID](5)",
                            ConnectionString);
                    break;
                case TypeDevice.Notebook:
                    LocationDataAdapter =
                        new SqlDataAdapter("SELECT * FROM dbo.[GetAllCanUsedLocationByTypeDeviceID](2)",
                            ConnectionString);
                    break;
                case TypeDevice.OtherEquipment:
                    LocationDataAdapter =
                        new SqlDataAdapter("SELECT * FROM dbo.[GetAllCanUsedLocationByTypeDeviceID](9)",
                            ConnectionString);
                    break;
                case TypeDevice.PC:
                    LocationDataAdapter =
                        new SqlDataAdapter("SELECT * FROM dbo.[GetAllCanUsedLocationByTypeDeviceID](1)",
                            ConnectionString);
                    break;
                case TypeDevice.PrinterScanner:
                    LocationDataAdapter =
                        new SqlDataAdapter("SELECT * FROM dbo.[GetAllCanUsedLocationByTypeDeviceID](3)",
                            ConnectionString);
                    break;
                case TypeDevice.Projector:
                    LocationDataAdapter =
                        new SqlDataAdapter("SELECT * FROM dbo.[GetAllCanUsedLocationByTypeDeviceID](7)",
                            ConnectionString);
                    break;
                case TypeDevice.ProjectorScreen:
                    LocationDataAdapter =
                        new SqlDataAdapter("SELECT * FROM dbo.[GetAllCanUsedLocationByTypeDeviceID](8)",
                            ConnectionString);
                    break;
            }

            DefaultDataSet.Tables["Location"].Clear();
            LocationDataAdapter?.Fill(DefaultDataSet, "Location");
            location.ItemsSource = DefaultDataSet.Tables["Location"].DefaultView;
            location.DisplayMemberPath = "Place";
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateSourceForCPU()
        {
            switch (Accounting.TypeDevice)
            {
                case TypeDevice.Notebook:
                    DefaultDataSet.Tables["CPU"].Clear();
                    new SqlDataAdapter("SELECT * FROM dbo.GetAllNotebookCPU()", ConnectionString).Fill(DefaultDataSet,
                        "CPU");
                    cpu.ItemsSource = DefaultDataSet.Tables["CPU"].DefaultView;
                    cpu.DisplayMemberPath = "CPUModel";
                    break;
                case TypeDevice.PC:
                    DefaultDataSet.Tables["CPU"].Clear();
                    new SqlDataAdapter("SELECT * FROM dbo.GetAllPCCPU()", ConnectionString).Fill(DefaultDataSet, "CPU");
                    cpu.ItemsSource = DefaultDataSet.Tables["CPU"].DefaultView;
                    cpu.DisplayMemberPath = "CPUModel";
                    break;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateSourceForVideoCard()
        {
            switch (Accounting.TypeDevice)
            {
                case TypeDevice.Notebook:
                    DefaultDataSet.Tables["VCard"].Clear();
                    new SqlDataAdapter("SELECT * FROM dbo.GetAllNotebookvCard()", ConnectionString).Fill(DefaultDataSet,
                        "VCard");
                    vCard.ItemsSource = DefaultDataSet.Tables["VCard"].DefaultView;
                    vCard.DisplayMemberPath = "VideoCard";
                    break;
                case TypeDevice.PC:
                    DefaultDataSet.Tables["VCard"].Clear();
                    new SqlDataAdapter("SELECT * FROM dbo.GetAllPCvCard()", ConnectionString).Fill(DefaultDataSet,
                        "VCard");
                    vCard.ItemsSource = DefaultDataSet.Tables["VCard"].DefaultView;
                    vCard.DisplayMemberPath = "VideoCard";
                    break;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateSourceForType()
        {
            switch (Accounting.TypeDevice)
            {
                case TypeDevice.NetworkSwitch:
                    DefaultDataSet.Tables["TypeNetworkSwitch"].Clear();
                    new SqlDataAdapter("SELECT * FROM dbo.GetAllTypeNetworkSwitch()", ConnectionString).Fill(
                        DefaultDataSet, "TypeNetworkSwitch");
                    type.ItemsSource = DefaultDataSet.Tables["TypeNetworkSwitch"].DefaultView;
                    type.DisplayMemberPath = "Name";
                    break;
                case TypeDevice.Notebook:
                    DefaultDataSet.Tables["TypeNotebook"].Clear();
                    new SqlDataAdapter("SELECT * FROM dbo.GetAllTypeNotebook()", ConnectionString).Fill(DefaultDataSet,
                        "TypeNotebook");
                    type.ItemsSource = DefaultDataSet.Tables["TypeNotebook"].DefaultView;
                    type.DisplayMemberPath = "Name";
                    break;
                case TypeDevice.PrinterScanner:
                    DefaultDataSet.Tables["TypePrinter"].Clear();
                    new SqlDataAdapter("SELECT * FROM dbo.GetAllTypePrinter()", ConnectionString).Fill(DefaultDataSet,
                        "TypePrinter");
                    type.ItemsSource = DefaultDataSet.Tables["TypePrinter"].DefaultView;
                    type.DisplayMemberPath = "Name";
                    break;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateSourceForFrequency()
        {
            DefaultDataSet.Tables["Frequency"].Clear();
            new SqlDataAdapter("SELECT * FROM dbo.GetAllFrequency()", ConnectionString).Fill(DefaultDataSet,
                "Frequency");
            screenFrequency.ItemsSource = DefaultDataSet.Tables["Frequency"].DefaultView;
            screenFrequency.DisplayMemberPath = "Name";
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateSourceForMatrixTechnology()
        {
            DefaultDataSet.Tables["MatrixTechnology"].Clear();
            new SqlDataAdapter("SELECT * FROM dbo.GetAllMatrixTechnology()", ConnectionString).Fill(DefaultDataSet,
                "MatrixTechnology");
            matrixTechnology.ItemsSource = DefaultDataSet.Tables["MatrixTechnology"].DefaultView;
            matrixTechnology.DisplayMemberPath = "Name";
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateSourceForPaperSize()
        {
            DefaultDataSet.Tables["PaperSize"].Clear();
            new SqlDataAdapter("SELECT * FROM dbo.GetAllPaperSize()", ConnectionString).Fill(DefaultDataSet,
                "PaperSize");
            paperSize.ItemsSource = DefaultDataSet.Tables["PaperSize"].DefaultView;
            paperSize.DisplayMemberPath = "Name";
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateSourceForProjectorTechnology()
        {
            DefaultDataSet.Tables["ProjectorTechnology"].Clear();
            new SqlDataAdapter("SELECT * FROM dbo.GetAllProjectorTechnology()", ConnectionString).Fill(DefaultDataSet,
                "ProjectorTechnology");
            projectorTechnology.ItemsSource = DefaultDataSet.Tables["ProjectorTechnology"].DefaultView;
            projectorTechnology.DisplayMemberPath = "Name";
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateSourceForResolution()
        {
            DefaultDataSet.Tables["Resolution"].Clear();
            new SqlDataAdapter("SELECT * FROM dbo.GetAllResolution()", ConnectionString).Fill(DefaultDataSet,
                "Resolution");
            resolution.ItemsSource = DefaultDataSet.Tables["Resolution"].DefaultView;
            resolution.DisplayMemberPath = "Name";
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateSourceForVideoConnectors()
        {
            DefaultDataSet.Tables["VideoConnectors"].Clear();
            new SqlDataAdapter("SELECT * FROM dbo.GetAllVideoConnector()", ConnectionString).Fill(DefaultDataSet,
                "VideoConnectors");
            videoConnectorsItems = new List<ListBoxItem>
            {
                Capacity = 32
            };
            foreach (DataRowView row in DefaultDataSet.Tables["VideoConnectors"].DefaultView)
                videoConnectorsItems.Add(new ListBoxItem {Content = row.Row[1].ToString()});

            vConnectors.ItemsSource = videoConnectorsItems;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateSourceForWifiFrequency()
        {
            DefaultDataSet.Tables["WifiFrequency"].Clear();
            new SqlDataAdapter("SELECT * FROM dbo.GetAllWiFiFrequency()", ConnectionString).Fill(DefaultDataSet,
                "WifiFrequency");
            wifiFrequency.ItemsSource = DefaultDataSet.Tables["WifiFrequency"].DefaultView;
            wifiFrequency.DisplayMemberPath = "Name";
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateSourceForInvoice()
        {
            DefaultDataSet.Tables["Invoice"].Clear();
            new SqlDataAdapter("SELECT [Number] FROM dbo.GetAllInvoice()", ConnectionString).Fill(DefaultDataSet,
                "Invoice");
            invoice.ItemsSource = DefaultDataSet.Tables["Invoice"].DefaultView;
            softwareInvoice.ItemsSource = DefaultDataSet.Tables["Invoice"].DefaultView;
            invoice.DisplayMemberPath = "Number";
            softwareInvoice.DisplayMemberPath = "Number";
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateSourceForMotherboard()
        {
            DefaultDataSet.Tables["Motherboard"].Clear();
            new SqlDataAdapter("SELECT * FROM dbo.GetAllMotherboard()", ConnectionString).Fill(DefaultDataSet,
                "Motherboard");
            motherboard.ItemsSource = DefaultDataSet.Tables["Motherboard"].DefaultView;
            motherboard.DisplayMemberPath = "Motherboard";
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateSourceForTypeLicense()
        {
            DefaultDataSet.Tables["TypeLicense"].Clear();
            new SqlDataAdapter("SELECT * FROM dbo.GetAllTypeSoftLicense()", ConnectionString).Fill(DefaultDataSet,
                "TypeLicense");
            typeLicense.ItemsSource = DefaultDataSet.Tables["TypeLicense"].DefaultView;
            typeLicense.DisplayMemberPath = "Name";
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void AddPC()
        {
            string commandString;
            SqlCommand command;
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = "AddPC";
                command = new SqlCommand(commandString, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt64(inventoryNumber.Text)));
                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToDecimal(cost.Text)));
                if (!string.IsNullOrWhiteSpace(invoice.Text)) command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text));
                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView) location?.SelectedItem)?.Row?[0]));
                if (!string.IsNullOrWhiteSpace(cpu.Text)) command.Parameters.Add(new SqlParameter("@CPU", cpu.Text));
                if (!string.IsNullOrWhiteSpace(cores.Text)) command.Parameters.Add(new SqlParameter("@Cores", Convert.ToInt32(cores.Text)));
                if (!string.IsNullOrWhiteSpace(frequency.Text)) command.Parameters.Add(new SqlParameter("@Frequency", Convert.ToInt32(frequency.Text)));
                if (!string.IsNullOrWhiteSpace(maxFrequency.Text)) command.Parameters.Add(new SqlParameter("@MaxFrequency", Convert.ToInt32(maxFrequency.Text)));
                if (!string.IsNullOrWhiteSpace(ram.Text)) command.Parameters.Add(new SqlParameter("@RAM", Convert.ToInt32(ram.Text)));
                if (!string.IsNullOrWhiteSpace(ramFrequency.Text)) command.Parameters.Add(new SqlParameter("@FrequencyRAM", Convert.ToInt32(ramFrequency.Text)));
                if (!string.IsNullOrWhiteSpace(ssd.Text)) command.Parameters.Add(new SqlParameter("@SSD", Convert.ToInt32(ssd.Text)));
                if (!string.IsNullOrWhiteSpace(hdd.Text)) command.Parameters.Add(new SqlParameter("@HDD", Convert.ToInt32(hdd.Text)));
                if (!string.IsNullOrWhiteSpace(vCard.Text)) command.Parameters.Add(new SqlParameter("@Video", vCard.Text));
                if (!string.IsNullOrWhiteSpace(videoram.Text)) command.Parameters.Add(new SqlParameter("@VRAM", Convert.ToInt32(videoram.Text)));
                command.Parameters.Add(new SqlParameter("@OSID", ((DataRowView) os?.SelectedItem)?[0]));
                if (!string.IsNullOrWhiteSpace(motherboard.Text)) command.Parameters.Add(new SqlParameter("@MB", motherboard.Text));
                command.Parameters.Add(new SqlParameter("@VConnectors", GetValueVideoConnectors(vConnectors)));
                var bytes = LoadImage(imageFilename.Text);
                if (bytes != null)
                    command.Parameters.Add(new SqlParameter("@Image", bytes));
                else
                    command.Parameters.Add(new SqlParameter("@Image", SqlDbType.VarBinary) {SqlValue = null});

                command.ExecuteNonQuery();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void AddNotebook()
        {
            string commandString;
            SqlCommand command;
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = "AddNotebook";
                command = new SqlCommand(commandString, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt64(inventoryNumber.Text)));
                command.Parameters.Add(new SqlParameter("@Type", ((DataRowView) type?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToDecimal(cost.Text)));
                if (!string.IsNullOrWhiteSpace(invoice.Text)) command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text));
                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?.Row?[0]));
                if (!string.IsNullOrWhiteSpace(diagonal.Text)) command.Parameters.Add(new SqlParameter("@Diagonal", Convert.ToDecimal(diagonal.Text)));
                if (!string.IsNullOrWhiteSpace(cpu.Text)) command.Parameters.Add(new SqlParameter("@CPU", cpu.Text));
                if (!string.IsNullOrWhiteSpace(cores.Text)) command.Parameters.Add(new SqlParameter("@Cores", Convert.ToInt32(cores.Text)));
                if (!string.IsNullOrWhiteSpace(frequency.Text)) command.Parameters.Add(new SqlParameter("@Frequency", Convert.ToInt32(frequency.Text)));
                if (!string.IsNullOrWhiteSpace(maxFrequency.Text)) command.Parameters.Add(new SqlParameter("@MaxFrequency", Convert.ToInt32(maxFrequency.Text)));
                if (!string.IsNullOrWhiteSpace(ram.Text)) command.Parameters.Add(new SqlParameter("@RAM", Convert.ToInt32(ram.Text)));
                if (!string.IsNullOrWhiteSpace(ramFrequency.Text)) command.Parameters.Add(new SqlParameter("@FrequencyRAM", Convert.ToInt32(ramFrequency.Text)));
                if (!string.IsNullOrWhiteSpace(ssd.Text)) command.Parameters.Add(new SqlParameter("@SSD", Convert.ToInt32(ssd.Text)));
                if (!string.IsNullOrWhiteSpace(hdd.Text)) command.Parameters.Add(new SqlParameter("@HDD", Convert.ToInt32(hdd.Text)));
                if (!string.IsNullOrWhiteSpace(vCard.Text)) command.Parameters.Add(new SqlParameter("@Video", invoice.Text));
                if (!string.IsNullOrWhiteSpace(videoram.Text)) command.Parameters.Add(new SqlParameter("@VRAM", Convert.ToInt32(videoram.Text)));
                command.Parameters.Add(new SqlParameter("@OSID", ((DataRowView) os?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@ResolutionID", ((DataRowView) resolution?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@FrequencyID", ((DataRowView) screenFrequency?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@MatrixID", ((DataRowView) matrixTechnology?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@VConnectors", GetValueVideoConnectors(vConnectors)));
                var bytes = LoadImage(imageFilename.Text);
                if (bytes != null) command.Parameters.Add(new SqlParameter("@Image", bytes));

                command.ExecuteNonQuery();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void AddMonitor()
        {
            string commandString;
            SqlCommand command;
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = "AddMonitor";
                command = new SqlCommand(commandString, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt64(inventoryNumber.Text)));
                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToDecimal(cost.Text)));
                if (!string.IsNullOrWhiteSpace(invoice.Text)) command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text));
                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView) location?.SelectedItem)?.Row?[0]));
                if (!string.IsNullOrWhiteSpace(diagonal.Text)) command.Parameters.Add(new SqlParameter("@Diagonal", Convert.ToDecimal(diagonal.Text)));
                command.Parameters.Add(new SqlParameter("@ResolutionID", ((DataRowView) resolution?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@FrequencyID", ((DataRowView) screenFrequency?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@MatrixID", ((DataRowView) matrixTechnology?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@VConnectors", GetValueVideoConnectors(vConnectors)));
                var bytes = LoadImage(imageFilename.Text);
                if (bytes != null) command.Parameters.Add(new SqlParameter("@Image", bytes));

                command.ExecuteNonQuery();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void AddNetworkSwitch()
        {
            string commandString;
            SqlCommand command;
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = "AddNetworkSwitch";
                command = new SqlCommand(commandString, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt64(inventoryNumber.Text)));
                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToDecimal(cost.Text)));
                if (!string.IsNullOrWhiteSpace(invoice.Text)) command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text));
                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView) location?.SelectedItem)?.Row?[0]));
                if (!string.IsNullOrWhiteSpace(ports.Text)) command.Parameters.Add(new SqlParameter("@Ports", Convert.ToInt32(ports.Text)));
                command.Parameters.Add(new SqlParameter("@TypeID", ((DataRowView) type?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@Frequency", ((DataRowView) wifiFrequency?.SelectedItem)?[0]));
                var bytes = LoadImage(imageFilename.Text);
                if (bytes != null) command.Parameters.Add(new SqlParameter("@Image", bytes));

                command.ExecuteNonQuery();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void AddInteractiveWhiteboard()
        {
            string commandString;
            SqlCommand command;
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = "AddInteractiveWhiteboard";
                command = new SqlCommand(commandString, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt64(inventoryNumber.Text)));
                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToDecimal(cost.Text)));
                if (!string.IsNullOrWhiteSpace(invoice.Text)) command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text));
                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView) location?.SelectedItem)?.Row?[0]));
                if (!string.IsNullOrWhiteSpace(diagonal.Text)) command.Parameters.Add(new SqlParameter("@Diagonal", Convert.ToDecimal(diagonal.Text)));
                var bytes = LoadImage(imageFilename.Text);
                if (bytes != null) command.Parameters.Add(new SqlParameter("@Image", bytes));

                command.ExecuteNonQuery();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void AddPrinterScanner()
        {
            string commandString;
            SqlCommand command;
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = "AddPrinterScanner";
                command = new SqlCommand(commandString, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt64(inventoryNumber.Text)));
                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToDecimal(cost.Text)));
                if (!string.IsNullOrWhiteSpace(invoice.Text)) command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text));
                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView) location?.SelectedItem)?.Row?[0]));
                command.Parameters.Add(new SqlParameter("@TypeID", ((DataRowView) type?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@PaperSizeID", ((DataRowView) paperSize?.SelectedItem)?[0]));
                var bytes = LoadImage(imageFilename.Text);
                if (bytes != null) command.Parameters.Add(new SqlParameter("@Image", bytes));

                command.ExecuteNonQuery();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void AddProjector()
        {
            string commandString;
            SqlCommand command;
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = "AddProjector";
                command = new SqlCommand(commandString, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt64(inventoryNumber.Text)));
                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToDecimal(cost.Text)));
                if (!string.IsNullOrWhiteSpace(invoice.Text)) command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text));
                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView) location?.SelectedItem)?.Row?[0]));
                command.Parameters.Add(new SqlParameter("@TechnologyID", ((DataRowView) projectorTechnology?.SelectedItem)?[0]));
                if (!string.IsNullOrWhiteSpace(diagonal.Text)) command.Parameters.Add(new SqlParameter("@Diagonal", Convert.ToDecimal(diagonal.Text)));
                command.Parameters.Add(new SqlParameter("@ResolutionID", ((DataRowView) resolution?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@VConnectors", GetValueVideoConnectors(vConnectors)));
                var bytes = LoadImage(imageFilename.Text);
                if (bytes != null) command.Parameters.Add(new SqlParameter("@Image", bytes));

                command.ExecuteNonQuery();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void AddProjectorScreen()
        {
            string commandString;
            SqlCommand command;
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = "AddProjectorScreen";
                command = new SqlCommand(commandString, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt64(inventoryNumber.Text)));
                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToDecimal(cost.Text)));
                if (!string.IsNullOrWhiteSpace(invoice.Text)) command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text));
                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView) location?.SelectedItem)?.Row?[0]));
                if (!string.IsNullOrWhiteSpace(diagonal.Text)) command.Parameters.Add(new SqlParameter("@Diagonal", Convert.ToDecimal(diagonal.Text)));
                command.Parameters.Add(new SqlParameter("@IsElectronic", Convert.ToBoolean(isEDrive.IsChecked)));
                command.Parameters.Add(new SqlParameter("@AspectRatioID", ((DataRowView) aspectRatio?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@InstalledID", ((DataRowView) screenInstalled?.SelectedItem)?[0]));
                var bytes = LoadImage(imageFilename.Text);
                if (bytes != null) command.Parameters.Add(new SqlParameter("@Image", bytes));

                command.ExecuteNonQuery();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void AddOtherEquipment()
        {
            string commandString;
            SqlCommand command;
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = "AddOtherEquipment";
                command = new SqlCommand(commandString, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt64(inventoryNumber.Text)));
                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToDecimal(cost.Text)));
                if (!string.IsNullOrWhiteSpace(invoice.Text)) command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text));
                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView) location?.SelectedItem)?.Row?[0]));
                var bytes = LoadImage(imageFilename.Text);
                if (bytes != null) command.Parameters.Add(new SqlParameter("@Image", bytes));

                command.ExecuteNonQuery();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdatePC()
        {
            string commandString;
            SqlCommand command;
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = "UpdatePCByID";
                command = new SqlCommand(commandString, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(new SqlParameter("@ID", DeviceID));
                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt64(inventoryNumber.Text)));
                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToDecimal(cost.Text)));
                if (!string.IsNullOrWhiteSpace(invoice.Text)) command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text));
                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView) location?.SelectedItem)?.Row?[0]));
                if (!string.IsNullOrWhiteSpace(cpu.Text)) command.Parameters.Add(new SqlParameter("@CPU", cpu.Text));
                if (!string.IsNullOrWhiteSpace(cores.Text)) command.Parameters.Add(new SqlParameter("@Cores", Convert.ToInt32(cores.Text)));
                if (!string.IsNullOrWhiteSpace(frequency.Text)) command.Parameters.Add(new SqlParameter("@Frequency", Convert.ToInt32(frequency.Text)));
                if (!string.IsNullOrWhiteSpace(maxFrequency.Text)) command.Parameters.Add(new SqlParameter("@MaxFrequency", Convert.ToInt32(maxFrequency.Text)));
                if (!string.IsNullOrWhiteSpace(ram.Text)) command.Parameters.Add(new SqlParameter("@RAM", Convert.ToInt32(ram.Text)));
                if (!string.IsNullOrWhiteSpace(ramFrequency.Text)) command.Parameters.Add(new SqlParameter("@FrequencyRAM", Convert.ToInt32(ramFrequency.Text)));
                if (!string.IsNullOrWhiteSpace(ssd.Text)) command.Parameters.Add(new SqlParameter("@SSD", Convert.ToInt32(ssd.Text)));
                if (!string.IsNullOrWhiteSpace(hdd.Text)) command.Parameters.Add(new SqlParameter("@HDD", Convert.ToInt32(hdd.Text)));
                if (!string.IsNullOrWhiteSpace(vCard.Text)) command.Parameters.Add(new SqlParameter("@Video", vCard.Text));
                if (!string.IsNullOrWhiteSpace(videoram.Text)) command.Parameters.Add(new SqlParameter("@VRAM", Convert.ToInt32(videoram.Text)));
                command.Parameters.Add(new SqlParameter("@OSID", ((DataRowView) os?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@MB", motherboard.Text));
                command.Parameters.Add(new SqlParameter("@VConnectors", GetValueVideoConnectors(vConnectors)));
                var bytes = LoadImage(imageFilename.Text);
                if (bytes != null) command.Parameters.Add(new SqlParameter("@Image", bytes));

                command.Parameters.Add(new SqlParameter("@IsChangeAnalog", IsChangeAnalog));
                command.ExecuteNonQuery();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateNotebook()
        {
            string commandString;
            SqlCommand command;
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = "UpdateNotebookByID";
                command = new SqlCommand(commandString, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(new SqlParameter("@ID", DeviceID));
                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt64(inventoryNumber.Text)));
                command.Parameters.Add(new SqlParameter("@Type", ((DataRowView) type?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToDecimal(cost.Text)));
                if (!string.IsNullOrWhiteSpace(invoice.Text)) command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text));
                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?.Row?[0]));
                if (!string.IsNullOrWhiteSpace(diagonal.Text)) command.Parameters.Add(new SqlParameter("@Diagonal", Convert.ToDecimal(diagonal.Text)));
                if (!string.IsNullOrWhiteSpace(cpu.Text)) command.Parameters.Add(new SqlParameter("@CPU", cpu.Text));
                if (!string.IsNullOrWhiteSpace(cores.Text)) command.Parameters.Add(new SqlParameter("@Cores", Convert.ToInt32(cores.Text)));
                if (!string.IsNullOrWhiteSpace(frequency.Text)) command.Parameters.Add(new SqlParameter("@Frequency", Convert.ToInt32(frequency.Text)));
                if (!string.IsNullOrWhiteSpace(maxFrequency.Text)) command.Parameters.Add(new SqlParameter("@MaxFrequency", Convert.ToInt32(maxFrequency.Text)));
                if (!string.IsNullOrWhiteSpace(ram.Text)) command.Parameters.Add(new SqlParameter("@RAM", Convert.ToInt32(ram.Text)));
                if (!string.IsNullOrWhiteSpace(ramFrequency.Text)) command.Parameters.Add(new SqlParameter("@FrequencyRAM", Convert.ToInt32(ramFrequency.Text)));
                if (!string.IsNullOrWhiteSpace(ssd.Text)) command.Parameters.Add(new SqlParameter("@SSD", Convert.ToInt32(ssd.Text)));
                if (!string.IsNullOrWhiteSpace(hdd.Text)) command.Parameters.Add(new SqlParameter("@HDD", Convert.ToInt32(hdd.Text)));
                if (!string.IsNullOrWhiteSpace(vCard.Text)) command.Parameters.Add(new SqlParameter("@Video", invoice.Text));
                if (!string.IsNullOrWhiteSpace(videoram.Text)) command.Parameters.Add(new SqlParameter("@VRAM", Convert.ToInt32(videoram.Text)));
                command.Parameters.Add(new SqlParameter("@OSID", ((DataRowView) os?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@ResolutionID", ((DataRowView) resolution?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@FrequencyID",
                    ((DataRowView) screenFrequency?.SelectedItem)?[0]));
                command.Parameters.Add(
                    new SqlParameter("@MatrixID", ((DataRowView) matrixTechnology?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@VConnectors", GetValueVideoConnectors(vConnectors)));
                var bytes = LoadImage(imageFilename.Text);
                if (bytes != null) command.Parameters.Add(new SqlParameter("@Image", bytes));

                command.Parameters.Add(new SqlParameter("@IsChangeAnalog", IsChangeAnalog));
                command.ExecuteNonQuery();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateMonitor()
        {
            string commandString;
            SqlCommand command;
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = "UpdateMonitorByID";
                command = new SqlCommand(commandString, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(new SqlParameter("@ID", DeviceID));
                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt64(inventoryNumber.Text)));
                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToDecimal(cost.Text)));
                if (!string.IsNullOrWhiteSpace(invoice.Text)) command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text));
                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?.Row?[0]));
                if (!string.IsNullOrWhiteSpace(diagonal.Text)) command.Parameters.Add(new SqlParameter("@Diagonal", Convert.ToDecimal(diagonal.Text)));
                command.Parameters.Add(new SqlParameter("@ResolutionID", ((DataRowView) resolution?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@FrequencyID", ((DataRowView) screenFrequency?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@MatrixID", ((DataRowView) matrixTechnology?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@VConnectors", GetValueVideoConnectors(vConnectors)));
                var bytes = LoadImage(imageFilename.Text);
                if (bytes != null) command.Parameters.Add(new SqlParameter("@Image", bytes));

                command.Parameters.Add(new SqlParameter("@IsChangeAnalog", IsChangeAnalog));
                command.ExecuteNonQuery();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateNetworkSwitch()
        {
            string commandString;
            SqlCommand command;
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = "UpdateNetworkSwitchByID";
                command = new SqlCommand(commandString, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(new SqlParameter("@ID", DeviceID));
                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt64(inventoryNumber.Text)));
                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToDecimal(cost.Text)));
                if (!string.IsNullOrWhiteSpace(invoice.Text)) command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text));
                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?.Row?[0]));
                if (!string.IsNullOrWhiteSpace(ports.Text)) command.Parameters.Add(new SqlParameter("@NumberOfPorts", Convert.ToInt32(ports.Text)));
                command.Parameters.Add(new SqlParameter("@TypeID", ((DataRowView) type?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@Frequency", ((DataRowView) wifiFrequency?.SelectedItem)?[0]));
                var bytes = LoadImage(imageFilename.Text);
                if (bytes != null) command.Parameters.Add(new SqlParameter("@Image", bytes));

                command.Parameters.Add(new SqlParameter("@IsChangeAnalog", IsChangeAnalog));
                command.ExecuteNonQuery();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateInteractiveWhiteboard()
        {
            string commandString;
            SqlCommand command;
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = "UpdateInteractiveWhiteboardByID";
                command = new SqlCommand(commandString, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(new SqlParameter("@ID", DeviceID));
                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt64(inventoryNumber.Text)));
                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToDecimal(cost.Text)));
                if (!string.IsNullOrWhiteSpace(invoice.Text)) command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text));
                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?.Row?[0]));
                if (!string.IsNullOrWhiteSpace(diagonal.Text)) command.Parameters.Add(new SqlParameter("@Diagonal", Convert.ToDecimal(diagonal.Text)));
                var bytes = LoadImage(imageFilename.Text);
                if (bytes != null) command.Parameters.Add(new SqlParameter("@Image", bytes));

                command.Parameters.Add(new SqlParameter("@IsChangeAnalog", IsChangeAnalog));
                command.ExecuteNonQuery();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdatePrinterScanner()
        {
            string commandString;
            SqlCommand command;
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = "UpdatePrinterScannerByID";
                command = new SqlCommand(commandString, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(new SqlParameter("@ID", DeviceID));
                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt64(inventoryNumber.Text)));
                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToDecimal(cost.Text)));
                if (!string.IsNullOrWhiteSpace(invoice.Text)) command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text));
                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView) location?.SelectedItem)?.Row?[0]));
                command.Parameters.Add(new SqlParameter("@TypeID", ((DataRowView) type?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@PaperSizeID", ((DataRowView) paperSize?.SelectedItem)?[0]));
                var bytes = LoadImage(imageFilename.Text);
                if (bytes != null) command.Parameters.Add(new SqlParameter("@Image", bytes));

                command.Parameters.Add(new SqlParameter("@IsChangeAnalog", IsChangeAnalog));
                command.ExecuteNonQuery();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateProjector()
        {
            string commandString;
            SqlCommand command;
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = "UpdateProjectorByID";
                command = new SqlCommand(commandString, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(new SqlParameter("@ID", DeviceID));
                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt64(inventoryNumber.Text)));
                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToDecimal(cost.Text)));
                if (!string.IsNullOrWhiteSpace(invoice.Text)) command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text));
                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView) location?.SelectedItem)?.Row?[0]));
                command.Parameters.Add(new SqlParameter("@TechnologyID", ((DataRowView) projectorTechnology?.SelectedItem)?[0]));
                if (!string.IsNullOrWhiteSpace(diagonal.Text)) command.Parameters.Add(new SqlParameter("@Diagonal", Convert.ToDecimal(diagonal.Text)));
                command.Parameters.Add(new SqlParameter("@ResolutionID", ((DataRowView) resolution?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@VConnectors", GetValueVideoConnectors(vConnectors)));
                var bytes = LoadImage(imageFilename.Text);
                if (bytes != null) command.Parameters.Add(new SqlParameter("@Image", bytes));

                command.Parameters.Add(new SqlParameter("@IsChangeAnalog", IsChangeAnalog));
                command.ExecuteNonQuery();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateProjectorScreen()
        {
            string commandString;
            SqlCommand command;
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = "UpdateProjectorScreenByID";
                command = new SqlCommand(commandString, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(new SqlParameter("@ID", DeviceID));
                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt64(inventoryNumber.Text)));
                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToDecimal(cost.Text)));
                if (!string.IsNullOrWhiteSpace(invoice.Text)) command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text));
                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?.Row?[0]));
                if (!string.IsNullOrWhiteSpace(diagonal.Text)) command.Parameters.Add(new SqlParameter("@Diagonal", Convert.ToDecimal(diagonal.Text)));
                command.Parameters.Add(new SqlParameter("@IsEDrive", Convert.ToBoolean(isEDrive.IsChecked)));
                command.Parameters.Add(new SqlParameter("@AspectRatioID", ((DataRowView) aspectRatio?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@InstalledID", ((DataRowView) screenInstalled?.SelectedItem)?[0]));
                var bytes = LoadImage(imageFilename.Text);
                if (bytes != null) command.Parameters.Add(new SqlParameter("@Image", bytes));

                command.Parameters.Add(new SqlParameter("@IsChangeAnalog", IsChangeAnalog));
                command.ExecuteNonQuery();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateOtherEquipment()
        {
            string commandString;
            SqlCommand command;
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = "UpdateOtherEquipmentByID";
                command = new SqlCommand(commandString, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(new SqlParameter("@ID", DeviceID));
                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt64(inventoryNumber.Text)));
                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToDecimal(cost.Text)));
                if (!string.IsNullOrWhiteSpace(invoice.Text)) command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text));
                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView) location?.SelectedItem)?.Row?[0]));
                var bytes = LoadImage(imageFilename.Text);
                if (bytes != null) command.Parameters.Add(new SqlParameter("@Image", bytes));

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
                    using (var fs = new FileStream(path, FileMode.Open))
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

            if (Accounting.TypeChange == TypeChange.Change)
            {
                var obj = ((DataRowView) Accounting.equipmentView.SelectedItems?[0]).Row["ImageID"];
                var id = Convert.ToInt32(obj is DBNull ? null : obj);
                if (id != 0) return Accounting.Images[id];
                return null;
            }

            return null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void AddSoftware()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = null;
                command = new SqlCommand("AddLicenseSoftware", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(new SqlParameter("@Name", softwareName.Text));
                command.Parameters.Add(new SqlParameter("@Price", Convert.ToDecimal(softwareCost.Text)));
                command.Parameters.Add(new SqlParameter("@Count", Convert.ToInt32(softwareCount.Text)));
                command.Parameters.Add(new SqlParameter("@InvoiceNumber", softwareInvoice.Text));
                command.Parameters.Add(new SqlParameter("@Type", ((DataRowView) typeLicense.SelectedItem).Row[0]));
                command?.ExecuteNonQuery();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void AddOS()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = null;
                command = new SqlCommand("AddOS", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(new SqlParameter("@Name", softwareName.Text));
                command.Parameters.Add(new SqlParameter("@Price", Convert.ToDecimal(softwareCost.Text)));
                command.Parameters.Add(new SqlParameter("@Count", Convert.ToInt32(softwareCount.Text)));
                command.Parameters.Add(new SqlParameter("@InvoiceNumber", softwareInvoice.Text));
                command?.ExecuteNonQuery();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateSoftware()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = null;
                command = new SqlCommand("UpdateLicenseSoftware", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(new SqlParameter("@ID", Accounting.SoftwareID));
                command.Parameters.Add(new SqlParameter("@Name", softwareName.Text));
                command.Parameters.Add(new SqlParameter("@Price", Convert.ToDecimal(softwareCost.Text)));
                command.Parameters.Add(new SqlParameter("@Count", Convert.ToInt32(softwareCount.Text)));
                command.Parameters.Add(new SqlParameter("@InvoiceNumber", softwareInvoice.Text));
                command.Parameters.Add(new SqlParameter("@Type", ((DataRowView) typeLicense.SelectedItem).Row[0]));
                command?.ExecuteNonQuery();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateOS()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = null;
                command = new SqlCommand("UpdateOS", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(new SqlParameter("@ID", Accounting.SoftwareID));
                command.Parameters.Add(new SqlParameter("@Name", softwareName.Text));
                command.Parameters.Add(new SqlParameter("@Price", Convert.ToDecimal(softwareCost.Text)));
                command.Parameters.Add(new SqlParameter("@Count", Convert.ToInt32(softwareCount.Text)));
                command.Parameters.Add(new SqlParameter("@InvoiceNumber", softwareInvoice.Text));
                command?.ExecuteNonQuery();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void GetLicenseSoftware(int id)
        {
            string commandString;
            SqlDataReader reader;
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = $"SELECT * FROM dbo.GetLicenseSoftwareByID({id})";
                reader = new SqlCommand(commandString, connection).ExecuteReader();
                if (reader.HasRows)
                    if (reader.Read())
                    {
                        soft = new LicenseSoftware
                        {
                            Name = reader["Name"].ToString(),
                            Cost = Convert.ToDecimal(!(reader["Price"] is DBNull) ? reader["Price"] : 0),
                            Count = Convert.ToInt32(!(reader["Count"] is DBNull) ? reader["Count"] : 0),
                            InvoiceNumber = reader["InvoiceNumber"].ToString(),
                            Type = Convert.ToInt32(reader["Type"])
                        };

                        softwareName.Text = soft.Name;
                        softwareCost.Text = soft.Cost.ToString();
                        softwareCount.Text = soft.Count.ToString();
                        softwareInvoice.Text = soft.InvoiceNumber;
                        foreach (var obj in typeLicense.ItemsSource)
                        {
                            var row = obj as DataRowView;
                            if (Convert.ToUInt32(row?.Row[0]) == ((LicenseSoftware) soft).Type)
                            {
                                typeLicense.SelectedItem = row;
                                break;
                            }
                        }
                    }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void GetOS(int id)
        {
            string commandString;
            SqlDataReader reader;
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = $"SELECT * FROM dbo.GetOSByID({id})";
                reader = new SqlCommand(commandString, connection).ExecuteReader();
                if (reader.HasRows)
                    if (reader.Read())
                    {
                        soft = new OS
                        {
                            Name = reader["Name"].ToString(),
                            Cost = Convert.ToDecimal(!(reader["Price"] is DBNull) ? reader["Price"] : 0),
                            Count = Convert.ToInt32(!(reader["Count"] is DBNull) ? reader["Count"] : 0),
                            InvoiceNumber = reader["InvoiceNumber"].ToString()
                        };

                        softwareName.Text = soft.Name;
                        softwareCost.Text = soft.Cost.ToString();
                        softwareCount.Text = soft.Count.ToString();
                        softwareInvoice.Text = soft.InvoiceNumber;
                    }
            }
        }

        private void AutoInvN_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    inventoryNumber.Text = new SqlCommand("SELECT dbo.GetNextInventoryNumber()", connection).ExecuteScalar()
                        .ToString();
                    inventoryNumber.IsEnabled = false;
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, exception.GetType().Name, MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void AutoInvN_Unchecked(object sender, RoutedEventArgs e)
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
            try
            {
                DisabledRepeatInvN_Checked();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, exception.GetType().Name, MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void DisabledRepeatInvN_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                DisabledRepeatInvN_Unchecked();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, exception.GetType().Name, MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void CloseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Accounting.IsHitTestVisible = true;
            try
            {
                switch (Accounting.NowView)
                {
                    case View.Equipment:
                        Accounting.UpdateEquipmentData();
                        Accounting.UpdateImages();
                        Accounting.ChangeEquipmentView();
                        break;
                    case View.Software:
                        Accounting.UpdateSoftwareData();
                        Accounting.ChangeSoftwareView();
                        break;
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, exception.GetType().Name, MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }

            Close();
        }

        private void Cpu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var row = ((DataRowView) cpu.SelectedItem)?.Row;
                frequency.Text = row?[1].ToString();
                maxFrequency.Text = row?[2].ToString();
                cores.Text = row?[3].ToString();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, exception.GetType().Name, MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void ImageLoad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = new OpenFileDialog
                {
                    Filter = "Image Files(*.BMP;*.PNG;*.JPG;*.GIF)|*.BMP;*.PNG;*.JPG;*.GIF"
                };
                if (dialog.ShowDialog() == false) return;
                imageFilename.Text = dialog.FileName;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, exception.GetType().Name, MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void SaveChangesForSoftware(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveOrUpdateSoftwareDB();
                Accounting.UpdateSoftwareData();
                Accounting.ChangeSoftwareView();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, exception.GetType().Name, MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void SaveChanges(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveOrUpdateEquipmentDB();
                Accounting.UpdateEquipmentData();
                Accounting.UpdateImages();
                Accounting.ChangeEquipmentView();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, exception.GetType().Name, MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }

            if (Accounting.TypeChange == TypeChange.Change) CloseCommand.Execute(this, sender as UIElement);
            else ChangeView();
        }

        private void MoveWindow(object sender, MouseButtonEventArgs e)
        {
            DragMove();
            if (Accounting != null)
            {
                Accounting.Left = Left - (Accounting.Width - Width) / 2;
                double t = Top - (Accounting.Height - Height) / 2;
                Accounting.Top = t < 0 ? 0 : t;
            }
        }

        private void Window_DragOver(object sender, DragEventArgs e)
        {
            if (Accounting != null)
            {
                Accounting.Left = Left - (Accounting.Width - Width) / 2;
                double t = Top - (Accounting.Height - Height) / 2;
                Accounting.Top = t < 0 ? 0 : t;
            }
        }
    }
}