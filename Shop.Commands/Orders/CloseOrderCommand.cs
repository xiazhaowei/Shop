using System;
using ENode.Commanding;

namespace Shop.Commands.Orders
{
    public class CloseOrderCommand : Command<Guid>
    {
        public Guid GoodsId { get; private set; }

        public CloseOrderCommand() { }
        public CloseOrderCommand(Guid orderId,Guid goodsId) : base(orderId)
        {
            GoodsId = goodsId;
        }
    }
}
