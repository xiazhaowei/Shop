using ENode.Eventing;
using System;

namespace Shop.Domain.Events.Admins
{
    public class AdminDeletedEvent:DomainEvent<Guid>
    {
        public AdminDeletedEvent() { }
    }
}
