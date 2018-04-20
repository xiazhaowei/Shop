using ENode.Domain;
using Shop.Domain.Events.Notifications;
using System;

namespace Shop.Domain.Models.Notifications
{
    /// <summary>
    /// 通知聚合跟
    /// </summary>
    public class Notification:AggregateRoot<Guid>
    {
        private NotificationInfo _info;

        public Notification(Guid id, NotificationInfo info):base(id)
        {
            ApplyEvent(new NotificationCreatedEvent(info));
        }

        /// <summary>
        /// 设置通知已经推送
        /// </summary>
        public void SetSmsed()
        {
            ApplyEvent(new NotificationSmsedEvent());
        }
       
        /// <summary>
        /// 设置已读
        /// </summary>
        public void SetReaded()
        {
            ApplyEvent(new NotificationReadedEvent());
        }

        public void Delete()
        {
            ApplyEvent(new NotificationDeletedEvent());
        }

        #region Handle
        private void Handle(NotificationCreatedEvent evnt)
        {
            _info = evnt.Info;
        }
        private void Handle(NotificationSmsedEvent evnt)
        {
            _info.IsSmsed = true;
            _info.IsMessaged = true;
        }
        
        private void Handle(NotificationReadedEvent evnt)
        {
            _info.IsRead = true;
        }
        private void Handle(NotificationDeletedEvent evnt)
        {
            _info = null;
        }
        #endregion


    }
}
