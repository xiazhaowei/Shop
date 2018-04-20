using ENode.Eventing;
using Shop.Domain.Models.Goodses.Specifications;
using System;
using System.Collections.Generic;

namespace Shop.Domain.Events.Goodses.Specifications
{
    [Serializable]
    public class SpecificationsUpdatedEvent : DomainEvent<Guid>
    {
        public IList<Specification> Specifications { get; private set; }

        public SpecificationsUpdatedEvent() { }
        public SpecificationsUpdatedEvent(IList<Specification> specifications)
        {
            Specifications = specifications;
        }
    }
}
