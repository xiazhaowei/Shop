using Shop.Common.Enums;
using System;

namespace Shop.Api.Models.Request.Admins
{
    public class EditAdminRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string LoginName { get; set; }
        public string Portrait { get; set; }
        public AdminRole Role { get; set; }
        public bool IsLocked { get; set; }
    }
}