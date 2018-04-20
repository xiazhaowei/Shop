using ENode.Commanding;
using Shop.Common.Enums;
using System;
using System.Collections.Generic;

namespace Shop.Commands.GoodsBlocks
{
    public class UpdateGoodsBlockCommand:Command<Guid>
    {
        public string Name { get; private set; }
        public string Thumb { get; private set; }
        public string Banner { get; private set; }
        public GoodsBlockGoodsLayout Layout { get; private set; }
        public IList<Guid> Goodses { get; private set; }
        public bool IsShow { get; private set; }
        public int Sort { get; private set; }

        public UpdateGoodsBlockCommand() { }
        public UpdateGoodsBlockCommand(
            string name,
            string thumb,
            string banner,
            GoodsBlockGoodsLayout layout,
            IList<Guid> goodses,
            bool isShow,
            int sort)
        {
            Name = name;
            Thumb = thumb;
            Banner = banner;
            Layout = layout;
            Goodses = goodses;
            IsShow = isShow;
            Sort = sort;
        }
    }
}
