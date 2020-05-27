namespace AccountingPC.AccountingReport
{
    internal enum SortOrder
    {
        Asc,
        Desc,
        //Default,
    }

    internal class OrderName
    {
        public SortOrder Order { get; set; }
        public string Name { get; set; }
    }

    /// <summary>
    /// Параметр сортировки. Если IsAscending = true, сортировка выполняется по возрастанию, иначе по убыванию
    /// </summary>
    internal class SortingParam
    {
        private ReportColumn column;
        private SortOrder order;
        private ColumnRelation columnRelation;
        private OrderName orderName;

        public ReportColumn Column
        {
            get => column;
            set
            {
                column = value;
                columnRelation = ReportColumnRelation.GetColumnName(value);
            }
        }
        public SortOrder Order
        {
            get => order;
            set
            {
                order = value;
                orderName = new OrderName() { Order = value, Name = SortOrderRelation.GetOrderName(value) };
            }
        }

        public ColumnRelation ColumnRelation
        {
            get => columnRelation;
            set
            {
                columnRelation = value;
                column = value.Column;
            }
        }
        public OrderName OrderName
        {
            get => orderName;
            set
            {
                orderName = value;
                order = value.Order;
            }
        }
    }
}
