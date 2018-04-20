using System;
using System.Collections.Generic;

namespace Shop.QueryServices.Dtos
{
    public class Order
    {

        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public int Status { get; set; }
        public decimal Total { get; set; }
        public decimal ShopCash { get; set; }
        public decimal StoreTotal { get; set; }
        public DateTime? ReservationExpirationDate { get; set; }

        public bool IsFreeOfCharge { get; set; }
        public IList<OrderLine> Lines { get; set; }
       
    }
    
}
