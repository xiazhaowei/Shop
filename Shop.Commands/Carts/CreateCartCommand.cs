using ENode.Commanding;
using System;

namespace Shop.Commands.Carts
{
    public class CreateCartCommand:Command<Guid>
    {
        public Guid UserId { get;private set; }

        public CreateCartCommand() { }
        public CreateCartCommand(Guid id,Guid userId):base(id)
        {
            UserId = userId;
        }

    }
}
