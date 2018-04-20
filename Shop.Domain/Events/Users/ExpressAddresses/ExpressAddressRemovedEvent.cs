using ENode.Eventing;
using System;

namespace Shop.Domain.Events.Users.ExpressAddresses
{
    [Serializable]
    public class ExpressAddressRemovedEvent: DomainEvent<Guid>
    {
        public Guid ExpressAddressId { get; private set; }

        public ExpressAddressRemovedEvent() { }
        public ExpressAddressRemovedEvent(Guid expressAddressId)
        {
            ExpressAddressId = expressAddressId;
        }
    }
}
