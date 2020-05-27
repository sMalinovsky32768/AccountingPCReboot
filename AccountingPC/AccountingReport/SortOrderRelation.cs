using System;
using System.Collections.ObjectModel;

namespace AccountingPC.AccountingReport
{
    internal static class SortOrderRelation
    {
        public static ObservableCollection<OrderName> OrderNames { get; private set; } = new ObservableCollection<OrderName>()
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
            //new OrderName(){
            //    Order=SortOrder.Default,
            //    Name="По умолчанию",
            //},
        };

        public static string GetOrderName(SortOrder order)
        {
            foreach (OrderName orderName in OrderNames)
            {
                if (orderName.Order == order)
                {
                    return orderName.Name;
                }
            }
            return String.Empty;
        }
    }
}
