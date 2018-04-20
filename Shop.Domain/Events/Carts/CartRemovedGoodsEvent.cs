using ENode.Eventing;
using System;

namespace Shop.Domain.Events.Carts
{
    [Serializable]
    public class CartRemovedGoodsEvent:DomainEvent<Guid>
    {
        public Guid CartGoodsId { get; private set; }
        public int FinalCount { get; private set; }

        public CartRemovedGoodsEvent() { }
        public CartRemovedGoodsEvent(Guid cartGodosId,int finalCount)
        {
            CartGoodsId = cartGodosId;
            FinalCount = finalCount;
        }
    }
}
