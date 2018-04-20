using Shop.Common.Enums;

namespace Shop.Api.Models.Request.StoreOrders
{
    public class ListPageRequest
    {
        public string Number { get; set; }
        public string Mobile { get; set; }
        public string StoreName { get; set; }
        public int Page { get; set; }
        public StoreOrderStatus Status { get; set; }
    }
}