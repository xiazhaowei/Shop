using ENode.Eventing;
using Shop.Domain.Models.Notifications;
using System;

namespace Shop.Domain.Events.Notifications
{
    [Serializable]
    public class NotificationCreatedEvent:DomainEvent<Guid>
    {
        public NotificationInfo Info { get; private set; }

        public NotificationCreatedEvent() { }
        public NotificationCreatedEvent(NotificationInfo info)
        {
            Info = info;
        }
    }
}
