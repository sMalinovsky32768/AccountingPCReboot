using System;
using System.Windows.Controls;
using System.Data;
using System.Data.SqlClient;

namespace AccountingPC
{
    public partial class AccountingPCWindow
    {
        private void LoadInvoiceList()
        {
            invoiceDataAdapter = new SqlDataAdapter("SELECT * FROM dbo.[GetAllInvoice]()", ConnectionString);
            invoiceDataSet = new DataSet();
            invoiceDataAdapter.Fill(invoiceDataSet);
            invoiceList.ItemsSource = invoiceDataSet.Tables[0].DefaultView;
            invoiceList.DisplayMemberPath = "Number";
        }

        private void UpdateInvoiceData()
        {
            invoiceSoftwareAndEquipmentDataSet = new DataSet();
            invoiceSoftwareAndEquipmentDataSet.Tables.Add("Компьютеры");
            invoiceSoftwareAndEquipmentDataSet.Tables.Add("Ноутбуки и Моноблоки");
            invoiceSoftwareAndEquipmentDataSet.Tables.Add("Мониторы");
            invoiceSoftwareAndEquipmentDataSet.Tables.Add("Проекторы");
            invoiceSoftwareAndEquipmentDataSet.Tables.Add("Интерактивные доски");
            invoiceSoftwareAndEquipmentDataSet.Tables.Add("Экраны для проекторов");
            invoiceSoftwareAndEquipmentDataSet.Tables.Add("Принтеры и сканеры");
            invoiceSoftwareAndEquipmentDataSet.Tables.Add("Сетевое оборудование");
            invoiceSoftwareAndEquipmentDataSet.Tables.Add("Другое оборудование");
            invoiceSoftwareAndEquipmentDataSet.Tables.Add("Программное обеспечение");
            invoiceSoftwareAndEquipmentDataSet.Tables.Add("Операционные системы");

            new SqlDataAdapter($"SELECT * FROM dbo.GetAllPC() Where InvoiceID={InvoiceID}",
                ConnectionString).Fill(invoiceSoftwareAndEquipmentDataSet, "Компьютеры");
            new SqlDataAdapter($"SELECT * FROM dbo.GetAllNotebook() Where InvoiceID={InvoiceID}",
                ConnectionString).Fill(invoiceSoftwareAndEquipmentDataSet, "Ноутбуки и Моноблоки");
            new SqlDataAdapter($"SELECT * FROM dbo.GetAllMonitor() Where InvoiceID={InvoiceID}",
                ConnectionString).Fill(invoiceSoftwareAndEquipmentDataSet, "Мониторы");
            new SqlDataAdapter($"SELECT * FROM dbo.GetAllProjector() Where InvoiceID={InvoiceID}",
                ConnectionString).Fill(invoiceSoftwareAndEquipmentDataSet, "Проекторы");
            new SqlDataAdapter($"SELECT * FROM dbo.GetAllBoard() Where InvoiceID={InvoiceID}",
                ConnectionString).Fill(invoiceSoftwareAndEquipmentDataSet, "Интерактивные доски");
            new SqlDataAdapter($"SELECT * FROM dbo.GetAllScreen() Where InvoiceID={InvoiceID}",
                ConnectionString).Fill(invoiceSoftwareAndEquipmentDataSet, "Экраны для проекторов");
            new SqlDataAdapter($"SELECT * FROM dbo.GetAllPrinterScanner() Where InvoiceID={InvoiceID}",
                ConnectionString).Fill(invoiceSoftwareAndEquipmentDataSet, "Принтеры и сканеры");
            new SqlDataAdapter($"SELECT * FROM dbo.GetAllNetworkSwitch() Where InvoiceID={InvoiceID}",
                ConnectionString).Fill(invoiceSoftwareAndEquipmentDataSet, "Сетевое оборудование");
            new SqlDataAdapter($"SELECT * FROM dbo.GetAllOtherEquipment() Where InvoiceID={InvoiceID}",
                ConnectionString).Fill(invoiceSoftwareAndEquipmentDataSet, "Другое оборудование");
            new SqlDataAdapter($"SELECT * FROM dbo.GetAllSoftware() Where InvoiceID={InvoiceID}",
                ConnectionString).Fill(invoiceSoftwareAndEquipmentDataSet, "Программное обеспечение");
            new SqlDataAdapter($"SELECT * FROM dbo.GetAllOS() Where InvoiceID={InvoiceID}",
                ConnectionString).Fill(invoiceSoftwareAndEquipmentDataSet, "Операционные системы");

            foreach (DataTable table in invoiceSoftwareAndEquipmentDataSet.Tables)
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
            invoiceItemsControl.ItemsSource = invoiceSoftwareAndEquipmentDataSet.Tables;
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