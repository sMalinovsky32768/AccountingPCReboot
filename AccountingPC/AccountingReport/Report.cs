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
    internal partial class Report : INotifyPropertyChanged
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void InitializeColumn()
        {
            UsedReportColumns = new ObservableCollection<ReportColumnName>();
            UnusedReportColumns = new ObservableCollection<ReportColumnName>();
            if (Options.TypeReport != TypeReport.Full)
            {
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

            if (!Options.SplitByAudience || string.IsNullOrWhiteSpace(relation.SP)) 
            {
                commandText = "SELECT ";

                if (isFull)
                {
                    for (int j = 0; j < relation.Columns.Count; j++)
                    {
                        vs.Add(ReportColumnNameCollection.GetColumnName(relation.Columns[j]).Name);
                    }
                }
                else
                {
                    for (int j = 0; j < UsedReportColumns.Count; j++)
                    {
                        vs.Add(ReportColumnNameCollection.GetColumnName(UsedReportColumns[j].Column).Name);
                    }
                }

                int i = 0;
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
                commandText = $"EXEC {relation.SP} ";
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
            if (Options.SplitByAudience && 
                Options.TypeReport != TypeReport.OS && 
                Options.TypeReport != TypeReport.SoftAndOS && 
                Options.TypeReport != TypeReport.Software && 
                Options.TypeReport != TypeReport.UseSoft)
            {
                DataTable table = new DataTable();
                new SqlDataAdapter("Select ID, Name From Audience", ConnectionString).Fill(table);
                for (int z = 0; z < table.Rows.Count; z++)
                {
                    string tableName = ReportRelationCollection.Collection[Options.TypeReport].TableName + " " + table.Rows[z]["Name"].ToString();

                    set.Tables.Add(tableName);
                    new SqlDataAdapter(CommandTextBuilder(isFull, Convert.ToInt32(table.Rows[z]["ID"])), ConnectionString).Fill(set, tableName); 

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

                    for (int rowIndex = 0; rowIndex < set.Tables[tableName].Rows.Count; rowIndex++)
                    {
                        set.Tables[tableName].Rows[rowIndex]["Видеоразъемы"] = AccountingPCWindow.GetVideoConnectors(Convert.ToInt32(set.Tables[tableName].Rows[rowIndex]["VideoConnectors"]));
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
