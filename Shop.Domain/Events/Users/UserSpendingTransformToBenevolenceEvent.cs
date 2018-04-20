using ENode.Eventing;
using System;

namespace Shop.Domain.Events.Users
{
    [Serializable]
    public class UserSpendingTransformToBenevolenceEvent:DomainEvent<Guid>
    {
        public Guid WalletId { get; private set; }
        public decimal Amount { get; private set; }


        public UserSpendingTransformToBenevolenceEvent() { }
        public UserSpendingTransformToBenevolenceEvent(Guid walletId,decimal amount)
        {
            WalletId = walletId;
            Amount = amount;
        }
    }
}
