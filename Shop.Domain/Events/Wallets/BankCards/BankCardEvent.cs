using ENode.Eventing;
using Shop.Domain.Models.Wallets.BankCards;
using System;

namespace Shop.Domain.Events.Wallets.BankCards
{
    [Serializable]
    public class BankCardEvent: DomainEvent<Guid>
    {
        public Guid BankCardId { get; private set; }
        public BankCardInfo Info { get; private set; }

        public BankCardEvent() { }
        public BankCardEvent(Guid bankCardId, BankCardInfo info)
        {
            BankCardId = bankCardId;
            Info = info;
        }
    }
}
