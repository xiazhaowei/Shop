using Shop.Common.Enums;
using Shop.Domain.Models.Orders;
using System;

namespace Shop.Domain.Events.Orders
{
    /// <summary>
    /// 订单预定确认事件
    /// </summary>
    [Serializable]
    public class OrderReservationConfirmedEvent:OrderEvent
    {
        public OrderStatus OrderStatus { get; private set; }

        public OrderReservationConfirmedEvent() { }
        public OrderReservationConfirmedEvent(OrderTotal orderTotal, OrderStatus orderStatus)
            : base(orderTotal)
        {
            OrderStatus = orderStatus;
        }
    }
}
