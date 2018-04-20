using System.Collections.Generic;

namespace Shop.Api.Models.Response.Goodses
{
    public class HomePageGoodsesResponse:BaseApiResponse
    {
        public IList<Goods> NewGoodses { get; set; }
        public IList<Goods> RateGoodses { get; set; }
        public IList<Goods> SellOutGoodses { get; set; }
    }
}