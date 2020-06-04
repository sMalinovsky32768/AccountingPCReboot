using System.Collections.Generic;

namespace AccountingPC.AccountingReport
{
    internal class ReportRelationCollection
    {

        private static readonly Dictionary<TypeReport, ReportRelation> collection = new Dictionary<TypeReport, ReportRelation>()
        {
            {
                TypeReport.Simple,
                new ReportRelation()
                {
                    Function = "GetAllDevices",
                    TableName = "All Devices",
                    Columns = new List<ReportColumn>()
                    {
                        ReportColumn.InventoryNumber,
                        ReportColumn.Type,
                        ReportColumn.Name,
                        ReportColumn.Cost,
                        ReportColumn.InvoiceNumber,
                        ReportColumn.AcquisitionDate,
                        ReportColumn.Audience,
                    },
                }
            },
            {
                TypeReport.OnlyPC,
                new ReportRelation()
                {
                    Function = "GetAllPCForReport",
                    TableName = "PC",
                    Columns = new List<ReportColumn>()
                    {
                        ReportColumn.InventoryNumber,
                        ReportColumn.Name,
                        ReportColumn.Cost,
                        ReportColumn.Motherboard,
                        ReportColumn.CPU,
                        ReportColumn.Cores,
                        ReportColumn.ProcessorFrequency,
                        ReportColumn.MaxProcessorFrequency,
                        ReportColumn.RAM,
                        ReportColumn.FrequencyRAM,
                        ReportColumn.VCard,
                        ReportColumn.VideoRAM,
                        ReportColumn.SSD,
                        ReportColumn.HDD,
                        ReportColumn.OS,
                        ReportColumn.VideoConnectors,
                        ReportColumn.InvoiceNumber,
                        ReportColumn.AcquisitionDate,
                        ReportColumn.Audience,
                    },
                }
            },
            {
                TypeReport.OnlyNotebook,
                new ReportRelation()
                {
                    Function = "GetAllNotebookForReport",
                    TableName = "Laptops&Monoblocks",
                    Columns = new List<ReportColumn>()
                    {
                        ReportColumn.InventoryNumber,
                        ReportColumn.Type,
                        ReportColumn.Name,
                        ReportColumn.Cost,
                        ReportColumn.CPU,
                        ReportColumn.Cores,
                        ReportColumn.ProcessorFrequency,
                        ReportColumn.MaxProcessorFrequency,
                        ReportColumn.RAM,
                        ReportColumn.FrequencyRAM,
                        ReportColumn.VCard,
                        ReportColumn.VideoRAM,
                        ReportColumn.SSD,
                        ReportColumn.HDD,
                        ReportColumn.OS,
                        ReportColumn.VideoConnectors,
                        ReportColumn.ScreenDiagonal,
                        ReportColumn.ScreenResolution,
                        ReportColumn.ScreenFrequency,
                        ReportColumn.MatrixTechnology,
                        ReportColumn.InvoiceNumber,
                        ReportColumn.AcquisitionDate,
                        ReportColumn.Audience,
                    },
                }
            },
            {
                TypeReport.OnlyMonitor,
                new ReportRelation()
                {
                    Function = "GetAllMonitorForReport",
                    TableName = "Monitors",
                    Columns = new List<ReportColumn>()
                    {
                        ReportColumn.InventoryNumber,
                        ReportColumn.Name,
                        ReportColumn.Cost,
                        ReportColumn.VideoConnectors,
                        ReportColumn.ScreenDiagonal,
                        ReportColumn.ScreenResolution,
                        ReportColumn.ScreenFrequency,
                        ReportColumn.MatrixTechnology,
                        ReportColumn.InvoiceNumber,
                        ReportColumn.AcquisitionDate,
                        ReportColumn.Audience,
                    },
                }
            },
            {
                TypeReport.OnlyProjector,
                new ReportRelation()
                {
                    Function = "GetAllProjectorForReport",
                    TableName = "Projectors",
                    Columns = new List<ReportColumn>()
                    {
                        ReportColumn.InventoryNumber,
                        ReportColumn.Name,
                        ReportColumn.Cost,
                        ReportColumn.MaxDiagonal,
                        ReportColumn.ProjectorTechnology,
                        ReportColumn.ScreenResolution,
                        ReportColumn.VideoConnectors,
                        ReportColumn.InvoiceNumber,
                        ReportColumn.AcquisitionDate,
                        ReportColumn.Audience,
                    },
                }
            },
            {
                TypeReport.OnlyBoard,
                new ReportRelation()
                {
                    Function = "GetAllBoardForReport",
                    TableName = "Interactive whiteboard",
                    Columns = new List<ReportColumn>()
                    {
                        ReportColumn.InventoryNumber,
                        ReportColumn.Name,
                        ReportColumn.Cost,
                        ReportColumn.Diagonal,
                        ReportColumn.InvoiceNumber,
                        ReportColumn.AcquisitionDate,
                        ReportColumn.Audience,
                    },
                }
            },
            {
                TypeReport.OnlyScreen,
                new ReportRelation()
                {
                    Function = "GetAllScreenForReport",
                    TableName = "Projector screens",
                    Columns = new List<ReportColumn>()
                    {
                        ReportColumn.InventoryNumber,
                        ReportColumn.Name,
                        ReportColumn.Cost,
                        ReportColumn.Diagonal,
                        ReportColumn.IsElectronicDrive,
                        ReportColumn.AspectRatio,
                        ReportColumn.ScreenInstalled,
                        ReportColumn.InvoiceNumber,
                        ReportColumn.AcquisitionDate,
                        ReportColumn.Audience,
                    },
                }
            },
            {
                TypeReport.OnlyPrinterScanner,
                new ReportRelation()
                {
                    Function = "GetAllPrinterScannerForReport",
                    TableName = "Printers&Scanners",
                    Columns = new List<ReportColumn>()
                    {
                        ReportColumn.InventoryNumber,
                        ReportColumn.Type,
                        ReportColumn.Name,
                        ReportColumn.Cost,
                        ReportColumn.PaperSize,
                        ReportColumn.InvoiceNumber,
                        ReportColumn.AcquisitionDate,
                        ReportColumn.Audience,
                    },
                }
            },
            {
                TypeReport.OnlyNetworkSwitch,
                new ReportRelation()
                {
                    Function = "GetAllNetworkSwitchForReport",
                    TableName = "Network equipment",
                    Columns = new List<ReportColumn>()
                    {
                        ReportColumn.InventoryNumber,
                        ReportColumn.Type,
                        ReportColumn.Name,
                        ReportColumn.Cost,
                        ReportColumn.NumberOfPorts,
                        ReportColumn.WiFiFrequency,
                        ReportColumn.InvoiceNumber,
                        ReportColumn.AcquisitionDate,
                        ReportColumn.Audience,
                    },
                }
            },
            {
                TypeReport.OnlyOtherEquipment,
                new ReportRelation()
                {
                    Function = "GetAllOtherEquipmentForReport",
                    TableName = "Other equipment",
                    Columns = new List<ReportColumn>()
                    {
                        ReportColumn.InventoryNumber,
                        ReportColumn.Name,
                        ReportColumn.Cost,
                        ReportColumn.InvoiceNumber,
                        ReportColumn.AcquisitionDate,
                        ReportColumn.Audience,
                    },
                }
            },
            {
                TypeReport.Software,
                new ReportRelation()
                {
                    Function = "GetAllSoftwareForReport",
                    TableName = "Software",
                    Columns = new List<ReportColumn>()
                    {
                        ReportColumn.Name,
                        ReportColumn.Cost,
                        ReportColumn.Count,
                        ReportColumn.Cost,
                        ReportColumn.TotalCost,
                        ReportColumn.TypeLicense,
                        ReportColumn.InvoiceNumber,
                        ReportColumn.AcquisitionDate,
                    },
                }
            },
            {
                TypeReport.OS,
                new ReportRelation()
                {
                    Function = "GetAllOSForReport",
                    TableName = "OS",
                    Columns = new List<ReportColumn>()
                    {
                        ReportColumn.Name,
                        ReportColumn.Cost,
                        ReportColumn.Count,
                        ReportColumn.Cost,
                        ReportColumn.TotalCost,
                        ReportColumn.InvoiceNumber,
                        ReportColumn.AcquisitionDate,
                    },
                }
            },
            {
                TypeReport.SoftAndOS,
                new ReportRelation()
                {
                    Function = "GetAllSoftwareAndOSForReport",
                    TableName = "OS&Software",
                    Columns = new List<ReportColumn>()
                    {
                        ReportColumn.Type,
                        ReportColumn.Name,
                        ReportColumn.Cost,
                        ReportColumn.Count,
                        ReportColumn.Cost,
                        ReportColumn.TotalCost,
                        ReportColumn.TypeLicense,
                        ReportColumn.InvoiceNumber,
                        ReportColumn.AcquisitionDate,
                    },
                }
            },
        };

        public static Dictionary<TypeReport, ReportRelation> Collection => collection;
    }
}
