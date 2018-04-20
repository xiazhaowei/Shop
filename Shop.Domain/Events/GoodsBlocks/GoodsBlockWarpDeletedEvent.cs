using ENode.Eventing;
using System;

namespace Shop.Domain.Events.GoodsBlocks
{
    [Serializable]
    public class GoodsBlockWarpDeletedEvent:DomainEvent<Guid>
    {
        public GoodsBlockWarpDeletedEvent() { }
    }
}
