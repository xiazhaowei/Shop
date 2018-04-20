using System;

namespace Shop.Api.Models.Request.PubCategorys
{
    public class AddPubCategoryRequest
    {
        public string Name { get; set; }
        public string Thumb { get; set; }
        public bool IsShow { get; set; }
        public int Sort { get; set; }
        public Guid ParentId { get; set; }
    }
}