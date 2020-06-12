using AccountingPC.Properties;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AccountingPC.ParametersPages
{
    public partial class ParametersSecurityPage : Page
    {
        public ParametersSecurityPage()
        {
            InitializeComponent();
            switch (Settings.Default.USE_AUTH)
            {
                case true:
                    useAuth.SelectedIndex = 0;
                    break;
                case false:
                    useAuth.SelectedIndex = 1;
                    break;
            }
            login.Text = Security.Login;
        }

        private void ChangeClick(object sender, RoutedEventArgs e)
        {
            if (!(string.IsNullOrWhiteSpace(oldPass.Password) 
                || string.IsNullOrWhiteSpace(newPass.Password) 
                || string.IsNullOrWhiteSpace(repeatPass.Password)))
            {
                KeyValuePair<bool, string> res = ChangeCredentials();
                if (res.Key)
                {
                    changeStatus.Content = res.Value;
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
                            Dispatcher.Invoke(() => changeStatus.Content = string.Empty);

                        }
                        catch { }
                    });
                    task.Start();
                }
                else
                {
                    MessageBox.Show(res.Value);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public KeyValuePair<bool, string> ChangeCredentials()
        {
            if (newPass.Password == repeatPass.Password)
            {
                if(Security.UpdatCredentials(oldPass.Password, newPass.Password, login.Text))
                {
                    return new KeyValuePair<bool, string>(true, "Пароль успешно изменен");
                }
                else return new KeyValuePair<bool, string>(false, "Неверный пароль");
            }
            else
            {
                return new KeyValuePair<bool, string>(false, "Пароли не совпадают");
            }
        }

        private void UseAuth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (useAuth.SelectedIndex)
            {
                case 0:
                    Settings.Default.USE_AUTH = true;
                    break;
                case 1:
                    Settings.Default.USE_AUTH = false;
                    break;
            }
        }
    }
}
