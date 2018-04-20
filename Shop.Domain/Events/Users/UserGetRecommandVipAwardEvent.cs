using ENode.Eventing;
using System;

namespace Shop.Domain.Events.Users
{
    /// <summary>
    /// 用户直推奖
    /// </summary>
    [Serializable]
    public class UserGetRecommandVipAwardEvent : DomainEvent<Guid>
    {
        public Guid WalletId { get; private set; }
        public decimal Amount { get;private set; }

        public UserGetRecommandVipAwardEvent() { }
        public UserGetRecommandVipAwardEvent(Guid walletId,decimal amount)
        {
            WalletId = walletId;
            Amount = amount ;
        }
    }
}
