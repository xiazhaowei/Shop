using System;
using System.Collections.Generic;

namespace Shop.Api.Models.Response.Goodses
{
    public class GoodsListResponse:BaseApiResponse
    {
        public int Total { get; set; }
        public IList<Goods> Goodses { get; set; }
    }

    public class Goods
    {
        public Guid Id { get; set; }
        public IList<string> Pics { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal Benevolence { get; set; }
        public int SellOut { get; set; }
        public float Rate { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}