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
                    SP = "sp_GetAllDevices",
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
                    SP = "sp_GetAllPCForReport",
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
                    SP = "sp_GetAllNotebookForReport",
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
                        ReportColumn.IsWorkingCondition,
                    },
                }
            },
            {
                TypeReport.OnlyMonitor,
                new ReportRelation()
                {
                    Function = "GetAllMonitorForReport",
                    SP = "sp_GetAllMonitorForReport",
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
                        ReportColumn.IsWorkingCondition,
                    },
                }
            },
            {
                TypeReport.OnlyProjector,
                new ReportRelation()
                {
                    Function = "GetAllProjectorForReport",
                    SP = "sp_GetAllProjectorForReport",
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
                        ReportColumn.IsWorkingCondition,
                    },
                }
            },
            {
                TypeReport.OnlyBoard,
                new ReportRelation()
                {
                    Function = "GetAllBoardForReport",
                    SP = "sp_GetAllBoardForReport",
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
                        ReportColumn.IsWorkingCondition,
                    },
                }
            },
            {
                TypeReport.OnlyScreen,
                new ReportRelation()
                {
                    Function = "GetAllScreenForReport",
                    SP = "sp_GetAllScreenForReport",
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
                        ReportColumn.IsWorkingCondition,
                    },
                }
            },
            {
                TypeReport.OnlyPrinterScanner,
                new ReportRelation()
                {
                    Function = "GetAllPrinterScannerForReport",
                    SP = "sp_GetAllPrinterScannerForReport",
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
                        ReportColumn.IsWorkingCondition,
                    },
                }
            },
            {
                TypeReport.OnlyNetworkSwitch,
                new ReportRelation()
                {
                    Function = "GetAllNetworkSwitchForReport",
                    SP = "sp_GetAllNetworkSwitchForReport",
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
                        ReportColumn.IsWorkingCondition,
                    },
                }
            },
            {
                TypeReport.OnlyOtherEquipment,
                new ReportRelation()
                {
                    Function = "GetAllOtherEquipmentForReport",
                    SP = "sp_GetAllOtherEquipmentForReport",
                    TableName = "Other equipment",
                    Columns = new List<ReportColumn>()
                    {
                        ReportColumn.InventoryNumber,
                        ReportColumn.Name,
                        ReportColumn.Cost,
                        ReportColumn.InvoiceNumber,
                        ReportColumn.AcquisitionDate,
                        ReportColumn.Audience,
                        ReportColumn.IsWorkingCondition,
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
            {
                TypeReport.UseSoft,
                new ReportRelation()
                {
                    Function = "GetSoftAndOSInstaledList",
                    TableName = "Software&OS",
                    Columns = new List<ReportColumn>()
                    {
                        ReportColumn.Type,
                        ReportColumn.Name,
                        ReportColumn.Cost,
                        ReportColumn.Count,
                        ReportColumn.TotalCount,
                        ReportColumn.IsAvailable,
                    },
                }
            },
        };

        public static Dictionary<TypeReport, ReportRelation> Collection => collection;
    }
}
