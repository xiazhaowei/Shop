using ENode.Eventing;
using System;

namespace Shop.Domain.Events.Announcements
{
    [Serializable]
    public class AnnouncementUpdatedEvent : DomainEvent<Guid>
    {
        public string Title { get; private set; }
        public string Body { get; private set; }

        public AnnouncementUpdatedEvent() { }
        public AnnouncementUpdatedEvent(string title,string body)
        {
            Title = title;
            Body = body;
        }
    }
}
