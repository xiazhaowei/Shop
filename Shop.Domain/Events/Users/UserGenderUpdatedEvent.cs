using ENode.Eventing;
using System;

namespace Shop.Domain.Events.Users
{
    /// <summary>
    /// 更新性别事件
    /// </summary>
    [Serializable]
    public class UserGenderUpdatedEvent : DomainEvent<Guid>
    {
        public string Gender { get; private set; }

        public UserGenderUpdatedEvent() { }
        public UserGenderUpdatedEvent(string gender)
        {
            Gender = gender;
        }
    }
}
