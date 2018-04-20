using System;
using System.Collections.Generic;

namespace Shop.Api.Models.Request.Goodses
{
    public class UpdateGoodsSpecificationsRequest
    {
        public Guid GoodsId { get; set; }
        public IList<EditSpecificationInfo> Specifications { get; set; }
    }

    public class EditSpecificationInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Thumb { get; set; }
        public decimal Price { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal Benevolence { get; set; }
        public string Number { get; set; }
        public int Stock { get; set; }
        public string BarCode { get;  set; }
    }
}