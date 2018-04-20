using ENode.Commanding;
using Shop.Common.Enums;
using System;

namespace Shop.Commands.Users
{
    public class AcceptNewUpdateOrderCommand:Command<Guid>
    {
        public int GoodsCount { get; private set; }
        public UpdateOrderType UpdateOrderType { get; set; }

        public AcceptNewUpdateOrderCommand() { }
        public AcceptNewUpdateOrderCommand(int goodsCount,UpdateOrderType updateOrderType)
        {
            GoodsCount = goodsCount;
            UpdateOrderType = updateOrderType;
        }
    }
}
