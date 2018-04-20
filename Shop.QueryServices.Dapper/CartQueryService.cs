using Dapper;
using ECommon.Components;
using ECommon.Dapper;
using Shop.Common;
using Shop.QueryServices.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shop.QueryServices.Dapper
{
    /// <summary>
    /// 查询服务 实现
    /// </summary>
    [Component]
    public class CartQueryService : BaseQueryService, ICartQueryService
    {
        public Cart Info(Guid id)
        {
            using (var connection = GetConnection())
            {
                return connection.QueryList<Cart>(new { Id = id }, ConfigSettings.CartTable).SingleOrDefault();
            }
        }

        /// <summary>
        /// 获取购物车中的商品
        /// </summary>
        /// <param name="cartId"></param>
        /// <returns></returns>
        public IEnumerable<CartGoods> CartGoodses(Guid cartId)
        {
            var sql = string.Format(@"select b.Id,
            b.StoreId,
            b.GoodsId,
            b.SpecificationId,
            b.GoodsName,
            b.GoodsPic,
            b.SpecificationName,
            b.Price,
            b.OriginalPrice,
            b.Quantity,
            d.AvailableQuantity as Stock,
            b.Benevolence,
            a.Name as StoreName,
            c.IsPublished as IsGoodsPublished,
            c.Status as GoodsStatus 
            from {0} a inner join {1} b on a.Id=b.StoreId left join {2} c on b.GoodsId=c.Id left join {3} d on b.SpecificationId=d.Id 
            where b.CartId='{4}'", ConfigSettings.StoreTable,ConfigSettings.CartGoodsesTable,ConfigSettings.GoodsTable,ConfigSettings.SpecificationTable ,cartId);

            using (var connection = GetConnection())
            {
                return connection.Query<CartGoods>(sql);
            }
        }


    }
}