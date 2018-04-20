using System;

namespace Shop.Api.Models.Request.StoreOrders
{
    public class ApplyRefundRequest
    {
        public Guid Id { get; set; }
        public string Reason { get; set; }
        public decimal RefundAmount { get; set; }
    }
}