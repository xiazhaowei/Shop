using Shop.Domain.Models.Goodses.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Events.Goodses.Specifications
{
    [Serializable]
    public class SpecificationAddedEvent : SpecificationEvent
    {
        public int Stock { get; private set; }

        public SpecificationAddedEvent() { }
        public SpecificationAddedEvent(Guid specificationId, SpecificationInfo specificationInfo, int stock)
            : base(specificationId, specificationInfo)
        {
            Stock = stock;
        }
    }
}
