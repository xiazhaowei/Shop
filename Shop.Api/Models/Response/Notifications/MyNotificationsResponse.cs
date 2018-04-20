using Shop.Common.Enums;
using System;
using System.Collections.Generic;

namespace Shop.Api.Models.Response.Notifications
{
    public class MyNotificationsResponse:BaseApiResponse
    {
        public IList<Notification> Notifications { get; set; }
    }

    public class Notification
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Mobile { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public NotificationType Type { get; set; }
        public Guid AboutId { get; set; }
        public string Remark { get; set; }
        public string CreatedOn { get; set; }
        public bool IsRead { get; set; }
    }
}