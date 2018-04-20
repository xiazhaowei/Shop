using System;

namespace Shop.Domain.Models.Orders
{
    /// <summary>
    /// 预定的商品规格
    /// </summary>
    [Serializable]
    public class Specification
    {
        public Guid SpecificationId { get; private set; }
        public Guid GoodsId { get; private set; }
        public Guid StoreId { get; private set; }
        public string GoodsName { get; private set; }
        public string GoodsPic { get;private set; }
        public string SpecificationName { get; private set; }
        public decimal Price { get; private set; }
        public decimal OriginalPrice { get; private set; }
        public decimal Benevolence { get; private set; }

        public Specification() { }
        public Specification(
            Guid specificationId,
            Guid goodsId, 
            Guid storeId,
            string goodsName,
            string goodsPic,
            string specificationName,
            decimal price,
            decimal originalPrice,
            decimal benevolence)
        {
            SpecificationId = specificationId;
            GoodsId = goodsId;
            StoreId = storeId;
            GoodsName = goodsName;
            GoodsPic = goodsPic;
            SpecificationName = specificationName;
            Price = price;
            OriginalPrice = originalPrice;
            Benevolence = benevolence;
        }
    }
}
