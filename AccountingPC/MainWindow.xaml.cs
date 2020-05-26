using AccountingPC.Properties;
using System;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Security.Cryptography;

namespace AccountingPC
{
    public partial class MainWindow : Window
    {
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
            String login = loginTextBox.Text;
            String uName = Settings.Default.USER_NAME;
            String enPass = Convert.ToBase64String(SHA256.Create().ComputeHash(Encoding.ASCII.GetBytes(passwordTextBox.Password)));
            String setPass = Settings.Default.PASSWORD_HASH;
            bool isTrueLogin = login == uName;
            bool isTruePassword = enPass == setPass;
            if (isTrueLogin && isTruePassword)
            {
                Hide();
                new AccountingPCWindow().Show();
                Close();
            }
            else
            {
                MessageBox.Show("Неправильный логин или пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();// Для перемещение ока
        }
    }
}
