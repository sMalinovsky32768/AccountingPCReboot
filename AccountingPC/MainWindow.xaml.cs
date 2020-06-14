using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace AccountingPC
{
    public partial class MainWindow : Window
    {
        public static readonly RoutedCommand ExitCommand = new RoutedUICommand(
            "Exit", "ExitCommand", typeof(MainWindow),
            new InputGestureCollection(new InputGesture[] {new KeyGesture(Key.Escape)}));

        internal AccountingPCWindow accounting;

        public MainWindow()
        {
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
            loginTextBox.Focus();
        }

        private void LoginClick(object sender, RoutedEventArgs e)
        {
            var login = loginTextBox.Text;
            var pass = passwordBox.Password;
            try
            {
                if (Security.VerifyCredentials(login, pass))
                {
                    Hide();
                    accounting = new AccountingPCWindow();
                    accounting.Show();
                    Close();
                }
                else
                {
                    MessageBox.Show("Неправильный логин или пароль", "Ошибка", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, exception.GetType().Name, MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            accounting = null;
            Close();
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}