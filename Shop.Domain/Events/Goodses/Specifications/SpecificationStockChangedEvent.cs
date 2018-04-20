using ENode.Eventing;
using System;

namespace Shop.Domain.Events.Goodses.Specifications
{
    [Serializable]
    public class SpecificationStockChangedEvent:DomainEvent<Guid>
    {
        public Guid SpecificationId { get; private set; }
        public int Stock { get; private set; }
        public int AvailableQuantity { get; private set; }

        public SpecificationStockChangedEvent() { }
        public SpecificationStockChangedEvent(Guid specificationId, int stock, int availableQuantity)
        {
            SpecificationId = specificationId;
            Stock = stock;
            AvailableQuantity = availableQuantity;
        }
    }
}
