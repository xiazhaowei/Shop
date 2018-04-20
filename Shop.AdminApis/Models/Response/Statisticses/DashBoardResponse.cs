using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop.Api.Models.Response.Statisticses
{
    public class DashBoardResponse:BaseApiResponse
    {
        public int UserCount { get; set; }
        public int NewRegCount { get; set; }
        public int AmbassadorCount { get; set; }
        public int NewAmbassadorCount { get; set; }

        public decimal CashTotal { get; set; }
        public decimal LockedCashTotal { get; set; }
        public decimal RechargeApplysTotal { get; set; }
        public decimal BenevolenceTotal { get; set; }
        public decimal TodayBenevolenceAddedTotal { get; set; }
        public decimal LastIncentiveAmount { get; set; }
        public decimal TotalIncentiveAmount { get; set; }

        public decimal TotalSale { get; set; }
        public decimal TodaySale { get; set; }
        public int StoreOrderCount { get; set; }
        public int TodayStoreOrderCount { get; set; }

        public decimal TodayOrderProfit { get; set; }
        public decimal TotalOrderProfit { get; set; }
    }
}