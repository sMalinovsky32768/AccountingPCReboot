using AccountingPC.Properties;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using Application = System.Windows.Application;
using Clipboard = System.Windows.Clipboard;

namespace AccountingPC
{
    public partial class App : Application
    {
        public NotifyIcon notify;
        private ContextMenu notifyContextMenu;

        public App()
        {
            StartupUri = Settings.Default.USE_AUTH
                ? new Uri("pack://application:,,,/MainWindow.xaml")
                : new Uri("pack://application:,,,/AccountingPCWindow.xaml");
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (e.Args.Contains(@"\CreateDBNotRun"))
            {
                CreateDB();
                Shutdown(1);
            }
            else
            {
                if (Settings.Default.SHUTDOWN_ON_EXPLICIT)
                    Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
                else
                    Current.ShutdownMode = ShutdownMode.OnLastWindowClose;
                switch (Settings.Default.THEME)
                {
                    case 0:
                        Resources = new ResourceDictionary();
                        Resources.MergedDictionaries.Add(new ResourceDictionary
                            { Source = new Uri("pack://application:,,,/BlackTheme/Theme.xaml") });
                        break;
                    case 1:
                        Resources = new ResourceDictionary();
                        Resources.MergedDictionaries.Add(new ResourceDictionary
                            { Source = new Uri("pack://application:,,,/LightTheme/Theme.xaml") });
                        break;
                }

                notify = new NotifyIcon(new Container());
                notifyContextMenu = new ContextMenu(new[]
                    {new MenuItem("Выход", ShutdownCurrentApp)});
                notify.Icon = new Icon("images/icon.ico");
                notify.ContextMenu = notifyContextMenu;
                notify.Text = "AccountingPC";
                notify.DoubleClick += NotifyDoubleClick;
                if (Current.ShutdownMode == ShutdownMode.OnExplicitShutdown)
                    notify.Visible = true;
                else
                    notify.Visible = false;
                if (string.IsNullOrWhiteSpace(SecuritySettings.Default.LOGIN) ||
                    string.IsNullOrWhiteSpace(SecuritySettings.Default.PASSWORD))
                    Security.SetUserCredentials();
            }
        }

        private void ShutdownCurrentApp(object sender, EventArgs e)
        {
            Current.Shutdown();
        }

        private void NotifyDoubleClick(object sender, EventArgs e)
        {
            var isOpenWindow = false;
            foreach (var w in Current.Windows.OfType<Window>())
                if (w.WindowState == WindowState.Minimized || !w.IsActive)
                    isOpenWindow = true;
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
            var myConn = new SqlConnection(
                    "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True");
            bool exist = true;
            var myCommand = new SqlCommand("if EXISTS (SELECT name FROM sys.databases WHERE name = N'Accounting') select 1 else select 0", myConn);
            try
            {
                myConn.Open();
                exist = Convert.ToBoolean(myCommand.ExecuteScalar());
            }
            catch (Exception ex)
            {
                Clipboard.SetText(ex.ToString());
            }
            finally
            {
                if (myConn.State == ConnectionState.Open) myConn.Close();
            }

            if (!exist)
            {
                var assembly = Assembly.GetExecutingAssembly();
                var resourceName = assembly.GetManifestResourceNames().Single(s => s.EndsWith("OnlyDB.sql"));
                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        str = reader.ReadToEnd();
                    }
                }

                myCommand = new SqlCommand(str, myConn);
                try
                {
                    myConn.Open();
                    myCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Clipboard.SetText(ex.ToString());
                }
                finally
                {
                    if (myConn.State == ConnectionState.Open) myConn.Close();
                }

                resourceName = assembly.GetManifestResourceNames().Single(s => s.EndsWith("Schema.sql"));
                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        str = reader.ReadToEnd();
                    }
                }

                myCommand = new SqlCommand(str, myConn);
                try
                {
                    myConn.Open();
                    myCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Clipboard.SetText(ex.ToString());
                }
                finally
                {
                    if (myConn.State == ConnectionState.Open) myConn.Close();
                }

                resourceName = assembly.GetManifestResourceNames().Single(s => s.EndsWith("CommonData.sql"));
                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        str = reader.ReadToEnd();
                    }
                }

                myCommand = new SqlCommand(str, myConn);
                try
                {
                    myConn.Open();
                    myCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Clipboard.SetText(ex.ToString());
                }
                finally
                {
                    if (myConn.State == ConnectionState.Open) myConn.Close();
                }
            }
        }
    }
}