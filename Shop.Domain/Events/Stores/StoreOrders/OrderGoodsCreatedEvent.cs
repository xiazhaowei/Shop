using ENode.Eventing;
using Shop.Domain.Models.Stores;
using System;

namespace Shop.Domain.Events.Stores.StoreOrders
{
    [Serializable]
    public class OrderGoodsCreatedEvent:DomainEvent<Guid>
    {
        public Guid OrderId { get; private set; }
        public OrderGoodsInfo Info { get; private set; }
        public DateTime ServiceExpirationDate { get; private set; }

        public OrderGoodsCreatedEvent() { }
        public OrderGoodsCreatedEvent(Guid orderId,OrderGoodsInfo info,DateTime serviceExpirationDate)
        {
            OrderId = orderId;
            Info = info;
            ServiceExpirationDate = serviceExpirationDate;
        }
    }
}
