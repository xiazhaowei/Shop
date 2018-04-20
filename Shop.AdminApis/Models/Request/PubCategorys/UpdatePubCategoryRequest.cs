using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop.Api.Models.Request.PubCategorys
{
    public class UpdatePubCategoryRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Thumb { get; set; }
        public bool IsShow { get; set; }
        public int Sort { get; set; }
    }
}