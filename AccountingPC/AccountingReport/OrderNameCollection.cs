using System.Collections.ObjectModel;

namespace AccountingPC.AccountingReport
{
    internal static class OrderNameCollection
    {
        private static readonly ObservableCollection<OrderName> collection = new ObservableCollection<OrderName>()
        {
            new OrderName()
            {
                Order=SortOrder.Asc,
                Name="По возрастанию",
            },
            new OrderName(){
                Order=SortOrder.Desc,
                Name="По убыванию",
            },
        };

        public static ObservableCollection<OrderName> Collection => collection;
        public static OrderName GetOrderName(SortOrder order)
        {
            for (int i = 0; i < Collection.Count; i++)
            {
                OrderName orderName = Collection[i];
                if (orderName.Order == order)
                {
                    return orderName;
                }
            }

            return null;
        }
    }
}
