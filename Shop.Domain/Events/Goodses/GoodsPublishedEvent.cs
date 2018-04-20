using ENode.Eventing;
using System;

namespace Shop.Domain.Events.Goodses
{
    [Serializable]
    public class GoodsPublishedEvent:DomainEvent<Guid>
    {
        public GoodsPublishedEvent() { }
    }
}
