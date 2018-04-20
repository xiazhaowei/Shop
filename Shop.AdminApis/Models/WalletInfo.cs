using Shop.Common.Enums;
using System;

namespace Shop.Api.Models
{
    public class WalletInfo
    {
        public Guid Id { get; set; }
        public string AccessCode { get; set; }
        public decimal Cash { get; set; }
        public decimal ShopCash { get; set; }
        public decimal Benevolence { get; set; }
        public decimal YesterdayEarnings { get; set; }
        public decimal Earnings { get; set; }
        public Freeze IsFreeze { get; set; }
    }
}