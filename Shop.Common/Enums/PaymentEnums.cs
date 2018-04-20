using System.ComponentModel;

namespace Shop.Common.Enums
{
    public enum PaymentState
    {
        /// <summary>
        /// 发起支付
        /// </summary>
        [Description("发起支付")]
        Initiated = 0,
        /// <summary>
        /// 支付完成
        /// </summary>
        [Description("支付完成")]
        Completed = 1,
        /// <summary>
        /// 支付拒绝
        /// </summary>
        [Description("拒绝")]
        Rejected = 2
    }
}
