using System;

namespace Shop.Domain.Events.Wallets
{
    [Serializable]
    public class WalletCreatedEvent:WalletEvent
    {
        public WalletCreatedEvent() { }
        public WalletCreatedEvent(Guid userId)
            : base(userId)
        {
        }
    }
}
