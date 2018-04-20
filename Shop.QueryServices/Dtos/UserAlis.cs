using Shop.Common.Enums;
using System;

namespace Shop.QueryServices.Dtos
{

    public class UserAlis
    {
        public Guid Id { get; set; }
        public string NickName { get; set; }
        public string Mobile { get; set; }
        public string Portrait { get; set; }
        public DateTime CreatedOn { get; set; }
        public UserRole Role { get; set; }
    }
}
