using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;

namespace AccountingPC.AccountingReport
{
    public partial class ConfiguringReportWindow : Window
    {
        public ConfiguringReportWindow(TypeReport typeReport = TypeReport.Simple)
        {
            InitializeComponent();
            typeReportBox.ItemsSource = ReportNameCollection.Collection;
            typeReportBox.DisplayMemberPath = "Name";
            CurrentReport = new Report();
            CurrentReport.Options.CreateOptionsChangedEvent += Options_CreateOptionsChangedEvent;
            CurrentReport.Options.TypeReportChangedEvent += TypeReportChangedEventHandler;
            CurrentReport.Options.TypeReport = typeReport;
            sortingParamsList.ItemContainerGenerator.StatusChanged += ItemContainerGenerator_StatusChanged;
            typeReportBox.SelectedItem = CurrentReport.Options.ReportName;

            configureGrid.DataContext = CurrentReport;
        }

        internal Report CurrentReport { get; set; }

        private void Options_CreateOptionsChangedEvent()
        {
            switch (CurrentReport.Options.CreateOptions)
            {
                case CreateReportOptions.SaveAsXlsx:
                    IsSaveReport.IsChecked = true;
                    break;
                case CreateReportOptions.SaveAsPDF:
                    IsSaveAsPDF.IsChecked = true;
                    break;
                case CreateReportOptions.OpenExcel:
                    IsOperReport.IsChecked = true;
                    break;
                case CreateReportOptions.Print:
                    break;
                case CreateReportOptions.Preview:
                    break;
            }
        }

        private void ItemContainerGenerator_StatusChanged(object sender, EventArgs e)
        {
            if (sortingParamsList.ItemContainerGenerator.Status ==
                GeneratorStatus.ContainersGenerated)
            {
                var containers = sortingParamsList.Items.Cast<object>().Select(
                    item => (FrameworkElement) sortingParamsList.ItemContainerGenerator.ContainerFromItem(item));
                foreach (var container in containers) container.Loaded += Container_Loaded;
            }
        }

        private void Container_Loaded(object sender, RoutedEventArgs e)
        {
            var element = (FrameworkElement) sender;
            element.Loaded -= Container_Loaded;

            SetSourceForSorting();
        }

        private childItem FindVisualChild<childItem>(DependencyObject obj) where childItem : DependencyObject
        {
            if (obj != null)
                for (var i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
                {
                    var child = VisualTreeHelper.GetChild(obj, i);
                    if (child != null && child is childItem item)
                    {
                        return item;
                    }

                    var childOfChild = FindVisualChild<childItem>(child);
                    if (childOfChild != null) return childOfChild;
                }

            return null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetSourceForSorting()
        {
            var relations = new ObservableCollection<ReportColumnName>();
            var orderNames = new ObservableCollection<OrderName>();
            for (var i = 0; i < sortingParamsList.Items.Count; i++)
            {
                var item = sortingParamsList.Items[i];
                var listBoxItem = (ListBoxItem) sortingParamsList?.ItemContainerGenerator?.ContainerFromItem(item);
                var contentPresenter = FindVisualChild<ContentPresenter>(listBoxItem);
                var template = contentPresenter?.ContentTemplate;
                if (template != null)
                {
                    var colBox = (ComboBox) template?.FindName("col", contentPresenter);
                    var orderBox = (ComboBox) template?.FindName("order", contentPresenter);

                    colBox.ItemsSource = CurrentReport.UsedReportColumns.Except(relations);
                    orderBox.ItemsSource = OrderNameCollection.Collection;
                    if (colBox.SelectedItem != null) relations.Add((ReportColumnName) colBox.SelectedItem);

                    if (orderBox.SelectedItem != null) orderNames.Add((OrderName) orderBox.SelectedItem);
                }
            }
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void TypeReportChangedEventHandler()
        {
            if (CurrentReport.Options.TypeReport == TypeReport.Full)
            {
                selectionSortingParamGrid.Visibility = Visibility.Collapsed;
                selectionColumnGrid.Visibility = Visibility.Collapsed;
            }
            else
            {
                selectionSortingParamGrid.Visibility = Visibility.Visible;
                selectionColumnGrid.Visibility = Visibility.Visible;
            }

            if (CurrentReport.Options.TypeReport == TypeReport.UseSoft)
            {
                CurrentReport.Options.IsPeriod = false;
                periodGrid.Visibility = Visibility.Collapsed;
            }
            else
            {
                periodGrid.Visibility = Visibility.Visible;
            }

            if (CurrentReport.Options.TypeReport == TypeReport.UseSoft ||
                CurrentReport.Options.TypeReport == TypeReport.Software ||
                CurrentReport.Options.TypeReport == TypeReport.SoftAndOS ||
                CurrentReport.Options.TypeReport == TypeReport.OS)
            {
                CurrentReport.Options.SplitByAudience = false;
                split.Visibility = Visibility.Collapsed;
            }
            else
            {
                split.Visibility = Visibility.Visible;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var count = ((ObservableCollection<ReportName>) typeReportBox.ItemsSource).Count;
            for (var i = 0; i < count; i++)
            {
                var reportName = ((ObservableCollection<ReportName>) typeReportBox.ItemsSource)[i];
                if (CurrentReport.Options.TypeReport == reportName.Type)
                {
                    typeReportBox.SelectedItem = reportName;
                    return;
                }
            }
        }

        private void CreateReport_Click(object sender, RoutedEventArgs e)
        {
            switch (CurrentReport.Options.CreateOptions)
            {
                case CreateReportOptions.SaveAsXlsx:
                    SaveAsExcel();
                    break;
                case CreateReportOptions.SaveAsPDF:
                    SaveAsPDF();
                    break;
                case CreateReportOptions.OpenExcel:
                    OpenReport();
                    break;
                case CreateReportOptions.Print:
                    break;
                case CreateReportOptions.Preview:
                    break;
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void IsSaveReport_Checked(object sender, RoutedEventArgs e)
        {
            CurrentReport.Options.CreateOptions = CreateReportOptions.SaveAsXlsx;
        }

        private void IsOperReport_Checked(object sender, RoutedEventArgs e)
        {
            CurrentReport.Options.CreateOptions = CreateReportOptions.OpenExcel;
        }

        private void IsPrintReport_Checked(object sender, RoutedEventArgs e)
        {
            CurrentReport.Options.CreateOptions = CreateReportOptions.Print;
        }

        private void IsPreviewReport_Checked(object sender, RoutedEventArgs e)
        {
            CurrentReport.Options.CreateOptions = CreateReportOptions.Preview;
        }

        private void FromDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            toDate.BlackoutDates.Add(new CalendarDateRange(DateTime.MinValue, (DateTime) e.AddedItems[0]));
            toDate.BlackoutDates.Add(new CalendarDateRange(DateTime.Today.AddDays(1), DateTime.MaxValue));
        }

        private void ToDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            fromDate.BlackoutDates.Add(new CalendarDateRange(DateTime.MinValue, DateTime.Parse("31.12.1999")));
            fromDate.BlackoutDates.Add(new CalendarDateRange((DateTime) e.AddedItems[0], DateTime.MaxValue));
        }

        private void Split_Checked(object sender, RoutedEventArgs e)
        {
            selectionSortingParamGrid.Visibility = Visibility.Collapsed;
            selectionColumnGrid.Visibility = Visibility.Collapsed;
        }

        private void Split_Unchecked(object sender, RoutedEventArgs e)
        {
            if (CurrentReport.Options.TypeReport == TypeReport.Full)
            {
                selectionSortingParamGrid.Visibility = Visibility.Collapsed;
                selectionColumnGrid.Visibility = Visibility.Collapsed;
            }
            else
            {
                selectionSortingParamGrid.Visibility = Visibility.Visible;
                selectionColumnGrid.Visibility = Visibility.Visible;
            }
        }

        private void IsSaveAsPDF_Checked(object sender, RoutedEventArgs e)
        {
            CurrentReport.Options.CreateOptions = CreateReportOptions.SaveAsPDF;
        }

        private void SaveAsExcel()
        {
            var dialog = new SaveFileDialog
            {
                Filter = "XLSX files|*.xlsx",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                FileName = $"Report_{CurrentReport.Options.TypeReport}__{DateTime.Now:dd-MM-yyyy__HH-mm-ss__g}.xlsx"
            };
            if (dialog.ShowDialog(this) == false) return;

            var filename = dialog.FileName;
            CurrentReport.CreateReportExcel(filename);
        }

        private void SaveAsPDF()
        {
            var dialog = new SaveFileDialog
            {
                Filter = "Portable Document Format|*.pdf",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                FileName = $"Report_{CurrentReport.Options.TypeReport}__{DateTime.Now:dd-MM-yyyy__HH-mm-ss__g}.pdf"
            };
            if (dialog.ShowDialog(this) == false) return;

            var filename = dialog.FileName;
            CurrentReport.CreateReportExcel(filename);
        }

        private void OpenReport()
        {
            CurrentReport.CreateReportExcel();
        }
    }
}