using ENode.Eventing;
using System;

namespace Shop.Domain.Events.Categorys
{
    [Serializable]
    public class CategoryDeletedEvent:DomainEvent<Guid>
    {
        public CategoryDeletedEvent() { }
    }
}
