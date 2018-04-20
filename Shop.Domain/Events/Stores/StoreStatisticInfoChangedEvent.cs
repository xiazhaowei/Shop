using ENode.Eventing;
using Shop.Domain.Models.Stores;
using System;

namespace Shop.Domain.Events.Stores
{
    [Serializable]
    public class StoreStatisticInfoChangedEvent:DomainEvent<Guid>
    {
        public StoreStatisticInfo StatisticInfo { get; private set; }

        public StoreStatisticInfoChangedEvent() { }
        public StoreStatisticInfoChangedEvent(StoreStatisticInfo statisticInfo)
        {
            StatisticInfo = statisticInfo;
        }
    }
}
