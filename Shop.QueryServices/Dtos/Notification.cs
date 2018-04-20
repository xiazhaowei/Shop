using Shop.Common.Enums;
using System;

namespace Shop.QueryServices.Dtos
{
    public class Notification
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Mobile { get; set; }
        public string WeixinId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public NotificationType Type { get; set; }
        public Guid AboutId { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Remark { get; set; }
        public bool IsSmsed { get; set; }
        public bool IsMessaged { get; set; }
        public bool IsRead { get; set; }
        public string AboutObjectStream { get; set; }
    }
}
