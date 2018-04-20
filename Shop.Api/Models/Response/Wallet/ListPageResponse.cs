using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop.Api.Models.Response.Wallet
{
    public class ListPageResponse:BaseApiResponse
    {
        public int Total { get; set; }

        public IList<Wallet> Wallets { get; set; }
    }

    public class Wallet
    {
        public Guid Id { get; set; }
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
    }
}