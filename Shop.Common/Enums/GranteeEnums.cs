using System.ComponentModel;

namespace Shop.Common.Enums
{
    /// <summary>
    /// 筹款状态
    /// </summary>
    public enum GranteeStatus
    {
        [Description("生成")]
        Placed = 1, 
        [Description("已审核")]
        Verifyed=2,
        [Description("过期")]
        Expired = 3,    
        [Description("成功")]
        Success = 4,
        [Description("关闭")]
        Closed = 5
    }
}
