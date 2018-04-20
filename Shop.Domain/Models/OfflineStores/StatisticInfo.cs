using System;

namespace Shop.Domain.Models.OfflineStores
{
    public class StatisticInfo
    {
        public decimal TodaySale { get; set; }
        public decimal TotalSale { get; set; }
        public DateTime UpdatedOn { get; set; }

        public StatisticInfo(decimal todaySale,decimal totalSale,DateTime updatedOn)
        {
            TodaySale = todaySale;
            TotalSale = totalSale;
            UpdatedOn = updatedOn;
        }
    }
}
