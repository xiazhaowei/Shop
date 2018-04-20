using System.ComponentModel;

namespace Shop.Common.Enums
{
    public enum GoodsStatus
    {
        [Description("全部")]
        All=-1,
        [Description("待审核")]
        UnVerify=0,
        [Description("已审核")]
        Verifyed=1,

        [Description("驳回")]
        Refused = 2
    }
}
