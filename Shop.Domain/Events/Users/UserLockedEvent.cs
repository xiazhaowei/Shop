using ENode.Eventing;
using System;

namespace Shop.Domain.Events.Users
{
    [Serializable]
    public class UserLockedEvent : DomainEvent<Guid>
    {
        public UserLockedEvent() { }
    }
}
