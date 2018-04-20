using System.ComponentModel;

namespace Shop.Common.Enums
{
    /// <summary>
    /// 用户角色
    /// </summary>
    public enum UserRole
    {
        [Description("全部")]
        All=-1,

        [Description("普通会员")]
        Consumer = 0,

        [Description("VIP会员")]
        Passer = 1,

        [Description("五福经理")]
        VipPasser = 2,

        [Description("五福总监")]
        Director=3
    }

    
    public enum UserLock
    {
        /// <summary>
        /// 未锁定
        /// </summary>
        [Description("未锁定")]
        UnLocked=0,
        /// <summary>
        /// 锁定
        /// </summary>
        [Description("锁定")]
        Locked=1
    }

    public enum Freeze
    {
        [Description("未冻结")]
        UnFreeze=0,
        [Description("冻结")]
        Freeze=1
    }

    public enum UpdateOrderType
    {
        [Description("Vip升级单")]
        VipOrder=0,
        [Description("经理升级单")]
        VipPasserOrder=1,
    }
}
