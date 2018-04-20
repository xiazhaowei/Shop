using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop.Api.Models.Response.User
{
    public class UserInfoResponse:BaseApiResponse
    {
        public UserInfo UserInfo { get; set; }
    }
}