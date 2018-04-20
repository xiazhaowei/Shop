using ENode.Commanding;
using System;

namespace Shop.Commands.Users
{
    public class SetMyParentCommand:Command<Guid>
    {
        public Guid ParentId { get;private set; }

        public SetMyParentCommand() { }
        public SetMyParentCommand(Guid parentId)
        {
            ParentId = parentId;
        }
    }
}
