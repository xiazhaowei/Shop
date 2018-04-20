using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop.Api.Models.Request.Carts
{
    public class AddCartGoodsRequest
    {
        public Guid GoodsId { get; set; }
        public Guid StoreId { get; set; }
        public Guid SpecificationId { get; set; }

        public string GoodsName { get;  set; }
        public string GoodsPic { get; set; }
        public string SpecificationName { get;  set; }
        public decimal Price { get;  set; }
        public decimal OriginalPrice { get;  set; }
        public int Quantity { get; set; }
        public int Stock { get;  set; }
        public decimal Benevolence { get; set; }
    }
}