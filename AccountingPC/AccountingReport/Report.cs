using GemBox.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Security.RightsManagement;

namespace AccountingPC.AccountingReport
{
    public class ReportName
    {
        public TypeReport Type { get; set; }
        public string Name { get; set; }
    }

    internal class Report
    {
        public readonly static String ConnectionString  = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        //public static Dictionary<TypeReport, string> ReportNames { get; set; } = new Dictionary<TypeReport, string>()
        //{
        //    {TypeReport.Simple,             "Общий (оборудование)" },
        //    {TypeReport.Full,               "Полный" },
        //    {TypeReport.OnlyPC,             "Компьютеры" },
        //    {TypeReport.OnlyNotebook,       "Ноутбуки и Моноблоки" },
        //    {TypeReport.OnlyMonitor,        "Мониторы" },
        //    {TypeReport.OnlyProjector,      "Проекторы" },
        //    {TypeReport.OnlyBoard,          "Интерактивные доски" },
        //    {TypeReport.OnlyScreen,         "Экраны для проекторов" },
        //    {TypeReport.OnlyPrinterScanner, "Принтеры и сканеры" },
        //    {TypeReport.OnlyNetworkSwitch,  "Сетевое оборудование" },
        //    {TypeReport.OnlyOtherEquipment, "Прочее оборудование" },
        //    {TypeReport.Software,           "Программное обеспечение" },
        //    {TypeReport.OS,                 "Операционные системы" },
        //    {TypeReport.SoftAndOS,          "Общий (ПО&ОС)" },
        //};

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

        public ReportOptions Options { get; private set; } = new ReportOptions();

        public ObservableCollection<ColumnRelation> UsedReportColumns { get; set; }
        public ObservableCollection<ColumnRelation> UnusedReportColumns { get; set; }

        public static ObservableCollection<ReportName> ReportNamesCollection => reportNamesCollection;

        public Report() { Options.TypeReportChangedEvent += TypeChangedEventHandler; }

        //public Report(TypeReport typeReport)
        //{
        //    InitializeReport(new ReportOptions() { TypeReport = typeReport, });
        //}

        //public Report(ReportOptions options) 
        //{
        //    InitializeReport(options);
        //}

        private void InitializeReport(ReportOptions options)
        {
            Options = options;
            Options.TypeReportChangedEvent += TypeChangedEventHandler;
        }

        private void InitializeColumn()
        {
            UsedReportColumns = new ObservableCollection<ColumnRelation>();
            UnusedReportColumns = new ObservableCollection<ColumnRelation>();
            if (Options.TypeReport != TypeReport.Full)
            {
                foreach (ColumnRelation relation in ReportColumnRelation.ColumnRelationships)
                {
                    if (Relation[Options.TypeReport].Columns.Contains(relation.Column))
                    {
                        UnusedReportColumns.Add(relation);
                    }
                }
            }
        }
        public static ReportName GetReportName(TypeReport typeReport)
        {
            foreach (ReportName reportName in ReportNamesCollection)
            {
                if (reportName.Type == typeReport)
                {
                    return reportName;
                }
            }
            return null;
        }

        public static string CommandTextBuilder(ReportOptions options)
        {
            List<string> vs = new List<string>();

            string commandText = "SELECT ";

            ReportRelation relation = Relation[options.TypeReport];

            foreach (ReportColumn column in relation.Columns)
            {
                FieldInfo field = typeof(ReportOptions).GetField($"Is{column.ToString()}");

                if (Convert.ToBoolean(field?.GetValue(options)))
                {
                    vs.Add(ReportColumnRelation.GetColumnName(column));
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
            commandText += options.GetSortingString();

            return commandText;
        }

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

        private string CommandTextBuilder(bool isFull = false)
        {
            List<string> vs = new List<string>();

            ReportRelation relation = Relation[Options.TypeReport];

            string commandText = "SELECT ";

            if (isFull)
            {
                //foreach (ReportColumn column in relation.Columns)
                //{
                //    FieldInfo field = typeof(ReportOptions).GetField($"Is{column.ToString()}");

                //    if (Convert.ToBoolean(field?.GetValue(Options)))
                //    {
                //        vs.Add(ReportColumnRelation.GetColumnName(column));
                //    }
                //}
                foreach (ReportColumn column in relation.Columns)
                {
                    vs.Add(ReportColumnRelation.GetColumnName(column));
                }
            }
            else
            {
                foreach (ColumnRelation columnRelation in UsedReportColumns)
                {
                    vs.Add(ReportColumnRelation.GetColumnName(columnRelation.Column));
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
            commandText += Options.GetSortingString();

            return commandText;
        }

        private DataSet GetDataSet()
        {
            DataSet set = null;

            if (Options.TypeReport == TypeReport.Full)
            {
                foreach (TypeReport type in Relation.Keys)
                {
                    set.Tables.Add(Relation[type].TableName);
                    Options.TypeReport = type;
                    //new SqlDataAdapter($"Select * From dbo.{Relation[type].Function}(){options.SortingString}", ConnectionString).Fill(set, Relation[type].TableName);
                    new SqlDataAdapter(CommandTextBuilder(true), ConnectionString).Fill(set, Relation[type].TableName);
                }
            }
            else
            {
                set.Tables.Add(Relation[Options.TypeReport].TableName);
                //new SqlDataAdapter($"Select * From dbo.{Relation[options.TypeReport].Function}(){options.SortingString}", ConnectionString).Fill(set, Relation[options.TypeReport].TableName);
                new SqlDataAdapter(CommandTextBuilder(), ConnectionString).Fill(set, Relation[Options.TypeReport].TableName);
            }

            return set;
        }

        public ExcelFile CreateReport()
        {
            if (Options == null)
            {
                Options = new ReportOptions();
            }

            DataSet dataSet = GetDataSet();

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

        //public static void SaveReport(string path, ReportOptions options = null)
        //{
        //    CreateReport(options).Save(path);
        //}

        private void TypeChangedEventHandler()
        {
            InitializeColumn();
        }
    }
}
