using ENode.Eventing;
using Shop.Domain.Models.Admins;
using System;

namespace Shop.Domain.Events.Admins
{
    public class AdminCreatedEvent:DomainEvent<Guid>
    {
        public AdminInfo Info { get; set; }

        public AdminCreatedEvent() { }
        public AdminCreatedEvent(AdminInfo info)
        {
            Info = info;
        }
    }
}
