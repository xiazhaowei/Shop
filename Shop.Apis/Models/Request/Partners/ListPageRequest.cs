using Shop.Common.Enums;

namespace Shop.Api.Models.Request.Partners
{
    public class ListPageRequest
    {
        public string Mobile { get; set; }
        public string Region { get; set; }
        public PartnerLevel Level { get; set; }
        public int Page { get; set; }
    }
}