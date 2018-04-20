using Shop.QueryServices.Dtos;
using System;
using System.Collections.Generic;

namespace Shop.QueryServices
{
    public interface INotificationQueryService
    {
        Notification Find(Guid id);
        IEnumerable<Notification> Notifications();
        IEnumerable<Notification> UserNotifications(Guid userId);
    }
}
