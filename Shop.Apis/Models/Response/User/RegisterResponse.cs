namespace Shop.Api.Models.Response.User
{
    public class RegisterResponse:BaseApiResponse
    {
        public RegisterResult Result { get; set; }
    }

    public class RegisterResult
    {
        public string Id { get; set; }
    }
}