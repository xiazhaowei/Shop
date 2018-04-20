using System;

namespace Shop.Api.Models.Request.Users
{
    public class RegisterRequest
    {
        public Guid ParentId { get; set; }
        public string ParentMobile { get; set; }
        public string Mobile { get; set; }
        public string Password { get; set; }
        public string MsgCode { get; set; }
        public string Token { get; set; }
        public string NickName { get; set; }
        public string Gender { get; set; }
        public string Region { get; set; }
        public string Portrait { get; set; }
        public string WeixinId { get; set; }
        public string UnionId { get; set; }
    }
}