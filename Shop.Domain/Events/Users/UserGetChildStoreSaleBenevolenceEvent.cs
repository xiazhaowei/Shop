using ENode.Eventing;
using System;

namespace Shop.Domain.Events.Users
{
    /// <summary>
    /// 获取子的商家销售额善心分成
    /// </summary>
    [Serializable]
    public class UserGetChildStoreSaleBenevolenceEvent : DomainEvent<Guid>
    {
        public Guid ParentId { get; private set; }
        public decimal Amount { get; private set; }

        public UserGetChildStoreSaleBenevolenceEvent() { }
        public UserGetChildStoreSaleBenevolenceEvent(Guid parentId,decimal amount)
        {
            ParentId = parentId;
            Amount = amount;
        }
    }
}
