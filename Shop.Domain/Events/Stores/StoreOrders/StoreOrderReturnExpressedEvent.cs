using ENode.Eventing;
using Shop.Domain.Models.Stores.StoreOrders;
using System;

namespace Shop.Domain.Events.Stores.StoreOrders
{
    [Serializable]
    public class StoreOrderReturnExpressedEvent:DomainEvent<Guid>
    {
        public ExpressInfo ExpressInfo { get; set; }

        public StoreOrderReturnExpressedEvent() { }
        public StoreOrderReturnExpressedEvent(ExpressInfo expressInfo)
        {
            ExpressInfo = expressInfo;
        }
    }
}
