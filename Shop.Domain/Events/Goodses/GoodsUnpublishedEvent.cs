using ENode.Eventing;
using System;

namespace Shop.Domain.Events.Goodses
{
    [Serializable]
    public class GoodsUnpublishedEvent : DomainEvent<Guid>
    {
        public GoodsUnpublishedEvent() { }
    }
}
