using System;
using ENode.Commanding;

namespace Shop.Commands.Orders
{
    public class MarkAsExpiredCommand : Command<Guid>
    {
        public MarkAsExpiredCommand() { }
        public MarkAsExpiredCommand(Guid orderId) : base(orderId)
        {
        }
    }
}
