using System;

namespace Shop.Api.Models.Response.Goodses
{
    public class AddGoodsResponse:BaseApiResponse
    {
        public Guid GoodsId { get; set; }
    }
}