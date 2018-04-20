using System;
using System.Collections.Generic;

namespace Shop.Api.Models.Response.Goodses
{
    public class GoodsAlisesResponse:BaseApiResponse
    {
        public List<GoodsAlis> Goodses { get; set; }
    }

    public class GoodsAlis
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Pic { get; set; }
        public decimal Price { get; set; }
    }
}