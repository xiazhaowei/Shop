using ENode.Eventing;
using Shop.Domain.Models.ThirdCurrencys;
using System;

namespace Shop.Domain.Events.ThirdCurrencys
{
    [Serializable]
    public class ThirdCurrencyCreatedEvent:DomainEvent<Guid>
    {
        public ThirdCurrencyInfo Info { get; set; }

        public ThirdCurrencyCreatedEvent() { }
        public ThirdCurrencyCreatedEvent(ThirdCurrencyInfo info)
        {
            Info = info;
        }
    }
}
