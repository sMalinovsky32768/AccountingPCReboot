using GemBox.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace AccountingPC.AccountingReport
{
    public class ReportName
    {
        public TypeReport Type { get; set; }
        public string Name { get; set; }
    }

    internal class Report
    {
        public static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public delegate void ColumnUpdate();

        public event ColumnUpdate UsedColumnUpdateEvent;
        public event ColumnUpdate UnusedColumnUpdateEvent;

        private ObservableCollection<ColumnRelation> usedReportColumns;
        private ObservableCollection<ColumnRelation> unusedReportColumns;

        public ReportOptions Options { get; private set; } = new ReportOptions();

        public ObservableCollection<ColumnRelation> UsedReportColumns
        {
            get => usedReportColumns;
            set
            {
                usedReportColumns = value;
                UsedColumnUpdateEvent?.Invoke();
            }
        }
        public ObservableCollection<ColumnRelation> UnusedReportColumns 
        {
            get => unusedReportColumns;
            set
            {
                unusedReportColumns = value;
                UnusedColumnUpdateEvent?.Invoke();
            }
        }

        public Report() { Options.TypeReportChangedEvent += TypeChangedEventHandler; }

        private void InitializeColumn()
        {
            UsedReportColumns = new ObservableCollection<ColumnRelation>();
            UnusedReportColumns = new ObservableCollection<ColumnRelation>();
            if (Options.TypeReport != TypeReport.Full)
            {
                foreach (ColumnRelation relation in ReportColumnRelation.ColumnRelationships)
                {
                    if (ReportRelationCollection.Collection[Options.TypeReport].Columns.Contains(relation.Column))
                    {
                        UnusedReportColumns.Add(relation);
                    }
                }
            }
        }

        private string CommandTextBuilder(bool isFull = false)
        {
            List<string> vs = new List<string>();

            ReportRelation relation = ReportRelationCollection.Collection[Options.TypeReport];

            string commandText = "SELECT ";

            if (isFull)
            {
                foreach (ReportColumn column in relation.Columns)
                {
                    vs.Add(ReportColumnRelation.GetColumnName(column).Name);
                }
            }
            else
            {
                foreach (ColumnRelation columnRelation in UsedReportColumns)
                {
                    vs.Add(ReportColumnRelation.GetColumnName(columnRelation.Column).Name);
                }
            }

            int i = 0;
            foreach (string str in vs)
            {
                commandText += $"[{str}]";
                i++;
                if (i < vs.Count)
                    commandText += ", ";
            }

            commandText += $" FROM dbo.{relation.Function}()";
            commandText += Options.GetSortingString(isFull);

            return commandText;
        }

        private DataSet GetDataSet()
        {
            DataSet set = new DataSet();

            if (Options.TypeReport == TypeReport.Full)
            {
                foreach (TypeReport type in ReportRelationCollection.Collection.Keys)
                {
                    Options.TypeReport = type;
                    FillDataSet(set, true);
                }
                Options.TypeReport = TypeReport.Full;
            }
            else
            {
                FillDataSet(set);
            }

            return set;
        }

        private void FillDataSet(DataSet set, bool isFull = false)
        {
            string tableName = ReportRelationCollection.Collection[Options.TypeReport].TableName;

            set.Tables.Add(tableName);
            new SqlDataAdapter(CommandTextBuilder(isFull), ConnectionString).Fill(set, tableName);

            if (set.Tables[tableName].Columns.Contains("Видеоразъемы"))
            {
                set.Tables[tableName].Columns["Видеоразъемы"].ColumnName = "VideoConnectors";
                set.Tables[tableName].Columns.Add("Видеоразъемы");

                foreach (DataRow row in set.Tables[tableName].Rows)
                {
                    row["Видеоразъемы"] = AccountingPCWindow.GetVideoConnectors(Convert.ToInt32(row["VideoConnectors"]));
                }
            }

            int i = 0;
            foreach (ColumnRelation column in UsedReportColumns)
            {
                if (set.Tables[tableName].Columns.Contains(column.Name))
                {
                    set.Tables[tableName].Columns[column.Name].SetOrdinal(i);
                    i++;
                }
            }
        }

        public ExcelFile CreateReport()
        {
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

        private void TypeChangedEventHandler()
        {
            InitializeColumn();
        }
    }
}
