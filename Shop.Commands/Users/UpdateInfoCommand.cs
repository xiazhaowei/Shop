using ENode.Commanding;
using System;

namespace Shop.Commands.Users
{
    public class UpdateInfoCommand:Command<Guid>
    {
        public string NickName { get; set; }
        public string Region { get; set; }
        public string Portrait { get; set; }

        public UpdateInfoCommand() { }
        public UpdateInfoCommand(string nickName,string region,string portrait)
        {
            NickName = nickName;
            Region = region;
            Portrait = portrait;
        }
    }
}
