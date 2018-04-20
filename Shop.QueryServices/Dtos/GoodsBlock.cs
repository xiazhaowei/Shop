using Shop.Common.Enums;
using System;
using System.Collections.Generic;

namespace Shop.QueryServices.Dtos
{
    public class GoodsBlock
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Thumb { get; set; }
        public string Banner { get; set; }
        public GoodsBlockGoodsLayout Layout { get; set; }
        public bool IsShow { get; set; }
        public int Sort { get; set; }
        public IList<GoodsAlias> Goodses { get; set; }
    }
}
