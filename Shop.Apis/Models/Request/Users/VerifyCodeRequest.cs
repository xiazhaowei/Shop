namespace Shop.Api.Models.Request.Users
{
    /// <summary>
    /// verification_token 请求DTO
    /// </summary>
    public class VerifyCodeRequest
    {
        public string Region { get; set; }
        public string Phone { get; set; }
        public string Code { get; set; }
    }
}