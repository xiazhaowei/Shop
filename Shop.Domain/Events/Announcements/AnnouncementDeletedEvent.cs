using ENode.Eventing;
using System;

namespace Shop.Domain.Events.Announcements
{
    [Serializable]
    public class AnnouncementDeletedEvent:DomainEvent<Guid>
    {
        public AnnouncementDeletedEvent() { }
    }
}
