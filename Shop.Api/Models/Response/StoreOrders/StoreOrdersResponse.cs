using System;
using System.Collections.Generic;

namespace Shop.Api.Models.Response.StoreOrders
{
    public class StoreOrdersResponse:BaseApiResponse
    {
        public int Total { get; set; }
        public decimal TotalSum { get; set; }
        public IList<StoreOrder> StoreOrders { get; set; }
    }
}