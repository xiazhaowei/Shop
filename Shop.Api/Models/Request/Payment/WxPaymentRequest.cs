namespace Shop.Api.Models.Request.Payment
{
    public class WxPaymentRequest
    {
        public string OpenId { get; set; }
        public int Amount { get; set; }
        public string OrderNumber { get; set; }
    }
}