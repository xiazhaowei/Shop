namespace Shop.Api.Models.Response.User
{
    public class MyTeamResponse:BaseApiResponse
    {
        public int TotalUserCount { get; set; }
        public int TotalPasserCount { get; set; }
        public int TotalVipPasserCount { get; set; }
        public int DirectPasserCount { get; set; }
        public int DirectVipPasserCount { get; set; }
    }
}