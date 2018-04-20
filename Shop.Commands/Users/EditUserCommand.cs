using ENode.Commanding;
using Shop.Common.Enums;
using System;

namespace Shop.Commands.Users
{
    public class EditUserCommand:Command<Guid>
    {
        public string NickName { get; set; }
        public string Gender { get; set; }
        public UserRole Role { get; set; }

        public EditUserCommand() { }
        public EditUserCommand(
            Guid userId,
            string nickName,
            string gender,
            UserRole role):base(userId)
        {
            NickName = nickName;
            Gender = gender;
            Role = role;
        }
    }
}
