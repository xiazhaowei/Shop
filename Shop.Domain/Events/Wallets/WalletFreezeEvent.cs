using ENode.Eventing;
using System;

namespace Shop.Domain.Events.Wallets
{
    [Serializable]
    public class WalletFreezeEvent : DomainEvent<Guid>
    {
        public WalletFreezeEvent() { }
    }
}
