using System;
using ENode.Commanding;

namespace Shop.Commands.Goodses.Specifications
{
    public class CancelSpecificationReservationCommand : Command<Guid>
    {
        public Guid ReservationId { get; private set; }

        public CancelSpecificationReservationCommand() { }
        public CancelSpecificationReservationCommand(Guid goodsId, Guid reservationId) : base(goodsId)
        {
            ReservationId = reservationId;
        }
    }
}
