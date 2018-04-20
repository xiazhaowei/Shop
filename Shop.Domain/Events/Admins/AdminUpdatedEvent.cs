using ENode.Eventing;
using Shop.Domain.Models.Admins;
using System;

namespace Shop.Domain.Events.Admins
{
    public class AdminUpdatedEvent:DomainEvent<Guid>
    {
        public AdminEditableInfo Info { get; set; }

        public AdminUpdatedEvent() { }
        public AdminUpdatedEvent(AdminEditableInfo info)
        {
            Info = info;
        }
    }
}
