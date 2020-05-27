using System.Collections.Generic;

namespace AccountingPC.AccountingReport
{
    internal class ReportRelation
    {
        public string Function { get; set; }
        public string TableName { get; set; }
        public List<ReportColumn> Columns { get; set; }
    }
}
