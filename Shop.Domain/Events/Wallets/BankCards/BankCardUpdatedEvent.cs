using Shop.Domain.Models.Wallets.BankCards;
using System;

namespace Shop.Domain.Events.Wallets.BankCards
{
    [Serializable]
    public  class BankCardUpdatedEvent : BankCardEvent
    { 
        public BankCardUpdatedEvent() { }
        public BankCardUpdatedEvent(Guid bankCardId,BankCardInfo info):base(bankCardId,info)
        {
        }
    }
}
