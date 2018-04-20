using System;
using System.Collections.Generic;

namespace Shop.Api.Models.Response.Goodses
{
    public class GoodsesResponse : BaseApiResponse
    {
        public int Total { get; set; }
        public IList<GoodsDetails> Goodses { get; set; }
    }
    
}