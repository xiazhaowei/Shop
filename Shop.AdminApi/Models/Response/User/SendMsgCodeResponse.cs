namespace Shop.Api.Models.Response.User
{
    public class SendMsgCodeResponse:BaseApiResponse
    {
        public string Token { get; set; }
        public string MsgCode { get; set; }
    }
}