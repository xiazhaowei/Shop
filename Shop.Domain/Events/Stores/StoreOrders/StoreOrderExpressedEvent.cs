using ENode.Eventing;
using Shop.Domain.Models.Stores.StoreOrders;
using System;

namespace Shop.Domain.Events.Stores.StoreOrders
{
    [Serializable]
    public class StoreOrderExpressedEvent:DomainEvent<Guid>
    {
        public ExpressInfo ExpressInfo { get; set; }

        public StoreOrderExpressedEvent() { }
        public StoreOrderExpressedEvent(ExpressInfo expressInfo)
        {
            ExpressInfo = expressInfo;
        }
    }
}
