using ENode.Eventing;
using System;

namespace Shop.Domain.Events.Notifications
{
    [Serializable]
    public class NotificationDeletedEvent:DomainEvent<Guid>
    {
        public NotificationDeletedEvent() { }
    }
}
