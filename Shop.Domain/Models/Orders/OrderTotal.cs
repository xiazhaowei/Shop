using System;

namespace Shop.Domain.Models.Orders
{
    /// <summary>
    /// 订单总额 包含多个商品线
    /// </summary>
    [Serializable]
    public class OrderTotal
    {
        public OrderLine[] Lines { get; private set; }
        public decimal Total { get; private set; }
        public decimal StoreTotal { get; private set; }

        public OrderTotal() { }
        public OrderTotal(OrderLine[] lines, decimal total,decimal storeTotal)
        {
            Lines = lines;
            Total = total;
            StoreTotal = storeTotal;
        }
    }
}
