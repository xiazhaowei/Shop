using ENode.Eventing;
using Shop.Domain.Models.Stores.StoreOrders.GoodsServices;
using System;

namespace Shop.Domain.Events.Stores.StoreOrders.GoodsServices
{
    [Serializable]
    public class ServiceExpressedEvent:DomainEvent<Guid>
    {
        public ServiceExpressInfo Info { get; private set; }

        public ServiceExpressedEvent() { }
        public ServiceExpressedEvent(ServiceExpressInfo info)
        {
            Info = info;
        }
    }
}
