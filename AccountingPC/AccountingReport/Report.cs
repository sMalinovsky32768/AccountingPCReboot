using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Windows;

namespace AccountingPC.AccountingReport
{
    internal partial class Report : INotifyPropertyChanged
    {
        public delegate void ColumnUpdate();

        public static readonly string ConnectionString =
            ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        private ReportCommand addUsedReportColumns;

        private ReportCommand delUsedReportColumns;

        private ReportColumnName selectedUnusedColumn;
        private ReportColumnName selectedUsedColumn;
        private ObservableCollection<ReportColumnName> unusedReportColumns;

        private ObservableCollection<ReportColumnName> usedReportColumns;

        public Report()
        {
            Options = new ReportOptions(this);
            Options.TypeReportChangedEvent += TypeChangedEventHandler;
        }

        public ObservableCollection<ReportColumnName> UsedReportColumns
        {
            get => usedReportColumns;
            set
            {
                usedReportColumns = value;
                UsedColumnUpdateEvent?.Invoke();
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ReportColumnName> UnusedReportColumns
        {
            get => unusedReportColumns;
            set
            {
                unusedReportColumns = value;
                UnusedColumnUpdateEvent?.Invoke();
                OnPropertyChanged();
            }
        }

        public ReportColumnName SelectedUsedColumn
        {
            get => selectedUsedColumn;
            set
            {
                selectedUsedColumn = value;
                OnPropertyChanged();
            }
        }

        public ReportColumnName SelectedUnusedColumn
        {
            get => selectedUnusedColumn;
            set
            {
                selectedUnusedColumn = value;
                OnPropertyChanged();
            }
        }

        public ReportCommand AddUsedReportColumns => addUsedReportColumns ??
         (addUsedReportColumns = new ReportCommand(obj =>
             {
                 if (obj != null)
                 {
                     var columnName = obj as ReportColumnName;
                     for (var i = 0; i < UnusedReportColumns.Count; i++)
                     {
                         var reportColumnName = UnusedReportColumns[i];
                         if (reportColumnName == columnName)
                             SelectedUnusedColumn =
                                 UnusedReportColumns[
                                     UnusedReportColumns
                                         .IndexOf(columnName) <
                                     UnusedReportColumns.Count - 1
                                         ? UnusedReportColumns.IndexOf(
                                             columnName) + 1
                                         : 0];
                     }

                     UsedReportColumns.Add(columnName);
                     UnusedReportColumns.Remove(columnName);
                 }
             },
             obj =>
             {
                 if (UnusedReportColumns.Count > 0)
                 {
                     if (SelectedUnusedColumn != null)
                         return true;
                     return false;
                 }

                 return false;
             }));

        public ReportCommand DelUsedReportColumns => delUsedReportColumns ??
         (delUsedReportColumns = new ReportCommand(obj =>
             {
                 if (obj != null)
                 {
                     var columnName = obj as ReportColumnName;
                     for (var i = 0; i < UsedReportColumns.Count; i++)
                     {
                         var reportColumnName = UsedReportColumns[i];
                         if (reportColumnName == columnName)
                             SelectedUsedColumn =
                                 UsedReportColumns[
                                     UsedReportColumns.IndexOf(columnName) <
                                     UsedReportColumns.Count - 1
                                         ? UsedReportColumns.IndexOf(
                                             columnName) + 1
                                         : 0];
                     }

                     SortingParam sortingParam = null;
                     for (var i = 0;
                         i < Options.SortingParamList.Count;
                         i++)
                     {
                         var param = Options.SortingParamList[i];
                         if (param.ColumnName.Column == columnName.Column)
                         {
                             sortingParam = param;
                             break;
                         }
                     }

                     if (sortingParam != null)
                         Options.SortingParamList.Remove(sortingParam);

                     UnusedReportColumns.Add(columnName);
                     UsedReportColumns.Remove(columnName);
                 }
             },
             obj =>
             {
                 if (UsedReportColumns.Count > 0)
                 {
                     if (SelectedUsedColumn != null)
                         return true;
                     return false;
                 }

                 return false;
             }));

        public ReportOptions Options { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public event ColumnUpdate UsedColumnUpdateEvent;
        public event ColumnUpdate UnusedColumnUpdateEvent;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void InitializeColumn()
        {
            UsedReportColumns = new ObservableCollection<ReportColumnName>();
            UnusedReportColumns = new ObservableCollection<ReportColumnName>();
            if (Options.TypeReport != TypeReport.Full)
                for (var i = 0; i < ReportColumnNameCollection.Collection.Count; i++)
                {
                    var relation = ReportColumnNameCollection.Collection[i];
                    if (ReportRelationCollection.Collection[Options.TypeReport].Columns.Contains(relation.Column))
                        UnusedReportColumns.Add(relation);
                }

            if (UsedReportColumns.Count > 0) SelectedUsedColumn = UsedReportColumns?[0];

            if (UnusedReportColumns.Count > 0) SelectedUnusedColumn = UnusedReportColumns?[0];
        }

        private string CommandTextBuilder(bool isFull = false, int audience = 0)
        {
            var vs = new List<string>
            {
                Capacity = 32
            };

            var relation = ReportRelationCollection.Collection[Options.TypeReport];

            string commandText;

            if (!Options.SplitByAudience || string.IsNullOrWhiteSpace(relation.SP))
            {
                commandText = "SELECT ";

                if (isFull)
                    for (var j = 0; j < relation.Columns.Count; j++)
                        vs.Add(ReportColumnNameCollection.GetColumnName(relation.Columns[j]).Name);
                else
                    for (var j = 0; j < UsedReportColumns.Count; j++)
                        vs.Add(ReportColumnNameCollection.GetColumnName(UsedReportColumns[j].Column).Name);

                var i = 0;
                for (var j = 0; j < vs.Count; j++)
                {
                    commandText += $"[{vs[i]}]";
                    i++;
                    if (i < vs.Count) commandText += ", ";
                }

                commandText += $" FROM dbo.{relation.Function}(";
                if (Options.TypeReport != TypeReport.UseSoft)
                {
                    if (Options.IsPeriod)
                    {
                        if (Options.FromDate != null)
                            commandText += $"'{Options.FromDate.Value:yyyy-MM-dd}'";
                        else
                            commandText += "default";

                        commandText += ", ";
                        if (Options.FromDate != null)
                            commandText += $"'{Options.ToDate.Value:yyyy-MM-dd}'";
                        else
                            commandText += "default";
                    }
                    else
                    {
                        commandText += "default, default";
                    }
                }

                commandText += ")";
                commandText += Options.GetSortingString(isFull);
            }
            else
            {
                commandText = $"EXEC {relation.SP} ";
                if (Options.IsPeriod)
                {
                    if (Options.FromDate != null) commandText += $"@From='{Options.FromDate.Value:yyyy-MM-dd}', ";

                    if (Options.FromDate != null) commandText += $"@To='{Options.ToDate.Value:yyyy-MM-dd}', ";
                }

                commandText += $"@Audience={audience}";
            }


            return commandText;
        }

        private DataSet GetDataSet()
        {
            var set = new DataSet();

            if (Options.TypeReport == TypeReport.Full)
            {
                foreach (var type in ReportRelationCollection.Collection.Keys)
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
                var table = new DataTable();
                try
                {
                    new SqlDataAdapter("Select ID, Name From Audience", ConnectionString).Fill(table);
                    for (var z = 0; z < table.Rows.Count; z++)
                    {
                        var tableName = ReportRelationCollection.Collection[Options.TypeReport].TableName + " " +
                                        table.Rows[z]["Name"];

                        set.Tables.Add(tableName);
                        new SqlDataAdapter(CommandTextBuilder(isFull, Convert.ToInt32(table.Rows[z]["ID"])),
                            ConnectionString).Fill(set, tableName);

                        if (set.Tables[tableName].Columns.Contains("Видеоразъемы"))
                        {
                            set.Tables[tableName].Columns["Видеоразъемы"].ColumnName = "VideoConnectors";
                            set.Tables[tableName].Columns.Add("Видеоразъемы");

                            for (var rowIndex = 0; rowIndex < set.Tables[tableName].Rows.Count; rowIndex++)
                                set.Tables[tableName].Rows[rowIndex]["Видеоразъемы"] =
                                    AccountingPCWindow.GetVideoConnectors(
                                        Convert.ToInt32(set.Tables[tableName].Rows[rowIndex]["VideoConnectors"]));
                        }

                        var i = 0;
                        var colCount = UsedReportColumns.Count;
                        for (var j = 0; j < colCount; j++)
                            if (set.Tables[tableName].Columns.Contains(UsedReportColumns[i].Name))
                            {
                                set.Tables[tableName].Columns[UsedReportColumns[i].Name].SetOrdinal(i);
                                i++;
                            }

                        if (set.Tables[tableName].Columns.Contains("VideoConnectors"))
                            set.Tables[tableName].Columns.Remove("VideoConnectors");
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message, exception.GetType().Name, MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
            else
            {
                var tableName = ReportRelationCollection.Collection[Options.TypeReport].TableName;

                set.Tables.Add(tableName);
                try
                {
                    new SqlDataAdapter(CommandTextBuilder(isFull), ConnectionString).Fill(set, tableName);

                    if (set.Tables[tableName].Columns.Contains("Видеоразъемы"))
                    {
                        set.Tables[tableName].Columns["Видеоразъемы"].ColumnName = "VideoConnectors";
                        set.Tables[tableName].Columns.Add("Видеоразъемы");

                        for (var rowIndex = 0; rowIndex < set.Tables[tableName].Rows.Count; rowIndex++)
                            set.Tables[tableName].Rows[rowIndex]["Видеоразъемы"] =
                                AccountingPCWindow.GetVideoConnectors(
                                    Convert.ToInt32(set.Tables[tableName].Rows[rowIndex]["VideoConnectors"]));
                    }

                    var i = 0;
                    var colCount = UsedReportColumns.Count;
                    for (var j = 0; j < colCount; j++)
                        if (set.Tables[tableName].Columns.Contains(UsedReportColumns[i].Name))
                        {
                            set.Tables[tableName].Columns[UsedReportColumns[i].Name].SetOrdinal(i);
                            i++;
                        }

                    if (set.Tables[tableName].Columns.Contains("VideoConnectors"))
                        set.Tables[tableName].Columns.Remove("VideoConnectors");
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message, exception.GetType().Name, MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
        }

        private void TypeChangedEventHandler()
        {
            try
            {
                InitializeColumn();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, exception.GetType().Name, MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}