using GemBox.Spreadsheet;
using System.Windows;

namespace AccountingPC.AccountingReport
{
    /// <summary>
    /// Interaction logic for PreviewReportWindow.xaml
    /// </summary>
    public partial class PreviewReportWindow : Window
    {
        private ExcelFile workbook;
        public PreviewReportWindow(ExcelFile file)
        {
            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
            SpreadsheetInfo.FreeLimitReached += (sender, e) => e.FreeLimitReachedAction = FreeLimitReachedAction.ContinueAsTrial;

            InitializeComponent();

            workbook = file;

            var xpsDocument = workbook.ConvertToXpsDocument(SaveOptions.XpsDefault);
            this.DocViewer.Tag = xpsDocument;

            this.DocViewer.Document = xpsDocument.GetFixedDocumentSequence();
        }
    }
}
