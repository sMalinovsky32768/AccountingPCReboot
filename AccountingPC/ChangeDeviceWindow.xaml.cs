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

namespace AccountingPC
{
    public enum TypeChange
    {
        Change,
        Add,
    }
    /// <summary>
    /// Логика взаимодействия для ChangeDeviceWindow.xaml
    /// </summary>
    public partial class ChangeDeviceWindow : Window
    {
        TypeDevice typeDevice;
        TypeChange typeChange;
        int DeviceID;
        SqlDataAdapter adapter;
        DataSet set;

        public ChangeDeviceWindow(TypeDevice typeD, TypeChange change, int ID)
        {
            InitializeComponent();
            string commandString;
            typeDevice = typeD;
            typeChange = change;
            DeviceID = ID;
            SqlDataReader reader;
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            if (typeChange == TypeChange.Change)
            {
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
                                    uint invN = Convert.ToUInt32(reader["Инвентарный номер"]);
                                    string n = reader["Наименование"].ToString();
                                    uint cost = Convert.ToUInt32(reader["Цена"]);
                                    string mb = reader["Материнская плата"].ToString();
                                    string cpu = reader["Процессор"].ToString();
                                    uint ram = Convert.ToUInt32(reader["ОЗУ"]);
                                    string os = reader["Операционная система"].ToString();
                                    string invoice = reader["Номер накладной"].ToString();
                                    string location = reader["Расположение"].ToString();
                                    pc = new PC(invN, n, cost, mb, cpu, ram, os, invoice, location);
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
            }
        }

        public ChangeDeviceWindow(TypeDevice typeD, TypeChange change)
        {
            InitializeComponent();
            string commandString;
            typeDevice = typeD;
            typeChange = change;
            SqlDataReader reader;
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            if (typeChange == TypeChange.Add)
            {
                
            }
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();// Для перемещение ока
        }

        private void SaveChanges(object sender, RoutedEventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string commandString = $"Update {typeDevice} set InventoryNumber = {Convert.ToUInt32(inventoryNumberPC.Text)}, " +
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
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
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
    }
}
