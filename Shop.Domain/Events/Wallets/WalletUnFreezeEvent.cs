using ENode.Eventing;
using System;

namespace Shop.Domain.Events.Wallets
{
    [Serializable]
    public class WalletUnFreezeEvent : DomainEvent<Guid>
    {
        public WalletUnFreezeEvent() { }
    }
}
