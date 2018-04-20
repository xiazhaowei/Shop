using System.ComponentModel;

namespace Shop.Common.Enums
{
    public enum GoodsBlockGoodsLayout
    {
        [Description("单行")]
        SingleLine = 0,
        [Description("蜂窝")]
        Cols = 1
    }

    public enum GoodsBlockWarpStyle
    {
        [Description("单列缩略图")]
        SingleColThumb = 0,
        [Description("双列缩略图")]
        TwoColThumb = 1,
        [Description("四列缩略图同")]
        FourColThumb = 2,
        [Description("单列缩略图带商品")]
        SingleColThumWithGoods = 3,
        [Description("单行商品")]
        SingleLineGoods=4,
        [Description("蜂窝商品")]
        ColGoods=5,
    }
}
