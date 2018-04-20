using System;

namespace Shop.Api.Models.Response.Admins
{
    public class LoginResponse:BaseApiResponse
    {
        public User User { get; set; }
    }
    public class User
    {
        public Guid Id { get; set; }
        public string LoginName { get; set; }
        public string Name { get; set; }

        public string Password { get; set; }
        public string Portrait { get; set; }
        public string Role { get; set; }
    }
}