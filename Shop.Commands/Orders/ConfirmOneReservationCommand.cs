using ENode.Commanding;
using System;

namespace Shop.Commands.Orders
{
    /// <summary>
    /// 订单 确认某个商品预定成功与否 命令
    /// </summary>
    public class ConfirmOneReservationCommand : Command<Guid>
    {
        public bool IsReservationSuccess { get;private set; }
        public Guid GoodsId { get;private set; }


        public ConfirmOneReservationCommand() { }
        /// <summary>
        /// 确认预定成功与否的命令
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="isReservationSuccess"></param>
        public ConfirmOneReservationCommand(Guid orderId,Guid goodsId, bool isReservationSuccess) : base(orderId)
        {
            GoodsId = goodsId;
            IsReservationSuccess = isReservationSuccess;
        }
    }
}
