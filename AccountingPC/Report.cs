using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using GemBox.Spreadsheet;

namespace AccountingPC
{
    internal enum TypeReport
    {
        Simple,
        Full,
        OnlyPC,
        OnlyNotebook,
        OnlyMonitor,
        OnlyProjector,
        OnlyBoard,
        OnlyScreen,
        OnlyPrinterScanner,
        OnlyNetworkSwitch,
        OnlyOtherEquipment,
    }

    internal static class Report
    {
        public static String ConnectionString { get; private set; } = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        private static DataSet GetDataSet(TypeReport typeReport, ReportOptions options)
        {
            DataSet set;
            switch (typeReport)
            {
                case TypeReport.Simple:
                    set = new DataSet();
                    set.Tables.Add("All Devices");
                    new SqlDataAdapter("SELECT * FROM dbo.GetAllDevices()", ConnectionString).Fill(set, "All Devices");
                    break;
                case TypeReport.OnlyPC:
                    set = new DataSet();
                    set.Tables.Add("PC");
                    new SqlDataAdapter("SELECT * FROM dbo.GetAllPC()", ConnectionString).Fill(set, "PC");
                    break;
                case TypeReport.OnlyNotebook:
                    set = new DataSet();
                    set.Tables.Add("Laptops&Monoblocks");
                    new SqlDataAdapter("SELECT * FROM dbo.GetAllNotebook()", ConnectionString).Fill(set, "Laptops&Monoblocks");
                    break;
                case TypeReport.OnlyMonitor:
                    set = new DataSet();
                    set.Tables.Add("Monitors");
                    new SqlDataAdapter("SELECT * FROM dbo.GetAllMonitor()", ConnectionString).Fill(set, "Monitors");
                    break;
                case TypeReport.OnlyProjector:
                    set = new DataSet();
                    set.Tables.Add("Projectors");
                    new SqlDataAdapter("SELECT * FROM dbo.GetAllProjector()", ConnectionString).Fill(set, "Projectors");
                    break;
                case TypeReport.OnlyBoard:
                    set = new DataSet();
                    set.Tables.Add("Interactive whiteboard");
                    new SqlDataAdapter("SELECT * FROM dbo.GetAllBoard()", ConnectionString).Fill(set, "Interactive whiteboard");
                    break;
                case TypeReport.OnlyScreen:
                    set = new DataSet();
                    set.Tables.Add("Projector screens");
                    new SqlDataAdapter("SELECT * FROM dbo.GetAllScreen()", ConnectionString).Fill(set, "Projector screens");
                    break;
                case TypeReport.OnlyPrinterScanner:
                    set = new DataSet();
                    set.Tables.Add("Printers&Scanners");
                    new SqlDataAdapter("SELECT * FROM dbo.GetAllPrinterScanner()", ConnectionString).Fill(set, "Printers&Scanners");
                    break;
                case TypeReport.OnlyNetworkSwitch:
                    set = new DataSet();
                    set.Tables.Add("Network equipment");
                    new SqlDataAdapter("SELECT * FROM dbo.GetAllNetworkSwitch()", ConnectionString).Fill(set, "Network equipment");
                    break;
                case TypeReport.OnlyOtherEquipment:
                    set = new DataSet();
                    set.Tables.Add("Other equipment");
                    new SqlDataAdapter("SELECT * FROM dbo.GetAllOtherEquipment()", ConnectionString).Fill(set, "Other equipment");
                    break;
                case TypeReport.Full:
                default:
                    set = new DataSet();
                    set.Tables.Add("All Devices");
                    set.Tables.Add("PC");
                    set.Tables.Add("Laptops&Monoblocks");
                    set.Tables.Add("Monitors");
                    set.Tables.Add("Projectors");
                    set.Tables.Add("Interactive whiteboard");
                    set.Tables.Add("Projector screens");
                    set.Tables.Add("Printers&Scanners");
                    set.Tables.Add("Network equipment");
                    set.Tables.Add("Other equipment");
                    new SqlDataAdapter("SELECT * FROM dbo.GetAllDevices()", ConnectionString).Fill(set, "All Devices");
                    new SqlDataAdapter("SELECT * FROM dbo.GetAllPC()", ConnectionString).Fill(set, "PC");
                    new SqlDataAdapter("SELECT * FROM dbo.GetAllNotebook()", ConnectionString).Fill(set, "Laptops&Monoblocks");
                    new SqlDataAdapter("SELECT * FROM dbo.GetAllMonitor()", ConnectionString).Fill(set, "Monitors");
                    new SqlDataAdapter("SELECT * FROM dbo.GetAllProjector()", ConnectionString).Fill(set, "Projectors");
                    new SqlDataAdapter("SELECT * FROM dbo.GetAllBoard()", ConnectionString).Fill(set, "Interactive whiteboard");
                    new SqlDataAdapter("SELECT * FROM dbo.GetAllScreen()", ConnectionString).Fill(set, "Projector screens");
                    new SqlDataAdapter("SELECT * FROM dbo.GetAllPrinterScanner()", ConnectionString).Fill(set, "Printers&Scanners");
                    new SqlDataAdapter("SELECT * FROM dbo.GetAllNetworkSwitch()", ConnectionString).Fill(set, "Network equipment");
                    new SqlDataAdapter("SELECT * FROM dbo.GetAllOtherEquipment()", ConnectionString).Fill(set, "Other equipment");
                    break;
            }
            return set;
        }

        public static ExcelFile CreateReport(TypeReport typeReport = TypeReport.Simple, ReportOptions options = null)
        {
            if (options == null)
            {
                options = new ReportOptions
                {
                    IsInvN = true,
                    IsType = true,
                    IsName = true,
                    IsCost = true,
                    IsAcquisitionDate = true,
                    IsLocation = true,
                    SortingsArray = new Sorting[] { Sorting.InvN }
                };
            }

            DataSet dataSet = GetDataSet(typeReport, options);

            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");

            var book = new ExcelFile();

            foreach (DataTable dataTable in dataSet.Tables)
            {
                ExcelWorksheet worksheet = book.Worksheets.Add(dataTable.TableName);

                worksheet.InsertDataTable(dataTable, new InsertDataTableOptions()
                {
                    ColumnHeaders = true,
                });
            }

            return book;
        }
    }
}
