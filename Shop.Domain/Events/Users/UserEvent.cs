using ENode.Eventing;
using System;
using Shop.Domain.Models.Users;

namespace Shop.Domain.Events.Users
{
    /// <summary>
    /// 用户事件
    /// </summary>
    [Serializable]
    public abstract class UserEvent : DomainEvent<Guid>
    {
        public UserInfo Info { get; private set; }
        

        public UserEvent() { }
        public UserEvent(UserInfo info)
        {
            Info = info;
        }
    }
}
