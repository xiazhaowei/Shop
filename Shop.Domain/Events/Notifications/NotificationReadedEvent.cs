using ENode.Eventing;
using System;

namespace Shop.Domain.Events.Notifications
{
    [Serializable]
    public class NotificationReadedEvent:DomainEvent<Guid>
    {
        public NotificationReadedEvent() { }
    }
}
