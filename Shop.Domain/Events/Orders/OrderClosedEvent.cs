using System;

namespace Shop.Domain.Events.Orders
{
    [Serializable]
    public class OrderClosedEvent : OrderEvent
    {
        public OrderClosedEvent() { }
    }
}
