using ENode.Eventing;
using System;

namespace Shop.Domain.Events.OfflineStores
{
    [Serializable]
    public class OfflineStoreLockedEvent:DomainEvent<Guid>
    {
        public OfflineStoreLockedEvent() { }
    }
}
