using System;

namespace Shop.Api.Models.Request.Goodses
{
    public class GoodsListRequest
    {
        public Guid CategoryId { get; set; }
        public string Search { get; set; }
        public string Type { get; set; }

        public string Sort { get; set; }
        public int Page { get; set; }
    }
}