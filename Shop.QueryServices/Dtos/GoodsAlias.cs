using System;

namespace Shop.QueryServices.Dtos
{
    public class GoodsAlias
    {
        public Guid Id { get; set; }
        public string Pics { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal Benevolence { get; set; }
        public int SellOut { get; set; }
        public float Rate { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
