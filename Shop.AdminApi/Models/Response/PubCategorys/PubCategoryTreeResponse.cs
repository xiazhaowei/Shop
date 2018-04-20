using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop.Api.Models.Response.PubCategorys
{
    public class PubCategoryTreeResponse:BaseApiResponse
    {
        public IList<dynamic> Categorys { get; set; }
    }
}