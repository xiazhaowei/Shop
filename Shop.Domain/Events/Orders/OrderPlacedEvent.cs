using Shop.Common;
using Shop.Domain.Models.Orders;
using System;

namespace Shop.Domain.Events.Orders
{
    [Serializable]
    public class OrderPlacedEvent : OrderEvent
    {
        public Guid UserId { get; private set; }
        public ExpressAddressInfo ExpressAddressInfo { get; private set; }
        public DateTime ReservationExpirationDate { get; private set; }

        public OrderPlacedEvent() { }
        public OrderPlacedEvent(Guid userId,
            ExpressAddressInfo expressAddressInfo,
            OrderTotal orderTotal, 
            DateTime reservationExpirationDate):base(orderTotal)
        {
            UserId = userId;
            ExpressAddressInfo = expressAddressInfo;
            ReservationExpirationDate = reservationExpirationDate;
        }
    }
}
