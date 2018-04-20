namespace Shop.Api.Models.Response.User
{
    public class VerifyCodeResponse:BaseApiResponse
    {
       public VerificationTokenResult Result { get; set; }
    }

    public class VerificationTokenResult
    {
        public string VerificationToken { get; set; }
    }

}