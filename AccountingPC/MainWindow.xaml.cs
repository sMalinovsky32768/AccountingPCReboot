using AccountingPC.Properties;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace AccountingPC
{
    public partial class MainWindow : Window
    {
        internal AccountingPCWindow accounting;

        public static readonly RoutedCommand ExitCommand = new RoutedUICommand(
            "Exit", "ExitCommand", typeof(MainWindow),
            new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.Escape) }));

        public MainWindow()
        {
            InitializeComponent();
            loginTextBox.Focus();
        }

        private void LoginClick(object sender, RoutedEventArgs e)
        {
            string login = loginTextBox.Text;
            string pass = passwordBox.Password;
            if (Security.VerifyCredentials(login, pass))
            {
                Hide();
                accounting = new AccountingPCWindow();
                accounting.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Неправильный логин или пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
