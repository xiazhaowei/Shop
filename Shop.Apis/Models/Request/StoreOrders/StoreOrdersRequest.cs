using Shop.Common.Enums;
using System;

namespace Shop.Api.Models.Request.StoreOrders
{
    public class StoreOrdersRequest
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; }
        public StoreOrderStatus Status { get; set; }
        public int Page { get; set; }
    }
}