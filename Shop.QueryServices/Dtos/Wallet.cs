using Shop.Common.Enums;
using System;

namespace Shop.QueryServices.Dtos
{
    public class Wallet
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string AccessCode { get; set; }

        public string OwnerMobile { get; set; }
        public decimal Cash { get; set; }
        public decimal ShopCash { get; set; }
        public decimal Benevolence { get; set; }
        public decimal YesterdayEarnings { get; set; }
        public decimal Earnings { get; set; }
        public decimal YesterdayIndex { get; set; }
        public decimal BenevolenceTotal { get; set; }
        public decimal TodayBenevolenceAdded { get; set; }

        public decimal LockedCash { get; set; }
        public decimal TodayWithdrawAmount { get; set; }
        public decimal WeekWithdrawAmount { get; set; }

        public Freeze IsFreeze { get; set; }
    }
}
