using Shop.Common;
using Shop.Common.Enums;
using Shop.Domain.Models.Orders;
using System;

namespace Shop.Domain.Events.Orders
{
    /// <summary>
    /// 订单支付确认事件
    /// </summary>
    [Serializable]
    public class OrderPaymentConfirmedEvent : OrderEvent
    {
        public OrderStatus OrderStatus { get; private set; }
        public PayInfo PayInfo { get; private set; }

        public OrderPaymentConfirmedEvent() { }
        public OrderPaymentConfirmedEvent(OrderTotal orderTotal,PayInfo payInfo, OrderStatus orderStatus):base(orderTotal)
        {
            OrderStatus = orderStatus;
            PayInfo = payInfo;
        }
    }
}
