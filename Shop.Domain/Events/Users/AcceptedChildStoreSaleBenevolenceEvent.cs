using ENode.Eventing;
using System;

namespace Shop.Domain.Events.Users
{
    [Serializable]
    public class AcceptedChildStoreSaleBenevolenceEvent:DomainEvent<Guid>
    {
        public Guid WalletId { get; private set; }
        public decimal Amount { get; set; }

        public AcceptedChildStoreSaleBenevolenceEvent() { }
        public AcceptedChildStoreSaleBenevolenceEvent(Guid walletId,decimal amount)
        {
            WalletId = walletId;
            Amount = amount;
        }
    }
}
