using Shop.Api.Models.Response.Goodses;
using System;
using System.Collections.Generic;

namespace Shop.Api.Models.Response.Store
{
    public class HomeInfoResponse:BaseApiResponse
    {
        public StoreInfo StoreInfo { get; set; }
        public SubjectInfo SubjectInfo { get; set; }
        public IList<Goods> Goodses { get; set; }
    }
    
}