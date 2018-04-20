using Dapper;
using ECommon.Components;
using ECommon.Dapper;
using Shop.Common;
using Shop.Common.Enums;
using Shop.QueryServices.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shop.QueryServices.Dapper
{
    [Component]
    public class OrderQueryService : BaseQueryService,IOrderQueryService
    {
        public Order FindOrder(Guid orderId)
        {
            using (var connection = GetConnection())
            {
                var order = connection.QueryList<Order>(new { OrderId = orderId }, ConfigSettings.OrderTable).FirstOrDefault();
                if (order != null)
                {
                    order.Lines=connection.QueryList<OrderLine>(new { OrderId = orderId }, ConfigSettings.OrderLineTable).ToList();
                    return order;
                }
                return null;
            }
        }

        /// <summary>
        /// 获取已过期的未付款订单
        /// </summary>
        /// <returns></returns>
        public IEnumerable<OrderAlis> ExpiredUnPayOrders()
        {
            var sql = string.Format(@"select OrderId,
            UserId,
            Status,
            Total,
            ShopCash,
            StoreTotal,
            ReservationExpirationDate
            from {0} where Status={1} and ReservationExpirationDate<'{2}'", ConfigSettings.OrderTable,(int)OrderStatus.ReservationSuccess,DateTime.Now);

            using (var connection = GetConnection())
            {
                return connection.Query<OrderAlis>(sql);
            }
        }

    }
}