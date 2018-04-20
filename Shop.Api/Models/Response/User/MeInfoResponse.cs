using System;

namespace Shop.Api.Models.Response.User
{
    public class MeInfoResponse:BaseApiResponse
    {
        public UserInfo UserInfo { get; set; }
        public WalletInfo WalletInfo { get; set; }
        public StatisticsInfo StatisticsInfo { get; set; }
    }
    public class StatisticsInfo
    {
        public string StoreId { get; set; }
        public int TodayOrder { get; set; }
        public decimal TodaySale { get; set; }

        public string PartnerId { get; set; }
        public int RegionTodayOrder { get; set; }
        public decimal RegionTodaySale { get; set; }
    }

}