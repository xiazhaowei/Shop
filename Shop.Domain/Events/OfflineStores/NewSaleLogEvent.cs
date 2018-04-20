using ENode.Eventing;
using Shop.Domain.Models.OfflineStores;
using System;

namespace Shop.Domain.Events.OfflineStores
{
    [Serializable]
    public class NewSaleLogEvent:DomainEvent<Guid>
    {
        public SaleLogInfo SaleLogInfo { get; private set; }

        public NewSaleLogEvent() { }
        public NewSaleLogEvent(SaleLogInfo saleLogInfo)
        {
            SaleLogInfo = saleLogInfo;
        }
    }
}
