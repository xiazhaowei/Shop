using Shop.Common.Enums;
using System;

namespace Shop.Api.Models.Request.Users
{
    public class EditRequest
    {
        public Guid Id { get; set; }
        public string NickName { get; set; }
        public string Gender { get; set; }
        public UserRole Role { get; set; }
    }
}