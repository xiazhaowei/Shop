using ENode.Eventing;
using System;

namespace Shop.Domain.Events.Carts
{
    [Serializable]
    public class CartCreatedEvent:DomainEvent<Guid>
    {
        public Guid UserId { get; private set; }

        public CartCreatedEvent() { }
        public CartCreatedEvent(Guid userId)
        {
            UserId = userId;
        }
    }
}
