using ENode.Eventing;
using Shop.Common;
using System;

namespace Shop.Domain.Events.Payments
{
    [Serializable]
    public class PaymentCompletedEvent : DomainEvent<Guid>
    {
        public Guid OrderId { get; private set; }
        public PayInfo PayInfo { get; private set; }

        public PaymentCompletedEvent() { }
        public PaymentCompletedEvent(Guid orderId,PayInfo payInfo)
        {
            OrderId = orderId;
            PayInfo = payInfo;
        }
    }
}
