using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop.Api.Models.Request.OfflineStores
{
    public class ListPageRequest
    {
        public string Name { get; set; }
        public string Region { get; set; }
        public int Page { get; set; }
    }
}