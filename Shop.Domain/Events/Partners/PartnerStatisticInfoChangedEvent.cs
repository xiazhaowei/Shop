using ENode.Eventing;
using Shop.Domain.Models.Partners;
using System;

namespace Shop.Domain.Events.Partners
{
    [Serializable]
    public class PartnerStatisticInfoChangedEvent:DomainEvent<Guid>
    {
        public PartnerStatisticInfo StatisticInfo { get; private set; }

        public PartnerStatisticInfoChangedEvent() { }
        public PartnerStatisticInfoChangedEvent(PartnerStatisticInfo statisticInfo)
        {
            StatisticInfo = statisticInfo;
        }
    }
}
