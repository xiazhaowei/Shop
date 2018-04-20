using Shop.Common.Enums;

namespace Shop.Api.Models.Request.Users
{
    public class ListPageRequest
    {
        public UserRole Role { get; set; }
        public int Page { get; set; }
        public string Mobile { get; set; }
        public string NickName { get; set; }
    }
}