using ENode.Eventing;
using System;

namespace Shop.Domain.Events.Users
{
    [Serializable]
    public class UserUnLockedEvent : DomainEvent<Guid>
    {
        public UserUnLockedEvent() { }
    }
}
