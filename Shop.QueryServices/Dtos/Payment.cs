using System;

namespace Shop.QueryServices.Dtos
{
    public class Payment
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public int State { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal ShopCashAmount { get; set; }
        public string Description { get; set; }
    }
}
