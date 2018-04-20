using ENode.Commanding;
using Shop.Common.Enums;
using System;

namespace Shop.Commands.Admins
{
    public class UpdateAdminCommand:Command<Guid>
    {
        public string Name { get; private set; }
        public string LoginName { get; private set; }
        public string Portrait { get; private set; }
        public AdminRole Role { get; private set; }
        public bool IsLocked { get; private set; }

        public UpdateAdminCommand() { }
        public UpdateAdminCommand(
            string name,
            string loginName,
            string portrait,
            AdminRole role,
            bool isLocked)
        {
            Name = name;
            LoginName = loginName;
            Portrait = portrait;
            Role = role;
            IsLocked = isLocked;
        }
    }
}
