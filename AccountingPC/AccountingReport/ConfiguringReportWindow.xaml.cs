﻿using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace AccountingPC.AccountingReport
{
    /// <summary>
    /// Interaction logic for ConfiguringReportWindow.xaml
    /// </summary>
    public partial class ConfiguringReportWindow : Window
    {
        internal Report CurrentReport { get; set; }

        private string fileName;

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
                case CreateReportOptions.SaveToFile:
                    IsSaveReport.IsChecked = true;
                    break;
                case CreateReportOptions.OpenExcel:
                    IsOperReport.IsChecked = true;
                    break;
                case CreateReportOptions.Print:
                    IsPrintReport.IsChecked = true;
                    break;
                case CreateReportOptions.Preview:
                    IsPreviewReport.IsChecked = true;
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

            Grid grid = VisualTreeHelper.GetChild(element, 0) as Grid;

            SetSourceForSorting();
        }

        public void UpdateReportConfig()
        {
            if (CurrentReport.Options.TypeReport != TypeReport.Full)
            {
                selectionColumnGrid.IsEnabled = true;
                selectionSortingParamGrid.IsEnabled = true;

                //unusedColumn.ItemsSource = CurrentReport.UnusedReportColumns;
                //unusedColumn.DisplayMemberPath = "Name";

                //usedColumn.ItemsSource = CurrentReport.UsedReportColumns;
                //usedColumn.DisplayMemberPath = "Name";

                //SetSourceForSorting();
            }
            else
            {
                //unusedColumn.ItemsSource = null;
                //usedColumn.ItemsSource = null;
                //sortingParamsList.ItemsSource = null;

                //selectionColumnGrid.IsEnabled = false;
                //selectionSortingParamGrid.IsEnabled = false;
            }
        }

        private childItem FindVisualChild<childItem>(DependencyObject obj) where childItem : DependencyObject
        {
            if (obj != null)
            {
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
                        {
                            return childOfChild;
                        }
                    }
                }
            }

            return null;
        }

        private void SetSourceForSorting()
        {
            ObservableCollection<ReportColumnName> relations = new ObservableCollection<ReportColumnName>();
            ObservableCollection<OrderName> orderNames = new ObservableCollection<OrderName>();
            //foreach (object item in sortingParamsList.Items)
            //{
            //    ListBoxItem listBoxItem = (ListBoxItem)(sortingParamsList?.ItemContainerGenerator?.ContainerFromItem(item));
            //    ContentPresenter contentPresenter = FindVisualChild<ContentPresenter>(listBoxItem);
            //    DataTemplate template = contentPresenter?.ContentTemplate;
            //    if (template != null)
            //    {
            //        ComboBox colBox = (ComboBox)template?.FindName("col", contentPresenter);
            //        ComboBox orderBox = (ComboBox)template?.FindName("order", contentPresenter);

            //        colBox.ItemsSource = CurrentReport.UsedReportColumns.Except(relations);
            //        orderBox.ItemsSource = OrderNameCollection.Collection;
            //        //orderBox.ItemsSource = SortOrderRelation.OrderNames.Except(orderNames);
            //        if (colBox.SelectedItem != null)
            //        {
            //            relations.Add((ReportColumnName)colBox.SelectedItem);
            //        }
            //        if (orderBox.SelectedItem != null)
            //        {
            //            orderNames.Add((OrderName)orderBox.SelectedItem);
            //        }
            //    }
            //    else
            //    {
            //        //listBoxItem.Visibility = Visibility.Collapsed;
            //    }
            //}
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
            DragMove();// Для перемещение окна
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
            //foreach (ReportName reportName in ((ObservableCollection<ReportName>)typeReportBox.ItemsSource))
            //{
            //    if (CurrentReport.Options.TypeReport == reportName.Type)
            //    {
            //        typeReportBox.SelectedItem = reportName;
            //        return;
            //    }
            //}
            for (int i = 0; i < ((ObservableCollection<ReportName>)typeReportBox.ItemsSource).Count; i++)
            {
                ReportName reportName = ((ObservableCollection<ReportName>)typeReportBox.ItemsSource)[i];
                if (CurrentReport.Options.TypeReport == reportName.Type)
                {
                    typeReportBox.SelectedItem = reportName;
                    return;
                }
            }

            UpdateReportConfig();
        }

        private void CreateReport_Click(object sender, RoutedEventArgs e)
        {
            switch (CurrentReport.Options.CreateOptions)
            {
                case CreateReportOptions.SaveToFile:
                    Thread save = new Thread(new ParameterizedThreadStart(SaveReport))
                    {
                        IsBackground = false,
                    };
                    save.Start(GetFileName());
                    //SaveReport();
                    break;
                case CreateReportOptions.OpenExcel:
                    //OpenReport();
                    Thread open = new Thread(new ThreadStart(OpenReport))
                    {
                        IsBackground = false,
                    };
                    open.Start();
                    break;
                case CreateReportOptions.Print:
                    Thread print = new Thread(new ThreadStart(PrintReport))
                    {
                        IsBackground = false,
                    };
                    print.Start();
                    break;
                case CreateReportOptions.Preview:
                    Thread preview = new Thread(new ThreadStart(PreviewReport))
                    {
                        IsBackground = false,
                    };
                    preview.Start();
                    break;
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void IsSaveReport_Checked(object sender, RoutedEventArgs e)
        {
            CurrentReport.Options.CreateOptions = CreateReportOptions.SaveToFile;
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

        private string GetFileName()
        {
            Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "XLSX files (*.xlsx, *.xlsm, *.xltx, *.xltm)|*.xlsx;*.xlsm;*.xltx;*.xltm|XLS files (*.xls, *.xlt)|*.xls;*.xlt|ODS files (*.ods, *.ots)|*.ods;*.ots|CSV files (*.csv, *.tsv)|*.csv;*.tsv|HTML files (*.html, *.htm)|*.html;*.htm|Portable Document Format|*.pdf",
                //Filter = "SpreadSheet|*.xlsx;*.xls;*.csv;*.ods|Portable Document Format|*.pdf|HTML|*.html|Image|*.png;*.jpg;*.gif;.*bmp",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                FileName = $"Report_{CurrentReport.Options.TypeReport}__{DateTime.Now:dd-MM-yyyy__HH-mm-ss__g}.xlsx"
            };
            if (dialog.ShowDialog(this) == false)
            {
                return null;
            }

            return dialog.FileName;
        }

        private void OpenReport()
        {
            fileName = $@"{Path.GetTempPath()}Report_{CurrentReport.Options.TypeReport}__{DateTime.Now:dd-MM-yyyy__HH-mm-ss__g}.xlsx";
            Task task = new Task(() =>
            {
                Dispatcher.Invoke(() =>
                {
                    SaveReport(fileName);

                    Process excel = new Process
                    {
                        StartInfo = new ProcessStartInfo("excel.exe", $"/n {fileName}"),
                        EnableRaisingEvents = true
                    };
                    excel.Exited += (sender, e) => new FileInfo(fileName)?.Delete();
                    excel.Start();
                });
            });
            task.Start();
        }

        private void SaveReport(string fileName)
        {
            try
            {
                if (!string.IsNullOrEmpty(fileName))
                {
                    CurrentReport.CreateReport().Save(fileName);
                }
            }
            catch
            {

            }
        }

        private void SaveReport(object fileName)
        {
            try
            {
                Task task = new Task(() =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        SaveReport(fileName as string);
                    });
                });
                task.Start();
            }
            catch
            {

            }
        }

        private void PrintReport()
        {
            Task task = new Task(() =>
            {
                Dispatcher.Invoke(() =>
                {
                    CurrentReport.CreateReport().Print();
                });
            });
            task.Start();
        }

        private void PreviewReport()
        {
            Task task = new Task(() =>
            {
                Dispatcher.Invoke(() =>
                {
                    new PreviewReportWindow(CurrentReport.CreateReport()).ShowDialog();
                });
            });
            task.Start();
        }

        private void FromDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            //toDate.BlackoutDates.Add(new CalendarDateRange(DateTime.Parse("01.01.2000"), (DateTime)e.AddedItems[0]));
            toDate.BlackoutDates.Add(new CalendarDateRange(DateTime.MinValue, (DateTime)e.AddedItems[0]));
            toDate.BlackoutDates.Add(new CalendarDateRange(DateTime.Today.AddDays(1), DateTime.MaxValue));
        }

        private void ToDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            //fromDate.BlackoutDates.Add(new CalendarDateRange((DateTime)e.AddedItems[0], DateTime.Today));
            fromDate.BlackoutDates.Add(new CalendarDateRange(DateTime.MinValue, DateTime.Parse("31.12.1999")));
            fromDate.BlackoutDates.Add(new CalendarDateRange((DateTime)e.AddedItems[0], DateTime.MaxValue));
        }

        private void Split_Checked(object sender, RoutedEventArgs e)
        {
            selectionColumnGrid.Visibility = Visibility.Collapsed;
        }

        private void Split_Unchecked(object sender, RoutedEventArgs e)
        {
            selectionColumnGrid.Visibility = Visibility.Visible;
        }
    }

    internal static class OpenExcel
    {
        private static readonly Process excel = new Process();
        private static string fileName;

        public static void Open(string fName)
        {
            fileName = fName;
            excel.StartInfo = new ProcessStartInfo("excel.exe", $"/n {fileName}");
            excel.EnableRaisingEvents = true;
            excel.Exited += (sender, e) => new FileInfo(fileName)?.Delete();
            excel.Start();

            //excel = Process.Start("excel.exe", $"/n {fileName}");
            //excel.WaitForExit();
        }
    }
}
