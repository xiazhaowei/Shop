using ENode.Eventing;
using Shop.Domain.Models.Stores;
using System;

namespace Shop.Domain.Events.Stores
{
    [Serializable]
    public class StoreUpdatedEvent: DomainEvent<Guid>
    {
        public StoreEditableInfo Info { get; private set; }

        public StoreUpdatedEvent() { }
        public StoreUpdatedEvent(StoreEditableInfo info)
        {
            Info = info;
        }
    }
}
