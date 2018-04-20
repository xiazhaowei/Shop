using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop.Api.Models.Response.Categorys
{
    public class CategoryTreeResponse:BaseApiResponse
    {
        public IList<dynamic> Categorys { get; set; }
    }
}