using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;

namespace AccountingPC
{
    public partial class AccountingPCWindow
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void LoadInvoiceList()
        {
            DefaultDataSet.Tables["Invoice"].Clear();
            new SqlDataAdapter("SELECT * FROM dbo.[GetAllInvoice]()", ConnectionString).Fill(DefaultDataSet, "Invoice");
            invoiceList.ItemsSource = DefaultDataSet.Tables["Invoice"].DefaultView;
            invoiceList.DisplayMemberPath = "Number";
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

            for (int i = 0; i < InvoiceSoftwareAndEquipmentDataSet.Tables.Count; i++)
            {
                DataTable table = InvoiceSoftwareAndEquipmentDataSet.Tables[i];
                if (table.Columns.Contains("VideoConnectors"))
                {
                    if (table.Columns.Contains("VideoConnectors"))
                    {
                        table.Columns.Add("Видеоразъемы");
                        for (int j=0; j < table.Rows.Count; j++)
                        {
                            DataRow row = table.Rows[j];
                            row["Видеоразъемы"] = row["VideoConnectors"].GetType() == typeof(int) ?
                                GetVideoConnectors(Convert.ToInt32(row["VideoConnectors"])) : row["VideoConnectors"];
                        }
                        int i1 = table.DefaultView.Table.Columns.IndexOf("VideoConnectors");
                        int i2 = table.DefaultView.Table.Columns.IndexOf("Видеоразъемы");
                        table.DefaultView.Table.Columns["Видеоразъемы"].SetOrdinal(i1);
                        table.DefaultView.Table.Columns["VideoConnectors"].SetOrdinal(i2);
                    }
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ChangeInvoiceView()
        {
            UpdateInvoiceData();
            invoiceItemsControl.ItemsSource = InvoiceSoftwareAndEquipmentDataSet.Tables;
            invoiceNumberManager.Text = ((DataRowView)invoiceList?.SelectedItem)?.Row?["Number"].ToString();
            dateManager.SelectedDate = Convert.ToDateTime(((DataRowView)invoiceList?.SelectedItem)?.Row?["Date"]);
        }
    }
}