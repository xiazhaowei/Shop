using ENode.Eventing;
using System;

namespace Shop.Domain.Events.Users
{
    /// <summary>
    /// 更新密码事件
    /// </summary>
    [Serializable]
    public class UserPasswordUpdatedEvent : DomainEvent<Guid>
    {
        public string Password { get; private set; }

        public UserPasswordUpdatedEvent() { }
        public UserPasswordUpdatedEvent(string password)
        {
            Password = password;
        }
    }
}
