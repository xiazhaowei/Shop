using System;
using ENode.Commanding;
using Shop.Common;

namespace Shop.Commands.Orders
{
    public class ConfirmPaymentCommand : Command<Guid>
    {
        public PayInfo PayInfo { get; private set; }
        public bool IsPaymentSuccess { get; private set; }

        public ConfirmPaymentCommand() { }
        public ConfirmPaymentCommand(Guid orderId,PayInfo payInfo, bool isPaymentSuccess) : base(orderId)
        {
            PayInfo = payInfo;
            IsPaymentSuccess = isPaymentSuccess;
        }
    }
}
