using ENode.Commanding;
using Shop.Common.Enums;
using System;

namespace Shop.Commands.Notifications
{
    public class CreateNotificationCommand:Command<Guid>
    {
        public Guid UserId { get; private set; }
        public string Mobile { get; private set; }
        public string WeixinId { get; private set; }
        public string Title { get; private set; }
        public string Body { get; private set; }
        public NotificationType Type { get; private set; }
        public Guid AboutId { get; private set; }
        public string Remark { get; private set; }
        public bool IsSmsed { get;private set; }
        public bool IsMessaged { get;private set; }
        public bool IsRead { get;private set; }

        public CreateNotificationCommand() { }

        public CreateNotificationCommand(
            Guid id,
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
            bool isRead):base(id)
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
        }
    }
}
