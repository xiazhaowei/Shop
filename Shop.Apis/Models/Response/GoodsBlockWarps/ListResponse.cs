using Shop.Api.Models.Response.GoodsBlocks;
using System.Collections.Generic;

namespace Shop.Api.Models.Response.GoodsBlockWarps
{
    public class ListResponse:BaseApiResponse
    {
        public int Total { get; set; }
        public IList<GoodsBlockWarp> GoodsBlockWarps { get; set; }
    }


}