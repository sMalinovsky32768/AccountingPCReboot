using GemBox.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace AccountingPC.AccountingReport
{
    internal static class Report
    {

        public static Dictionary<TypeReport, string> ReportNames { get; set; } = new Dictionary<TypeReport, string>()
        {
            {TypeReport.Simple,             "Общий (оборудование)" },
            {TypeReport.Full,               "Полный" },
            {TypeReport.OnlyPC,             "Компьютеры" },
            {TypeReport.OnlyNotebook,       "Ноутбуки и Моноблоки" },
            {TypeReport.OnlyMonitor,        "Мониторы" },
            {TypeReport.OnlyProjector,      "Проекторы" },
            {TypeReport.OnlyBoard,          "Интерактивные доски" },
            {TypeReport.OnlyScreen,         "Экраны для проекторов" },
            {TypeReport.OnlyPrinterScanner, "Принтеры и сканеры" },
            {TypeReport.OnlyNetworkSwitch,  "Сетевое оборудование" },
            {TypeReport.OnlyOtherEquipment, "Прочее оборудование" },
            {TypeReport.Software,           "Программное обеспечение" },
            {TypeReport.OS,                 "Операционные системы" },
            {TypeReport.SoftAndOS,          "Общий (ПО&ОС)" },
        };
        public static Dictionary<TypeReport, ReportRelation> Relation { get; set; } = new Dictionary<TypeReport, ReportRelation>()
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
                        ReportColumn.InvoiceNumber,
                        ReportColumn.AcquisitionDate,
                        ReportColumn.Audience,
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
                        ReportColumn.InvoiceNumber,
                        ReportColumn.AcquisitionDate,
                        ReportColumn.Audience,
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
                        ReportColumn.InvoiceNumber,
                        ReportColumn.AcquisitionDate,
                        ReportColumn.Audience,
                        ReportColumn.VideoConnectors,
                        ReportColumn.ScreenDiagonal,
                        ReportColumn.ScreenResolution,
                        ReportColumn.ScreenFrequency,
                        ReportColumn.MatrixTechnology,
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
                        ReportColumn.InvoiceNumber,
                        ReportColumn.AcquisitionDate,
                        ReportColumn.Audience,
                        ReportColumn.MaxDiagonal,
                        ReportColumn.ProjectorTechnology,
                        ReportColumn.ScreenResolution,
                        ReportColumn.VideoConnectors,
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
                        ReportColumn.InvoiceNumber,
                        ReportColumn.AcquisitionDate,
                        ReportColumn.Audience,
                        ReportColumn.Diagonal,
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
                        ReportColumn.InvoiceNumber,
                        ReportColumn.AcquisitionDate,
                        ReportColumn.Audience,
                        ReportColumn.Diagonal,
                        ReportColumn.IsElectronicDrive,
                        ReportColumn.AspectRatio,
                        ReportColumn.ScreenInstalled,
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
                        ReportColumn.InvoiceNumber,
                        ReportColumn.AcquisitionDate,
                        ReportColumn.Audience,
                        ReportColumn.PaperSize,
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
                        ReportColumn.InvoiceNumber,
                        ReportColumn.AcquisitionDate,
                        ReportColumn.Audience,
                        ReportColumn.NumberOfPorts,
                        ReportColumn.WiFiFrequency,
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
                        ReportColumn.InvoiceNumber,
                        ReportColumn.AcquisitionDate,
                        ReportColumn.TypeLicense,
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
                        ReportColumn.InvoiceNumber,
                        ReportColumn.AcquisitionDate,
                        ReportColumn.TypeLicense,
                    },
                }
            },
        };

        public static String ConnectionString { get; private set; } = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        private static DataSet GetDataSet(ReportOptions options)
        {
            DataSet set = null;

            if (options.TypeReport == TypeReport.Full)
            {
                foreach (TypeReport type in Relation.Keys)
                {
                    set.Tables.Add(Relation[type].TableName);
                    options.TypeReport = type;
                    //new SqlDataAdapter($"Select * From dbo.{Relation[type].Function}(){options.SortingString}", ConnectionString).Fill(set, Relation[type].TableName);
                    new SqlDataAdapter(CommandTextBuilder(options), ConnectionString).Fill(set, Relation[type].TableName);
                }
            }
            else
            {
                set.Tables.Add(Relation[options.TypeReport].TableName);
                //new SqlDataAdapter($"Select * From dbo.{Relation[options.TypeReport].Function}(){options.SortingString}", ConnectionString).Fill(set, Relation[options.TypeReport].TableName);
                new SqlDataAdapter(CommandTextBuilder(options), ConnectionString).Fill(set, Relation[options.TypeReport].TableName);
            }

            return set;
        }

        private static string CommandTextBuilder(ReportOptions options)
        {
            List<string> vs = new List<string>();

            string commandText = "SELECT ";

            ReportRelation relation = Relation[options.TypeReport];

            foreach (ReportColumn column in relation.Columns)
            {
                FieldInfo field = typeof(ReportOptions).GetField($"Is{column.ToString()}");

                if (Convert.ToBoolean(field?.GetValue(options)))
                {
                    vs.Add(ReportColumnRelation.ColumnRelationships[column]);
                }
            }

            int i = 0;
            foreach (string str in vs)
            {
                commandText += $"[{str}]";
                if (i < vs.Count)
                    commandText += ", ";
                i++;
            }

            commandText += $" FROM dbo.{relation.TableName}";
            commandText += options.SortingString;

            return commandText;
        }

        public static ExcelFile CreateReport(ReportOptions options)
        {
            if (options == null)
            {
                options = new ReportOptions();
            }

            DataSet dataSet = GetDataSet(options);

            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
            SpreadsheetInfo.FreeLimitReached += (sender, e) => e.FreeLimitReachedAction = FreeLimitReachedAction.ContinueAsTrial;
            var book = new ExcelFile();

            foreach (DataTable dataTable in dataSet.Tables)
            {
                ExcelWorksheet worksheet = book.Worksheets.Add(dataTable.TableName);

                worksheet.InsertDataTable(dataTable, new InsertDataTableOptions()
                {
                    ColumnHeaders = true,
                });

                // Установка денежного типа для Цены
                if (worksheet.Cells.FindText("Цена", false, out _, out int col))
                {
                    worksheet.Columns[col].Cells.Style.NumberFormat = NumberFormatBuilder.Currency("\u20bd", 2, true, false, true);
                }
                foreach (ExcelColumn column in worksheet.Columns)
                {
                    column.AutoFit();
                }
            }

            return book;
        }

        public static void SaveReport(string path, ReportOptions options = null)
        {
            CreateReport(options).Save(path);
        }
    }
}
