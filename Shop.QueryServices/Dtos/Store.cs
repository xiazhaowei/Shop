using Shop.Common.Enums;
using System;
using System.ComponentModel;

namespace Shop.QueryServices.Dtos
{
    public class Store
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public string Mobile { get; set; }

        public string AccessCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Region { get; set; }
        public string Address { get; set; }

        public decimal TodaySale { get; set; }
        public decimal TotalSale { get; set; }
        public int TodayOrder { get; set; }
        public int TotalOrder { get; set; }
        public int OnSaleGoodsCount { get; set; }

        public string SubjectName { get; set; }
        public string SubjectNumber { get; set; }
        public string SubjectPic { get; set; }

        public string ReturnAddress { get; set; }
        public string ReturnAddressName { get; set; }
        public string ReturnAddressMobile { get; set; }

        public DateTime CreatedOn { get; set; }


        public StoreStatus Status { get; set; }
        public StoreType Type { get; set; }
        public bool IsLocked { get; set; }

    }

    
}
