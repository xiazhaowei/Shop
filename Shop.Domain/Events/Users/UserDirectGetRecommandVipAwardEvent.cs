using ENode.Eventing;
using Shop.Domain.Models.Users;
using System;

namespace Shop.Domain.Events.Users
{
    /// <summary>
    /// 用户直推奖
    /// </summary>
    [Serializable]
    public class UserDirectGetRecommandVipAwardEvent : DomainEvent<Guid>
    {
        public Guid WalletId { get; private set; }
        public decimal Amount { get;private set; }

        public UserDirectGetRecommandVipAwardEvent() { }
        public UserDirectGetRecommandVipAwardEvent(Guid walletId,decimal amount)
        {
            WalletId = walletId;
            Amount = amount;
        }
    }
}
