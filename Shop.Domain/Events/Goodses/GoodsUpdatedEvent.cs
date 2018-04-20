using ENode.Eventing;
using Shop.Domain.Models.Goodses;
using System;

namespace Shop.Domain.Events.Goodses
{
    [Serializable]
    public class GoodsUpdatedEvent:DomainEvent<Guid>
    {
        public GoodsEditableInfo Info { get; private set; }

        public GoodsUpdatedEvent() { }
        public GoodsUpdatedEvent(GoodsEditableInfo info)
        {
            Info = info;
        }
    }
}
