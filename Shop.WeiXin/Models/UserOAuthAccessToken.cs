using System;

namespace Shop.WeiXin.Models
{
    public class UserOAuthAccessToken
    {
        public string OpenId { get; set; }
        public DateTime StartTime { get; set; }
        public string AccessToken { get; set; }
    }
}