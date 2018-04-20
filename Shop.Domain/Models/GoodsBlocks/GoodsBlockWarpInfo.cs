using Shop.Common.Enums;
using System;
using System.Collections.Generic;

namespace Shop.Domain.Models.GoodsBlocks
{
    public class GoodsBlockWarpInfo
    {
        public string Name { get; private set; }
        public bool IsShow { get; private set; }
        public int Sort { get; private set; }
        public GoodsBlockWarpStyle Style { get; private set; }
        public IList<Guid> GoodsBlocks { get; private set; }

        public GoodsBlockWarpInfo(string name,GoodsBlockWarpStyle style,IList<Guid> goodsBlocks,bool isShow,int sort)
        {
            Name = name;
            Style = style;
            IsShow = isShow;
            Sort = sort;
            GoodsBlocks = goodsBlocks;
        }

    }
}
