using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using AccountingPC.Properties;

namespace AccountingPC
{
    public partial class ParametersWindow : Window
    {
        public static readonly RoutedCommand ExitCommand = new RoutedUICommand(
            "Exit", "ExitCommand", typeof(ParametersWindow),
            new InputGestureCollection(new InputGesture[] {new KeyGesture(Key.F4, ModifierKeys.Alt)}));

        public ParametersWindow()
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

        }

        private void SelectedOption(object sender, RoutedEventArgs e)
        {
            var item = (ListBoxItem) sender;
            if (frameSettings == null) frameSettings = new Frame();

            switch (item.Content)
            {
                case "Основное":
                    frameSettings.Source =
                        new Uri("ParametersPages/ParametersBasicPage.xaml", UriKind.RelativeOrAbsolute);
                    break;
                case "Стили":
                    frameSettings.Source = new Uri("ParametersPages/ParametersStylesPage.xaml",
                        UriKind.RelativeOrAbsolute);
                    break;
                case "Безопасность":
                    frameSettings.Source = new Uri("ParametersPages/ParametersSecurityPage.xaml",
                        UriKind.RelativeOrAbsolute);
                    break;
            }
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            int oldtheme = Settings.Default.THEME;
            if (((Button) sender).IsDefault) Settings.Default.Save();
            if (Settings.Default.THEME == oldtheme)
            {
                switch (Settings.Default.THEME)
                {
                    case 0:
                        Application.Current.Resources = new ResourceDictionary();
                        Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary
                            { Source = new Uri("pack://application:,,,/BlackTheme/Theme.xaml") });
                        break;
                    case 1:
                        Application.Current.Resources = new ResourceDictionary();
                        Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary
                            { Source = new Uri("pack://application:,,,/LightTheme/Theme.xaml") });
                        break;
                }
            }

            DialogResult = true;
            Close();
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void ExitApp(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}