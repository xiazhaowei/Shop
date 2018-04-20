using ENode.Eventing;
using System;

namespace Shop.Domain.Events.Goodses
{
    [Serializable]
    public class GoodsDeletedEvent:DomainEvent<Guid>
    {
        public GoodsDeletedEvent() { }
    }
}
