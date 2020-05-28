using System.Collections.ObjectModel;

namespace AccountingPC.AccountingReport
{
    public class ReportName
    {
        public TypeReport Type { get; set; }
        public string Name { get; set; }

        //public static bool operator ==(ReportName value1, ReportName value2) => (value1.Type == value2.Type) && (value1.Name == value2.Name);

        //public static bool operator !=(ReportName value1, ReportName value2) => (value1.Type != value2.Type) || (value1.Name != value2.Name);

        //public override bool Equals(object obj)
        //{
        //    return this == (ReportName)obj;
        //}

        //public override int GetHashCode()
        //{
        //    return base.GetHashCode();
        //}
    }

    internal static class ReportNameCollection
    {
        private static readonly ObservableCollection<ReportName> collection = new ObservableCollection<ReportName>()
        {
            new ReportName() {Type = TypeReport.Simple,             Name = "Общий (оборудование)" },
            new ReportName() {Type = TypeReport.Full,               Name = "Полный" },
            new ReportName() {Type = TypeReport.OnlyPC,             Name = "Компьютеры" },
            new ReportName() {Type = TypeReport.OnlyNotebook,       Name = "Ноутбуки и Моноблоки" },
            new ReportName() {Type = TypeReport.OnlyMonitor,        Name = "Мониторы" },
            new ReportName() {Type = TypeReport.OnlyProjector,      Name = "Проекторы" },
            new ReportName() {Type = TypeReport.OnlyBoard,          Name = "Интерактивные доски" },
            new ReportName() {Type = TypeReport.OnlyScreen,         Name = "Экраны для проекторов" },
            new ReportName() {Type = TypeReport.OnlyPrinterScanner, Name = "Принтеры и сканеры" },
            new ReportName() {Type = TypeReport.OnlyNetworkSwitch,  Name = "Сетевое оборудование" },
            new ReportName() {Type = TypeReport.OnlyOtherEquipment, Name = "Прочее оборудование" },
            new ReportName() {Type = TypeReport.Software,           Name = "Программное обеспечение" },
            new ReportName() {Type = TypeReport.OS,                 Name = "Операционные системы" },
            new ReportName() {Type = TypeReport.SoftAndOS,          Name = "Общий (ПО&ОС)" },
        };

        public static ObservableCollection<ReportName> Collection => collection;

        public static ReportName GetReportName(TypeReport typeReport)
        {
            foreach (ReportName reportName in ReportNameCollection.Collection)
            {
                if (reportName.Type == typeReport)
                {
                    return reportName;
                }
            }
            return null;
        }
    }
}
