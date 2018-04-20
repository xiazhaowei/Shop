namespace Shop.Api.Models.Response.Payments
{
    public class WeChatPayResponse:BaseApiResponse
    {
        public string partnerid { get; set; }
        public string prepayid { get; set; }
        public string noncestr { get; set; }
        public string timestamp { get; set; }
        public string sign { get; set; }
    }
}