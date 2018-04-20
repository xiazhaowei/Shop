using ENode.Eventing;
using System;

namespace Shop.Domain.Events.Stores.StoreOrders
{
    [Serializable]
    public class AgreeReturnEvent:DomainEvent<Guid>
    {
        public string Remark { get; private set; }

        public AgreeReturnEvent() { }
        public AgreeReturnEvent(string remark) {
            Remark = remark;
        }
    }
}
