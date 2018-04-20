using ENode.Eventing;
using System;

namespace Shop.Domain.Events.Wallets.BankCards
{
    [Serializable]
    public class BankCardRemovedEvent: DomainEvent<Guid>
    {
        public Guid BankCardId { get; private set; }

        public BankCardRemovedEvent() { }
        public BankCardRemovedEvent(Guid bankCardId)
        {
            BankCardId = bankCardId;
        }
    }
}
