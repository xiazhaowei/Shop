using ENode.Eventing;
using System;

namespace Shop.Domain.Events.GoodsBlocks
{
    [Serializable]
    public class GoodsBlockDeletedEvent:DomainEvent<Guid>
    {
        public GoodsBlockDeletedEvent() { }
    }
}
