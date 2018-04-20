using System;
using System.Configuration;

namespace Shop.Common
{
    /// <summary>
    /// 公共配置
    /// </summary>
    public class ConfigSettings
    {
        /// <summary>
        /// 订单预定自动过期时间 30分钟
        /// </summary>
        public static TimeSpan ReservationAutoExpiration = TimeSpan.FromMinutes(30);

        /// <summary>
        /// 包裹退款退货自动同意时间
        /// </summary>
        public static TimeSpan OrderAutoAgreeExpiration = TimeSpan.FromDays(3);

        /// <summary>
        /// 包裹自动确认收货时间
        /// </summary>
        public static TimeSpan OrderAutoConfirmDeliver = TimeSpan.FromDays(15);

        /// <summary>
        /// 包裹退货填写发货单期限
        /// </summary>
        public static TimeSpan OrderReturnAddExpressExiration = TimeSpan.FromDays(5);
        /// <summary>
        /// 订单商品服务自动过期时间 10天
        /// </summary>
        public static TimeSpan OrderGoodsServiceAutoExpiration = TimeSpan.FromDays(10);

        public static decimal IncentiveFeePersent = 0.1M;//每次善心激励收取10%手续费
        public static decimal BalanceFeePersent = 0M;//分红手续费

        public static decimal BenevolenceCalculationFactor = 35M;//善心计算因子

        /// <summary>
        /// ENode 数据库链接字符串
        /// </summary>
        public static string ENodeConnectionString { get; set; }
        /// <summary>
        ///  数据库链接字符串
        /// </summary>
        public static string ConnectionString { get; set; }

        /// <summary>
        /// 成为传递使者 最低推荐人数
        /// </summary>
        public static int ToPasserRecommendCount { get; set; }
        /// <summary>
        /// 成为传递使者 最低消费金额
        /// </summary>
        public static decimal ToPasserSpendingAmount { get; set; }
        /// <summary>
        /// 成为传递大使 缴纳年份金额
        /// </summary>
        public static decimal ToAmbassadorChargeAmount { get; set; }

        /// <summary>
        /// 一个福豆的价值
        /// </summary>
        public static decimal BenevolenceValue { get; set; }

        /// <summary>
        /// 16层领导人奖励福豆比例
        /// </summary>
        public static bool IsLeadershipAward { get; set; }
        public static decimal Leadership1_8 {get;set;}
        public static decimal Leadership8_16 { get; set; }

        /// <summary>
        /// 高利润商品奖励阈值
        /// </summary>
        public static decimal HighProfitAwardThreshold { get; set; }


        /// <summary>
        /// 用户推荐奖励
        /// </summary>
        public static bool IsRecommandAward { get; set; }
        public static decimal DirectRecommandAward { get; set; }
        public static decimal DirectRecommandVipPasserAward { get; set; }
        public static decimal VipPasserAward { get; set; }
        public static decimal VipPasserAward2 { get; set; }
        //感恩奖比例
        public static decimal GratefulAwardPersent { get; set; }

        public static decimal OneDayWithdrawLimit { get; set; }
        public static decimal OneWeekWithdrawLimit { get; set; }
        /// <summary>
        /// 拿推荐商家销售商品额的百分比1%
        /// </summary>
        public static decimal RecommandStoreGetPercent { get; set; }

        //表名称
        public static string AdminTable { get; set; }
        public static string AdminOperatRecordTable { get; set; }

        public static string UserTable { get; set; }
        public static string UserMobileIndexTable { get; set; }
        public static string ExpressAddressTable { get; set; }
        public static string UserGiftTable { get; set; }

        public static string CartGoodsesTable { get; set; }
        public static string CartTable { get; set; }

        public static string AnnouncementTable { get; set; }
        public static string CategoryTable { get; set; }
        public static string PubCategoryTable { get; set; }

        public static string GoodsBlockTable { get; set; }
        public static string GoodsBlockGoodsesTable { get; set; }

        public static string GoodsBlockWarpTable { get; set; }
        public static string GoodsBlockWarpGoodsBlocksTable { get; set; }

        public static string GoodsTable { get; set; }
        public static string GoodsPubCategorysTable { get;set; }
        public static string SpecificationTable { get; set; }
        public static string GoodsParamTable { get; set; }
        public static string GoodsCommentsTable { get; set; }
        public static string ReservationItemsTable { get; set; }

        public static string OrderTable { get; set; }
        public static string OrderLineTable { get; set; }
        public static string PaymentTable { get; set; }
        public static string PaymentItemTable { get; set; }

        public static string StoreTable { get; set; }
        public static string StoreOrderTable { get; set; }
        public static string OrderGoodsTable { get; set; }
        public static string ApplyServiceTable { get; set; }
        public static string ServiceExpressTable { get; set; }

        public static string OfflineStoreTable { get; set; }
        public static string OfflineStoreSaleLogTable { get; set; }

        public static string SectionTable { get; set; }
        public static string PartnerTable { get; set; }
        public static string PartnerBalanceLogTable { get; set; }

        public static string ThirdCurrencyTable { get; set; }
        public static string ThirdCurrencyImportLogTable { get; set; }

        public static string NotificationTable { get; set; }

        public static string WalletTable { get; set; }
        public static string CashTransferTable { get; set; }
        public static string ShopCashTransferTable { get; set; }

        public static string BenevolenceTransferTable { get; set; }
        public static string BankCardTable { get; set; }
        public static string WithdrawApplysTable { get; set; }
        public static string RechargeApplysTable { get; set; }

        public static string BenevolenceIndexIncentivesTable { get; set; }

        public static string GranteeTable { get; set; }
        public static string GranteeMoneyHelpsTable { get; set; }
        public static string GranteeTestifysTable { get; set; }

        public static int NameServerPort { get; set; }
        public static int BrokerProducerPort { get; set; }
        public static int BrokerConsumerPort { get; set; }
        public static int BrokerAdminPort { get; set; }

        public static int ApiPort { get; set; }
        public static int AdminApiPort { get; set; }
        public static int TimerTaskPort { get; set; }

        /// <summary>
        /// 初始化配置
        /// </summary>
        public static void Initialize()
        {
            if (ConfigurationManager.ConnectionStrings["enode"] != null)
            {
                ENodeConnectionString = ConfigurationManager.ConnectionStrings["enode"].ConnectionString;
            }
            if (ConfigurationManager.ConnectionStrings["shop"] != null)
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["shop"].ConnectionString;
            }

            ToPasserRecommendCount = 10;
            ToPasserSpendingAmount = 99;
            ToAmbassadorChargeAmount = 10000;

            BenevolenceValue = 100;

            OneDayWithdrawLimit = 200000M;
            OneWeekWithdrawLimit = 1000000M;

            RecommandStoreGetPercent = 0.005M;//0.5%

            IsLeadershipAward = false;
            Leadership1_8 = 0.01M;
            Leadership8_16 = 0.015M;

            HighProfitAwardThreshold = 10000M;

            IsRecommandAward = true;
            DirectRecommandAward = 100M;
            DirectRecommandVipPasserAward = 1000M;
            VipPasserAward = 50M;
            VipPasserAward2 = 500M;

            GratefulAwardPersent = 0.05M;

            AdminTable = "Admins";
            AdminOperatRecordTable = "AdminOperatRecords";

            UserTable = "Users";
            UserMobileIndexTable = "UserMobiles";
            ExpressAddressTable = "UserExpressAddresses";
            UserGiftTable = "UserGifts";

            CartTable = "Carts";
            CartGoodsesTable = "CartGoodses";

            CategoryTable = "Categorys";
            PubCategoryTable = "PubCategorys";
            AnnouncementTable = "Announcements";

            GoodsBlockTable = "GoodsBlocks";
            GoodsBlockGoodsesTable = "GoodsBlockGoodses";

            GoodsBlockWarpTable = "GoodsBlockWarps";
            GoodsBlockWarpGoodsBlocksTable = "GoodsBlockWarpGoodsBlocks";

            GoodsTable = "Goodses";
            GoodsPubCategorysTable = "GoodsPubCategorys";
            SpecificationTable = "Specifications";
            GoodsParamTable = "GoodsParams";
            GoodsCommentsTable = "GoodsComments";
            ReservationItemsTable = "ReservationItems";

            StoreTable = "Stores";
            StoreOrderTable = "StoreOrders";
            OrderGoodsTable = "OrderGoodses";
            ApplyServiceTable = "ApplyServices";
            ServiceExpressTable = "ServiceExpresses";

            OfflineStoreTable = "OfflineStores";
            OfflineStoreSaleLogTable = "OfflineStoreSaleLogs";

            SectionTable = "StoreSections";
            PartnerTable = "Partners";
            PartnerBalanceLogTable = "PartnerBalanceLogs";

            ThirdCurrencyTable = "ThirdCurrencys";
            ThirdCurrencyImportLogTable = "ThirdCurrencyImportLogs";

            OrderTable = "Orders";
            OrderLineTable = "OrderLines";

            NotificationTable = "Notifications";

            WalletTable = "Wallets";
            CashTransferTable = "CashTransfers";
            ShopCashTransferTable = "ShopCashTransfers";
            BenevolenceTransferTable = "BenevolenceTransfers";
            BankCardTable = "BankCards";
            WithdrawApplysTable = "WithdrawApplys";
            RechargeApplysTable = "RechargeApplys";

            BenevolenceIndexIncentivesTable = "BenevolenceIndexIncentives";

            GranteeTable = "Grantees";
            GranteeMoneyHelpsTable = "GranteeMoneyHelps";
            GranteeTestifysTable = "GranteeTestifys";

            PaymentTable = "Payments";
            PaymentItemTable = "PaymentItems";

            NameServerPort = 11100;
            BrokerProducerPort = 11101;
            BrokerConsumerPort = 11102;
            BrokerAdminPort = 11103;

            ApiPort = 9000;
            AdminApiPort = 9001;
            TimerTaskPort = 9002;
        }
    }
}
