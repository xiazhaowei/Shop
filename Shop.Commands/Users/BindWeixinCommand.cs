using ENode.Commanding;
using System;

namespace Shop.Commands.Users
{
    public class BindWeixinCommand : Command<Guid>
    {
        public string WeixinId { get;private set; }
        public string UnionId { get;private set; }

        public BindWeixinCommand() { }
        public BindWeixinCommand(string weixinId,string unionId)
        {
            WeixinId = weixinId;
            UnionId = unionId;
        }
    }
}
