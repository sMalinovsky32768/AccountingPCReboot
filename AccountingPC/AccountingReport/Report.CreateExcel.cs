using Microsoft.Office.Interop.Excel;
using System;
using System.Reflection;
using System.Windows;
using Application = Microsoft.Office.Interop.Excel.Application;

namespace AccountingPC.AccountingReport
{
    internal partial class Report
    {
        public void CreateReportExcel(string path = null)
        {
            System.Data.DataSet dataSet = GetDataSet();

            Application application;
            _Workbook workbook;
            _Worksheet activeWorksheet;
            application = new Application
            {
                Visible = false
            };

            workbook = application.Workbooks.Add(Missing.Value);
            try
            {
                activeWorksheet = (_Worksheet)workbook.ActiveSheet;
                for (int i = 0; i < dataSet.Tables.Count; i++)
                {
                    System.Data.DataTable dataTable = dataSet.Tables[i];
                    int columnCount = dataTable.Columns.Count;
                    int rowCount = dataTable.Rows.Count;

                    _Worksheet worksheet = workbook.Worksheets.Add();
                    worksheet.Name = dataTable.TableName;

                    // Установка заголовков
                    for (int j = 0; j < columnCount; j++) 
                    {
                        worksheet.Cells[1, j + 2] = dataTable.Columns[j].ColumnName;
                    }
                    // Заполнение ячеек
                    for (int j = 0; j < rowCount; j++) 
                    {
                        System.Data.DataRow row = dataTable.Rows[j];
                        for (int k = 0; k < columnCount; k++)
                        {
                            worksheet.Cells[j + 2, k + 2] = row[k];
                        }
                    }

                    worksheet.Cells[rowCount + 2, 1] = "Итого: ";

                    if (Options.IsShowUnitOfMeasurement)
                    {
                        dynamic cost = worksheet.Cells.Find("Цена");
                        if (cost != null)
                        {
                            Range target = worksheet.Range[
                                worksheet.Cells[((Range)cost).Row + 1, ((Range)cost).Column],
                                worksheet.Cells[rowCount + 1, ((Range)cost).Column]];
                            target.NumberFormat = "# ##0 \u20bd";
                            dynamic totalCost = worksheet.Cells.Find("Общая стоимость");
                            if (totalCost != null)
                            {
                                target = worksheet.Range[
                                    worksheet.Cells[((Range)totalCost).Row + 1, ((Range)totalCost).Column],
                                    worksheet.Cells[rowCount + 1, ((Range)totalCost).Column]];
                                target.NumberFormat = "# ##0 \u20bd";
                                dynamic count = worksheet.Cells.Find("Количество");
                                if (count != null)
                                {
                                    Range newRange = worksheet.Cells[((Range)cost).Row + 1, ((Range)cost).Column];
                                    Range newRange1 = worksheet.Cells[((Range)count).Row + 1, ((Range)count).Column];
                                    string formula = $"={newRange.Address[false, false]}*{newRange1.Address[false, false]}";
                                    target.Formula = formula;
                                }
                            }
                        }
                        else
                        {
                            dynamic totalCost = worksheet.Cells.Find("Общая стоимость");
                            if (totalCost != null)
                            {
                                Range target = worksheet.Range[
                                    worksheet.Cells[((Range)totalCost).Row + 1, ((Range)totalCost).Column],
                                    worksheet.Cells[rowCount + 1, ((Range)totalCost).Column]];
                                target.NumberFormat = "# ##0 \u20bd";
                            }
                        }

                        dynamic inventory = worksheet.Cells.Find("Инвентарный номер");
                        if (inventory != null)
                        {
                            Range target = worksheet.Range[
                                worksheet.Cells[((Range)inventory).Row + 1, ((Range)inventory).Column],
                                worksheet.Cells[rowCount + 1, ((Range)inventory).Column]];
                            target.NumberFormat = "000000000000000";
                        }

                        dynamic date = worksheet.Cells.Find("Дата приобретения");
                        if (date != null)
                        {
                            Range target = worksheet.Range[
                                worksheet.Cells[((Range)date).Row + 1, ((Range)date).Column],
                                worksheet.Cells[rowCount + 1, ((Range)date).Column]];
                            target.NumberFormat = "dd-MM-yyyy";
                        }

                        dynamic diagonal = worksheet.Cells.Find("Диагональ") ?? (worksheet.Cells.Find("Диагональ экрана") ?? worksheet.Cells.Find("Максимальная диагональ"));
                        if (diagonal != null)
                        {
                            Range target = worksheet.Range[
                                worksheet.Cells[((Range)diagonal).Row + 1, ((Range)diagonal).Column],
                                worksheet.Cells[rowCount + 1, ((Range)diagonal).Column]];
                            target.NumberFormat = "##.#__\u2033";
                        }

                        dynamic baseFreq = worksheet.Cells.Find("Базовая частота");
                        if (baseFreq != null)
                        {
                            Range target = worksheet.Range[
                                worksheet.Cells[((Range)baseFreq).Row + 1, ((Range)baseFreq).Column],
                                worksheet.Cells[rowCount + 1, ((Range)baseFreq).Column]];
                            target.NumberFormat = "# ##0__МГц";
                        }

                        dynamic maxFreq = worksheet.Cells.Find("Максимальная частота");
                        if (maxFreq != null)
                        {
                            Range target = worksheet.Range[
                                worksheet.Cells[((Range)maxFreq).Row + 1, ((Range)maxFreq).Column],
                                worksheet.Cells[rowCount + 1, ((Range)maxFreq).Column]];
                            target.NumberFormat = "# ##0__МГц";
                        }

                        dynamic refreshFreq = worksheet.Cells.Find("Частота обновления");
                        if (refreshFreq != null)
                        {
                            Range target = worksheet.Range[
                                worksheet.Cells[((Range)refreshFreq).Row + 1, ((Range)refreshFreq).Column],
                                worksheet.Cells[rowCount + 1, ((Range)refreshFreq).Column]];
                            target.NumberFormat = "# ##0__Гц";
                        }

                        dynamic ram = worksheet.Cells.Find("ОЗУ");
                        if (ram != null)
                        {
                            Range target = worksheet.Range[
                                worksheet.Cells[((Range)ram).Row + 1, ((Range)ram).Column],
                                worksheet.Cells[rowCount + 1, ((Range)ram).Column]];
                            target.NumberFormat = "# ##0__Гб";
                        }

                        dynamic vram = worksheet.Cells.Find("Видеопамять");
                        if (vram != null)
                        {
                            Range target = worksheet.Range[
                                worksheet.Cells[((Range)vram).Row + 1, ((Range)vram).Column],
                                worksheet.Cells[rowCount + 1, ((Range)vram).Column]];
                            target.NumberFormat = "# ##0__Гб";
                        }

                        dynamic hdd = worksheet.Cells.Find("Объем HDD");
                        if (hdd != null)
                        {
                            Range target = worksheet.Range[
                                worksheet.Cells[((Range)hdd).Row + 1, ((Range)hdd).Column],
                                worksheet.Cells[rowCount + 1, ((Range)hdd).Column]];
                            target.NumberFormat = "# ##0__Гб";
                        }

                        dynamic ssd = worksheet.Cells.Find("Объем SSD");
                        if (ssd != null)
                        {
                            Range target = worksheet.Range[
                                worksheet.Cells[((Range)ssd).Row + 1, ((Range)ssd).Column],
                                worksheet.Cells[rowCount + 1, ((Range)ssd).Column]];
                            target.NumberFormat = "# ##0__Гб";
                        }
                    }

                    if (Options.IsCountMaxMinAverageSum)
                    {
                        dynamic cost = worksheet.Cells.Find("Цена");
                        if (cost != null)
                        {
                            Range begin = worksheet.Cells[((Range)cost).Row + 1, ((Range)cost).Column];
                            Range end = worksheet.Cells[rowCount + 1, ((Range)cost).Column];
                            ((Range)worksheet.Cells[rowCount + 2, ((Range)cost).Column]).Formula = $"=SUM({begin.Address[false, false]}:{end.Address[false, false]})";

                            worksheet.Cells[rowCount + 3, ((Range)cost).Column - 1] = "Минимальная цена";
                            ((Range)worksheet.Cells[rowCount + 3, ((Range)cost).Column]).Formula = $"=MIN({begin.Address[false, false]}:{end.Address[false, false]})";

                            worksheet.Cells[rowCount + 4, ((Range)cost).Column - 1] = "Средняя цена";
                            ((Range)worksheet.Cells[rowCount + 4, ((Range)cost).Column]).Formula = $"=AVERAGE({begin.Address[false, false]}:{end.Address[false, false]})";

                            worksheet.Cells[rowCount + 5, ((Range)cost).Column - 1] = "Максимальная цена";
                            ((Range)worksheet.Cells[rowCount + 5, ((Range)cost).Column]).Formula = $"=MAX({begin.Address[false, false]}:{end.Address[false, false]})";

                            if (Options.IsShowUnitOfMeasurement)
                            {
                                for (int j = 2; j <= 5; j++)
                                {
                                    ((Range)worksheet.Cells[rowCount + j, ((Range)cost).Column]).NumberFormat = "# ##0 \u20bd";
                                }
                            }
                        }

                        dynamic totalCost = worksheet.Cells.Find("Общая стоимость");
                        if (totalCost != null)
                        {
                            Range begin = worksheet.Cells[((Range)totalCost).Row + 1, ((Range)totalCost).Column];
                            Range end = worksheet.Cells[rowCount + 1, ((Range)totalCost).Column];
                            ((Range)worksheet.Cells[rowCount + 2, ((Range)totalCost).Column]).Formula = $"=SUM({begin.Address[false, false]}:{end.Address[false, false]})";

                            if (Options.IsShowUnitOfMeasurement)
                            {
                                ((Range)worksheet.Cells[rowCount + 2, ((Range)totalCost).Column]).NumberFormat = "# ##0 \u20bd";
                            }
                        }

                        dynamic count = worksheet.Cells.Find("Количество");
                        if (count != null)
                        {
                            Range begin = worksheet.Cells[((Range)count).Row + 1, ((Range)count).Column];
                            Range end = worksheet.Cells[rowCount + 1, ((Range)count).Column];
                            ((Range)worksheet.Cells[rowCount + 2, ((Range)count).Column]).Formula = $"=SUM({begin.Address[false, false]}:{end.Address[false, false]})";
                        }

                        dynamic inventory = worksheet.Cells.Find("Инвентарный номер");
                        if (inventory != null)
                        {
                            Range begin = worksheet.Cells[((Range)inventory).Row + 1, ((Range)inventory).Column];
                            Range end = worksheet.Cells[rowCount + 1, ((Range)inventory).Column];
                            ((Range)worksheet.Cells[rowCount + 2, ((Range)inventory).Column]).Formula = $"=COUNT({begin.Address[false, false]}:{end.Address[false, false]})";
                            if (Options.IsShowUnitOfMeasurement)
                            {
                                ((Range)worksheet.Cells[rowCount + 2, ((Range)inventory).Column]).NumberFormat = "0__Устройств";
                            }
                        }
                    }

                    Range range = worksheet.Range[worksheet.Cells[1, 1], worksheet.Cells[rowCount + 2, columnCount + 2]];
                    range.EntireColumn.AutoFit();
                }
                if (workbook.Worksheets.Count > 1)
                {
                    activeWorksheet.Delete();
                }
                if (path==null)
                {
                    application.Visible = true;
                    application.UserControl = true;
                }
                else
                {
                    if (Options.CreateOptions == CreateReportOptions.SaveAsXlsx)
                    {
                        workbook.SaveAs(path);
                    }
                    if (Options.CreateOptions == CreateReportOptions.SaveAsPDF)
                    {
                        workbook.ExportAsFixedFormat(XlFixedFormatType.xlTypePDF, path);
                    }
                    workbook.Close();
                    application.Quit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                workbook.Close();
                application.Quit();
            }
        }
    }
}
