using System;

namespace Shop.Api.Models.Request.Users
{
    public class ResetPasswordRequest
    {
        public string Password { get; set; }
        public string Mobile { get; set; }
    }
}