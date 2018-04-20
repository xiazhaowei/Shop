using ENode.Eventing;
using Shop.Domain.Models.GoodsBlocks;
using System;

namespace Shop.Domain.Events.GoodsBlocks
{
    [Serializable]
    public class GoodsBlockWarpUpdatedEvent:DomainEvent<Guid>
    {
        public GoodsBlockWarpInfo Info { get; private set; }

        public GoodsBlockWarpUpdatedEvent() { }
        public GoodsBlockWarpUpdatedEvent(GoodsBlockWarpInfo info)
        {
            Info = info;
        }
    }
}
