using ENode.Eventing;
using Shop.Domain.Models.Goodses;
using Shop.Domain.Models.Goodses.Specifications;
using System;
using System.Collections.Generic;

namespace Shop.Domain.Events.Goodses.Specifications
{
    /// <summary>
    /// 商品预定
    /// </summary>
    [Serializable]
    public class SpecificationReservedEvent : DomainEvent<Guid>
    {
        public Guid ReservationId { get; private set; }
        public IEnumerable<ReservationItem> ReservationItems { get; private set; }
        public IEnumerable<SpecificationAvailableQuantity> SpecificationAvailableQuantities { get; private set; }

        public SpecificationReservedEvent() { }
        public SpecificationReservedEvent(Guid reservationId, IEnumerable<ReservationItem> reservationItems, IEnumerable<SpecificationAvailableQuantity> specificationAvailableQuantities)
        {
            ReservationId = reservationId;
            ReservationItems = reservationItems;
            SpecificationAvailableQuantities = specificationAvailableQuantities;
        }
    }
}
