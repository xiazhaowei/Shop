using System;
using System.Collections.Generic;

namespace Shop.Api.Models.Response.Goodses
{
    public class GoodsInfoResponse:BaseApiResponse
    {
        public GoodsDetails GoodsDetails { get; set; }
        public Store.StoreInfo StoreInfo { get; set; }
        public IList<Specification> Specifications { get; set; }
        public IList<GoodsParam> GoodsParams { get; set; }
        public IList<Comment> Comments { get; set; }
    }

    public class Specification
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Thumb { get; set; }
        public decimal Price { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal Benevolence { get; set; }
        public int Stock { get; set; }
        public int AvailableQuantity { get; set; }
        public string Number { get; set; }
        public string BarCode { get; set; }
    }

    public class Comment
    {
        public Guid Id { get; set; }
        public Single Rate { get; set; }
        public string NickName { get; set; }
        public string Body { get; set; }
        public IList<string> Thumbs { get; set; }
        public string CreatedOn { get; set; }
    }

    public class GoodsParam
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }


}