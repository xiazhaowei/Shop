using ENode.Eventing;
using Shop.Domain.Models.Goodses.Specifications;
using System;
using System.Collections.Generic;

namespace Shop.Domain.Events.Goodses.Specifications
{
    [Serializable]
    public class SpecificationReservationCancelledEvent:DomainEvent<Guid>
    {
        public Guid ReservationId { get; set; }
        public IEnumerable<SpecificationAvailableQuantity> SpecificationAvailableQuantities { get; set; }

        public SpecificationReservationCancelledEvent() { }
        public SpecificationReservationCancelledEvent(Guid reservationId, IEnumerable<SpecificationAvailableQuantity> specificationAvailableQuantities)
        {
            ReservationId = reservationId;
            SpecificationAvailableQuantities = specificationAvailableQuantities;
        }
    }
}
