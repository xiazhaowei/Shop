using ENode.Commanding;
using System;

namespace Shop.Commands.Users
{
    /// <summary>
    /// 修改昵称 命令
    /// </summary>
    public class UpdateNickNameCommand : Command<Guid>
    {
        public string NickName { get;private set; }

        public UpdateNickNameCommand() { }
        public UpdateNickNameCommand(string nickName)
        {
            NickName = nickName;
        }
    }
}
