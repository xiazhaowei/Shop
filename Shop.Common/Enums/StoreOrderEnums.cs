using System.ComponentModel;

namespace Shop.Common.Enums
{
    public enum StoreOrderStatus
    {
        [Description("全部")]
        All=-1,
        /// <summary>
        /// 已提交，待发货
        /// </summary>
        [Description("待发货")]
        Placed=0,
        /// <summary>
        /// 邮递中待收货
        /// </summary>
        [Description("待收货")]
        Expressing=1,
        /// <summary>
        /// 仅退款
        /// </summary>
        [Description("仅退款")]
        OnlyRefund=2,

        /// <summary>
        /// 退货退款
        /// </summary>
        [Description("退货退款")]
        ReturnAndRefund=3,

        [Description("同意退货")]
        AgreeReturn=4,

        [Description("退货发货")]
        ReturnExpressing=7,

        [Description("确认收货")]
        Success=5,
        /// <summary>
        /// 已关闭
        /// </summary>
        [Description("已关闭")]
        Closed=6

    }

    
}
