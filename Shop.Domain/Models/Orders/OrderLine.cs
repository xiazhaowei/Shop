using System;

namespace Shop.Domain.Models.Orders
{
    /// <summary>
    /// 商品线
    /// </summary>
    [Serializable]
    public class OrderLine
    {
        public SpecificationQuantity SpecificationQuantity { get; private set; }
        public decimal LineTotal { get; private set; }
        public decimal StoreLineTotal { get; private set; }

        public OrderLine() { }
        public OrderLine(SpecificationQuantity specificationQuantity, 
            decimal lineTotal,
            decimal storeLineTotal)
        {
            SpecificationQuantity = specificationQuantity;
            LineTotal = lineTotal;
            StoreLineTotal = storeLineTotal;
        }
    }
}
