using System.Collections.Generic;

namespace Shop.Api.Models.Response.GoodsBlocks
{
    public class ListResponse:BaseApiResponse
    {
        public int Total { get; set; }
        public List<GoodsBlock> GoodsBlocks { get; set; }
    }
}