using Shop.Common;
using Shop.Domain.Models.Orders;
using System;

namespace Shop.Domain.Events.Orders
{
    [Serializable]
    public class OrderSuccessedEvent:OrderEvent
    {
        public Guid UserId { get; set; }
        public ExpressAddressInfo ExpressAddressInfo { get; set; }
        public PayInfo PayInfo { get; set; }

        public OrderSuccessedEvent() { }
        public OrderSuccessedEvent(
            Guid userId,
            OrderTotal orderTotal,
            ExpressAddressInfo expressAddressInfo,
            PayInfo payInfo):base(orderTotal)
        {
            UserId = userId;
            ExpressAddressInfo = expressAddressInfo;
            PayInfo = payInfo;
        }
    }
}
