using ENode.Eventing;
using System;

namespace Shop.Domain.Events.Partners
{
    [Serializable]
    public class PartnerDeletedEvent:DomainEvent<Guid>
    {
        public PartnerDeletedEvent() { }
    }
}
