using Shop.Common.Enums;

namespace Shop.Api.Models.Request.Goodses
{
    public class ListPageRequest
    {
        public string Name { get; set; }
        public GoodsStatus Status { get; set; }
        public string IsPublished { get; set; }
        public int Page { get; set; }
    }
}