using ENode.Eventing;
using System;

namespace Shop.Domain.Events.Users
{
    [Serializable]
    public class UserBindedWeixinEvent:DomainEvent<Guid>
    {
        public string WeixinId { get;private set; }
        public string UnionId { get;private set; }

        public UserBindedWeixinEvent() { }
        public UserBindedWeixinEvent(string weixinId,string unionId)
        {
            WeixinId = weixinId;
            UnionId = unionId;
        }
    }
}
