using ENode.Eventing;
using Shop.Domain.Models.Stores.StoreOrders.GoodsServices;
using System;

namespace Shop.Domain.Events.Stores.StoreOrders.GoodsServices
{
    [Serializable]
    public class ServiceFinishExpressedEvent : DomainEvent<Guid>
    {
        public ServiceFinishExpressInfo Info { get; private set; }

        public ServiceFinishExpressedEvent() { }
        public ServiceFinishExpressedEvent(ServiceFinishExpressInfo info)
        {
            Info = info;
        }
    }
}
