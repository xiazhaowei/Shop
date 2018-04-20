using ENode.Eventing;
using System;

namespace Shop.Domain.Events.Users
{
    /// <summary>
    /// 获取子的商家销售额现金分成
    /// </summary>
    [Serializable]
    public class UserGetChildStoreSaleCashEvent : DomainEvent<Guid>
    {
        public Guid ParentId { get; private set; }
        public decimal Amount { get; private set; }

        public UserGetChildStoreSaleCashEvent() { }
        public UserGetChildStoreSaleCashEvent(Guid parentId,decimal amount)
        {
            ParentId = parentId;
            Amount = amount;
        }
    }
}
