using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

    internal enum SortOrder
    {
        Asc,
        Desc,
        //Default,
    }

    internal enum CreateReportOptions
    {
        SaveToFile,
        OpenExcel,
        Print,
        Preview,
    }

    internal class OrderName
    {
        public SortOrder Order { get; set; }
        public string Name { get; set; }
    }

    internal static class SortOrderRelation
    {
        public static ObservableCollection<OrderName> OrderNames { get; private set; } = new ObservableCollection<OrderName>()
        {
            new OrderName()
            {
                Order=SortOrder.Asc,
                Name="По возрастанию",
            },
            new OrderName(){
                Order=SortOrder.Desc,
                Name="По убыванию",
            },
            //new OrderName(){
            //    Order=SortOrder.Default,
            //    Name="По умолчанию",
            //},
        };

        public static string GetOrderName(SortOrder order)
        {
            foreach (OrderName orderName in OrderNames)
            {
                if (orderName.Order == order)
                {
                    return orderName.Name;
                }
            }
            return String.Empty;
        }
    }

    /// <summary>
    /// Параметр сортировки. Если IsAscending = true, сортировка выполняется по возрастанию, иначе по убыванию
    /// </summary>
    internal class SortingParam
    {
        private ReportColumn column;
        private SortOrder order;
        private ColumnRelation columnRelation;
        private OrderName orderName;

        public ReportColumn Column
        {
            get => column;
            set
            {
                column = value;
                columnRelation = new ColumnRelation(value, ReportColumnRelation.GetColumnName(value));
            }
        }
        public SortOrder Order
        {
            get => order;
            set
            {
                order = value;
                orderName = new OrderName() { Order = value, Name = SortOrderRelation.GetOrderName(value) };
            }
        }

        public ColumnRelation ColumnRelation 
        {
            get => columnRelation;
            set
            {
                columnRelation = value;
                column = value.Column;
            }
        }
        public OrderName OrderName
        {
            get => orderName;
            set
            {
                orderName = value;
                order = value.Order;
            }
        }
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
                reportName = Report.GetReportName(TypeReport);
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
        private Grouping Grouping { get; set; }

        public ObservableCollection<SortingParam> SortingParamList { get; set; } = new ObservableCollection<SortingParam>();

        public string GetSortingString()
        {
            string temp = string.Empty;

            if (SortingParamList.Count > 0)
                return string.Empty;
            temp += " order by ";

            int i = 0;
            foreach (SortingParam param in SortingParamList)
            {
                List<ReportColumn> columns = Report.Relation[TypeReport].Columns;
                if (columns.Contains(param.Column))
                {
                    temp += $"[{ReportColumnRelation.GetColumnName(param.Column)}] ";
                    if (param.Order == SortOrder.Asc)
                        temp += "asc";
                    else
                        temp += "desc";
                    if (i < SortingParamList.Count)
                        temp += ", ";
                    i++;
                }
            }

            return temp;
        }

        public ReportOptions() { SortingParamList.CollectionChanged += SortingParamList_CollectionChanged; }

        public ReportOptions(TypeReport typeReport)
        {
            TypeReport = TypeReport;
            SortingParamList = new ObservableCollection<SortingParam>()
            {
                new SortingParam()
                {
                    Column = ReportColumn.InventoryNumber,
                    Order = SortOrder.Asc,
                }
            };
            SortingParamList.CollectionChanged += SortingParamList_CollectionChanged;
        }

        private void SortingParamList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            //if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            //{
            //    foreach (SortingParam param in e.NewItems)
            //    {
            //        foreach (SortingParam param1 in SortingParamList)
            //        {
            //            if (param.Column == param1.Column)
            //            {
            //                SortingParamList.Remove(param);
            //            }
            //        }
            //    }
            //}
        }
    }
}
