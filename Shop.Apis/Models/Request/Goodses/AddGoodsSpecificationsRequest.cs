using System;
using System.Collections.Generic;

namespace Shop.Api.Models.Request.Goodses
{
    public class AddGoodsSpecificationsRequest
    {
        public Guid GoodsId { get; set; }
        public IList<SpecificationInfo> Specifications { get; set; }
    }

    public class SpecificationInfo
    {
        public IList<string> Name { get; set; }
        public IList<string> Value { get; set; }
        public string Thumb { get; set; }
        public decimal Price { get; set; }
        public decimal OriginalPrice { get; set; }
        public string Number { get; set; }
        public int Stock { get; set; }
        public string BarCode { get;  set; }
    }
}