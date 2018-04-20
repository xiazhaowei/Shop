using ENode.Eventing;
using System;

namespace Shop.Domain.Events.Grantees
{
    [Serializable]
    public class GranteeVerifyedEvent:DomainEvent<Guid>
    {
        public GranteeVerifyedEvent() { }
    }
}
