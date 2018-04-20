using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop.Api.Models.Request.Orders
{
    public class AddOrderRequest
    {
        public ExpressAddressInfo ExpressAddress { get; set; }
        public IList<CartGoods> CartGoodses { get; set; }
    }

    public class CartGoods
    {
        public Guid SpecificationId { get;  set; }
        public Guid GoodsId { get;  set; }
        public Guid StoreId { get;  set; }
        public string GoodsName { get;  set; }
        public string GoodsPic { get; set; }
        public string SpecificationName { get; set; }
        public decimal Price { get;  set; }
        public decimal OriginalPrice { get;  set; }
        public int Quantity { get;  set; }
        public decimal Benevolence { get;  set; }
    }
    public class ExpressAddressInfo
    {
        public string Region { get; set; }
        public string Address { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Zip { get; set; }
     }
}