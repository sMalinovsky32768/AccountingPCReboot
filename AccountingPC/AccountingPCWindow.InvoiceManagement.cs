using System;
using System.Data;
using System.Data.SqlClient;

namespace AccountingPC
{
    public partial class AccountingPCWindow
    {
        private void LoadInvoiceList()
        {
            InvoiceDataSet = new DataSet();
            new SqlDataAdapter("SELECT * FROM dbo.[GetAllInvoice]()", ConnectionString).Fill(InvoiceDataSet);
            invoiceList.ItemsSource = InvoiceDataSet.Tables[0].DefaultView;
            invoiceList.DisplayMemberPath = "Number";
        }

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

            foreach (DataTable table in InvoiceSoftwareAndEquipmentDataSet.Tables)
            {
                if (table.Columns.Contains("VideoConnectors"))
                {
                    if (table.Columns.Contains("VideoConnectors"))
                    {
                        table.Columns.Add("Видеоразъемы");
                        foreach (DataRow row in table.Rows)
                        {
                            row["Видеоразъемы"] = row["VideoConnectors"].GetType() == typeof(int) ?
                                GetVideoConnectors(Convert.ToInt32(row["VideoConnectors"])) : row["VideoConnectors"];
                        }
                        int i = table.DefaultView.Table.Columns.IndexOf("VideoConnectors");
                        int ii = table.DefaultView.Table.Columns.IndexOf("Видеоразъемы");
                        table.DefaultView.Table.Columns["Видеоразъемы"].SetOrdinal(i);
                        table.DefaultView.Table.Columns["VideoConnectors"].SetOrdinal(ii);
                    }
                }
            }

        }

        private void ChangeInvoiceView()
        {
            UpdateInvoiceData();
            invoiceItemsControl.ItemsSource = InvoiceSoftwareAndEquipmentDataSet.Tables;
            //changeInvoicePanel.DataContext = invoiceList.SelectedItem;
            invoiceNumberManager.Text = ((DataRowView)invoiceList?.SelectedItem)?.Row?["Number"].ToString();
            dateManager.SelectedDate = Convert.ToDateTime(((DataRowView)invoiceList?.SelectedItem)?.Row?["Date"]);
            //invoicePCView.ItemsSource = invoiceSoftwareAndEquipmentDataSet.Tables["Компьютеры"].DefaultView;
            //invoiceNotebookView.ItemsSource = invoiceSoftwareAndEquipmentDataSet.Tables["Ноутбуки и Моноблоки"].DefaultView;
            //invoiceMonitorView.ItemsSource = invoiceSoftwareAndEquipmentDataSet.Tables["Мониторы"].DefaultView;
            //invoiceProjectorView.ItemsSource = invoiceSoftwareAndEquipmentDataSet.Tables["Проекторы"].DefaultView;
            //invoiceBoardView.ItemsSource = invoiceSoftwareAndEquipmentDataSet.Tables["Интерактивные доски"].DefaultView;
            //invoiceScreenView.ItemsSource = invoiceSoftwareAndEquipmentDataSet.Tables["Экраны для проекторов"].DefaultView;
            //invoicePrinterScannerView.ItemsSource = invoiceSoftwareAndEquipmentDataSet.Tables["Принтеры и сканеры"].DefaultView;
            //invoiceNetworkSwitchView.ItemsSource = invoiceSoftwareAndEquipmentDataSet.Tables["Сетевое оборудование"].DefaultView;
            //invoiceOtherEquipmentView.ItemsSource = invoiceSoftwareAndEquipmentDataSet.Tables["Другое оборудование"].DefaultView;
            //invoiceSoftwareView.ItemsSource = invoiceSoftwareAndEquipmentDataSet.Tables["Программное обеспечение"].DefaultView;
            //invoiceOSView.ItemsSource = invoiceSoftwareAndEquipmentDataSet.Tables["Операционные системы"].DefaultView;
        }
    }
}