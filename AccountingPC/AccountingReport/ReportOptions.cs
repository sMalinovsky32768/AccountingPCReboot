using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingPC.AccountingReport
{
    internal enum TypeReport
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

    /// <summary>
    /// Параметр сортировки. Если IsAscending = true, сортировка выполняется по возрастанию, иначе по убыванию
    /// </summary>
    internal class SortingParam
    {
        public ReportColumn Column { get; set; } = ReportColumn.InventoryNumber;
        public bool IsAscending { get; set; } = true;
    }

    internal class ReportOptions
    {
        public TypeReport TypeReport { get; set; } = TypeReport.Simple;
        public bool IsInvN { get; set; } = true;
        public bool IsType { get; set; } = true;
        public bool IsName { get; set; } = true;
        public bool IsCost { get; set; } = true;
        public bool IsInvoice { get; set; } = false;
        public bool IsAcquisitionDate { get; set; } = true;
        public bool IsLocation { get; set; } = true;
        public List<SortingParam> SortingParamList { get; set; } = new List<SortingParam>() { new SortingParam() };
        public string SortingString
        {
            get
            {
                string temp = string.Empty;
                if (SortingParamList.Count > 0)
                    temp += " order by ";

                int i = 0;

                foreach (SortingParam param in SortingParamList)
                {
                    List<ReportColumn> columns = Report.Relation[TypeReport].Columns;
                    if (columns.Contains(param.Column))
                    {
                        temp += $"[{ReportColumnRelation.ColumnRelationships[param.Column]}] ";
                        if (param.IsAscending)
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
        }
        private Grouping Grouping { get; set; }
    }
}
