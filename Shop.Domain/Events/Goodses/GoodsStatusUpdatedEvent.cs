using ENode.Eventing;
using Shop.Common.Enums;
using System;

namespace Shop.Domain.Events.Goodses
{
    [Serializable]
    public class GoodsStatusUpdatedEvent:DomainEvent<Guid>
    {
        public GoodsStatus Status { get; private set; }

        public GoodsStatusUpdatedEvent() { }
        public GoodsStatusUpdatedEvent(GoodsStatus status)
        {
            Status = status;
        }
    }
}
