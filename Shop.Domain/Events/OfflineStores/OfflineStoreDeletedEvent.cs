using ENode.Eventing;
using System;

namespace Shop.Domain.Events.OfflineStores
{
    [Serializable]
    public class OfflineStoreDeletedEvent:DomainEvent<Guid>
    {
        public OfflineStoreDeletedEvent() { }
    }
}
