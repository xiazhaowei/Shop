using System.ComponentModel;

namespace Shop.Common.Enums
{
    /// <summary>
    /// 订单商品服务状态
    /// </summary>
    public enum OrderGoodsStatus
    {
        /// <summary>
        /// 正常 在服务期
        /// </summary>
        [Description("正常")]
        Normal = 0,
        /// <summary>
        /// 退货
        /// </summary>
        [Description("退货")]
        SalesReturn = 1,
        /// <summary>
        /// 退款
        /// </summary>
        [Description("退款")]
        Refund = 2,
        /// <summary>
        /// 维修
        /// </summary>
        [Description("维修")]
        Service = 3,
        /// <summary>
        /// 上门服务
        /// </summary>
        [Description("上门服务")]
        ToDoorService = 4,
        /// <summary>
        /// 换货
        /// </summary>
        [Description("换货")]
        Change = 5,

        /// <summary>
        /// 待用户邮递 已同意服务
        /// </summary>
        [Description("已同意服务")]
        TobeSent = 10,
        /// <summary>
        /// 退款成功
        /// </summary>
        [Description("退款成功")]
        RefundSuccess = 11,
        /// <summary>
        /// 服务期已过 不在支持退换  可以结算奖金了
        /// </summary>
        [Description("服务期已过")]
        Expire = 12,
        /// <summary>
        /// 售后服务结束 也可以结算奖金
        /// </summary>
        [Description("服务结束")]
        Closed = 13
    }

    /// <summary>
    /// 商品服务类型
    /// </summary>
    public enum GoodsServiceType
    {
        /// <summary>
        /// 退货
        /// </summary>
        SalesReturn = 1,
        /// <summary>
        /// 退款
        /// </summary>
        Refund = 2,
        /// <summary>
        /// 维修
        /// </summary>
        Service = 3,
        /// <summary>
        /// 上门服务
        /// </summary>
        ToDoorService = 4,
        /// <summary>
        /// 换货
        /// </summary>
        Change = 5
    }
}
