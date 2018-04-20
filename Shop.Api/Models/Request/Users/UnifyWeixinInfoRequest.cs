using System;

namespace Shop.Api.Models.Request.Users
{
    public class UnifyWeixinInfoRequest
    {
        public Guid Id { get; set; }
        public string NickName { get; set; }
        public string Region { get; set; }
        public string Portrait { get; set; }
    }
}