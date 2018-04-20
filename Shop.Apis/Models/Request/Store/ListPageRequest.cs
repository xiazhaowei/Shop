using Shop.Common.Enums;

namespace Shop.Api.Models.Request.Store
{
    public class ListPageRequest
    {
        public string Name { get; set; }
        public string Region { get; set; }
        public int Page { get; set; }
        public StoreStatus Status { get; set; }
        public StoreType Type { get; set; }
    }
}