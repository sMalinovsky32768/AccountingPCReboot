﻿using AccountingPC.Properties;
using System.Windows.Controls;

namespace AccountingPC.ParametersPages
{
    public partial class ParametersStylesPage : Page
    {
        public ParametersStylesPage()
        {
            InitializeComponent();
            Theme.SelectedIndex = Settings.Default.THEME;
        }

        private void Theme_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Settings.Default.THEME = Theme.SelectedIndex;
        }
    }
}
