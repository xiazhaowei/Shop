using ENode.Commanding;
using System;

namespace Shop.Commands.Stores.StoreOrders
{
    public class ApplyReturnAndRefundCommand : Command<Guid>
    {
        public string Reason { get;private set; }
        public decimal RefundAmount { get;private set; }

        public ApplyReturnAndRefundCommand() { }
        public ApplyReturnAndRefundCommand(string reason,decimal refundAmount)
        {
            Reason = reason;
            RefundAmount = refundAmount;
        }
    }
}
