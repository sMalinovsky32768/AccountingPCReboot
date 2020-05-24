using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingPC
{
    internal enum Sorting
    {
        InvN,
        Name,
        Cost,
        Invoice,
        Audience,
    }

    internal enum Grouping
    {

    }

    internal class ReportOptions
    {
        public bool IsInvN { get; set; }
        public bool IsType { get; set; }
        public bool IsName { get; set; }
        public bool IsCost { get; set; }
        public bool IsInvoice { get; set; }
        public bool IsAcquisitionDate { get; set; }
        public bool IsLocation { get; set; }
        public Sorting[] SortingsArray { get; set; }
        private Grouping Grouping { get; set; }
    }
}
