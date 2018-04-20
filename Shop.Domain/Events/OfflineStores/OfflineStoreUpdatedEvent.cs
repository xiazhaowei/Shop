using ENode.Eventing;
using Shop.Domain.Models.OfflineStores;
using System;

namespace Shop.Domain.Events.OfflineStores
{
    [Serializable]
    public class OfflineStoreUpdatedEvent:DomainEvent<Guid>
    {
        public OfflineStoreInfo Info { get; set; }

        public OfflineStoreUpdatedEvent() { }
        public OfflineStoreUpdatedEvent(OfflineStoreInfo info)
        {
            Info = info;
        }
    }
}
