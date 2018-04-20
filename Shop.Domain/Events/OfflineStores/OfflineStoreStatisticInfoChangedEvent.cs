using ENode.Eventing;
using Shop.Domain.Models.OfflineStores;
using System;

namespace Shop.Domain.Events.OfflineStores
{
    [Serializable]
    public class OfflineStoreStatisticInfoChangedEvent:DomainEvent<Guid>
    {
        public StatisticInfo Info { get;private set; }

        public OfflineStoreStatisticInfoChangedEvent() { }
        public OfflineStoreStatisticInfoChangedEvent(StatisticInfo info)
        {
            Info = info;
        }
    }
}
