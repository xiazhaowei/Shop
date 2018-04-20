using Shop.Common.Enums;
using System;
using System.Collections.Generic;

namespace Shop.Api.Models.Request.GoodsBlocks
{
    public class AddGoodsBlockRequest
    {
        public string Name { get; set; }
        public string Thumb { get; set; }
        public string Banner { get; set; }
        public List<Guid> Goodses { get; set; }
        public GoodsBlockGoodsLayout Layout { get; set; }
        public bool IsShow { get; set; }
        public int Sort { get; set; }
    }
}