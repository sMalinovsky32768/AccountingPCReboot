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

        //public static bool operator ==(OrderName value1, OrderName value2) => (value1.Order == value2.Order) && (value1.Name == value2.Name);

        //public static bool operator !=(OrderName value1, OrderName value2) => (value1.Order != value2.Order) || (value1.Name != value2.Name);

        //public override bool Equals(object obj)
        //{
        //    return this == (OrderName)obj;
        //}

        //public override int GetHashCode()
        //{
        //    return base.GetHashCode();
        //}
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

        //public static bool operator ==(SortingParam value1, SortingParam value2) => (value1.Order == value2.Order) && (value1.Column == value2.Column);

        //public static bool operator !=(SortingParam value1, SortingParam value2) => (value1.Order != value2.Order) || (value1.Column != value2.Column);

        //public override bool Equals(object obj)
        //{
        //    return this == (SortingParam)obj;
        //}

        //public override int GetHashCode()
        //{
        //    return base.GetHashCode();
        //}
    }
}
