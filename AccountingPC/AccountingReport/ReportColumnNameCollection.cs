using System.Collections.ObjectModel;

namespace AccountingPC.AccountingReport
{
    internal enum ReportColumn : byte
    {
        InventoryNumber,
        Name,
        Cost,
        InvoiceNumber,
        AcquisitionDate,
        Audience,
        Diagonal,
        ScreenDiagonal,
        MaxDiagonal,
        IsElectronicDrive,
        AspectRatio,
        ScreenInstalled,
        ProjectorTechnology,
        ScreenResolution,
        VideoConnectors,
        Type,
        PaperSize,
        Motherboard,
        CPU,
        Cores,
        ProcessorFrequency,
        MaxProcessorFrequency,
        RAM,
        FrequencyRAM,
        VCard,
        VideoRAM,
        SSD,
        HDD,
        OS,
        ScreenFrequency,
        MatrixTechnology,
        NumberOfPorts,
        WiFiFrequency,
        TotalCost,
        TypeLicense,
        Count,
        IsAvailable,
        TotalCount,
        IsWorkingCondition
    }

    internal class ReportColumnName
    {
        public ReportColumnName(ReportColumn column, string name)
        {
            Column = column;
            Name = name;
        }

        public ReportColumn Column { get; set; }
        public string Name { get; set; }
    }

    internal static class ReportColumnNameCollection
    {
        public static ObservableCollection<ReportColumnName> Collection { get; } =
            new ObservableCollection<ReportColumnName>
            {
                new ReportColumnName(ReportColumn.InventoryNumber, "Инвентарный номер"),
                new ReportColumnName(ReportColumn.Type, "Тип"),
                new ReportColumnName(ReportColumn.Name, "Наименование"),
                new ReportColumnName(ReportColumn.Cost, "Цена"),
                new ReportColumnName(ReportColumn.InvoiceNumber, "Номер накладной"),
                new ReportColumnName(ReportColumn.AcquisitionDate, "Дата приобретения"),
                new ReportColumnName(ReportColumn.Audience, "Аудитория"),
                new ReportColumnName(ReportColumn.Diagonal, "Диагональ"),
                new ReportColumnName(ReportColumn.ScreenDiagonal, "Диагональ экрана"),
                new ReportColumnName(ReportColumn.MaxDiagonal, "Максимальная диагональ"),
                new ReportColumnName(ReportColumn.IsElectronicDrive, "С электроприводом"),
                new ReportColumnName(ReportColumn.AspectRatio, "Соотношение сторон"),
                new ReportColumnName(ReportColumn.ScreenInstalled, "Установка экрана"),
                new ReportColumnName(ReportColumn.ProjectorTechnology, "Технология проецирования"),
                new ReportColumnName(ReportColumn.ScreenResolution, "Максимальное разрешение"),
                new ReportColumnName(ReportColumn.VideoConnectors, "Видеоразъемы"),
                new ReportColumnName(ReportColumn.PaperSize, "Максимальный формат"),
                new ReportColumnName(ReportColumn.Motherboard, "Материнская плата"),
                new ReportColumnName(ReportColumn.CPU, "Процессор"),
                new ReportColumnName(ReportColumn.Cores, "Количество ядер"),
                new ReportColumnName(ReportColumn.ProcessorFrequency, "Базовая частота"),
                new ReportColumnName(ReportColumn.MaxProcessorFrequency, "Максимальная частота"),
                new ReportColumnName(ReportColumn.RAM, "ОЗУ"),
                new ReportColumnName(ReportColumn.FrequencyRAM, "Частота памяти"),
                new ReportColumnName(ReportColumn.VCard, "Видеокарта"),
                new ReportColumnName(ReportColumn.VideoRAM, "Видеопамять"),
                new ReportColumnName(ReportColumn.SSD, "Объем SSD"),
                new ReportColumnName(ReportColumn.HDD, "Объем HDD"),
                new ReportColumnName(ReportColumn.OS, "Операционная система"),
                new ReportColumnName(ReportColumn.ScreenFrequency, "Частота обновления"),
                new ReportColumnName(ReportColumn.MatrixTechnology, "Технология изготовления матрицы"),
                new ReportColumnName(ReportColumn.NumberOfPorts, "Количество портов"),
                new ReportColumnName(ReportColumn.WiFiFrequency, "Частота Wi-Fi"),
                new ReportColumnName(ReportColumn.TotalCost, "Общая стоимость"),
                new ReportColumnName(ReportColumn.TypeLicense, "Тип лицензии"),
                new ReportColumnName(ReportColumn.Count, "Количество"),
                new ReportColumnName(ReportColumn.IsAvailable, "Доступно"),
                new ReportColumnName(ReportColumn.TotalCount, "Общее количество"),
                new ReportColumnName(ReportColumn.IsWorkingCondition, "Состояние")
            };

        public static ReportColumnName GetColumnName(ReportColumn column)
        {
            for (var i = 0; i < Collection.Count; i++)
                if (Collection[i].Column == column)
                    return Collection[i];
            return null;
        }
    }
}