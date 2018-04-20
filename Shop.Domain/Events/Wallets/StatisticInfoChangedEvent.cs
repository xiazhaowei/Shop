using ENode.Eventing;
using Shop.Domain.Models.Wallets;
using System;

namespace Shop.Domain.Events.Wallets
{
    [Serializable]
    public class StatisticInfoChangedEvent:DomainEvent<Guid>
    {
        public WalletStatisticInfo Info { get; set; }

        public StatisticInfoChangedEvent() { }
        public StatisticInfoChangedEvent(WalletStatisticInfo info)
        {
            Info = info;
        }
    }
}
