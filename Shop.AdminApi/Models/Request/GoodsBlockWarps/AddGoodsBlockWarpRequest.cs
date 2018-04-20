using Shop.Common.Enums;
using System;
using System.Collections.Generic;

namespace Shop.Api.Models.Request.GoodsBlockWarps
{
    public class AddGoodsBlockWarpRequest
    {
        public string Name { get; set; }
        public GoodsBlockWarpStyle Style { get; set; }
        public List<Guid> GoodsBlocks { get; set; }
        public int Sort { get; set; }
        public bool IsShow { get; set; }
    }
}