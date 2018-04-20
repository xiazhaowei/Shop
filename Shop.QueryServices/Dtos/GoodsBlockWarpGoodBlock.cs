using System;

namespace Shop.QueryServices.Dtos
{
    public class GoodsBlockWarpGoodBlock
    {
        public Guid Id { get; set; }
        public Guid GoodsBlockWarpId { get; set; }
        public Guid GoodsBlockId { get; set; }
    }
}
