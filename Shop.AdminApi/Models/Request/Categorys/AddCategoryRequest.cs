using Shop.Common.Enums;
using System;

namespace Shop.Api.Models.Request.Categorys
{
    public class AddCategoryRequest
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Thumb { get; set; }
        public CategoryType Type { get; set; }
        public int Sort { get; set; }
        public bool IsShow { get; set; }
        public Guid ParentId { get; set; }
    }
}