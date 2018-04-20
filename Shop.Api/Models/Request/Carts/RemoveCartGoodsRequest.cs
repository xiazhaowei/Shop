using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop.Api.Models.Request.Carts
{
    public class RemoveCartGoodsRequest
    {
        public Guid Id { get; set; }
        public Guid CartId { get; set; }
    }
}