using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AccountingPC.AccountingReport
{
    /// <summary>
    /// Interaction logic for ConfiguringReportWindow.xaml
    /// </summary>
    public partial class ConfiguringReportWindow : Window
    {
        internal Report CurrentReport { get; set; }
        public ConfiguringReportWindow(TypeReport typeReport = TypeReport.Full)
        {
            InitializeComponent();
            CurrentReport = new Report(typeReport);
            CurrentReport.Options.TypeReportChangedEvent += TypeReportChangedEventHandler;
        }

        public void UpdateReportConfig()
        {
            if (CurrentReport.Options.TypeReport != TypeReport.Full)
            {
                selectionColumnGrid.IsEnabled = true;

                unusedColumn.ItemsSource = CurrentReport.UnusedReportColumns;
                unusedColumn.DisplayMemberPath = "Name";

                usedColumn.ItemsSource = CurrentReport.UsedReportColumns;
                usedColumn.DisplayMemberPath = "Name";
            }
            else
            {
                selectionColumnGrid.IsEnabled = false;
            }
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //System.IO.Path.GetTempPath();
            DragMove();// Для перемещение окна
        }

        private void TypeReportBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            KeyValuePair<TypeReport, string> pair = (KeyValuePair<TypeReport, string>)typeReportBox.SelectedItem;
            CurrentReport.Options.TypeReport = pair.Key;
            UpdateReportConfig();
        }

        private void TypeReportChangedEventHandler()
        {
            foreach (KeyValuePair<TypeReport, string> pair in ((Dictionary<TypeReport, string>)typeReportBox.ItemsSource))
            {
                if (pair.Key == CurrentReport.Options.TypeReport &&
                    ((KeyValuePair<TypeReport, string>)typeReportBox.SelectedItem).Key != pair.Key) 
                {
                    typeReportBox.SelectedItem = pair;
                    break;
                }
            }
        }

        private void UseColumnButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentReport.UsedReportColumns.Add((ColumnRelation)unusedColumn.SelectedItem);
            CurrentReport.UnusedReportColumns.Remove((ColumnRelation)unusedColumn.SelectedItem);
        }

        private void NotUseColumnButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentReport.UnusedReportColumns.Add((ColumnRelation)usedColumn.SelectedItem);
            CurrentReport.UsedReportColumns.Remove((ColumnRelation)usedColumn.SelectedItem);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            typeReportBox.ItemsSource = Report.ReportNames;
            typeReportBox.DisplayMemberPath = "Value";

            UpdateReportConfig();
        }
    }
}
