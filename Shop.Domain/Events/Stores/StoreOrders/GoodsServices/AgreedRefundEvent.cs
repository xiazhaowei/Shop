using System;

namespace Shop.Domain.Events.Stores.StoreOrders.GoodsServices
{
    [Serializable]
    public class AgreedRefundEvent:ServiceEvent
    {
        public AgreedRefundEvent() { }
        public AgreedRefundEvent(string serviceNumber) : base(serviceNumber) { }
    }
}
