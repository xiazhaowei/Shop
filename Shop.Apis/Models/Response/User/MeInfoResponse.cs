namespace Shop.Api.Models.Response.User
{
    public class MeInfoResponse:BaseApiResponse
    {
        public UserInfo UserInfo { get; set; }
        public WalletInfo WalletInfo { get; set; }
    }
}