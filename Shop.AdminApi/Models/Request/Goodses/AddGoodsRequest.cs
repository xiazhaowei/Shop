using System;
using System.Collections.Generic;

namespace Shop.Api.Models.Request.Goodses
{
    public class AddGoodsRequest
    {
        public Guid Id { get; set; }
        public Guid StoreId { get; set; }
        public IList<Guid> CategoryIds { get; set; }
        public string Name { get; set; }
        public string Description { get;  set; }
        public IList<string> Pics { get;  set; }
        public decimal Price { get; set; }
        public decimal OriginalPrice { get;  set; }
        public int Stock { get;  set; }

        public bool IsPayOnDelivery { get;  set; }
        public bool IsInvoice { get;  set; }
        public bool Is7SalesReturn { get;  set; }
        public int Sort { get;  set; }
    }
}