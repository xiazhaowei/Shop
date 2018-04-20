using System;

namespace Shop.QueryServices.Dtos
{
    public class GoodsBlockGoods
    {
        public Guid Id { get; set; }
        public Guid GoodsBlockId { get; set; }
        public Guid GoodsId { get; set; }
    }
}
