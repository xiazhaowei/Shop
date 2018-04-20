using Shop.Common.Enums;
using System;
using System.Collections.Generic;

namespace Shop.QueryServices.Dtos
{
    public class GoodsBlockWarpAlis
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public GoodsBlockWarpStyle Style { get; set; }
        public bool IsShow { get; set; }
        public int Sort { get; set; }
    }
}
