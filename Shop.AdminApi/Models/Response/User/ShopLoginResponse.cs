using System;

namespace Shop.Api.Models.Response.User
{
    public class ShopLoginResponse:BaseApiResponse
    {
        public UserInfo UserInfo { get; set; }
        public StoreInfo StoreInfo { get; set; }
        public ReturnAddressInfo ReturnAddressInfo { get; set; }
    }

    public class StoreInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Region { get; set; }
        public string Address { get; set; }
       
        public int TodayOrder { get; set; }
        public decimal TodaySale { get; set; }
        public int TotalOrder { get; set; }
        public decimal TotalSale { get; set; }
    }
    public class ReturnAddressInfo
    {
        public Guid StoreId { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
    }
}