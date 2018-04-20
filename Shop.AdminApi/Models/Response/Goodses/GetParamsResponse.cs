using System.Collections.Generic;

namespace Shop.Api.Models.Response.Goodses
{
    public class GetParamsResponse:BaseApiResponse
    {
        public IList<GoodsParam> Params { get; set; }
    }
}