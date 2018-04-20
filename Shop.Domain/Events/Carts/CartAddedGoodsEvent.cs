using ENode.Eventing;
using Shop.Domain.Models.Carts.CartGoodses;
using System;

namespace Shop.Domain.Events.Carts
{
    [Serializable]
    public class CartAddedGoodsEvent:DomainEvent<Guid>
    {
        public Guid CartGoodsId { get; private set; }
        public CartGoodsInfo Info { get; private set; }
        public int FinalCount { get; private set; }

        public CartAddedGoodsEvent() { }
        public CartAddedGoodsEvent(Guid cartGoodsId,CartGoodsInfo info,int finalCount)
        {
            CartGoodsId = cartGoodsId;
            Info = info;
            FinalCount = finalCount;
        }
    }
}
