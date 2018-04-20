using Shop.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop.Api.Models.Response.Store
{
    public class ListPageResponse:BaseApiResponse
    {
        public int Total { get; set; }
        public IList<Store> Stores { get; set; }
    }

    public class Store
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public string Mobile { get; set; }

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

        public string Type { get; set; }
        public string Status { get; set; }
        public bool IsLocked { get; set; }
    }
}