using System.ComponentModel;

namespace Shop.Common.Enums
{
    public enum AdminRole
    {
        [Description("全部")]
        All = -1,

        [Description("管理员")]
        Admin = 0,

        [Description("财务")]
        Accountant = 1,

        [Description("商品管理")]
        GoodsManager = 2,

        [Description("客服")]
        CustomerService = 3
    }
}
