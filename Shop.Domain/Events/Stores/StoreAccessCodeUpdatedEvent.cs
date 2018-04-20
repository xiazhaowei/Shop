using ENode.Eventing;
using System;

namespace Shop.Domain.Events.Stores
{
    [Serializable]
    public class StoreAccessCodeUpdatedEvent:DomainEvent<Guid>
    {
        public string AccessCode { get; private set; }

        public StoreAccessCodeUpdatedEvent() { }
        public StoreAccessCodeUpdatedEvent(string accessCode)
        {
            AccessCode = accessCode;
        }
    }
}
