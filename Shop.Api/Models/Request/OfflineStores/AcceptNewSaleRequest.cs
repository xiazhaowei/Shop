using System;

namespace Shop.Api.Models.Request.OfflineStores
{
    public class AcceptNewSaleRequest
    {
        public Guid OfflineStoreId { get; set; }
        public decimal Amount { get; set; }
    }
}