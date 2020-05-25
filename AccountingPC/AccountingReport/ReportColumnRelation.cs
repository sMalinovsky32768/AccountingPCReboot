using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    internal static class ReportColumnRelation
    {
        public static Dictionary<ReportColumn, String> ColumnRelationships { get; set; }
        static ReportColumnRelation()
        {
            ColumnRelationships.Add(ReportColumn.InventoryNumber, "Инвентарный номер");
            ColumnRelationships.Add(ReportColumn.Name, "Наименование");
            ColumnRelationships.Add(ReportColumn.Cost, "Цена");
            ColumnRelationships.Add(ReportColumn.InvoiceNumber, "Номер накладной");
            ColumnRelationships.Add(ReportColumn.AcquisitionDate, "Дата приобретения");
            ColumnRelationships.Add(ReportColumn.Audience, "Аудитория");
            ColumnRelationships.Add(ReportColumn.Diagonal, "Диагональ");
            ColumnRelationships.Add(ReportColumn.ScreenDiagonal, "Диагональ экрана");
            ColumnRelationships.Add(ReportColumn.MaxDiagonal, "Максимальная диагональ");
            ColumnRelationships.Add(ReportColumn.IsElectronicDrive, "С электроприводом");
            ColumnRelationships.Add(ReportColumn.AspectRatio, "Соотношение сторон");
            ColumnRelationships.Add(ReportColumn.ScreenInstalled, "Установка экрана");
            ColumnRelationships.Add(ReportColumn.ProjectorTechnology, "Технология проецирования");
            ColumnRelationships.Add(ReportColumn.ScreenResolution, "Максимальное разрешение");
            ColumnRelationships.Add(ReportColumn.VideoConnectors, "VideoConnectors");
            ColumnRelationships.Add(ReportColumn.Type, "Тип");
            ColumnRelationships.Add(ReportColumn.PaperSize, "Максимальный формат");
            ColumnRelationships.Add(ReportColumn.Motherboard, "Материнская плата");
            ColumnRelationships.Add(ReportColumn.CPU, "Процессор");
            ColumnRelationships.Add(ReportColumn.Cores, "Количество ядер");
            ColumnRelationships.Add(ReportColumn.ProcessorFrequency, "Базовая частота");
            ColumnRelationships.Add(ReportColumn.MaxProcessorFrequency, "Максимальная частота");
            ColumnRelationships.Add(ReportColumn.RAM, "ОЗУ");
            ColumnRelationships.Add(ReportColumn.FrequencyRAM, "Частота памяти");
            ColumnRelationships.Add(ReportColumn.VCard, "Видеокарта");
            ColumnRelationships.Add(ReportColumn.VideoRAM, "Видеопамять");
            ColumnRelationships.Add(ReportColumn.SSD, "Объем SSD");
            ColumnRelationships.Add(ReportColumn.HDD, "Объем HDD");
            ColumnRelationships.Add(ReportColumn.OS, "Операционная система");
            ColumnRelationships.Add(ReportColumn.ScreenFrequency, "Частота обновления");
            ColumnRelationships.Add(ReportColumn.MatrixTechnology, "Технология изготовления матрицы");
            ColumnRelationships.Add(ReportColumn.NumberOfPorts, "Количество портов");
            ColumnRelationships.Add(ReportColumn.WiFiFrequency, "Частота Wi-Fi");
            ColumnRelationships.Add(ReportColumn.TotalCost, "Общая стоимость");
            ColumnRelationships.Add(ReportColumn.TypeLicense, "Тип лицензии");
            ColumnRelationships.Add(ReportColumn.Count, "Количество");
        }
    }
}
