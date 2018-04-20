using ENode.Eventing;
using System;

namespace Shop.Domain.Events.Grantees
{
    [Serializable]
    public class GranteeUnVerifyedEvent : DomainEvent<Guid>
    {
        public GranteeUnVerifyedEvent() { }
    }
}
