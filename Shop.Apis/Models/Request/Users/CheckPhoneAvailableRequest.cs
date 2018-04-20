namespace Shop.Api.Models.Request.Users
{
    /// <summary>
    /// 请求DTO
    /// </summary>
    public class CheckPhoneAvailableRequest
    {
        public string Region { get; set; }
        public string Phone { get; set; }
    }
}