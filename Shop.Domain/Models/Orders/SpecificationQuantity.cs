using System;

namespace Shop.Domain.Models.Orders
{
    /// <summary>
    /// 规格商品数量
    /// </summary>
    [Serializable]
    public  class SpecificationQuantity
    {
        public Specification Specification { get; private set; }
        public int Quantity { get; private set; }

        public SpecificationQuantity() { }
        public SpecificationQuantity(Specification specification, int quantity)
        {
            Specification = specification;
            Quantity = quantity;
        }
    }
}
