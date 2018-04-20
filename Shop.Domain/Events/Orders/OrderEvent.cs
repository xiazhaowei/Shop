using ENode.Eventing;
using Shop.Domain.Models.Orders;
using System;

namespace Shop.Domain.Events.Orders
{
    [Serializable]
    public class OrderEvent:DomainEvent<Guid>
    {
        public OrderTotal OrderTotal { get; private set; }

        public OrderEvent() { }
        public OrderEvent(OrderTotal orderTotal) {
            OrderTotal = orderTotal;
        }
    }
}
