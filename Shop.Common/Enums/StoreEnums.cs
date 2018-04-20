using System.ComponentModel;

namespace Shop.Common.Enums
{
    public enum StoreStatus
    {
        [Description("全部")]
        All = -1,
        [Description("申请")]
        Apply,
        [Description("正常")]
        Normal,
        [Description("主体错误")]
        SubjectErr
    }
    public enum StoreType
    {
        [Description("全部")]
        All = -1,
        [Description("第三方店铺")]
        ThirdParty,
        [Description("自营")]
        Self
    }
}
