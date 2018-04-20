using Shop.Domain.Models.Orders;
using System;

namespace Shop.Domain.Events.Orders
{
    [Serializable]
    public class OrderExpiredEvent: OrderEvent
    {
        public OrderExpiredEvent() { }
        public OrderExpiredEvent(OrderTotal orderTotal):base(orderTotal) { }
    }
}
