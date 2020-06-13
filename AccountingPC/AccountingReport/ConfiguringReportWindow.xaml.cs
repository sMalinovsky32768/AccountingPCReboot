using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AccountingPC.AccountingReport
{
    public partial class ConfiguringReportWindow : Window
    {
        internal Report CurrentReport { get; set; }

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
                System.Windows.Controls.Primitives.GeneratorStatus.ContainersGenerated)
            {
                System.Collections.Generic.IEnumerable<FrameworkElement> containers = sortingParamsList.Items.Cast<object>().Select(
                    item => (FrameworkElement)sortingParamsList.ItemContainerGenerator.ContainerFromItem(item));
                foreach (FrameworkElement container in containers)
                {
                    container.Loaded += Container_Loaded;
                }
            }
        }

        private void Container_Loaded(object sender, RoutedEventArgs e)
        {
            FrameworkElement element = (FrameworkElement)sender;
            element.Loaded -= Container_Loaded;

            SetSourceForSorting();
        }

        private childItem FindVisualChild<childItem>(DependencyObject obj) where childItem : DependencyObject
        {
            if (obj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                    if (child != null && child is childItem item)
                    {
                        return item;
                    }
                    else
                    {
                        childItem childOfChild = FindVisualChild<childItem>(child);
                        if (childOfChild != null)
                        {
                            return childOfChild;
                        }
                    }
                }
            }

            return null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetSourceForSorting()
        {
            ObservableCollection<ReportColumnName> relations = new ObservableCollection<ReportColumnName>();
            ObservableCollection<OrderName> orderNames = new ObservableCollection<OrderName>();
            for (int i = 0; i < sortingParamsList.Items.Count; i++)
            {
                object item = sortingParamsList.Items[i];
                ListBoxItem listBoxItem = (ListBoxItem)(sortingParamsList?.ItemContainerGenerator?.ContainerFromItem(item));
                ContentPresenter contentPresenter = FindVisualChild<ContentPresenter>(listBoxItem);
                DataTemplate template = contentPresenter?.ContentTemplate;
                if (template != null)
                {
                    ComboBox colBox = (ComboBox)template?.FindName("col", contentPresenter);
                    ComboBox orderBox = (ComboBox)template?.FindName("order", contentPresenter);

                    colBox.ItemsSource = CurrentReport.UsedReportColumns.Except(relations);
                    orderBox.ItemsSource = OrderNameCollection.Collection;
                    if (colBox.SelectedItem != null)
                    {
                        relations.Add((ReportColumnName)colBox.SelectedItem);
                    }

                    if (orderBox.SelectedItem != null)
                    {
                        orderNames.Add((OrderName)orderBox.SelectedItem);
                    }
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
            int count = ((ObservableCollection<ReportName>)typeReportBox.ItemsSource).Count;
            for (int i = 0; i < count; i++)
            {
                ReportName reportName = ((ObservableCollection<ReportName>)typeReportBox.ItemsSource)[i];
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
            toDate.BlackoutDates.Add(new CalendarDateRange(DateTime.MinValue, (DateTime)e.AddedItems[0]));
            toDate.BlackoutDates.Add(new CalendarDateRange(DateTime.Today.AddDays(1), DateTime.MaxValue));
        }

        private void ToDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            fromDate.BlackoutDates.Add(new CalendarDateRange(DateTime.MinValue, DateTime.Parse("31.12.1999")));
            fromDate.BlackoutDates.Add(new CalendarDateRange((DateTime)e.AddedItems[0], DateTime.MaxValue));
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
            Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "XLSX files|*.xlsx",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                FileName = $"Report_{CurrentReport.Options.TypeReport}__{DateTime.Now:dd-MM-yyyy__HH-mm-ss__g}.xlsx"
            };
            if (dialog.ShowDialog(this) == false)
            {
                return;
            }

            string filename = dialog.FileName;
            CurrentReport.CreateReportExcel(filename);
        }

        private void SaveAsPDF()
        {
            Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "Portable Document Format|*.pdf",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                FileName = $"Report_{CurrentReport.Options.TypeReport}__{DateTime.Now:dd-MM-yyyy__HH-mm-ss__g}.pdf"
            };
            if (dialog.ShowDialog(this) == false)
            {
                return;
            }

            string filename = dialog.FileName;
            CurrentReport.CreateReportExcel(filename);
        }

        private void OpenReport()
        {
            CurrentReport.CreateReportExcel();
        }
    }
}
