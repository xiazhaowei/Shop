using ENode.Commanding;
using Shop.Common.Enums;
using System;

namespace Shop.Commands.Goodses
{
    public class UpdateStatusCommand:Command<Guid>
    {
        public GoodsStatus Status { get; private set; }

        public UpdateStatusCommand() { }
        public UpdateStatusCommand(Guid id,GoodsStatus status):base(id)
        {
            Status = status;
        }
    }
}
