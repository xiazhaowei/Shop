using System;
using System.Collections.Generic;

namespace Shop.Api.Models.Response.Categorys
{
    public class ListPageResponse:BaseApiResponse
    {
        public int Total { get; set; }
        public IList<Category> Categorys { get; set; }
    }

    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Thumb { get; set; }
        public string Type { get; set; }
        public bool IsShow { get; set; }
        public int Sort { get; set; }
    }
}