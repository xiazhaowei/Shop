using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop.Api.Models.Request.Admins
{
    public class LoginRequest
    {
        public string Name { get; set; }
        public string Password { get; set; }
    }
}