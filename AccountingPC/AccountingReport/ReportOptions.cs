using GemBox.Spreadsheet.Charts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AccountingPC.AccountingReport
{
    public enum TypeReport
    {
        Simple,
        Full,
        OnlyPC,
        OnlyNotebook,
        OnlyMonitor,
        OnlyProjector,
        OnlyBoard,
        OnlyScreen,
        OnlyPrinterScanner,
        OnlyNetworkSwitch,
        OnlyOtherEquipment,
        Software,
        OS,
        SoftAndOS,
    }

    internal enum Grouping
    {

    }

    internal enum CreateReportOptions
    {
        SaveToFile,
        OpenExcel,
        Print,
        Preview,
    }

    internal class ReportOptions : INotifyPropertyChanged
    {
        private ReportCommand addSortingParam;
        public ReportCommand AddSortingParam
        {
            get
            {
                return addSortingParam ??
                    (addSortingParam = new ReportCommand(obj =>
                    {
                        SortingParamList.Add(new SortingParam());
                        SelectedSortingParam = SortingParamList[SortingParamList.Count - 1];
                    },
                    (obj) =>
                    {
                        foreach (SortingParam param in SortingParamList)
                        {
                            if (param.ColumnName == null) return false;
                        }
                        if (Report.UsedReportColumns.Count <= SortingParamList.Count) return false;
                        if (SortingParamList.Count > 5) return false;
                        return true;
                    }));
            }
        }

        private ReportCommand delSortingParam;
        public ReportCommand DelSortingParam
        {
            get
            {
                return delSortingParam ??
                    (delSortingParam = new ReportCommand(obj =>
                    {
                        SortingParam param = obj as SortingParam;
                        if (param != null)
                        {
                            foreach (SortingParam sortingParam in SortingParamList)
                            {
                                if (param == sortingParam) SelectedSortingParam =
                                            SortingParamList[SortingParamList.IndexOf(param) < SortingParamList.Count - 2 ?
                                            SortingParamList.IndexOf(param) + 1 : 0];
                            }
                            sortingParamList.Remove(param);
                        }
                    },
                    (obj) =>
                    {
                        if (SortingParamList.Count > 0)
                            if (SelectedSortingParam != null)
                                return true;
                            else
                                return false;
                        else
                            return false;
                    }));
            }
        }

        public delegate void ReportChanged();

        public event ReportChanged TypeReportChangedEvent;
        public event ReportChanged CreateOptionsChangedEvent;

        private TypeReport typeReport;
        private ReportName reportName;
        private CreateReportOptions createOptions = CreateReportOptions.SaveToFile;
        private ObservableCollection<SortingParam> sortingParamList = new ObservableCollection<SortingParam>();
        private SortingParam selectedSortingParam;

        public TypeReport TypeReport
        {
            get => typeReport;
            set
            {
                typeReport = value;
                reportName = ReportNameCollection.GetReportName(TypeReport);
                TypeReportChangedEvent?.Invoke();
                OnPropertyChanged("TypeReport");
            }
        }
        public ReportName ReportName
        {
            get => reportName;
            set
            {
                reportName = value;
                typeReport = value.Type;
                TypeReportChangedEvent?.Invoke();
                OnPropertyChanged("ReportName");
            }
        }

        public CreateReportOptions CreateOptions
        {
            get => createOptions;
            set
            {
                createOptions = value;
                CreateOptionsChangedEvent?.Invoke();
                OnPropertyChanged("CreateOptions");
            }
        }

        public bool IsPeriod { get; set; }

        public DateTime FromDate { get; set; } = DateTime.Parse($"01.01.{DateTime.Today.Year}");
        public DateTime ToDate { get; set; } = DateTime.Parse($"31.12.{DateTime.Today.Year}");

        //private Grouping Grouping { get; set; }

        public ObservableCollection<SortingParam> SortingParamList
        {
            get => sortingParamList;
            set
            {
                sortingParamList = value;
                OnPropertyChanged("SortingParamList");
            }
        }

        public SortingParam SelectedSortingParam 
        {
            get => selectedSortingParam;
            set
            {
                selectedSortingParam = value;
                OnPropertyChanged("SelectedSortingParam");
            }
        }
        public string GetSortingString(bool isFull = false)
        {
            string temp = string.Empty;

            if (isFull) return string.Empty;

            if (SortingParamList.Count <= 0)
                return string.Empty;
            temp += " order by ";

            int i = 0;
            foreach (SortingParam param in SortingParamList)
            {
                List<ReportColumn> columns = ReportRelationCollection.Collection[TypeReport].Columns;
                if (columns.Contains(param.ColumnName.Column))
                {
                    temp += $"[{ReportColumnNameCollection.GetColumnName(param.ColumnName.Column).Name}] ";
                    if (param.OrderName.Order == SortOrder.Asc)
                        temp += "asc";
                    else
                        temp += "desc";
                    i++;
                    if (i < SortingParamList.Count)
                        temp += ", ";
                }
            }

            return temp;
        }

        private Report Report { get; set; }

        public ReportOptions(Report report) { Report = report; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
