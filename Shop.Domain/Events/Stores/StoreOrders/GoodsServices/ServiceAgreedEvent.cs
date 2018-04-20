using Shop.Common.Enums;
using Shop.Domain.Models.Stores;
using System;

namespace Shop.Domain.Events.Stores.StoreOrders.GoodsServices
{
    [Serializable]
    public class ServiceAgreedEvent: ServiceEvent
    {
        public OrderGoodsStatus Status { get; set; }

        public ServiceAgreedEvent() { }
        public ServiceAgreedEvent(string serviceNumber, OrderGoodsStatus status) : base(serviceNumber)
        {
            Status = status;
        }
    }
}
