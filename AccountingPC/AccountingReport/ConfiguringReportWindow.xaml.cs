﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            //typeReportBox.ItemsSource = Report.ReportNames;
            //typeReportBox.DisplayMemberPath = "Value";
            typeReportBox.ItemsSource = Report.ReportNamesCollection;
            typeReportBox.DisplayMemberPath = "Name";
            CurrentReport = new Report();
            CurrentReport.Options.TypeReportChangedEvent += TypeReportChangedEventHandler;
            CurrentReport.Options.CreateOptionsChangedEvent += Options_CreateOptionsChangedEvent;
            CurrentReport.Options.TypeReport = typeReport;
            //typeReportBox.SetBinding(ComboBox.SelectedItemProperty, "CurrentReport.Options.ReportName");
            sortingParamsList.ItemContainerGenerator.StatusChanged += ItemContainerGenerator_StatusChanged;
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
                var containers = sortingParamsList.Items.Cast<object>().Select(
                    item => (FrameworkElement)sortingParamsList.ItemContainerGenerator.ContainerFromItem(item));
                foreach(var container in containers)
                {
                    container.Loaded += Container_Loaded;
                }
            }
        }

        private void Container_Loaded(object sender, RoutedEventArgs e)
        {
            var element = (FrameworkElement)sender;
            element.Loaded -= Container_Loaded;

            var grid = VisualTreeHelper.GetChild(element, 0) as Grid;

            SetSourceForSorting();
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

                //SetSourceForSorting();
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
                else
                {
                    //listBoxItem.Visibility = Visibility.Collapsed;
                }
            }
            if (sortingParamsList.Items.Count > 0)
            {
                //_ = sortingParamsList.Items[sortingParamsList.Items.Count - 1].GetType;
            }
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //System.IO.Path.GetTempPath();
            DragMove();// Для перемещение окна
        }

        private void TypeReportBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //KeyValuePair<TypeReport, string> pair = (KeyValuePair<TypeReport, string>)typeReportBox.SelectedItem;
            //CurrentReport = new Report(pair.Key);
            if (typeReportBox.SelectedItem != null)
                CurrentReport.Options.TypeReport = ((ReportName)typeReportBox.SelectedItem).Type;

            //CurrentReport.Options.TypeReport = pair.Key;
        }

        private void TypeReportChangedEventHandler()
        {
            foreach (ReportName reportName in ((ObservableCollection<ReportName>)typeReportBox.ItemsSource))
            {
                if (reportName.Type == CurrentReport.Options.TypeReport)
                {
                    if (((ReportName)typeReportBox?.SelectedItem)?.Type != reportName.Type)
                    {
                        
                    }
                    typeReportBox.SelectedItem = reportName;
                    break;
                }
            }

            sortingParamsList.ItemsSource = CurrentReport.Options.SortingParamList;
            UpdateReportConfig();
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
            foreach (ReportName reportName in typeReportBox.ItemsSource)
            {
                if (CurrentReport.Options.TypeReport == reportName.Type)
                {
                    typeReportBox.SelectedItem = reportName;
                    return;
                }
            }

            UpdateReportConfig();
        }

        private void DelSortingParamButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentReport.Options.SortingParamList.Remove((SortingParam)sortingParamsList.SelectedItem);
            //SetSourceForSorting();
        }

        private void AddSortingParamButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentReport.Options.SortingParamList.Add(new SortingParam());
            //SetSourceForSorting();
        }

        private void SortingParamsList_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            //SetSourceForSorting();
        }

        private void CreateReport_Click(object sender, RoutedEventArgs e)
        {
            //CurrentReport.CreateReport().Save();
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
    }
}