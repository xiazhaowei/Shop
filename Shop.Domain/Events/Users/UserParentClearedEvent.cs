using ENode.Eventing;
using System;

namespace Shop.Domain.Events.Users
{
    [Serializable]
    public class UserParentClearedEvent:DomainEvent<Guid>
    {
        public UserParentClearedEvent() { }
    }
}
