using AccountingPC.Properties;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AccountingPC
{
    /// <summary>
    /// Логика взаимодействия для ParametersWindow.xaml
    /// </summary>
    public partial class ParametersWindow : Window
    {
        public static readonly RoutedCommand ExitCommand = new RoutedUICommand(
            "Exit", "ExitCommand", typeof(ParametersWindow),
            new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.F4, ModifierKeys.Alt) }));

        public ParametersWindow()
        {
            InitializeComponent();
        }

        private void SelectedOption(object sender, RoutedEventArgs e)
        {
            ListBoxItem item = (ListBoxItem)sender;
            if (frameSettings == null)
            {
                frameSettings = new Frame();
            }

            switch (item.Content)
            {
                case "Основное":
                    frameSettings.Source = new Uri("ParametersPages/ParametersBasicPage.xaml", UriKind.RelativeOrAbsolute);
                    break;
                case "Стили":
                    frameSettings.Source = new Uri("ParametersPages/ParametersStylesPage.xaml", UriKind.RelativeOrAbsolute);
                    // frameSettings.Source = new Uri("Maps/MapDefault.xaml", UriKind.RelativeOrAbsolute);
                    break;
                case "Безопасность":
                    frameSettings.Source = new Uri("ParametersPages/ParametersSecurityPage.xaml", UriKind.RelativeOrAbsolute);
                    break;
                default:
                    break;
            }
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).IsDefault)
            {
                Settings.Default.Save();
            }
            switch (Settings.Default.THEME)
            {
                case 0:
                    Application.Current.Resources = new ResourceDictionary();
                    Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("pack://application:,,,/BlackTheme/Theme.xaml") });
                    break;
                case 1:
                    Application.Current.Resources = new ResourceDictionary();
                    Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("pack://application:,,,/LightTheme/Theme.xaml") });
                    break;
            }
            DialogResult = true;
            Close();
            //((AccountingPCWindow)Owner).ChangeWindowState();
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();// Для перемещение ока
        }

        private void ExitApp(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
