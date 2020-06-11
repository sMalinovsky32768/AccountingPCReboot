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
    /// <summary>
    /// Interaction logic for ChangePlaceWindow.xaml
    /// </summary>
    public partial class ChangePlaceWindow : Window
    {
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public static string ConnectionString => connectionString;
        public static readonly RoutedCommand CloseCommand = new RoutedUICommand(
            "Close", "CloseCommand", typeof(AccountingPCWindow),
            new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.Escape) }));

        public AccountingPCWindow Accounting { get; private set; }
        internal Place CurrentPlace { get; set; }
        private int AudienceID { get; set; }
        private int AudienceTableID { get; set; }

        public ChangePlaceWindow(AccountingPCWindow window)
        {
            CurrentPlace = new Place();
            InitializeComponent();
            Owner = window;
            Accounting = window;
            devicesOnPlace.ItemContainerGenerator.StatusChanged += ItemContainerGenerator_StatusChanged;
        }

        private void ItemContainerGenerator_StatusChanged(object sender, EventArgs e)
        {
            if (devicesOnPlace.ItemContainerGenerator.Status ==
                System.Windows.Controls.Primitives.GeneratorStatus.ContainersGenerated)
            {
                IEnumerable<FrameworkElement> containers = devicesOnPlace.Items.Cast<object>().Select(
                    item => (FrameworkElement)devicesOnPlace.ItemContainerGenerator.ContainerFromItem(item));
                foreach (FrameworkElement container in containers)
                {
                    container.Loaded += Container_Loaded;
                }
            }
            devicesOnPlace.ItemsSource = CurrentPlace.TypeDeviceCollection;
        }

        private void Container_Loaded(object sender, RoutedEventArgs e)
        {
            FrameworkElement element = (FrameworkElement)sender;
            element.Loaded -= Container_Loaded;

            //if (VisualTreeHelper.GetChildrenCount(element) > 0)
            //{
            //    Border border = VisualTreeHelper.GetChild(element, 0) as Border;

            //    SetSourceForDevice(border);
            //}

            SetSourceForDevice(element);
            //SetSourceForDevice();
        }

        private void SetSourceForDevice()
        {
            for (int i = 0; i < devicesOnPlace.Items.Count; i++)
            {
                object item = devicesOnPlace.Items[i];
                ListBoxItem listBoxItem = (ListBoxItem)(devicesOnPlace?.ItemContainerGenerator?.ContainerFromItem(item));
                ContentPresenter contentPresenter = FindVisualChild<ContentPresenter>(listBoxItem);
                DataTemplate template = contentPresenter?.ContentTemplate;
                if (template != null)
                {
                    //ComboBox typeBox = (ComboBox)template?.FindName("__type", contentPresenter);
                    ComboBox deviceBox = (ComboBox)template?.FindName("__device", contentPresenter);

                    //typeBox.ItemsSource = CurrentReport.UsedReportColumns.Except(relations);
                    deviceBox.ItemsSource = ((TypeDeviceOnPlace)item).Table.DefaultView;
                    foreach (DataRowView rowView in deviceBox.ItemsSource)
                    {
                        if (Convert.ToInt32(rowView.Row["ID"]) == ((TypeDeviceOnPlace)item)?.Device?.ID)
                        {
                            deviceBox.SelectedItem = rowView;
                            break;
                        }
                    }
                }
            }
        }

        private void SetSourceForDevice(int i)
        {
            object item = devicesOnPlace.Items[i];
            ListBoxItem listBoxItem = (ListBoxItem)(devicesOnPlace?.ItemContainerGenerator?.ContainerFromItem(item));
            ContentPresenter contentPresenter = FindVisualChild<ContentPresenter>(listBoxItem);
            DataTemplate template = contentPresenter?.ContentTemplate;
            if (template != null)
            {
                //ComboBox typeBox = (ComboBox)template?.FindName("__type", contentPresenter);
                ComboBox deviceBox = (ComboBox)template?.FindName("__device", contentPresenter);

                //typeBox.ItemsSource = CurrentReport.UsedReportColumns.Except(relations);
                deviceBox.ItemsSource = ((TypeDeviceOnPlace)item).Table.DefaultView;
                foreach (DataRowView rowView in deviceBox.ItemsSource)
                {
                    if (Convert.ToInt32(rowView.Row["ID"]) == ((TypeDeviceOnPlace)item)?.Device?.ID)
                    {
                        deviceBox.SelectedItem = rowView;
                        break;
                    }
                }
            }
        }

        private void SetSourceForDevice(FrameworkElement element)
        {
            //ListBoxItem listBoxItem = (ListBoxItem)(devicesOnPlace?.ItemContainerGenerator?.ContainerFromItem(element));
            //ContentPresenter contentPresenter = FindVisualChild<ContentPresenter>(listBoxItem);
            ContentPresenter contentPresenter = FindVisualChild<ContentPresenter>(element);
            DataTemplate template = contentPresenter?.ContentTemplate;
            if (template != null)
            {
                //ComboBox typeBox = (ComboBox)template?.FindName("__type", contentPresenter);
                ComboBox deviceBox = (ComboBox)template?.FindName("__device", contentPresenter);

                //typeBox.ItemsSource = CurrentReport.UsedReportColumns.Except(relations);
                if (deviceBox != null)
                {
                    deviceBox.ItemsSource = ((TypeDeviceOnPlace)element.DataContext).Table.DefaultView;
                    foreach (DataRowView rowView in deviceBox.ItemsSource)
                    {
                        if (Convert.ToInt32(rowView.Row["ID"]) == ((TypeDeviceOnPlace)element.DataContext)?.Device?.ID)
                        {
                            deviceBox.SelectedItem = rowView;
                            break;
                        }
                    }
                }
            }
        }

        private childItem FindVisualChild<childItem>(DependencyObject obj) where childItem : DependencyObject
        {
            if (obj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                    if (child != null && child is childItem)
                    {
                        return (childItem)child;
                    }
                    else
                    {
                        childItem childOfChild = FindVisualChild<childItem>(child);
                        if (childOfChild != null)
                        {
                            return childOfChild;
                        }
                    }
                }
            }

            return null;
        }

        private void Window_DragOver(object sender, DragEventArgs e)
        {
            if (Accounting != null)
            {
                Accounting.Left = Left - (Accounting.Width - Width) / 2;
                Accounting.Top = Top - (Accounting.Height - Height) / 2;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Accounting.IsHitTestVisible = false;
            switch (Accounting.TypeChange)
            {
                case TypeChange.Add:
                    break;
                case TypeChange.Change:
                    AudienceID = Accounting.AudienceID;
                    AudienceTableID = Accounting.AudienceTableID;
                    string commandString;
                    SqlDataReader reader;
                    using (SqlConnection connection = new SqlConnection(ConnectionString))
                    {
                        connection.Open();
                        CurrentPlace.ID = AudienceTableID;
                        commandString = $"SELECT [Место] FROM [dbo].[GetAllLocationInAudience]({AudienceID}) Where ID={AudienceTableID}";
                        CurrentPlace.Name = new SqlCommand(commandString, connection).ExecuteScalar().ToString();
                        commandString = $"SELECT * FROM dbo.GetAllTypeAndDeviceOnPlace({AudienceTableID})";
                        reader = new SqlCommand(commandString, connection).ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                if (reader["ID"].GetType() != typeof(DBNull)) // если место занято
                                {
                                    TypeDeviceOnPlace temp = new TypeDeviceOnPlace(CurrentPlace)
                                    {
                                        PlaceID = Convert.ToInt32(reader["PlaceID"]),
                                        TypeDevice = TypeDeviceNames.GetTypeDeviceName((TypeDevice)Enum.Parse(typeof(TypeDevice), reader["TypeName"].ToString())),
                                        Device = new DeviceOnPlace
                                        {
                                            ID = Convert.ToInt32(reader["ID"]),
                                            Name = reader["FullName"].ToString(),
                                        },
                                    };
                                    foreach(DataRowView rowView in temp.Table.DefaultView)
                                    {
                                        if (Convert.ToInt32(rowView.Row["ID"]) == Convert.ToInt32(reader["ID"]))
                                        {
                                            temp.Row = rowView;
                                            break;
                                        }
                                    }
                                    CurrentPlace.TypeDeviceCollection.Add(temp);
                                }
                                else
                                {
                                    CurrentPlace.TypeDeviceCollection.Add(new TypeDeviceOnPlace(CurrentPlace)
                                    {
                                        PlaceID = Convert.ToInt32(reader["PlaceID"]),
                                        TypeDevice = TypeDeviceNames.GetTypeDeviceName((TypeDevice)Enum.Parse(typeof(TypeDevice), reader["TypeName"].ToString())),
                                    });
                                }
                            }
                        }
                    }
                    break;
            }

            mainGrid.DataContext = CurrentPlace;
        }

        private void CloseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //Accounting.Show();
            Accounting.IsHitTestVisible = true;
            Accounting.ChangeLocationView();
            Accounting.viewGrid.IsEnabled = true;
            Accounting.menu.IsEnabled = true;
            Close();
        }

        private void MoveWindow(object sender, MouseButtonEventArgs e)
        {
            DragMove();
            if (Accounting != null)
            {
                Accounting.Left = Left - (Accounting.Width - Width) / 2;
                Accounting.Top = Top - (Accounting.Height - Height) / 2;
            }
        }

        private void Type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetSourceForDevice(VisualTreeHelper.GetParent(VisualTreeHelper.GetParent(VisualTreeHelper.GetParent(VisualTreeHelper.GetParent(sender as ComboBox)))) as ListBoxItem);
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Accounting.IsHitTestVisible = true;
            Accounting.ChangeLocationView();
            Accounting.viewGrid.IsEnabled = true;
            Accounting.menu.IsEnabled = true;
            Close();
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                new SqlCommand($"Update AudienceTable Set Name=N'{CurrentPlace.Name}' where ID={CurrentPlace.ID}").ExecuteNonQuery();
                int count = CurrentPlace.TypeDeviceRemovedCollection.Count;
                for (int i = 0; i < count; i++)
                {
                    TypeDeviceOnPlace temp = CurrentPlace.TypeDeviceRemovedCollection[i];
                    if (temp.Row != null) 
                    {
                        new SqlCommand($"Update {temp.Row["TableName"]} set PlaceID=null Where ID={temp.Row["ID"]}").ExecuteNonQuery();
                    }
                    if (temp.PlaceID != 0)
                    {
                        if (temp.IsRemoved)
                        {
                            new SqlCommand($"Delete from Place Where ID={temp.PlaceID}").ExecuteNonQuery();
                        }
                    }
                }

                count = CurrentPlace.TypeDeviceCollection.Count;
                for (int i = 0; i < count; i++)
                {
                    TypeDeviceOnPlace temp = CurrentPlace.TypeDeviceCollection[i];
                    if (temp.PlaceID != 0)
                    {
                        new SqlCommand($"Update Place set TypeDeviceID=dbo.GetTypeDeviceID(N'{temp.TypeDevice.Type}') Where ID={temp.PlaceID}").ExecuteNonQuery();
                        if (temp.Row != null) // устройство
                        {
                            new SqlCommand($"Update {temp.Row["TypeDevice"]} set PlaceID={temp.PlaceID} Where ID={temp.Row["ID"]}").ExecuteNonQuery();
                            // проверить id!=0
                            // если да то добавить в бд
                            // найти есть ли устройство на этом месте
                            // если есть удалить у него PlaceID (присвоить null)
                            // [GetDeviceOnPlaceID], TableName взять из temp.TypeDevice
                            // при этом учитывать старый и новый тип устройсива (его тоже изменить)
                            // установить для temp.Row в бд PlaceID=temp.PlaceID

                        }
                    }
                    else
                    {
                        SqlCommand command = new SqlCommand($"dbo.[AddLocation]")
                        {
                            CommandType = CommandType.StoredProcedure,
                        };
                        command.Parameters.Add(new SqlParameter("@AudienceTableID", AudienceTableID));
                        command.Parameters.Add(new SqlParameter("@TypeDeviceID", 
                            Convert.ToInt32(new SqlCommand($"Select dbo.GetTypeDeviceID(N'{temp.TypeDevice.Type}')").ExecuteScalar())));
                        SqlParameter parameter = new SqlParameter
                        {
                            ParameterName = "@PlaceID",
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(parameter);
                        command.ExecuteNonQuery();
                        int PID = Convert.ToInt32(command.Parameters["@PlaceID"].Value);
                        if (temp.Row != null) // устройство
                        {
                            new SqlCommand($"Update {temp.Row["TypeDevice"]} set PlaceID={PID} Where ID={temp.Row["ID"]}").ExecuteNonQuery();
                        }
                    }
                }
            }
        }
    }
}
