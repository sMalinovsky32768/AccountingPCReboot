using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Windows;

namespace AccountingPC
{
    public partial class AccountingPCWindow
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void LoadInvoiceList()
        {
            try
            {
                DefaultDataSet.Tables["Invoice"].Clear();
                new SqlDataAdapter("SELECT * FROM dbo.[GetAllInvoice]()", ConnectionString).Fill(DefaultDataSet, "Invoice");
                invoiceList.ItemsSource = DefaultDataSet.Tables["Invoice"].DefaultView;
                invoiceList.DisplayMemberPath = "Number";
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, exception.GetType().Name, MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateInvoiceData()
        {
            InvoiceSoftwareAndEquipmentDataSet = new DataSet();
            InvoiceSoftwareAndEquipmentDataSet.Tables.Add("Компьютеры");
            InvoiceSoftwareAndEquipmentDataSet.Tables.Add("Ноутбуки и Моноблоки");
            InvoiceSoftwareAndEquipmentDataSet.Tables.Add("Мониторы");
            InvoiceSoftwareAndEquipmentDataSet.Tables.Add("Проекторы");
            InvoiceSoftwareAndEquipmentDataSet.Tables.Add("Интерактивные доски");
            InvoiceSoftwareAndEquipmentDataSet.Tables.Add("Экраны для проекторов");
            InvoiceSoftwareAndEquipmentDataSet.Tables.Add("Принтеры и сканеры");
            InvoiceSoftwareAndEquipmentDataSet.Tables.Add("Сетевое оборудование");
            InvoiceSoftwareAndEquipmentDataSet.Tables.Add("Другое оборудование");
            InvoiceSoftwareAndEquipmentDataSet.Tables.Add("Программное обеспечение");
            InvoiceSoftwareAndEquipmentDataSet.Tables.Add("Операционные системы");

            new SqlDataAdapter($"SELECT * FROM dbo.GetAllPC() Where InvoiceID={InvoiceID}",
                ConnectionString).Fill(InvoiceSoftwareAndEquipmentDataSet, "Компьютеры");
            new SqlDataAdapter($"SELECT * FROM dbo.GetAllNotebook() Where InvoiceID={InvoiceID}",
                ConnectionString).Fill(InvoiceSoftwareAndEquipmentDataSet, "Ноутбуки и Моноблоки");
            new SqlDataAdapter($"SELECT * FROM dbo.GetAllMonitor() Where InvoiceID={InvoiceID}",
                ConnectionString).Fill(InvoiceSoftwareAndEquipmentDataSet, "Мониторы");
            new SqlDataAdapter($"SELECT * FROM dbo.GetAllProjector() Where InvoiceID={InvoiceID}",
                ConnectionString).Fill(InvoiceSoftwareAndEquipmentDataSet, "Проекторы");
            new SqlDataAdapter($"SELECT * FROM dbo.GetAllBoard() Where InvoiceID={InvoiceID}",
                ConnectionString).Fill(InvoiceSoftwareAndEquipmentDataSet, "Интерактивные доски");
            new SqlDataAdapter($"SELECT * FROM dbo.GetAllScreen() Where InvoiceID={InvoiceID}",
                ConnectionString).Fill(InvoiceSoftwareAndEquipmentDataSet, "Экраны для проекторов");
            new SqlDataAdapter($"SELECT * FROM dbo.GetAllPrinterScanner() Where InvoiceID={InvoiceID}",
                ConnectionString).Fill(InvoiceSoftwareAndEquipmentDataSet, "Принтеры и сканеры");
            new SqlDataAdapter($"SELECT * FROM dbo.GetAllNetworkSwitch() Where InvoiceID={InvoiceID}",
                ConnectionString).Fill(InvoiceSoftwareAndEquipmentDataSet, "Сетевое оборудование");
            new SqlDataAdapter($"SELECT * FROM dbo.GetAllOtherEquipment() Where InvoiceID={InvoiceID}",
                ConnectionString).Fill(InvoiceSoftwareAndEquipmentDataSet, "Другое оборудование");
            new SqlDataAdapter($"SELECT * FROM dbo.GetAllSoftware() Where InvoiceID={InvoiceID}",
                ConnectionString).Fill(InvoiceSoftwareAndEquipmentDataSet, "Программное обеспечение");
            new SqlDataAdapter($"SELECT * FROM dbo.GetAllOS() Where InvoiceID={InvoiceID}",
                ConnectionString).Fill(InvoiceSoftwareAndEquipmentDataSet, "Операционные системы");

            for (var i = 0; i < InvoiceSoftwareAndEquipmentDataSet.Tables.Count; i++)
            {
                var table = InvoiceSoftwareAndEquipmentDataSet.Tables[i];
                if (table.Columns.Contains("VideoConnectors"))
                    if (table.Columns.Contains("VideoConnectors"))
                    {
                        table.Columns.Add("Видеоразъемы");
                        for (var j = 0; j < table.Rows.Count; j++)
                        {
                            var row = table.Rows[j];
                            row["Видеоразъемы"] = row["VideoConnectors"] is int
                                ? GetVideoConnectors(Convert.ToInt32(row["VideoConnectors"]))
                                : row["VideoConnectors"];
                        }

                        var i1 = table.DefaultView.Table.Columns.IndexOf("VideoConnectors");
                        var i2 = table.DefaultView.Table.Columns.IndexOf("Видеоразъемы");
                        table.DefaultView.Table.Columns["Видеоразъемы"].SetOrdinal(i1);
                        table.DefaultView.Table.Columns["VideoConnectors"].SetOrdinal(i2);
                    }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ChangeInvoiceView()
        {
            try
            {
                UpdateInvoiceData();
                invoiceItemsControl.ItemsSource = InvoiceSoftwareAndEquipmentDataSet.Tables;
                invoiceNumberManager.Text = ((DataRowView) invoiceList?.SelectedItem)?.Row?["Number"].ToString() ?? string.Empty;
                dateManager.SelectedDate = Convert.ToDateTime(((DataRowView) invoiceList?.SelectedItem)?.Row?["Date"]);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, exception.GetType().Name, MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}