using System.ComponentModel;

namespace Shop.Common.Enums
{
    public enum NotificationType
    {
        [Description("全部")]
        All = -1,

        [Description("新订单")]
        NewStoreOrder = 0,
        [Description("订单发货")]
        StoreOrderDelivered = 1,
        [Description("用户确认收货")]
        StoreOrderConfirmDelivered = 2,
        [Description("商家同意退货")]
        StoreOrderAgreeReturn=3,
        [Description("订单退款")]
        StoreOrderRefunded=4,

        [Description("支付密码重置")]
        PayPasswordReseted=5,
        [Description("登录密码重置")]
        PasswordReseted=6,

        [Description("代理分红")]
        PartnerBalance=7,

        [Description("系统通知")]
        System=8
    }
}
