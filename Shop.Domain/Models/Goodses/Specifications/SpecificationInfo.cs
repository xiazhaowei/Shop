using System;

namespace Shop.Domain.Models.Goodses.Specifications
{
    /// <summary>
    /// 商品规格基本信息
    /// </summary>
    [Serializable]
    public class SpecificationInfo
    {
        /// <summary>
        /// 规格名称 颜色,尺码
        /// </summary>
        public string Name { get; private set; }
        public string Value { get; private set; }
        public string Thumb { get; private set; }
        public decimal Price { get; private set; }
        public decimal OriginalPrice { get; private set; }
        public decimal Benevolence { get; private set; }
        public string Number { get; private set; }
        public string BarCode { get; private set; }

        public SpecificationInfo(
            string name,
            string value, 
            string thumb,
            decimal price,
            decimal originalPrice,
            decimal benevolence,
            string number,
            string barCode)
        {
            Name = name;
            Value = value;
            Thumb = thumb;
            Price = price;
            OriginalPrice = originalPrice;
            Benevolence = benevolence;
            Number = number;
            BarCode = barCode;
        }
    }
}
