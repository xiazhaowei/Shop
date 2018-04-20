using Shop.Common.Enums;
using System;
using System.Collections.Generic;

namespace Shop.Api.Models.Request.Goodses
{
    public class UpdateGoodsRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get;  set; }
        public IList<string> Pics { get;  set; }
        public decimal Price { get;  set; }
        public decimal Benevolence { get; set; }
        public int SellOut { get;  set; }
        public GoodsStatus Status { get; set; }
        public string RefusedReason { get; set; }

        public IList<EditSpecificationInfo> Specifications { get; set; }
    }
}