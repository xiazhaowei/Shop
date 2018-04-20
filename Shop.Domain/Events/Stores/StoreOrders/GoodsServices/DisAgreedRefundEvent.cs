using System;

namespace Shop.Domain.Events.Stores.StoreOrders.GoodsServices
{
    [Serializable]
    public class DisAgreedRefundEvent:ServiceEvent
    {
        public DisAgreedRefundEvent() { }
        public DisAgreedRefundEvent(string serviceNumber) : base(serviceNumber) { }
    }
}
