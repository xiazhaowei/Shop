using ENode.Eventing;
using Shop.Common.Enums;
using System;

namespace Shop.Domain.Events.Users
{
    [Serializable]
    public class UserEditedEvent:DomainEvent<Guid>
    {
        public string NickName { get; set; }
        public string Gender { get; set; }
        public UserRole Role { get; set; }

        public UserEditedEvent() { }
        public UserEditedEvent(string nickName,string gender,UserRole role)
        {
            NickName = nickName;
            Gender = gender;
            Role = role;
        }
    }
}
