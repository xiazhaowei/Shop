using ENode.Eventing;
using System;

namespace Shop.Domain.Events.Stores.StoreOrders.GoodsServices
{
    [Serializable]
    public class ServiceEvent: DomainEvent<Guid>
    {
        public string ServiceNumber { get; private set; }

        public ServiceEvent() { }
        public ServiceEvent(string serviceNumber)
        {
            ServiceNumber = serviceNumber;
        }
    }
}
