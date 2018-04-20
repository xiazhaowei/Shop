using Shop.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop.Api.Models.Request.Goodses
{
    public class ListPageRequest
    {
        public string Name { get; set; }
        public string StoreName { get; set; }
        public GoodsStatus Status { get; set; }
        public int Page { get; set; }
    }
}