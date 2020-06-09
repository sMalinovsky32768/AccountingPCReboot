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
        public ReportCommand AddUsedReportColumns => addUsedReportColumns ??
                    (addUsedReportColumns = new ReportCommand(obj =>
                    {
                        if (obj != null)
                        {
                            ReportColumnName columnName = obj as ReportColumnName;
                            //foreach (ReportColumnName reportColumnName in UnusedReportColumns)
                            //{
                            //    if (reportColumnName == columnName)
                            //    {
                            //        SelectedUnusedColumn =
                            //        UnusedReportColumns[UnusedReportColumns.IndexOf(columnName) < UnusedReportColumns.Count - 1 ?
                            //        UnusedReportColumns.IndexOf(columnName) + 1 : 0];
                            //    }
                            //}
                            for (int i = 0; i < UnusedReportColumns.Count; i++)
                            {
                                ReportColumnName reportColumnName = UnusedReportColumns[i];
                                if (reportColumnName == columnName)
                                {
                                    SelectedUnusedColumn =
                                    UnusedReportColumns[UnusedReportColumns.IndexOf(columnName) < UnusedReportColumns.Count - 1 ?
                                    UnusedReportColumns.IndexOf(columnName) + 1 : 0];
                                }
                            }
                            UsedReportColumns.Add(columnName);
                            UnusedReportColumns.Remove(columnName);
                        }
                    },
                    (obj) =>
                    {
                        if (UnusedReportColumns.Count > 0)
                        {
                            if (SelectedUnusedColumn != null)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }));

        private ReportCommand delUsedReportColumns;
        public ReportCommand DelUsedReportColumns => delUsedReportColumns ??
                    (delUsedReportColumns = new ReportCommand(obj =>
                    {
                        if (obj != null)
                        {
                            ReportColumnName columnName = obj as ReportColumnName;
                            //foreach (ReportColumnName reportColumnName in UsedReportColumns)
                            //{
                            //    if (reportColumnName == columnName)
                            //    {
                            //        SelectedUsedColumn =
                            //        UsedReportColumns[UsedReportColumns.IndexOf(columnName) < UsedReportColumns.Count - 1 ?
                            //        UsedReportColumns.IndexOf(columnName) + 1 : 0];
                            //    }
                            //}
                            for (int i = 0; i < UsedReportColumns.Count; i++)
                            {
                                ReportColumnName reportColumnName = UsedReportColumns[i];
                                if (reportColumnName == columnName)
                                {
                                    SelectedUsedColumn =
                                    UsedReportColumns[UsedReportColumns.IndexOf(columnName) < UsedReportColumns.Count - 1 ?
                                    UsedReportColumns.IndexOf(columnName) + 1 : 0];
                                }
                            }
                            SortingParam sortingParam = null;
                            //foreach (SortingParam param in Options.SortingParamList)
                            //{
                            //    if (param.ColumnName.Column == columnName.Column)
                            //    {
                            //        sortingParam = param;
                            //        break;
                            //    }
                            //}
                            for (int i = 0; i < Options.SortingParamList.Count; i++)
                            {
                                SortingParam param = Options.SortingParamList[i];
                                if (param.ColumnName.Column == columnName.Column)
                                {
                                    sortingParam = param;
                                    break;
                                }
                            }
                            if (sortingParam != null)
                            {
                                Options.SortingParamList.Remove(sortingParam);
                            }

                            UnusedReportColumns.Add(columnName);
                            UsedReportColumns.Remove(columnName);
                        }
                    },
                    (obj) =>
                    {
                        if (UsedReportColumns.Count > 0)
                        {
                            if (SelectedUsedColumn != null)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }));

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
                //foreach (ReportColumnName relation in ReportColumnNameCollection.Collection)
                //{
                //    if (ReportRelationCollection.Collection[Options.TypeReport].Columns.Contains(relation.Column))
                //    {
                //        UnusedReportColumns.Add(relation);
                //    }
                //}
                for (int i = 0; i < ReportColumnNameCollection.Collection.Count; i++)
                {
                    ReportColumnName relation = ReportColumnNameCollection.Collection[i];
                    if (ReportRelationCollection.Collection[Options.TypeReport].Columns.Contains(relation.Column))
                    {
                        UnusedReportColumns.Add(relation);
                    }
                }
            }
            if (UsedReportColumns.Count > 0)
            {
                SelectedUsedColumn = UsedReportColumns?[0];
            }

            if (UnusedReportColumns.Count > 0)
            {
                SelectedUnusedColumn = UnusedReportColumns?[0];
            }
        }

        private string CommandTextBuilder(bool isFull = false, int audience = 0)
        {
            List<string> vs = new List<string>
            {
                Capacity = 32
            };

            ReportRelation relation = ReportRelationCollection.Collection[Options.TypeReport];
            
            string commandText;

            if (!Options.SplitByAudience)
            {
                commandText = "SELECT ";

                if (isFull)
                {
                    //foreach (ReportColumn column in relation.Columns)
                    //{
                    //    vs.Add(ReportColumnNameCollection.GetColumnName(column).Name);
                    //}
                    for (int j = 0; j < relation.Columns.Count; j++)
                    {
                        vs.Add(ReportColumnNameCollection.GetColumnName(relation.Columns[j]).Name);
                    }
                }
                else
                {
                    //foreach (ReportColumnName columnRelation in UsedReportColumns)
                    //{
                    //    vs.Add(ReportColumnNameCollection.GetColumnName(columnRelation.Column).Name);
                    //}
                    for (int j = 0; j < UsedReportColumns.Count; j++)
                    {
                        vs.Add(ReportColumnNameCollection.GetColumnName(UsedReportColumns[j].Column).Name);
                    }
                }

                int i = 0;
                //foreach (string str in vs)
                //{
                //    commandText += $"[{str}]";
                //    i++;
                //    if (i < vs.Count)
                //    {
                //        commandText += ", ";
                //    }
                //}
                for (int j = 0; j < vs.Count; j++)
                {
                    commandText += $"[{vs[i]}]";
                    i++;
                    if (i < vs.Count)
                    {
                        commandText += ", ";
                    }
                }

                commandText += $" FROM dbo.{relation.Function}(";
                if (Options.TypeReport != TypeReport.UseSoft)
                {
                    if (Options.IsPeriod)
                    {
                        if (Options.FromDate != null)
                        {
                            commandText += $"'{Options.FromDate.Value:yyyy-MM-dd}'";
                        }
                        else
                        {
                            commandText += $"default";
                        }

                        commandText += $", ";
                        if (Options.FromDate != null)
                        {
                            commandText += $"'{Options.ToDate.Value:yyyy-MM-dd}'";
                        }
                        else
                        {
                            commandText += $"default";
                        }
                    }
                    else
                    {
                        commandText += $"default, default";
                    }
                }
                commandText += $")";
                commandText += Options.GetSortingString(isFull);
            }
            else
            {
                commandText = $"EXEC {relation.SP} ";//добавить параметры
                if (Options.IsPeriod)
                {
                    if (Options.FromDate != null)
                    {
                        commandText += $"@From='{Options.FromDate.Value:yyyy-MM-dd}', ";
                    }

                    if (Options.FromDate != null)
                    {
                        commandText += $"@To='{Options.ToDate.Value:yyyy-MM-dd}', ";
                    }
                }
                commandText += $"@Audience={audience}";
            }
            

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
            if (Options.SplitByAudience)
            {
                DataTable table = new DataTable();
                new SqlDataAdapter("Select ID, Name From Audience", ConnectionString).Fill(table);
                for (int z = 0; z < table.Rows.Count; z++)
                {
                    string tableName = ReportRelationCollection.Collection[Options.TypeReport].TableName + " " + table.Rows[z]["Name"].ToString();

                    set.Tables.Add(tableName);
                    new SqlDataAdapter(CommandTextBuilder(isFull, Convert.ToInt32(table.Rows[z]["ID"])), ConnectionString).Fill(set, tableName); // добавить where audience 

                    if (set.Tables[tableName].Columns.Contains("Видеоразъемы"))
                    {
                        set.Tables[tableName].Columns["Видеоразъемы"].ColumnName = "VideoConnectors";
                        set.Tables[tableName].Columns.Add("Видеоразъемы");

                        for (int rowIndex = 0; rowIndex < set.Tables[tableName].Rows.Count; rowIndex++)
                        {
                            set.Tables[tableName].Rows[rowIndex]["Видеоразъемы"] =
                                AccountingPCWindow.GetVideoConnectors(Convert.ToInt32(set.Tables[tableName].Rows[rowIndex]["VideoConnectors"]));
                        }
                    }

                    int i = 0;
                    int colCount = UsedReportColumns.Count;
                    for (int j = 0; j < colCount; j++)
                    {
                        if (set.Tables[tableName].Columns.Contains(UsedReportColumns[i].Name))
                        {
                            set.Tables[tableName].Columns[UsedReportColumns[i].Name].SetOrdinal(i);
                            i++;
                        }
                    }

                    if (set.Tables[tableName].Columns.Contains("VideoConnectors"))
                    {
                        set.Tables[tableName].Columns.Remove("VideoConnectors");
                    }
                }
            }
            else
            {
                string tableName = ReportRelationCollection.Collection[Options.TypeReport].TableName;

                set.Tables.Add(tableName);
                new SqlDataAdapter(CommandTextBuilder(isFull), ConnectionString).Fill(set, tableName);

                if (set.Tables[tableName].Columns.Contains("Видеоразъемы"))
                {
                    set.Tables[tableName].Columns["Видеоразъемы"].ColumnName = "VideoConnectors";
                    set.Tables[tableName].Columns.Add("Видеоразъемы");

                    //foreach (DataRow row in set.Tables[tableName].Rows)
                    //{
                    //    row["Видеоразъемы"] = AccountingPCWindow.GetVideoConnectors(Convert.ToInt32(row["VideoConnectors"]));
                    //}
                    for (int rowIndex = 0; rowIndex < set.Tables[tableName].Rows.Count; rowIndex++)
                    {
                        set.Tables[tableName].Rows[rowIndex]["Видеоразъемы"] = AccountingPCWindow.GetVideoConnectors(Convert.ToInt32(set.Tables[tableName].Rows[rowIndex]["VideoConnectors"]));
                    }
                }

                int i = 0;
                //foreach (ReportColumnName column in UsedReportColumns)
                //{
                //    if (set.Tables[tableName].Columns.Contains(column.Name))
                //    {
                //        set.Tables[tableName].Columns[column.Name].SetOrdinal(i);
                //        i++;
                //    }
                //}

                int colCount = UsedReportColumns.Count;
                for (int j = 0; j < colCount; j++)
                {
                    if (set.Tables[tableName].Columns.Contains(UsedReportColumns[i].Name))
                    {
                        set.Tables[tableName].Columns[UsedReportColumns[i].Name].SetOrdinal(i);
                        i++;
                    }
                }


                if (set.Tables[tableName].Columns.Contains("VideoConnectors"))
                {
                    set.Tables[tableName].Columns.Remove("VideoConnectors");
                }
            }
            
            //string tableName = ReportRelationCollection.Collection[Options.TypeReport].TableName;

            //set.Tables.Add(tableName);
            //new SqlDataAdapter(CommandTextBuilder(isFull), ConnectionString).Fill(set, tableName);

            //if (set.Tables[tableName].Columns.Contains("Видеоразъемы"))
            //{
            //    set.Tables[tableName].Columns["Видеоразъемы"].ColumnName = "VideoConnectors";
            //    set.Tables[tableName].Columns.Add("Видеоразъемы");

            //    //foreach (DataRow row in set.Tables[tableName].Rows)
            //    //{
            //    //    row["Видеоразъемы"] = AccountingPCWindow.GetVideoConnectors(Convert.ToInt32(row["VideoConnectors"]));
            //    //}
            //    for (int rowIndex = 0; rowIndex < set.Tables[tableName].Rows.Count; rowIndex++)
            //    {
            //        set.Tables[tableName].Rows[rowIndex]["Видеоразъемы"] = AccountingPCWindow.GetVideoConnectors(Convert.ToInt32(set.Tables[tableName].Rows[rowIndex]["VideoConnectors"]));
            //    }
            //}

            //int i = 0;
            ////foreach (ReportColumnName column in UsedReportColumns)
            ////{
            ////    if (set.Tables[tableName].Columns.Contains(column.Name))
            ////    {
            ////        set.Tables[tableName].Columns[column.Name].SetOrdinal(i);
            ////        i++;
            ////    }
            ////}

            //int colCount = UsedReportColumns.Count;
            //for (int j=0; j < colCount; j++)
            //{
            //    if (set.Tables[tableName].Columns.Contains(UsedReportColumns[i].Name))
            //    {
            //        set.Tables[tableName].Columns[UsedReportColumns[i].Name].SetOrdinal(i);
            //        i++;
            //    }
            //}


            //if (set.Tables[tableName].Columns.Contains("VideoConnectors"))
            //{
            //    set.Tables[tableName].Columns.Remove("VideoConnectors");
            //}
        }

        public ExcelFile CreateReport()
        {
            DataSet dataSet = GetDataSet();

            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
            SpreadsheetInfo.FreeLimitReached += (sender, e) => e.FreeLimitReachedAction = FreeLimitReachedAction.ContinueAsTrial;

            ExcelFile book = new ExcelFile();

            //foreach (DataTable dataTable in dataSet.Tables)
            for (int q = 0; q < dataSet.Tables.Count; q++)
            {
                DataTable dataTable = dataSet.Tables[q];
                ExcelWorksheet worksheet = book.Worksheets.Add(dataTable.TableName);

                worksheet.InsertDataTable(dataTable, new InsertDataTableOptions()
                {
                    ColumnHeaders = true,
                    StartColumn = 2,
                    StartRow = 1
                });
                int rowCount = worksheet.Rows.Count;
                worksheet.Cells[rowCount + 1, 0].Value = "Итого:";
                int col;
                if (Options.IsShowUnitOfMeasurement)
                {
                    // Установка денежного типа для Цены
                    if (worksheet.Cells.FindText("Цена", false, out _, out col))
                    {
                        worksheet.Columns[col].Cells.Style.NumberFormat = NumberFormatBuilder.Currency("\u20bd", 2, true, false, true);

                        if (worksheet.Cells.FindText("Общая стоимость", false, out _, out int col1))
                        {
                            worksheet.Columns[col].Cells.Style.NumberFormat = NumberFormatBuilder.Currency("\u20bd", 2, true, false, true);
                            if (worksheet.Cells.FindText("Количество", false, out _, out int col2))
                            {
                                //int rowIndex;
                                //foreach (ExcelRow row in worksheet.Rows)
                                //{
                                //    rowIndex = row.Index;
                                //    if (rowIndex == 0)
                                //    {
                                //        continue;
                                //    }

                                //    string cell = worksheet.Cells[rowIndex, col].Name;
                                //    string cell2 = worksheet.Cells[rowIndex, col2].Name;
                                //    string formula = $"={cell}*{cell2}";
                                //    worksheet.Cells[rowIndex, col1].Formula = formula;
                                //}
                                string totalCostName = worksheet.Columns[col1].Name;
                                string countName = worksheet.Columns[col2].Name;
                                for (int i = 1; i < rowCount; i++)
                                {
                                    worksheet.Cells[i, col1].Formula = $"={totalCostName}{i}*{countName}{i}";
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
                        worksheet.Columns[col].Cells.Style.NumberFormat = "###__МГц";
                    }
                    if (worksheet.Cells.FindText("Максимальная частота", false, out _, out col))
                    {
                        worksheet.Columns[col].Cells.Style.NumberFormat = "###__МГц";
                    }
                    if (worksheet.Cells.FindText("Частота обновления", false, out _, out col))
                    {
                        worksheet.Columns[col].Cells.Style.NumberFormat = "###__МГц";
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
                }

                if (Options.IsCountMaxMinAverageSum)
                {
                    // Установка денежного типа для Цены
                    if (worksheet.Cells.FindText("Цена", false, out _, out col))
                    {
                        string begin = worksheet.Cells[0, col].Name;
                        string end = worksheet.Cells[rowCount, col].Name;
                        worksheet.Cells[rowCount + 1, col].Formula = $"=SUM({begin}:{end})";

                        worksheet.Cells[rowCount + 2, col - 1].Value = "Минимальная цена";
                        worksheet.Cells[rowCount + 2, col].Formula = $"=MIN({begin}:{end})";

                        worksheet.Cells[rowCount + 3, col - 1].Value = "Средняя цена";
                        worksheet.Cells[rowCount + 3, col].Formula = $"=AVERAGE({begin}:{end})";

                        worksheet.Cells[rowCount + 4, col - 1].Value = "Максимальная цена";
                        worksheet.Cells[rowCount + 4, col].Formula = $"=MAX({begin}:{end})";

                        if (Options.IsShowUnitOfMeasurement)
                        {
                            worksheet.Cells[rowCount + 1, col].Style.NumberFormat = NumberFormatBuilder.Currency("\u20bd", 2, true, false, true);
                            worksheet.Cells[rowCount + 2, col].Style.NumberFormat = NumberFormatBuilder.Currency("\u20bd", 2, true, false, true);
                            worksheet.Cells[rowCount + 3, col].Style.NumberFormat = NumberFormatBuilder.Currency("\u20bd", 2, true, false, true);
                            worksheet.Cells[rowCount + 4, col].Style.NumberFormat = NumberFormatBuilder.Currency("\u20bd", 2, true, false, true);
                        }

                        if (worksheet.Cells.FindText("Общая стоимость", false, out _, out int col1))
                        {
                            begin = worksheet.Cells[0, col1].Name;
                            end = worksheet.Cells[rowCount, col1].Name;
                            worksheet.Cells[rowCount + 1, col1].Formula = $"=SUM({begin}:{end})";
                            if (Options.IsShowUnitOfMeasurement)  
                                worksheet.Columns[col].Cells.Style.NumberFormat = NumberFormatBuilder.Currency("\u20bd", 2, true, false, true);
                            if (worksheet.Cells.FindText("Количество", false, out _, out int col2))
                            {
                                begin = worksheet.Cells[0, col2].Name;
                                end = worksheet.Cells[rowCount, col2].Name;
                                worksheet.Cells[rowCount + 1, col2].Formula = $"=SUM({begin}:{end})";

                                //int rowIndex;
                                //foreach (ExcelRow row in worksheet.Rows)
                                //{
                                //    rowIndex = row.Index;
                                //    if (rowIndex == 0)
                                //    {
                                //        continue;
                                //    }

                                //    string cell = worksheet.Cells[rowIndex, col].Name;
                                //    string cell2 = worksheet.Cells[rowIndex, col2].Name;
                                //    string formula = $"={cell}*{cell2}";
                                //    worksheet.Cells[rowIndex, col1].Formula = formula;
                                //}
                                string totalCostName = worksheet.Columns[col1].Name;
                                string countName = worksheet.Columns[col2].Name;
                                for (int i = 1; i < rowCount; i++)
                                {
                                    worksheet.Cells[i, col1].Formula = $"={totalCostName}{i}*{countName}{i}";
                                }
                            }
                        }
                    }
                    if (worksheet.Cells.FindText("Инвентарный номер", false, out _, out col))
                    {
                        worksheet.Columns[col].Cells.Style.NumberFormat = "000000000000000";
                        string begin = worksheet.Cells[0, col].Name;
                        string end = worksheet.Cells[rowCount, col].Name;
                        worksheet.Cells[rowCount + 1, col].Formula = $"=COUNT({begin}:{end})";
                        if (Options.IsShowUnitOfMeasurement) 
                            worksheet.Cells[rowCount + 1, col].Style.NumberFormat = "0__Устройств";
                    }
                }
                //foreach (ExcelColumn column in worksheet.Columns)
                //{
                //    column.AutoFit();
                //}

                int columnCount = worksheet.CalculateMaxUsedColumns();
                for (int i = 0; i < columnCount; i++)
                {
                    worksheet.Columns[i].AutoFit(1, worksheet.Rows[0], worksheet.Rows[worksheet.Rows.Count - 1]);
                }
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
