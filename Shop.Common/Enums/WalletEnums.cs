using System.ComponentModel;

namespace Shop.Common.Enums
{
    public enum WithdrawApplyStatus
    {
        [Description("全部")]
        All = -1,
        [Description("提交")]
        Placed,
        [Description("成功")]
        Success,
        [Description("拒绝")]
        Rejected
    }

    public enum RechargeApplyStatus
    {
        [Description("全部")]
        All = -1,
        [Description("提交")]
        Placed,
        [Description("成功")]
        Success,
        [Description("拒绝")]
        Rejected
    }

    public enum WalletDirection
    {
        [Description("进账")]
        In = 0,//进账
        [Description("出账")]
        Out = 1//出账
    }
    public enum CashTransferType
    {
        [Description("全部")]
        All = -1,

        [Description("充值")]
        Charge = 0,
        [Description("提现")]
        Withdraw = 1,
        [Description("转账")]
        Transfer = 2,
        [Description("激励")]
        Incentive = 3,
        [Description("消费")]
        Shopping = 4,
        [Description("系统操作")]
        SystemOp=5,
        [Description("退款")]
        Refund=6,
        [Description("店铺售货")]
        StoreSell=7,
        [Description("推荐商家售货")]
        RecommendStoreAward =8,
        [Description("代理分红")]
        UnionAward = 9,
        [Description("推荐奖励")]
        RecommendUserAward=10
    }
    public enum ShopCashTransferType
    {
        [Description("全部")]
        All = -1,
        [Description("导入")]
        Charge = 0,
        [Description("消费")]
        Shopping = 1,
        [Description("系统操作")]
        SystemOp = 2,
        [Description("退款")]
        Refund = 3,
    }
    public enum CashTransferStatus
    {
        [Description("提交")]
        Placed = 0,
        [Description("成功")]
        Success = 1,
        [Description("失败")]
        Refused = 2
    }

    public enum ShopCashTransferStatus
    {
        [Description("提交")]
        Placed = 0,
        [Description("成功")]
        Success = 1,
        [Description("失败")]
        Refused = 2
    }


    public enum BenevolenceTransferType
    {
        [Description("全部")]
        All=-1,

        [Description("购物奖励")]
        ShoppingAward = 0,
        [Description("商家奖励")]
        StoreAward = 1,
        [Description("推荐奖励")]
        RecommendUserAward = 2,
        [Description("推荐商家奖励")]
        RecommendStoreAward = 3,
        [Description("代理分红")]
        UnionAward = 4,
        [Description("转账")]
        Transfer = 5,
        [Description("福豆激励")]
        Incentive = 6,
        [Description("系统操作")]
        SystemOp = 7
    }

    public enum BenevolenceTransferStatus
    {
        [Description("提交")]
        Placed = 0,
        [Description("成功")]
        Success = 1,
        [Description("失败")]
        Refused = 2
    }

    
}
