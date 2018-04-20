using System;

namespace Shop.Commands.Orders
{
    public class SpecificationInfo
    {
        public Guid SpecificationId { get; private set; }
        public Guid GoodsId { get;private set; }
        public Guid StoreId { get; private set; }
        public string GoodsName { get; private set; }
        public string GoodsPic { get; private set; }
        public string SpecificationName { get; private set; }
        public decimal Price { get; private set; }
        public decimal OriginalPrice { get; private set; }
        public int Quantity { get; private set; }
        public decimal Benevolence { get; private set; }

        public SpecificationInfo(
            Guid specificationId,
            Guid goodsId,
            Guid storeId,
            string goodsName,
            string goodsPic,
            string specificationName,
            decimal price,
            decimal originalPrice,
            int quantity,
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
            Quantity = quantity;
            Benevolence = benevolence;
        }
    }
}
