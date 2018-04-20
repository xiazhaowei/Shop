using System;
using ENode.Commanding;

namespace Shop.Commands.Goodses.Specifications
{
    public class CommitSpecificationReservationCommand : Command<Guid>
    {
        public Guid ReservationId { get; private set; }

        public CommitSpecificationReservationCommand() { }
        public CommitSpecificationReservationCommand(Guid goodsId, Guid reservationId) : base(goodsId)
        {
            ReservationId = reservationId;
        }
    }
}
