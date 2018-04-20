using Shop.Common.Enums;
using System;

namespace Shop.QueryServices.Dtos
{
    public class Admin
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string LoginName { get; set; }
        public string Portrait { get; set; }
        public string Password { get; set; }
        public AdminRole Role { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsLocked { get; set; }
    }
}
