using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop.Api.Models.Request.Goodses
{
    public class UpdateGoodsParamsRequest
    {
        public Guid GoodsId { get; set; }
        public IList<GoodsParamInfo> Params { get; set; }
    }

    public class GoodsParamInfo
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}