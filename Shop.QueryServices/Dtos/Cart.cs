using System;

namespace Shop.QueryServices.Dtos
{
    public class Cart
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public int GoodsCount { get; set; }
    }
}
