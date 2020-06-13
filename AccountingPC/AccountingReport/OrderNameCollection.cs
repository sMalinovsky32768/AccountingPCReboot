using System.Collections.ObjectModel;

namespace AccountingPC.AccountingReport
{
    internal static class OrderNameCollection
    {
        public static ObservableCollection<OrderName> Collection { get; } = new ObservableCollection<OrderName>
        {
            new OrderName
            {
                Order = SortOrder.Asc,
                Name = "По возрастанию"
            },
            new OrderName
            {
                Order = SortOrder.Desc,
                Name = "По убыванию"
            }
        };

        public static OrderName GetOrderName(SortOrder order)
        {
            for (var i = 0; i < Collection.Count; i++)
            {
                var orderName = Collection[i];
                if (orderName.Order == order) return orderName;
            }

            return null;
        }
    }
}