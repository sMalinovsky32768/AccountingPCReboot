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
        public delegate void TypeReportChanged();
        public event TypeReportChanged TypeReportChangedEvent;

        private TypeReport typeReport;
        public TypeReport TypeReport {
            get => typeReport;
            set
            {
                typeReport = value;
                TypeReportChangedEvent?.Invoke();
            }
        }
        //public bool IsInventoryNumber { get; set; } = true;
        //public bool IsName { get; set; } = true;
        //public bool IsCost { get; set; } = true;
        //public bool IsInvoiceNumber { get; set; } = true;
        //public bool IsAcquisitionDate { get; set; } = true;
        //public bool IsAudience { get; set; } = true;
        //public bool IsDiagonal { get; set; } = true;
        //public bool IsScreenDiagonal { get; set; } = true;
        //public bool IsMaxDiagonal { get; set; } = true;
        //public bool IsIsElectronicDrive { get; set; } = true;
        //public bool IsAspectRatio { get; set; } = true;
        //public bool IsScreenInstalled { get; set; } = true;
        //public bool IsProjectorTechnology { get; set; } = true;
        //public bool IsScreenResolution { get; set; } = true;
        //public bool IsVideoConnectors { get; set; } = true;
        //public bool IsType { get; set; } = true;
        //public bool IsPaperSize { get; set; } = true;
        //public bool IsMotherboard { get; set; } = true;
        //public bool IsCPU { get; set; } = true;
        //public bool IsCores { get; set; } = true;
        //public bool IsProcessorFrequency { get; set; } = true;
        //public bool IsMaxProcessorFrequency { get; set; } = true;
        //public bool IsRAM { get; set; } = true;
        //public bool IsFrequencyRAM { get; set; } = true;
        //public bool IsVCard { get; set; } = true;
        //public bool IsVideoRAM { get; set; } = true;
        //public bool IsSSD { get; set; } = true;
        //public bool IsHDD { get; set; } = true;
        //public bool IsOS { get; set; } = true;
        //public bool IsScreenFrequency { get; set; } = true;
        //public bool IsMatrixTechnology { get; set; } = true;
        //public bool IsNumberOfPorts { get; set; } = true;
        //public bool IsWiFiFrequency { get; set; } = true;
        //public bool IsTotalCost { get; set; } = true;
        //public bool IsTypeLicense { get; set; } = true;
        //public bool IsCount { get; set; } = true;
        private Grouping Grouping { get; set; }

        public ObservableCollection<SortingParam> SortingParamList { get; set; }

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
                    if (param.Order==SortOrder.Asc)
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

        public ReportOptions()
        {
            TypeReport = TypeReport.Simple;
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
