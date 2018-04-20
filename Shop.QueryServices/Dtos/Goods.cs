using System;

namespace Shop.QueryServices.Dtos
{
    public class Goods
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal OriginalPrice { get; set; }
        public DateTimeOffset CreatedOn { get; set; }

        public bool IsPublished { get; set; }

        
    }
}
