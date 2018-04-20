using ENode.Commanding;
using System;

namespace Shop.Commands.Users
{

    public class InvotedNewUserCommand:Command<Guid>
    {
        public Guid UserId { get; set; }

        public InvotedNewUserCommand() { }
        public InvotedNewUserCommand(Guid userId)
        {
            UserId = userId;
        }
    }
}
