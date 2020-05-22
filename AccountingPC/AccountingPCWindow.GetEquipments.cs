using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Controls;

namespace AccountingPC
{
    public partial class AccountingPCWindow
    {
        private void GetPC(String connectionString, Device d, int id)
        {
            String commandString;
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(connectionString))
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

        private void GetNotebook(String connectionString, Device d, int id)
        {
            String commandString;
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(connectionString))
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

        private void GetMonitor(String connectionString, Device d, int id)
        {
            String commandString;
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(connectionString))
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

        private void GetProjector(String connectionString, Device d, int id)
        {
            String commandString;
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(connectionString))
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

        private void GetInteractiveWhiteboard(String connectionString, Device d, int id)
        {
            String commandString;
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(connectionString))
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

        private void GetProjectorScreen(String connectionString, Device d, int id)
        {
            String commandString;
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(connectionString))
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

        private void GetPrinterScanner(String connectionString, Device d, int id)
        {
            String commandString;
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(connectionString))
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

        private void GetNetworkSwitch(String connectionString, Device d, int id)
        {
            String commandString;
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(connectionString))
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

        private void GetOtherEquipment(String connectionString, Device d, int id)
        {
            String commandString;
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(connectionString))
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
    }
}
