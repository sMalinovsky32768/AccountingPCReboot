using AccountingPC.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using static System.Diagnostics.Process;
using Application = System.Windows.Application;
using Clipboard = System.Windows.Clipboard;
using MessageBox = System.Windows.MessageBox;

namespace AccountingPC
{
    public partial class App : Application
    {
        public NotifyIcon Notify;
        private ContextMenu notifyContextMenu;

        public App()
        {
            StartupUri = Settings.Default.USE_AUTH
                ? new Uri("pack://application:,,,/MainWindow.xaml")
                : new Uri("pack://application:,,,/AccountingPCWindow.xaml");
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (GetProcessesByName(GetCurrentProcess().ProcessName).Length > 1)
            {
                MessageBox.Show("Приложение уже запущено", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                GetCurrentProcess().Kill();
                //Shutdown(2);
            }
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

                Notify = new NotifyIcon(new Container());
                notifyContextMenu = new ContextMenu(new[]
                    {new MenuItem("Выход", ShutdownCurrentApp)});
                try
                {
                    var assembly = Assembly.GetExecutingAssembly();
                    var resourceName = assembly.GetManifestResourceNames().Single(s => s.EndsWith("icon.ico"));
                    using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                    {
                        if (stream != null)
                            Notify.Icon = new Icon(stream);
                    }
                }
                catch (Exception ex)
                {
                    Clipboard.SetText(ex.ToString());
                }

                Notify.ContextMenu = notifyContextMenu;
                Notify.Text = "AccountingPC";
                Notify.DoubleClick += NotifyDoubleClick;
                if (Current.ShutdownMode == ShutdownMode.OnExplicitShutdown)
                    Notify.Visible = true;
                else
                    Notify.Visible = false;
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
            var myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["MasterConnection"].ConnectionString);
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
                MessageBox.Show(ex.ToString(), ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (myConn.State == ConnectionState.Open) myConn.Close();
            }

            if (!exist)
            {
                var assembly = Assembly.GetExecutingAssembly();
                var resourceName = assembly.GetManifestResourceNames().Single(s => s.EndsWith("OnlyDB.sql"));
                string str = null;
                using (var stream = assembly.GetManifestResourceStream(resourceName))
                {
                    if (stream != null)
                        using (var reader = new StreamReader(stream))
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

                myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"]
                    .ConnectionString);

                resourceName = assembly.GetManifestResourceNames().Single(s => s.EndsWith("Schema.sql"));
                using (var stream = assembly.GetManifestResourceStream(resourceName))
                {
                    if (stream != null)
                        using (var reader = new StreamReader(stream))
                        {
                            str = reader.ReadToEnd();
                        }
                }

                string[] arr = str?.Split(new[] { "GO" }, StringSplitOptions.None);

                if (arr != null)
                {
                    List<string> arrList = new List<string>()
                    {
                        Capacity = 512,
                    };
                    for (int i = 0; i < arr.Length; i++)
                    {
                        str = arr[i];
                        myCommand = new SqlCommand(str, myConn);
                        try
                        {
                            myConn.Open();
                            myCommand.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            Clipboard.SetText(ex.ToString());
                            arrList.Add(str);
                        }
                        finally
                        {
                            if (myConn.State == ConnectionState.Open) myConn.Close();
                        }
                    }

                    while (arrList.Count != 0)
                    {
                        int l = arr.Length;
                        for (int i = 0; i < l; i++)
                        {
                            str = arr[i];
                            myCommand = new SqlCommand(str, myConn);
                            try
                            {
                                myConn.Open();
                                myCommand.ExecuteNonQuery();
                                arrList.Remove(str);
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



                resourceName = assembly.GetManifestResourceNames().Single(s => s.EndsWith("CommonData.sql"));
                using (var stream = assembly.GetManifestResourceStream(resourceName))
                {
                    if (stream != null)
                        using (var reader = new StreamReader(stream))
                        {
                            str = reader.ReadToEnd();
                        }
                }

                arr = str?.Split(new[] { "GO" }, StringSplitOptions.None);

                if (arr != null)
                {
                    for (int i = 0; i < arr.Length; i++)
                    {
                        str = arr[i];
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
    }
}