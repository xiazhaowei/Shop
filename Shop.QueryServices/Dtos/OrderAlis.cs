using System;

namespace Shop.QueryServices.Dtos
{
    public class OrderAlis
    {
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public int Status { get; set; }
        public decimal Total { get; set; }
        public decimal ShopCash { get; set; }
        public decimal StoreTotal { get; set; }
        public DateTime ReservationExpirationDate { get; set; }
    }
    
}
