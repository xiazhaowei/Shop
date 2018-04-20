using ENode.Commanding;
using System;

namespace Shop.Commands.Users
{
    /// <summary>
    /// 修改头像 命令
    /// </summary>
    public class UpdatePortraitCommand : Command<Guid>
    {
        public string Portrait { get;private set; }

        public UpdatePortraitCommand() { }
        public UpdatePortraitCommand(string portrait)
        {
            Portrait = portrait;
        }
    }
}
