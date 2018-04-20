using ENode.Eventing;
using System;

namespace Shop.Domain.Events.Users
{
    /// <summary>
    /// 更新昵称事件
    /// </summary>
    [Serializable]
    public class UserNickNameUpdatedEvent : DomainEvent<Guid>
    {
        public string NickName { get; private set; }

        public UserNickNameUpdatedEvent() { }
        public UserNickNameUpdatedEvent(string nickName)
        {
            NickName = nickName;
        }
    }
}
