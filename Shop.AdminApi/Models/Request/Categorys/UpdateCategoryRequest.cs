using Shop.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop.Api.Models.Request.Categorys
{
    public class UpdateCategoryRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Thumb { get; set; }
        public CategoryType Type { get; set; }
        public bool IsShow { get; set; }
        public int Sort { get; set; }
    }
}