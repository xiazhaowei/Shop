using System;
using ENode.Commanding;
using Shop.Common;

namespace Shop.Commands.Payments
{
    public class CompletePaymentCommand : Command<Guid>
    {
        public PayInfo PayInfo { get; private set; }

        public CompletePaymentCommand() { }
        public CompletePaymentCommand(PayInfo payInfo)
        {
            PayInfo = payInfo;
        }
    }
}
