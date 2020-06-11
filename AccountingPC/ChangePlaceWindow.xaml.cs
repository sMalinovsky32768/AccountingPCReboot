using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AccountingPC
{
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

            SetSourceForDevice(element);
        }

        private void SetSourceForDevice(FrameworkElement element)
        {
            ContentPresenter contentPresenter = FindVisualChild<ContentPresenter>(element);
            DataTemplate template = contentPresenter?.ContentTemplate;
            if (template != null)
            {
                ComboBox deviceBox = (ComboBox)template?.FindName("__device", contentPresenter);

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
                    if (child != null && child is childItem item)
                    {
                        return item;
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
                    saveButton.Content = "Добавить";
                    break;
                case TypeChange.Change:
                    saveButton.Content = "Изменить";
                    AudienceID = Accounting.AudienceID;
                    AudienceTableID = Accounting.AudienceTableID;
                    string commandString;
                    SqlDataReader reader;
                    using (SqlConnection connection = new SqlConnection(ConnectionString))
                    {
                        connection.Open();
                        CurrentPlace.ID = AudienceTableID;
                        commandString = $"SELECT [Name] FROM [dbo].[GetAllLocationInAudience]({AudienceID}) Where ID={AudienceTableID}";
                        CurrentPlace.Name = new SqlCommand(commandString, connection).ExecuteScalar().ToString();
                        commandString = $"SELECT * FROM dbo.GetAllTypeAndDeviceOnPlace({AudienceTableID})";
                        reader = new SqlCommand(commandString, connection).ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                if (reader["ID"].GetType() != typeof(DBNull)) // если место занято
                                {
                                    TypeDeviceOnPlace temp = new TypeDeviceOnPlace(
                                        TypeDeviceNames.GetTypeDeviceName((TypeDevice)Enum.Parse(typeof(TypeDevice), reader["TypeName"].ToString())),
                                        null, 
                                        new DeviceOnPlace
                                        {
                                            ID = Convert.ToInt32(reader["ID"]),
                                            Name = reader["FullName"].ToString(),
                                        },
                                        Convert.ToInt32(reader["PlaceID"]),
                                        CurrentPlace);
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
                                    CurrentPlace.TypeDeviceCollection.Add(new TypeDeviceOnPlace(
                                        TypeDeviceNames.GetTypeDeviceName((TypeDevice)Enum.Parse(typeof(TypeDevice), reader["TypeName"].ToString())),
                                        null,
                                        null,
                                        Convert.ToInt32(reader["PlaceID"]),
                                        CurrentPlace));
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
            Accounting.IsHitTestVisible = true;
            Accounting.ChangeLocationView();
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
            SetSourceForDevice(VisualTreeHelper.GetParent(VisualTreeHelper.GetParent(
                VisualTreeHelper.GetParent(VisualTreeHelper.GetParent(sender as ComboBox)))) as ListBoxItem);
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Accounting.IsHitTestVisible = true;
            Accounting.ChangeLocationView();
            Close();
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                if (AudienceTableID == 0)
                {
                    SqlCommand command = new SqlCommand($"AddLocation", connection)
                    {
                        CommandType = CommandType.StoredProcedure,
                    };
                    command.Parameters.Add(new SqlParameter("@AudienceID", AudienceID));
                    SqlParameter parameter = new SqlParameter
                    {
                        ParameterName = "@TableID",
                        Direction = ParameterDirection.Output,
                        DbType = DbType.Int32,
                    };
                    command.Parameters.Add(parameter);
                    command.ExecuteNonQuery();
                    AudienceTableID = Convert.ToInt32(command.Parameters["@TableID"].Value);
                }
                new SqlCommand($"Update AudienceTable Set Name=N'{CurrentPlace.Name}' where ID={CurrentPlace.ID}", connection).ExecuteNonQuery();
                int count = CurrentPlace.TypeDeviceRemovedCollection.Count;
                for (int i = 0; i < count; i++)
                {
                    TypeDeviceOnPlace temp = CurrentPlace.TypeDeviceRemovedCollection[i];
                    if (temp.Row != null) 
                    {
                        object obj = new SqlCommand($"Select TOP(1) PlaceID from {temp.Row["TableName"]} Where ID={temp.Row["ID"]}", connection).ExecuteScalar();
                        int id = Convert.ToInt32(obj.GetType() != typeof(DBNull) ? obj : 0);
                        if (id == temp.PlaceID)
                            new SqlCommand($"Update {temp.Row["TableName"]} set PlaceID=null Where ID={temp.Row["ID"]}", connection).ExecuteNonQuery();
                    }
                    if (temp.PlaceID != 0)
                    {
                        if (temp.IsRemoved)
                        {
                            new SqlCommand($"Delete from Place Where ID={temp.PlaceID}", connection).ExecuteNonQuery();
                        }
                    }
                }

                count = CurrentPlace.TypeDeviceCollection.Count;
                for (int i = 0; i < count; i++)
                {
                    TypeDeviceOnPlace temp = CurrentPlace.TypeDeviceCollection[i];
                    if (temp.PlaceID != 0)
                    {
                        new SqlCommand($"Update Place set TypeDeviceID=dbo.GetTypeDeviceID(N'{temp.TypeDevice.Type}') Where ID={temp.PlaceID}", connection).ExecuteNonQuery();
                        if (temp.Row != null) // устройство
                        {
                            new SqlCommand($"Update {temp.Row["TableName"]} set PlaceID={temp.PlaceID} Where ID={temp.Row["ID"]}", connection).ExecuteNonQuery();

                        }
                    }
                    else
                    {
                        SqlCommand command = new SqlCommand($"AddLocation", connection)
                        {
                            CommandType = CommandType.StoredProcedure,
                        };
                        command.Parameters.Add(new SqlParameter("@AudienceTableID", AudienceTableID));
                        command.Parameters.Add(new SqlParameter("@TypeDeviceID", 
                            Convert.ToInt32(new SqlCommand($"Select dbo.GetTypeDeviceID(N'{temp.TypeDevice.Type}')", connection).ExecuteScalar())));
                        SqlParameter parameter = new SqlParameter
                        {
                            ParameterName = "@PlaceID",
                            Direction = ParameterDirection.Output,
                            DbType = DbType.Int32,
                        };
                        command.Parameters.Add(parameter);
                        command.ExecuteNonQuery();
                        int PID = Convert.ToInt32(command.Parameters["@PlaceID"].Value);
                        if (temp.Row != null) // устройство
                        {
                            new SqlCommand($"Update {temp.Row["TableName"]} set PlaceID={PID} Where ID={temp.Row["ID"]}", connection).ExecuteNonQuery();
                        }
                    }
                }
            }
            Accounting.IsHitTestVisible = true;
            Accounting.ChangeLocationView();
            Close();
        }
    }
}
