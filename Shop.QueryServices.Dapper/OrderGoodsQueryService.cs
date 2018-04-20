using Dapper;
using ECommon.Components;
using Shop.Common;
using Shop.Common.Enums;
using Shop.QueryServices.Dtos;
using System;
using System.Collections.Generic;

namespace Shop.QueryServices.Dapper
{
    [Component]
    public class OrderGoodsQueryService:BaseQueryService,IOrderGoodsQueryService
    {
        public IEnumerable<OrderGoodsAlis> ExpiredNormalGoodses()
        {
            var sql = string.Format(@"select Id,
            OrderId,
            GoodsId,
            SpecificationId,
            WalletId,
            StoreOwnerWalletId,
            Quantity,
            Price,
            OriginalPrice,
            Total,
            ShopCash,
            StoreTotal,
            Surrender,
            ServiceExpirationDate,
            Status 
            from {0} where Status={1} and ServiceExpirationDate<'{2}'", ConfigSettings.OrderGoodsTable, (int)OrderGoodsStatus.Normal, DateTime.Now);

            using (var connection = GetConnection())
            {
                return connection.Query<OrderGoodsAlis>(sql);
            }
        }
    }
}
