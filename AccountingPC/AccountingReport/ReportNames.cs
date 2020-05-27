using System.Collections.ObjectModel;

namespace AccountingPC.AccountingReport
{
    internal static class ReportNames
    {
        private static readonly ObservableCollection<ReportName> reportNamesCollection = new ObservableCollection<ReportName>()
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

        public static ObservableCollection<ReportName> ReportNamesCollection => reportNamesCollection;

        public static ReportName GetReportName(TypeReport typeReport)
        {
            foreach (ReportName reportName in ReportNames.ReportNamesCollection)
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
