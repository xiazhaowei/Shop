using ENode.Eventing;
using System;

namespace Shop.Domain.Events.Announcements
{
    [Serializable]
    public class AnnouncementCreatedEvent:DomainEvent<Guid>
    {
        public string Title { get; private set; }
        public string Body { get; private set; }

        public AnnouncementCreatedEvent() { }
        public AnnouncementCreatedEvent(string title,string body)
        {
            Title = title;
            Body = body;
        }
    }
}
