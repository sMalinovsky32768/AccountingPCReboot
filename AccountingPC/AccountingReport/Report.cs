using GemBox.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;

namespace AccountingPC.AccountingReport
{
    internal class Report : INotifyPropertyChanged
    {
        public static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public delegate void ColumnUpdate();

        public event ColumnUpdate UsedColumnUpdateEvent;
        public event ColumnUpdate UnusedColumnUpdateEvent;

        private ObservableCollection<ReportColumnName> usedReportColumns;
        private ObservableCollection<ReportColumnName> unusedReportColumns;

        private ReportColumnName selectedUnusedColumn;
        private ReportColumnName selectedUsedColumn;

        public ObservableCollection<ReportColumnName> UsedReportColumns
        {
            get => usedReportColumns;
            set
            {
                usedReportColumns = value;
                UsedColumnUpdateEvent?.Invoke();
                OnPropertyChanged("UsedReportColumns");
            }
        }
        public ObservableCollection<ReportColumnName> UnusedReportColumns
        {
            get => unusedReportColumns;
            set
            {
                unusedReportColumns = value;
                UnusedColumnUpdateEvent?.Invoke();
                OnPropertyChanged("UnusedReportColumns");
            }
        }

        public ReportColumnName SelectedUsedColumn
        {
            get => selectedUsedColumn;
            set
            {
                selectedUsedColumn = value;
                OnPropertyChanged("SelectedUsedColumn");
            }
        }
        public ReportColumnName SelectedUnusedColumn
        {
            get => selectedUnusedColumn;
            set
            {
                selectedUnusedColumn = value;
                OnPropertyChanged("SelectedUnusedColumn");
            }
        }

        private ReportCommand addUsedReportColumns;
        public ReportCommand AddUsedReportColumns
        {
            get
            {
                return addUsedReportColumns ??
                    (addUsedReportColumns = new ReportCommand(obj =>
                    {
                        if (obj != null)
                        {
                            ReportColumnName columnName = obj as ReportColumnName;
                            foreach (ReportColumnName reportColumnName in UnusedReportColumns)
                            {
                                if (reportColumnName == columnName) SelectedUnusedColumn =
                                    UnusedReportColumns[UnusedReportColumns.IndexOf(columnName) < UnusedReportColumns.Count - 1 ?
                                    UnusedReportColumns.IndexOf(columnName) + 1 : 0];
                            }
                            UsedReportColumns.Add(columnName);
                            UnusedReportColumns.Remove(columnName);
                        }
                    },
                    (obj) =>
                    {
                        if (UnusedReportColumns.Count > 0)
                            if (SelectedUnusedColumn != null)
                                return true;
                            else
                                return false;
                        else
                            return false;
                    }));
            }
        }

        private ReportCommand delUsedReportColumns;
        public ReportCommand DelUsedReportColumns
        {
            get
            {
                return delUsedReportColumns ??
                    (delUsedReportColumns = new ReportCommand(obj =>
                    {
                        if (obj != null)
                        {
                            ReportColumnName columnName = obj as ReportColumnName;
                            foreach (ReportColumnName reportColumnName in UsedReportColumns)
                            {
                                if (reportColumnName == columnName) SelectedUsedColumn =
                                    UsedReportColumns[UsedReportColumns.IndexOf(columnName) < UsedReportColumns.Count - 1 ?
                                    UsedReportColumns.IndexOf(columnName) + 1 : 0];
                            }
                            UnusedReportColumns.Add(columnName);
                            UsedReportColumns.Remove(columnName);
                        }
                    },
                    (obj) =>
                    {
                        if (UsedReportColumns.Count > 0)
                            if (SelectedUsedColumn != null)
                                return true;
                            else
                                return false;
                        else
                            return false;
                    }));
            }
        }

        public ReportOptions Options { get; private set; }

        public Report()
        {
            Options = new ReportOptions(this);
            Options.TypeReportChangedEvent += TypeChangedEventHandler;
        }

        private void InitializeColumn()
        {
            UsedReportColumns = new ObservableCollection<ReportColumnName>();
            UnusedReportColumns = new ObservableCollection<ReportColumnName>();
            if (Options.TypeReport != TypeReport.Full)
            {
                foreach (ReportColumnName relation in ReportColumnNameCollection.Collection)
                {
                    if (ReportRelationCollection.Collection[Options.TypeReport].Columns.Contains(relation.Column))
                    {
                        UnusedReportColumns.Add(relation);
                    }
                }
            }
            if (UsedReportColumns.Count > 0)
                SelectedUsedColumn = UsedReportColumns?[0];
            if (UnusedReportColumns.Count > 0)
                SelectedUnusedColumn = UnusedReportColumns?[0];
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
                    vs.Add(ReportColumnNameCollection.GetColumnName(column).Name);
                }
            }
            else
            {
                foreach (ReportColumnName columnRelation in UsedReportColumns)
                {
                    vs.Add(ReportColumnNameCollection.GetColumnName(columnRelation.Column).Name);
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
            foreach (ReportColumnName column in UsedReportColumns)
            {
                if (set.Tables[tableName].Columns.Contains(column.Name))
                {
                    set.Tables[tableName].Columns[column.Name].SetOrdinal(i);
                    i++;
                }
            }

            if (set.Tables[tableName].Columns.Contains("VideoConnectors"))
            {
                set.Tables[tableName].Columns.Remove("VideoConnectors");
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

                int col;
                // Установка денежного типа для Цены
                if (worksheet.Cells.FindText("Цена", false, out _, out col))
                {
                    worksheet.Columns[col].Cells.Style.NumberFormat = NumberFormatBuilder.Currency("\u20bd", 2, true, false, true);
                    if (worksheet.Cells.FindText("Общая стоимость", false, out _, out int col1))
                    {
                        worksheet.Columns[col].Cells.Style.NumberFormat = NumberFormatBuilder.Currency("\u20bd", 2, true, false, true);
                        if (worksheet.Cells.FindText("Количество", false, out _, out int col2))
                        {
                            foreach (ExcelRow row in worksheet.Rows)
                            {
                                int rowIndex = row.Index;
                                if (rowIndex == 0) continue;
                                string cell = worksheet.Cells[rowIndex, col].Name;
                                string cell2 = worksheet.Cells[rowIndex, col2].Name;
                                string formula = $"={cell}*{cell2}";
                                worksheet.Cells[rowIndex, col1].Formula = formula;
                                //worksheet.Columns[col1].Cells[row.Index, col1].Formula = $"{worksheet.Cells[row.Index, col]}*{worksheet.Cells[row.Index, col2]}";
                            }
                        }
                    }
                }
                if (worksheet.Cells.FindText("Инвентарный номер", false, out _, out col))
                {
                    worksheet.Columns[col].Cells.Style.NumberFormat = "000000000000000";
                }
                if (worksheet.Cells.FindText("Дата приобретения", false, out _, out col))
                {
                    worksheet.Columns[col].Cells.Style.NumberFormat = "dd-MM-yyyy";
                }
                if (worksheet.Cells.FindText("Диагональ", false, out _, out col) ||
                    worksheet.Cells.FindText("Диагональ экрана", false, out _, out col) ||
                    worksheet.Cells.FindText("Максимальная диагональ", false, out _, out col))
                {
                    worksheet.Columns[col].Cells.Style.NumberFormat =
                        NumberFormatBuilder.Accounting(2, true, false, "\u2033", false);
                }
                if (worksheet.Cells.FindText("Базовая частота", false, out _, out col))
                {
                    worksheet.Columns[col].Cells.Style.NumberFormat = "###__ГГц";
                }
                if (worksheet.Cells.FindText("Максимальная частота", false, out _, out col))
                {
                    worksheet.Columns[col].Cells.Style.NumberFormat = "###__ГГц";
                }
                if (worksheet.Cells.FindText("Частота обновления", false, out _, out col))
                {
                    worksheet.Columns[col].Cells.Style.NumberFormat = "###__ГГц";
                }
                if (worksheet.Cells.FindText("ОЗУ", false, out _, out col))
                {
                    worksheet.Columns[col].Cells.Style.NumberFormat = "###__ГБ";
                }
                if (worksheet.Cells.FindText("Видеопамять", false, out _, out col))
                {
                    worksheet.Columns[col].Cells.Style.NumberFormat = "###__ГБ";
                }
                if (worksheet.Cells.FindText("Объем SSD", false, out _, out col))
                {
                    worksheet.Columns[col].Cells.Style.NumberFormat = "###__ГБ";
                }
                if (worksheet.Cells.FindText("Объем HDD", false, out _, out col))
                {
                    worksheet.Columns[col].Cells.Style.NumberFormat = "###__ГБ";
                }

                //foreach (ExcelColumn column in worksheet.Columns)
                //{
                //    column.AutoFit();
                //}

                int columnCount = worksheet.CalculateMaxUsedColumns();
                for (int i = 0; i < columnCount; i++)
                    worksheet.Columns[i].AutoFit(1, worksheet.Rows[0], worksheet.Rows[worksheet.Rows.Count - 1]);
            }

            return book;
        }

        private void TypeChangedEventHandler()
        {
            InitializeColumn();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
