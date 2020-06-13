namespace AccountingPC.AccountingReport
{
    internal enum SortOrder : byte
    {
        Asc,
        Desc
    }

    internal class OrderName
    {
        public SortOrder Order { get; set; }
        public string Name { get; set; }
    }

    internal class SortingParam
    {
        public ReportColumnName ColumnName { get; set; }

        public OrderName OrderName { get; set; } = OrderNameCollection.GetOrderName(SortOrder.Asc);
    }
}