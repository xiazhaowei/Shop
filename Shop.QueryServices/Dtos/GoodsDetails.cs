using Shop.Common.Enums;
using System;

namespace Shop.QueryServices.Dtos
{
    public class GoodsDetails
    {
        public Guid Id { get; set; }
        public Guid StoreId { get; set; }
        public string Pics { get; set; }
        public string StoreName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal OriginalPrice { get; set; }
        public int Stock { get; set; }
        public decimal Benevolence { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsPayOnDelivery { get;  set; }
        public bool IsInvoice { get;  set; }
        public bool Is7SalesReturn { get;  set; }
        public int Sort { get; set; }

        public Single Rate { get;  set; }
        public Single PriceRate { get;  set; }
        public Single DescribeRate { get;  set; }
        public Single QualityRate { get;  set; }
        public Single ExpressRate { get;  set; }
        public int RateCount { get;  set; }

        public int SellOut { get; set; }
        public bool IsPublished { get; set; }
        public GoodsStatus Status { get; set; }
        public string RefusedReason { get; set; }
    }
}
