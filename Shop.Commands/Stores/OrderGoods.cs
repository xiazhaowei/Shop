using Shop.Common;
using System;

namespace Shop.Commands.Stores
{
    /// <summary>
    /// 订单商品 有两个地方使用了一个是创建订单商品时，一个是创建商家订单时
    /// </summary>
    [Serializable]
    public class OrderGoods
    {
        public Guid GoodsId { get;private set; }
        public Guid SpecificationId { get; private set; }
        //创建订单商品命令单独可以设置这两个属性
        public Guid WalletId { get;  set; }
        public Guid StoreOwnerWalletId { get;  set; }

        public string GoodsName { get; private set; }
        public string GoodsPic { get; private set; }
        public string SpecificationName { get; private set; }
        public int Quantity { get; private set; }
        public decimal Price { get; private set; }
        public decimal OrigianlPrice { get; private set; }
        public PayDetailInfo PayDetailInfo { get; private set; }
        public decimal Benevolence { get; private set; }

        public OrderGoods
            (Guid goodsId,
            Guid specificationId,
            string goodsName,
            string goodsPic,
            string specificationName,
            decimal price,
            decimal originalPrice,
            int quantity,
            PayDetailInfo payDetailInfo,
            decimal benevolence)
        {
            GoodsId = goodsId;
            SpecificationId = specificationId;
            GoodsName = goodsName;
            GoodsPic = goodsPic;
            SpecificationName = specificationName;
            Price = price;
            OrigianlPrice = originalPrice;
            Quantity = quantity;
            PayDetailInfo = payDetailInfo;
            Benevolence = benevolence;
        }
    }
}
