using System;

namespace Shop.Api.Models.Response.Orders
{
    public class AddOrderResponse:BaseApiResponse
    {
        public Guid OrderId { get; set; }
        public Guid PaymentId { get; set; }
    }
}