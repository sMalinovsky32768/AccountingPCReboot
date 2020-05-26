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
        public ConfiguringReportWindow(TypeReport typeReport = TypeReport.Simple)
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
                selectionSortingParamGrid.IsEnabled = true;

                unusedColumn.ItemsSource = CurrentReport.UnusedReportColumns;
                unusedColumn.DisplayMemberPath = "Name";

                usedColumn.ItemsSource = CurrentReport.UsedReportColumns;
                usedColumn.DisplayMemberPath = "Name";

                SetSourceForSorting();
            }
            else
            {
                selectionColumnGrid.IsEnabled = false;
                selectionSortingParamGrid.IsEnabled = false;
            }
        }

        private childItem FindVisualChild<childItem>(DependencyObject obj) where childItem : DependencyObject
        {
            if (obj!=null)
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                    if (child != null && child is childItem)
                    {
                        return (childItem)child;
                    }
                    else
                    {
                        childItem childOfChild = FindVisualChild<childItem>(child);
                        if (childOfChild != null)
                            return childOfChild;
                    }
                }
            return null;
        }

        private void SetSourceForSorting()
        {
            foreach (var item in sortingParamsList.Items)
            {
                ListBoxItem listBoxItem = (ListBoxItem)(sortingParamsList?.ItemContainerGenerator?.ContainerFromItem(item));
                ContentPresenter contentPresenter = FindVisualChild<ContentPresenter>(listBoxItem);
                DataTemplate template = contentPresenter?.ContentTemplate;
                if (template != null)
                {
                    ((ComboBox)template?.FindName("col", contentPresenter)).ItemsSource = CurrentReport.UsedReportColumns;
                    ((ComboBox)template?.FindName("order", contentPresenter)).ItemsSource = SortOrderRelation.OrderNames;
                }
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
            sortingParamsList.ItemsSource = CurrentReport.Options.SortingParamList;
        }

        private void UseColumnButton_Click(object sender, RoutedEventArgs e)
        {
            int i = unusedColumn.SelectedIndex;
            CurrentReport.UsedReportColumns.Add((ColumnRelation)unusedColumn.SelectedItem);
            CurrentReport.UnusedReportColumns.Remove((ColumnRelation)unusedColumn.SelectedItem);
            unusedColumn.SelectedIndex = i < unusedColumn.Items.Count ? i : 0;
        }

        private void NotUseColumnButton_Click(object sender, RoutedEventArgs e)
        {
            int i = usedColumn.SelectedIndex;
            CurrentReport.UnusedReportColumns.Add((ColumnRelation)usedColumn.SelectedItem);
            CurrentReport.UsedReportColumns.Remove((ColumnRelation)usedColumn.SelectedItem);
            usedColumn.SelectedIndex = i < usedColumn.Items.Count ? i : 0;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            typeReportBox.ItemsSource = Report.ReportNames;
            typeReportBox.DisplayMemberPath = "Value";
            
            foreach (KeyValuePair<TypeReport,string> pair in typeReportBox.ItemsSource)
            {
                if (CurrentReport.Options.TypeReport == pair.Key)
                {
                    typeReportBox.SelectedItem = pair;
                    return;
                }
            }

            UpdateReportConfig();
        }

        private void DelSortingParamButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentReport.Options.SortingParamList.Remove((SortingParam)sortingParamsList.SelectedItem);
            SetSourceForSorting();
        }

        private void AddSortingParamButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentReport.Options.SortingParamList.Add(new SortingParam());
            SetSourceForSorting();
        }

        private void SortingParamsList_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            SetSourceForSorting();
        }
    }
}
