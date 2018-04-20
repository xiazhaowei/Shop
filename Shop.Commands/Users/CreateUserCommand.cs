using ENode.Commanding;
using System;

namespace Shop.Commands.Users
{
    /// <summary>
    /// 创建用户 命令
    /// </summary>
    public class CreateUserCommand : Command<Guid>
    {
        public Guid ParentId { get; private set; }
        public string NickName { get;private set; }
        public string Portrait { get; private set; }
        public string Gender { get; private set; }
        public string Mobile { get; private set; }
        public string Region { get; private set; }
        public string Password { get; private set; }
        public string WeixinId { get; private set; }
        public string UnionId { get; private set; }

        public CreateUserCommand() { }
        public CreateUserCommand(Guid id,
            Guid parentId,
            string nickName,
            string portrait,
            string gender,
            string mobile,
            string region,
            string password,
            string weixinId,
            string unionId):base(id)
        {
            ParentId = parentId;
            NickName = nickName;
            Portrait = portrait;
            Gender = gender;
            Mobile = mobile;
            Region = region;
            Password = password;
            WeixinId = weixinId;
            UnionId = unionId;
        }
    }
}
