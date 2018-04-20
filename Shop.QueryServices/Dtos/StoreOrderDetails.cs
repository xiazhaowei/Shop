using System;
using System.Collections.Generic;

namespace Shop.QueryServices.Dtos
{
    public  class StoreOrderDetails:StoreOrder
    {
        public IList<StoreOrderGoods> StoreOrderGoodses { get; set; }
    }

    public class StoreOrderGoods
    {
        public Guid Id { get; set; }
        public Guid GoodsId { get; set; }
        public Guid SpecificationId { get; set; }
        public string GoodsName { get; set; }
        public string GoodsPic { get; set; }
        public string SpecificationName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal Benevolence { get; set; }
        public decimal Total { get; set; }
        public decimal ShopCash { get; set; }
        public decimal StoreTotal { get; set; }
    }
}
