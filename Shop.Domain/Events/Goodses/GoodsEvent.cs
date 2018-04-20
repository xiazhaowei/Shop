using ENode.Eventing;
using Shop.Domain.Models.Goodses;
using System;

namespace Shop.Domain.Events.Goodses
{
    [Serializable]
    public class GoodsEvent:DomainEvent<Guid>
    {
        public GoodsInfo Info { get; private set; }

        public GoodsEvent() { }
        public GoodsEvent(GoodsInfo info)
        {
            Info = info;
        }
    }
}
