using ENode.Eventing;
using Shop.Domain.Models.Goodses.Specifications;
using System;
using System.Collections.Generic;

namespace Shop.Domain.Events.Goodses.Specifications
{
    /// <summary>
    /// 确认预定事件
    /// </summary>
    [Serializable]
    public class SpecificationReservationCommittedEvent:DomainEvent<Guid>
    {
        public Guid ReservationId { get; set; }
        public IEnumerable<SpecificationStock> SpecificationStocks { get; set; }

        public SpecificationReservationCommittedEvent() { }
        public SpecificationReservationCommittedEvent(Guid reservationId, IEnumerable<SpecificationStock> specificationStocks)
        {
            ReservationId = reservationId;
            SpecificationStocks = specificationStocks;
        }
    }
}
