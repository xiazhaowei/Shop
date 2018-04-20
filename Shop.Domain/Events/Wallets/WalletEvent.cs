using ENode.Eventing;
using System;

namespace Shop.Domain.Events.Wallets
{
    [Serializable]
    public abstract class WalletEvent : DomainEvent<Guid>
    {
        public Guid UserId { get; private set; }

        public WalletEvent() { }
        public WalletEvent(Guid userId)
        {
            UserId = userId;
        }
    }
}
