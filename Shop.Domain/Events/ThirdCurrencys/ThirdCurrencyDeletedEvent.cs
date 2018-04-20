using ENode.Eventing;
using System;

namespace Shop.Domain.Events.ThirdCurrencys
{
    [Serializable]
    public class ThirdCurrencyDeletedEvent:DomainEvent<Guid>
    {
        public ThirdCurrencyDeletedEvent() { }
    }
}
