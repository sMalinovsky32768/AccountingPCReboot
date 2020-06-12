using AccountingPC.Properties;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace AccountingPC
{
    public partial class App : Application
    {
        public System.Windows.Forms.NotifyIcon notify;
        private System.Windows.Forms.ContextMenu notifyContextMenu;

        public App()
        {
            if (Settings.Default.USE_AUTH)
            {
                StartupUri = new Uri("pack://application:,,,/MainWindow.xaml");
            }
            else
            {
                StartupUri = new Uri("pack://application:,,,/AccountingPCWindow.xaml");
            }
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (Settings.Default.SHUTDOWN_ON_EXPLICIT)
            {
                App.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            }
            else
            {
                App.Current.ShutdownMode = ShutdownMode.OnLastWindowClose;
            }
            switch (Settings.Default.THEME)
            {
                case 0:
                    Resources = new ResourceDictionary();
                    Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("pack://application:,,,/BlackTheme/Theme.xaml") });
                    break;
                case 1:
                    Resources = new ResourceDictionary();
                    Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("pack://application:,,,/LightTheme/Theme.xaml") });
                    break;
            }
            notify = new System.Windows.Forms.NotifyIcon(new System.ComponentModel.Container());
            notifyContextMenu = new System.Windows.Forms.ContextMenu(new System.Windows.Forms.MenuItem[]
            { new System.Windows.Forms.MenuItem("Выход", new EventHandler(ShutdownCurrentApp)) });
            notify.Icon = new System.Drawing.Icon("images/icon.ico");
            notify.ContextMenu = notifyContextMenu;
            notify.Text = "AccountingPC";
            notify.DoubleClick += new System.EventHandler(NotifyDoubleClick);
            if (App.Current.ShutdownMode == ShutdownMode.OnExplicitShutdown)
            {
                notify.Visible = true;
            }
            else
            {

                notify.Visible = false;
            }
            if (string.IsNullOrWhiteSpace(SecuritySettings.Default.LOGIN) ||
                string.IsNullOrWhiteSpace(SecuritySettings.Default.PASSWORD))
                Security.SetUserCredentials();
            // CreateDB();
        }

        private void ShutdownCurrentApp(object sender, EventArgs e)
        {
            App.Current.Shutdown();
        }

        private void NotifyDoubleClick(object sender, EventArgs e)
        {
            bool isOpenWindow = false;
            foreach (Window w in App.Current.Windows.OfType<Window>())
            {
                if ((w.WindowState == WindowState.Minimized) || (!w.IsActive))
                {
                    isOpenWindow = true;
                }
            }
            if (!isOpenWindow)
            {
                if (Settings.Default.USE_AUTH)
                    new MainWindow().Show();
                else
                    new AccountingPCWindow().Show();
            }
        }

        private void CreateDB()
        {
            string str;
            SqlConnection myConn = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True");
            str = "";

            SqlCommand myCommand = new SqlCommand(str, myConn);
            try
            {
                myConn.Open();
                myCommand.ExecuteNonQuery();
            }
            catch (System.Exception ex)
            {
                Clipboard.SetText(ex.ToString());
            }
            finally
            {
                if (myConn.State == ConnectionState.Open)
                {
                    myConn.Close();
                }
            }
        }
    }
}
