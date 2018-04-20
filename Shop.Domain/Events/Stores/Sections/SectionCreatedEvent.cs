using ENode.Eventing;
using System;

namespace Shop.Domain.Events.Stores.Sections
{
    [Serializable]
    public class SectionCreatedEvent : DomainEvent<Guid>
    {
        public string Name { get; private set; }
        public string Description { get; private set; }

        private SectionCreatedEvent() { }
        public SectionCreatedEvent(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
