using System;
using ENode.Commanding;

namespace Shop.Commands.Orders
{
    public class MarkAsSuccessCommand : Command<Guid>
    {
        public Guid GoodsId { get; private set; }
        public MarkAsSuccessCommand() { }
        public MarkAsSuccessCommand(Guid orderId,Guid goodsId) : base(orderId)
        {
            GoodsId = goodsId;
        }
    }
}
