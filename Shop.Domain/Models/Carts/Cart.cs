using ENode.Domain;
using Shop.Domain.Events.Carts;
using Shop.Domain.Models.Carts.CartGoodses;
using System;
using System.Collections.Generic;
using System.Linq;
using Xia.Common.Extensions;

namespace Shop.Domain.Models.Carts
{
    /// <summary>
    /// 购物车聚合跟
    /// </summary>
    public class Cart:AggregateRoot<Guid>
    {
        private Guid _userId;
        private int _goodsCount;//商品数量
        private IList<CartGoods> _cartGoodses;

        public Cart(Guid id,Guid userId):base(id)
        {
            ApplyEvent(new CartCreatedEvent(userId));
        }

        /// <summary>
        /// 添加商品
        /// </summary>
        /// <param name="cartGoodsInfo"></param>
        public void AddGoods(CartGoodsInfo cartGoodsInfo)
        {
            cartGoodsInfo.CheckNotNull(nameof(cartGoodsInfo));
            var finalCount = _goodsCount;
            finalCount += cartGoodsInfo.Quantity;
            //判断是否存在该商品
            var cartGoods = _cartGoodses.SingleOrDefault(x => x.Info.SpecificationId == cartGoodsInfo.SpecificationId);
            if(cartGoods!=null)
            {//存在商品只更新数量即可
                var finalQuantity = cartGoods.Info.Quantity + cartGoodsInfo.Quantity;
                ApplyEvent(new CartGoodsQuantityChangedEvent(cartGoods.Id, finalQuantity, finalCount));
            }
            else
            {
                ApplyEvent(new CartAddedGoodsEvent(Guid.NewGuid(),cartGoodsInfo, finalCount));
            }
        }

        /// <summary>
        /// 删除购物车商品
        /// </summary>
        /// <param name="cartGoodsId"></param>
        public void RemoveGoods(Guid cartGoodsId)
        {
            var cartGoods = _cartGoodses.SingleOrDefault(x => x.Id == cartGoodsId);
            if (cartGoods == null)
            {
                throw new Exception("购物车不存在该商品.");
            }
            var finalCount = _goodsCount;
            finalCount -= cartGoods.Info.Quantity;
            ApplyEvent(new CartRemovedGoodsEvent(cartGoodsId, finalCount));
        }


        #region Handle


        private void Handle(CartCreatedEvent evnt)
        {
            _userId = evnt.UserId;
            _cartGoodses = new List<CartGoods>();
        }
        private void Handle(CartAddedGoodsEvent evnt)
        {
            _cartGoodses.Add(new CartGoods(evnt.CartGoodsId,evnt.Info));
            _goodsCount = evnt.FinalCount;
        }
        private void Handle(CartRemovedGoodsEvent evnt)
        {
            _cartGoodses.Remove(_cartGoodses.Single(x=>x.Id==evnt.CartGoodsId));
            _goodsCount = evnt.FinalCount;
        }

        private void Handle(CartGoodsQuantityChangedEvent evnt)
        {
            _cartGoodses.Single(x => x.Id == evnt.CartGoodsId).Info.Quantity = evnt.FinalQuantity;
            _goodsCount = evnt.FinalCount;
        }

        #endregion
    }
}
