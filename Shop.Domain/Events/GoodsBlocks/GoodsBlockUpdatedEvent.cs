using ENode.Eventing;
using Shop.Domain.Models.GoodsBlocks;
using System;

namespace Shop.Domain.Events.GoodsBlocks
{
    [Serializable]
    public class GoodsBlockUpdatedEvent:DomainEvent<Guid>
    {
        public GoodsBlockInfo Info { get; private set; }

        public GoodsBlockUpdatedEvent() { }
        public GoodsBlockUpdatedEvent(GoodsBlockInfo info)
        {
            Info = info;
        }
    }
}
