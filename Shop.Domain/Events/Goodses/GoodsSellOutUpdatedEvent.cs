using ENode.Eventing;
using System;

namespace Shop.Domain.Events.Goodses
{
    [Serializable]
    public class GoodsSellOutUpdatedEvent:DomainEvent<Guid>
    {
        public int FinallySellOut { get; private set; }

        public GoodsSellOutUpdatedEvent() { }
        public GoodsSellOutUpdatedEvent(int finallySellOut)
        {
            FinallySellOut = finallySellOut;
        }
    }
}
