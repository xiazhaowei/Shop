using System;

namespace Shop.QueryServices.Dtos
{
    public class Specification
    {
        public Guid Id { get; set; }
        public Guid GoodsId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Thumb { get; set; }
        public decimal Price { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal Benevolence { get; set; }
        public string Number { get; set; }
        public string BarCode { get; set; }
        public int Stock { get; set; }
        public int AvailableQuantity { get; set; }
    }
}
