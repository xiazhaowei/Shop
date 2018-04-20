using Shop.Common.Enums;
using System;

namespace Shop.Domain.Models.Notifications
{
    public class NotificationInfo
    {
        public Guid UserId { get; set; }
        public string Mobile { get; set; }
        public string WeixinId { get; set; }
        public string Title { get;private set; }
        public string Body { get;  set; }
        public NotificationType Type { get; private set; }
        public Guid AboutId { get;private set; }
        public string Remark { get;private set; }
        public bool IsSmsed { get; set; }
        public bool IsMessaged { get; set; }
        public bool IsRead { get; set; }
        public string AboutObjectStream { get; set; }

        public NotificationInfo(
            Guid userId,
            string mobile,
            string weixinId,
            string title,
            string body,
            NotificationType type,
            Guid aboutId,
            string remark,
            bool isSmsed,
            bool isMessaged,
            bool isRead,
            string aboutObjectStream)
        {
            UserId = userId;
            Mobile = mobile;
            WeixinId = weixinId;
            Title = title;
            Body = body;
            Type = type;
            AboutId = aboutId;
            Remark = remark;
            IsSmsed = isSmsed;
            IsMessaged = isMessaged;
            IsRead = isRead;
            AboutObjectStream = aboutObjectStream;
        }
    }
}
