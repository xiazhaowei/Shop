using ENode.Eventing;
using System;

namespace Shop.Domain.Events.Users
{
    public class UserInfoUpdatedEvent:DomainEvent<Guid>
    {
        public string NickName { get; set; }
        public string Region { get; set; }
        public string Portrait { get; set; }

        public UserInfoUpdatedEvent() { }
        public UserInfoUpdatedEvent(string nickName,string region,string portrait)
        {
            NickName = nickName;
            Region = region;
            Portrait = portrait;
        }
    }
}
