using ENode.Eventing;
using Shop.Domain.Models.GoodsBlocks;
using System;

namespace Shop.Domain.Events.GoodsBlocks
{
    [Serializable]
    public class GoodsBlockCreatedEvent:DomainEvent<Guid>
    {
        public GoodsBlockInfo Info { get; private set; }

        public GoodsBlockCreatedEvent() { }
        public GoodsBlockCreatedEvent(GoodsBlockInfo info)
        {
            Info = info;
        }
    }
}
