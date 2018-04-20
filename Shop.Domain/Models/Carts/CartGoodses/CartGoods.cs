using System;

namespace Shop.Domain.Models.Carts.CartGoodses
{
    public class CartGoods
    {
        public Guid Id { get;private set; }
        public CartGoodsInfo Info { get; private set; }

        public CartGoods(Guid id,CartGoodsInfo info)
        {
            Id = id;
            Info = info;
        }
    }
}
