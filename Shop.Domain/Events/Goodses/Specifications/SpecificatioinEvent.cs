using ENode.Eventing;
using Shop.Domain.Models.Goodses.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Events.Goodses.Specifications
{
    [Serializable]
    public abstract class SpecificationEvent : DomainEvent<Guid>
    {
        public Guid SpecificationId { get; private set; }
        public SpecificationInfo SpecificationInfo { get; private set; }

        public SpecificationEvent() { }
        public SpecificationEvent(Guid specificationId, SpecificationInfo specificationInfo)
        {
            SpecificationId = specificationId;
            SpecificationInfo = specificationInfo;
        }
    }
}
