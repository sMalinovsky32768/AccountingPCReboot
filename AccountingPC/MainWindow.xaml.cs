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

        // Выход при Esc
        public static readonly RoutedCommand ExitCommand = new RoutedUICommand(
            "Exit", "ExitCommand", typeof(MainWindow),
            new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.Escape) }));
        //Authorization authorization;

        public MainWindow()
        {
            InitializeComponent();
            loginTextBox.Focus();
            //authorization = new Authorization();
            //DataContext = authorization;
        }

        private void LoginClick(object sender, RoutedEventArgs e)
        {
            string login = loginTextBox.Text;
            string uName = Settings.Default.USER_NAME;
            string enPass = Convert.ToBase64String(SHA256.Create().ComputeHash(Encoding.ASCII.GetBytes(passwordTextBox.Password)));
            string setPass = Settings.Default.PASSWORD_HASH;
            bool isTrueLogin = login == uName;
            bool isTruePassword = enPass == setPass;
            if (isTrueLogin && isTruePassword)
            {
                Hide();
                //new AccountingPCWindow().Show();
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
            DragMove();// Для перемещение ока
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //accounting = new AccountingPCWindow();
        }
    }
}
