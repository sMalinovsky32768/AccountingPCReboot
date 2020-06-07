using AccountingPC.Properties;
using System.Windows.Controls;

namespace AccountingPC.ParametersPages
{
    /// <summary>
    /// Логика взаимодействия для ParametersBasicPage.xaml
    /// </summary>
    public partial class ParametersBasicPage : Page
    {
        public ParametersBasicPage()
        {
            InitializeComponent();
            switch (Settings.Default.SHUTDOWN_ON_EXPLICIT)
            {
                case true:
                    isOnExplicitShutdown.SelectedIndex = 0;
                    break;
                case false:
                    isOnExplicitShutdown.SelectedIndex = 1;
                    break;
            }
        }

        private void IsOnExplicitShutdown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (isOnExplicitShutdown.SelectedIndex)
            {
                case 0:
                    Settings.Default.SHUTDOWN_ON_EXPLICIT = true;
                    break;
                case 1:
                    Settings.Default.SHUTDOWN_ON_EXPLICIT = false;
                    break;
            }
        }
    }
}
