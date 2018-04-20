using Shop.Common.Enums;
using System;

namespace Shop.QueryServices.Dtos
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Thumb { get; set; }
        public string Url { get; set; }
        public CategoryType Type { get; set; }
        public bool IsShow { get; set; }
        public int Sort { get; set; }

    }
}
