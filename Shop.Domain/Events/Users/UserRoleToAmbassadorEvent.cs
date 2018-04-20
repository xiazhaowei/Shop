using ENode.Eventing;
using System;

namespace Shop.Domain.Events.Users
{
    /// <summary>
    /// 用户角色申请为 传递大使
    /// </summary>
    [Serializable]
    public class UserRoleToAmbassadorEvent : DomainEvent<Guid>
    {
        public bool OnlyUpdateTime { get; private set; }
        public DateTime ExpireTime { get; set; }

        public UserRoleToAmbassadorEvent() { }
        public UserRoleToAmbassadorEvent(bool onlyUpdateTime,DateTime expireTime)
        {
            OnlyUpdateTime = onlyUpdateTime;
            ExpireTime = expireTime;
        }
    }
}
