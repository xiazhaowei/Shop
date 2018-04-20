using ENode.Eventing;
using System;

namespace Shop.Domain.Events.Stores.StoreOrders
{
    [Serializable]
    public class StoreOrderDeletedEvent:DomainEvent<Guid>
    {
        public StoreOrderDeletedEvent() { }
    }
}
