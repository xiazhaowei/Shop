using System;

namespace Shop.Api.Models.Request.StoreOrders
{
    public class DeliverRequest
    {
        public Guid Id { get; set; }
        public string ExpressName { get; set; }
        public string ExpressCode { get; set; }
        public string ExpressNumber { get; set; }
    }
}