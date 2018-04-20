using System;

namespace Shop.Domain.Models.Stores
{
    /// <summary>
    /// 店铺统计信息
    /// </summary>
    public class StoreStatisticInfo
    {
        public decimal TodaySale { get;  set; }
        public decimal TotalSale { get;  set; }
        public int TodayOrder { get; set; }
        public int TotalOrder { get;  set; }
        public int OnSaleGoodsCount { get;  set; }
        public DateTime UpdatedOn { get;  set; }

        public StoreStatisticInfo(decimal todaySale,decimal totalSale,int todayOrder,int totalOrder,int onSaleGoodsCount,DateTime updatedOn)
        {
            TodaySale = todaySale;
            TotalSale = totalSale;
            TodayOrder = todayOrder;
            TotalOrder = totalOrder;
            OnSaleGoodsCount = onSaleGoodsCount;

            UpdatedOn = updatedOn;
        }
    }
}
