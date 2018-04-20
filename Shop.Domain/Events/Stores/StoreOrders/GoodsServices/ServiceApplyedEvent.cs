using ENode.Eventing;
using Shop.Domain.Models.Stores.StoreOrders.GoodsServices;
using System;

namespace Shop.Domain.Events.Stores.StoreOrders.GoodsServices
{
    [Serializable]
    public class ServiceApplyedEvent:DomainEvent<Guid>
    {
        public ServiceApplyInfo Info { get; private set; }

        public ServiceApplyedEvent() { }
        public ServiceApplyedEvent(ServiceApplyInfo info)
        {
            Info = info;
        }
    }
}
