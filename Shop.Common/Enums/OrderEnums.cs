using System.ComponentModel;

namespace Shop.Common.Enums
{
    public enum OrderStatus
    {
        [Description("提交")]
        Placed = 1,                //订单已生成
        [Description("预定成功")]
        ReservationSuccess = 2,    //预定已成功（下单已成功）
        [Description("预定失败")]
        ReservationFailed = 3,     //预定已失败（下单失败）
        [Description("付款成功")]
        PaymentSuccess = 4,        //付款已成功
        [Description("付款拒绝")]
        PaymentRejected = 5,       //付款已拒绝
        [Description("过期")]
        Expired = 6,               //订单已过期
        [Description("交易成功")]
        Success = 7,               //交易已成功
        [Description("关闭")]
        Closed = 8                 //订单已关闭
    }
}
