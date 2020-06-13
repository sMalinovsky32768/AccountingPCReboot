using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AccountingPC.AccountingReport
{
    public enum TypeReport : byte
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
        UseSoft
    }

    internal enum CreateReportOptions : byte
    {
        SaveAsXlsx,
        SaveAsPDF,
        OpenExcel,
        Print,
        Preview
    }

    internal class ReportOptions : INotifyPropertyChanged
    {
        public delegate void ReportChanged();

        private ReportCommand addSortingParam;
        private CreateReportOptions createOptions = CreateReportOptions.SaveAsXlsx;

        private ReportCommand delSortingParam;
        private ReportName reportName;
        private SortingParam selectedSortingParam;
        private ObservableCollection<SortingParam> sortingParamList = new ObservableCollection<SortingParam>();

        private TypeReport typeReport;

        public ReportOptions(Report report)
        {
            Report = report;
        }

        public ReportCommand AddSortingParam => addSortingParam ??
                                                (addSortingParam = new ReportCommand(obj =>
                                                    {
                                                        SortingParamList.Add(new SortingParam());
                                                        SelectedSortingParam =
                                                            SortingParamList[SortingParamList.Count - 1];
                                                    },
                                                    obj =>
                                                    {
                                                        for (var i = 0; i < SortingParamList.Count; i++)
                                                            if (SortingParamList[i].ColumnName == null)
                                                                return false;
                                                        if (Report.UsedReportColumns.Count <= SortingParamList.Count)
                                                            return false;

                                                        if (SortingParamList.Count > 5) return false;

                                                        return true;
                                                    }));

        public ReportCommand DelSortingParam => delSortingParam ??
                                                (delSortingParam = new ReportCommand(obj =>
                                                    {
                                                        if (obj is SortingParam param)
                                                        {
                                                            for (var i = 0; i < SortingParamList.Count; i++)
                                                                if (param == SortingParamList[i])
                                                                    SelectedSortingParam =
                                                                        SortingParamList[
                                                                            SortingParamList.IndexOf(param) <
                                                                            SortingParamList.Count - 2
                                                                                ? SortingParamList.IndexOf(param) + 1
                                                                                : 0];
                                                            sortingParamList.Remove(param);
                                                        }
                                                    },
                                                    obj =>
                                                    {
                                                        if (SortingParamList.Count > 0)
                                                        {
                                                            if (SelectedSortingParam != null)
                                                                return true;
                                                            return false;
                                                        }

                                                        return false;
                                                    }));

        public bool IsShowUnitOfMeasurement { get; set; } = true;
        public bool IsCountMaxMinAverageSum { get; set; } = true;

        public bool SplitByAudience { get; set; } = false;

        public TypeReport TypeReport
        {
            get => typeReport;
            set
            {
                typeReport = value;
                reportName = ReportNameCollection.GetReportName(TypeReport);
                TypeReportChangedEvent?.Invoke();
                OnPropertyChanged();
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
                OnPropertyChanged();
            }
        }

        public CreateReportOptions CreateOptions
        {
            get => createOptions;
            set
            {
                createOptions = value;
                CreateOptionsChangedEvent?.Invoke();
                OnPropertyChanged();
            }
        }

        public bool IsPeriod { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public ObservableCollection<SortingParam> SortingParamList
        {
            get => sortingParamList;
            set
            {
                sortingParamList = value;
                OnPropertyChanged();
            }
        }

        public SortingParam SelectedSortingParam
        {
            get => selectedSortingParam;
            set
            {
                selectedSortingParam = value;
                OnPropertyChanged();
            }
        }

        private Report Report { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public event ReportChanged TypeReportChangedEvent;
        public event ReportChanged CreateOptionsChangedEvent;

        public string GetSortingString(bool isFull = false)
        {
            var temp = string.Empty;

            if (isFull) return string.Empty;

            if (SortingParamList.Count <= 0) return string.Empty;

            temp += " order by ";

            var i = 0;
            for (var j = 0; j < SortingParamList.Count; j++)
            {
                var param = SortingParamList[j];
                var columns = ReportRelationCollection.Collection[TypeReport].Columns;
                if (columns.Contains(param.ColumnName.Column))
                {
                    temp += $"[{ReportColumnNameCollection.GetColumnName(param.ColumnName.Column).Name}] ";
                    if (param.OrderName.Order == SortOrder.Asc)
                        temp += "asc";
                    else
                        temp += "desc";

                    i++;
                    if (i < SortingParamList.Count) temp += ", ";
                }
            }

            return temp;
        }

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}