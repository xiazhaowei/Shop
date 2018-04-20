using System;

namespace Shop.Commands.Goodses.Specifications
{
    [Serializable]
    public class SpecificationInfo
    {
        public Guid Id { get;private set; }
        public string Name { get; private set; }
        public string Value { get; private set; }
        public string Thumb { get; private set; }
        public decimal Price { get; private set; }
        public decimal OriginalPrice { get; private set; }
        public decimal Benevolence { get; private set; }
        public string Number { get; private set; }
        public string BarCode { get; private set; }
        public int Stock { get; private set; }

        public SpecificationInfo(
            Guid id,
            string name,
            string value,
            string thumb,
            decimal price,
            decimal originalPrice,
            decimal benevolence,
            string number,
            string barCode,
            int stock)
        {
            Id = id;
            Name = name;
            Value = value;
            Thumb = thumb;
            Price = price;
            OriginalPrice = originalPrice;
            Benevolence = benevolence;
            Number = number;
            BarCode = barCode;
            Stock = stock;
        }
    }
}
