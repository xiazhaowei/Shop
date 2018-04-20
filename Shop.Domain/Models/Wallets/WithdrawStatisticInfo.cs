using System;

namespace Shop.Domain.Models.Wallets
{
    public class WithdrawStatisticInfo
    {
        public decimal TodayWithdrawAmount { get;  set; }
        public decimal WeekWithdrawAmount { get;  set; }
        public decimal WithdrawTotalAmount { get;  set; }
        public DateTime LastWithdrawTime { get;  set; }

        public WithdrawStatisticInfo(
            decimal todayWithdrawAmount,
            decimal weekWithdrawAmount,
            decimal withdrawTotalAmount,
            DateTime lastWithdrawTime)
        {
            TodayWithdrawAmount = todayWithdrawAmount;
            WeekWithdrawAmount = weekWithdrawAmount;
            WithdrawTotalAmount = withdrawTotalAmount;
            LastWithdrawTime = lastWithdrawTime;
        }
    }
}
