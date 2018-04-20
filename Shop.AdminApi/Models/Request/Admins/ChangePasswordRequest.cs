using System;

namespace Shop.Api.Models.Request.Admins
{
    public class ChangePasswordRequest
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}