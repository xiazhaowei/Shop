using ENode.Commanding;
using Shop.Common.Enums;
using System;
using System.Collections.Generic;

namespace Shop.Commands.GoodsBlocks
{
    public class UpdateGoodsBlockWarpCommand:Command<Guid>
    {
        public string Name { get; private set; }
        public GoodsBlockWarpStyle Style { get; private set; }
        public IList<Guid> GoodsBlocks { get; private set; }
        public bool IsShow { get; private set; }
        public int Sort { get; private set; }

        public UpdateGoodsBlockWarpCommand() { }
        public UpdateGoodsBlockWarpCommand(
            string name,
            GoodsBlockWarpStyle style,
            IList<Guid> goodsBlocks,
            bool isShow,
            int sort)
        {
            Name = name;
            Style = style;
            GoodsBlocks = goodsBlocks;
            IsShow = isShow;
            Sort = sort;
        }
    }
}
