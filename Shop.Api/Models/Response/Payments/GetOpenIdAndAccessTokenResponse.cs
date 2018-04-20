namespace Shop.Api.Models.Response.Payments
{
    public class GetOpenIdAndAccessTokenResponse:BaseApiResponse
    {
        public string openid { get; set; }
        public string access_token { get; set; }
    }
}