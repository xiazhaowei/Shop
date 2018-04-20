using Shop.Common.Enums;
using System;

namespace Shop.QueryServices.Dtos
{
    public class OrderGoods
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid GoodsId { get; set; }
        public Guid SpecificationId { get; set; }
        public Guid WalletId { get; set; }
        public Guid StoreOwnerWalletId { get; set; }
        public string GoodsName { get; set; }
        public string GoodsPic { get; set; }
        public string SpecificationName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal Total { get; set; }
        public decimal ShopCash { get; set; }
        public decimal StoreTotal { get; set; }
        public decimal Surrender { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ServiceExpirationDate { get; set; }
        public OrderGoodsStatus Status { get; set; }
    }
}
