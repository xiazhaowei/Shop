using Shop.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop.Api.Models.Response.User
{
    public class ListPageResponse:BaseApiResponse
    {
        public int Total { get; set; }
        public IList<User> Users { get; set; }
    }

    public class User
    {
        public Guid Id { get; set; }
        public Guid ParentId { get; set; }
        public string NickName { get; set; }
        public string Mobile { get; set; }
        public string Gender { get; set; }
        public string Region { get; set; }
        public string Role { get; set; }
        public UserLock IsLocked { get; set; }
        public Freeze IsFreeze { get; set; }
    }
}