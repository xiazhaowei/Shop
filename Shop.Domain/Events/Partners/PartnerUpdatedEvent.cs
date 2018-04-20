using ENode.Eventing;
using Shop.Domain.Models.Partners;
using System;

namespace Shop.Domain.Events.Partners
{
    [Serializable]
    public class PartnerUpdatedEvent:DomainEvent<Guid>
    {
        public PartnerInfo Info { get; private set; }

        public PartnerUpdatedEvent() { }
        public PartnerUpdatedEvent(PartnerInfo info)
        {
            Info = info;
        }
    }
}
