using ENode.Eventing;
using System;

namespace Shop.Domain.Events.Stores
{
    [Serializable]
    public class StoreLockedEvent:DomainEvent<Guid>
    {
        public StoreLockedEvent() { }
    }
}
