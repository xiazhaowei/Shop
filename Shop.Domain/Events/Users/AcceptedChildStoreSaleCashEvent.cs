using ENode.Eventing;
using System;

namespace Shop.Domain.Events.Users
{
    [Serializable]
    public class AcceptedChildStoreSaleCashEvent:DomainEvent<Guid>
    {
        public Guid WalletId { get; private set; }
        public decimal Amount { get; set; }

        public AcceptedChildStoreSaleCashEvent() { }
        public AcceptedChildStoreSaleCashEvent(Guid walletId,decimal amount)
        {
            WalletId = walletId;
            Amount = amount;
        }
    }
}
