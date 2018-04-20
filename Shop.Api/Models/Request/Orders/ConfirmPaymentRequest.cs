using System;

namespace Shop.Api.Models.Request.Orders
{
    public class ConfirmPaymentRequest
    {
        public Guid OrderId { get; set; }
        public bool IsPaymentSuccess { get; set; }
    }
}