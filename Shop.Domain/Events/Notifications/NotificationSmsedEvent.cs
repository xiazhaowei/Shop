using ENode.Eventing;
using System;

namespace Shop.Domain.Events.Notifications
{
    public class NotificationSmsedEvent:DomainEvent<Guid>
    {
        public NotificationSmsedEvent() { }
    }
}
