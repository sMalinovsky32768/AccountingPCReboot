﻿using System.Collections.ObjectModel;

namespace AccountingPC.AccountingReport
{
    public class ReportName
    {
        public TypeReport Type { get; set; }
        public string Name { get; set; }
    }

    internal static class ReportNameCollection
    {
        public static ObservableCollection<ReportName> Collection { get; } = new ObservableCollection<ReportName>
        {
            new ReportName {Type = TypeReport.Simple, Name = "Общий (оборудование)"},
            new ReportName {Type = TypeReport.Full, Name = "Полный"},
            new ReportName {Type = TypeReport.OnlyPC, Name = "Компьютеры"},
            new ReportName {Type = TypeReport.OnlyNotebook, Name = "Ноутбуки и Моноблоки"},
            new ReportName {Type = TypeReport.OnlyMonitor, Name = "Мониторы"},
            new ReportName {Type = TypeReport.OnlyProjector, Name = "Проекторы"},
            new ReportName {Type = TypeReport.OnlyBoard, Name = "Интерактивное оборудование"},
            new ReportName {Type = TypeReport.OnlyScreen, Name = "Экраны для проекторов"},
            new ReportName {Type = TypeReport.OnlyPrinterScanner, Name = "Принтеры и сканеры"},
            new ReportName {Type = TypeReport.OnlyNetworkSwitch, Name = "Сетевое оборудование"},
            new ReportName {Type = TypeReport.OnlyOtherEquipment, Name = "Прочее оборудование"},
            new ReportName {Type = TypeReport.Software, Name = "Программное обеспечение"},
            new ReportName {Type = TypeReport.OS, Name = "Операционные системы"},
            new ReportName {Type = TypeReport.SoftAndOS, Name = "Общий (ПО&ОС)"},
            new ReportName {Type = TypeReport.UseSoft, Name = "Использование программного обеспечения"}
        };

        public static ReportName GetReportName(TypeReport typeReport)
        {
            for (var i = 0; i < Collection.Count; i++)
                if (Collection[i].Type == typeReport)
                    return Collection[i];
            return null;
        }
    }
}