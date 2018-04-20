using Shop.QueryServices.Dtos;
using System;
using System.Collections.Generic;

namespace Shop.QueryServices
{
    public interface IOrderQueryService
    {
        Order FindOrder(Guid orderId);

        IEnumerable<OrderAlis> ExpiredUnPayOrders();
    }
}