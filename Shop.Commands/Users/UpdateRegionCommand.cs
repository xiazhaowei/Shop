using ENode.Commanding;
using System;

namespace Shop.Commands.Users
{
    /// <summary>
    /// 修改地区 命令
    /// </summary>
    public class UpdateRegionCommand : Command<Guid>
    {
        public string Region { get;private set; }

        public UpdateRegionCommand() { }
        public UpdateRegionCommand(string region)
        {
            Region = region;
        }
    }
}
