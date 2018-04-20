using ENode.Eventing;
using System;

namespace Shop.Domain.Events.PubCategorys
{
    [Serializable]
    public class PubCategoryDeletedEvent:DomainEvent<Guid>
    {
        public PubCategoryDeletedEvent() { }
    }
}
