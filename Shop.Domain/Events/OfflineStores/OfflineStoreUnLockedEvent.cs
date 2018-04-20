using ENode.Eventing;
using System;

namespace Shop.Domain.Events.OfflineStores
{
    [Serializable]
    public class OfflineStoreUnLockedEvent:DomainEvent<Guid>
    {
        public OfflineStoreUnLockedEvent() { }
    }
}
