using Shop.Api.Models.Response.Goodses;
using System;
using System.Collections.Generic;

namespace Shop.Api.Models.Response.GoodsBlocks
{
    public class HomeGoodsBlockWarpsResponse:BaseApiResponse
    {
        public IList<GoodsBlockWarp> GoodsBlockWarps { get; set; }
    }

    public class GoodsBlockWarp
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsShow { get; set; }
        public int Sort { get; set; }
        public string Style { get; set; }
        public IList<GoodsBlock> GoodsBlocks { get; set; }
    }
    public class GoodsBlock
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Thumb { get; set; }
        public string Banner { get; set; }
        public string Layout { get; set; }
        public bool IsShow { get; set; }
        public int Sort { get; set; }
        public IList<Goods> Goodses { get; set; }
    }
}