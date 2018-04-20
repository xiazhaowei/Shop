using ENode.Commanding;
using System;

namespace Shop.Commands.Carts
{
    public class RemoveCartGoodsCommand:Command<Guid>
    {
        public Guid CartGoodsId { get; private set; }

        public RemoveCartGoodsCommand() { }
        public RemoveCartGoodsCommand(Guid cartGoodsId)
        {
            CartGoodsId = cartGoodsId;
        }
    }
}
