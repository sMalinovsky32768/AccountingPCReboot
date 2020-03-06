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

namespace AccountingPC
{
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
        SqlDataAdapter adapter;
        DataSet set;
        TypeDevice typeDevice;

        public AccountingPCWindow()
        {
            InitializeComponent();
            lastHeight = Height;
            lastWidth = Width;
            list.SelectedIndex = 0;
            UpdateDataGrid();
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
            DragMove();// Для перемещение ока
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
            UpdateDataGrid();
        }

        private void AddDevice(object sender, RoutedEventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
            }
            new ChangeDeviceWindow(typeDevice, TypeChange.Add).ShowDialog();
            UpdateDataGrid();
        }

        private void ChangeDevice(object sender, RoutedEventArgs e)
        {
            DataRow row = ((DataRowView)view.SelectedItem).Row;
            int id = Convert.ToInt32(row[0]);
            //new ChangeDeviceWindow(typeDevice, TypeChange.Change, id).ShowDialog();
            UpdateDataGrid();
        }

        private void DeleteDevice(object sender, RoutedEventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                foreach (object obj in view.SelectedItems)
                {
                    DataRow row = ((DataRowView)obj).Row;
                    int id = Convert.ToInt32(row[0]);
                    SqlCommand command = new SqlCommand($"Delete{typeDevice.ToString()}ByID", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@ID", id));
                    int res = command.ExecuteNonQuery();
                }
            }
            UpdateDataGrid();
        }

        private void UpdateDataGrid()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
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
    }
}
