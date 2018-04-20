using Shop.Common.Enums;
using System;

namespace Shop.Domain.Models.Users
{
    public class RecommandUserInfo
    {
        public Guid UserId { get; set; }
        public UserRole Role { get; set; }
        public int Level { get; set; }

        public RecommandUserInfo(Guid userId,UserRole role,int level)
        {
            UserId = userId;
            Role = role;
            Level = level;
        }
    }
}
