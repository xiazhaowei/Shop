using ENode.Commanding;
using System;

namespace Shop.Commands.Users
{
    /// <summary>
    /// 修改密码 命令
    /// </summary>
    public class UpdatePasswordCommand : Command<Guid>
    {
        public string Password { get; private set; }

        public UpdatePasswordCommand() { }
        public UpdatePasswordCommand(string password)
        {
            Password = password;
        }
    }
}
