using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AccountingPC
{
    public partial class ChangePlaceWindow : Window
    {
        public static readonly RoutedCommand CloseCommand = new RoutedUICommand(
            "Close", "CloseCommand", typeof(AccountingPCWindow),
            new InputGestureCollection(new InputGesture[] {new KeyGesture(Key.Escape)}));

        public ChangePlaceWindow(AccountingPCWindow window)
        {
            CurrentPlace = new Place();
            InitializeComponent();
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                var resourceName = assembly.GetManifestResourceNames().Single(s => s.EndsWith("icon.ico"));
                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
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
            devicesOnPlace.ItemContainerGenerator.StatusChanged += ItemContainerGenerator_StatusChanged;
        }

        public static string ConnectionString { get; } =
            ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public AccountingPCWindow Accounting { get; }
        internal Place CurrentPlace { get; set; }
        private int AudienceID { get; set; }
        private int AudienceTableID { get; set; }

        private void ItemContainerGenerator_StatusChanged(object sender, EventArgs e)
        {
            try
            {
                if (devicesOnPlace.ItemContainerGenerator.Status ==
                    GeneratorStatus.ContainersGenerated)
                {
                    var containers = devicesOnPlace.Items.Cast<object>().Select(
                        item => (FrameworkElement) devicesOnPlace.ItemContainerGenerator.ContainerFromItem(item));
                    foreach (var container in containers) container.Loaded += Container_Loaded;
                }

                devicesOnPlace.ItemsSource = CurrentPlace.TypeDeviceCollection;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, exception.GetType().Name, MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void Container_Loaded(object sender, RoutedEventArgs e)
        {
            var element = (FrameworkElement) sender;
            element.Loaded -= Container_Loaded;

            SetSourceForDevice(element);
        }

        private void SetSourceForDevice(FrameworkElement element)
        {
            var contentPresenter = FindVisualChild<ContentPresenter>(element);
            var template = contentPresenter?.ContentTemplate;
            if (template != null)
            {
                var deviceBox = (ComboBox) template?.FindName("__device", contentPresenter);

                if (deviceBox != null)
                {
                    deviceBox.ItemsSource = ((TypeDeviceOnPlace) element.DataContext).Table.DefaultView;
                    foreach (DataRowView rowView in deviceBox.ItemsSource)
                        if (Convert.ToInt32(rowView.Row["ID"]) == ((TypeDeviceOnPlace) element.DataContext)?.Device?.ID)
                        {
                            deviceBox.SelectedItem = rowView;
                            break;
                        }
                }
            }
        }

        private childItem FindVisualChild<childItem>(DependencyObject obj) where childItem : DependencyObject
        {
            if (obj != null)
                for (var i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
                {
                    var child = VisualTreeHelper.GetChild(obj, i);
                    if (child != null && child is childItem item)
                    {
                        return item;
                    }

                    var childOfChild = FindVisualChild<childItem>(child);
                    if (childOfChild != null) return childOfChild;
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
                    try
                    {
                        AudienceID = Accounting.AudienceID;
                        AudienceTableID = Accounting.AudienceTableID;
                        string commandString;
                        SqlDataReader reader;
                        using (var connection = new SqlConnection(ConnectionString))
                        {
                            connection.Open();
                            CurrentPlace.ID = AudienceTableID;
                            commandString =
                                $"SELECT [Name] FROM [dbo].[GetAllLocationInAudience]({AudienceID}) Where ID={AudienceTableID}";
                            CurrentPlace.Name = new SqlCommand(commandString, connection).ExecuteScalar().ToString();
                            commandString = $"SELECT * FROM dbo.GetAllTypeAndDeviceOnPlace({AudienceTableID})";
                            reader = new SqlCommand(commandString, connection).ExecuteReader();
                            if (reader.HasRows)
                                while (reader.Read())
                                    if (!(reader["ID"] is DBNull)) // если место занято
                                    {
                                        var temp = new TypeDeviceOnPlace(
                                            TypeDeviceNames.GetTypeDeviceName((TypeDevice) Enum.Parse(typeof(TypeDevice),
                                                reader["TypeName"].ToString())),
                                            null,
                                            new DeviceOnPlace
                                            {
                                                ID = Convert.ToInt32(reader["ID"]),
                                                Name = reader["FullName"].ToString()
                                            },
                                            Convert.ToInt32(reader["PlaceID"]),
                                            CurrentPlace);
                                        foreach (DataRowView rowView in temp.Table.DefaultView)
                                            if (Convert.ToInt32(rowView.Row["ID"]) == Convert.ToInt32(reader["ID"]))
                                            {
                                                temp.Row = rowView;
                                                break;
                                            }

                                        CurrentPlace.TypeDeviceCollection.Add(temp);
                                    }
                                    else
                                    {
                                        CurrentPlace.TypeDeviceCollection.Add(new TypeDeviceOnPlace(
                                            TypeDeviceNames.GetTypeDeviceName((TypeDevice)Enum.Parse(typeof(TypeDevice),
                                                reader["TypeName"].ToString())),
                                            null,
                                            null,
                                            Convert.ToInt32(reader["PlaceID"]),
                                            CurrentPlace));
                                    }
                        }
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message, exception.GetType().Name, MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }

                    break;
            }

            mainGrid.DataContext = CurrentPlace;
        }

        private void CloseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Accounting.IsHitTestVisible = true;
            try
            {
                Accounting.ChangeLocationView();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, exception.GetType().Name, MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
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
            try
            {
                SetSourceForDevice(VisualTreeHelper.GetParent(VisualTreeHelper.GetParent(
                    VisualTreeHelper.GetParent(VisualTreeHelper.GetParent(sender as ComboBox)))) as ListBoxItem);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, exception.GetType().Name, MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Accounting.IsHitTestVisible = true;
            try
            {
                Accounting.ChangeLocationView();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, exception.GetType().Name, MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            Close();
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    if (AudienceTableID == 0)
                    {
                        var command = new SqlCommand("AddLocation", connection)
                        {
                            CommandType = CommandType.StoredProcedure
                        };
                        command.Parameters.Add(new SqlParameter("@AudienceID", AudienceID));
                        var parameter = new SqlParameter
                        {
                            ParameterName = "@TableID",
                            Direction = ParameterDirection.Output,
                            DbType = DbType.Int32
                        };
                        command.Parameters.Add(parameter);
                        command.ExecuteNonQuery();
                        AudienceTableID = Convert.ToInt32(command.Parameters["@TableID"].Value);
                    }

                    new SqlCommand($"Update AudienceTable Set Name=N'{CurrentPlace.Name}' where ID={CurrentPlace.ID}",
                        connection).ExecuteNonQuery();
                    var count = CurrentPlace.TypeDeviceRemovedCollection.Count;
                    for (var i = 0; i < count; i++)
                    {
                        var temp = CurrentPlace.TypeDeviceRemovedCollection[i];
                        if (temp.Row != null)
                        {
                            var obj = new SqlCommand(
                                    $"Select TOP(1) PlaceID from {temp.Row["TableName"]} Where ID={temp.Row["ID"]}",
                                    connection)
                                .ExecuteScalar();
                            var id = Convert.ToInt32(!(obj is DBNull) ? obj : 0);
                            if (id == temp.PlaceID)
                                new SqlCommand($"Update {temp.Row["TableName"]} set PlaceID=null Where ID={temp.Row["ID"]}",
                                    connection).ExecuteNonQuery();
                        }

                        if (temp.PlaceID != 0)
                            if (temp.IsRemoved)
                                new SqlCommand($"Delete from Place Where ID={temp.PlaceID}", connection).ExecuteNonQuery();
                    }

                    count = CurrentPlace.TypeDeviceCollection.Count;
                    for (var i = 0; i < count; i++)
                    {
                        var temp = CurrentPlace.TypeDeviceCollection[i];
                        if (temp.PlaceID != 0)
                        {
                            new SqlCommand(
                                $"Update Place set TypeDeviceID=dbo.GetTypeDeviceID(N'{temp.TypeDevice.Type}') Where ID={temp.PlaceID}",
                                connection).ExecuteNonQuery();
                            if (temp.Row != null) // устройство
                                new SqlCommand(
                                    $"Update {temp.Row["TableName"]} set PlaceID={temp.PlaceID} Where ID={temp.Row["ID"]}",
                                    connection).ExecuteNonQuery();
                        }
                        else
                        {
                            var command = new SqlCommand("AddLocation", connection)
                            {
                                CommandType = CommandType.StoredProcedure
                            };
                            command.Parameters.Add(new SqlParameter("@AudienceTableID", AudienceTableID));
                            command.Parameters.Add(new SqlParameter("@TypeDeviceID",
                                Convert.ToInt32(new SqlCommand($"Select dbo.GetTypeDeviceID(N'{temp.TypeDevice.Type}')",
                                    connection).ExecuteScalar())));
                            var parameter = new SqlParameter
                            {
                                ParameterName = "@PlaceID",
                                Direction = ParameterDirection.Output,
                                DbType = DbType.Int32
                            };
                            command.Parameters.Add(parameter);
                            command.ExecuteNonQuery();
                            var PID = Convert.ToInt32(command.Parameters["@PlaceID"].Value);
                            if (temp.Row != null) // устройство
                                new SqlCommand(
                                    $"Update {temp.Row["TableName"]} set PlaceID={PID} Where ID={temp.Row["ID"]}",
                                    connection).ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message, exception.GetType().Name, MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    if (connection.State== ConnectionState.Open) connection.Close();
                }
            }

            Accounting.IsHitTestVisible = true;
            Accounting.ChangeLocationView();
            Close();
        }
    }
}