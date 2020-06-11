namespace AccountingPC.AccountingReport
{
    internal enum SortOrder : byte
    {
        Asc,
        Desc,
    }

    internal class OrderName
    {
        public SortOrder Order { get; set; }
        public string Name { get; set; }
    }

    internal class SortingParam
    {
        private ReportColumnName columnName;
        private OrderName orderName = OrderNameCollection.GetOrderName(SortOrder.Asc);

        public ReportColumnName ColumnName
        {
            get => columnName;
            set => columnName = value;
        }
        public OrderName OrderName
        {
            get => orderName;
            set => orderName = value;
        }
    }
}
