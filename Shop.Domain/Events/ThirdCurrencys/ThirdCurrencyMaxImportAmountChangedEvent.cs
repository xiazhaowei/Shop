using ENode.Eventing;
using System;

namespace Shop.Domain.Events.ThirdCurrencys
{
    [Serializable]
    public class ThirdCurrencyMaxImportAmountChangedEvent:DomainEvent<Guid>
    {
        public decimal FinallyAmount { get; set; }

        public ThirdCurrencyMaxImportAmountChangedEvent() { }

        public ThirdCurrencyMaxImportAmountChangedEvent(decimal finallyAmount)
        {
            FinallyAmount = finallyAmount;
        }
    }
}
