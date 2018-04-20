using ENode.Eventing;
using Shop.Domain.Models.OfflineStores;
using System;

namespace Shop.Domain.Events.OfflineStores
{
    [Serializable]
    public class OfflineStoreCreatedEvent:DomainEvent<Guid>
    {
        public Guid UserId { get; set; }
        public OfflineStoreInfo Info { get; set; }

        public OfflineStoreCreatedEvent() { }
        public OfflineStoreCreatedEvent(Guid userId, OfflineStoreInfo info)
        {
            UserId = userId;
            Info = info;
        }
    }
}
