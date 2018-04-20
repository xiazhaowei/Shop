using Shop.Domain.Models.Goodses.Specifications;
using System;

namespace Shop.Domain.Events.Goodses.Specifications
{
    [Serializable]
    public class SpecificationUpdatedEvent:SpecificationEvent
    {
        public SpecificationUpdatedEvent() { }
        public SpecificationUpdatedEvent(Guid specificationId,SpecificationInfo specificationInfo):
            base(specificationId, specificationInfo)
        {

        }
    }
}
