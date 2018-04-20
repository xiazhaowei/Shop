using ENode.Eventing;
using Shop.Domain.Models.GoodsBlocks;
using System;

namespace Shop.Domain.Events.GoodsBlocks
{
    [Serializable]
    public class GoodsBlockWarpCreatedEvent:DomainEvent<Guid>
    {
        public GoodsBlockWarpInfo Info { get; private set; }

        public GoodsBlockWarpCreatedEvent() { }
        public GoodsBlockWarpCreatedEvent(GoodsBlockWarpInfo info)
        {
            Info = info;
        }
    }
}
