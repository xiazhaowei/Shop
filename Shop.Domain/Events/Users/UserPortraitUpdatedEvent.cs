using ENode.Eventing;
using System;

namespace Shop.Domain.Events.Users
{
    /// <summary>
    /// 更新头像事件
    /// </summary>
    [Serializable]
    public class UserPortraitUpdatedEvent : DomainEvent<Guid>
    {
        public string Portrait { get; private set; }

        public UserPortraitUpdatedEvent() { }
        public UserPortraitUpdatedEvent(string portrait)
        {
            Portrait = portrait;
        }
    }
}
