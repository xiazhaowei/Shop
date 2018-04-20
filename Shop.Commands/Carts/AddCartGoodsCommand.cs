using ENode.Commanding;
using System;

namespace Shop.Commands.Carts
{
    public class AddCartGoodsCommand:Command<Guid>
    {
        public Guid StoreId { get; private set; }
        public Guid GoodsId { get; private set; }
        public Guid SpecificationId { get; private set; }
        public string GoodsName { get; private set; }
        public string GoodsPic { get; private set; }
        public string SpecificationName { get; private set; }
        public decimal Price { get; private set; }
        public decimal OriginalPrice { get; private set; }
        public int Quantity { get; set; }
        public int Stock { get; private set; }
        public decimal Benevolence { get; private set; }

        public AddCartGoodsCommand() { }
        public AddCartGoodsCommand(
            Guid storeId,
            Guid goodsId,
            Guid specificationId,
            string goodsName,
            string goodsPic,
            string specificationName,
            decimal price,
            decimal originalPrice,
            int quantity,
            int stock,
            decimal benevolence)
        {
            StoreId = storeId;
            GoodsId = goodsId;
            SpecificationId = specificationId;
            GoodsName = goodsName;
            GoodsPic = goodsPic;
            SpecificationName = specificationName;
            Price = price;
            OriginalPrice = originalPrice;
            Quantity = quantity;
            Stock = stock;
            Benevolence = benevolence;
        }
    }
}
