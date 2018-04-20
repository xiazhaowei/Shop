namespace Shop.Api.Models.Request.Users
{
    public class LoginRequest
    {
        /// <summary>
        /// 国际电话区号
        /// </summary>
        public string Region { get; set; }
        public string Mobile { get; set; }
        public string Password { get; set; }
    }
}