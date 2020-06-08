using System;
using System.Collections.Generic;
using System.Configuration;
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
            InitializeComponent();
            Owner = window;
            Accounting = window;
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
            CurrentPlace = new Place();
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
                        commandString = $"SELECT [Место] FROM [dbo].[GetAllLocationInAudience] ({AudienceID}) Where ID={AudienceTableID}";
                        CurrentPlace.Name = new SqlCommand(commandString, connection).ExecuteScalar().ToString();
                        commandString = $"SELECT * FROM dbo.GetAllTypeAndDeviceOnPlace({AudienceTableID})";
                        reader = new SqlCommand(commandString, connection).ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                CurrentPlace.TypeDeviceCollection.Add(new TypeDeviceOnPlace()
                                {
                                    PlaceID = Convert.ToInt32(reader["PlaceID"]),
                                    TypeDevice = (TypeDevice)reader["TableName"],
                                    Device = new DeviceOnPlace
                                    {
                                        ID = Convert.ToInt32(reader["ID"]),
                                        Name = reader["FullName"].ToString(),
                                    }
                                });
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

        }
    }
}
