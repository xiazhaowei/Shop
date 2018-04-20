using System;

namespace Shop.QueryServices.Dtos
{
    /// <summary>
    /// 订单项
    /// </summary>
    public class OrderLine
    {
        public Guid OrderId { get; set; }
        public Guid SpecificationId { get; set; }
        public string SpecificationName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal LineTotal { get; set; }
        public decimal StoreLineTotal { get; set; }
    }
}
