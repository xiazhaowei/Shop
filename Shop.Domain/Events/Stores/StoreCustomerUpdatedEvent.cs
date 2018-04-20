using ENode.Eventing;
using Shop.Domain.Models.Stores;
using System;

namespace Shop.Domain.Events.Stores
{
    [Serializable]
    public class StoreCustomerUpdatedEvent: DomainEvent<Guid>
    {
        public StoreCustomerEditableInfo Info { get; private set; }

        public StoreCustomerUpdatedEvent() { }
        public StoreCustomerUpdatedEvent(StoreCustomerEditableInfo info)
        {
            Info = info;
        }
    }
}
