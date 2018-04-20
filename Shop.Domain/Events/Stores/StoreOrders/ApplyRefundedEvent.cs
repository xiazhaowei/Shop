using ENode.Eventing;
using Shop.Domain.Models.Stores.StoreOrders;
using System;

namespace Shop.Domain.Events.Stores.StoreOrders
{
    [Serializable]
    public class ApplyRefundedEvent:DomainEvent<Guid>
    {
        public RefundApplyInfo RefundApplyInfo { get; set; }

        public ApplyRefundedEvent() { }
        public ApplyRefundedEvent(RefundApplyInfo refundApplyInfo)
        {
            RefundApplyInfo = refundApplyInfo;
        }
    }
}
