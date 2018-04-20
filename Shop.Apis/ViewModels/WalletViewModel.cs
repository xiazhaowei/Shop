using System;

namespace Shop.Apis.ViewModels
{
    public class WalletViewModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string AccessCode { get; set; }
        public decimal Cash { get; set; }
        public decimal Benevolence { get; set; }
        public decimal YesterdayEarnings { get; set; }
        public decimal Earnings { get; set; }
        public decimal YesterdayIndex { get; set; }
        public decimal BenevolenceTotal { get; set; }
        public decimal TodayBenevolenceAdded { get; set; }
    }
}