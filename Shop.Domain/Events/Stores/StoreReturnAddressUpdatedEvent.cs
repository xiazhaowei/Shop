using ENode.Eventing;
using Shop.Domain.Models.Stores;
using System;

namespace Shop.Domain.Events.Stores
{
    [Serializable]
    public class StoreReturnAddressUpdatedEvent:DomainEvent<Guid>
    {
        public ReturnAddressInfo Info { get;private set; }

        public StoreReturnAddressUpdatedEvent() { }
        public StoreReturnAddressUpdatedEvent(ReturnAddressInfo info)
        {
            Info = info;
        }
    }
}
