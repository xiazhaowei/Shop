using Shop.Common.Enums;

namespace Shop.Api.Models.Request.Admins
{
    public class AddAdminRequest
    {
        public string Name { get; set; }
        public string LoginName { get; set; }
        public string Portrait { get; set; }
        public string Password { get; set; }
        public AdminRole Role { get; set; }
        public bool IsLocked { get; set; }
    }
}