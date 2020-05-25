using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingPC.AccountingReport
{
    internal class ReportRelation
    {
        public string Function { get; set; }
        public string TableName { get; set; }
        public List<ReportColumn> Columns { get; set; }
    }
}
