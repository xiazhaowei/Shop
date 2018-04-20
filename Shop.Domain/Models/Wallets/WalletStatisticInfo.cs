using System;

namespace Shop.Domain.Models.Wallets
{
    /// <summary>
    /// 钱包统计信息
    /// </summary>
    public class WalletStatisticInfo
    {
        /// <summary>
        /// 昨日善心收益
        /// </summary>
        public decimal YesterdayEarnings { get; set; }
        /// <summary>
        /// 累计善心收益
        /// </summary>
        public decimal Earnings { get; set; }
        /// <summary>
        /// 昨日善心指数
        /// </summary>
        public decimal YesterdayIndex { get; set; }
        /// <summary>
        /// 累计获得善心数
        /// </summary>
        public decimal BenevolenceTotal { get; set; }
        /// <summary>
        /// 今日新增量
        /// </summary>
        public decimal TodayBenevolenceAdded { get; set; }

        public DateTime UpdatedOn { get;  set; }

        public WalletStatisticInfo(
            decimal yesterdayEarnings,
            decimal earnings,
            decimal yesterdayIndex,
            decimal benevolenceTotal,
            decimal todayBenevolenceAdded,
            DateTime updatedOn)
        {
            YesterdayEarnings = yesterdayEarnings;
            Earnings = earnings;
            YesterdayIndex = yesterdayIndex;
            BenevolenceTotal = benevolenceTotal;
            TodayBenevolenceAdded = todayBenevolenceAdded;

            UpdatedOn = updatedOn;
        }
    }
}
