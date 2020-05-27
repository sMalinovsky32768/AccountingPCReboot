using System.Collections.ObjectModel;

namespace AccountingPC.AccountingReport
{
    internal enum ReportColumn
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
    }

    internal class ColumnRelation
    {
        public ReportColumn Column { get; set; }
        public string Name { get; set; }

        public ColumnRelation(ReportColumn column, string name)
        {
            Column = column;
            Name = name;
        }
    }

    internal static class ReportColumnRelation
    {
        public static ObservableCollection<ColumnRelation> ColumnRelationships { get; set; } = new ObservableCollection<ColumnRelation>();
        static ReportColumnRelation()
        {
            ColumnRelationships.Add(new ColumnRelation(ReportColumn.InventoryNumber, "Инвентарный номер"));
            ColumnRelationships.Add(new ColumnRelation(ReportColumn.Name, "Наименование"));
            ColumnRelationships.Add(new ColumnRelation(ReportColumn.Cost, "Цена"));
            ColumnRelationships.Add(new ColumnRelation(ReportColumn.InvoiceNumber, "Номер накладной"));
            ColumnRelationships.Add(new ColumnRelation(ReportColumn.AcquisitionDate, "Дата приобретения"));
            ColumnRelationships.Add(new ColumnRelation(ReportColumn.Audience, "Аудитория"));
            ColumnRelationships.Add(new ColumnRelation(ReportColumn.Diagonal, "Диагональ"));
            ColumnRelationships.Add(new ColumnRelation(ReportColumn.ScreenDiagonal, "Диагональ экрана"));
            ColumnRelationships.Add(new ColumnRelation(ReportColumn.MaxDiagonal, "Максимальная диагональ"));
            ColumnRelationships.Add(new ColumnRelation(ReportColumn.IsElectronicDrive, "С электроприводом"));
            ColumnRelationships.Add(new ColumnRelation(ReportColumn.AspectRatio, "Соотношение сторон"));
            ColumnRelationships.Add(new ColumnRelation(ReportColumn.ScreenInstalled, "Установка экрана"));
            ColumnRelationships.Add(new ColumnRelation(ReportColumn.ProjectorTechnology, "Технология проецирования"));
            ColumnRelationships.Add(new ColumnRelation(ReportColumn.ScreenResolution, "Максимальное разрешение"));
            ColumnRelationships.Add(new ColumnRelation(ReportColumn.VideoConnectors, "Видеоразъемы"));
            ColumnRelationships.Add(new ColumnRelation(ReportColumn.Type, "Тип"));
            ColumnRelationships.Add(new ColumnRelation(ReportColumn.PaperSize, "Максимальный формат"));
            ColumnRelationships.Add(new ColumnRelation(ReportColumn.Motherboard, "Материнская плата"));
            ColumnRelationships.Add(new ColumnRelation(ReportColumn.CPU, "Процессор"));
            ColumnRelationships.Add(new ColumnRelation(ReportColumn.Cores, "Количество ядер"));
            ColumnRelationships.Add(new ColumnRelation(ReportColumn.ProcessorFrequency, "Базовая частота"));
            ColumnRelationships.Add(new ColumnRelation(ReportColumn.MaxProcessorFrequency, "Максимальная частота"));
            ColumnRelationships.Add(new ColumnRelation(ReportColumn.RAM, "ОЗУ"));
            ColumnRelationships.Add(new ColumnRelation(ReportColumn.FrequencyRAM, "Частота памяти"));
            ColumnRelationships.Add(new ColumnRelation(ReportColumn.VCard, "Видеокарта"));
            ColumnRelationships.Add(new ColumnRelation(ReportColumn.VideoRAM, "Видеопамять"));
            ColumnRelationships.Add(new ColumnRelation(ReportColumn.SSD, "Объем SSD"));
            ColumnRelationships.Add(new ColumnRelation(ReportColumn.HDD, "Объем HDD"));
            ColumnRelationships.Add(new ColumnRelation(ReportColumn.OS, "Операционная система"));
            ColumnRelationships.Add(new ColumnRelation(ReportColumn.ScreenFrequency, "Частота обновления"));
            ColumnRelationships.Add(new ColumnRelation(ReportColumn.MatrixTechnology, "Технология изготовления матрицы"));
            ColumnRelationships.Add(new ColumnRelation(ReportColumn.NumberOfPorts, "Количество портов"));
            ColumnRelationships.Add(new ColumnRelation(ReportColumn.WiFiFrequency, "Частота Wi-Fi"));
            ColumnRelationships.Add(new ColumnRelation(ReportColumn.TotalCost, "Общая стоимость"));
            ColumnRelationships.Add(new ColumnRelation(ReportColumn.TypeLicense, "Тип лицензии"));
            ColumnRelationships.Add(new ColumnRelation(ReportColumn.Count, "Количество"));
        }

        public static ColumnRelation GetColumnName(ReportColumn column)
        {
            foreach (ColumnRelation relation in ColumnRelationships)
            {
                if (relation.Column == column)
                {
                    return relation;
                }
            }
            return null;
        }
    }
}
