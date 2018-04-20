using ENode.Eventing;
using System;

namespace Shop.Domain.Events.Users
{
    /// <summary>
    /// 更新地区事件
    /// </summary>
    [Serializable]
    public class UserRegionUpdatedEvent : DomainEvent<Guid>
    {
        public string Region { get; private set; }

        public UserRegionUpdatedEvent() { }
        public UserRegionUpdatedEvent(string region)
        {
            Region = region;
        }
    }
}
