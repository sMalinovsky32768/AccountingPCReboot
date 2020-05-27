using System.Collections.Generic;

namespace AccountingPC.AccountingReport
{
    internal class ReportRelation
    {
        public string Function { get; set; }
        public string TableName { get; set; }
        public List<ReportColumn> Columns { get; set; }

        //public static bool operator ==(ReportRelation value1, ReportRelation value2) => value1.Function == value2.Function;

        //public static bool operator !=(ReportRelation value1, ReportRelation value2) => value1.Function != value2.Function;

        //public override bool Equals(object obj)
        //{
        //    return this == (ReportRelation)obj;
        //}

        //public override int GetHashCode()
        //{
        //    return base.GetHashCode();
        //}
    }
}
