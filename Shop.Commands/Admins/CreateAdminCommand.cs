using ENode.Commanding;
using Shop.Common.Enums;
using System;

namespace Shop.Commands.Admins
{
    public class CreateAdminCommand:Command<Guid>
    {
        public string Name { get; private set; }
        public string LoginName { get; private set; }
        public string Portrait { get; private set; }
        public string Password { get; private set; }
        public AdminRole Role { get; private set; }
        public bool IsLocked { get; private set; }

        public CreateAdminCommand() { }
        public CreateAdminCommand(Guid id,
            string name,
            string loginName,
            string portrait,
            string password,
            AdminRole role,
            bool isLocked):base(id)
        {
            Name = name;
            LoginName = loginName;
            Portrait = portrait;
            Role = role;
            Password = password;
            IsLocked = isLocked;
        }
    }
}
