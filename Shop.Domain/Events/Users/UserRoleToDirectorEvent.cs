using ENode.Eventing;
using System;

namespace Shop.Domain.Events.Users
{
    public class UserRoleToDirectorEvent:DomainEvent<Guid>
    {
        public UserRoleToDirectorEvent() { }
    }
}
