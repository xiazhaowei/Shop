using Shop.Domain.Models.Wallets.BankCards;
using System;

namespace Shop.Domain.Events.Wallets.BankCards
{
    [Serializable]
    public class BankCardAddedEvent: BankCardEvent
    {
        public BankCardAddedEvent() { }
        public BankCardAddedEvent(Guid bankCardId, BankCardInfo info):base(bankCardId, info)
        {
            
        }
    }
}
