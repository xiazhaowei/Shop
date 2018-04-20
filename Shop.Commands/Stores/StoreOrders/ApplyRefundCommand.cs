using ENode.Commanding;
using System;

namespace Shop.Commands.Stores.StoreOrders
{
    public class ApplyRefundCommand:Command<Guid>
    {
        public string Reason { get;private set; }
        public decimal RefundAmount { get;private set; }

        public ApplyRefundCommand() { }
        public ApplyRefundCommand(string reason,decimal refundAmount)
        {
            Reason = reason;
            RefundAmount = refundAmount;
        }
    }
}
