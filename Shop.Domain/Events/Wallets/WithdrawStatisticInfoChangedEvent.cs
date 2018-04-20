using ENode.Eventing;
using Shop.Domain.Models.Wallets;
using System;

namespace Shop.Domain.Events.Wallets
{
    [Serializable]
    public class WithdrawStatisticInfoChangedEvent:DomainEvent<Guid>
    {
        public WithdrawStatisticInfo Info { get; set; }

        public WithdrawStatisticInfoChangedEvent() { }
        public WithdrawStatisticInfoChangedEvent(WithdrawStatisticInfo info)
        {
            Info = info;
        }
    }
}
