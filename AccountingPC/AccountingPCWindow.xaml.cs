using AccountingPC.Properties;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Data;
using AccountingPC.AccountingReport;
using System.Linq;

namespace AccountingPC
{
    /// <summary>
    /// Логика взаимодействия для AccountingPCWindow.xaml
    /// </summary>
    public partial class AccountingPCWindow : Window
    {

        public static String ConnectionString { get; private set; } = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public static readonly RoutedCommand ParametersCommand = new RoutedUICommand(
            "Parameters", "ParametersCommand", typeof(AccountingPCWindow),
            new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.F12) }));

        public static readonly RoutedCommand ExitCommand = new RoutedUICommand(
            "Exit", "ExitCommand", typeof(AccountingPCWindow),
            new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.F4, ModifierKeys.Alt) }));

        public static readonly RoutedCommand PopupCloseCommand = new RoutedUICommand(
            "PopupClose", "PopupCloseCommand", typeof(AccountingPCWindow),
            new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.Escape) }));

        public static readonly RoutedCommand SelectViewEquipmentCommand = new RoutedUICommand(
            "SelectViewEquipment", "SelectViewEquipmentCommand", typeof(AccountingPCWindow),
            new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.E, ModifierKeys.Alt) }));

        public static readonly RoutedCommand SelectViewSoftwareCommand = new RoutedUICommand(
            "SelectViewSoftware", "SelectViewSoftwareCommand", typeof(AccountingPCWindow),
            new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.S, ModifierKeys.Alt) }));

        public static readonly RoutedCommand SelectViewInvoiceCommand = new RoutedUICommand(
            "SelectViewInvoice", "SelectViewInvoiceCommand", typeof(AccountingPCWindow),
            new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.I, ModifierKeys.Alt) }));

        public static readonly RoutedCommand SelectViewLocationCommand = new RoutedUICommand(
            "SelectViewLocation", "SelectViewLocationCommand", typeof(AccountingPCWindow),
            new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.L, ModifierKeys.Alt) }));


        public static readonly RoutedCommand UpdateViewCommand = new RoutedUICommand(
            "UpdateView", "UpdateViewCommand", typeof(AccountingPCWindow),
            new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.F5), new KeyGesture(Key.R, ModifierKeys.Control) }));

        public ChangeWindow changeWindow;

        internal View NowView { get; set; }
        internal TypeDevice TypeDevice { get; set; }
        internal TypeSoft TypeSoft { get; set; }
        internal TypeChange TypeChange { get; set; }
        internal int DeviceID { get; set; }
        internal int SoftwareID { get; set; }
        public bool IsPreOpenEquipmentPopup { get; set; } = false;
        public bool IsPreOpenSoftwarePopup { get; set; } = false;

        internal Dictionary<int, byte[]> images;

        List<InstalledSoftware> pcSoftware;
        List<InstalledSoftware> notebookSoftware;
        List<InstalledSoftware> pcNotInstalledSoftware;
        List<InstalledSoftware> notebookNotInstalledSoftware;

        public double lastHeight;
        public double lastWidth;

        SqlDataAdapter softwareDataAdapter;
        SqlDataAdapter pcNotInstalledSoftwareDataAdapter;
        SqlDataAdapter pcSoftwareDataAdapter;
        SqlDataAdapter notebookNotInstalledSoftwareDataAdapter;
        SqlDataAdapter notebookSoftwareDataAdapter;
        SqlDataAdapter osDataAdapter;
        SqlDataAdapter boardDataAdapter;
        SqlDataAdapter monitorDataAdapter;
        SqlDataAdapter networkSwitchDataAdapter;
        SqlDataAdapter notebookDataAdapter;
        SqlDataAdapter otherEquipmentDataAdapter;
        SqlDataAdapter pcDataAdapter;
        SqlDataAdapter printerScannerDataAdapter;
        SqlDataAdapter projectorDataAdapter;
        SqlDataAdapter projectorScreenDataAdapter;
        //SqlDataAdapter typeDeviceDataAdapter;

        DataSet softwareDataSet;
        DataSet pcNotInstalledSoftwareDataSet;
        DataSet pcSoftwareDataSet;
        DataSet notebookNotInstalledSoftwareDataSet;
        DataSet notebookSoftwareDataSet;
        DataSet osDataSet;
        DataSet boardDataSet;
        DataSet monitorDataSet;
        DataSet networkSwitchDataSet;
        DataSet notebookDataSet;
        DataSet otherEquipmentDataSet;
        DataSet pcDataSet;
        DataSet printerScannerDataSet;
        DataSet projectorDataSet;
        DataSet projectorScreenDataSet;
        //DataSet typeDeviceDataSet;

        static AccountingPCWindow()
        {
            //AvailableInvNProperty = DependencyProperty.Register("AvailableInvN", typeof(bool), typeof(AccountingPCWindow));
        }
        public AccountingPCWindow()
        {
            InitializeComponent();
            lastHeight = Height;
            lastWidth = Width;
            UpdateAllEquipmentData();
            UpdateAllSoftwareData();
            UpdateImages();
            //equipmentCategoryList.SelectedIndex = 0;
            IsPreOpenEquipmentPopup = false;
            NowView = View.Equipment;
            // Создание меню отчетов
            reportMenu.ItemContainerGenerator.StatusChanged += ItemContainerGenerator_StatusChanged;
            reportMenu.ItemsSource = ReportNameCollection.Collection;
            reportMenu.DisplayMemberPath = "Name";
            //if (reportMenu.ItemContainerGenerator.Status ==
            //    System.Windows.Controls.Primitives.GeneratorStatus.ContainersGenerated)
            //{
            //    var containers = reportMenu.Items.Cast<object>().Select(
            //        item => (FrameworkElement)reportMenu.ItemContainerGenerator.ContainerFromItem(item));
            //    foreach (var container in containers)
            //    {
            //        (container as MenuItem).Click += CreateReport_Click;
            //    }
            //}

            //softwareCategoryList.SelectedIndex = 0;
            //equipmentCategoryList.SelectedIndex = 0;
        }

        private void ItemContainerGenerator_StatusChanged(object sender, EventArgs e)
        {
            if (reportMenu.ItemContainerGenerator.Status ==
                System.Windows.Controls.Primitives.GeneratorStatus.ContainersGenerated)
            {
                var containers = reportMenu.Items.Cast<object>().Select(
                    item => (FrameworkElement)reportMenu.ItemContainerGenerator.ContainerFromItem(item));
                foreach (var container in containers)
                {
                    (container as MenuItem).Click += CreateReport_Click;
                }
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //ChangePopupPreClose();
            //ChangePopupPostClose();
            if (changeWindow != null)
            {
                changeWindow.Left = Left - (changeWindow.Width - Width) / 2;
                changeWindow.Top = Top - (changeWindow.Height - Height) / 2;
            }
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
            DragMove();

            if (changeWindow != null)
            {
                changeWindow.Left = Left - (changeWindow.Width - Width) / 2;
                changeWindow.Top = Top - (changeWindow.Height - Height) / 2;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadFromSettings();
            ChangeWindowState();
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
            DataRow row = ((DataRowView)equipmentView.SelectedItem).Row;
            DeviceID = Convert.ToInt32(row[0]);
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
                    Int32 id = Convert.ToInt32(row[0]);
                    SqlCommand command = new SqlCommand($"Delete{TypeDevice.ToString()}ByID", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@ID", id));
                    Int32 res = command.ExecuteNonQuery();
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

        private void Window_LostFocus(object sender, RoutedEventArgs e)
        {
            //changePopupPreClose();
        }

        private void Window_GotFocus(object sender, RoutedEventArgs e)
        {
            //changePopupPostClose();
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            ChangePopupPreClose();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            ChangePopupPostClose();
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
            DataRow row = ((DataRowView)softwareView.SelectedItem).Row;
            DeviceID = Convert.ToInt32(row[0]);
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
                    Int32 id = Convert.ToInt32(row[0]);
                    SqlCommand command = new SqlCommand($"Delete{TypeSoft.ToString()}", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@ID", id));
                    Int32 res = command.ExecuteNonQuery();
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
                using (MemoryStream stream = new MemoryStream(images[id]))
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
                        command = new SqlCommand($"DeleteInstalledSoftwarePC", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@PCID", DeviceID));
                        command.Parameters.Add(new SqlParameter("@LicenseID", licenseID));
                        break;
                    case TypeDevice.Notebook:
                        command = new SqlCommand($"DeleteInstalledSoftwareNotebook", connection);
                        command.CommandType = CommandType.StoredProcedure;
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
                SqlCommand command = null;
                switch (TypeDevice)
                {
                    case TypeDevice.PC:
                        command = new SqlCommand($"AddInstalledSoftwarePC", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@PCID", DeviceID));
                        command.Parameters.Add(new SqlParameter("@LicenseID", licenseID));
                        break;
                    case TypeDevice.Notebook:
                        command = new SqlCommand($"AddInstalledSoftwareNotebook", connection);
                        command.CommandType = CommandType.StoredProcedure;
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
            //System.Windows.Forms.SaveFileDialog dialog = new System.Windows.Forms.SaveFileDialog();
            //dialog.Filter = "Excel | *.xlsx;*.xls";
            //dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            //dialog.FileName = $"Report_{DateTime.Now.ToString("dd-MM-yyyy__HH-mm-ss__g")}.xlsx";
            //if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
            //    return;
            //Report.SaveReport(dialog.FileName);
            ReportName reportName = ((sender as MenuItem).Header as ReportName);
            ConfiguringReportWindow reportWindow;
            if (reportName != null)
            {
                reportWindow = new ConfiguringReportWindow(reportName.Type);
                reportWindow.Owner = this;
            }
            else
            {
                reportWindow = new ConfiguringReportWindow(reportName.Type);
                reportWindow.Owner = this;
            }
            reportWindow?.ShowDialog();
        }

        private void OpenParameters(object sender, ExecutedRoutedEventArgs e)
        {
            //new ParametersWindow().ShowDialog();
            ParametersWindow parametersWindow = new ParametersWindow();
            parametersWindow.Owner = this;
            parametersWindow.ShowDialog();
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

        private void softwareView_AutoGeneratedColumns(object sender, EventArgs e)
        {
            softwareView.Columns[((DataView)softwareView.ItemsSource).Table.Columns.IndexOf("ID")].Visibility = Visibility.Collapsed;
            ((DataGridTextColumn)softwareView.Columns[((DataView)softwareView.ItemsSource).Table.
                Columns.IndexOf("Дата приобретения")]).Binding.StringFormat = "dd.MM.yyyy";
        }

        private void equipmentView_AutoGeneratedColumns(object sender, EventArgs e)
        {
            equipmentView.Columns[((DataView)equipmentView.ItemsSource).Table.Columns.IndexOf("ID")].Visibility = Visibility.Collapsed;
            if (((DataView)equipmentView.ItemsSource).Table.Columns.Contains("VideoConnectors"))
                equipmentView.Columns[((DataView)equipmentView.ItemsSource).Table.Columns.IndexOf("VideoConnectors")].Visibility = Visibility.Collapsed;
            equipmentView.Columns[((DataView)equipmentView.ItemsSource).Table.Columns.IndexOf("ImageID")].Visibility = Visibility.Collapsed;
            ((DataGridTextColumn)equipmentView.Columns[((DataView)equipmentView.ItemsSource).Table.
                Columns.IndexOf("Дата приобретения")]).Binding.StringFormat = "dd.MM.yyyy";
        }
    }
}
