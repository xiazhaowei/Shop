using ENode.Eventing;
using System;

namespace Shop.Domain.Events.Users
{
    [Serializable]
    public class UserGetChildCashEvent:DomainEvent<Guid>
    {
        public Guid WalletId { get; private set; }
        public decimal Amount { get; private set; }
        public int Level { get; private set; }

        public UserGetChildCashEvent() { }
        public UserGetChildCashEvent(Guid walletId, decimal amount,int level)
        {
            WalletId = walletId;
            Amount = amount;
            Level = level;
        }
    }
}
