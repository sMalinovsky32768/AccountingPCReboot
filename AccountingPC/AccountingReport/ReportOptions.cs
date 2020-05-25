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
        public bool IsInventoryNumber { get; set; } = true;
        public bool IsName { get; set; } = true;
        public bool IsCost { get; set; } = true;
        public bool IsInvoiceNumber { get; set; } = true;
        public bool IsAcquisitionDate { get; set; } = true;
        public bool IsAudience { get; set; } = true;
        public bool IsDiagonal { get; set; } = true;
        public bool IsScreenDiagonal { get; set; } = true;
        public bool IsMaxDiagonal { get; set; } = true;
        public bool IsIsElectronicDrive { get; set; } = true;
        public bool IsAspectRatio { get; set; } = true;
        public bool IsScreenInstalled { get; set; } = true;
        public bool IsProjectorTechnology { get; set; } = true;
        public bool IsScreenResolution { get; set; } = true;
        public bool IsVideoConnectors { get; set; } = true;
        public bool IsType { get; set; } = true;
        public bool IsPaperSize { get; set; } = true;
        public bool IsMotherboard { get; set; } = true;
        public bool IsCPU { get; set; } = true;
        public bool IsCores { get; set; } = true;
        public bool IsProcessorFrequency { get; set; } = true;
        public bool IsMaxProcessorFrequency { get; set; } = true;
        public bool IsRAM { get; set; } = true;
        public bool IsFrequencyRAM { get; set; } = true;
        public bool IsVCard { get; set; } = true;
        public bool IsVideoRAM { get; set; } = true;
        public bool IsSSD { get; set; } = true;
        public bool IsHDD { get; set; } = true;
        public bool IsOS { get; set; } = true;
        public bool IsScreenFrequency { get; set; } = true;
        public bool IsMatrixTechnology { get; set; } = true;
        public bool IsNumberOfPorts { get; set; } = true;
        public bool IsWiFiFrequency { get; set; } = true;
        public bool IsTotalCost { get; set; } = true;
        public bool IsTypeLicense { get; set; } = true;
        public bool IsCount { get; set; } = true;
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
