using ENode.Eventing;
using System;

namespace Shop.Domain.Events.Users
{
    [Serializable]
    public class MyParentSetedEvent:DomainEvent<Guid>
    {
        public Guid ParentId { get;private set; }

        public MyParentSetedEvent() { }
        public MyParentSetedEvent(Guid parentId)
        {
            ParentId = parentId;
        }
    }
}
