using System;
using System.Collections.Generic;
using ENode.Commanding;

namespace Shop.Commands.Goodses.Specifications
{
    /// <summary>
    /// 商品的预定
    /// </summary>
    public class MakeSpecificationReservationCommand : Command<Guid>
    {
        /// <summary>
        /// 订单ID
        /// </summary>
        public Guid ReservationId { get; private set; }
        /// <summary>
        /// 预定 规格信息（规格+数量）
        /// </summary>
        public IEnumerable<SpecificationReservationItemInfo> Specifications { get; private set; }

        public MakeSpecificationReservationCommand() { }
        /// <summary>
        /// 商品预定指令
        /// </summary>
        /// <param name="goodsId"></param>
        public MakeSpecificationReservationCommand(Guid goodsId,
            Guid reservationId,
            IEnumerable<SpecificationReservationItemInfo> specifications
            ) : base(goodsId)
        {
            ReservationId = reservationId;
            Specifications = specifications;
        }
    }
}
