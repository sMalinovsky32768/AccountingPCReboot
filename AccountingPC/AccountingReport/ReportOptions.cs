using System.Collections.Generic;
using System.Collections.ObjectModel;

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

    internal class ReportOptions
    {
        public delegate void ReportChanged();

        public event ReportChanged TypeReportChangedEvent;
        public event ReportChanged CreateOptionsChangedEvent;

        private TypeReport typeReport;
        private ReportName reportName;
        private CreateReportOptions createOptions = CreateReportOptions.SaveToFile;

        public TypeReport TypeReport
        {
            get => typeReport;
            set
            {
                typeReport = value;
                reportName = ReportNames.GetReportName(TypeReport);
                TypeReportChangedEvent?.Invoke();
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
            }
        }

        public CreateReportOptions CreateOptions 
        {
            get => createOptions;
            set
            {
                createOptions = value;
                CreateOptionsChangedEvent?.Invoke();
            } 
        }

        //private Grouping Grouping { get; set; }

        public ObservableCollection<SortingParam> SortingParamList { get; set; } = new ObservableCollection<SortingParam>();

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
                if (columns.Contains(param.Column))
                {
                    temp += $"[{ReportColumnRelation.GetColumnName(param.Column).Name}] ";
                    if (param.Order == SortOrder.Asc)
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

        public ReportOptions() { }

        public ReportOptions(TypeReport typeReport)
        {
            TypeReport = TypeReport;
            if (typeReport != TypeReport.Full)
            {
                SortingParamList = new ObservableCollection<SortingParam>()
                {
                    new SortingParam()
                    {
                        Column = ReportColumn.InventoryNumber,
                        Order = SortOrder.Asc,
                    }
                };
            }
        }
    }
}
