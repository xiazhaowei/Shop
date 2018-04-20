using ENode.Eventing;
using Shop.Domain.Models.ThirdCurrencys;
using System;

namespace Shop.Domain.Events.ThirdCurrencys
{
    [Serializable]
    public class ThirdCurrencyUpdatedEvent:DomainEvent<Guid>
    {
        public ThirdCurrencyInfo Info { get; set; }

        public ThirdCurrencyUpdatedEvent() { }
        public ThirdCurrencyUpdatedEvent(ThirdCurrencyInfo info)
        {
            Info = info;
        }
    }
}
