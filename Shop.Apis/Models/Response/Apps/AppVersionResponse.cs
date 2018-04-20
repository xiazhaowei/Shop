namespace Shop.Api.Models.Response.Apps
{
    public class AppVersionResponse:BaseApiResponse
    {
        public string Version { get; set; }
        public string Content { get; set; }
    }
}