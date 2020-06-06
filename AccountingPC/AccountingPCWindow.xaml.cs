﻿using AccountingPC.AccountingReport;
using AccountingPC.Properties;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace AccountingPC
{
    /// <summary>
    /// Логика взаимодействия для AccountingPCWindow.xaml
    /// </summary>
    public partial class AccountingPCWindow : Window
    {
        public static string ConnectionString { get; private set; } = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public static readonly RoutedUICommand ParametersCommand;
        public static readonly RoutedUICommand ExitCommand;
        public static readonly RoutedUICommand PopupCloseCommand;
        public static readonly RoutedUICommand SelectViewEquipmentCommand;
        public static readonly RoutedUICommand SelectViewSoftwareCommand;
        public static readonly RoutedUICommand SelectViewInvoiceCommand;
        public static readonly RoutedUICommand SelectViewLocationCommand;
        public static readonly RoutedUICommand UpdateViewCommand;

        static AccountingPCWindow()
        {
            ParametersCommand = new RoutedUICommand(
            "Parameters", "ParametersCommand", typeof(AccountingPCWindow),
            new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.F12) }));

            ExitCommand = new RoutedUICommand(
            "Exit", "ExitCommand", typeof(AccountingPCWindow),
            new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.F4, ModifierKeys.Alt) }));

            PopupCloseCommand = new RoutedUICommand(
            "PopupClose", "PopupCloseCommand", typeof(AccountingPCWindow),
            new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.Escape) }));

            SelectViewEquipmentCommand = new RoutedUICommand(
            "SelectViewEquipment", "SelectViewEquipmentCommand", typeof(AccountingPCWindow),
            new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.E, ModifierKeys.Alt) }));

            SelectViewSoftwareCommand = new RoutedUICommand(
            "SelectViewSoftware", "SelectViewSoftwareCommand", typeof(AccountingPCWindow),
            new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.S, ModifierKeys.Alt) }));

            SelectViewInvoiceCommand = new RoutedUICommand(
            "SelectViewInvoice", "SelectViewInvoiceCommand", typeof(AccountingPCWindow),
            new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.I, ModifierKeys.Alt) }));

            SelectViewLocationCommand = new RoutedUICommand(
            "SelectViewLocation", "SelectViewLocationCommand", typeof(AccountingPCWindow),
            new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.L, ModifierKeys.Alt) }));


            UpdateViewCommand = new RoutedUICommand(
            "UpdateView", "UpdateViewCommand", typeof(AccountingPCWindow),
            new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.F5), new KeyGesture(Key.R, ModifierKeys.Control) }));

        }

        public ChangeWindow changeWindow;

        internal View NowView { get; set; }
        internal TypeDevice TypeDevice { get; set; }
        internal TypeSoft TypeSoft { get; set; }
        internal TypeChange TypeChange { get; set; }
        internal int DeviceID { get; set; }
        internal int SoftwareID { get; set; }
        internal int InvoiceID { get; set; }
        internal int AudienceID { get; set; }
        internal int AudienceTableID { get; set; }
        public bool IsPreOpenEquipmentPopup { get; set; } = false;
        public bool IsPreOpenSoftwarePopup { get; set; } = false;

        internal List<InstalledSoftware> PcSoftware { get; set; }
        internal List<InstalledSoftware> NotebookSoftware { get; set; }
        internal List<InstalledSoftware> PcNotInstalledSoftware { get; set; }
        internal List<InstalledSoftware> NotebookNotInstalledSoftware { get; set; }

        public DataSet DefaultDataSet { get; private set; }

        //public DataSet SoftwareDataSet { get; set; }
        //public DataSet PcNotInstalledSoftwareDataSet { get; set; }
        //public DataSet PcSoftwareDataSet { get; set; }
        //public DataSet NotebookNotInstalledSoftwareDataSet { get; set; }
        //public DataSet NotebookSoftwareDataSet { get; set; }
        //public DataSet OsDataSet { get; set; }
        //public DataSet BoardDataSet { get; set; }
        //public DataSet MonitorDataSet { get; set; }
        //public DataSet NetworkSwitchDataSet { get; set; }
        //public DataSet NotebookDataSet { get; set; }
        //public DataSet OtherEquipmentDataSet { get; set; }
        //public DataSet PcDataSet { get; set; }
        //public DataSet PrinterScannerDataSet { get; set; }
        //public DataSet ProjectorDataSet { get; set; }
        //public DataSet ProjectorScreenDataSet { get; set; }
        //public DataSet InvoiceDataSet { get; set; }
        //public DataSet AudienceDataSet { get; set; }
        //public DataSet AudiencePlaceDataSet { get; set; }
        //public DataSet DeviceOnPlaceDataSet { get; set; }
        public DataSet InvoiceSoftwareAndEquipmentDataSet { get; set; } // Независимый DataSet для накладных
        internal Dictionary<int, byte[]> Images { get; set; }

        public double lastHeight;
        public double lastWidth;

        public AccountingPCWindow()
        {
            DefaultDataSet = new DataSet("Default DataSet");
            DefaultDataSet.Tables.Add("Software");
            DefaultDataSet.Tables.Add("PCNotInstalledSoftware");
            DefaultDataSet.Tables.Add("PCSoftware");
            DefaultDataSet.Tables.Add("NotebookNotInstalledSoftware");
            DefaultDataSet.Tables.Add("NotebookSoftware");
            DefaultDataSet.Tables.Add("OS");
            DefaultDataSet.Tables.Add("Board");
            DefaultDataSet.Tables.Add("Monitor");
            DefaultDataSet.Tables.Add("NetworkSwitch");
            DefaultDataSet.Tables.Add("Notebook");
            DefaultDataSet.Tables.Add("OtherEquipment");
            DefaultDataSet.Tables.Add("PC");
            DefaultDataSet.Tables.Add("PrinterScanner");
            DefaultDataSet.Tables.Add("Projector");
            DefaultDataSet.Tables.Add("ProjectorScreen");
            DefaultDataSet.Tables.Add("Invoice");
            DefaultDataSet.Tables.Add("Audience");
            DefaultDataSet.Tables.Add("AudiencePlace");
            DefaultDataSet.Tables.Add("DeviceOnPlace");

            InitializeComponent();
            lastHeight = Height;
            lastWidth = Width;
            UpdateAllEquipmentData();
            UpdateAllSoftwareData();
            UpdateImages();

            IsPreOpenEquipmentPopup = false;
            NowView = View.Equipment;

            // Создание меню отчетов
            reportMenu.ItemContainerGenerator.StatusChanged += ItemContainerGenerator_StatusChanged;
            reportMenu.ItemsSource = ReportNameCollection.Collection;
            reportMenu.DisplayMemberPath = "Name";
        }

        private void ItemContainerGenerator_StatusChanged(object sender, EventArgs e)
        {
            if (reportMenu.ItemContainerGenerator.Status ==
                System.Windows.Controls.Primitives.GeneratorStatus.ContainersGenerated)
            {
                IEnumerable<FrameworkElement> containers = reportMenu.Items.Cast<object>().Select(
                    item => (FrameworkElement)reportMenu.ItemContainerGenerator.ContainerFromItem(item));
                foreach (FrameworkElement container in containers)
                {
                    (container as MenuItem).Click += CreateReport_Click;
                }
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ChangeWindowState();
            if (changeWindow != null)
            {
                changeWindow.Left = Left - (changeWindow.Width - Width) / 2;
                changeWindow.Top = Top - (changeWindow.Height - Height) / 2;
            }
        }

        private void ExitApp(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource.GetType() == typeof(MenuItem))
            {
                if (((MenuItem)e.OriginalSource).Name == "menuExit")
                {
                    if (Settings.Default.SHUTDOWN_ON_EXPLICIT)
                    {
                        App.Current.Shutdown();
                    }
                    else
                    {
                        Close();
                    }
                }
                else { }
            }
            else if (e.OriginalSource.GetType() == typeof(Button))
            {
                if (((Button)e.OriginalSource).Name == "buttonExit")
                {
                    Close();
                }
                else { }
            }
            else
            {
                Close();
            }
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
            ChangeWindowState();
            if (changeWindow != null)
            {
                changeWindow.Left = Left - (changeWindow.Width - Width) / 2;
                changeWindow.Top = Top - (changeWindow.Height - Height) / 2;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadFromSettings();
            //ChangeWindowState();
            SelectViewEquipment();
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
            switch (NowView)
            {
                case View.Equipment:
                    ChangeEquipmentView();
                    break;
                case View.Software:
                    ChangeSoftwareView();
                    break;
                case View.Location:
                    AudienceID = Convert.ToInt32(((DataRowView)audienceList?.SelectedItem)?.Row?["ID"]);
                    ChangeLocationView();
                    break;
                case View.Invoice:
                    InvoiceID = Convert.ToInt32(((DataRowView)invoiceList?.SelectedItem)?.Row?["ID"]);
                    ChangeInvoiceView();
                    break;
            }
        }

        private void AddDevice(object sender, RoutedEventArgs e)
        {
            TypeChange = TypeChange.Add;
            //changeEquipmentPopup.IsOpen = true;
            OpenChangeWindow();
        }

        private void ChangeDevice(object sender, RoutedEventArgs e)
        {
            //DataRow row = ((DataRowView)equipmentView.SelectedItem).Row;
            //DeviceID = Convert.ToInt32(row[0]);
            TypeChange = TypeChange.Change;
            //changeEquipmentPopup.IsOpen = true;
            OpenChangeWindow();
        }

        private void DeleteDevice(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                foreach (object obj in equipmentView.SelectedItems)
                {
                    DataRow row = ((DataRowView)obj).Row;
                    int id = Convert.ToInt32(row[0]);
                    SqlCommand command = new SqlCommand($"Delete{TypeDevice.ToString()}ByID", connection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    command.Parameters.Add(new SqlParameter("@ID", id));
                    int res = command.ExecuteNonQuery();
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
            UpdateEquipmentData();
        }

        private void SelectViewEquipment(object sender, ExecutedRoutedEventArgs e)
        {
            SelectViewEquipment();
        }

        private void SelectViewSoftware(object sender, ExecutedRoutedEventArgs e)
        {
            SelectViewSoftware();
        }

        private void SelectViewInvoice(object sender, ExecutedRoutedEventArgs e)
        {
            SelectViewInvoice();
        }

        private void SelectViewLocation(object sender, ExecutedRoutedEventArgs e)
        {
            SelectViewLocation();
        }

        private void AddSoftware(object sender, RoutedEventArgs e)
        {
            TypeChange = TypeChange.Add;
            //changeSoftwarePopup.IsOpen = true;
            OpenChangeWindow();
        }

        private void ChangeSoftware(object sender, RoutedEventArgs e)
        {
            //DataRow row = ((DataRowView)softwareView.SelectedItem).Row;
            //DeviceID = Convert.ToInt32(row[0]);
            TypeChange = TypeChange.Change;
            //changeSoftwarePopup.IsOpen = true;
            OpenChangeWindow();
        }

        private void DeleteSoftware(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                foreach (object obj in softwareView.SelectedItems)
                {
                    DataRow row = ((DataRowView)obj).Row;
                    int id = Convert.ToInt32(row[0]);
                    SqlCommand command = new SqlCommand($"Delete{TypeSoft.ToString()}", connection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    command.Parameters.Add(new SqlParameter("@ID", id));
                    int res = command.ExecuteNonQuery();
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
            UpdateSoftwareData();
        }

        private void EquipmentView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DeviceID = Convert.ToInt32(((DataRowView)equipmentView?.SelectedItem)?.Row?[0]);
            BitmapFrame frame = null;
            object obj = equipmentView.SelectedItems.Count > 0 ? (((DataRowView)equipmentView.SelectedItems?[0])?.Row["ImageID"]) : 0;
            int id = Convert.ToInt32(obj.GetType() == typeof(DBNull) ? 0 : obj);
            if (id != 0)
            {
                using (MemoryStream stream = new MemoryStream(Images[id]))
                {
                    frame = BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                }
            }
            deviceImage.Source = frame;
            UpdateSoftwareOnDevice();
        }

        private void UpdateView_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            switch (NowView)
            {
                case View.Equipment:
                    UpdateAllEquipmentData();
                    ChangeEquipmentView();
                    UpdateImages();
                    break;
                case View.Software:
                    UpdateAllSoftwareData();
                    ChangeSoftwareView();
                    break;
                case View.Location:
                    break;
            }
        }

        private void AddSoftware_Click(object sender, RoutedEventArgs e)
        {
            ((Button)sender).ContextMenu.IsOpen = true;
        }

        private void DelSoftware_Click(object sender, RoutedEventArgs e)
        {
            int licenseID = ((InstalledSoftware)softwareOnDevice.SelectedItem).ID;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = null;
                switch (TypeDevice)
                {
                    case TypeDevice.PC:
                        command = new SqlCommand($"DeleteInstalledSoftwarePC", connection)
                        {
                            CommandType = CommandType.StoredProcedure
                        };
                        command.Parameters.Add(new SqlParameter("@PCID", DeviceID));
                        command.Parameters.Add(new SqlParameter("@LicenseID", licenseID));
                        break;
                    case TypeDevice.Notebook:
                        command = new SqlCommand($"DeleteInstalledSoftwareNotebook", connection)
                        {
                            CommandType = CommandType.StoredProcedure
                        };
                        command.Parameters.Add(new SqlParameter("@NotebookID", DeviceID));
                        command.Parameters.Add(new SqlParameter("@LicenseID", licenseID));
                        break;
                }
                command?.ExecuteNonQuery();
            }
            UpdateSoftwareOnDevice();
        }

        private void AddSoftwareItem(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            InstalledSoftware software = (InstalledSoftware)button.DataContext;
            int licenseID = software.ID;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = null;
                switch (TypeDevice)
                {
                    case TypeDevice.PC:
                        command = new SqlCommand($"AddInstalledSoftwarePC", connection)
                        {
                            CommandType = CommandType.StoredProcedure
                        };
                        command.Parameters.Add(new SqlParameter("@PCID", DeviceID));
                        command.Parameters.Add(new SqlParameter("@LicenseID", licenseID));
                        break;
                    case TypeDevice.Notebook:
                        command = new SqlCommand($"AddInstalledSoftwareNotebook", connection)
                        {
                            CommandType = CommandType.StoredProcedure
                        };
                        command.Parameters.Add(new SqlParameter("@NotebookID", DeviceID));
                        command.Parameters.Add(new SqlParameter("@LicenseID", licenseID));
                        break;
                }
                command?.ExecuteNonQuery();
            }
            UpdateSoftwareOnDevice();
        }

        private void SoftwareView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SoftwareID = Convert.ToInt32(((DataRowView)softwareView?.SelectedItem)?.Row?[0]);
        }

        private void CreateReport_Click(object sender, RoutedEventArgs e)
        {
            ReportName reportName = ((sender as MenuItem).Header as ReportName);
            ConfiguringReportWindow reportWindow;
            if (reportName != null)
            {
                reportWindow = new ConfiguringReportWindow(reportName.Type)
                {
                    Owner = this
                };
            }
            else
            {
                reportWindow = new ConfiguringReportWindow
                {
                    Owner = this
                };
            }
            reportWindow?.ShowDialog();
        }

        private void OpenParameters(object sender, ExecutedRoutedEventArgs e)
        {
            //new ParametersWindow().ShowDialog();
            ParametersWindow parametersWindow = new ParametersWindow
            {
                Owner = this
            };
            bool? q = parametersWindow.ShowDialog();
            //if (q == true) ChangeWindowState();
        }

        private void OpenChangeWindow()
        {
            changeWindow = new ChangeWindow(this);
            //changeWindow.Owner = this;
            changeWindow.Show();
        }

        private void Window_DragOver(object sender, DragEventArgs e)
        {
            if (changeWindow != null)
            {
                changeWindow.Left = Left - (changeWindow.Width - Width) / 2;
                changeWindow.Top = Top - (changeWindow.Height - Height) / 2;
            }
        }

        private void SoftwareView_AutoGeneratedColumns(object sender, EventArgs e)
        {
            softwareView.Columns[((DataView)softwareView.ItemsSource).Table.Columns.IndexOf("ID")].Visibility = Visibility.Collapsed;

            softwareView.Columns[((DataView)softwareView.ItemsSource).Table.Columns.IndexOf("InvoiceID")].Visibility = Visibility.Collapsed;

            ((DataGridTextColumn)softwareView.Columns[((DataView)softwareView.ItemsSource).Table.
                Columns.IndexOf("Дата приобретения")]).Binding.StringFormat = "dd.MM.yyyy";
        }

        private void EquipmentView_AutoGeneratedColumns(object sender, EventArgs e)
        {
            equipmentView.Columns[((DataView)equipmentView.ItemsSource).Table.Columns.IndexOf("ID")].Visibility = Visibility.Collapsed;

            equipmentView.Columns[((DataView)equipmentView.ItemsSource).Table.Columns.IndexOf("InvoiceID")].Visibility = Visibility.Collapsed;

            if (((DataView)equipmentView.ItemsSource).Table.Columns.Contains("VideoConnectors"))
            {
                equipmentView.Columns[((DataView)equipmentView.ItemsSource).Table.Columns.IndexOf("VideoConnectors")].Visibility = Visibility.Collapsed;
            }

            equipmentView.Columns[((DataView)equipmentView.ItemsSource).Table.Columns.IndexOf("ImageID")].Visibility = Visibility.Collapsed;

            ((DataGridTextColumn)equipmentView.Columns[((DataView)equipmentView.ItemsSource).Table.
                Columns.IndexOf("Дата приобретения")]).Binding.StringFormat = "dd.MM.yyyy";
        }

        private void InvoiceView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid grid = (DataGrid)e.OriginalSource;
            string tableName = ((DataView)grid.ItemsSource).Table.TableName;
        }

        private void InvoiceView_AutoGeneratedColumns(object sender, EventArgs e)
        {
            DataGrid grid = sender as DataGrid;
            if (((DataView)grid.ItemsSource).Table.Columns.Contains("ID"))
            {
                grid.Columns[((DataView)grid.ItemsSource).Table.Columns.IndexOf("ID")].Visibility = Visibility.Collapsed;
            }

            if (((DataView)grid.ItemsSource).Table.Columns.Contains("InvoiceID"))
            {
                grid.Columns[((DataView)grid.ItemsSource).Table.Columns.IndexOf("InvoiceID")].Visibility = Visibility.Collapsed;
            }

            if (((DataView)grid.ItemsSource).Table.Columns.Contains("VideoConnectors"))
            {
                grid.Columns[((DataView)grid.ItemsSource).Table.Columns.IndexOf("VideoConnectors")].Visibility = Visibility.Collapsed;
            }

            if (((DataView)grid.ItemsSource).Table.Columns.Contains("ImageID"))
            {
                grid.Columns[((DataView)grid.ItemsSource).Table.Columns.IndexOf("ImageID")].Visibility = Visibility.Collapsed;
            }

            if (((DataView)grid.ItemsSource).Table.Columns.Contains("Дата приобретения"))
            {
                ((DataGridTextColumn)grid.Columns[((DataView)grid.ItemsSource).Table.
                    Columns.IndexOf("Дата приобретения")]).Binding.StringFormat = "dd.MM.yyyy";
            }
        }

        private void ChangeInvoice_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand("Update Invoice set Number=@Number, Date=@Date Where ID=@ID", connection);
                command.Parameters.AddWithValue("@Number", invoiceNumberManager.Text);
                command.Parameters.AddWithValue("@Date", $"{dateManager.Text:yyyy-MM-dd}");
                command.Parameters.AddWithValue("@ID", InvoiceID);
                command.ExecuteNonQuery();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            addAudienceGrid.Visibility = addAudienceGrid.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
            audienceName.Text = string.Empty;
        }

        private void AudienceTableView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRow row = ((DataRowView)audienceTableView?.SelectedItem)?.Row;
            AudienceTableID = Convert.ToInt32(row?["ID"]);
            //DeviceOnPlaceDataSet = new DataSet();
            //new SqlDataAdapter($"select * from dbo.[GetAllDevicesOnPlace]({AudienceTableID})", ConnectionString).Fill(DeviceOnPlaceDataSet);
            //devicesOnPlace.ItemsSource = DeviceOnPlaceDataSet.Tables[0].DefaultView;
            DefaultDataSet.Tables["DeviceOnPlace"].Clear();
            new SqlDataAdapter($"select * from dbo.[GetAllDevicesOnPlace]({AudienceTableID})", ConnectionString).Fill(DefaultDataSet, "DeviceOnPlace");
            devicesOnPlace.ItemsSource = DefaultDataSet.Tables["DeviceOnPlace"].DefaultView;
            devicesOnPlace.SelectedIndex = 0;
        }

        private void AudienceTableView_AutoGeneratedColumns(object sender, EventArgs e)
        {
            DataGrid grid = sender as DataGrid;
            if (((DataView)grid.ItemsSource).Table.Columns.Contains("ID"))
            {
                grid.Columns[((DataView)grid.ItemsSource).Table.Columns.IndexOf("ID")].Visibility = Visibility.Collapsed;
            }
        }

        private void DevicesOnPlace_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BitmapFrame frame = null;
            object obj = (((DataRowView)devicesOnPlace.SelectedItems?[0])?.Row["ImageID"]);
            int id = Convert.ToInt32(obj.GetType() == typeof(DBNull) ? 0 : obj);
            if (id != 0)
            {
                using (MemoryStream stream = new MemoryStream(Images[id]))
                {
                    frame = BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                }
            }
            deviceImageOnPlace.Source = frame;
        }
    }
}
