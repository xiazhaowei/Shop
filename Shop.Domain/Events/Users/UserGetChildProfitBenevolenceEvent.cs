using ENode.Eventing;
using System;

namespace Shop.Domain.Events.Users
{
    /// <summary>
    /// 获取子的善心分成
    /// </summary>
    [Serializable]
    public class UserGetChildProfitBenevolenceEvent:DomainEvent<Guid>
    {
        public Guid WalletId { get; private set; }
        public decimal Amount { get; private set; }
        public int Level { get; private set; }

        public UserGetChildProfitBenevolenceEvent() { }
        public UserGetChildProfitBenevolenceEvent(
            Guid walletId,
            decimal amount,
            int level)
        {
            WalletId = walletId;
            Amount = amount;
            Level = level;
        }
    }
}
