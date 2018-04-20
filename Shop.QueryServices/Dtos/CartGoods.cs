using Shop.Common.Enums;
using System;

namespace Shop.QueryServices.Dtos
{
    public class CartGoods
    {
        public Guid Id { get; set; }
        public Guid StoreId { get;  set; }
        public Guid GoodsId { get;  set; }
        public Guid SpecificationId { get;  set; }
        public string StoreName { get; set; }
        public string GoodsName { get;  set; }
        public string GoodsPic { get; set; }
        public string SpecificationName { get;  set; }
        public decimal Price { get;  set; }
        public decimal OriginalPrice { get;  set; }
        public int Quantity { get; set; }
        public int Stock { get;  set; }
        public decimal Benevolence { get; set; }
        public bool IsGoodsPublished { get; set; }
        public GoodsStatus GoodsStatus { get; set; }
    }
}
