using ENode.Eventing;
using System;

namespace Shop.Domain.Events.Admins
{
    /// <summary>
    /// 更新密码事件
    /// </summary>
    [Serializable]
    public class AdminPasswordUpdatedEvent : DomainEvent<Guid>
    {
        public string Password { get; private set; }

        public AdminPasswordUpdatedEvent() { }
        public AdminPasswordUpdatedEvent(string password)
        {
            Password = password;
        }
    }
}
