using ENode.Eventing;
using System;

namespace Shop.Domain.Events.Users
{
    public class UserRoleToVipPasserEvent:DomainEvent<Guid>
    {
        public UserRoleToVipPasserEvent() { }
    }
}
