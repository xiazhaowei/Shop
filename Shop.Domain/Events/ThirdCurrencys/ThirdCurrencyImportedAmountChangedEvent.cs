using ENode.Eventing;
using System;

namespace Shop.Domain.Events.ThirdCurrencys
{
    [Serializable]
    public class ThirdCurrencyImportedAmountChangedEvent : DomainEvent<Guid>
    {
        public decimal ImportedAmount { get; set; }

        public ThirdCurrencyImportedAmountChangedEvent() { }

        public ThirdCurrencyImportedAmountChangedEvent(decimal finallyAmount)
        {
            ImportedAmount = finallyAmount;
        }
    }
}
