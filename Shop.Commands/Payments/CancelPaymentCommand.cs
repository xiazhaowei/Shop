using System;
using ENode.Commanding;

namespace Shop.Commands.Payments
{
    public class CancelPaymentCommand : Command<Guid>
    {
        public CancelPaymentCommand() { }
    }
}
