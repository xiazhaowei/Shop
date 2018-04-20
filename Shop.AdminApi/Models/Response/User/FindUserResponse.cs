namespace Shop.Api.Models.Response.User
{
    public class FindUserResponse:BaseApiResponse
    {
        public FindUserResult Result { get; set; }
    }
    public class FindUserResult
    {
        public string Id { get; set; }
        public string NickName { get; set; }
        public string PortraitUri { get; set; }
    }
}