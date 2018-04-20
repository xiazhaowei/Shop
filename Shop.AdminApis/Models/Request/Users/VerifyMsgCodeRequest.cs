namespace Shop.Api.Models.Request.Users
{
    public class VerifyMsgCodeRequest
    {
        public string Token { get; set; }
        public string MsgCode { get; set; }
    }
}