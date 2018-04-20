namespace Shop.Api.Models.Request.Payment
{
    public class PaymentRequest
    {
        public decimal Amount { get; set; }
        public string OrderNumber { get; set; }
    }
}