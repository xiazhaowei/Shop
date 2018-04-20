using ENode.Eventing;
using System;

namespace Shop.Domain.Events.Stores
{
    [Serializable]
    public class StoreUnLockedEvent:DomainEvent<Guid>
    {
        public StoreUnLockedEvent() { }
    }
}
