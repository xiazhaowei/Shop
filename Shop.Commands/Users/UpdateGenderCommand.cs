using ENode.Commanding;
using System;

namespace Shop.Commands.Users
{
    /// <summary>
    /// 修改性别 命令
    /// </summary>
    public class UpdateGenderCommand : Command<Guid>
    {
        public string Gender { get;private set; }

        public UpdateGenderCommand() { }
        public UpdateGenderCommand(string gender)
        {
            Gender = gender;
        }
    }
}
